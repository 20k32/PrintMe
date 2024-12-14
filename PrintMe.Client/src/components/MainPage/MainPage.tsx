import React, { useEffect, useState } from "react";
import FilterFoldGroup from "./components/FilterSection";
import MapSection from "./components/MapSection";
import backgroundImage from "./assets/images/background.png";

const MainPage: React.FC = () => {
  const [key, setKey] = useState(0);
  const [filters, setFilters] = useState<Record<string, string[]>>({});

  useEffect(() => {
    setKey((prev) => prev + 1);
  }, []);

  const handleFiltersChange = (newFilters: Record<string, string[]>) => {
    setFilters(newFilters);
  };

  return (
    <div
      style={{
        backgroundImage: `url(${backgroundImage})`,
        backgroundSize: "cover",
        backgroundRepeat: "no-repeat",
        backgroundPosition: "center center",
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "flex-start",
        gap: "30px",
        padding: "40px",
      }}
    >
      <div
        style={{
          flex: 3,
          borderRadius: "25px",
          overflow: "hidden",
          boxShadow: "0 8px 16px rgba(0, 0, 0, 0.2)",
          backgroundColor: "#fff",
          height: "90vh",
        }}
      >
        <MapSection key={key} filters={filters} />
      </div>

      <div
        style={{
          flex: 1,
          borderRadius: "25px",
          overflow: "hidden",
          boxShadow: "0 8px 16px rgba(0, 0, 0, 0.2)",
          backgroundColor: "#fff",
          height: "auto",
          display: "flex",
          flexDirection: "column",
          justifyContent: "space-between",
          padding: "20px",
          minHeight: "40vh",
        }}
      >
        <h2 className="fs-4 mb-3">Filter by</h2>
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            gap: "30px",
            flexWrap: "wrap",
          }}
        >
          <FilterFoldGroup onFiltersChange={handleFiltersChange} />
        </div>
      </div>
    </div>
  );
};

export default MainPage;
