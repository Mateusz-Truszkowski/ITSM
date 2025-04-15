import React from "react";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel.jsx";
import { useEffect, useState } from "react";
import { fetchServices,fetchServicesReport } from "../hooks/services.js";
import { useCheckTokenValidity } from "../global.js";

function Services() {
  const [services, setServices] = useState([]);
  const checkToken = useCheckTokenValidity();
  const [isLoading, setIsLoading] = useState(true);

  const displayServices = async () => {
    try {
      const servicesData = await fetchServices();

      if (servicesData === null) {
        throw new Error("error fetching services");
      }
      setServices(servicesData);
      setIsLoading(false);
    } catch (error) {
      console.log("Error occured: " + error);
    }
  };
  const MakeReport = async () => {
    try {
      const Data = await fetchServicesReport();

      if (Data === null) {
        throw new Error("Report creating error");
      }
    } catch (error) {
      console.log("Error occured: " + error);
    }
  };
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayServices();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ openRecord }) => (
          <div className="records-container">
            <h2 className="records-header">Services</h2>
            <button className="report-button" onClick={MakeReport}>
              Download report
            </button>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : services && services.length > 0 ? (
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
                  {services.map((service) => (
                    <tr onClick={() => openRecord(service.id)} key={service.id}>
                      <td>{service.id}</td>
                      <td>{service.name}</td>
                      <td>{service.description}</td>
                      <td>
                        {new Date(service.contractingDate).toLocaleDateString()}
                      </td>
                      <td>{service.status}</td>
                      <td>{service.sla}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : null}
            {services.length === 0 && !isLoading && (
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
