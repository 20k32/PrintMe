import { useState } from 'react';
import './assets/css/loginSignup.css';
import user_icon from './assets/images/person.png';
import email_icon from './assets/images/email.png';
import password_icon from './assets/images/password.png';

interface LoginSignupProps {
    onClick: (isLoggedIn: boolean) => void;
    showLS: boolean;
    onClose: () => void; // Функція для закриття модального вікна
  }
  

  export const LoginSignup: React.FC<LoginSignupProps> = ({
    onClick,
    showLS,
    onClose,
  }) => {
    const [action, setAction] = useState("Sign In"); 

    const handleChangetrue = () => {
        onClick(true);
        onClose(); // Закриває модальне вікно після успішного входу
      };

  const closeWindow = () =>{
    showLS=false;
  }

  return (
    <>
    {showLS && (
        <div onClick={onClose} className="background">
      <div className="container" onClick={(e) => e.stopPropagation()}>
        {}
        <div className="header">
            <div 
                className={action === "Sign In" ? "text active" : "text"} 
                onClick={() => setAction("Sign In")}
            >
                SIGN IN
            </div>
            <div 
                className={action === "Sign Up" ? "text active" : "text"} 
                onClick={() => setAction("Sign Up")}
            >
                SIGN UP
            </div>
        </div>

        {}
        <div className="inputs">
            {action === "Sign Up" && (
                <>
                    <div className="input">
                        <img src={user_icon} alt="" />
                        <input type="text" placeholder="First Name" />
                    </div>
                    <div className="input">
                        <img src={user_icon} alt="" />
                        <input type="text" placeholder="Last Name" />
                    </div>
                </>
            )}
            
            <div className="input">
                <img src={email_icon} alt="" />
                <input type="email" placeholder="Email" />
            </div>
            
            <div className="input">
                <img src={password_icon} alt="" />
                <input type="password" placeholder="Password" />      
            </div>
        </div>

        {}
        {action === "Sign In" && (
            <div className="forgot-password">
                Forgot password? <span>Click here</span>
            </div>
        )}
        
        {}
        <div className="submit-container">
            <div className="submit" onClick={handleChangetrue}>
                {action === "Sign In" ? "SIGN IN" : "SIGN UP"}
            </div>
        </div>
    </div>
    </div>
    )};
    </>
  );
};


export default LoginSignup;
