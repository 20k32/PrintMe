import "./headerstyle.css";
import chats from "../../assets/images/chats.png";
import profile from "../../assets/images/profile.png";
import logOut from "../../assets/images/log-out.png";
import isLogined from "./logined.json";

const Header = () => {
  return (
    <div className="header-container">
      <div className="header-logo">PrintMe</div>
      <div className="header-pages">
        <a href="#">main page</a>
        <a href="#">info</a>
        {isLogined.logined && (
          <>
            <a href="#">printers</a>
            <a href="#">orders</a>
          </>
        )}
      </div>
      <div className="header-profile">
        {isLogined.logined ? (
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
          <>
            <a href="#">sign up/in</a>
          </>
        )}
      </div>
    </div>
  );
};

export default Header;
