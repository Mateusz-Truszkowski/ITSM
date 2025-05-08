import React, { useState, useEffect } from "react";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { useCheckTokenValidity } from "../global";
import { createDevice } from "../hooks/devices";
import { fetchUsers } from "../hooks/users";
import { useNavigate } from "react-router-dom";

function DeviceCreate() {
  const checkToken = useCheckTokenValidity();
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [acquisitionDate, setAcquisitionDate] = useState("");
  const [depreciationDate, setDepreciationDate] = useState("");
  const [userId, setUserId] = useState("");
  const [status, setStatus] = useState("Active");
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    const loadUsers = async () => {
      const data = await fetchUsers();
      setUsers(data);
      setIsLoading(false);
    };
    loadUsers();
  }, []);

  const saveRecord = async () => {
    const deviceData = {
      name,
      description,
      acquisitionDate: new Date(acquisitionDate).toISOString(),
      depreciationDate: new Date(depreciationDate).toISOString(),
      UserId: userId,
      status,
    };

    const success = await createDevice(deviceData);
    if (success) {
      setError(false);
      navigate("/devices");
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
                <h1>Create new device</h1>
                <button className="edit-button" onClick={saveRecord}>
                  Save
                </button>
              </div>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <input className="record-value-edit" value={name} onChange={(e) => setName(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <textarea className="record-value-edit" value={description} onChange={(e) => setDescription(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Acquisition Date:</span>
                  <input className="record-value-edit" type="date" value={acquisitionDate} onChange={(e) => setAcquisitionDate(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Depreciation Date:</span>
                  <input className="record-value-edit" type="date" value={depreciationDate} onChange={(e) => setDepreciationDate(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Owner:</span>
                  <select className="record-value-edit" value={userId} onChange={(e) => setUserId(e.target.value)}>
                    <option value="">Select user</option>
                    {users.map((u) => (
                      <option key={u.id} value={u.id}>
                        {u.name} {u.surname}
                      </option>
                    ))}
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <select className="record-value-edit" value={status} onChange={(e) => setStatus(e.target.value)}>
                    <option value="Active">Active</option>
                    <option value="Inactive">Inactive</option>
                  </select>
                </div>
              </div>
              {error && (
                <div className="failed-message">
                  <p>!</p>
                  <span>Error occurred</span>
                </div>
              )}
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default DeviceCreate;
