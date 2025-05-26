import { serverPath } from "../global";
import { saveAs } from "file-saver";

export const fetchTickets = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/tickets", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json();
    console.log("Tickets:", data);
    return data;
  } catch (error) {
    console.error("Error fetching tickets:", error);
    return null;
  }
};

export const fetchTicket = async (ticketId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/tickets/${ticketId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json();
    console.log("Users:", data);
    return data;
  } catch (error) {
    console.error(`Error fetching ticket ${ticketId}:`, error);
    return null;
  }
};
export const fetchTicketReport = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/tickets/report", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.blob();
    saveAs(data, "TicketsReport.xlsx");
  } catch (error) {
    console.error("Error fetching tickets:", error);
    return null;
  }
};
export const createTicket = async (ticketData) => {
  const response = await fetch(serverPath +"/tickets", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("authToken")}`,
    },
    body: JSON.stringify(ticketData),
  });

  return response.ok;
};
export const saveTicket = async (ticketData) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/tickets`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(ticketData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return true;
  } catch (error) {
    console.error(`Error saving ticket: `, error);
    return false;
  }
};

export const deleteTicket = async (ticketId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/tickets/${ticketId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.ok;
  } catch (error) {
    console.error(`Error fetching ticket ${ticketId}:`, error);
    return null;
  }
};