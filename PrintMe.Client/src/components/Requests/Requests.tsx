import { useEffect, useState } from 'react';
import { RequestDto, requestsService } from '../../services/requestsService';

const Requests = () => {
    const [requests, setRequests] = useState<RequestDto[]>([]);
    const [error, setError] = useState<string>('');
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        setIsLoading(true);
        requestsService.getMyRequests()
            .then(data => {
                setRequests(data);
                setIsLoading(false);
            })
            .catch(error => {
                console.error('There was an error fetching the requests!', error);
                setError(error.message || 'Failed to load requests');
                setIsLoading(false);
            });
    }, []);

    if (error) {
        return <div className="alert alert-danger">{error}</div>;
    }

    return (
        <div className="container mt-4">
            <h1 className="mb-4">My Requests</h1>
            {isLoading ? (
                <div>Loading...</div>
            ) : requests && requests.length > 0 ? (
                <div className="list-group">
                    {requests.map(request => (
                        <div key={request.requestId} className="list-group-item">
                            <h5>Request #{request.requestId}</h5>
                            {request.description && (
                                <p className="mb-1"><strong>Description:</strong> {request.description}</p>
                            )}
                            {request.userTextData && (
                                <p className="mb-1"><strong>Additional Info:</strong> {request.userTextData}</p>
                            )}
                            <p className="mb-1"><strong>Status:</strong> {request.requestStatus.status}</p>
                            <p className="mb-1"><strong>Type:</strong> {request.requestType.type}</p>
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
