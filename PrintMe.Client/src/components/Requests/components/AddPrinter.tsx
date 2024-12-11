import React, { useState } from "react";
import { requestsService } from "../../../services/requestsService";
import { useNavigate } from "react-router-dom";
import MapSection from "../../MainPage/components/MapSection";
import { handleApiError } from '../../../utils/apiErrorHandler';
import { PrinterApplicationDto } from "../../../services/requestsService";

const Materials = [
  { id: 1, name: "PLA" },
  { id: 2, name: "ABS" },
  { id: 3, name: "PETG" },
  { id: 4, name: "TPU" },
];

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

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setPrinter((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleLocationSelect = (location: { x: number; y: number }) => {
    setPrinter((prev) => ({
      ...prev,
      locationX: location.x,
      locationY: location.y,
    }));
  };

  const handleMaterialChange = (materialId: number) => {
    setSelectedMaterials((prev) => {
      const newMaterials = prev.includes(materialId)
        ? prev.filter((id) => id !== materialId)
        : [...prev, materialId];

      setPrinter((prevPrinter) => ({
        ...prevPrinter,
        materials: newMaterials.map((id) => ({ materialId: id })),
      }));

      return newMaterials;
    });
  };

  return (
    <div className="container mt-4">
      <h2>Add New Printer</h2>
      {error && <div className="alert alert-danger">{error}</div>}

      <form onSubmit={handleSubmit}>

        <div className="mb-3">
          <label className="form-label">Description</label>
          <input
            type="text"
            className="form-control"
            name="description"
            value={printer.description}
            onChange={handleInputChange}
          />
        </div>

        <div className="row">
          <div className="col-md-6 mb-3">
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
          <div className="col-md-6 mb-3">
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
        </div>

        <div className="row">
          <div className="col-md-6 mb-3">
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
          <div className="col-md-6 mb-3">
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

        {(printer.maxModelHeight <= 0 || printer.maxModelWidth <= 0) && (
          <div className="alert alert-warning mb-2">
            Maximum height and width must be greater than 0
          </div>
        )}

        <div className="mb-3">
          <label className="form-label">Materials</label>
          {selectedMaterials.length === 0 && (
            <div className="alert alert-warning">
              Select materials your printer can work with
            </div>
          )}
          <div className="d-flex flex-wrap gap-2">
            {Materials.map((material) => (
              <div key={material.id} className="form-check">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id={`material-${material.id}`}
                  checked={selectedMaterials.includes(material.id)}
                  onChange={() => handleMaterialChange(material.id)}
                />
                <label
                  className="form-check-label"
                  htmlFor={`material-${material.id}`}
                >
                  {material.name}
                </label>
              </div>
            ))}
          </div>
        </div>

        <div className="mb-3">
          {!printer.locationX && !printer.locationY && (
            <div className="alert alert-warning mb-2">
              You should select location by clicking on the map or searching for
              an address
            </div>
          )}
          <div className="border rounded" style={{ minHeight: "500px" }}>
            <MapSection
              onLocationSelect={handleLocationSelect}
              selectionMode={true}
            />
          </div>
        </div>

        <button
          type="submit"
          className="btn btn-primary mb-5"
          disabled={
            !isLoading ||
            !printer.locationX ||
            !printer.locationY ||
            printer.materials.length === 0 ||
            printer.maxModelHeight <= 0 ||
            printer.maxModelWidth <= 0
          }
        >
          {!isLoading ? "Submitting..." : "Submit Printer"}
        </button>
      </form>
    </div>
  );
};
