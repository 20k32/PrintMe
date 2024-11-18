import { useState } from "react";
import "./assets/css/loginSignup.css";
import user_icon from "./assets/images/person.png";
import email_icon from "./assets/images/email.png";
import password_icon from "./assets/images/password.png";
import { authService } from "../../services/authService";

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
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");

  const [errors, setErrors] = useState({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
  });

  const handleChangetrue = () => {
    onClick(true);
    onClose(); // Закриває модальне вікно після успішного входу
  };

  const submit = async () => {
    let hasError = false;
    const newErrors = { email: "", password: "", firstName: "", lastName: "" };

    if (action === "Sign Up") {
      if (!firstName) {
        newErrors.firstName = "First Name is required";
        hasError = true;
      }
      if (!lastName) {
        newErrors.lastName = "Last Name is required";
        hasError = true;
      }
    }

    if (!email) {
      newErrors.email = "Email is required";
      hasError = true;
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      newErrors.email = "Invalid email address";
      hasError = true;
    }

    if (!password) {
      newErrors.password = "Password is required";
      hasError = true;
    } else if (password.length < 8) {
      newErrors.password = "Password must be at least 8 characters";
      hasError = true;
    }

    setErrors(newErrors);

    if (hasError) {
      return;
    }

    if (action === "Sign In") {
      try {
        const token = await authService.login({
          name: email,
          role: "user",
        });
        localStorage.setItem("token", token);
        handleChangetrue();
      } catch (error) {
        console.error("Login failed:", error);
      }
    } else {
      handleChangetrue();
    }
  };


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
                          <input
                              type="text"
                              placeholder="First Name"
                              value={firstName}
                              onChange={(e) => setFirstName(e.target.value)}
                          />
                          {errors.firstName && <div className="error">{errors.firstName}</div>}
                        </div>
                        <div className="input">
                          <img src={user_icon} alt="" />
                          <input
                              type="text"
                              placeholder="Last Name"
                              value={lastName}
                              onChange={(e) => setLastName(e.target.value)}
                          />
                          {errors.lastName && <div className="error">{errors.lastName}</div>}
                        </div>
                      </>
                  )}

                  <div className="input">
                    <img src={email_icon} alt="" />
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    {errors.email && <div className="error">{errors.email}</div>}
                  </div>

                  <div className="input">
                    <img src={password_icon} alt="" />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    {errors.password && <div className="error">{errors.password}</div>}
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
                  <div className="submit" onClick={submit}>
                    {action === "Sign In" ? "SIGN IN" : "SIGN UP"}
                  </div>
                </div>
              </div>
            </div>
        )}
        ;
      </>
  );
};

export default LoginSignup;

