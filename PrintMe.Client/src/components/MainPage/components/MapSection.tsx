import React from "react";
import { GoogleMap, LoadScript } from "@react-google-maps/api";
import { GOOGLE_MAPS_API_KEY, MAP_CONFIG } from "../constants";
import "../assets/css/mapSection.css";

const MapSection: React.FC = () => {
  return (
    <div className="map-section">
      <h2>Map of printers</h2>
      <div className="map">
        <LoadScript googleMapsApiKey={GOOGLE_MAPS_API_KEY}>
          <GoogleMap
            mapContainerStyle={MAP_CONFIG.containerStyle}
            center={MAP_CONFIG.center}
            zoom={MAP_CONFIG.zoom}
          />
        </LoadScript>
      </div>
    </div>
  );
};

export default MapSection;
