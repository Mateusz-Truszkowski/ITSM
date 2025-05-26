import React, { useState, useEffect } from "react";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { checkToken } from "../global";
import { createUser } from "../hooks/users";
import { useNavigate } from "react-router-dom";

function UserCreate() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);

  // Wymagane pola
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [email, setEmail] = useState("");
  const [group, setGroup] = useState("User");
  const [occupation, setOccupation] = useState("");
  const [status, setStatus] = useState("Active");
  const [error, setError] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);
    setIsLoading(false);
  }, []);

  const saveRecord = async () => {
    const userData = {
      login,
      password,
      name,
      surname,
      email,
      group,
      occupation,
      status,
      creationDate: new Date().toISOString(), // aktualna data
    };

    const success = await createUser(userData);
    if (success) {
      setError(false);
      navigate("/users");
    } else {
      setError(true);
    }
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {() =>
          isLoading ? (
            <div className="loading-spinner">
              <div className="spinner" />
            </div>
          ) : (
            <div className="record-details-wrapper">
              <div className="record-details-container">
                <div className="record-details-header">
                  <h1>Create new user</h1>
                  <button className="edit-button" onClick={saveRecord}>
                    Save
                  </button>
                </div>
                <div className="record-fields">
                  <div className="record-field">
                    <span className="record-label">Login:</span>
                    <input
                      className="record-value-edit"
                      value={login}
                      onChange={(e) => setLogin(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Password:</span>
                    <input
                      className="record-value-edit"
                      type="password"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Name:</span>
                    <input
                      className="record-value-edit"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Surname:</span>
                    <input
                      className="record-value-edit"
                      value={surname}
                      onChange={(e) => setSurname(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Email:</span>
                    <input
                      className="record-value-edit"
                      type="email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Group:</span>
                    <select
                      className="record-value-edit"
                      value={group}
                      onChange={(e) => setGroup(e.target.value)}
                    >
                      <option value="Admin">Admin</option>
                      <option value="Operator">Operator</option>
                      <option value="User">User</option>
                    </select>
                  </div>
                  <div className="record-field">
                    <span className="record-label">Occupation:</span>
                    <input
                      className="record-value-edit"
                      value={occupation}
                      onChange={(e) => setOccupation(e.target.value)}
                    />
                  </div>
                  <div className="record-field">
                    <span className="record-label">Status:</span>
                    <select
                      className="record-value-edit"
                      value={status}
                      onChange={(e) => setStatus(e.target.value)}
                    >
                      <option value="Active">Active</option>
                      <option value="Inactive">Inactive</option>
                    </select>
                  </div>
                </div>
                {error && (
                  <div className="failed-message">
                    <p>!</p>
                    <span>Error occurred while saving the user</span>
                  </div>
                )}
              </div>
            </div>
          )
        }
      </MainPanel>
    </>
  );
}

export default UserCreate;
