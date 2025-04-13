import { serverPath } from "../global";

export const fetchServices = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/services", {
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
    return data;
  } catch (error) {
    console.error("Error fetching tickets:", error);
    return null;
  }
};

export const fetchService = async (serviceId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/services/${serviceId}`, {
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
    console.log("Service:", data);
    return data;
  } catch (error) {
    console.error(`Error fetching service ${serviceId}:`, error);
    return null;
  }
};
