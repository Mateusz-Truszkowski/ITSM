import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link } from "react-router-dom";

function TicketDetails() {
  const { ticketId } = useParams();

  const [tickets, setTickets] = useState([]);

  const displayTickets = async () => {
    try {
      const token = localStorage.getItem("authToken");

      const response = await fetch(
        `https://localhost:63728/tickets/${ticketId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!response.ok) {
        console.log("Błąd podczas pobierania ticketa", response.status);
        return;
      }

      const ticketData = await response.json();

      // Pobierz unikalne ID użytkowników
      const userIds = [
        ...new Set(
          [ticketData.requesterId, ticketData.assigneeId].filter(
            (id) => id !== null
          )
        ),
      ];

      // Pobierz dane użytkowników
      const userResponses = await Promise.all(
        userIds.map((id) =>
          fetch(`https://localhost:63728/users/${id}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          })
        )
      );

      const userData = await Promise.all(
        userResponses.map((res) => res.json())
      );

      const userMap = {};
      userData.forEach((user) => {
        userMap[user.id] = `${user.name} ${user.surname}`;
      });

      // Wzbogacenie ticketa
      const enrichedTicket = {
        ...ticketData,
        requesterName: userMap[ticketData.requesterId] ?? "—",
        assigneeName: userMap[ticketData.assigneeId] ?? "—",
      };

      setTickets(enrichedTicket); // teraz setTickets ustawia pojedyńczy obiekt
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  useEffect(() => {
    displayTickets();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <h1 className="record-details-header">Ticket Details</h1>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">ID:</span>
                  <span className="record-value">
                    {tickets ? tickets.id : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <span className="record-value">
                    {tickets ? tickets.name : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <span className="record-value">
                    {tickets ? tickets.description : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Created:</span>
                  <span className="record-value">
                    {tickets && tickets.creationDate
                      ? new Date(tickets.creationDate).toLocaleDateString()
                      : ""}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Date:</span>
                  <span className="record-value">
                    {tickets && tickets.solutiondate
                      ? new Date(tickets.solutiondate).toLocaleDateString()
                      : ""}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Description:</span>
                  <span className="record-value">
                    {tickets ? tickets.solutiondescription : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Priority:</span>
                  <span className="record-value">
                    {tickets ? tickets.priority : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Type:</span>
                  <span className="record-value">
                    {tickets ? tickets.type : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <span className="record-value">
                    {tickets ? tickets.status : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Service ID:</span>
                  <span className="record-value">
                    {tickets ? tickets.serviceId : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Assignee:</span>
                  <span className="record-value">
                    {tickets ? (
                      <Link to={`/users/${tickets.assigneeId}`}>
                        {tickets.assigneeName}
                      </Link>
                    ) : (
                      "Loading..."
                    )}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Requester:</span>
                  <span className="record-value">
                    {tickets ? (
                      <Link to={`/users/${tickets.requesterId}`}>
                        {tickets.requesterName}
                      </Link>
                    ) : (
                      "Loading..."
                    )}
                  </span>
                </div>
              </div>
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default TicketDetails;
