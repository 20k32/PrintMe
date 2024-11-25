import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/header.css";
import chats from "../../assets/images/chats.png";
import profile from "../../assets/images/profile.png";
import logOut from "../../assets/images/log-out.png";

interface HeaderProps {
  isLogined: boolean;
  showLS: (isShowedLS: boolean) => void;
  onLogout: () => void;
}

const Header: React.FC<HeaderProps> = ({ isLogined, showLS, onLogout }) => {
  const handleShowLS = () => {
    showLS(true);
  };

  return (
    <div className="container-fluid" style={{ backgroundColor: "#090126" }}>
      <header className="d-flex align-items-center py-4">
        {/* Логотип */}
        <a
          href="/"
          className="d-flex align-items-center flex-grow-1 text-white text-decoration-none fw-bold"
          style={{ fontSize: "1.8rem", padding: "6px 12px" }}
        >
          PrintMe
        </a>

        {/* Навигация */}
        <ul className="nav justify-content-center flex-grow-1 fw-bold">
          <li>
            <a href="#" className="nav-link px-3 text-white fs-5">
              Main Page
            </a>
          </li>
          <li>
            <a href="#" className="nav-link px-3 text-white fs-5">
              Info
            </a>
          </li>
          {isLogined && (
            <>
              <li>
                <a href="#" className="nav-link px-3 text-white fs-5">
                  Printers
                </a>
              </li>
              <li>
                <a href="#" className="nav-link px-3 text-white fs-5">
                  Orders
                </a>
              </li>
            </>
          )}
        </ul>

        {}
        <div className="d-flex align-items-center flex-grow-1 justify-content-end">
          {isLogined ? (
            <div className="d-flex align-items-center gap-4">
              <a href="#">
                <img src={chats} alt="Chats" width="30" height="30" />
              </a>
              <a href="#">
                <img src={profile} alt="Profile" width="30" height="30" />
              </a>
              <a href="#" onClick={onLogout}>
                <img src={logOut} alt="Log Out" width="30" height="30" />
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
