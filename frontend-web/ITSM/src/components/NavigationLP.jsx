import { Link } from "react-router-dom";
import "../assets/HeaderLP.css";
import logo from "../assets/images/logo.png";
import { useState, useEffect } from "react";

function NavigationLP() {
  const [loggedIn, setLoggedIn] = useState(false);

  useEffect(() => {
    const checkLoginStatus = () => {
      const token = localStorage.getItem("authToken");
      setLoggedIn(!!token); // If token exists, set loggedIn to true
    };

    checkLoginStatus(); // Run the check on initial load
  }, []);

  function Logout() {
    localStorage.clear();
    window.location.href = "/";
  }

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
              <span class="userName">{localStorage.getItem("name")}</span>
            </div>
            <button onClick={Logout} className="navigation-item logout">
              Logout
            </button>
          </>
        )}
      </div>
    </section>
  );
}

export default NavigationLP;
