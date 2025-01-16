import { useEffect, useState } from "react";
import { handleApiError } from "../../utils/apiErrorHandler";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/orders.css";
import { PrintOrderDto } from "../../types/api";
import { ordersService } from "../../services/ordersService";
import { useNavigate } from "react-router-dom";


const Orders: React.FC = () => {
  const [orders, setOrders] = useState<PrintOrderDto[]>([]);
  const [error, setError] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);
  const [userNames, setUserNames] = useState<Record<number, { firstName: string; lastName: string }>>({});
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrders = async () => {
      setIsLoading(true);
      try {
        const data = await ordersService.getMyOrders();
        setOrders(data);

        const userIds = Array.from(new Set(data.map((order) => order.userId)));

        const userPromises = userIds.map((userId) =>
          ordersService.getUserById(userId).then((userData) => ({
            userId,
            userData,
          }))
        );

        const users = await Promise.all(userPromises);

        const userMap = users.reduce(
          (acc, { userId, userData }) => ({
            ...acc,
            [userId]: userData,
          }),
          {}
        );

        setUserNames(userMap);
      } catch (error: unknown) {
        const errorMessage = handleApiError(error);
        setError(errorMessage);
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrders();
  }, []);


  const getStatusDisplay = (statusId: number) => {
    switch (statusId) {
      case 1:
        return <span className="badge bg-warning">Pending</span>;
      case 2:
        return <span className="badge bg-danger">Declined</span>;
      case 3:
        return <span className="badge bg-success">Started</span>;
      case 4:
        return <span className="badge bg-danger">Aborted</span>;
      case 5:
        return <span className="badge bg-secondary">Archived</span>;
      default:
        return <span className="badge bg-secondary">Unknown</span>;
    }
  };

  const handleOrderClick = (printOrderId: number) => {
    navigate(`/orders/${printOrderId}`);
  };

  return (
    <div className="orders-container">
      <div className="orders-content container">
        {/* Header */}
        <div className="orders-header row">
          <div className="header-column col">Order ID</div>
          <div className="header-column col">Buyer</div>
          <div className="header-column col">Creation date</div>
          <div className="header-column col">Deadline</div>
          <div className="header-column col">Price</div>
          <div className="header-column col">Status</div>
          <div className="header-column col">Chat</div>
        </div>

        {/* Orders List */}
        {!isLoading && orders.length > 0 ? (
          orders.map((order) => (
            <div
              className="order row"
              key={order.printOrderId}
              onClick={() => handleOrderClick(order.printOrderId)}
              style={{ cursor: "pointer" }}
            >
              <div className="header-column col">{order.printOrderId}</div>
              <div className="header-column col">
                {userNames[order.userId]
                  ? `${userNames[order.userId].firstName} ${userNames[order.userId].lastName}`
                  : "Loading..."}
              </div>
              <div className="header-column col">{order.startDate}</div>
              <div className="header-column col">{order.dueDate}</div>
              <div className="header-column col">{order.price}$</div>
              <div className="header-column col">
                {getStatusDisplay(order.printOrderStatusId)}
              </div>
              <div className="header-column col">
                <a href="#" className="text-white header-icon">
                  <i className="bi bi-chat-dots-fill fs-2"></i>
                </a>
              </div>
            </div>
          ))
        ) : isLoading ? (
          <div className="text-center">Loading...</div>
        ) : (
          <div className="text-center">No orders found.</div>
        )}
      </div>
    </div>
  );
};

export default Orders;
