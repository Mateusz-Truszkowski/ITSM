import { Logout } from "./components/NavigationLP";

export const serverPath = "https://localhost:63728";

export const useCheckTokenValidity = () => {
  const checkToken = (token) => {
    try {
      const payload = JSON.parse(atob(token.split(".")[1]));
      const expired = Date.now() >= payload.exp * 1000;

      if (expired) {
        localStorage.removeItem("authToken");
        localStorage.removeItem("name");
        Logout();
      }
    } catch (err) {
      console.error("Błąd przy sprawdzaniu tokena:", err);
      Logout();
    }
  };

  return checkToken;
};
