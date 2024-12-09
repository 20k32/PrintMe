import React, { useState } from "react";
import {
  GoogleMap,
  LoadScript,
  StandaloneSearchBox,
  Marker,
} from "@react-google-maps/api";
import { GOOGLE_MAPS_API_KEY, MAP_CONFIG } from "../../../constants";

interface MapSectionProps {
  onLocationSelect?: (location: { x: number; y: number }) => void;
  selectionMode?: boolean;
}

const MapSection: React.FC<MapSectionProps> = ({ onLocationSelect, selectionMode = false }) => {
  const [map, setMap] = useState<google.maps.Map | null>(null);
  const [searchBox, setSearchBox] =
    useState<google.maps.places.SearchBox | null>(null);
  const [selectedLocation, setSelectedLocation] = useState<google.maps.LatLng | null>(null);

  const onLoad = (map: google.maps.Map) => {
    setMap(map);
  };

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

  return (
    <div className="d-flex flex-column justify-content-between h-100">
      <div className="bg-white shadow-sm p-3 rounded-start flex-grow-1">
        <h2 className="fs-3 mb-3">
          {selectionMode ? "Select Printer Location" : "Map of printers"}
        </h2>
        <LoadScript googleMapsApiKey={GOOGLE_MAPS_API_KEY} libraries={MAP_CONFIG.libraries}>
          <div
            className="border border-dark rounded shadow-sm mx-auto"
            style={{
              height: selectionMode ? "400px" : "80%",
              width: "100%",
              overflow: "hidden",
            }}
          >
            <GoogleMap
              mapContainerStyle={{
                width: "100%",
                height: "100%",
              }}
              center={MAP_CONFIG.center}
              zoom={MAP_CONFIG.zoom}
              onLoad={onLoad}
              onClick={handleMapClick}
            >
              {selectedLocation && selectionMode && (
                <Marker position={selectedLocation} />
              )}
            </GoogleMap>
          </div>
          <div className="d-flex justify-content-center align-items-center gap-3 mt-3">
            <StandaloneSearchBox
              onLoad={onSearchBoxLoad}
              onPlacesChanged={onPlacesChanged}
            >
              <input
                type="text"
                className="form-control w-100"
                placeholder="Search location..."
                style={{
                  boxShadow: "none",
                }}
              />
            </StandaloneSearchBox>
            <button className="btn btn-primary px-4" onClick={resetPosition}>
              Reset position
            </button>
          </div>
        </LoadScript>
      </div>
    </div>
  );
};

export default MapSection;
