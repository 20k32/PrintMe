import { useCallback, useEffect, useState, useMemo } from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { printersService } from "../../services/printersService";
import { userService } from "../../services/userService";
import { PrinterDto } from "../../types/api";
import { handleApiError } from "../../utils/apiErrorHandler";
import { toast } from "react-toastify";
import "./assets/printers.css";

const Printers: React.FC = () => {
  const [printers, setPrinters] = useState<PrinterDto[]>([]);
  const [error, setError] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);
  const [showDeactivated, setShowDeactivated] = useState(false);
  const [isEmailVerified, setIsEmailVerified] = useState(false);

  const navigate = useNavigate();

  const fetchPrinters = useCallback(() => {
    setIsLoading(true);
    printersService
      .getMyPrinters()
      .then((data) => {
        setPrinters(data);
      })
      .catch((error) => {
        if (error.response?.status !== 404) {
          setError(handleApiError(error));
        }
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    fetchPrinters();
  }, [fetchPrinters]);

  useEffect(() => {
      userService
          .getIsUserEmailVerified()
          .then((isVerified) => setIsEmailVerified(isVerified))
          .catch((error) => {
            console.error("Failed to fetch email verification status:", error)
            setIsEmailVerified(false)
          })
  }, [])
  
  const filteredPrinters = useMemo(() => {
    return printers.filter((printer) =>
      showDeactivated ? true : !printer.isDeactivated
    );
  }, [printers, showDeactivated]);

  const handleDeactivate = async (printerId: number) => {
    if (!window.confirm("Are you sure you want to deactivate this printer?")) {
      return;
    }

    try {
      await printersService.deactivatePrinter(printerId);
      toast.success("Printer deactivated successfully");
      fetchPrinters();
    } catch (error) {
      setError(handleApiError(error));
      toast.error("Failed to deactivate printer");
    }
  };

  const handleActivate = async (printerId: number) => {
    if (!window.confirm('Are you sure you want to activate this printer?')) {
        return;
    }

    try {
        await printersService.activatePrinter(printerId);
        toast.success('Printer activated successfully');
        fetchPrinters();
    } catch (error) {
        setError(handleApiError(error));
        toast.error('Failed to activate printer');
    }
  };

  const renderSkeletonCards = () =>
    Array(3)
      .fill(null)
      .map((_, index) => (
        <div key={index} className="col-md-6 col-lg-4">
          <div className="printer-card card skeleton">
            <div className="card-body">
              <div className="skeleton-title"></div>
              <div className="skeleton-text"></div>
              <div className="skeleton-badges">
                <span></span>
                <span></span>
              </div>
            </div>
          </div>
        </div>
      ));

  return (
    <div className="requests-container">
      <div className="requests-content">
        <div className="printers-header">
          <div className="header-top">
            <h1 className="text-white">My Printers</h1>
            <div className="toggle-container">
              <label className="switch-label" htmlFor="showDeactivated">
                <div className="switch">
                  <input
                    type="checkbox"
                    id="showDeactivated"
                    checked={showDeactivated}
                    onChange={(e) => setShowDeactivated(e.target.checked)}
                  />
                  <span className="slider"></span>
                </div>
                <span className="switch-text">
                  {showDeactivated ? "Hide" : "Show"} deactivated printers
                </span>
              </label>
            </div>
          </div>
        </div>

        {isLoading ? (
          <div className="row g-4 printers-grid">{renderSkeletonCards()}</div>
        ) : error ? (
          <div className="alert alert-danger">
            <i className="bi bi-exclamation-triangle-fill me-2"></i>
            {error}
          </div>
        ) : filteredPrinters.length === 0 ? (
          <div className="empty-state">
            <h3>No printers found</h3>
            <p>
              {showDeactivated
                ? "You don't have any deactivated printers"
                : "Start by adding your first printer"}
            </p>
          </div>
        ) : (
          <div className="row g-4 printers-grid">
            {filteredPrinters.map((printer) => (
              <div key={printer.id}
              className="col-md-6 col-lg-4"
              onClick={() => navigate(`/printers/${printer.id}`)}
              >
                <div
                  className={`printer-card card ${
                    printer.isDeactivated ? "deactivated" : ""
                  }`}
                >
                  <div className="card-body">
                    <div className="status-badge">
                      {printer.isDeactivated ? (
                        <span className="badge bg-danger">Deactivated</span>
                      ) : (
                        <span className="badge bg-success">Active</span>
                      )}
                    </div>
                    <h5 className="card-title">{printer.modelName}</h5>
                    <p className="card-text">{printer.description}</p>
                    <div className="materials-badges">
                      {printer.materials.map((material) => (
                        <span
                          key={material.printMaterialId}
                          className="material-badge"
                        >
                          {material.name}
                        </span>
                      ))}
                    </div>
                    {printer.isDeactivated ? (
                        <button
                        className="btn btn-success activate-btn"
                        onClick={async (e) => {
                          e.stopPropagation();
                          await handleActivate(printer.id);
                        }}
                      >
                        Activate
                      </button>
                    ) : (
                      <button
                        className="btn btn-danger deactivate-btn"
                        onClick={async (e) => {
                          e.stopPropagation();
                          await handleDeactivate(printer.id);
                        }}
                      >
                        Deactivate
                      </button>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
        <div className="d-flex gap-4 justify-content-center mb-4 mt-5">
          {isEmailVerified === false ? (
            <div content="Please verify your email to add a printer">
              <div className="request-card disabled">
                <i className="bi bi-printer-fill mb-3 fs-1"></i>
                <h3>Add Printer</h3>
                <p>Verify your email to register your 3D printer</p>
              </div>
            </div>
          ) : (
            <Link to="/printers/add" className="request-card">
              <i className="bi bi-printer-fill mb-3 fs-1"></i>
              <h3>Add Printer</h3>
              <p>Register your 3D printer and start earning</p>
            </Link>
          )}
        </div>
      </div>
    </div>
  );
};

export default Printers;
