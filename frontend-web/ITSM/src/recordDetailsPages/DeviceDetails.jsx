import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchDevice } from "../hooks/devices";
import { useCheckTokenValidity } from "../global";
import { fetchUser } from "../hooks/users";

function DeviceDetails() {
  const [device, setDevice] = useState(null);
  const { deviceId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const checkToken = useCheckTokenValidity();
  const navigate = useNavigate();

  const displayDevice = async () => {
    try {
      const deviceData = await fetchDevice(deviceId);
      if (deviceData === null) {
        throw new Error("Error fetching device");
      }

      const userIds = [deviceData.userId].filter((id) => id !== null);

      const userResponses = await Promise.all(
        userIds.map((id) => fetchUser(id))
      );

      const userMap = {};
      userResponses.forEach((user) => {
        if (user && user.id) {
          userMap[user.id] = `${user.name} ${user.surname}`;
        }
      });

      const enrichedDevice = {
        ...deviceData,
        userName: userMap[deviceData.userId] ?? "—",
      };

      setDevice(enrichedDevice);
      setIsLoading(false);
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayDevice();
  }, []);

  const editRecord = () => {
    navigate(`/devices/${device.id}/edit`);
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Device Details</h1>
                <button className="edit-button" onClick={editRecord}>
                  Edit
                </button>
              </div>
              <div className="record-fields">
                {isLoading ? (
                  <div className="loading-spinner">
                    <div className="spinner"></div>
                  </div>
                ) : (
                  <>
                    <div className="record-field">
                      <span className="record-label">Name:</span>
                      <span className="record-value">{device.name}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Description:</span>
                      <span className="record-value">{device.description}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Acquisition Date:</span>
                      <span className="record-value">
                        {device.acquisitionDate
                          ? new Date(
                              device.acquisitionDate
                            ).toLocaleDateString()
                          : ""}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Depreciation Date:</span>
                      <span className="record-value">
                        {device.depreciationDate
                          ? new Date(
                              device.depreciationDate
                            ).toLocaleDateString()
                          : ""}
                      </span>
                    </div>
                    {
                      <div className="record-field">
                        <span className="record-label">Owner:</span>
                        <span className="record-value">
                          {
                            <Link to={`/users/${device.userId}`}>
                              {device.userName}
                            </Link>
                          }
                        </span>
                      </div>
                    }
                    <div className="record-field">
                      <span className="record-label">Status:</span>
                      <span className="record-value">{device.status}</span>
                    </div>
                  </>
                )}
              </div>
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default DeviceDetails;
