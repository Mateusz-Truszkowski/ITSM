import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Tickets.css";
import MainPanel from "../components/MainPanel";
import NavigationLP from "../components/NavigationLP";
import React, { useEffect, useState } from "react";
import { fetchTickets } from "../hooks/tickets.js";
import { fetchUser } from "../hooks/users.js";

function Tickets() {
  const [tickets, setTickets] = useState([]);

  const displayTickets = async () => {
    const ticketsData = await fetchTickets();

    const userIds = [
      ...new Set(
        ticketsData
          .flatMap((t) => [t.requesterId, t.assigneeId])
          .filter((id) => id !== null)
      ),
    ];

    const userData = await Promise.all(userIds.map((id) => fetchUser(id)));

    const userMap = {};
    userData.forEach((user) => {
      userMap[user.id] = `${user.name} ${user.surname}`;
    });

    const ticketsWithNames = ticketsData.map((ticket) => ({
      ...ticket,
      requesterName: userMap[ticket.requesterId] ?? "—",
      assigneeName: userMap[ticket.assigneeId] ?? "—",
    }));

    setTickets(ticketsWithNames);
  };

  useEffect(() => {
    displayTickets();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, isLoading, openRecord }) => (
          <div className="records-container">
            <h2 className="records-header">Tickets</h2>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : data && data.length > 0 ? (
              <table className="records-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Type</th>
                    <th>Status</th>
                    <th>Priority</th>
                    <th>Created</th>
                    <th>Requester</th>
                    <th>Assignee</th>
                  </tr>
                </thead>
                <tbody>
                  {tickets.map((ticket) => (
                    <tr onClick={() => openRecord(ticket.id)} key={ticket.id}>
                      <td>{ticket.id}</td>
                      <td>{ticket.name}</td>
                      <td>{ticket.type}</td>
                      <td>{ticket.status}</td>
                      <td>{ticket.priority}</td>
                      <td>
                        {new Date(ticket.creationDate).toLocaleDateString()}
                      </td>
                      <td className="requester-td">
                        <span className="requester">
                          {ticket.requesterName}
                        </span>
                      </td>
                      <td className="assignee-td">
                        <span className="assignee">{ticket.assigneeName}</span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : null}
            {data.length === 0 && !isLoading && (
              <div className="no-records">
                <p>No tickets available</p>
              </div>
            )}
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default Tickets;
