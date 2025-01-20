import { useEffect, useState } from 'react';
import { requestsService } from '../../services/requestsService';
import { RequestDto, RequestType } from '../../types/requests';
import { handleApiError } from '../../utils/apiErrorHandler';
import "./assets/requests.css";

const Requests: React.FC = () => {
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
                return <span className="badge bg-warning">Pending</span>;
            case 2:
                return <span className="badge bg-success">Approved</span>;
            case 3:
                return <span className="badge bg-danger">Rejected</span>;
            default:
                return <span className="badge bg-secondary">Unknown</span>;
        }
    };

    return (
        <div className="requests-container">
            <div className="requests-content">

                {!isLoading && (
                    <div className="requests-list-container">
                        <h2 className="text-white mb-4">Your Requests</h2>
                        {requests.length > 0 ? (
                            <div className="requests-list">
                                {requests.map((request) => (
                                    <div key={request.requestId} className="request-item">
                                        <div className="d-flex justify-content-between align-items-center mb-2">
                                            <h4>{request.requestTypeId === RequestType.PrinterApplication ? "Printer Application" : "Request"}</h4>
                                            {getStatusDisplay(request.requestStatusId)}
                                        </div>
                                        <div className="request-details">
                                            {request.description && (
                                                <p className="request-description mb-0">
                                                    {request.description}
                                                </p>
                                            )}
                                        </div>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <div className="alert alert-dark">
                                You don't have any requests yet.
                            </div>
                        )}
                    </div>
                )}

                {error && <div className="alert alert-danger mt-4">{error}</div>}
            </div>
        </div>
    );
};

export default Requests;
