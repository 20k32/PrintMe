import React, { useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { FilterOption } from "../types";
import { FILTER_OPTIONS, INITIAL_FILTER_STATE, FilterKey } from "../../../constants";

const FilterFold: React.FC<FilterOption> = ({
  label,
  options,
  state,
  setState,
  isOpen,
  setIsOpen,
}) => {
  const [showContent, setShowContent] = useState(isOpen);
  const selectedCount = state.length;

  useEffect(() => {
    if (isOpen) {
      setShowContent(true);
    } else {
      const timer = setTimeout(() => {
        setShowContent(false); 
      }, 400);
      return () => clearTimeout(timer);
    }
  }, [isOpen]);

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const currentState = [...state];

    if (e.target.checked) {
      currentState.push(value);
    } else {
      const index = currentState.indexOf(value);
      if (index > -1) {
        currentState.splice(index, 1);
      }
    }

    setState(currentState);
  };

  return (
    <div className="filter-fold border rounded p-2">
      <label
        onClick={() => setIsOpen(!isOpen)}
        className="d-flex justify-content-between align-items-center px-3 py-2 bg-light rounded mb-2"
        style={{ cursor: "pointer" }}
      >
        <span>{label}</span>
        {selectedCount > 0 && (
          <span className="badge bg-primary ms-2">{selectedCount}</span>
        )}
        <span
          className={`ms-2 arrow-icon ${isOpen ? "rotate-180" : "rotate-0"}`}
        >
          ↑
        </span>
      </label>
      <div
        className={`content-wrapper ${
          isOpen ? "content-expanded" : "content-collapsed"
        }`}
      >
        {showContent &&
          options.map((option) => (
            <label key={option} className="d-flex align-items-center px-3 py-2">
              <input
                type="checkbox"
                value={option}
                checked={state.includes(option)}
                onChange={handleCheckboxChange}
                className="form-check-input me-2"
              />
              {option}
            </label>
          ))}
      </div>
    </div>
  );
};

const FilterFoldGroup: React.FC = () => {
  const [filters, setFilters] = useState(INITIAL_FILTER_STATE);
  const [openStates, setOpenStates] = useState({
    materials: true,
    sizes: true,
    colors: true,
  });

  const updateFilter = (key: FilterKey) => (value: string[]) => {
    setFilters((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const updateOpenState =
    (key: FilterKey) => (value: boolean | ((prev: boolean) => boolean)) => {
      setOpenStates((prev) => ({
        ...prev,
        [key]: typeof value === "function" ? value(prev[key]) : value,
      }));
    };

  return (
    <div className="bg-white p-4 rounded h-100">
      <div className="d-flex flex-wrap gap-3">
        <FilterFold
          label="Materials"
          options={FILTER_OPTIONS.materials}
          state={filters.materials}
          setState={updateFilter("materials")}
          isOpen={openStates.materials}
          setIsOpen={updateOpenState("materials")}
        />
        <FilterFold
          label="Size"
          options={FILTER_OPTIONS.sizes}
          state={filters.sizes}
          setState={updateFilter("sizes")}
          isOpen={openStates.sizes}
          setIsOpen={updateOpenState("sizes")}
        />
        <FilterFold
          label="Colors"
          options={FILTER_OPTIONS.colors}
          state={filters.colors}
          setState={updateFilter("colors")}
          isOpen={openStates.colors}
          setIsOpen={updateOpenState("colors")}
        />
      </div>
      <p className="alert alert-warning mt-4">
        You can add your default address in profile
      </p>
    </div>
  );
};

export default FilterFoldGroup;
