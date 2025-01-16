import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { ordersService } from "../../../services/ordersService";
import { PrintOrderDto } from "../../../types/api";
import "./../assets/orderDetails.css";

interface User {
  firstName: string;
  lastName: string;
}

const OrderDetails: React.FC = () => {
  const { orderId } = useParams<{ orderId: string }>();
  const navigate = useNavigate();
  const [order, setOrder] = useState<PrintOrderDto | null>(null);
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchOrderDetails = async () => {
      setIsLoading(true);
      try {
        const data = await ordersService.getMyOrders();
        const orderDetails = data.find((o) => o.printOrderId === Number(orderId));
        setOrder(orderDetails || null);

        if (orderDetails) {
          const userData = await ordersService.getUserById(orderDetails.userId);
          setUser(userData);
        }
      } catch (error) {
        console.error("Error fetching order details", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrderDetails();
  }, [orderId]);

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

  const getMaterialDisplay = (materialId: number) => {
    switch (materialId) {
      case 1:
        return "Gleason - Rutherford";
      case 2:
        return "Hansen - Beatty";
      case 3:
        return "Ritchie - Maggio";
      case 4:
        return "Thompson LLC";
      default:
        return "Unknown Material";
    }
  };

  const handleBackClick = () => {
    navigate("/orders");
  };

  if (isLoading)
    return (
      <div className="orderd-container py-5">
        <div className="text-center mt-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
    );

  if (!order)
    return (
      <div className="orderd-container py-5">
        <div className="text-center text-white mt-5">
          <h2>Order not found</h2>
        </div>
      </div>
    );

  return (
    <div className="orderd-container py-5">
      <div className="card shadow-lg p-4">
        <div className="card-header text-white d-flex align-items-center justify-content-between">
          <h4>Order #{order.printOrderId}</h4>
          <a href="#" onClick={handleBackClick} className="text-white header-icon">
            <i className="bi bi-arrow-bar-left fs-2"></i>
          </a>
        </div>
        <div className="card-body">
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Status:</h5>
              <p>{getStatusDisplay(order.printOrderStatusId)}</p>
            </div>
            <div className="col-md-6">
              <h5>Order Description:</h5>
              <p>{order.itemDescription}</p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Dates:</h5>
              <p>
                {order.startDate} - {order.dueDate}
              </p>
            </div>
            <div className="col-md-6">
              <h5>Price:</h5>
              <p>${order.price}</p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Material:</h5>
              <p>{getMaterialDisplay(order.itemMaterialId)}</p>
            </div>
            <div className="col-md-6">
              <h5>Quantity:</h5>
              <p>{order.itemQuantity}</p>
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-md-6">
              <h5>Files:</h5>
              {order.itemLink}
            </div>
            <div className="col-md-6">
              <h5>Chat with:</h5>
              <p>
                {user ? `${user.firstName} ${user.lastName}` : "Loading..."}{" "}
                <a href="#" className="text-white header-icon">
                  <i className="bi bi-chat-dots-fill"></i>
                </a>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderDetails;
