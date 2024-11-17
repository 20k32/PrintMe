import { useState } from "react";
import "./App.css";
import Header from "./components/Header/Header.tsx";
import MainPage from "./components/MainPage/MainPage.tsx";
import LoginSignup from "./components/LoginSignup/LoginSignup.tsx";

function App() {
  const [isLogined, setIsLogined] = useState<boolean>(false);
  const [showLogin, setShowLogin] = useState<boolean>(false);

  const handleCloseLogin = () => {
    setShowLogin(false); // Закриває модальне вікно
  };

  return (
    <>
      <Header isLogined={isLogined} showLS={setShowLogin} />
      <LoginSignup
        onClick={setIsLogined}
        showLS={showLogin}
        onClose={handleCloseLogin} // Передає функцію закриття
      />      
      <MainPage />
    </>
  );
}

export default App;
