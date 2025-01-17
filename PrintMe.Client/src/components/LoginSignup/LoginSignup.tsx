import React, { useState, useEffect, useCallback } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/loginsignup.css";
import { authService } from "../../services/authService";
import { registrationService } from "../../services/registrationService";
import { handleApiError } from '../../utils/apiErrorHandler';

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
  const [isValid, setIsValid] = useState(false);

  useEffect(() => {
    const validateFields = () => {
      const newErrors: { [key: string]: string } = {};

      if (action === "Sign Up") {
        if (!formData.firstName.trim()) {
          newErrors.firstName = "First name is required.";
        }
        if (!formData.lastName.trim()) {
          newErrors.lastName = "Last name is required.";
        }
      }

      const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;

      if (!formData.email.trim()) {
        newErrors.email = "Email is required.";
      } else if (
        !emailRegex.test(formData.email)
      ) {
        newErrors.email = "Invalid email format.";
      }

      if (!formData.password.trim()) {
        newErrors.password = "Password is required.";
      } else if (formData.password.length < 6 && action === "Sign Up") {
        newErrors.password = "Password must be at least 6 characters.";
      }

      setErrors(newErrors);
      return Object.keys(newErrors).length === 0;
    };

    const isValidForm = validateFields();
    setIsValid(isValidForm);
  }, [formData, action]);

  const handleInputChange = (field: string, value: string) => {
    setFormData({ ...formData, [field]: value });
  };

  const submit = async () => {
    if (isValid) {
      try {
        if (action === "Sign In") {
          await authService.login({ email: formData.email, password: formData.password });
        } else if (action === "Sign Up") {
          await registrationService.register(formData);
        }

        const token = authService.getToken();
        if (token) {
          onClick(true);
          onClose();
          window.location.reload();
        }
      } catch (error) {
        setErrors({ 
          general: handleApiError(error, {
            unauthorized: "Invalid email or password.",
            notFound: "User not found.",
            conflict: "User already exists.",
            badRequest: "Invalid request. Please check your input."
          })
        });
      }
    }
  };

  const handleModalClick = useCallback(() => {
    if (window.getSelection()?.toString()) {
      return;
    }
    onClose();
  }, [onClose]);

  return (
    <>
      {showLS && (
        <div className="login-modal d-flex align-items-center justify-content-center position-fixed w-100 h-100"
             style={{ zIndex: 9999 }}
             onMouseUp={handleModalClick}>
          <div className="login-container p-4" 
               style={{ width: "400px" }}
               onMouseUp={(e) => e.stopPropagation()}>
            <div className="d-flex justify-content-center gap-4 mb-5">
              <h2 className={`login-tab ${action === "Sign In" ? "active" : ""}`}
                  onClick={() => setAction("Sign In")}>
                SIGN IN
              </h2>
              <h2 className={`login-tab ${action === "Sign Up" ? "active" : ""}`}
                  onClick={() => setAction("Sign Up")}>
                SIGN UP
              </h2>
            </div>

            <form className="d-flex flex-column gap-3">
              {action === "Sign Up" && (
                <>
                  <div className="position-relative">
                    <div className="input-group">
                      <span className="input-group-text input-icon">
                        <i className="bi bi-person-fill"></i>
                      </span>
                      <input
                        type="text"
                        className="form-control form-input"
                        placeholder="First Name"
                        value={formData.firstName}
                        onChange={(e) => handleInputChange("firstName", e.target.value)}
                        autoComplete="given-name"
                      />
                    </div>
                    {errors.firstName && (
                      <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                        {errors.firstName}
                      </small>
                    )}
                  </div>

                  <div className="position-relative">
                    <div className="input-group">
                      <span className="input-group-text input-icon">
                        <i className="bi bi-person-fill"></i>
                      </span>
                      <input
                        type="text"
                        className="form-control form-input"
                        placeholder="Last Name"
                        value={formData.lastName}
                        onChange={(e) => handleInputChange("lastName", e.target.value)}
                        autoComplete="family-name"
                      />
                    </div>
                    {errors.lastName && (
                      <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                        {errors.lastName}
                      </small>
                    )}
                  </div>
                </>
              )}

              <div className="position-relative">
                <div className="input-group">
                  <span className="input-group-text input-icon">
                    <i className="bi bi-envelope-fill"></i>
                  </span>
                  <input
                    type="email"
                    className="form-control form-input"
                    placeholder="Email"
                    value={formData.email}
                    onChange={(e) => handleInputChange("email", e.target.value)}
                    autoComplete="email"
                  />
                </div>
                {errors.email && (
                  <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                    {errors.email}
                  </small>
                )}
              </div>

              <div className="position-relative">
                <div className="input-group">
                  <span className="input-group-text input-icon">
                    <i className="bi bi-lock-fill"></i>
                  </span>
                  <input
                    type="password"
                    className="form-control form-input"
                    placeholder="Password"
                    value={formData.password}
                    onChange={(e) => handleInputChange("password", e.target.value)}
                    autoComplete={action === "Sign In" ? "current-password" : "new-password"}
                  />
                </div>
                {errors.password && (
                  <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                    {errors.password}
                  </small>
                )}
              </div>

              {action === "Sign In" && (
                <div className="text-end">
                  <a href="#" className="text-decoration-none" style={{ color: "#6c30f3" }}>
                    Forgot password?
                  </a>
                </div>
              )}

              {errors.general && (
                <div className="alert alert-danger" role="alert">
                  {errors.general}
                </div>
              )}

              <button
                type="button"
                className="submit-button"
                onClick={submit}
                disabled={!isValid}
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
