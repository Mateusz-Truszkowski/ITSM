import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchDevice, saveDevice } from "../hooks/devices";
import { fetchUsers } from "../hooks/users";
import { checkToken } from "../global";
import { fetchUser } from "../hooks/users";

function DeviceDetailsEdit() {
  const [device, setDevice] = useState(null);
  const { deviceId } = useParams();
  const [isLoading, setIsLoading] = useState(true);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [acquisitionDate, setAcquisitionDate] = useState("");
  const [depreciationDate, setDepreciationDate] = useState("");
  const [user, setUser] = useState("");
  const [status, setStatus] = useState("");
  const [error, setError] = useState(false);
  const [users, setUsers] = useState([]);
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
      setName(enrichedDevice.name);
      setDescription(enrichedDevice.description);
      var date = new Date(enrichedDevice.acquisitionDate).toLocaleDateString();
      var [day, month, year] = date.split(".");
      var formattedDay = day.padStart(2, "0");
      var formattedMonth = month.padStart(2, "0");
      var finalDate = `${year}-${formattedMonth}-${formattedDay}`;
      setAcquisitionDate(finalDate);
      date = new Date(enrichedDevice.depreciationDate).toLocaleDateString();
      [day, month, year] = date.split(".");
      formattedDay = day.padStart(2, "0");
      formattedMonth = month.padStart(2, "0");
      finalDate = `${year}-${formattedMonth}-${formattedDay}`;
      setDepreciationDate(finalDate);
      setUser(enrichedDevice.userId);
      setStatus(enrichedDevice.status);

      setIsLoading(false);
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  const handleNameChange = (event) => {
    setName(event.target.value);
  };

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value);
  };

  const handleAcquisitionDateChange = (event) => {
    setAcquisitionDate(event.target.value);
  };

  const handelDepreciationDateChange = (event) => {
    setDepreciationDate(event.target.value);
  };

  const handleUserChange = (event) => {
    setUser(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
  };

  const saveRecord = async () => {
    const acqDate = new Date(acquisitionDate).toLocaleDateString();
    var [day, month, year] = acqDate.split(".");
    const acqDateFormatted = `${year}-${month.padStart(2, "0")}-${day.padStart(
      2,
      "0"
    )}T00:00:00`;
    const depDate = new Date(depreciationDate).toLocaleDateString();
    [day, month, year] = depDate.split(".");
    const depDateFormatted = `${year}-${month.padStart(2, "0")}-${day.padStart(
      2,
      "0"
    )}T00:00:00`;

    const deviceData = {
      id: deviceId,
      name: name,
      description: description,
      acquisitionDate: acqDateFormatted,
      depreciationDate: depDateFormatted,
      UserId: user,
      status: status,
    };
    const success = await saveDevice(deviceData);
    if (success) {
      setError(false);
      navigate(`/devices/${deviceId}`);
    } else {
      setError(true);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    displayDevice();

    const getUsers = async () => {
      const fetchedUsers = await fetchUsers();
      setUsers(fetchedUsers);
    };
    getUsers();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Device Details</h1>
                <button className="edit-button" onClick={saveRecord}>
                  Save
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
                      <input
                        className="record-value-edit"
                        value={name}
                        type="text"
                        onChange={handleNameChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Description:</span>
                      <textarea
                        className="record-value-edit"
                        value={description}
                        type="text"
                        onChange={handleDescriptionChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Acquisition Date:</span>
                      <input
                        className="record-value-edit"
                        value={acquisitionDate}
                        type="date"
                        onChange={handleAcquisitionDateChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Depreciation Date:</span>
                      <input
                        className="record-value-edit"
                        value={depreciationDate}
                        type="date"
                        onChange={handelDepreciationDateChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Owner:</span>
                      <select
                        className="record-value-edit"
                        value={user}
                        onChange={handleUserChange}
                      >
                        {users.map((user) => (
                          <option key={user.id} value={user.id}>
                            {user.name + " " + user.surname}
                          </option>
                        ))}
                      </select>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Status:</span>
                      <select
                        className="record-value-edit"
                        value={status}
                        type="text"
                        onChange={handleStatusChange}
                      >
                        <option value="Active">Active</option>
                        <option value="Inactive">Inactive</option>
                      </select>
                    </div>
                  </>
                )}
              </div>
              {error && (
                <div className="failed-message">
                  <p>!</p>
                  <span>Error occured</span>
                </div>
              )}
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default DeviceDetailsEdit;
