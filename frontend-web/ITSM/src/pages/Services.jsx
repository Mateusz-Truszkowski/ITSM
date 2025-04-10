import React from "react";
import { useNavigate } from "react-router-dom";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";

function Services() {
  const navigate = useNavigate();

  const openService = async (serviceId) => {
    console.log("Otwarto serwis: " + serviceId);
    navigate(`/services/${serviceId}`);
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, isLoading }) => (
          <div className="records-container">
            <h2 className="records-header">Users</h2>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : data && data.length > 0 ? (
              <table className="records-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Contracting Date</th>
                    <th>Status</th>
                    <th>SLA</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((service) => (
                    <tr
                      onClick={() => openService(service.id)}
                      key={service.id}
                    >
                      <td>{service.id}</td>
                      <td>{service.name}</td>
                      <td>{service.description}</td>
                      <td>
                        {new Date(service.contractingdate).toLocaleDateString()}
                      </td>
                      <td>{service.status}</td>
                      <td>{service.sla}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : null}
            {data.length === 0 && !isLoading && (
              <div className="no-records">
                <p>No services available</p>
              </div>
            )}
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default Services;
