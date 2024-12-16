import React, { useState, useEffect } from "react";
import { requestsService } from "../../../services/requestsService";
import { printerService } from "../../../services/printerService";
import { useNavigate } from "react-router-dom";
import MapSection from "../../MainPage/components/MapSection";
import { handleApiError } from '../../../utils/apiErrorHandler';
import { PrinterApplicationDto } from "../../../types/requests";
import { Material } from "../../../constants";
import "../assets/requests.css";

export const AddPrinter = () => {
  const navigate = useNavigate();
  const [printer, setPrinter] = useState<PrinterApplicationDto>({
    description: "",
    minModelHeight: 0,
    minModelWidth: 0,
    maxModelHeight: 0,
    maxModelWidth: 0,
    locationX: 0,
    locationY: 0,
    materials: [],
  });
  const [error, setError] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);
  const [selectedMaterials, setSelectedMaterials] = useState<number[]>([]);
  const [materials, setMaterials] = useState<Material[]>([]);
  const [materialsLoading, setMaterialsLoading] = useState(true);

  useEffect(() => {
    const loadMaterials = async () => {
      try {
        const fetchedMaterials = await printerService.getMaterials();
        setMaterials(fetchedMaterials);
      } catch (error) {
        setError(handleApiError(error, {
          badRequest: "Failed to load materials."
        }));
      } finally {
        setMaterialsLoading(false);
      }
    };

    loadMaterials();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      await requestsService.submitPrinterApplication(printer);
      navigate("/requests");
    } catch (error) {
      setError(handleApiError(error, {
        badRequest: "Failed to submit printer application. Please check your input."
      }));
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setPrinter((prev: PrinterApplicationDto) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleLocationSelect = (location: { x: number; y: number }) => {
    setPrinter((prev: PrinterApplicationDto) => ({
      ...prev,
      locationX: location.x,
      locationY: location.y,
    }));
  };

  const handleMaterialChange = (materialId: number) => {
    setSelectedMaterials((prev: number[]) => {
      const newMaterials = prev.includes(materialId)
        ? prev.filter((id) => id !== materialId)
        : [...prev, materialId];

      setPrinter((prevPrinter: PrinterApplicationDto) => ({
        ...prevPrinter,
        materials: newMaterials.map((id) => ({ materialId: id })),
      }));

      return newMaterials;
    });
  };

  return (
    <div className="requests-container">
      <div className="requests-content">
        <h1 className="text-white mb-4">Add New Printer</h1>
        <div className="printer-form-container">
          {error && (
            <div className="alert alert-danger d-flex align-items-center">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div className="printer-form-section">
              <h5 className="mb-3">Description</h5>
              <textarea
                className="form-control"
                name="description"
                value={printer.description}
                onChange={handleInputChange}
                rows={3}
              />
            </div>

            <div className="printer-form-section">
              <h5 className="mb-3">Model Dimensions</h5>
              <div className="row g-3">
                <div className="col-md-6">
                  <label className="form-label">Min Height (mm)</label>
                  <input
                    type="number"
                    className="form-control"
                    name="minModelHeight"
                    value={printer.minModelHeight}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Min Width (mm)</label>
                  <input
                    type="number"
                    className="form-control"
                    name="minModelWidth"
                    value={printer.minModelWidth}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Max Height (mm)</label>
                  <input
                    type="number"
                    className="form-control"
                    name="maxModelHeight"
                    value={printer.maxModelHeight}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Max Width (mm)</label>
                  <input
                    type="number"
                    className="form-control"
                    name="maxModelWidth"
                    value={printer.maxModelWidth}
                    onChange={handleInputChange}
                    required
                  />
                </div>
              </div>
            </div>

            <div className="printer-form-section">
              <h5 className="mb-3">Materials</h5>
              {materialsLoading ? (
                <div className="text-center py-3">
                  <div className="spinner-border text-light" role="status">
                    <span className="visually-hidden">Loading materials...</span>
                  </div>
                </div>
              ) : (
                <div className="materials-grid">
                  {materials.map((material) => (
                    <label
                      key={material.printMaterialId}
                      className="material-checkbox"
                      htmlFor={`material-${material.printMaterialId}`}
                    >
                      <div className="form-check">
                        <input
                          type="checkbox"
                          className="form-check-input"
                          id={`material-${material.printMaterialId}`}
                          checked={selectedMaterials.includes(material.printMaterialId)}
                          onChange={() => handleMaterialChange(material.printMaterialId)}
                        />
                        <span className="form-check-label">
                          {material.name}
                        </span>
                      </div>
                    </label>
                  ))}
                </div>
              )}
            </div>

            <div className="printer-form-section">
              {!printer.locationX && !printer.locationY && (
                <div className="alert alert-warning mb-2">
                  Please select a location by clicking on the map or searching for an address
                </div>
              )}
              <div style={{ minHeight: "500px" }}>
                <MapSection
                  onLocationSelect={handleLocationSelect}
                  selectionMode={true}
                />
              </div>
            </div>

            <button
              type="submit"
              className="btn btn-primary btn-lg w-100"
              disabled={
                !isLoading ||
                !printer.locationX ||
                !printer.locationY ||
                printer.materials.length === 0 ||
                printer.maxModelHeight <= 0 ||
                printer.maxModelWidth <= 0
              }
            >
              {!isLoading ? (
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
              ) : (
                <i className="bi bi-check-circle me-2"></i>
              )}
              Submit Printer
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};
