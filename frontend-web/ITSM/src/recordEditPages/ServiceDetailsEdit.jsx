import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { saveService, fetchService } from "../hooks/services";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { checkToken } from "../global";

function ServiceDetailsEdit() {
  const [service, setService] = useState();
  const { serviceId } = useParams();
  const [isLoading, setIsLoading] = useState(true);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [contractingDate, setContractingDate] = useState("");
  const [status, setStatus] = useState("");
  const [sla, setSla] = useState("");
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const displayService = async () => {
    try {
      const serviceData = await fetchService(serviceId);

      if (serviceData === null) {
        throw new Error("Error fetching service");
      }

      setService(serviceData);
      const date = new Date(serviceData.contractingDate).toLocaleDateString();
      const [day, month, year] = date.split(".");
      const formattedDay = day.padStart(2, "0");
      const formattedMonth = month.padStart(2, "0");
      const finalDate = `${year}-${formattedMonth}-${formattedDay}`;
      setContractingDate(finalDate);

      setName(serviceData.name);
      setDescription(serviceData.description);
      setStatus(serviceData.status);
      setSla(serviceData.sla);

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

  const handleContractingDateChange = (event) => {
    setContractingDate(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
  };

  const handleSlaChange = (event) => {
    setSla(event.target.value);
  };

  const saveRecord = async () => {
    const serviceData = {
      Id: serviceId,
      name: name,
      description: description,
      contractingDate: contractingDate,
      status: status,
      sla: sla,
    };
    const success = await saveService(serviceData);
    if (success) {
      setError(false);
      navigate(`/services/${serviceId}`);
    } else {
      setError(true);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    checkToken(token);

    displayService();
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
                      <span className="record-label">Contracting Date:</span>
                      <input
                        className="record-value-edit"
                        value={contractingDate}
                        type="date"
                        onChange={handleContractingDateChange}
                      />
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
                    <div className="record-field">
                      <span className="record-label">SLA:</span>
                      <input
                        className="record-value-edit"
                        value={sla}
                        type="text"
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

export default ServiceDetailsEdit;
