// TO DO

import React from "react";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchUser } from "../hooks/users";
import { useCheckTokenValidity } from "../global";

function UserDetailsEdit() {
  const [user, setUser] = useState();
  const { userId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const checkToken = useCheckTokenValidity();

  const displayUser = async () => {
    try {
      const userData = await fetchUser(userId);

      if (userData === null) {
        throw new Error("error fetching user");
      }

      setUser(userData);
      setIsLoading(false);
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayUser();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <h1 className="record-details-header">User Details</h1>
              {isLoading ? (
                <div className="loading-spinner">
                  <div className="spinner"></div>
                </div>
              ) : (
                <>
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
                </>
              )}
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default UserDetailsEdit;
