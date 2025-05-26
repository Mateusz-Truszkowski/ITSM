import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { saveTicket, fetchTicket } from "../hooks/tickets";
import { fetchUser, fetchUsers } from "../hooks/users";
import { fetchServices } from "../hooks/services";
import { checkToken } from "../global";

function TicketDetailsEdit() {
  const { ticketId } = useParams();

  const [isLoading, setIsLoading] = useState(true);
  const [ticket, setTicket] = useState();
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
  const [services, setServices] = useState([]);
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
      setCreationDate(ticketData.creationDate?.slice(0, 10));
      setSolutionDate(ticketData.solutionDate?.slice(0, 10));
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
    const ticketData = {
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
    const success = await saveTicket(ticketData);
    if (success) {
      setError(false);
      navigate(`/tickets/${ticketId}`);
    } else {
      setError(true);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    displayTickets();
    const getServices = async () => {
      const fetchedServices = await fetchServices();
      setServices(fetchedServices);
    };
    getServices();

    const getUsers = async () => {
      const fetchedUsers = await fetchUsers();
      setUsers(fetchedUsers);
    };
    getUsers();
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
                  <input
                    className="record-value-edit"
                    value={name}
                    onChange={handleNameChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <textarea
                    className="record-value-edit"
                    value={description}
                    onChange={handleDescriptionChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Created:</span>
                  <input
                    className="record-value-edit"
                    type="date"
                    value={creationDate}
                    onChange={handleCreationDateChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Date:</span>
                  <input
                    className="record-value-edit"
                    type="date"
                    value={solutionDate}
                    onChange={handleSolutionDateChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Description:</span>
                  <textarea
                    className="record-value-edit"
                    value={solutionDescription}
                    onChange={handleSolutionDescriptionChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Priority:</span>
                  <input
                    className="record-value-edit"
                    type="number"
                    value={priority}
                    onChange={handlePriorityChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Type:</span>
                  <select
                    className="record-value-edit"
                    value={type}
                    onChange={handleTypeChange}
                  >
                    <option value="Bug">Bug</option>
                    <option value="Performance">Performance</option>
                    <option value="Support">Support</option>
                    <option value="Feature Request">Feature Request</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <select
                    className="record-value-edit"
                    value={status}
                    onChange={handleStatusChange}
                  >
                    <option value="Open">Open</option>
                    <option value="In Progress">In Progress</option>
                    <option value="Closed">Closed</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Service ID:</span>
                  <input
                    className="record-value-edit"
                    type="number"
                    value={serviceId}
                    onChange={handleServiceIdChange}
                  />
                  <span className="record-label">Service:</span>
                  <select
                    className="record-value-edit"
                    value={serviceId}
                    onChange={handleServiceIdChange}
                  >
                    <option value="">Select service</option>
                    {services.map((service) => (
                      <option key={service.id} value={service.id}>
                        {service.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="record-field">
                  <span className="record-label">Assignee:</span>
                  <input
                    className="record-value-edit"
                    type="number"
                    value={assigneeId}
                    onChange={handleAsigneeChange}
                  />
                </div>
                <div className="record-field">
                  <span className="record-label">Requester:</span>
                  <select
                    className="record-value-edit"
                    value={requesterId}
                    onChange={handleRequesterChange}
                  >
                    <option value="">Select requester</option>
                    {users.map((user) => (
                      <option key={user.id} value={user.id}>
                        {user.name + " " + user.surname}
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
