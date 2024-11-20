import { useState } from "react";
import "./assets/css/loginSignup.css";
import user_icon from "./assets/images/person.png";
import email_icon from "./assets/images/email.png";
import password_icon from "./assets/images/password.png";
import { authService } from "../../services/authService";

interface LoginSignupProps {
  onClick: (isLoggedIn: boolean) => void;
  showLS: boolean;
  onClose: () => void;
}

// Валідація для кожного поля
const validateFirstName = (value: string) => {
  if (!value) return "First Name is required";
  if (!/^[A-Za-z]+$/.test(value)) return "First Name must contain only letters";
  return "";
};

const validateLastName = (value: string) => {
  if (!value) return "Last Name is required";
  if (!/^[A-Za-z]+$/.test(value)) return "Last Name must contain only letters";
  return "";
};

const validateEmail = (value: string) => {
  if (!value) return "Email is required";
  if (!/\S+@\S+\.\S+/.test(value)) return "Invalid email address";
  return "";
};

const validatePassword = (value: string) => {
  if (!value) return "Password is required";
  if (value.length < 8) return "Password must be at least 8 characters";
  return "";
};

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

  // Обробка змін у полях
  const handleChange = (field: string, value: string) => {
    let error = "";
    if (field === "firstName") {
      setFirstName(value);
      error = validateFirstName(value);
    } else if (field === "lastName") {
      setLastName(value);
      error = validateLastName(value);
    } else if (field === "email") {
      setEmail(value);
      error = validateEmail(value);
    } else if (field === "password") {
      setPassword(value);
      error = validatePassword(value);
    }
    setErrors((prev) => ({ ...prev, [field]: error }));
  };

  const submit = async () => {
    const newErrors = {
      firstName: action === "Sign Up" ? validateFirstName(firstName) : "",
      lastName: action === "Sign Up" ? validateLastName(lastName) : "",
      email: validateEmail(email),
      password: validatePassword(password),
    };

    const hasError = Object.values(newErrors).some((error) => error !== "");
    setErrors(newErrors);

    if (hasError) return;

    if (action === "Sign In") {
      try {
        const token = await authService.login({
          name: email,
          role: "user",
        });
        localStorage.setItem("token", token);
        onClick(true);
        onClose();
      } catch (error) {
        console.error("Login failed:", error);
      }
    } else {
      onClick(true);
      onClose();
    }
  };

  return (
      <>
        {showLS && (
            <div onClick={onClose} className="background">
              <div className="container" onClick={(e) => e.stopPropagation()}>
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

                <div className="inputs">
                  {action === "Sign Up" && (
                      <>
                        <div className="input">
                          <img src={user_icon} alt="" />
                          <input
                              type="text"
                              placeholder="First Name"
                              value={firstName}
                              onChange={(e) => handleChange("firstName", e.target.value)}
                          />
                        </div>
                        {errors.firstName && (
                            <div className="error">{errors.firstName}</div>
                        )}

                        <div className="input">
                          <img src={user_icon} alt="" />
                          <input
                              type="text"
                              placeholder="Last Name"
                              value={lastName}
                              onChange={(e) => handleChange("lastName", e.target.value)}
                          />
                        </div>
                        {errors.lastName && (
                            <div className="error">{errors.lastName}</div>
                        )}
                      </>
                  )}

                  <div className="input">
                    <img src={email_icon} alt="" />
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => handleChange("email", e.target.value)}
                    />
                  </div>
                  {errors.email && <div className="error">{errors.email}</div>}

                  <div className="input">
                    <img src={password_icon} alt="" />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => handleChange("password", e.target.value)}
                    />
                  </div>
                  {errors.password && <div className="error">{errors.password}</div>}
                </div>

                <div className="submit-container">
                  <button
                      className="submit"
                      onClick={submit}
                      disabled={
                        action === "Sign In"
                            ? errors.email !== "" || errors.password !== ""
                            : Object.values(errors).some((error) => error !== "")
                      }
                  >
                    {action === "Sign In" ? "SIGN IN" : "SIGN UP"}
                  </button>
                </div>
              </div>
            </div>
        )}
      </>
  );
};

export default LoginSignup;
