import { useState, useEffect } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";
import "./App.css";
import Header from "./components/Header/Header.tsx";
import MainPage from "./components/MainPage/MainPage.tsx";
import LoginSignup from "./components/LoginSignup/LoginSignup.tsx";
import Requests from "./components/Requests/Requests.tsx";
import { authService } from "./services/authService";
import Profile from "./components/Profile/Profile.tsx";

function App() {
  const [isLogined, setIsLogined] = useState<boolean>(authService.isLoggedIn());
  const [showLogin, setShowLogin] = useState<boolean>(false);

  const navigate = useNavigate();

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
    navigate("/mainpage");
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
        <Route index element={<MainPage />} />
        <Route path="/main" element={<MainPage />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/requests" element={<Requests />} />
      </Routes>
    </>
  );
}

export default App;