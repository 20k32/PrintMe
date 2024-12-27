import { PrintMaterial } from "../constants";
import { baseApiService } from './baseApiService';
import { printerService } from './printerService';
import { MarkerDto, RequestData } from '../types/api';

export interface FetchParams extends RequestData {
    maxModelHeight?: number;
    maxModelWidth?: number;
    materials?: PrintMaterial[];
}

const getPrinters = async (params: FetchParams): Promise<MarkerDto[]> => {
    return baseApiService.put<MarkerDto[]>('/printers/markers', params);
};

const createInfoWindowContent = (modelName: string, materials: PrintMaterial[]) => `
    <div>
        <h6>Printer ${modelName}</h6>
        <p>Materials: ${materials.map(material => material.name).join(", ")}</p>
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
    map: map
  });

  const infoWindow = new google.maps.InfoWindow({
    content: createInfoWindowContent(printerInfo.modelName, printerInfo.materials)
  });

  advancedMarker.addListener("click", () => {
    document.querySelectorAll('.gm-ui-hover-effect').forEach(element => {
      (element as HTMLElement).click();
    });
    
    infoWindow.open({
      anchor: advancedMarker,
      map
    });
  });

  markerCache.set(cacheKey, advancedMarker);
  return advancedMarker;
};

export const markersService = {
  async getGoogleMapsMarkers(map: google.maps.Map, params: FetchParams): Promise<google.maps.marker.AdvancedMarkerElement[]> {
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker") as google.maps.MarkerLibrary;
    const markersData = await getPrinters(params);
    
    for (const [key, marker] of markerCache.entries()) {
      if (!markersData.some(m => `${m.id}-${m.locationX}-${m.locationY}` === key)) {
        marker.map = null;
        markerCache.delete(key);
      }
    }

    const newMarkers = await Promise.all(
      markersData.map(marker => createMarker(marker, map, AdvancedMarkerElement))
    );

    return newMarkers;
  },
  clearMarkers: () => {
    markerCache.forEach(marker => marker.map = null);
    markerCache.clear();
  },
  getMarkers: () => baseApiService.get<MarkerDto[]>('/markers', true, true)
};
