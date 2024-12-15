import { Material } from "../constants";
import { baseApiService } from './baseApiService';
import { printerService } from './printerService';
import { MarkerDto, RequestData } from '../types/api';

export interface FetchParams extends RequestData {
    maxModelHeight?: number;
    maxModelWidth?: number;
    materials?: Material[];
}

const getPrinters = async (params: FetchParams): Promise<MarkerDto[]> => {
    return baseApiService.put<MarkerDto[]>('/printers/markers', params);
};

let markers: google.maps.marker.AdvancedMarkerElement[] = [];

const clearAllMarkers = () => {
    markers.forEach(marker => marker.map = null);
    markers = [];
};

const createInfoWindowContent = (modelName: string, materials: Material[]) => `
    <div>
        <h6>Printer ${modelName}</h6>
        <p>Materials: ${materials.map(material => material.name).join(", ")}</p>
    </div>
`;

export const markersService = {
    async getGoogleMapsMarkers(map: google.maps.Map, params: FetchParams): Promise<google.maps.marker.AdvancedMarkerElement[]> {
        clearAllMarkers();

        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker") as google.maps.MarkerLibrary;
        const markersData = await getPrinters(params);
        
        const newMarkers = await Promise.all(markersData.map(async marker => {

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

            return advancedMarker;
        }));

        markers = newMarkers;
        return newMarkers;
    },
    clearMarkers: clearAllMarkers,
    getMarkers: () => baseApiService.get<MarkerDto[]>('/markers', true, true)
};
