import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/orders.css";
import { PrintOrderDto } from "../../types/api";
import { ordersService } from "../../services/ordersService";
import { userService } from "../../services/userService";
import { useNavigate } from "react-router-dom";
import { getStatusDisplay } from "../../utils/orderUtils";

const Orders: React.FC = () => {
  const [orders, setOrders] = useState<PrintOrderDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [userNames, setUserNames] = useState<Record<number, { firstName: string; lastName: string }>>({});
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrders = async () => {
      setIsLoading(true);
      try {
        const data = await ordersService.getMyOrders();
        setOrders(data);

        const userIds = Array.from(new Set(data.map((order) => order.executorId)));

        const userPromises = userIds.map((executorId) =>
          userService.getUserFullNameById(executorId).then((userData) => ({
            executorId,
            userData,
          }))
        );

        const users = await Promise.all(userPromises);

        const userMap = users.reduce(
          (acc, { executorId, userData }) => ({
            ...acc,
            [executorId]: userData,
          }),
          {}
        );

        setUserNames(userMap);
      } catch (error: unknown) {
        console.error('Error fetching orders:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrders();
  }, []);

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
                {userNames[order.executorId]
                  ? `${userNames[order.executorId].firstName} ${userNames[order.executorId].lastName}`
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
