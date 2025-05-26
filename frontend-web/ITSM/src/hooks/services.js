import { serverPath } from "../global";
import { saveAs } from "file-saver";

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

export const saveService = async (serviceData) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/services`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(serviceData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return true;
  } catch (error) {
    console.error(`Error saving service: `, error);
    return false;
  }
};

export const createService = async (serviceData) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/services`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(serviceData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return true;
  } catch (error) {
    console.error(`Error saving service: `, error);
    return false;
  }
};

export const fetchServicesReport = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/services/report", {
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
    saveAs(data, "ServicesReport.xlsx");
  } catch (error) {
    console.error("Error fetching services:", error);
    return null;
  }
};

export const deleteService = async (serviceId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/services/${serviceId}`, {
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
    console.error(`Error fetching service ${serviceId}:`, error);
    return null;
  }
};