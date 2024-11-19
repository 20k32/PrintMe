import React from "react";
import MapSection from "./components/MapSection";
import FilterSection from "./components/FilterSection";
import "./assets/css/mainPage.css";

const MainPage: React.FC = () => {
  return (
    <div className="app">
      <main className="main-content">
        <MapSection />
        <FilterSection />
      </main>
    </div>
  );
};

export default MainPage;
