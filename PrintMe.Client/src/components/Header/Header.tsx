import React from "react";
import { Link, useLocation } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/header.css";

interface HeaderProps {
  isLogined: boolean;
  showLS: (isShowedLS: boolean) => void;
  onLogout: () => void;
}

const Header: React.FC<HeaderProps> = ({ isLogined, showLS, onLogout }) => {
  const location = useLocation();

  const handleShowLS = () => {
    showLS(true);
  };

  return (
    <div className="container-fluid" style={{ backgroundColor: "#090126" }}>
      <header className="d-flex align-items-center py-4">
        <Link to="/"
          className="d-flex align-items-center flex-grow-1 text-white text-decoration-none fw-bold"
          style={{ fontSize: "1.8rem", padding: "6px 12px" }}
        >
          PrintMe
        </Link>

        <ul className="nav justify-content-center flex-grow-1 fw-bold">
          <li>
            <Link to="/main" className={`nav-link px-3 text-white fs-5 ${location.pathname === '/main' ? 'active' : ''}`}>
              Main Page
            </Link>
          </li>
          <li>
            <Link to="/info" className="nav-link px-3 text-white fs-5">
              Info
            </Link>
          </li>
          {isLogined && (
            <>
              <li>
                <Link to='/printers' className={`nav-link px-3 text-white fs-5 ${location.pathname === '/printers' ? 'active' : ''}`}>
                  Printers
                </Link>
              </li>
              <li>
                <Link to="/orders" className={`nav-link px-3 text-white fs-5 ${location.pathname === '/orders' ? 'active' : ''}`}>
                  Orders
                </Link>
              </li>
              <li>
                <Link to="/requests" className={`nav-link px-3 text-white fs-5 ${location.pathname === '/requests' ? 'active' : ''}`}>
                  Requests
                </Link>
              </li>
            </>
          )}
        </ul>

        <div className="d-flex align-items-center flex-grow-1 justify-content-end">
          {isLogined ? (
            <div className="d-flex align-items-center gap-4">
              <Link to="/chatPage" className="text-white header-icon">
                <i className="bi bi-chat-dots-fill fs-2"></i>
              </Link>
              <Link to="/profile" className={`text-white header-icon ${location.pathname === '/profile' ? 'active' : ''}`}>
                  <i className="bi bi-person-circle fs-2"></i>
              </Link>
              <a href="#" onClick={onLogout} className="text-white header-icon">
                  <i className="bi bi-box-arrow-right fs-2"></i>
              </a>
            </div>
          ) : (
            <button
              type="button"
              className="btn btn-outline-light fw-bold fs-5"
              onClick={handleShowLS}
            >
              Sign in/up
            </button>
          )}
        </div>
      </header>
    </div>
  );
};

export default Header;