import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import React, { useState, useEffect, useCallback } from "react"; // useCallback z react
import { useLocation, Link } from "react-router-dom"; // Pozostałe importy z react-router-dom
import rocket from "../assets/images/rocket.png";
import ticket from "../assets/icons/ticket-icon.png";
import person from "../assets/icons/user-icon.png";
import cog from "../assets/icons/cog-icon.png";
import laptop from "../assets/icons/laptop-icon.png";

function MainPanelDetails({ children }) {
  const location = useLocation();
  const token = localStorage.getItem("authToken");
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const fetchDataFromBackend = useCallback(
    async (table) => {
      setIsLoading(true);
      try {
        const response = await fetch(`https://localhost:63728/${table}`, {
          // Poprawiony endpoint bez `/${}`
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        });

        if (!response.ok) {
          console.log("Błąd podczas pobierania danych:", response.status);
          return;
        }

        const resultData = await response.json();
        setData(resultData);
      } catch (error) {
        console.error("Wystąpił błąd:", error);
      } finally {
        setIsLoading(false);
      }
    },
    [token]
  );

  useEffect(() => {
    if (location.pathname === "/dashboard") {
      setIsLoading(false);
      return;
    }

    fetchDataFromBackend(location.pathname.substring(1)); // Fetching data based on path
  }, [location.pathname, fetchDataFromBackend]);

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
          <div className="content">
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : (
              // Przekazanie children jako funkcji renderującej
              children({ data, isLoading })
            )}
          </div>
        </div>
      ) : (
        <div className="unauthorized">
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

export default MainPanelDetails;
