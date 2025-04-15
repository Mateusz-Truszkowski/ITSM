import { serverPath } from "../global";
import { saveAs } from "file-saver";

export const fetchUsers = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/users", {
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
    console.error("Error fetching users:", error);
    return null;
  }
};

export const fetchUser = async (userId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/users/${userId}`, {
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
    console.error(`Error fetching user ${userId}:`, error);
    return null;
  }
};
export const fetchUsersReport = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/users/report", {
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
    saveAs(data, "userssReport.xlsx");
  } catch (error) {
    console.error("Error fetching users:", error);
    return null;
  }
};