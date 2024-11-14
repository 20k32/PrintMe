import React from "react";
import "./assets/header.css";
import chats from "../../assets/images/chats.png";
import profile from "../../assets/images/profile.png";
import logOut from "../../assets/images/log-out.png";

interface HeaderProps {
  isLogined: boolean;
}

const Header: React.FC<HeaderProps> = ({ isLogined }) => {
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
              <img src={logOut} alt="logOut" />
            </a>
          </>
        ) : (
          <a href="#">sign up/in</a>
        )}
      </div>
    </div>
  );
};

export default Header;
