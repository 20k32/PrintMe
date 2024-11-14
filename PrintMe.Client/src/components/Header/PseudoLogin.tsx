import { useState } from "react";

interface PseudoLoginProps {
  onClick: (isLoggedIn: boolean) => void;
}

const PseudoLogin: React.FC<PseudoLoginProps> = ({ onClick }) => {
  const [isLogined, setIsLogined] = useState<boolean>(false);

  const handleChangetrue = () => {
    setIsLogined(true);
    onClick(true);
  };
  const handleChangefalse = () => {
    setIsLogined(false);
    onClick(false);
  };

  return (
    <div>
      <button onClick={handleChangetrue}>Login</button>
      <button onClick={handleChangefalse}>Logout</button>
    </div>
  );
};

export default PseudoLogin;
