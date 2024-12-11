import { useEffect, useState } from 'react';
import { RequestDto, requestsService } from '../../services/requestsService';

const AdminRequests = () => {
    const [requests, setRequests] = useState<RequestDto[]>([]);
    const [error, setError] = useState<string>('');
    const [isLoading, setIsLoading] = useState(true);

    const updateRequestStatus = (requestDto: RequestDto, newStatus: string) => {
        setRequests(prevRequests =>
            prevRequests.map(request =>
                request.requestId === requestDto.requestId
                    ? { ...request, requestStatus: { ...request.requestStatus, status: newStatus } }
                    : request
            )
        );
    };


    const onAccept = async (request: RequestDto) => {
        try{
            // accept logic 
            const result = await requestsService.approveRequest(request.requestId);
            
            if(result.statusCode == 200) {
                updateRequestStatus(request, "Approved");
            }
            else{
                console.error(result.statusCode, result.message, result.value);
            }
        }catch (exception){
            console.error('There was an error fetching the requests!', exception);
        }
    };

    const onDecline = async (request: RequestDto) => {

        // decline logic
        const result = await requestsService.declineRequest(request.requestId);

        if(result.statusCode == 200) {
            updateRequestStatus(request, "Declined");
        }
        else{
            console.error(result.statusCode, result.message, result.value);
        }
        console.clear();
        console.log(`Request ${request} declined`);
        console.log(`Status: ${request.requestStatus.id}`);
        
    };


    useEffect(() => {
        setIsLoading(true);
        requestsService.getAllRequests()
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
            <h1 className="mb-4">Requests from users</h1>
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
                            {request.requestStatus.status === "Pending" && (
                                <>
                                    <button className="btn btn-primary"
                                            onClick={() => onAccept(request)}>Accept
                                    </button>
                                    <button className="btn btn-danger"
                                            onClick={() => onDecline(request)}>Decline
                                    </button>
                                </>
                            )}
                        </div>
                    ))}
                </div>
            ) : (
                <p>No requests found</p>
            )}
        </div>
    );
};

export default AdminRequests;