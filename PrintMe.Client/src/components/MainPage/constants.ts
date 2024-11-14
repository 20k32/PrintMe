export const GOOGLE_MAPS_API_KEY = "AIzaSyBTQjA84vcQVkumDzEnZHSABh6NAUsLh4A";

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
