import { Material } from '../../constants';

export type FilterOption = {
  label: string;
  options: Material[];
  state: Material[];
  setState: (value: Material[]) => void;
  isOpen: boolean;
  setIsOpen: (value: boolean | ((prev: boolean) => boolean)) => void;
};
