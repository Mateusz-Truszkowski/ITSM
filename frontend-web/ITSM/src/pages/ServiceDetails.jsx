import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";

function ServiceDetails() {
  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, openRecord, isLoading }) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <h1 className="record-details-header">Service Details</h1>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <span className="record-value">
                    {data ? data.name : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <span className="record-value">
                    {data ? data.description : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Contracting Date:</span>
                  <span className="record-value">
                    {data
                      ? new Date(data.contractingDate).toLocaleDateString()
                      : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <span className="record-value">
                    {data ? data.status : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">SLA:</span>
                  <span className="record-value">
                    {data ? data.sla : "Loading..."}
                  </span>
                </div>
              </div>
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default ServiceDetails;
