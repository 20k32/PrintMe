import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom"; // Import useNavigate
import "bootstrap/dist/css/bootstrap.min.css";
import { userService } from "../../services/userService";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./assets/profile.css";

interface UserInfo {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  shouldHidePhoneNumber: boolean;
  description: string;
  isVerified: boolean;
}

const UserProfile: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate(); // Initialize useNavigate
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    if (userId) {
      userService
        .getUserProfile(Number(userId))
        .then((data) => setUserInfo(data))
        .catch((error) => {
          console.error("Failed to load user data", error);
          setErrors({ general: error.message });
        });
    }
  }, [userId]);

  if (!userInfo) {
    return <div>Loading...</div>;
  }

  return (
    <div className="profile-container">
      <ToastContainer />
      <div className="profile-content">
        <div className="profile-card">
        <div className="container">
        <div className="back-arrow">
          <i
            className="bi bi-arrow-left"
            onClick={() => navigate(-1)} // Navigate back to the previous page
          ></i>
          <h2>Profile</h2>
        </div>
        </div>
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
                    <p className="profile-text">{userInfo.firstName}</p>
                  </div>
                  <div className="form-group mb-4">
                    <label className="profile-label">Last Name:</label>
                    <p className="profile-text">{userInfo.lastName}</p>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="form-group mb-4">
                    <label className="profile-label">Email:</label>
                    <p className="profile-text">{userInfo.email}</p>
                    {userInfo.isVerified ? (
                      <p className="text-success mt-2">Email Verified</p>
                    ) : (
                      <p className="text-danger mt-2">Email Not Verified</p>
                    )}
                  </div>
                  <div className="form-group mb-4">
                    <label className="profile-label">Phone Number:</label>
                    {userInfo.shouldHidePhoneNumber ? (
                      <p className="profile-text">User has hidden their phone number</p>
                    ) : (
                      <p className="profile-text">{userInfo.phoneNumber || "No number"}</p>
                    )}
                  </div>
                </div>
              </div>
              <div className="form-group mb-4">
                <label className="profile-label">About me:</label>
                <p className="profile-text">{userInfo.description || "No description"}</p>
              </div>
            </div>
          </div>
          {errors.general && (
            <div className="alert alert-danger mt-3">{errors.general}</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default UserProfile;