import React, { useState, useEffect, useCallback } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { FilterOption } from "../types";
import { INITIAL_FILTER_STATE, FilterKey, Material } from "../../../constants";
import { printerService } from "../../../services/printerService";

const SizeInput: React.FC<{
  label: string;
  value: number;
  onChange: (value: number) => void;
}> = ({ label, value, onChange }) => {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value === '' ? 0 : Number(e.target.value);
    onChange(val);
  };

  return (
    <div className="form-group border-0 rounded p-3" style={{ background: 'rgba(255, 255, 255, 0.08)' }}>
      <label className="form-label text-white mb-2">{label} (mm)</label>
      <input
        type="number"
        className="form-control"
        value={value || ''}
        onChange={handleChange}
        min="0"
        placeholder="Enter size in mm"
        style={{
          background: "rgba(255, 255, 255, 0.15)",
          border: "1px solid rgba(255, 255, 255, 0.2)",
          color: "white",
          backdropFilter: "blur(10px)",
        }}
      />
    </div>
  );
};

interface FilterFoldProps extends Omit<FilterOption, 'options' | 'state' | 'setState'> {
  options: Material[];
  state: Material[];
  setState: (value: Material[]) => void;
}

const FilterFold: React.FC<FilterFoldProps> = ({
  label,
  options = [],
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
    const materialId = Number(e.target.value);
    const currentState = [...state];
    
    if (e.target.checked) {
      const material = options.find(m => m.printMaterialId === materialId);
      if (material) currentState.push(material);
    } else {
      const index = currentState.findIndex(m => m.printMaterialId === materialId);
      if (index > -1) {
        currentState.splice(index, 1);
      }
    }

    setState(currentState);
  };

  return (
    <div className="filter-fold p-2" style={{ 
      background: 'rgba(255, 255, 255, 0.05)',
      borderRadius: '10px',
      width: '100%'
    }}>
      <label
        onClick={() => setIsOpen(!isOpen)}
        className="d-flex justify-content-between align-items-center px-3 py-2 rounded mb-2 text-white"
        style={{ 
          cursor: "pointer", 
          background: 'rgba(255, 255, 255, 0.1)',
          transition: 'all 0.3s ease'
        }}
      >
        <span className="fw-bold">{label}</span>
        <div className="d-flex align-items-center">
          {selectedCount > 0 && (
            <span className="badge rounded-pill" style={{
              background: 'rgba(255, 255, 255, 0.2)',
              marginRight: '10px'
            }}>{selectedCount}</span>
          )}
          <i className={`bi ${isOpen ? 'bi-chevron-up' : 'bi-chevron-down'}`}></i>
        </div>
      </label>
      <div
        className={`content-wrapper ${
          isOpen ? "content-expanded" : "content-collapsed"
        }`}
        style={{
          maxHeight: isOpen ? '300px' : '0',
          overflow: 'hidden',
          visibility: isOpen ? 'visible' : 'hidden',
          transition: 'max-height 0.3s ease, visibility 0s' + (isOpen ? ' 0s' : ' 0.3s'),
        }}
      >
        <div className="materials-grid p-2" style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fill, minmax(140px, 1fr))',
          gap: '8px'
        }}>
          {showContent && options?.map((option) => (
            <label 
              key={option.printMaterialId} 
              className="material-item d-flex align-items-center px-3 py-2 text-white"
              style={{
                background: state.some(m => m.printMaterialId === option.printMaterialId) 
                  ? 'rgba(255, 255, 255, 0.2)' 
                  : 'rgba(255, 255, 255, 0.1)',
                borderRadius: '8px',
                cursor: 'pointer',
                transition: 'all 0.2s ease'
              }}
            >
              <input
                type="checkbox"
                value={option.printMaterialId}
                checked={state.some(m => m.printMaterialId === option.printMaterialId)}
                onChange={handleCheckboxChange}
                className="form-check-input me-2"
              />
              <span style={{ fontSize: '0.9rem' }}>{option.name}</span>
            </label>
          ))}
        </div>
      </div>
    </div>
  );
};

interface FilterFoldGroupProps {
  onFiltersChange: (filters: {
    materials: Material[];
    maxModelHeight: number;
    maxModelWidth: number;
  }) => void;
}

type FilterValue = Material[] | number;

const FilterFoldGroup: React.FC<FilterFoldGroupProps> = ({ onFiltersChange }) => {
  const [filters, setFilters] = useState(INITIAL_FILTER_STATE);
  const [openStates, setOpenStates] = useState<Record<FilterKey, boolean>>({
    materials: true,
    maxModelHeight: true,
    maxModelWidth: true
  });
  const [materialOptions, setMaterialOptions] = useState<Material[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMaterials = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const materials = await printerService.getMaterials();
        setMaterialOptions(materials);
      } catch (error) {
        console.error("Failed to fetch materials", error);
        setError("Failed to load materials. Please try again later.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchMaterials();
  }, []);

  const updateFilter = useCallback((key: FilterKey) => (value: FilterValue) => {
    setFilters(prev => ({
      ...prev,
      [key]: value,
    }));
  }, []);

  const updateOpenState = useCallback((key: FilterKey) => (value: boolean | ((prev: boolean) => boolean)) => {
    setOpenStates(prev => ({
      ...prev,
      [key]: typeof value === "function" ? value(prev[key]) : value,
    }));
  }, []);

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      onFiltersChange(filters);
    }, 300);

    return () => clearTimeout(timeoutId);
  }, [filters, onFiltersChange]);

  return (
    <div className="p-0 rounded h-100" style={{ background: 'transparent' }}>
      <div className="d-flex flex-wrap gap-3">
        {isLoading ? (
          <div className="text-white p-3">Loading materials...</div>
        ) : error ? (
          <div className="text-danger p-3">{error}</div>
        ) : (
          <FilterFold
            label="Materials"
            options={materialOptions}
            state={filters.materials}
            setState={updateFilter("materials") as (value: Material[]) => void}
            isOpen={openStates.materials}
            setIsOpen={updateOpenState("materials")}
          />
        )}
        <div className="d-flex gap-3">
          <SizeInput
            label="Height"
            value={filters.maxModelHeight}
            onChange={updateFilter("maxModelHeight") as (value: number) => void}
          />
          <SizeInput
            label="Width"
            value={filters.maxModelWidth}
            onChange={updateFilter("maxModelWidth") as (value: number) => void}
          />
        </div>
      </div>
      <div className="alert mt-4 text-white" style={{ background: 'rgba(255, 255, 255, 0.1)' }}>
        You can add your default address in profile
      </div>
    </div>
  );
};

export default FilterFoldGroup;
