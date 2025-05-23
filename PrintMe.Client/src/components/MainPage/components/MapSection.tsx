import React, {
  useState,
  useCallback,
  useEffect,
  useRef,
  useMemo,
} from "react";
import {
  GoogleMap,
  useJsApiLoader,
  StandaloneSearchBox,
  InfoWindow,
} from "@react-google-maps/api";
import {
  GOOGLE_MAPS_API_KEY,
  MAP_CONFIG,
  GOOGLE_MAPS_LIBRARIES,
} from "../../../constants";
import { markersService } from "../../../services/markersService";
import { authService } from "../../../services/authService";
import {
  FetchParams,
  MarkerWithPrinterInfo,
} from "../../../services/markersService";
import { SimplePrinterDto } from "../../../types/api";
import {userService} from "../../../services/userService.ts";

interface MapSectionProps {
  onLocationSelect?: (location: { x: number; y: number }) => void;
  selectionMode?: boolean;
  filters?: FetchParams;
  onMarkerClick?: (printer: SimplePrinterDto) => void;
  singleMarkerLocation?: { lat: number; lng: number };
  userPrinterIds: number[];
}

interface AdvancedMarkerProps {
  position: google.maps.LatLng;
  map: google.maps.Map;
}

const AdvancedMarker: React.FC<AdvancedMarkerProps> = ({ position, map }) => {
  const markerRef = useRef<google.maps.marker.AdvancedMarkerElement | null>(
    null
  );

  useEffect(() => {
    let isMounted = true;

    const initMarker = async () => {
      try {
        const { AdvancedMarkerElement } = (await google.maps.importLibrary(
          "marker"
        )) as google.maps.MarkerLibrary;
        if (isMounted && !markerRef.current) {
          markerRef.current = new AdvancedMarkerElement({
            position,
            map,
          });
        }
      } catch (error) {
        console.error("Error initializing marker:", error);
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

interface MarkerContentProps {
  printerInfo: SimplePrinterDto;
  isLoggedIn: boolean;
  onCreateOrder: () => void;
  isUserPrinter: boolean;
  isUserEmailVerified: boolean;
}

const MarkerContent: React.FC<MarkerContentProps> = ({
  printerInfo,
  isLoggedIn,
  onCreateOrder,
  isUserPrinter,
  isUserEmailVerified,
}) => (
  <div style={{ color: "black" }}>
    <h6>Printer {printerInfo.modelName}</h6>
    <p>
      Materials:{" "}
      {printerInfo.materials.map((material) => material.name).join(", ")}
    </p>
    {isLoggedIn ? (
      isUserPrinter ? (
        <button
          className="btn btn-secondary create-order-btn"
          disabled
          title="You can't place an order on your own printer"
        >
          Your Printer
        </button>
      ) : isUserEmailVerified ? (
        <button
          className="btn btn-primary create-order-btn"
          style={{ backgroundColor: "#2c1d55" }}
          onClick={onCreateOrder}
        >
          Create Order
        </button>
      ) : (
        <button
          className="btn btn-secondary create-order-btn"
          disabled
          title="Please verify your email to create an order"
        >
          Verify Email to Order
        </button>
      )
    ) : (
      <button
        className="btn btn-primary create-order-btn"
        style={{ backgroundColor: "gray" }}
        disabled
        title="Please log in to create an order"
      >
        Create Order
      </button>
    )}
  </div>
);

const InfoWindowContent: React.FC<{
  marker: MarkerWithPrinterInfo;
  onClose: () => void;
  onMarkerClick: (printer: SimplePrinterDto) => void;
  userPrinterIds: number[];
  isUserEmailVerified: boolean;
  }> = ({ marker, onClose, onMarkerClick, userPrinterIds, isUserEmailVerified }) => {
  const isLoggedIn = authService.isLoggedIn();
  const isUserPrinter = userPrinterIds.includes(marker.printerInfo.id);
  return (
    <InfoWindow
      anchor={marker as unknown as google.maps.MVCObject}
      onCloseClick={onClose}
    >
      <MarkerContent
        printerInfo={marker.printerInfo}
        isLoggedIn={isLoggedIn}
        onCreateOrder={() => onMarkerClick(marker.printerInfo)}
        isUserPrinter={isUserPrinter}
        isUserEmailVerified={isUserEmailVerified}
      />
    </InfoWindow>
  );
};

const MapSection: React.FC<MapSectionProps> = ({
  onLocationSelect,
  selectionMode = false,
  filters = {} as FetchParams,
  onMarkerClick,
  singleMarkerLocation,
  userPrinterIds,
  }) => {
  const { isLoaded } = useJsApiLoader({
    googleMapsApiKey: GOOGLE_MAPS_API_KEY,
    libraries: GOOGLE_MAPS_LIBRARIES,
    mapIds: ["7dabcc41eef25214"],
  });

  const [map, setMap] = useState<google.maps.Map | null>(null);
  const [searchBox, setSearchBox] =
    useState<google.maps.places.SearchBox | null>(null);
  const [selectedLocation, setSelectedLocation] =
    useState<google.maps.LatLng | null>(null);
  const [activeMarker, setActiveMarker] =
    useState<MarkerWithPrinterInfo | null>(null);
  const [isUserEmailVerified, setIsUserEmailVerified] = 
      useState<boolean>(false);

  const markersRef = useRef<MarkerWithPrinterInfo[]>([]);
  const lastFiltersRef = useRef<FetchParams>({});

  const onLoadMap = useCallback((mapInstance: google.maps.Map) => {
    setMap(mapInstance);
    if (singleMarkerLocation) {
      setSingleMarker(singleMarkerLocation.lat, singleMarkerLocation.lng);
    }
  }, [singleMarkerLocation]);

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
              y: place.geometry.location.lng(),
            });
          }
        }
      }
    }
  };

  const resetPosition = () => {
    if (map) {
      map.setZoom(MAP_CONFIG.zoom);
      setTimeout(() => {
        map?.setCenter(MAP_CONFIG.center);
      }, 50);
    }
  };

  const handleMapClick = (e: google.maps.MapMouseEvent) => {
    if (selectionMode && onLocationSelect && e.latLng) {
      const location = e.latLng;
      setSelectedLocation(location);
      onLocationSelect({
        x: location.lng(),
        y: location.lat(),
      });
    }
  };

  const handleMarkerClick = useCallback(
    (printer: SimplePrinterDto) => {
      if (onMarkerClick) {
        onMarkerClick(printer);
      }
    },
    [onMarkerClick]
  );

  const setupMarkerUI = useCallback((marker: MarkerWithPrinterInfo) => {
    marker.addListener("click", () => {
      setActiveMarker(null);
      setTimeout(() => {
        setActiveMarker(marker);
      }, 10);
    });
  }, []);

  const memoizedFilters = useMemo(() => filters, [filters]);

  const setSingleMarker = useCallback((lat: number, lng: number) => {
    if (map) {
      const location = new google.maps.LatLng(lat, lng);
      setSelectedLocation(location);
      map.setCenter(location);
      map.setZoom(5);
    }
  }, [map]);

  useEffect(() => {
    let isMounted = true;
    let debounceTimeout: NodeJS.Timeout;

    if (map && !selectionMode) {
      debounceTimeout = setTimeout(async () => {
        if (isMounted) {
          try {
            if (
              JSON.stringify(lastFiltersRef.current) !==
              JSON.stringify(memoizedFilters)
            ) {
              const newMarkers = await markersService.getGoogleMapsMarkers(
                map,
                memoizedFilters
              );

              newMarkers.forEach((marker) => setupMarkerUI(marker));

              markersRef.current.forEach((marker) => {
                if (!newMarkers.includes(marker)) {
                  marker.map = null;
                }
              });

              markersRef.current = newMarkers;
              lastFiltersRef.current = memoizedFilters;
            }
          } catch (error) {
            console.error("Error loading markers:", error);
          }
        }
      }, 300);
    }

    return () => {
      isMounted = false;
      if (debounceTimeout) {
        clearTimeout(debounceTimeout);
      }
    };
  }, [memoizedFilters, map, selectionMode, setupMarkerUI, handleMarkerClick]);

  useEffect(() => {
    const fetchUserEmailVerificationStatus = async () => {
      if (authService.isLoggedIn()) {
        try {
          const isVerified = await userService.getIsUserEmailVerified();
          setIsUserEmailVerified(isVerified);
        } catch (error) {
          console.error("Error fetching user email verification status:", error)
        }
      }
    }

    fetchUserEmailVerificationStatus()
  }, [])
  
  useEffect(() => {
    return () => {
      markersRef.current.forEach((marker) => (marker.map = null));
      markersRef.current = [];
    };
  }, []);

  useEffect(() => {
    if (map && singleMarkerLocation) {
      setSingleMarker(singleMarkerLocation.lat, singleMarkerLocation.lng);
    }
  }, [map, singleMarkerLocation, setSingleMarker]);

  if (!isLoaded) {
    return (
      <div className="d-flex justify-content-center align-items-center h-100">
        Loading...
      </div>
    );
  }

  return (
    <>
      <div className="d-flex flex-column h-100">
        {!singleMarkerLocation && (
          <h2 className="text-white fs-3 mb-3">
            {selectionMode ? "Select Printer Location" : "Map of printers"}
          </h2>
        )}
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
            {selectedLocation && map && (
              <AdvancedMarker position={selectedLocation} map={map} />
            )}
            {activeMarker && (
              <InfoWindowContent
                marker={activeMarker}
                onClose={() => setActiveMarker(null)}
                onMarkerClick={handleMarkerClick}
                userPrinterIds={userPrinterIds}
                isUserEmailVerified={isUserEmailVerified}
              />
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
    </>
  );
};

export default React.memo(MapSection)