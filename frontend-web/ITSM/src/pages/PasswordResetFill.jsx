import "../assets/GeneralLP.css";
import "../assets/FormLP.css";
import React, { useState, useEffect } from "react";
import person from "../assets/icons/user-icon.png";
import NavigationLP from "../components/NavigationLP";
import { useLocation, useNavigate } from "react-router-dom";

function PasswordResetFill() {
  const [password, setPassword] = useState(""); // Nowe hasło
  const [confirmPassword, setConfirmPassword] = useState(""); // Potwierdzenie hasła
  const [errorMessage, setErrorMessage] = useState(""); // Stan na komunikat błędu
  const [token, setToken] = useState("");
  const [success, setSuccess] = useState(0); // 0 - not tried, 1 - failed, 2 - success
  const location = useLocation();
  const navigate = useNavigate();

  // Pobranie tokenu z URL
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const token = params.get("token");
    setToken(token);
  }, [location]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!token) {
      console.error("Brak tokenu!");
      setSuccess(1);
      setErrorMessage("Niepoprawny link");
      return;
    }

    // Sprawdzanie, czy hasła są takie same
    if (password !== confirmPassword) {
      setErrorMessage("The passwords do not match!");
      return;
    } else if (password == null || confirmPassword == null) {
      setErrorMessage("Fill new password");
      return;
    } else {
      setErrorMessage(""); // Wyczyść komunikat o błędzie, jeśli hasła są takie same
    }
    const NewPassRequest = {
      Token: token,
      NewPassword: password,
    };
    console.log(NewPassRequest.Token); // Zmieniamy 'token' na 'Token'
    console.log(NewPassRequest.NewPassword);
    // Jeśli wszystkie warunki są spełnione, wysyłamy żądanie
    try {
      let response = await fetch("https://localhost:63728/auth/setnewpass", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(NewPassRequest),
      });

      if (response.ok) {
        const data = await response.text();
        console.log("Ustawiono nowe hasło dla użytkownika: ", data);
        setSuccess(2);
        setTimeout(() => {
          navigate("/login");
        }, 1000);
      } else {
        console.log("Błąd podczas ustawiania hasła:", response.status);
        setSuccess(1);
        setErrorMessage("Server error");
      }
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  // Funkcja obsługująca zmianę hasła
  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
    if (success !== 2) setSuccess(0);
  };

  // Funkcja obsługująca zmianę potwierdzenia hasła
  const handleConfirmPasswordChange = (event) => {
    setConfirmPassword(event.target.value);
    if (success !== 2) setSuccess(0);
  };

  return (
    <>
      <NavigationLP />
      <div className="resetpass-container">
        <form onSubmit={handleSubmit} className="form">
          <p className="SetNewPass">Set new password</p>
          <div className="inputs">
            <div className="NewPassword">
              <input
                onChange={handlePasswordChange}
                placeholder="New password"
                type="password"
              />
              <img src={person} alt="" />
            </div>
            <div className="Confirmpassword">
              <input
                onChange={handleConfirmPasswordChange}
                placeholder="Confirm password"
                type="password"
              />
              <img src={person} alt="" />
            </div>
          </div>
          <div className="login-button">
            <button type="submit">Set new password</button>
          </div>
          {success === 2 && (
            <div className="confirmation-message">
              <p>&#x2713;</p> {/*checkmark */}
              <span>Password reset successful!</span>
            </div>
          )}
          {success === 1 && (
            <div className="failed-message">
              <p>!</p>
              <span>{errorMessage}</span>
            </div>
          )}
        </form>
      </div>
    </>
  );
}

export default PasswordResetFill;
