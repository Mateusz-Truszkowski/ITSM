import React from "react";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { fetchUser, deleteUser } from "../hooks/users";
import { useCheckTokenValidity } from "../global";

function UserDetails() {
  const [user, setUser] = useState();
  const { userId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const checkToken = useCheckTokenValidity();
  const navigate = useNavigate();

  const displayUser = async () => {
    try {
      const userData = await fetchUser(userId);

      if (!userData) {
        throw new Error("Brak danych użytkownika");
      }

      setUser(userData);
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);
    displayUser();
  
  }, []);
  const editRecord = () => {
    navigate(`/users/${userId}/edit`);
  };
  const deleteRecord = async () => {
    await deleteUser(userId);
    navigate(`/users`);
  }

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {() => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>User Details</h1>
                <div className="Button-container">
                  <button className="delete-button" onClick={deleteRecord}>
                    delete
                  </button>
                  <button className="edit-button" onClick={editRecord}>
                    Edit
                  </button>
                </div>
              </div>
              {isLoading ? (
                <div className="loading-spinner">
                  <div className="spinner"></div>
                </div>
              ) : (
                <div className="record-fields">
                  <div className="record-field">
                    <span className="record-label">Name:</span>
                    <span className="record-value">{user.name}</span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Surname:</span>
                    <span className="record-value">{user.surname}</span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Email:</span>
                    <span className="record-value">{user.email}</span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Created:</span>
                    <span className="record-value">
                      {new Date(user.creationDate).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Group:</span>
                    <span className="record-value">{user.group}</span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Occupation:</span>
                    <span className="record-value">{user.occupation}</span>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Status:</span>
                    <span className="record-value">{user.status}</span>
                  </div>
                </div>
              )}
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default UserDetails;
