import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import React from "react"; // useCallback z react
import { useLocation, useNavigate, Link } from "react-router-dom"; // Pozostałe importy z react-router-dom
import rocket from "../assets/images/rocket.png";
import ticket from "../assets/icons/ticket-icon.png";
import person from "../assets/icons/user-icon.png";
import cog from "../assets/icons/cog-icon.png";
import laptop from "../assets/icons/laptop-icon.png";
import Notification from "../generalPages/Notification";

function MainPanel({ children }) {
  const location = useLocation();
  const token = localStorage.getItem("authToken");
  const navigate = useNavigate();

  const openRecord = async (recordId) => {
    console.log("Otwarto rekord o numerze: " + recordId);
    navigate(`${location.pathname}/${recordId}`);
  };

  return (
    <>
      {token !== null ? (
        <div className="dashboard">
          <div className="navigation">
            <div className="nav-logo-div">
              <img className="nav-logo" src={rocket} alt="Logo" />
            </div>
            <div className="tabs">
              <Link
                to="/tickets"
                className={`tab ${
                  location.pathname === "/tickets" ? "active" : ""
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={ticket} alt="Tickets" />
                  </div>
                  <span className="tab-text">Tickets</span>
                </button>
              </Link>
              <Link
                to="/users"
                className={`tab ${
                  location.pathname === "/users" ? "active" : ""
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={person} alt="Users" />
                  </div>
                  <span className="tab-text">Users</span>
                </button>
              </Link>
              <Link
                to="/services"
                className={`tab ${
                  location.pathname === "/services" ? "active" : ""
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={cog} alt="Services" />
                  </div>
                  <span className="tab-text">Services</span>
                </button>
              </Link>
              <Link
                to="/devices"
                className={`tab ${
                  location.pathname === "/devices" ? "active" : ""
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={laptop} alt="Devices" />
                  </div>
                  <span className="tab-text">Devices</span>
                </button>
              </Link>
            </div>
          </div>
          <div className="content">{children({ openRecord })}</div>
        </div>
      ) : (
        <>
          <div className="unauthorized">
            <p className="unauthorized-message">NO ACCESS</p>
            <p className="unauthorized-login">
              PLEASE{" "}
              <Link to="/login">
                <span className="message-login">LOG IN</span>
              </Link>
            </p>
          </div>
          <Notification message="Zostałeś automatycznie wylogowany" />
        </>
      )}
    </>
  );
}

export default MainPanel;
