import {PrintMaterial} from "../constants";
import {baseApiService} from "./baseApiService";
import {printersService} from "./printersService";
import {MarkerDto, RequestData, SimplePrinterDto} from "../types/api";

export interface FetchParams extends RequestData {
  maxModelHeight?: number;
  maxModelWidth?: number;
  materials?: PrintMaterial[];
}

const getPrinters = async (params: FetchParams): Promise<MarkerDto[]> => {
  return baseApiService.put<MarkerDto[]>("/printers/markers", params);
};

const markerCache = new Map<string, google.maps.marker.AdvancedMarkerElement>();

export interface MarkerWithPrinterInfo extends google.maps.marker.AdvancedMarkerElement {
  printerInfo: SimplePrinterDto;
}

const createMarker = async (
    marker: MarkerDto,
    map: google.maps.Map,
    AdvancedMarkerElement: typeof google.maps.marker.AdvancedMarkerElement,
): Promise<MarkerWithPrinterInfo> => {
  const cacheKey = `${marker.id}-${marker.locationX}-${marker.locationY}`;

  if (markerCache.has(cacheKey)) {
    const existingMarker = markerCache.get(cacheKey)! as MarkerWithPrinterInfo;
    existingMarker.map = map;
    return existingMarker;
  }

  const position = { lat: marker.locationY, lng: marker.locationX };
  const printerInfo = await printersService.getPrinterMinimalInfo(marker.id);

  const advancedMarker = new AdvancedMarkerElement({
    position,
    title: `Printer ${printerInfo.modelName}`,
    map: map,
  }) as MarkerWithPrinterInfo;

  advancedMarker.printerInfo = printerInfo;

  markerCache.set(cacheKey, advancedMarker);
  return advancedMarker;
};

export const markersService = {
  async getGoogleMapsMarkers(
      map: google.maps.Map,
      params: FetchParams,
  ): Promise<MarkerWithPrinterInfo[]> {
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

    return await Promise.all(
        markersData.map((marker) => createMarker(marker, map, AdvancedMarkerElement))
    );
  },
  clearMarkers: () => {
    markerCache.forEach((marker) => (marker.map = null));
    markerCache.clear();
  },
  getMarkers: () => baseApiService.get<MarkerDto[]>("/markers", true, true),
};
