import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { fetchService, deleteService } from "../hooks/services";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useCheckTokenValidity } from "../global";

function ServiceDetails() {
  const [service, setService] = useState();
  const { serviceId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const checkToken = useCheckTokenValidity();
  const navigate = useNavigate();

  const displayService = async () => {
    try {
      const serviceData = await fetchService(serviceId);

      if (serviceData === null) {
        throw new Error("error fetching service");
      }

      setService(serviceData);
      console.log(serviceData);
      setIsLoading(false);
    } catch (error) {
      console.error("Wystąpił błąd:", error);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayService();
  }, []);

  const editRecord = () => {
    navigate(`/services/${service.id}/edit`);
  };
  const deleteRecord = async () => {
    await deleteService(service.id);
    navigate(`/services`);
  }

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Service Details</h1>
                <div className="Button-container">
                  <button className="delete-button" onClick={deleteRecord}>
                    delete
                  </button>
                  <button className="edit-button" onClick={editRecord}>
                    Edit
                  </button>
                </div>
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
                      <span className="record-value">{service.name}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Description:</span>
                      <span className="record-value">
                        {service.description}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Contracting Date:</span>
                      <span className="record-value">
                        {new Date(service.contractingDate).toLocaleDateString()}
                      </span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">Status:</span>
                      <span className="record-value">{service.status}</span>
                    </div>
                    <div className="record-field">
                      <span className="record-label">SLA:</span>
                      <span className="record-value">{service.sla}</span>
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

export default ServiceDetails;
