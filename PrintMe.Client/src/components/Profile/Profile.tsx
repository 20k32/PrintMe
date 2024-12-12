import { useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import profileIcon from "./assets/images/image.png";
import { profileService } from "../../services/profileService";
import backgroundImage from "./assets/images/background.png";


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

  const decodeToken = (token: string): any => {
    try {
      const payload = token.split(".")[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (err) {
      console.error("Failed to decode token:", err);
      return null;
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

  // Завантаження даних користувача
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const decoded = decodeToken(token);
      if (decoded) {
        profileService
          .fetchUserData(token)
          .then((data) => setUserInfo(data))
          .catch((error) => {
            console.error("Failed to load user data", error);
            setErrors({ general: "Failed to load user data" });
          });
      }
    }
  }, []);


  const handleSave = async () => {
    const hasErrors = Object.values(errors).some((error) => error);
    if (hasErrors) return;

    const token = localStorage.getItem("token");
    if (!token) {
      setErrors({ general: "Token not found" });
      return;
    }

    try {
      const updatedProfile = await profileService.updateProfile(
        {
          userId: userInfo.userId,
          firstName: userInfo.firstName,
          lastName: userInfo.lastName,
          email: userInfo.email,
          phoneNumber: userInfo.phoneNumber || "",
          userStatusId: 1,
          shouldHidePhoneNumber: userInfo.shouldHidePhoneNumber,
          description: userInfo.description,
          userRole: "USER",
        },
        token
      );
      console.log("Profile updated successfully:", updatedProfile);
      setIsEditing(false);
    } catch (error) {
      console.error("Update failed:", error);
      setErrors({
        general: (error as Error).message || "Update failed. Please try again.",
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
    <div className="container-fluid d-flex justify-content-center align-items-center min-vh-100" style={{backgroundImage: `url(${backgroundImage})`}}>
      <div className="card shadow-lg p-5 " style={{ height: "1000px", width: "1500px", borderRadius: "25px" }}>
        <form onSubmit={handleFormSubmit} className="d-flex flex-column flex-grow-1">
          <div className="fs-2 fw-bold mb-5">
            PROFILE
          </div>
          <div className="row mt-2">
            {/* Image Section */}
            <div className="col-md-4 d-flex justify-content-center">
          <img 
            src={profileIcon} 
            alt="Profile Icon" 
            className="img-fluid" 
            style={{ width: "150px", height: "150px"}} // Фіксований розмір
          />
        </div>
            {/* Info Section */}
            <div className="col-md-8 fs-3">
              <div className="row">
                {/* Left Info */}
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="fw-bold"><span>First Name:</span></label>
                    {isEditing ? (
                      <>
                    <input
                      type="text"
                      className="form-control w-75"
                      value={userInfo.firstName}
                      onChange={(e) => handleInputChange("firstName", e.target.value)}
                    />
                        {errors.firstName && (
                          <div className="text-danger">{errors.firstName}</div>
                        )}
                      </>
                    ) : (
                      <p>{userInfo.firstName}</p>
                    )}
                  </div>
                  <div className="mb-3">
                    <label className="fw-bold"><span>Last Name:</span></label>
                    {isEditing ? (
                      <>
                    <input
                      type="text"
                      className="form-control w-75"
                      value={userInfo.lastName}
                      onChange={(e) => handleInputChange("lastName", e.target.value)}
                    />
                        {errors.lastName && (
                          <div className="text-danger">{errors.lastName}</div>
                        )}
                      </>
                    ) : (
                      <p>{userInfo.lastName}</p>
                    )}
                  </div>
                </div>
                {/* Right Info */}
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="fw-bold"><span>Email:</span></label>
                    {isEditing ? (
                      <>
                        <input
                          type="email"
                          className="form-control w-75"
                          value={userInfo.email}
                          onChange={(e) => handleInputChange("email", e.target.value)}
                        />
                        {errors.email && (
                          <div className="text-danger">{errors.email}</div>
                        )}
                      </>
                    ) : (
                      <p>{userInfo.email}</p>
                    )}
                  </div>
                  <div className="mb-3">
                    <label className="fw-bold"><span>Phone Number:</span></label>
                    {isEditing ? (
                      <>
                        <div className="form-check">
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
                          <label className="form-check-label fs-5">Hide Phone Number</label>
                        </div>
                        <input
                          type="text"
                          className="form-control w-75"
                          value={userInfo.phoneNumber || ""}
                          onChange={(e) => handleInputChange("phoneNumber", e.target.value)}
                        />
                        {errors.phoneNumber && (
                          <div className="text-danger">{errors.phoneNumber}</div>
                        )}
                      </>
                    ) : (
                      <p>{userInfo.phoneNumber}</p>
                    )}
                  </div>
                </div>
              </div>
              <div className="mb-3">
                <label className="fw-bold"><span>About me:</span></label>
                {isEditing ? (
                  <>
                    <textarea
                      className="form-control w-75"
                      value={userInfo.description}
                      onChange={(e) => handleInputChange("description", e.target.value)}
                    />
                    {errors.description && (
                      <div className="text-danger">{errors.description}</div>
                    )}
                  </>
                ) : (
                  <p>{userInfo.description || "No description"}</p>
                )}
              </div>
            </div>
          </div>
          <div className="mt-auto text-end ">
          <button type="submit" className="btn btn-primary w-20 fs-3 bg" style={{ backgroundColor: "#6c30f3", width: "200px" }}>
              {isEditing ? "Save" : "Edit profile"}
            </button>
          </div>
          {errors.general && (
            <div className="text-danger text-center mt-3">{errors.general}</div>
          )}
        </form>
      </div>
    </div>
  );
};

export default Profile;