import "../assets/GeneralLP.css";
import "../assets/FormLP.css";
import React, { useState } from "react";
import person from "../assets/icons/user-icon.png";
import NavigationLP from "../components/NavigationLP";

function PasswordReset() {
  const [login, setLogin] = useState("");
  const [success, setSuccess] = useState(0); // 0 - not tried, 1 - failed, 2 - success

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      let response = await fetch("https://localhost:63728/auth/reset", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(login),
      });

      if (response.ok) {
        const data = await response.text();
        console.log("Reset hasła:", data);
        setSuccess(2);
      } else {
        console.log("Nie znaleziono podanego loginu:", response.status);
        setSuccess(1);
      }
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  const handleUsernameChange = (event) => {
    setLogin(event.target.value);
    if (success !== 2) setSuccess(0);
  };

  return (
    <>
      <NavigationLP />
      <div className="resetpass-container">
        <form onSubmit={handleSubmit} className="form">
          <p className="login">Password reset</p>
          <div className="inputs">
            <div className="username">
              <input
                onChange={handleUsernameChange}
                placeholder="Username"
                type="text"
              />
              <img src={person} alt="" />
            </div>
          </div>
          <div className="login-button">
            <button type="submit">Ok</button>
          </div>
          {success === 2 && (
            <div className="confirmation-message">
              <p>&#x2713;</p> {/*checkmark */}
            </div>
          )}
          {success === 1 && (
            <div className="failed-message">
              <p>!</p>
            </div>
          )}
        </form>
      </div>
    </>
  );
}

export default PasswordReset;
