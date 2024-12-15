import React, { useState, useCallback, useEffect, useRef } from "react";
import {
  GoogleMap,
  useJsApiLoader,
  StandaloneSearchBox,
} from "@react-google-maps/api";
import { GOOGLE_MAPS_API_KEY, MAP_CONFIG, GOOGLE_MAPS_LIBRARIES } from "../../../constants";
import { markersService } from "../../../services/markersService";
import { FetchParams } from "../../../services/markersService";

interface MapSectionProps {
  onLocationSelect?: (location: { x: number; y: number }) => void;
  selectionMode?: boolean;
  filters?: FetchParams;
}

interface AdvancedMarkerProps {
  position: google.maps.LatLng;
  map: google.maps.Map;
}

const AdvancedMarker: React.FC<AdvancedMarkerProps> = ({ position, map }) => {
  const markerRef = useRef<google.maps.marker.AdvancedMarkerElement | null>(null);

  useEffect(() => {
    let isMounted = true;

    const initMarker = async () => {
      try {
        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker") as google.maps.MarkerLibrary;
        if (isMounted && !markerRef.current) {
          markerRef.current = new AdvancedMarkerElement({
            position,
            map
          });
        }
      } catch (error) {
        console.error('Error initializing marker:', error);
      }
    };

    initMarker();

    return () => {
      isMounted = false;
      if (markerRef.current) {
        markerRef.current.map = null;
        markerRef.current = null;
      }
    };
  }, [position, map]);

  return null;
};

const MapSection: React.FC<MapSectionProps> = ({ onLocationSelect, selectionMode = false, filters = {} as FetchParams }) => {
  const { isLoaded } = useJsApiLoader({
    googleMapsApiKey: GOOGLE_MAPS_API_KEY,
    libraries: GOOGLE_MAPS_LIBRARIES,
    mapIds: ["7dabcc41eef25214"],
  });

  const [map, setMap] = useState<google.maps.Map | null>(null);
  const [searchBox, setSearchBox] =
    useState<google.maps.places.SearchBox | null>(null);
  const [selectedLocation, setSelectedLocation] = useState<google.maps.LatLng | null>(null);

  const onLoadMap = useCallback((mapInstance: google.maps.Map) => {
    setMap(mapInstance);
  }, []);

  const onUnmountMap = useCallback(() => {
    setMap(null);
  }, []);

  const onSearchBoxLoad = (ref: google.maps.places.SearchBox) => {
    setSearchBox(ref);
  };

  const onPlacesChanged = () => {
    if (searchBox && map) {
      const places = searchBox.getPlaces();
      if (places && places.length > 0) {
        const place = places[0];
        if (place.geometry?.location) {
          map.setCenter(place.geometry.location);
          map.setZoom(12);

          if (selectionMode && onLocationSelect) {
            setSelectedLocation(place.geometry.location);
            onLocationSelect({
              x: place.geometry.location.lat(),
              y: place.geometry.location.lng()
            });
          }
        }
      }
    }
  };

  const resetPosition = () => {
    if (map) {
      map.setCenter(MAP_CONFIG.center);
      map.setZoom(MAP_CONFIG.zoom);
    }
  };

  const handleMapClick = (e: google.maps.MapMouseEvent) => {
    if (selectionMode && onLocationSelect && e.latLng) {
      const location = e.latLng;
      setSelectedLocation(location);
      onLocationSelect({
        x: location.lat(),
        y: location.lng()
      });
    }
  };

  useEffect(() => {
    let isMounted = true;
    let debounceTimeout: NodeJS.Timeout;

    if (map && !selectionMode) {
      debounceTimeout = setTimeout(async () => {
        if (isMounted) {
          try {
            await markersService.getGoogleMapsMarkers(map, filters);
          } catch (error) {
            console.error('Error loading markers:', error);
          }
        }
      }, 500);
    }

    return () => {
      isMounted = false;
      if (debounceTimeout) {
        clearTimeout(debounceTimeout);
      }
    };
  }, [filters, map, selectionMode]);

  if (!isLoaded) {
    return <div className="d-flex justify-content-center align-items-center h-100">
      Loading...
      </div>;
  }

  return (
    <div className="d-flex flex-column h-100">
      <h2 className="text-white fs-3 mb-3">
        {selectionMode ? "Select Printer Location" : "Map of printers"}
      </h2>
      <div
        className="map-wrapper mb-3"
        style={{
          height: selectionMode ? "400px" : "calc(100vh - 350px)",
          width: "100%",
          borderRadius: "10px",
          overflow: "hidden",
          background: "rgba(255, 255, 255, 0.05)",
        }}
      >
        <GoogleMap
          mapContainerStyle={{
            width: "100%",
            height: "100%",
          }}
          center={MAP_CONFIG.center}
          zoom={MAP_CONFIG.zoom}
          onLoad={onLoadMap}
          onUnmount={onUnmountMap}
          onClick={handleMapClick}
          options={{ 
            mapId: MAP_CONFIG.mapId,
            minZoom: MAP_CONFIG.minZoom,
            restriction: {
              latLngBounds: MAP_CONFIG.maxBounds,
              strictBounds: true,
            },
            streetViewControl: false,
          }}
        >
          {selectedLocation && selectionMode && map && (
            <AdvancedMarker position={selectedLocation} map={map} />
          )}
        </GoogleMap>
      </div>
      <div className="d-flex justify-content-center align-items-center gap-3">
        <StandaloneSearchBox
          onLoad={onSearchBoxLoad}
          onPlacesChanged={onPlacesChanged}
        >
          <input
            type="text"
            className="form-control map-search-input"
            placeholder="Search location..."
          />
        </StandaloneSearchBox>
        {!selectionMode && (
          <button 
            className="btn d-flex align-items-center gap-2"
            style={{
              background: "rgba(255, 255, 255, 0.15)",
              border: "1px solid rgba(255, 255, 255, 0.2)",
              color: "white",
              backdropFilter: "blur(10px)",
              height: "42px",
              borderRadius: "8px",
              padding: "0 20px",
              transition: "all 0.2s ease",
            }}
            onClick={resetPosition}
          >
            <i className="bi bi-geo-alt"></i>
            Reset position
          </button>
        )}
      </div>
    </div>
  );
};

export default MapSection;