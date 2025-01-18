import React, { useEffect, useState, useCallback, useMemo } from "react";
import FilterFoldGroup from "./components/FilterSection";
import MapSection from "./components/MapSection";
import OrderModal from "./components/OrderModal";
import { FilterState } from "../../constants";
import { FetchParams } from "../../services/markersService";
import { SimplePrinterDto } from "../../types/api";
import "./assets/mainpage.css";

const MainPage: React.FC = () => {
  const [key, setKey] = useState(0);
  const [filters, setFilters] = useState<FetchParams>({});
  const [selectedPrinter, setSelectedPrinter] = useState<SimplePrinterDto | null>(null);

  useEffect(() => {
    setKey((prev) => prev + 1);
  }, []);

  const debouncedSetFilters = useMemo(() => {
    let timeoutId: NodeJS.Timeout;
    return (newFilters: FilterState) => {
      clearTimeout(timeoutId);
      timeoutId = setTimeout(() => {
        const convertedFilters = { ...newFilters } as FetchParams;
        setFilters(prev => {
          if (JSON.stringify(prev) === JSON.stringify(convertedFilters)) {
            return prev;
          }
          return convertedFilters;
        });
      }, 10);
    };
  }, []);

  const handleFiltersChange = useCallback((newFilters: FilterState) => {
    debouncedSetFilters(newFilters);
  }, [debouncedSetFilters]);

  const handleMarkerClick = useCallback((printer: SimplePrinterDto) => {
    setSelectedPrinter(printer);
  }, []);

  const handleOrderSubmit = useCallback((orderData: unknown) => {
    console.log(orderData);
    setSelectedPrinter(null);
  }, []);

  return (
    <div className="mainpage-container">
      <div className="mainpage-content">
        <div className="map-container">
          <MapSection 
            key={key} 
            filters={filters} 
            onMarkerClick={handleMarkerClick}
          />
        </div>

        <div className="filter-container">
          <h2 className="text-white fs-4 mb-3">Filter by</h2>
          <div className="filter-content">
            <FilterFoldGroup onFiltersChange={handleFiltersChange} />
          </div>
        </div>
      </div>

      {selectedPrinter && (
        <OrderModal
          printer={selectedPrinter}
          onClose={() => setSelectedPrinter(null)}
          onSubmit={handleOrderSubmit}
        />
      )}
    </div>
  );
};

export default MainPage;
