import React, { useEffect, useState, useCallback } from "react";
import FilterFoldGroup from "./components/FilterSection";
import MapSection from "./components/MapSection";
import { FilterState } from "../../constants";
import { FetchParams } from "../../services/markersService";
import "./assets/mainpage.css";

const MainPage: React.FC = () => {
  const [key, setKey] = useState(0);
  const [filters, setFilters] = useState<FetchParams>({});

  useEffect(() => {
    setKey((prev) => prev + 1);
  }, []);

  const handleFiltersChange = useCallback((newFilters: FilterState) => {
    const convertedFilters = { ...newFilters } as FetchParams;
    setFilters(prev => {
      if (JSON.stringify(prev) === JSON.stringify(convertedFilters)) {
        return prev;
      }
      return convertedFilters;
    });
  }, []);

  return (
    <div className="mainpage-container">
      <div className="mainpage-content">
        <div className="map-container">
          <MapSection key={key} filters={filters} />
        </div>

        <div className="filter-container">
          <h2 className="text-white fs-4 mb-3">Filter by</h2>
          <div className="filter-content">
            <FilterFoldGroup onFiltersChange={handleFiltersChange} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default MainPage;
