export type FilterOption = {
  label: string;
  options: string[];
  state: string[];
  setState: (value: string[]) => void;
  isOpen: boolean;
  setIsOpen: (value: boolean | ((prev: boolean) => boolean)) => void;
};
