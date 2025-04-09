import "../assets/GeneralLP.css";
import "../assets/FormLP.css";
import { Link } from "react-router-dom";
import React, { useState } from "react";
import person from "../assets/icons/user-icon.png";
import lock from "../assets/icons/password-icon.png";
import NavigationLP from "../components/NavigationLP";

function PasswordReset() {
  const [login, setLogin] = useState("");
  

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
      } else {
        console.log("Nie znaleziono podanego loginu:", response.status);
      }
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  const handleUsernameChange = (event) => {
    setLogin(event.target.value);
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
        </form>
      </div>
    </>
  );
}

export default PasswordReset