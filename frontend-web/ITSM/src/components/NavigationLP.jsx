import { Link } from "react-router-dom";
import "../assets/HeaderLP.css";
import logo from "../assets/images/logo.png";
import { useState, useEffect } from "react";

export function Logout() {
  localStorage.clear();
  window.location.href = "/logged-out";
}

export function LogoutManual() {
  localStorage.clear();
  window.location.href = "/";
}

function NavigationLP() {
  const [loggedIn, setLoggedIn] = useState(false);

  useEffect(() => {
    const checkLoginStatus = () => {
      const token = localStorage.getItem("authToken");
      setLoggedIn(!!token);
    };

    checkLoginStatus();
  }, []);

  return (
    <section className="header">
      <Link className="logo-link" to="/">
        <img src={logo} alt="Logo" />
      </Link>
      <div className="navigation-bar">
        {!loggedIn ? (
          <>
            <Link to="/">
              <div className="navigation-item">About</div>
            </Link>
            <Link to="/">
              <div className="navigation-item">Contact</div>
            </Link>
            <Link to="/login">
              <div className="navigation-item">Login | Register</div>
            </Link>
          </>
        ) : (
          <>
            <div className="userName-div">
              Hello,{" "}
              <span className="userName">{localStorage.getItem("name")}</span>
            </div>
            <button onClick={LogoutManual} className="navigation-item logout">
              Logout
            </button>
          </>
        )}
      </div>
    </section>
  );
}

export default NavigationLP;
