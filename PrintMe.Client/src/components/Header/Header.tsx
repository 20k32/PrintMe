import React from "react";
import "./assets/css/header.css";
import chats from "./assets/images/chats.png";
import profile from "./assets/images/profile.png";
import logOut from "./assets/images/log-out.png";

interface HeaderProps {
  isLogined: boolean;
  showLS: (isShowedLS: boolean) => void; // Функція для відкриття/закриття LoginSignup
  onLogout: () => void;
}

const Header: React.FC<HeaderProps> = ({ isLogined, showLS, onLogout }) => {
  const handleShowLS = () => {
    showLS(true); // Відкриває модальне вікно
  };

  return (
    <div className="header-container">
      <div className="header-logo">PrintMe</div>
      <div className="header-pages">
        <a href="#">main page</a>
        <a href="#">info</a>
        {isLogined && (
          <>
            <a href="#">printers</a>
            <a href="#">orders</a>
          </>
        )}
      </div>
      <div className="header-profile">
        {isLogined ? (
          <>
            <a href="#">
              <img src={chats} alt="chats" />
            </a>
            <a href="#">
              <img src={profile} alt="profile" />
            </a>
            <a href="#">
              <img src={logOut} alt="logOut" onClick={onLogout} />
            </a>
          </>
        ) : (
          <a onClick={handleShowLS} href="#">
            sign up/in
          </a>
        )}
      </div>
    </div>
  );
};

export default Header;
