import { useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { profileService } from "../../services/profileService";
import "./assets/profile.css";

interface UserInfo {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  shouldHidePhoneNumber: boolean;
  description: string;
}

const Profile = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [userInfo, setUserInfo] = useState<UserInfo>({
    userId: 0,
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: null,
    shouldHidePhoneNumber: true,
    description: "",
  });

  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  const decodeToken = (token: string): string | undefined => {
    try {
      const payload = token.split(".")[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (err) {
      console.error("Failed to decode token:", err);
    }
  };

  const validateField = (field: string, value: string) => {
    let error = "";
    if (field === "firstName" && !value.trim()) {
      error = "First name is required.";
    } else if (field === "lastName" && !value.trim()) {
      error = "Last name is required.";
    } else if (field === "email") {
      const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
      if (!value.trim()) {
        error = "Email is required.";
      } else if (!emailRegex.test(value)) {
        error = "Invalid email format.";
      }
    } else if (field === "phoneNumber" && value) {
      if (!/^\d{10,15}$/.test(value)) {
        error = "Phone number must contain only digits (10-15 characters).";
      }
    } else if (field === "description" && value.length > 500) {
      error = "Description cannot exceed 500 characters.";
    }

    setErrors((prev) => ({ ...prev, [field]: error }));
  };

  const handleInputChange = (field: keyof UserInfo, value: string) => {
    setUserInfo((prev) => ({ ...prev, [field]: value }));
    validateField(field, value);
  };

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const decoded = decodeToken(token);
      if (decoded) {
        profileService
          .fetchUserData()
          .then((data) => setUserInfo(data))
          .catch((error) => {
            console.error("Failed to load user data", error);
            setErrors({ general: error.message });
          });
      }
    }
  }, []);

  const handleSave = async () => {
    const hasErrors = Object.values(errors).some((error) => error);
    if (hasErrors) return;

    const token = localStorage.getItem("token");
    if (!token) {
      setErrors({ general: "You must be logged in" });
      return;
    }

    try {
      await profileService.updateProfile({
        userId: userInfo.userId,
        firstName: userInfo.firstName,
        lastName: userInfo.lastName,
        email: userInfo.email,
        phoneNumber: userInfo.phoneNumber || "",
        userStatusId: 1,
        shouldHidePhoneNumber: userInfo.shouldHidePhoneNumber,
        description: userInfo.description,
        userRole: "USER",
      });
      setIsEditing(false);
    } catch (error) {
      setErrors({
        general: error instanceof Error ? error.message : "Update failed. Please try again.",
      });
    }
  };

  const handleFormSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (isEditing) {
      handleSave();
    } else {
      setIsEditing(true);
    }
  };

  return (
    <div className="profile-container">
      <div className="profile-content">
        <div className="profile-card">
          <form onSubmit={handleFormSubmit}>
            <div className="fs-2 fw-bold mb-5 text-white">Profile</div>
            <div className="row mt-2">
              <div className="col-md-4 d-flex justify-content-center align-items-start">
                <div className="profile-avatar">
                  <i className="bi bi-person-circle"></i>
                </div>
              </div>
              <div className="col-md-8">
                <div className="row">
                  <div className="col-md-6">
                    <div className="form-group mb-4">
                      <label className="profile-label">First Name:</label>
                      {isEditing ? (
                        <>
                          <input
                            type="text"
                            className="form-control profile-input"
                            value={userInfo.firstName}
                            onChange={(e) =>
                              handleInputChange("firstName", e.target.value)
                            }
                          />
                          {errors.firstName && (
                            <div className="text-danger mt-1">{errors.firstName}</div>
                          )}
                        </>
                      ) : (
                        <p className="profile-text">{userInfo.firstName}</p>
                      )}
                    </div>
                    <div className="form-group mb-4">
                      <label className="profile-label">Last Name:</label>
                      {isEditing ? (
                        <>
                          <input
                            type="text"
                            className="form-control profile-input"
                            value={userInfo.lastName}
                            onChange={(e) =>
                              handleInputChange("lastName", e.target.value)
                            }
                          />
                          {errors.lastName && (
                            <div className="text-danger mt-1">{errors.lastName}</div>
                          )}
                        </>
                      ) : (
                        <p className="profile-text">{userInfo.lastName}</p>
                      )}
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="form-group mb-4">
                      <label className="profile-label">Email:</label>
                      {isEditing ? (
                        <>
                          <input
                            type="email"
                            className="form-control profile-input"
                            value={userInfo.email}
                            onChange={(e) =>
                              handleInputChange("email", e.target.value)
                            }
                          />
                          {errors.email && (
                            <div className="text-danger mt-1">{errors.email}</div>
                          )}
                        </>
                      ) : (
                        <p className="profile-text">{userInfo.email}</p>
                      )}
                    </div>
                    <div className="form-group mb-4">
                      <label className="profile-label">Phone Number:</label>
                      {isEditing ? (
                        <>
                          <input
                            type="text"
                            className="form-control profile-input"
                            value={userInfo.phoneNumber || ""}
                            onChange={(e) =>
                              handleInputChange("phoneNumber", e.target.value)
                            }
                          />
                          <div className="form-check mt-2">
                            <input
                              type="checkbox"
                              className="form-check-input"
                              checked={userInfo.shouldHidePhoneNumber}
                              onChange={(e) =>
                                setUserInfo((prev) => ({
                                  ...prev,
                                  shouldHidePhoneNumber: e.target.checked,
                                }))
                              }
                            />
                            <label className="form-check-label">
                              Hide Phone Number
                            </label>
                          </div>
                          {errors.phoneNumber && (
                            <div className="text-danger mt-1">{errors.phoneNumber}</div>
                          )}
                        </>
                      ) : (
                        <p className="profile-text">{userInfo.phoneNumber || <p className="text-opacity-25 text-white">No number</p>}</p>
                      )}
                    </div>
                  </div>
                </div>
                <div className="form-group mb-4">
                  <label className="profile-label">About me:</label>
                  {isEditing ? (
                    <>
                      <textarea
                        className="form-control profile-input"
                        value={userInfo.description}
                        rows={4}
                        onChange={(e) =>
                          handleInputChange("description", e.target.value)
                        }
                      />
                      {errors.description && (
                        <div className="text-danger mt-1">{errors.description}</div>
                      )}
                    </>
                  ) : (
                    <p className="profile-text">{userInfo.description || <p className="text-opacity-25 text-white">No description</p>}</p>
                  )}
                </div>
              </div>
            </div>
            <div className="text-end mt-4">
              <button type="submit" className="profile-button">
                {isEditing ? (
                  <>
                    <i className="bi bi-check-circle me-2"></i>
                    Save Changes
                  </>
                ) : (
                  <>
                    <i className="bi bi-pencil me-2"></i>
                    Edit Profile
                  </>
                )}
              </button>
            </div>
            {errors.general && (
              <div className="alert alert-danger mt-3">{errors.general}</div>
            )}
          </form>
        </div>
      </div>
    </div>
  );
};

export default Profile;
