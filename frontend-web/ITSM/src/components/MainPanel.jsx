import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import React, { useState, useEffect, useCallback } from "react"; // useCallback z react
import { useLocation, useNavigate, Link } from "react-router-dom"; // Pozostałe importy z react-router-dom
import rocket from "../assets/images/rocket.png";
import ticket from "../assets/icons/ticket-icon.png";
import person from "../assets/icons/user-icon.png";
import cog from "../assets/icons/cog-icon.png";
import laptop from "../assets/icons/laptop-icon.png";

function MainPanel({ children }) {
  const location = useLocation();
  const token = localStorage.getItem("authToken");
  const navigate = useNavigate();
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const fetchDataFromBackend = useCallback(
    async (table) => {
      setIsLoading(true);
      try {
        const response = await fetch(`https://localhost:63728/${table}`, {
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

  const openRecord = async (recordId) => {
    console.log("Otwarto rekord o numerze: " + recordId);
    navigate(`${location.pathname}/${recordId}`);
  };

  useEffect(() => {
    // Jeśli ścieżka to /dashboard, nie pobieramy danych i ustawiamy isLoading na false
    if (location.pathname === "/dashboard") {
      setIsLoading(false); // Ustawiamy na false, żeby nie wyświetlać spinnera
      return;
    }

    // Jeśli to inna ścieżka, pobieramy dane
    fetchDataFromBackend(location.pathname.substring(1));
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
              children({ data, openRecord })
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

export default MainPanel;
