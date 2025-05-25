// TO DO

import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link } from "react-router-dom";
import { saveTicket,fetchTicket } from "../hooks/tickets";
import { fetchUser } from "../hooks/users";
import { useCheckTokenValidity } from "../global";

function TicketDetailsEdit() {
  const { ticketId } = useParams();
  const checkToken = useCheckTokenValidity();
  const [isLoading, setIsLoading] = useState(true);
  const [ticket, setTicket] = useState();

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

  const displayTickets = async () => {
    try {
      const ticketData = await fetchTicket(ticketId);
      console.log(ticketData);

      if (ticketData === null) {
        throw new Error("error fetching ticket");
      }

      const userIds = [
        ...new Set(
          [ticketData.requesterId, ticketData.assigneeId].filter(
            (id) => id !== null
          )
        ),
      ];

      const userResponses = await Promise.all(
        userIds.map((id) => fetchUser(id))
      );
      setName(ticketData.name);
      setDescription(ticketData.description);
      setCreationDate(ticketData.creationDate);
      setSolutionDate(ticketData.solutionDate);
      setSolutionDescription(ticketData.solutiondescription);
      setPriority(ticketData.priority);
      setType(ticketData.type);
      setStatus(ticketData.status);
      setServiceId(ticketData.serviceId);
      setAssigneeId(ticketData.assigneeId);
      setRequesterId(ticketData.requesterId);
      const userData = await Promise.all(userResponses.map((res) => res));

      const userMap = {};
      userData.forEach((user) => {
        userMap[user.id] = `${user.name} ${user.surname}`;
      });

      const enrichedTicket = {
        ...ticketData,
        requesterName: userMap[ticketData.requesterId] ?? "—",
        assigneeName: userMap[ticketData.assigneeId] ?? "—",
      };

      setTicket(enrichedTicket);
      setIsLoading(false);

      
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
    
  };

 const handleNameChange = (event) => {
    setName(event.target.value);
  };

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value);
  };

  const handleSolutionDescriptionChange = (event) => {
    setSolutionDescription(event.target.value);
  };

  const handlePriorityChange = (event) => {
    setPriority(event.target.value);
  };

  const handleTypeChange = (event) => {
    setType(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
  };

  const handleServiceIdChange = (event) => {
    setServiceId(event.target.value);
  };
  const handleSolutionDateChange = (event) => {
    setSolutionDate(event.target.value);
  };
  const handleCreationDateChange = (event) => {
    setCreationDate(event.target.value);
  };
  const handleAsigneeChange = (event) => {
    setAssigneeId(event.target.value);
  };
  const handleRequesterChange = (event) => {
    setRequesterId(event.target.value);
  };
  const saveRecord = async () => {
    const serviceData = {
      name: name,
      description: description,
      creationDate: creationDate,
      solutionDate: solutionDate,
      solutionDescription: solutionDescription,
      priority: priority,
      type: type,
      status: status,
      serviceId: serviceId,
      assigneeId: assigneeId,
      requesterId: requesterId,
    };
    const success = await saveTicket(serviceData);
    if (success) {
      setError(false);
      navigate(`/tickets/${ticketId}`);
    } else {
      setError(true);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayTickets();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {() => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Edit ticket</h1>
                <button className="edit-button" onClick={saveRecord}>
                  Save
                </button>
              </div>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <input className="record-value-edit" 
                  value={name} 
                  onChange={handleNameChange} />
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <textarea className="record-value-edit" 
                  value={description} 
                  onChange={handleDescriptionChange} />
                </div>
                <div className="record-field">
                  <span className="record-label">Created:</span>
                  <input className="record-value-edit" 
                  type="date" value={creationDate} 
                 onChange={handleCreationDateChange}/>
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Date:</span>
                  <input className="record-value-edit" 
                  type="date" 
                  value={solutionDate} 
                  onChange={handleSolutionDateChange}/>
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Description:</span>
                  <textarea className="record-value-edit" 
                  value={solutionDescription} 
                  onChange={handleSolutionDescriptionChange}/>
                </div>
                <div className="record-field">
                  <span className="record-label">Priority:</span>
                  <input className="record-value-edit"
                   type="number" 
                   value={priority} 
                  onChange={handlePriorityChange} />
                </div>
                <div className="record-field">
                  <span className="record-label">Type:</span>
                  <select className="record-value-edit" 
                  value={type} 
                  onChange={handleTypeChange}>
                    <option value="Bug">Bug</option>
                    <option value="Performance">Performance</option>
                    <option value="Support">Support</option>
                    <option value="Feature Request">Feature Request</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <select className="record-value-edit" 
                  value={status}
                  onChange={handleStatusChange}>
                    <option value="Open">Open</option>
                    <option value="In Progress">In Progress</option>
                    <option value="Closed">Closed</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Service ID:</span>
                  <input className="record-value-edit" 
                  type="number" 
                  value={serviceId} 
                  onChange={handleServiceIdChange} />
                </div>
                <div className="record-field">
                  <span className="record-label">Assignee:</span>
                  <input className="record-value-edit" 
                  type="number"
                  value={assigneeId} 
                  onChange={handleAsigneeChange}/>
                </div>
                <div className="record-field">
                  <span className="record-label">Requester:</span>
                  <select className="record-value-edit" 
                  value={requesterId} 
                  onChange={handleRequesterChange}>
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

export default TicketDetailsEdit;
