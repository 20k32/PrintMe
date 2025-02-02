import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { PrinterDto } from "../../../types/api";
import { printersService } from "../../../services/printersService";
import { handleApiError } from "../../../utils/apiErrorHandler";
import { toast } from "react-toastify";
import MapSection from "../../MainPage/components/MapSection";
import "./../assets/printerDetail.css";

const PrinterDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [printer, setPrinter] = useState<PrinterDto | null>(null);
  const [error, setError] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchPrinter = async () => {
      setIsLoading(true);
      try {
        const data = await printersService.getPrinterById(Number(id));
        setPrinter(data);
      } catch (error) {
        setError(handleApiError(error));
        toast.error("Failed to fetch printer details");
      } finally {
        setIsLoading(false);
      }
    };

    fetchPrinter();
  }, [id]);

  const handleBackClick = () => {
    navigate("/printers");
  };

  if (isLoading)
    return (
      <div className="printerd-container py-5">
        <div className="text-center mt-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
    );

  if (error)
    return (
      <div className="printerd-container py-5">
        <div className="text-center text-white mt-5">
          <h2>{error}</h2>
        </div>
      </div>
    );

  if (!printer)
    return (
      <div className="printerd-container py-5">
        <div className="text-center text-white mt-5">
          <h2>Printer not found</h2>
        </div>
      </div>
    );

  return (
    <div className="printerd-container py-5">
      <div className="card shadow-lg p-4">
        <div className="card-header text-white d-flex align-items-center justify-content-between">
          <h4>Printer #{printer.id}</h4>
          <a href="#" onClick={handleBackClick} className="text-white header-icon">
            <i className="bi bi-arrow-bar-left fs-2"></i>
          </a>
        </div>
        <div className="card-body">
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Model Name:</h5>
              <p>{printer.modelName}</p>
            </div>
            <div className="col-md-6">
              <h5>Description:</h5>
              <p>{printer.description}</p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Min Dimensions:</h5>
              <p>
                Height: {printer.minModelHeight}, Width: {printer.minModelWidth}
              </p>
            </div>
            <div className="col-md-6">
              <h5>Max Dimensions:</h5>
              <p>
                Height: {printer.maxModelHeight}, Width: {printer.maxModelWidth}
              </p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Location:</h5>
              <MapSection selectionMode={true} singleMarkerLocation={{ lat: printer.locationY, lng: printer.locationX }}/>
            </div>
            <div className="col-md-6">
              <h5>Is Deactivated:</h5>
              <p>{printer.isDeactivated ? "Yes" : "No"}</p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-12">
              <h5>Materials:</h5>
              <ul>
                {printer.materials.map((material) => (
                  <li key={material.printMaterialId}>{material.name}</li>
                ))}
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PrinterDetail;
