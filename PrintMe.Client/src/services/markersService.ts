import axios from 'axios';
import { API_BASE_URL } from "../constants";
import { printerService } from './printerService';

interface FetchParams {
    materials?: number[];
    maxHeight?: number;
    maxWidth?: number;
}

interface MarkerDto {
    id: number;
    locationX: number;
    locationY: number;
}

const getPrinters = async (params: FetchParams): Promise<MarkerDto[]> => {
    const response = await axios.get<MarkerDto[]>(`${API_BASE_URL}/printers/markers`, { params });
    return response.data;
};

let markers: google.maps.marker.AdvancedMarkerElement[] = [];

const clearAllMarkers = () => {
    markers.forEach(marker => marker.map = null);
    markers = [];
};

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
                content: `
                    <div>
                        <h6>Printer ${printerInfo.modelName}</h6>
                        <p>Materials: ${printerInfo.materials.map(material => material.name).join(", ")}</p>
                    </div>
                `
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
    clearMarkers: clearAllMarkers
};
