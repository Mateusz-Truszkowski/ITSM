import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import { Link, useLocation } from "react-router-dom";
import rocket from "../assets/images/rocket.png";
import ticket from "../assets/icons/ticket-icon.png";
import person from "../assets/icons/user-icon.png";
import cog from "../assets/icons/cog-icon.png";
import laptop from "../assets/icons/laptop-icon.png";

function MainPanel() {
  const location = useLocation();
  const token = localStorage.getItem("authToken");

  return (
    <>
      {token !== null ? (
        <div className="dashboard">
          <div className="navigation">
            <div className="nav-logo-div">
              <img className="nav-logo" src={rocket} alt="" />
            </div>
            <div className="tabs">
              <Link
                to="/tickets"
                className={`tab ${
                  location.pathname === "/tickets"
                    ? "tickets active"
                    : "tickets"
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={ticket} alt="" />
                  </div>
                  <span className="tab-text">Tickets</span>
                </button>
              </Link>
              <Link
                to="/users"
                className={`tab ${
                  location.pathname === "/users" ? "users active" : "users"
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={person} alt="" />
                  </div>
                  <span className="tab-text">Users</span>
                </button>
              </Link>
              <Link
                to="/services"
                className={`tab ${
                  location.pathname === "/services"
                    ? "services active"
                    : "services"
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={cog} alt="" />
                  </div>
                  <span className="tab-text">Services</span>
                </button>
              </Link>
              <Link
                to="/devices"
                className={`tab ${
                  location.pathname === "/devices"
                    ? "devices active"
                    : "devices"
                }`}
              >
                <button className="tab">
                  <div className="tab-image">
                    <img src={laptop} alt="" />
                  </div>
                  <span className="tab-text">Devices</span>
                </button>
              </Link>
            </div>
          </div>
          <div className="content"></div>
        </div>
      ) : (
        <div class="unathorized">
          <p className="unauthorized-message">NO ACCESS</p>
          <p className="unauthorized-login">
            PLEASE{" "}
            <Link to="/login">
              <span className="message-login">LOG IN</span>
            </Link>
          </p>
        </div>
      )}
    </>
  );
}

export default MainPanel;
