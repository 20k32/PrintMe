import React, { useState, useEffect } from "react";
import { FilterOption } from "../types";
import { FILTER_OPTIONS, INITIAL_FILTER_STATE, FilterKey } from "../../../constants";
import "../assets/css/filterSection.css";

const FilterFold: React.FC<FilterOption> = ({
  label,
  options,
  state,
  setState,
  isOpen,
  setIsOpen,
}) => {
  const [animationState, setAnimationState] = useState(
    isOpen ? "open" : "closed"
  );
  const [showContent, setShowContent] = useState(isOpen);
  const selectedCount = state.length;

  useEffect(() => {
    if (isOpen) {
      setShowContent(true);
      setAnimationState("opening");
    } else {
      setAnimationState("closing");
      const timer = setTimeout(() => {
        setShowContent(false);
      }, 200);
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
    <div className={`${animationState}`}>
      <label onClick={() => setIsOpen(!isOpen)} className="filterLabel">
        <span className="labelText">{label}</span>
        {selectedCount > 0 && <span className="counter">{selectedCount}</span>}
        <span className="arrow">{isOpen ? "↑" : "↓"}</span>
      </label>
      {showContent && (
        <div className="checkboxGroup">
          {options.map((option) => (
            <label key={option}>
              <input
                type="checkbox"
                value={option}
                checked={state.includes(option)}
                onChange={handleCheckboxChange}
              />
              {option}
            </label>
          ))}
        </div>
      )}
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
    <div className="filter-section">
      <h2>Filter by</h2>
      <div className="filter-options">
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
      <p className="tip-container">You can add your default address in profile</p>
    </div>
  );
};

export default FilterFoldGroup;
