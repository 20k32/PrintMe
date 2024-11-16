import React, { useState } from "react";
import {
  GoogleMap,
  LoadScript,
  StandaloneSearchBox,
} from "@react-google-maps/api";
import { GOOGLE_MAPS_API_KEY, MAP_CONFIG } from "../constants";
import "../assets/css/mapSection.css";

const MapSection: React.FC = () => {
  const [map, setMap] = useState<google.maps.Map | null>(null);
  const [searchBox, setSearchBox] =
    useState<google.maps.places.SearchBox | null>(null);

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

  return (
    <div className="map-section">
      <h2>Map of printers</h2>
      <div className="map-container">
        <LoadScript
          googleMapsApiKey={GOOGLE_MAPS_API_KEY}
          libraries={["places"]}
        >
          <div className="map-controls">
            <StandaloneSearchBox
              onLoad={onSearchBoxLoad}
              onPlacesChanged={onPlacesChanged}
            >
              <input
                type="text"
                placeholder="Search location..."
                className="map-search-input"
              />
            </StandaloneSearchBox>
            <button onClick={resetPosition} className="reset-button">
              Reset position
            </button>
          </div>
          <div className="map">
            <GoogleMap
              mapContainerStyle={MAP_CONFIG.containerStyle}
              center={MAP_CONFIG.center}
              zoom={MAP_CONFIG.zoom}
              onLoad={onLoad}
            />
          </div>
        </LoadScript>
      </div>
    </div>
  );
};

export default MapSection;
