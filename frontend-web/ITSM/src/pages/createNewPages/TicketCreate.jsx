import React, { useState, useEffect } from "react";
import NavigationLP from "../../components/NavigationLP";
import MainPanel from "../../components/MainPanel";
import "../assets/RecordDetails.css";
import { useCheckTokenValidity } from "../../global";
import { createTicket } from "../../hooks/tickets";
import { fetchUsers } from "../../hooks/users";
import { useNavigate } from "react-router-dom";

function TicketCreate() {
  const checkToken = useCheckTokenValidity();
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [creationDate, setCreationDate] = useState("");
  const [solutionDate, setSolutionDate] = useState("");
  const [solutionDescription, setSolutionDescription] = useState("");
  const [priority, setPriority] = useState("");
  const [type, setType] = useState("Bug");
  const [status, setStatus] = useState("Open");
  const [serviceId, setServiceId] = useState("");
  const [assigneeId, setAssigneeId] = useState("");
  const [requesterId, setRequesterId] = useState("");
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    const loadUsers = async () => {
      const data = await fetchUsers();
      setUsers(data);
    };

    loadUsers();
  }, []);

  const saveRecord = async () => {
    // Walidacja minimalna
    if (!name || !description || !creationDate || !priority || !type || !status || !serviceId || !assigneeId || !requesterId) {
      alert("Please fill in all required fields.");
      return;
    }

    const ticketData = {
      name,
      description,
      creationDate: new Date(creationDate).toISOString(),
      solutionDate: solutionDate ? new Date(solutionDate).toISOString() : null,
      solutionDescription,
      priority: parseInt(priority),
      type,
      status,
      serviceId: parseInt(serviceId),
      assigneeId: parseInt(assigneeId),
      requesterId: parseInt(requesterId),
    };

    const success = await createTicket(ticketData);
    if (success) {
      setError(false);
      navigate("/tickets");
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
                <h1>Create new ticket</h1>
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
                  <span className="record-label">Created:</span>
                  <input className="record-value-edit" type="date" value={creationDate} onChange={(e) => setCreationDate(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Date:</span>
                  <input className="record-value-edit" type="date" value={solutionDate} onChange={(e) => setSolutionDate(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Description:</span>
                  <textarea className="record-value-edit" value={solutionDescription} onChange={(e) => setSolutionDescription(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Priority:</span>
                  <input className="record-value-edit" type="number" value={priority} onChange={(e) => setPriority(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Type:</span>
                  <select className="record-value-edit" value={type} onChange={(e) => setType(e.target.value)}>
                    <option value="Bug">Bug</option>
                    <option value="Performance">Performance</option>
                    <option value="Support">Support</option>
                    <option value="Feature Request">Feature Request</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <select className="record-value-edit" value={status} onChange={(e) => setStatus(e.target.value)}>
                    <option value="Open">Open</option>
                    <option value="In Progress">In Progress</option>
                    <option value="Closed">Closed</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Service ID:</span>
                  <input className="record-value-edit" type="number" value={serviceId} onChange={(e) => setServiceId(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Assignee:</span>
                  <input className="record-value-edit" type="number" value={assigneeId} onChange={(e) => setAssigneeId(e.target.value)} />
                </div>
                <div className="record-field">
                  <span className="record-label">Requester:</span>
                  <select className="record-value-edit" value={requesterId} onChange={(e) => setRequesterId(e.target.value)}>
                    <option value="">Select requester</option>
                    {users.map((u) => (
                      <option key={u.id} value={u.id}>
                        {u.name} {u.surname}
                      </option>
                    ))}
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

export default TicketCreate;
