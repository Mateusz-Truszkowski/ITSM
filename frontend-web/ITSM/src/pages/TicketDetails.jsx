import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanelDetails from "../components/MainPanelDetails";

function TicketDetails() {
  const { ticketId } = useParams();
  const [ticket, setTicket] = useState(null);
  const token = localStorage.getItem("authToken");

  useEffect(() => {
    const fetchTicketDetails = async () => {
      try {
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

        if (response.ok) {
          const data = await response.json();
          setTicket(data);
        } else {
          console.log("Błąd pobierania szczegółów ticketu");
        }
      } catch (error) {
        console.error("Błąd:", error);
      }
    };

    fetchTicketDetails();
  }, [ticketId, token]);

  return (
    <>
      <NavigationLP />
      <MainPanelDetails>
        <div>
          <h1>Ticket Details</h1>
          <p>
            <strong>ID:</strong> {ticket.id}
          </p>
          <p>
            <strong>Name:</strong> {ticket.name}
          </p>
          <p>
            <strong>Description:</strong> {ticket.description}
          </p>
          <p>
            <strong>Status:</strong> {ticket.status}
          </p>
          <p>
            <strong>Assignee:</strong> {ticket.assigneeName}
          </p>
          <p>
            <strong>Requester:</strong> {ticket.requesterName}
          </p>
          {/* Możesz dodać więcej informacji w zależności od struktury ticketu */}
        </div>
      </MainPanelDetails>
    </>
  );
}

export default TicketDetails;
