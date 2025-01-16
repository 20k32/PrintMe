import React, { useState } from 'react';
import { SimplePrinterDto } from '../../../types/api';
import { orderService } from '../../../services/orderService';
import { toast } from 'react-toastify';

interface OrderModalProps {
  printer: SimplePrinterDto;
  onClose: () => void;
  onSubmit: (orderData: {
    orderName: string;
    description: string;
    contact: string;
    price: string;
    file: File | null;
    printerName: string;
    dueDate: string;
    quantity: number;
    selectedMaterial: number;
  }) => void;
}

const OrderModal: React.FC<OrderModalProps> = ({ printer, onClose }) => {
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const formatUrl = (url: string): string => {
    if (!url) return url;
    return url.startsWith('http://') || url.startsWith('https://')
      ? url
      : `https://${url}`;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      const formData = new FormData(e.target as HTMLFormElement);
      const orderData = {
        printerId: printer.id,
        price: Number(formData.get('price')),
        itemLink: formatUrl(String(formData.get('fileName'))),
        dueDate: String(formData.get('dueDate')),
        itemQuantity: Number(formData.get('quantity')),
        itemDescription: String(formData.get('description')),
        itemMaterialId: Number(formData.get('selectedMaterial')),
      };

      await orderService.createOrder(orderData);
      toast.success('Order created successfully!');
      onClose();
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create order';
      setError(errorMessage);
      setError(errorMessage);
      toast.error(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div
      className="modal d-block"
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        width: "100vw",
        height: "100vh",
        backgroundColor: "rgba(0, 0, 0, 0.5)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        zIndex: 1050,
        padding: "1rem",
      }}
    >
      <div
        className="modal-dialog"
        style={{
          maxWidth: "600px",
          width: "100%",
          position: "absolute",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
        }}
      >
        <div
          className="modal-content"
          style={{
            backgroundColor: "#554877",
            color: "#ffffff",
            borderRadius: "15px",
          }}
        >
          <div className="modal-header border-0">
            <h3 className="modal-title w-100 text-center">
              Create Order for {printer.modelName}
            </h3>
            <button
              type="button"
              className="btn-close"
              onClick={onClose}
              style={{
                color: "#ffffff",
                opacity: 1,
                fontSize: "1.5rem",
                border: "none",
                background: "none",
              }}
            >
              &times;
            </button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body">
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}
              <div className="d-flex flex-column gap-3">
                <div>
                  <label htmlFor="description" className="form-label">
                    Description:
                  </label>
                  <textarea
                    id="description"
                    name="description"
                    className="form-control"
                    rows={3}
                    required
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  />
                </div>

                <div>
                  <label htmlFor="dueDate" className="form-label">
                    Due Date:
                  </label>
                  <input
                    type="date"
                    id="dueDate"
                    name="dueDate"
                    className="form-control"
                    required
                    min={new Date().toISOString().split("T")[0]}
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  />
                </div>

                <div>
                  <label htmlFor="quantity" className="form-label">
                    Quantity:
                  </label>
                  <input
                    type="number"
                    id="quantity"
                    name="quantity"
                    className="form-control"
                    required
                    min="1"
                    defaultValue="1"
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  />
                </div>

                <div>
                  <label htmlFor="material" className="form-label">
                    Material:
                  </label>
                  <select
                    id="material"
                    name="selectedMaterial"
                    className="form-control"
                    required
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  >
                    <option value="">Select material</option>
                    {printer.materials.map((material) => (
                      <option
                        key={material.printMaterialId}
                        value={material.printMaterialId}
                      >
                        {material.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label htmlFor="fileName" className="form-label">
                    File URL:
                  </label>
                  <input
                    type="text"
                    id="fileName"
                    name="fileName"
                    className="form-control"
                    placeholder="Enter file URL (with or without http/https)"
                    pattern="^(https?:\/\/)?.+"
                    required
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  />
                </div>

                <div>
                  <label htmlFor="price" className="form-label">
                    Price:
                  </label>
                  <input
                    type="number"
                    id="price"
                    name="price"
                    className="form-control"
                    step="0.01"
                    min="0"
                    required
                    style={{
                      backgroundColor: "#776d91",
                      color: "#ffffff",
                      border: "1px solid #ffffff",
                      borderRadius: "10px",
                    }}
                  />
                </div>
              </div>
            </div>
            <div className="modal-footer border-0 d-flex justify-content-center">
              <button
                type="submit"
                className="btn btn-lg"
                disabled={isSubmitting}
                style={{
                  backgroundColor: "#2c1d55",
                  color: "#ffffff",
                  borderRadius: "10px",
                  padding: "10px 50px",
                }}
              >
                {isSubmitting ? 'Creating...' : 'Create'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default OrderModal;
