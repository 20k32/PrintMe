import { Libraries } from "@react-google-maps/api";

export const GOOGLE_MAPS_API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

export const MAP_ID = "7dabcc41eef25214";

export const GOOGLE_MAPS_LIBRARIES: Libraries = ['places', 'geometry', 'drawing'];

export const MAP_CONFIG = {
  center: { // Ukraine center
    lat: 48.3794,
    lng: 31.1656,
  },
  zoom: 6,
  minZoom: 1,
  maxBounds: {
    north: 85,
    south: -85,
    west: -180,
    east: 180,
  },
  containerStyle: {
    width: "100%",
    height: "100%",
  },
  libraries: GOOGLE_MAPS_LIBRARIES,
  mapId: MAP_ID
};

export const FILTER_OPTIONS = {
  materials: [] as Material[],
};

export type FilterKey = 'materials' | 'maxModelHeight' | 'maxModelWidth';

export interface FilterState {
  materials: Material[];
  maxModelHeight: number;
  maxModelWidth: number;
}

export const INITIAL_FILTER_STATE: FilterState = {
  maxModelHeight: 0,
  maxModelWidth: 0,
  materials: [],
};

export interface Material {
  printMaterialId: number;
  name: string;
}

export const API_BASE_URL = "http://localhost:5193/api";