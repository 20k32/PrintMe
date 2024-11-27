import { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import personIcon from "./assets/images/person.png";
import emailIcon from "./assets/images/email.png";
import passwordIcon from "./assets/images/password.png";
import { authService } from "../../services/authService";
import { registerService } from "../../services/registrationService";

interface LoginSignupProps {
  onClick: (isLoggedIn: boolean) => void;
  showLS: boolean;
  onClose: () => void;
}

const LoginSignup: React.FC<LoginSignupProps> = ({
  onClick,
  showLS,
  onClose,
}) => {
  const [action, setAction] = useState("Sign In");
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  const validateFields = () => {
    const newErrors: { [key: string]: string } = {};

    if (action === "Sign Up") {
      if (!formData.firstName.trim()) {
        newErrors.firstName = "First name is required.";
      }
      if (!formData.lastName.trim()) {
        newErrors.lastName = "Last name is required.";
      }
      if (formData.password.length < 6) {
        newErrors.password = "Password must be at least 6 characters.";
      }
    }

    if (!formData.email.trim()) {
      newErrors.email = "Email is required.";
    } else if (
      !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(formData.email)
    ) {
      newErrors.email = "Invalid email format.";
    }

    if (!formData.password.trim()) {
      newErrors.password = "Password is required.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const submit = async () => {
    if (validateFields()) {
      try {
        let token;
        if (action === "Sign In") {
          token = await authService.login({ email: formData.email, password: formData.password });
        } else if (action === "Sign Up") {
          token = await registerService.register(formData);
        }
        if (token) {
          onClick(true);
          onClose();
        }
      } catch (error) {
        setErrors({ general: (error as Error).message || "Action failed. Please try again." });
      }
    }
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData({ ...formData, [field]: value });
  };

  return (
    <>
      {showLS && (
        <div
          className="d-flex align-items-center justify-content-center position-fixed w-100 h-100 bg-dark bg-opacity-50"
          style={{ zIndex: 9999 }}
          onClick={onClose}
        >
          <div
            className="bg-white p-4 rounded shadow-lg"
            style={{ width: "400px" }}
            onClick={(e) => e.stopPropagation()}
          >
            <div className="d-flex justify-content-center mb-4">
              <h2
                className={`me-4 ${
                  action === "Sign In"
                    ? "text-primary text-decoration-underline fw-bold"
                    : "text-muted"
                }`}
                onClick={() => setAction("Sign In")}
                style={{
                  cursor: "pointer",
                  color: action === "Sign In" ? "#6c30f3" : "#6c757d",
                }}
              >
                SIGN IN
              </h2>
              <h2
                className={`${
                  action === "Sign Up"
                    ? "text-primary text-decoration-underline fw-bold"
                    : "text-muted"
                }`}
                onClick={() => setAction("Sign Up")}
                style={{
                  cursor: "pointer",
                  color: action === "Sign Up" ? "#6c30f3" : "#6c757d",
                }}
              >
                SIGN UP
              </h2>
            </div>
            <form>
              {action === "Sign Up" && (
                <>
                  <div className="mb-3 position-relative">
                    <div className="input-group">
                      <span className="input-group-text bg-light border-0">
                        <img src={personIcon} alt="First Name" style={{ width: "20px" }} />
                      </span>
                      <input
                        type="text"
                        className="form-control bg-light border-0 text-dark"
                        placeholder="First Name"
                        value={formData.firstName}
                        onChange={(e) =>
                          handleInputChange("firstName", e.target.value)
                        }
                        style={{
                          boxShadow: "none",
                          backgroundColor: "#d3d3d3",
                        }}
                      />
                    </div>
                    {errors.firstName && (
                      <small className="text-danger position-absolute">
                        {errors.firstName}
                      </small>
                    )}
                  </div>
                  <div className="mb-3 position-relative">
                    <div className="input-group">
                      <span className="input-group-text bg-light border-0">
                        <img src={personIcon} alt="Last Name" style={{ width: "20px" }} />
                      </span>
                      <input
                        type="text"
                        className="form-control bg-light border-0 text-dark"
                        placeholder="Last Name"
                        value={formData.lastName}
                        onChange={(e) =>
                          handleInputChange("lastName", e.target.value)
                        }
                        style={{
                          boxShadow: "none",
                          backgroundColor: "#d3d3d3",
                        }}
                      />
                    </div>
                    {errors.lastName && (
                      <small className="text-danger position-absolute">
                        {errors.lastName}
                      </small>
                    )}
                  </div>
                </>
              )}
              <div className="mb-3 position-relative">
                <div className="input-group">
                  <span className="input-group-text bg-light border-0">
                    <img src={emailIcon} alt="Email" style={{ width: "20px" }} />
                  </span>
                  <input
                    type="email"
                    className="form-control bg-light border-0 text-dark"
                    placeholder="Email"
                    value={formData.email}
                    onChange={(e) => handleInputChange("email", e.target.value)}
                    style={{
                      boxShadow: "none",
                      backgroundColor: "#d3d3d3",
                    }}
                  />
                </div>
                {errors.email && (
                  <small className="text-danger position-absolute">
                    {errors.email}
                  </small>
                )}
              </div>
              <div className="mb-3 position-relative">
                <div className="input-group">
                  <span className="input-group-text bg-light border-0">
                    <img
                      src={passwordIcon}
                      alt="Password"
                      style={{ width: "20px" }}
                    />
                  </span>
                  <input
                    type="password"
                    className="form-control bg-light border-0 text-dark"
                    placeholder="Password"
                    value={formData.password}
                    onChange={(e) =>
                      handleInputChange("password", e.target.value)
                    }
                    style={{
                      boxShadow: "none",
                      backgroundColor: "#d3d3d3",
                    }}
                  />
                </div>
                {errors.password && (
                  <small className="text-danger position-absolute">
                    {errors.password}
                  </small>
                )}
              </div>
              {action === "Sign In" && (
                <div className="text-center mb-3">
                  <small>
                    Forgot password?{" "}
                    <a href="#" className="text-primary">
                      Click here
                    </a>
                  </small>
                </div>
              )}
              <button
                type="button"
                className="btn btn-primary w-100 py-2 fw-bold"
                onClick={submit}
                style={{
                  backgroundColor: "#6c30f3",
                  borderRadius: "20px",
                }}
              >
                {action === "Sign In" ? "SIGN IN" : "SIGN UP"}
              </button>
            </form>
          </div>
        </div>
      )}
    </>
  );
};

export default LoginSignup;
