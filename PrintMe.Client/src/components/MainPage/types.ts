import { PrintMaterial } from '../../constants';

export type FilterOption = {
  label: string;
  options: PrintMaterial[];
  state: PrintMaterial[];
  setState: (value: PrintMaterial[]) => void;
  isOpen: boolean;
  setIsOpen: (value: boolean | ((prev: boolean) => boolean)) => void;
};
