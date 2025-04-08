import "../assets/FooterLP.css";
import "../assets/HeaderLP.css";
import "../assets/GeneralLP.css";
import itsm from "../assets/images/itsm.png";
//import React, { useState } from "react";
import "../assets/Form.css";
import person from "../assets/icons/person.png";

function Login() {
  /*const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Logging in with:", username, password);
    // Dodaj tutaj logikę do logowania użytkownika
  };*/
  return (
    <>
      <section className="header">
        <img src={itsm} alt="" />
        <div className="navigation-bar">
          <div className="navigation-item">
            <a href="index.html">About</a>
          </div>
          <div className="navigation-item">
            <a href="index.html">Contact</a>
          </div>
          <div className="navigation-item">
            <a href="login.html">Login</a> |<a href="register.html">Register</a>
          </div>
        </div>
      </section>
      <div className="login-container">
        <div className="form">
          <p className="login">Login</p>
          <div className="inputs">
            <div className="username">
              <input placeholder="Username" type="text" />
              <img src={person} alt="" />
            </div>
            <input placeholder="Password" type="text" />
          </div>
        </div>
      </div>
    </>
  );
}

export default Login;
