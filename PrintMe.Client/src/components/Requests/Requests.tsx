import { useEffect, useState } from 'react';
import { requestsService } from '../../services/requestsService';
import { roleService } from '../../services/roleService';
import { RequestDto, RequestType } from '../../types/requests';
import { handleApiError } from '../../utils/apiErrorHandler';
import { toast } from 'react-toastify';
import "./assets/requests.css";

const statusOptions = [
  { id: 1, name: "Pending" },
  { id: 2, name: "Approved" },
  { id: 3, name: "Rejected" },
  { id: 4, name: "Started" },
  { id: 5, name: "Aborted" },
  { id: 6, name: "Archived" },
];

const requestTypeOptions = [
  { id: 1, name: "Printer Application" },
  { id: 2, name: "Printer Description Changing" },
  { id: 3, name: "User Report" },
  { id: 4, name: "Account Deletion" },
];

interface PopupProps {
  requestId: number;
  selectedReason: string;
  onReasonChange: (requestId: number, reason: string) => void;
  onSubmit: () => void;
  onClose: () => void;
}

const Popup: React.FC<PopupProps> = ({ requestId, selectedReason, onReasonChange, onSubmit, onClose }) => {
  return (
    <div className="popup">
      <div className="popup-content">
      <h5 style={{ color: "black" }}>Select reason for decline</h5>
        <select
          className="admin-select form-select mt-2"
          value={selectedReason}
          onChange={(e) => onReasonChange(requestId, e.target.value)}
        >
          <option value="">Select reason for decline</option>
          <option value="Inappropriate">Inappropriate</option>
          <option value="OffensiveContent">Offensive Content</option>
          <option value="SystemAbuse">System Abuse</option>
        </select>
        <div className='admin-btn-group'>
          <button className="btn admin-btn admin-btn-danger mt-2" onClick={onSubmit}>Submit</button>
          <button className="btn admin-btn admin-btn-primary mt-2" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  );
};

