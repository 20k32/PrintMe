import { Libraries } from "@react-google-maps/api";

export const GOOGLE_MAPS_API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

export const MAP_ID = "7dabcc41eef25214";


export const MAP_CONFIG = {
  center: { // Ukraine center
    lat: 48.3794,
    lng: 31.1656,
  },
  zoom: 5.6,
  containerStyle: {
    width: "100%",
    height: "100%",
  },
  libraries: ['places', 'geometry', 'drawing'] as Libraries,
  mapId: MAP_ID
};

export const FILTER_OPTIONS = {
  materials: ["PLA", "ABS", "PETG"],
  sizes: ["Small", "Medium", "Large"],
  colors: ["Red", "Green", "Blue"],
};

export type FilterKey = 'materials' | 'sizes' | 'colors';

export const INITIAL_FILTER_STATE: Record<FilterKey, string[]> = {
  materials: [],
  sizes: [],
  colors: [],
};

export const API_BASE_URL = "http://localhost:5193/api";