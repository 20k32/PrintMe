import { Material } from "../constants";
import { baseApiService } from "./baseApiService";
import { printerService } from "./printerService";
import { MarkerDto, RequestData } from "../types/api";

export interface FetchParams extends RequestData {
  maxModelHeight?: number;
  maxModelWidth?: number;
  materials?: Material[];
}

interface PrinterInfo {
  id: number;
  modelName: string;
  materials: Material[];
}

const getPrinters = async (params: FetchParams): Promise<MarkerDto[]> => {
  return baseApiService.put<MarkerDto[]>("/printers/markers", params);
};

const createInfoWindowContent = (modelName: string, materials: Material[]) => `
    <div>
        <h6>Printer ${modelName}</h6>
        <p>Materials: ${materials.map((material) => material.name).join(", ")}</p>
        <button class="btn btn-primary create-order-btn" data-printer="${encodeURIComponent(modelName)}" style="background-color: #2c1d55">Create Order</button>
    </div>
`;

const markerCache = new Map<string, google.maps.marker.AdvancedMarkerElement>();

const createMarker = async (
    marker: MarkerDto,
    map: google.maps.Map,
    AdvancedMarkerElement: typeof google.maps.marker.AdvancedMarkerElement
): Promise<google.maps.marker.AdvancedMarkerElement> => {
  const cacheKey = `${marker.id}-${marker.locationX}-${marker.locationY}`;

  if (markerCache.has(cacheKey)) {
    const existingMarker = markerCache.get(cacheKey)!;
    existingMarker.map = map;
    return existingMarker;
  }

  const position = { lat: marker.locationY, lng: marker.locationX };
  const printerInfo = await printerService.getPrinterMinimalInfo(marker.id);

  const advancedMarker = new AdvancedMarkerElement({
    position,
    title: `Printer ${printerInfo.modelName}`,
    map: map,
  });

  const infoWindow = new google.maps.InfoWindow({
    content: createInfoWindowContent(printerInfo.modelName, printerInfo.materials),
  });

  advancedMarker.addListener("click", () => {
    document.querySelectorAll(".gm-ui-hover-effect").forEach((element) => {
      (element as HTMLElement).click();
    });

    infoWindow.open({
      anchor: advancedMarker,
      map,
    });

    google.maps.event.addListenerOnce(infoWindow, 'domready', () => {
      const createOrderBtn = document.querySelector('.create-order-btn');
      if (createOrderBtn) {
        createOrderBtn.addEventListener("click", () => {
          renderModal(printerInfo);
        });
      }
    });
  });

  markerCache.set(cacheKey, advancedMarker);
  return advancedMarker;
};

const renderModal = (printerInfo: PrinterInfo) => {
  const existingModal = document.getElementById("orderModal");
  if (existingModal) existingModal.remove();

  const modal = document.createElement("div");
  modal.id = "orderModal";
  modal.className = "modal d-block";
  modal.tabIndex = -1;
  modal.style.cssText = `
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
`;

  modal.innerHTML = `
  <div class="modal-dialog" style="max-width: 600px;">
    <div class="modal-content" style="background-color: #554877; color: #ffffff; border-radius: 15px;">
      <div class="modal-header border-0">
        <h3 class="modal-title w-100 text-center">Create Order for ${printerInfo.modelName}</h3>
        <button type="button" class="btn-close" aria-label="Close" id="closeModalButton"
                style="color: #ffffff; opacity: 1; font-size: 1.5rem; border: none; background: none;">&times;</button>
      </div>
      <div class="modal-body">
        <form id="orderForm" class="d-flex flex-column gap-3">
          <div>
            <label for="orderName" class="form-label">Order Name:</label>
            <input type="text" id="orderName" class="form-control" 
                   style="background-color: #776d91; color: #ffffff; border: 1px solid #ffffff; 
                   border-radius: 10px;">
          </div>
          <div>
            <label for="description" class="form-label">Description:</label>
            <textarea id="description" class="form-control" rows="3"
                      style="background-color: #776d91; color: #ffffff; border: 1px solid #ffffff; 
                      border-radius: 10px;"></textarea>
          </div>
          <div>
            <label for="fileInput" class="form-label">Attach File:</label>
            <input type="file" id="fileInput" class="form-control" 
                   style="background-color: #776d91; color: #ffffff; border: 1px solid #ffffff; 
                   border-radius: 10px;">
          </div>
          <div>
            <label for="contact" class="form-label">Add Your Contact:</label>
            <input type="text" id="contact" class="form-control" 
                   style="background-color: #776d91; color: #ffffff; border: 1px solid #ffffff; 
                   border-radius: 10px;">
          </div>
          <div>
            <label for="price" class="form-label">Price:</label>
            <input type="number" id="price" class="form-control"
                   style="background-color: #776d91; color: #ffffff; border: 1px solid #ffffff; 
                   border-radius: 10px;">
          </div>
        </form>
      </div>
      <div class="modal-footer border-0 d-flex justify-content-center">
        <button id="submitOrder" class="btn btn-lg" 
                style="background-color: #2c1d55; color: #ffffff; border-radius: 10px; 
                padding: 10px 50px;">
          Create
        </button>
      </div>
    </div>
  </div>
`;

  document.body.appendChild(modal);

  document.getElementById("closeModalButton")?.addEventListener("click", () => {
    modal.remove(); 
  });

  document.getElementById("submitOrder")?.addEventListener("click", () => {
    const orderName = (document.getElementById("orderName") as HTMLInputElement).value;
    const description = (document.getElementById("description") as HTMLTextAreaElement).value;
    const fileInput = (document.getElementById("fileInput") as HTMLInputElement).files;
    const contact = (document.getElementById("contact") as HTMLInputElement).value;
    const price = (document.getElementById("price") as HTMLInputElement).value;

    console.log({
      orderName,
      description,
      contact,
      price,
      file: fileInput ? fileInput[0] : null,
      printerName: printerInfo.modelName,
    });

    modal.remove(); 
  });


};

export const markersService = {
  async getGoogleMapsMarkers(
      map: google.maps.Map,
      params: FetchParams
  ): Promise<google.maps.marker.AdvancedMarkerElement[]> {
    const { AdvancedMarkerElement } = (await google.maps.importLibrary(
        "marker"
    )) as google.maps.MarkerLibrary;
    const markersData = await getPrinters(params);

    for (const [key, marker] of markerCache.entries()) {
      if (!markersData.some((m) => `${m.id}-${m.locationX}-${m.locationY}` === key)) {
        marker.map = null;
        markerCache.delete(key);
      }
    }

    const newMarkers = await Promise.all(
        markersData.map((marker) => createMarker(marker, map, AdvancedMarkerElement))
    );

    return newMarkers;
  },
  clearMarkers: () => {
    markerCache.forEach((marker) => (marker.map = null));
    markerCache.clear();
  },
  getMarkers: () => baseApiService.get<MarkerDto[]>("/markers", true, true),
};
