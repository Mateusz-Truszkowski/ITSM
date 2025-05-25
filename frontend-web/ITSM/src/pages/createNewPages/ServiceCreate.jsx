import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { createService } from "../hooks/services";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useCheckTokenValidity } from "../global";

function ServiceCreate() {
  const [service, setService] = useState();
  const { serviceId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const checkToken = useCheckTokenValidity();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [contractingDate, setContractingDate] = useState("");
  const [status, setStatus] = useState("");
  const [sla, setSla] = useState("");
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const handleNameChange = (event) => {
    setName(event.target.value);
  };

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value);
  };

  const handleContractingDateChange = (event) => {
    setContractingDate(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
  };

  const handleSlaChange = (event) => {
    setSla(event.target.value);
  };

  const createRecord = async () => {
    const date = new Date(contractingDate).toLocaleDateString();
    const [day, month, year] = date.split(".");
    const formattedDay = day.padStart(2, "0");
    const formattedMonth = month.padStart(2, "0");
    const finalDate = `${year}-${formattedMonth}-${formattedDay}`;
    setContractingDate(finalDate);

    const serviceData = {
      name: name,
      description: description,
      contractingDate: contractingDate,
      Status: status,
      sla: sla,
    };
    const success = await createService(serviceData);
    if (success) {
      setError(false);
      navigate(`/services`);
    } else {
      setError(true);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    setIsLoading(false);
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Service Details</h1>
                <button className="edit-button" onClick={createRecord}>
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
                        type="text"
                        onChange={handleNameChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Description:</span>
                      <textarea
                        className="record-value-edit"
                        type="text"
                        onChange={handleDescriptionChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Contracting Date:</span>
                      <input
                        className="record-value-edit"
                        type="date"
                        onChange={handleContractingDateChange}
                      />
                    </div>
                    <div className="record-field">
                      <span className="record-label">Status:</span>
                      <select
                        className="record-value-edit"
                        type="text"
                        onChange={handleStatusChange}
                      >
                        <option value="Active">Active</option>
                        <option value="Inactive">Inactive</option>
                      </select>
                    </div>
                    <div className="record-field">
                      <span className="record-label">SLA:</span>
                      <input
                        className="record-value-edit"
                        type="number"
                        onChange={handleSlaChange}
                      />
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

export default ServiceCreate;
