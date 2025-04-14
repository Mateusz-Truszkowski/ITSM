import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link } from "react-router-dom";
import { fetchTicket } from "../hooks/tickets";
import { fetchUser } from "../hooks/users";
import { useCheckTokenValidity } from "../global";

function TicketDetails() {
  const { ticketId } = useParams();
  const checkToken = useCheckTokenValidity();
  const [isLoading, setIsLoading] = useState(true);
  const [ticket, setTicket] = useState();

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
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <h1 className="record-details-header">Ticket Details</h1>
              <div className="record-fields">
                {isLoading ? (
                  <div className="loading-spinner">
                    <div className="spinner"></div>
                  </div>
                ) : (
                  <>
                    <div className="record-field">
                      <span className="record-label">Name:</span>
                      <span className="record-value">{ticket.name}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Description:</span>
                      <span className="record-value">{ticket.description}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Created:</span>
                      <span className="record-value">
                        {new Date(ticket.creationDate).toLocaleDateString()}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Solution Date:</span>
                      <span className="record-value">
                        {ticket.solutiondate
                          ? new Date(ticket.solutiondate).toLocaleDateString()
                          : ""}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">
                        Solution Description:
                      </span>
                      <span className="record-value">
                        {ticket.solutiondescription}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Priority:</span>
                      <span className="record-value">{ticket.priority}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Type:</span>
                      <span className="record-value">{ticket.type}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Status:</span>
                      <span className="record-value">{ticket.status}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Service ID:</span>
                      <span className="record-value">{ticket.serviceId}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Assignee:</span>
                      <span className="record-value">
                        {
                          <Link to={`/users/${ticket.assigneeId}`}>
                            {ticket.assigneeName}
                          </Link>
                        }
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Requester:</span>
                      <span className="record-value">
                        {
                          <Link to={`/users/${ticket.requesterId}`}>
                            {ticket.requesterName}
                          </Link>
                        }
                      </span>
                    </div>
                  </>
                )}
              </div>
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default TicketDetails;
