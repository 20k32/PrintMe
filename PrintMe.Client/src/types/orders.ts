export interface PrintOrderDto {
    printOrderId: number;
    userId: number;
    printerId: number;
    price: number;
    startDate: string;
    dueDate: string;
    itemLink: string | null;
    itemQuantity: number;
    itemDescription: string | null;
    itemMaterialId: number;
    printOrderStatusId: number;
    printOrderStatusReasonId: number;
  }
  