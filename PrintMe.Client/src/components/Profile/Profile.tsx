import { useState, useEffect } from "react";
import "./assets/css/profile.css";
import profileIcon from "./assets/images/image.png";
import { profileService } from "../../services/profileService";

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

  const validateFields = () => {
    const newErrors: { [key: string]: string } = {};
    if (!userInfo.firstName.trim()) {
      newErrors.firstName = "First name is required.";
    }
    if (!userInfo.lastName.trim()) {
      newErrors.lastName = "Last name is required.";
    }
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
    if (!userInfo.email.trim()) {
      newErrors.email = "Email is required.";
    } else if (!emailRegex.test(userInfo.email)) {
      newErrors.email = "Invalid email format.";
    }
    if (userInfo.phoneNumber && !/^\d{10,15}$/.test(userInfo.phoneNumber)) {
      newErrors.phoneNumber = "Phone number must contain only digits (10-15 characters).";
    }
    if (userInfo.description.length > 500) {
      newErrors.description = "Description cannot exceed 500 characters.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
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
    if (!validateFields()) return;

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
    <div className="main-content">
      <div className="profile-page-container">
        <form onSubmit={handleFormSubmit}>
          <div className="header">PROFILE</div>
          <div className="content-container">
            <div className="profile-image">
              <img src={profileIcon} alt="Profile Icon" />
            </div>
            <div className="profile-info">
              <div className="profile-main-info">
                <div className="left-info">
                  <div className="info-in">
                    <span>First Name:<br /></span>
                    {isEditing ? (
                      <>
                        <input
                          value={userInfo.firstName}
                          onChange={(e) =>
                            setUserInfo((prev) => ({
                              ...prev,
                              firstName: e.target.value,
                            }))
                          }
                        />
                        {errors.firstName && (
                          <div className="error-message">{errors.firstName}</div>
                        )}
                      </>
                    ) : (
                      <label>{userInfo.firstName}</label>
                    )}
                  </div>
                  <div className="info-in">
                    <span>Last Name:<br /></span>
                    {isEditing ? (
                      <>
                        <input
                          value={userInfo.lastName}
                          onChange={(e) =>
                            setUserInfo((prev) => ({
                              ...prev,
                              lastName: e.target.value,
                            }))
                          }
                        />
                        {errors.lastName && (
                          <div className="error-message">{errors.lastName}</div>
                        )}
                      </>
                    ) : (
                      <label>{userInfo.lastName}</label>
                    )}
                  </div>
                </div>
                <div className="right-info">
                  <div className="info-in">
                    <span>Email:<br /></span>
                    {isEditing ? (
                      <>
                        <input
                          type="email"
                          value={userInfo.email}
                          onChange={(e) =>
                            setUserInfo((prev) => ({
                              ...prev,
                              email: e.target.value,
                            }))
                          }
                        />
                        {errors.email && (
                          <div className="error-message">{errors.email}</div>
                        )}
                      </>
                    ) : (
                      <label>{userInfo.email}</label>
                    )}
                  </div>
                  <div className="info-in">
                    <span>Phone Number:<br /></span>
                    {isEditing ? (
                      <>
                        <input
                          type="checkbox"
                          checked={userInfo.shouldHidePhoneNumber}
                          onChange={(e) =>
                            setUserInfo((prev) => ({
                              ...prev,
                              shouldHidePhoneNumber: e.target.checked,
                            }))
                          }
                        />{" "}
                        Hide Phone Number
                        <br />
                        <input
                          type="text"
                          value={userInfo.phoneNumber || ""}
                          onChange={(e) =>
                            setUserInfo((prev) => ({
                              ...prev,
                              phoneNumber: e.target.value,
                            }))
                          }
                        />
                        {errors.phoneNumber && (
                          <div className="error-message">{errors.phoneNumber}</div>
                        )}
                      </>
                    ) : (
                      <label>{userInfo.phoneNumber}</label>
                    )}
                  </div>
                </div>
              </div>
              <div className="about-me">
                <span>About me:<br /></span>
                {isEditing ? (
                  <>
                    <textarea
                      value={userInfo.description}
                      onChange={(e) =>
                        setUserInfo((prev) => ({
                          ...prev,
                          description: e.target.value,
                        }))
                      }
                    />
                    {errors.description && (
                      <div className="error-message">{errors.description}</div>
                    )}
                  </>
                ) : (
                  <label>{userInfo.description || "No description"}</label>
                )}
              </div>
            </div>
          </div>
          <div className="submit-button">
            <button type="submit">
              {isEditing ? "Save" : "Edit profile"}
            </button>
          </div>
          {errors.general && (
            <div className="error-message">{errors.general}</div>
          )}
        </form>
      </div>
    </div>
  );
};

export default Profile;
