import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { ordersService } from "../../../services/ordersService";
import { userService } from "../../../services/userService";
import { PrintOrderDto } from "../../../types/api";
import { printersService } from "../../../services/printersService";
import { getStatusDisplay } from "../../../utils/orderUtils";
import "./../assets/orderDetails.css";

interface User {
  firstName: string;
  lastName: string;
}

interface Material {
  printMaterialId: number;
  name: string;
}

const OrderDetails: React.FC = () => {
  const { orderId } = useParams<{ orderId: string }>();
  const navigate = useNavigate();
  const [order, setOrder] = useState<PrintOrderDto | null>(null);
  const [user, setUser] = useState<User | null>(null);
  const [customer, setCustomer] = useState<User | null>(null);
  const [materials, setMaterials] = useState<Material[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isExecutorView, setIsExecutorView] = useState(false);

  useEffect(() => {
    const fetchOrderDetails = async () => {
      setIsLoading(true);
      try {
        let data = await ordersService.getMyOrders();
        let orderDetails = data.find(
          (o) => o.printOrderId === Number(orderId)
        );

        if (!orderDetails) {
          data = await ordersService.getOrdersAsExecutor();
          orderDetails = data.find(
            (o) => o.printOrderId === Number(orderId)
          );
          if (orderDetails) {
            setIsExecutorView(true);
          }
        }

        setOrder(orderDetails || null);

        if (orderDetails) {
          const executorData = await userService.getUserFullNameById(
            orderDetails.executorId
          );
          setUser(executorData);

          const customerData = await userService.getUserFullNameById(
            orderDetails.userId
          );
          setCustomer(customerData);
        }

        const materialsData = await printersService.getMaterials();
        setMaterials(materialsData);
      } catch (error) {
        console.error("Error fetching order details or materials", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrderDetails();
  }, [orderId]);

  const getMaterialDisplay = (materialId: number) => {
    const material = materials.find((m) => m.printMaterialId === materialId);
    return material ? material.name : "Unknown Material";
  };

  const handleBackClick = () => {
    navigate("/orders");
  };

  const handleEditClick = () => {
    if (!orderId) return;
    navigate(`/orders/${orderId}/edit`);
  };

  const handleAbortClick = async () => {
    if (!orderId) return;

    try {
      await ordersService.abortOrder(Number(orderId));
      const updatedOrder = await ordersService.getOrderById(Number(orderId));
      setOrder(updatedOrder as PrintOrderDto);
    } catch (error) {
      console.error("Error aborting order", error);
    }
  };

  const handleChatClick = (userId: number) => {
    navigate(`/profile/${userId}`);
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
          <div className="d-flex align-items-center">
            <h4 className="mb-0">Order #{order.printOrderId}</h4>
            {order.printOrderStatusId === 1 && (
              <div className="ms-3 d-flex gap-2">
                <button
                  className="btn btn-outline-secondary"
                  onClick={handleEditClick}
                >
                  <i className="bi bi-pencil me-2"></i>
                  Edit
                </button>
                <button
                  className="btn btn-outline-danger"
                  onClick={handleAbortClick}
                >
                  <i className="bi bi-x-circle me-2"></i>
                  Abort
                </button>
              </div>
            )}
          </div>
          <a
            href="#"
            onClick={handleBackClick}
            className="text-white header-icon"
          >
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
                {isExecutorView 
                  ? customer ? `${customer.firstName} ${customer.lastName}` : "Loading..."
                  : user ? `${user.firstName} ${user.lastName}` : "Loading..."
                }{" "}
                <a
                  href="#"
                  onClick={() => handleChatClick(isExecutorView ? order.userId : order.executorId)}
                  className="text-white header-icon"
                >
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