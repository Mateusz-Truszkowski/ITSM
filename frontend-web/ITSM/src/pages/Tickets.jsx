import "../assets/GeneralLP.css";
import NavigationLP from "../components/NavigationLP.jsx";
import "../assets/MainPanel.css";
import "../assets/Tickets.css";
import MainPanel from "../components/MainPanel";
import React, { useEffect, useState } from "react";

function Tickets() {
  const [tickets, setTickets] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const displayTickets = async () => {
    try {
      const token = localStorage.getItem("authToken");

      const response = await fetch("https://localhost:63728/tickets", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        console.log("Błąd podczas pobierania ticketów:", response.status);
        return;
      }

      const ticketsData = await response.json();

      // Pobierz unikalne ID użytkowników
      const userIds = [
        ...new Set(
          ticketsData
            .flatMap((t) => [t.requesterId, t.assigneeId])
            .filter((id) => id !== null)
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

      // Tworzymy mapę użytkowników
      const userMap = {};
      userData.forEach((user) => {
        userMap[user.id] = `${user.name} ${user.surname}`; // Możesz dodać nazwisko, jeśli jest dostępne
      });

      // Wzbogacenie ticketów
      const enrichedTickets = ticketsData.map((ticket) => ({
        ...ticket,
        requesterName: userMap[ticket.requesterId] ?? "—", // Jeśli nie ma użytkownika, to wyświetlimy "—"
        assigneeName: userMap[ticket.assigneeId] ?? "—", // To samo dla assignee
      }));

      setTickets(enrichedTickets); // Ustawienie wzbogaconych ticketów do stanu
      setIsLoading(false); // Ustawienie stanu ładowania na false po załadowaniu danych
    } catch (error) {
      console.error("Wystąpił błąd:", error);
      setIsLoading(false); // Zatrzymanie ładowania, jeśli wystąpił błąd
    }
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
