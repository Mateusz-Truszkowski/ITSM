import React from "react";
import { useNavigate } from "react-router-dom";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";

function Devices() {
  const navigate = useNavigate();

  const openDevice = async (deviceId) => {
    console.log("Otwarto urządzenie: " + deviceId);
    navigate(`/devices/${deviceId}`);
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, isLoading }) => (
          <div className="records-container">
            <h2 className="records-header">Devices</h2>
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
                    <th>Acquisition Date</th>
                    <th>Status</th>
                    <th>SLA</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((device) => (
                    <tr onClick={() => openDevice(device.id)} key={device.id}>
                      <td>{device.id}</td>
                      <td>{device.name}</td>
                      <td>{device.description}</td>
                      <td>
                        {new Date(device.acquisitionDate).toLocaleDateString()}
                      </td>
                      <td>{device.status}</td>
                      <td>{device.sla}</td>
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

export default Devices;
