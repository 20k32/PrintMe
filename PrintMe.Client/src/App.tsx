import { useState, useEffect } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";
import "./App.css";
import Header from "./components/Header/Header.tsx";
import MainPage from "./components/MainPage/MainPage.tsx";
import LoginSignup from "./components/LoginSignup/LoginSignup.tsx";
import Requests from "./components/Requests/Requests.tsx";
import { authService } from "./services/authService";
import Profile from "./components/Profile/Profile.tsx";
import { AddPrinter } from "./components/Printers/components/AddPrinter.tsx";
import Orders from "./components/Orders/Orders.tsx";
import OrderDetails from "./components/Orders/components/OrderDetails.tsx";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Printers from "./components/Printers/Printers.tsx";
import PrinterDetail from "./components/Printers/components/PrinterDetail.tsx";
import EditOrder from "./components/Orders/components/EditOrder";
import Chat from "./components/Chat/Chat.tsx";

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
        <Route path="/printers" element={<Printers />} />
        <Route path="/printers/add" element={<AddPrinter />} />
        <Route path="/printers/:id" element={<PrinterDetail />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/orders/:orderId" element={<OrderDetails />} />
        <Route path="/orders/:orderId/edit" element={<EditOrder />} />
          <Route path="/chatPage" element={<Chat />} />
      </Routes>
      <ToastContainer
        position="bottom-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="dark"
      />
    </>
  );
}

export default App;