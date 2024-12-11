import { useState, useEffect } from "react";
import { Routes, Route } from "react-router-dom";
import "./App.css";
import Header from "./components/Header/Header.tsx";
import MainPage from "./components/MainPage/MainPage.tsx";
import LoginSignup from "./components/LoginSignup/LoginSignup.tsx";
import Requests from "./components/Requests/Requests.tsx";
import { authService } from "./services/authService";
import { AddPrinter } from "./components/Requests/components/AddPrinter";

function App() {
  const [isLogined, setIsLogined] = useState<boolean>(authService.isLoggedIn());
  const [showLogin, setShowLogin] = useState<boolean>(false);

  useEffect(() => {
    const loginState = authService.isLoggedIn();
    setIsLogined(loginState);
  }, []);

  const handleCloseLogin = () => {
    setShowLogin(false);
  };

  const handleLogout = () => {
    setIsLogined(false);
    authService.logout();
  };

  return (
    <>
      <Header 
        isLogined={isLogined} 
        showLS={setShowLogin} 
        onLogout={handleLogout}
      />
      <LoginSignup
        onClick={setIsLogined}
        showLS={showLogin}
        onClose={handleCloseLogin}
      />      
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/requests" element={<Requests />} />
        <Route path="/requests/printer" element={<AddPrinter />} />
      </Routes>
    </>
  );
}

export default App;
