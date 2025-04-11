const serverPath = "https://localhost:63728";

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
  }
};
