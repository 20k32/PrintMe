import React, { useState } from "react";
import {
  GoogleMap,
  LoadScript,
  StandaloneSearchBox,
  Libraries,
} from "@react-google-maps/api";
import { GOOGLE_MAPS_API_KEY, MAP_CONFIG } from "../../../constants";

const libraries: Libraries = ["places"];

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
    <div
      className="h-100 d-flex flex-column justify-content-between"
      style={{ overflow: "hidden" }}
    >
      <div className="bg-white shadow-sm p-4 rounded-start flex-grow-1">
        <h2 className="fs-3 mb-3">Map of printers</h2>
        <LoadScript googleMapsApiKey={GOOGLE_MAPS_API_KEY} libraries={libraries}>
          <div
            className="border border-dark rounded shadow-sm mx-auto"
            style={{
              height: "80%",
              minHeight: "400px", 
              flex: 1,
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
            />
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
