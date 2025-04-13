import { serverPath } from "../global";

export const fetchDevices = async () => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + "/devices", {
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
    console.error("Error fetching devices:", error);
    return null;
  }
};

export const fetchDevice = async (deviceId) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/devices/${deviceId}`, {
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
    console.error(`Error fetching device ${deviceId}:`, error);
    return null;
  }
};

export const saveDevice = async (deviceData) => {
  const token = localStorage.getItem("authToken");

  try {
    const response = await fetch(serverPath + `/services`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(deviceData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return true;
  } catch (error) {
    console.error(`Error saving device: `, error);
    return false;
  }
};
