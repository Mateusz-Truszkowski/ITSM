import "../assets/GeneralLP.css";
import NavigationLP from "../components/NavigationLP.jsx";
import "../assets/MainPanel.css";
import "../assets/Tickets.css";
import MainPanel from "../components/MainPanel";

import React from "react";

function Tickets() {
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
                  {data.map((ticket) => (
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
