import { Logout } from "./components/NavigationLP";

export const serverPath = "https://localhost:63728";

export const checkToken = (token) => {
  if (!token || token === null) {
    return false;
  }

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    const expired = Date.now() >= payload.exp * 1000;

    if (expired) {
      localStorage.removeItem("authToken");
      localStorage.removeItem("name");

      Logout();
      return false;
    }

    return true;
  } catch (err) {
    console.error("Błąd przy sprawdzaniu tokena:", err);

    localStorage.removeItem("authToken");
    localStorage.removeItem("name");

    Logout();
    return false;
  }
};
