import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";

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
      <MainPanel>
        {({ data, openRecord, isLoading }) => (
          <div>{
            <div>
              <h1>Ticket Details</h1>
              <p>
                <strong>ID:</strong> {ticket ? ticket.id : "Loading..."}
              </p>
              <p>
                <strong>Name:</strong> {ticket ? ticket.name : "Loading..."}
              </p>
              <p>
                <strong>Description:</strong> {ticket ? ticket.description : "Loading..."}
              </p>
              <p>
                <strong>Status:</strong> {ticket ? ticket.status : "Loading..."}
              </p>
              <p>
                <strong>Assignee:</strong> {ticket ? ticket.assigneeName : "Loading..."}
              </p>
              <p>
                <strong>Requester:</strong> {ticket ? ticket.requesterName : "Loading..."}
              </p>
            </div>
          }</div>
        )}
      </MainPanel>
    </>
  );
}

export default TicketDetails;
