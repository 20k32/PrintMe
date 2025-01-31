import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { PrintOrderDto, UpdatePartialOrderRequest } from "../../../types/api";
import { ordersService } from "../../../services/ordersService";
import { printersService } from "../../../services/printersService";
import { PrintMaterial } from "../../../constants";
import "./../assets/orderDetails.css";

const EditOrder: React.FC = () => {
  const { orderId } = useParams<{ orderId: string }>();
  const navigate = useNavigate();
  const [order, setOrder] = useState<PrintOrderDto | null>(null);
  const [materials, setMaterials] = useState<PrintMaterial[]>([]);
  const [formData, setFormData] = useState<UpdatePartialOrderRequest>({
    orderId: Number(orderId),
    itemDescription: '',
    itemQuantity: 0,
    itemMaterialId: 0,
    itemLink: '',
    dueDate: new Date().toISOString().split('T')[0],
    price: 0
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch order details
        const response = await ordersService.getOrderById(Number(orderId));
        const data = response as PrintOrderDto;
        setOrder(data);
        
        // Fetch printer materials
        const printerData = await printersService.getPrinterMinimalInfo(data.printerId);
        setMaterials(printerData.materials);

        setFormData({
          orderId: data.printOrderId,
          itemDescription: data.itemDescription || '',
          itemQuantity: data.itemQuantity,
          itemMaterialId: data.itemMaterialId,
          itemLink: data.itemLink || '',
          dueDate: data.dueDate,
          price: data.price
        });
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [orderId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    let processedValue: string | number = value;
    
    // Convert numeric fields
    if (name === 'itemQuantity' || name === 'price' || name === 'itemMaterialId') {
      processedValue = Number(value);
    }

    setFormData(prev => ({
      ...prev,
      [name]: processedValue
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await ordersService.updateOrder(formData);
      navigate(`/orders/${orderId}`);
    } catch (error) {
      console.error("Error updating order:", error);
    }
  };

  if (!order) return <div>Loading...</div>;

  return (
    <div className="orderd-container py-5">
      <div className="card shadow-lg p-4">
        <div className="card-header text-white">
          <h4>Edit Order #{orderId}</h4>
        </div>
        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label className="form-label">Description</label>
              <textarea
                className="form-control"
                name="itemDescription"
                value={formData.itemDescription}
                onChange={handleChange}
                required
              />
            </div>

            <div className="mb-3">
              <label className="form-label">File Link</label>
              <input
                type="url"
                className="form-control"
                name="itemLink"
                value={formData.itemLink}
                onChange={handleChange}
                required
              />
            </div>

            <div className="row mb-3">
              <div className="col-md-6">
                <label className="form-label">Quantity</label>
                <input
                  type="number"
                  className="form-control"
                  name="itemQuantity"
                  value={formData.itemQuantity}
                  onChange={handleChange}
                  min="1"
                  required
                />
              </div>
              
              <div className="col-md-6">
                <label className="form-label">Material</label>
                <select
                  className="form-select"
                  name="itemMaterialId"
                  value={formData.itemMaterialId}
                  onChange={handleChange}
                  required
                >
                  <option value="">Select material</option>
                  {materials.map((material) => (
                    <option key={material.printMaterialId} value={material.printMaterialId}>
                      {material.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="row mb-3">
              <div className="col-md-6">
                <label className="form-label">Due Date</label>
                <input
                  type="date"
                  className="form-control"
                  name="dueDate"
                  value={formData.dueDate?.split('T')[0]}
                  onChange={handleChange}
                  min={new Date().toISOString().split('T')[0]}
                  required
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Price</label>
                <input
                  type="number"
                  className="form-control"
                  name="price"
                  value={formData.price}
                  onChange={handleChange}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
            </div>

            <div className="d-flex justify-content-between">
              <button type="button" className="btn btn-outline-danger" onClick={() => navigate(`/orders/${orderId}`)}>
                Cancel
              </button>
              <button type="submit" className="btn btn-outline-light">
                Save Changes
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditOrder;