const Requests: React.FC = () => {
  const [requests, setRequests] = useState<RequestDto[]>([]);
  const [error, setError] = useState<string>('');
  const [isLoading, setIsLoading] = useState(true);
  const [selectedReasons, setSelectedReasons] = useState<{ [key: number]: string }>({});
  const [isAdmin, setIsAdmin] = useState<boolean>(false);
  const [showDeclinePopup, setShowDeclinePopup] = useState<{ [key: number]: boolean }>({});
  const [selectedStatusFilters, setSelectedStatusFilters] = useState<number[]>([]);
  const [selectedRequestTypeFilters, setSelectedRequestTypeFilters] = useState<number[]>([]);

  useEffect(() => {
    const fetchRequests = async () => {
      setIsLoading(true);
      try {
        const roleData = await roleService.getMyRole();
        const userRole = roleData.userRole;
        setIsAdmin(userRole === 'Admin');
        const requestsData = await requestsService.getAllRequests();
        setRequests(requestsData);
      } catch (error: unknown) {
        if ((error as { response?: { status?: number } }).response?.status === 404) {
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
      setShowDeclinePopup(prev => ({ ...prev, [request.requestId]: false }));
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

  const getRequestTypeDisplay = (requestTypeId: number) => {
    switch (requestTypeId) {
      case 1:
        return "Printer Application";
      case 2:
        return "Printer Description Changing";
      case 3:
        return "User Report";
      case 4:
        return "Account Deletion";
      default:
        return "Unknown";
    }
  };

  const handleDeclineClick = (requestId: number) => {
    setShowDeclinePopup(prev => ({ ...prev, [requestId]: true }));
  };

  const handleClosePopup = (requestId: number) => {
    setShowDeclinePopup(prev => ({ ...prev, [requestId]: false }));
  };

  const handleStatusFilterChange = (statusId: number) => {
    setSelectedStatusFilters(prevFilters =>
      prevFilters.includes(statusId)
        ? prevFilters.filter(id => id !== statusId)
        : [...prevFilters, statusId]
    );
  };

  const handleRequestTypeFilterChange = (requestTypeId: number) => {
    setSelectedRequestTypeFilters(prevFilters =>
      prevFilters.includes(requestTypeId)
        ? prevFilters.filter(id => id !== requestTypeId)
        : [...prevFilters, requestTypeId]
    );
  };

  const resetFilters = () => {
    setSelectedStatusFilters([]);
    setSelectedRequestTypeFilters([]);
  };

  const filteredRequests = requests.filter(request =>
    (selectedStatusFilters.length === 0 || selectedStatusFilters.includes(request.requestStatusId)) &&
    (selectedRequestTypeFilters.length === 0 || selectedRequestTypeFilters.includes(request.requestTypeId))
  );

  return (
    <div className="requests-container">
      <div className="requests-content">
        <div className="main-content">
          {!isLoading && (
            <div className="requests-list-container">
              <h2 className="text-white mb-4">{isAdmin ? "Requests from users" : "Your Requests"}</h2>
              {filteredRequests.length > 0 ? (
                <div className="requests-list">
                  {filteredRequests.map((request) => (
                    <div key={request.requestId} className="request-item">
                      <div className="d-flex justify-content-between align-items-center mb-2">
                        <h4>
                          {request.requestTypeId === RequestType.PrinterApplication ? "Printer Application" : "Request"} #{request.requestId}
                        </h4>
                        {getStatusDisplay(request.requestStatusId)}
                      </div>
                      <div className="request-details">
                        <p className="mb-1 text-white"><strong>Description:</strong> {request.description}</p>
                        <p className="mb-1 text-white"><strong>Type:</strong> {getRequestTypeDisplay(request.requestTypeId)}</p>
                      </div>
                      {isAdmin && request.requestStatusId === 1 && (
                        <>
                          <div className="admin-btn-group">
                            <button className="btn admin-btn admin-btn-primary" onClick={() => onAccept(request)}>Accept</button>
                            <button className="btn admin-btn admin-btn-danger" onClick={() => handleDeclineClick(request.requestId)}>Decline</button>
                          </div>
                        </>
                      )}
                    </div>
                  ))}
                </div>
              ) : (
                <div className="alert alert-dark">
                  {isAdmin ? "No requests found" : "You don't have any requests yet."}
                </div>
              )}
            </div>
          )}

          {error && <div className="alert alert-danger mt-4">{error}</div>}
        </div>
        <div className="filter-container">
          <label htmlFor="status-filter" className="filter-label">Filter by Status</label>
          <div className="btn-group">
            {statusOptions.map(option => (
              <div key={option.id} className="form-check">
                <input
                  className="form-check-input"
                  type="checkbox"
                  id={`status-${option.id}`}
                  checked={selectedStatusFilters.includes(option.id)}
                  onChange={() => handleStatusFilterChange(option.id)}
                />
                <label className="form-check-label" htmlFor={`status-${option.id}`}>
                  {option.name}
                </label>
              </div>
            ))}
          </div>
          <label htmlFor="request-type-filter" className="filter-label">Filter by Request Type</label>
          <div className="btn-group">
            {requestTypeOptions.map(option => (
              <div key={option.id} className="form-check">
                <input
                  className="form-check-input"
                  type="checkbox"
                  id={`request-type-${option.id}`}
                  checked={selectedRequestTypeFilters.includes(option.id)}
                  onChange={() => handleRequestTypeFilterChange(option.id)}
                />
                <label className="form-check-label" htmlFor={`request-type-${option.id}`}>
                  {option.name}
                </label>
              </div>
            ))}
          </div>
          <button className="btn btn-secondary mt-2" onClick={resetFilters}>Reset Filters</button>
        </div>
      </div>
      {Object.keys(showDeclinePopup).map(requestId => (
        showDeclinePopup[Number(requestId)] && (
          <Popup
            key={requestId}
            requestId={Number(requestId)}
            selectedReason={selectedReasons[Number(requestId)] || ''}
            onReasonChange={handleReasonChange}
            onSubmit={() => onDecline(requests.find(request => request.requestId === Number(requestId))!)}
            onClose={() => handleClosePopup(Number(requestId))}
          />
        )
      ))}
    </div>
  );
};

export default Requests;