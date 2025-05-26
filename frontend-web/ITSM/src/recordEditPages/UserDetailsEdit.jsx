import React, { useEffect, useState } from "react";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { useParams, useNavigate } from "react-router-dom";
import { fetchUser, updateUser } from "../hooks/users";
import { checkToken } from "../global";

function UserDetailsEdit() {
  const { userId } = useParams();
  const navigate = useNavigate();

  const [user, setUser] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    const loadUser = async () => {
      const data = await fetchUser(userId);
      if (!data) {
        setError(true);
      } else {
        setUser(data);
      }
      setIsLoading(false);
    };

    loadUser();
  }, []);

  const handleChange = (field, value) => {
    setUser((prev) => ({ ...prev, [field]: value }));
  };

  const saveChanges = async () => {
    const success = await updateUser(userId, user);
    if (success) {
      navigate(`/users/${userId}`);
    } else {
      setError(true);
    }
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {() => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Edit User</h1>
                <button className="edit-button" onClick={saveChanges}>
                  Save
                </button>
              </div>
              {isLoading ? (
                <div className="loading-spinner">
                  <div className="spinner"></div>
                </div>
              ) : (
                <div className="record-fields">
                  <div className="record-field">
                    <span className="record-label">Name:</span>
                    <input
                      className="record-value-edit"
                      value={user.name || ""}
                      onChange={(e) => handleChange("name", e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Surname:</span>
                    <input
                      className="record-value-edit"
                      value={user.surname || ""}
                      onChange={(e) => handleChange("surname", e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Email:</span>
                    <input
                      className="record-value-edit"
                      type="email"
                      value={user.email || ""}
                      onChange={(e) => handleChange("email", e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Group:</span>
                    <input
                      className="record-value-edit"
                      value={user.group || ""}
                      onChange={(e) => handleChange("group", e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Occupation:</span>
                    <input
                      className="record-value-edit"
                      value={user.occupation || ""}
                      onChange={(e) =>
                        handleChange("occupation", e.target.value)
                      }
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Status:</span>
                    <select
                      className="record-value-edit"
                      value={user.status || ""}
                      onChange={(e) => handleChange("status", e.target.value)}
                    >
                      <option value="Active">Active</option>
                      <option value="Inactive">Inactive</option>
                    </select>
                  </div>
                </div>
              )}
              {error && (
                <div className="failed-message">
                  <p>!</p>
                  <span>Error saving user</span>
                </div>
              )}
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default UserDetailsEdit;
