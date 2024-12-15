import { useEffect, useState } from 'react';
import { RequestDto, RequestType, requestsService } from '../../services/requestsService';
import { useNavigate } from 'react-router-dom';
import { handleApiError } from '../../utils/apiErrorHandler';

const Requests = () => {
    const navigate = useNavigate();
    const [requests, setRequests] = useState<RequestDto[]>([]);
    const [error, setError] = useState<string>('');
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        setIsLoading(true);
        requestsService.getMyRequests()
            .then((data) => {
                setRequests(data);
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

    const getStatusDisplay = (statusId: number) => {
        switch (statusId) {
            case 1:
                return <span className="text-warning">Pending</span>;
            case 2:
                return <span className="text-success">Approved</span>;
            case 3:
                return <span className="text-danger">Rejected</span>;
            default:
                return <span className="text-muted">Unknown</span>;
        }
    };

    const getTypeDisplay = (typeId: number) => {
        switch (typeId) {
            case 2:
                return "Printer Application";
            default:
                return "Unknown";
        }
    }

    const handleRequestTypeSelect = (type: RequestType) => {
        switch (type) {
            case RequestType.PrinterApplication:
                navigate('/requests/printer');
                break;
        }
    };

    if (error) {
      return <div className="alert alert-danger">{error}</div>;
    }

    return (
      <div className="container mt-4">
        <h1 className="mb-4">My Requests</h1>
        <button
          className="btn btn-primary mb-3"
          onClick={() => handleRequestTypeSelect(RequestType.PrinterApplication)}
        >
          Add printer
        </button>

        {isLoading ? (
          <div>Loading...</div>
        ) : requests && requests.length > 0 ? (
          <div className="list-group">
            {requests.map((request) => (
              <div key={request.requestId} className="list-group-item">
                <h5>Request #{request.requestId}</h5>
                {request.description && (
                  <p className="mb-1">
                    <strong>Description:</strong> {request.description}
                  </p>
                )}
                <p className="mb-1">
                  <strong>Status:</strong>{" "}
                  {getStatusDisplay(request.requestStatusId)}
                </p>
                <p className="mb-1">
                  <strong>Type:</strong> {getTypeDisplay(request.requestTypeId)}
                </p>
              </div>
            ))}
          </div>
        ) : (
          <p>No requests found</p>
        )}
      </div>
    );
};

export default Requests;
