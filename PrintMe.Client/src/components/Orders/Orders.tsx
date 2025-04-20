import type React from "react"
import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/orders.css";
import { PrintOrderDto } from "../../types/api";
import { ordersService } from "../../services/ordersService";
import { userService } from "../../services/userService";
import { useNavigate } from "react-router-dom";
import { getStatusDisplay } from "../../utils/orderUtils";

type OrderType = "all" | "my" | "executor"

const statusOptions = [
  { id: 1, name: "Pending" },
  { id: 2, name: "Declined" },
  { id: 3, name: "Started" },
  { id: 4, name: "Aborted" },
  { id: 5, name: "Archived" },
]
const Orders: React.FC = () => {
  const [myOrders, setMyOrders] = useState<PrintOrderDto[]>([])
  const [executorOrders, setExecutorOrders] = useState<PrintOrderDto[]>([])
  const [isLoading, setIsLoading] = useState(true);
  const [userNames, setUserNames] = useState<Record<number, { firstName: string; lastName: string }>>({});
  const [filterStatus, setFilterStatus] = useState<number | "all">("all")
  const [filterOrderType, setFilterOrderType] = useState<OrderType>("all")
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrders = async () => {
      setIsLoading(true);
      try {
        const [myOrdersData, executorOrdersData] = await Promise.all([
          ordersService.getMyOrders(),
          ordersService.getOrdersAsExecutor(),
        ])
        setMyOrders(myOrdersData)
        setExecutorOrders(executorOrdersData)

        const allOrders = [...myOrdersData, ...executorOrdersData]
        const userIds = Array.from(new Set(
            [...allOrders.map((order) => order.userId), ...allOrders.map((order) => order.executorId)].filter(Boolean),
        ),);

        const userPromises = userIds.map((userId) =>
          userService.getUserFullNameById(userId).then((userData) => ({
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
        console.error('Error fetching orders:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrders();
  }, []);
  
  const filterOrders = (orders: PrintOrderDto[]) => {
    return orders.filter((order) => {
      return filterStatus === "all" || order.printOrderStatusId === filterStatus
    })
  }

  const getFilteredOrders = () => {
    const filteredMyOrders = filterOrders(myOrders)
    const filteredExecutorOrders = filterOrders(executorOrders)

    switch (filterOrderType) {
      case "my":
        return { myOrders: filteredMyOrders, executorOrders: [] }
      case "executor":
        return { myOrders: [], executorOrders: filteredExecutorOrders }
      default:
        return { myOrders: filteredMyOrders, executorOrders: filteredExecutorOrders }
    }
  }

  const { myOrders: filteredMyOrders, executorOrders: filteredExecutorOrders } = getFilteredOrders()
  
  const handleOrderClick = (printOrderId: number) => {
    navigate(`/orders/${printOrderId}`);
  };

  const OrdersList: React.FC<{ orders: PrintOrderDto[]; title: string; isMyOrders: boolean }> = ({ orders, title, isMyOrders,}) => (
      <>
        <h2 className="text-white mb-3">{title}</h2>
        <div className="orders-header row">
          <div className="header-column col">Order ID</div>
          <div className="header-column col">{isMyOrders ? "Executor" : "Buyer"}</div>
          <div className="header-column col">Creation date</div>
          <div className="header-column col">Deadline</div>
          <div className="header-column col">Price</div>
          <div className="header-column col">Status</div>
          <div className="header-column col">Chat</div>
        </div>
        {orders.map((order) => (
            <div
                className="order row"
                key={order.printOrderId}
                onClick={() => handleOrderClick(order.printOrderId)}
                style={{cursor: "pointer"}}
            >
              <div className="header-column col">{order.printOrderId}</div>
              <div className="header-column col">
                {isMyOrders
                    ? userNames[order.executorId]
                        ? `${userNames[order.executorId].firstName} ${userNames[order.executorId].lastName}`
                        : "Loading..."
                    : userNames[order.userId]
                        ? `${userNames[order.userId].firstName} ${userNames[order.userId].lastName}`
                        : "Loading..."}
              </div>
              <div className="header-column col">{order.startDate}</div>
              <div className="header-column col">{order.dueDate}</div>
              <div className="header-column col">{order.price}$</div>
              <div className="header-column col">{getStatusDisplay(order.printOrderStatusId)}</div>
              <div className="header-column col">
                <a
                    href="#"
                    className="text-white header-icon"
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(`/chatPage/`, {state: {selectedUserId: isMyOrders ? order.executorId : order.userId}});}}
                >
                  <i className="bi bi-chat-dots-fill fs-2"></i>
                </a>
              </div>
            </div>
        ))}
      </>
  )

  const showOrderTypeFilter = executorOrders.length > 0

  return (
      <div className="orders-container">
        <div className="orders-content container">
          <div className="filters mb-4">
            <div className={`row ${!showOrderTypeFilter ? "justify-content-center" : ""}`}>
              <div className={`${showOrderTypeFilter ? "col-md-6" : "col-md-3"}`}>
                <label htmlFor="status-filter" className="filter-label">
                  Filter by Status
              </label>
              <select
                  id="status-filter"
                  className="filter-select"
                  value={filterStatus}
                  onChange={(e) => setFilterStatus(e.target.value === "all" ? "all" : Number(e.target.value))}
              >
                <option value="all">All Statuses</option>
                {statusOptions.map((status) => (
                    <option key={status.id} value={status.id}>
                      {status.name}
                    </option>
                ))}
              </select>
            </div>
            {showOrderTypeFilter && (
                <div className="col-md-6">
                  <label htmlFor="order-type-filter" className="filter-label">
                    Filter by Order Type
                  </label>
                  <select
                      id="order-type-filter"
                      className="filter-select"
                      value={filterOrderType}
                      onChange={(e) => setFilterOrderType(e.target.value as OrderType)}
                  >
                    <option value="all">All Orders</option>
                    <option value="my">Orders I've Made</option>
                    <option value="executor">Orders I'm Executing</option>
                  </select>
                </div>
            )}
          </div>
        </div>

        {/* Orders List */}
        {isLoading ? (
            <div className="text-center text-white">Loading...</div>
        ) : (
            <>
              {(filterOrderType === "all" || filterOrderType === "my") && filteredMyOrders.length > 0 && (
                  <OrdersList orders={filteredMyOrders} title="Orders I've Made" isMyOrders={true}/>
              )}
              {(filterOrderType === "all" || filterOrderType === "executor") && filteredExecutorOrders.length > 0 && (
                  <OrdersList orders={filteredExecutorOrders} title="Orders I'm Executing" isMyOrders={false}/>
              )}
              {filteredMyOrders.length === 0 && filteredExecutorOrders.length === 0 && (
                  <div className="text-center text-white">No orders found.</div>
              )}
            </>
        )}
      </div>
    </div>
  )
}

export default Orders;
