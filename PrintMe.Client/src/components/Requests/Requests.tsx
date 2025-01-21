import { useEffect, useState } from 'react';
import { requestsService } from '../../services/requestsService';
import { roleService } from '../../services/roleService';
import { RequestDto, RequestType } from '../../types/requests';
import { handleApiError } from '../../utils/apiErrorHandler';
import { toast } from 'react-toastify';
import "./assets/requests.css";

const Requests: React.FC = () => {
    const [requests, setRequests] = useState<RequestDto[]>([]);
    const [error, setError] = useState<string>('');
    const [isLoading, setIsLoading] = useState(true);
    const [selectedReasons, setSelectedReasons] = useState<{ [key: number]: string }>({});
    const [isAdmin, setIsAdmin] = useState<boolean>(false);

    useEffect(() => {
        const fetchRequests = async () => {
            setIsLoading(true);
            try {
                const roleData = await roleService.getMyRole();
                console.log(roleData);
                const userRole = roleData.userRole;
                setIsAdmin(userRole === 'Admin');
                const requestsData = userRole === 'Admin' ? await requestsService.getAllRequests() : await requestsService.getMyRequests();
                setRequests(requestsData);
            } catch (error: any) {
                if (error.response?.status === 404) {
                    setError('No requests found.');
                } else {
                    console.error('There was an error fetching the requests or roles!', error);
                    setError(handleApiError(error));
                }
            } finally {
                setIsLoading(false);
            }
        };

        fetchRequests();
    }, []);

    const updateRequestStatus = (requestDto: RequestDto, newStatus: number) => {
        setRequests(prevRequests =>
            prevRequests.map(request =>
                request.requestId === requestDto.requestId
                    ? { ...request, requestStatusId: newStatus }
                    : request
            )
        );
    };

    const onAccept = async (request: RequestDto) => {
        try {
            await requestsService.approveRequest(request.requestId);
            updateRequestStatus(request, 2); // Assuming 2 is the status code for "Approved"
        } catch (exception) {
            console.error('There was an error accepting the request!', exception);
        }
    };

    const onDecline = async (request: RequestDto) => {
        const selectedReason = selectedReasons[request.requestId];
        if (!selectedReason) {
            toast.warning("Please select a reason for declining the request.");
            return;
        }
        try {
            await requestsService.declineRequest(request.requestId, selectedReason);
            updateRequestStatus(request, 3); // Assuming 3 is the status code for "Declined"
        } catch (exception) {
            console.error('There was an error declining the request!', exception);
        }
    };

    const handleReasonChange = (requestId: number, reason: string) => {
        setSelectedReasons(prevReasons => ({
            ...prevReasons,
            [requestId]: reason
        }));
    };

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

    if (isAdmin) {
        return (
            <div className="requests-container">
                <div className="container mt-4">
                    {isLoading ? (
                        <div>Loading...</div>
                    ) : requests && requests.length > 0 ? (
                        <div className="list-group">
                            <h1 className="mb-4 text-white">Requests from users</h1>
                            {requests.map(request => (
                                <div key={request.requestId} className="list-group-item">
                                    <h5>Request #{request.requestId}</h5>
                                    {request.description && (
                                        <p className="mb-1"><strong>Description:</strong> {request.description}</p>
                                    )}
                                    {request.userTextData && (
                                        <p className="mb-1"><strong>Additional Info:</strong> {request.userTextData}</p>
                                    )}
                                    <p className="mb-1"><strong>Status:</strong> {getStatusDisplay(request.requestStatusId)}</p>
                                    <p className="mb-1"><strong>Type:</strong> {request.requestTypeId}</p>
                                    {request.requestStatusId === 1 && (
                                        <>
                                        <div className='btn-group'>
                                            <button className="btn btn-primary" onClick={() => onAccept(request)}>Accept</button>
                                            <button className="btn btn-danger" onClick={() => onDecline(request)}>Decline</button>
                                        </div>
                                            <select
                                                className="form-select mt-2"
                                                value={selectedReasons[request.requestId] || ''}
                                                onChange={(e) => handleReasonChange(request.requestId, e.target.value)}
                                            >
                                                <option value="">Select reason for decline</option>
                                                <option value="Inappropriate">Inappropriate</option>
                                                <option value="OffensiveContent">Offensive Content</option>
                                                <option value="SystemAbuse">System Abuse</option>
                                            </select>
                                        </>
                                    )}
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p>No requests found</p>
                    )}
                </div>
            </div>
        );
    }

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