import React from "react";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Devices.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";
import { useEffect, useState } from "react";
import { fetchDevices,fetchDevicesReport } from "../hooks/devices.js";
import { useCheckTokenValidity } from "../global";
import { useNavigate } from "react-router-dom";

function Devices() {
  const [devices, setDevices] = useState([]);
  const checkToken = useCheckTokenValidity();
  const [isLoading, setIsLoading] = useState(true);

  const displayDevices = async () => {
    try {
      const devicesData = await fetchDevices();

      if (devicesData === null) {
        throw new Error("error fetching devices");
      }
      setDevices(devicesData);
      setIsLoading(false);
    } catch (error) {
      console.log("Error occured: " + error);
    }
  };
  const MakeReport = async () => {
    try {
      const Data = await fetchDevicesReport();

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
    displayDevices();
  }, []);

const navigate = useNavigate();
const CreateDevice = () => {
  navigate("/devices/create");
};

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ openRecord }) => (
          <div className="records-container">
            <h2 className="records-header">Devices</h2>
            <button className="report-button" onClick={CreateDevice}>
              New
            </button>
            <button className="report-button" onClick={MakeReport}>
              Download report
            </button>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : devices && devices.length > 0 ? (
              <table className="records-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Acquisition Date</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {devices.map((device) => (
                    <tr onClick={() => openRecord(device.id)} key={device.id}>
                      <td>{device.id}</td>
                      <td>{device.name}</td>
                      <td>{device.description}</td>
                      <td>
                        {new Date(device.acquisitionDate).toLocaleDateString()}
                      </td>
                      <td>{device.status}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : null}
            {devices.length === 0 && !isLoading && (
              <div className="no-records">
                <p>No devices available</p>
              </div>
            )}
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default Devices;
