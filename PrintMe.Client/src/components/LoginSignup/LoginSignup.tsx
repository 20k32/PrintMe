import React, { useEffect, useCallback } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/loginsignup.css";
import { useDispatch, useSelector } from 'react-redux';
import { useForm, SubmitHandler } from 'react-hook-form';
import { RootState, AppDispatch } from '../../store/store';
import { login } from '../../store/authSlice';
import { authService } from "../../services/authService";
import { registrationService } from "../../services/registrationService";
import { handleApiError } from '../../utils/apiErrorHandler';

interface LoginSignupProps {
  showLS: boolean;
  onClose: () => void;
}

interface FormData {
  firstName?: string;
  lastName?: string;
  email: string;
  password: string;
  general?: string;
}

const LoginSignup: React.FC<LoginSignupProps> = ({ showLS, onClose }) => {
  const dispatch = useDispatch<AppDispatch>();
  const loading = useSelector((state: RootState) => state.auth.loading);
  const error = useSelector((state: RootState) => state.auth.error);

  const { register, handleSubmit, formState: { errors, isValid }, setError, reset } = useForm<FormData>({
    mode: "onChange"
  });

  const [action, setAction] = React.useState("Sign In");

  useEffect(() => {
    reset();
  }, [action, reset]);

  const onSubmit: SubmitHandler<FormData> = async (data) => {
    try {
      if (action === "Sign In") {
        await dispatch(login({ email: data.email, password: data.password })).unwrap();
      } else if (action === "Sign Up") {
        await registrationService.register({
          firstName: data.firstName || '',
          lastName: data.lastName || '',
          email: data.email,
          password: data.password
        });
      }

      const token = authService.getToken();
      if (token) {
        onClose();
        window.location.reload();
      }
    } catch (error) {
      setError("general", {
        type: "manual",
        message: handleApiError(error, {
          unauthorized: "Invalid email or password.",
          notFound: "User not found.",
          conflict: "User already exists.",
          badRequest: "Invalid request. Please check your input."
        })
      });
    }
  };

  const handleModalClick = useCallback(() => {
    if (window.getSelection()?.toString()) {
      return;
    }
    onClose();
  }, [onClose]);

  if (!showLS) return null;

  return (
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

        <form className="d-flex flex-column gap-3" onSubmit={handleSubmit(onSubmit)}>
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
                    {...register("firstName", { required: "First name is required." })}
                    autoComplete="given-name"
                  />
                </div>
                {errors.firstName && (
                  <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                    {errors.firstName.message}
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
                    {...register("lastName", { required: "Last name is required." })}
                    autoComplete="family-name"
                  />
                </div>
                {errors.lastName && (
                  <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                    {errors.lastName.message}
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
                {...register("email", {
                  required: "Email is required.",
                  pattern: {
                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                    message: "Invalid email format."
                  }
                })}
                autoComplete="email"
              />
            </div>
            {errors.email && (
              <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                {errors.email.message}
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
                {...register("password", {
                  required: "Password is required.",
                  minLength: {
                    value: 6,
                    message: "Password must be at least 6 characters."
                  }
                })}
                autoComplete={action === "Sign In" ? "current-password" : "new-password"}
              />
            </div>
            {errors.password && (
              <small className="text-danger position-absolute start-0 bottom-0 translate-y-100 px-5">
                {errors.password.message}
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
              {errors.general.message}
            </div>
          )}

          <button
            type="submit"
            className="submit-button"
            disabled={!isValid}
          >
            {action === "Sign In" ? "SIGN IN" : "SIGN UP"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginSignup;