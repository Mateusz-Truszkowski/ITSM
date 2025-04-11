import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link } from "react-router-dom";

function DeviceDetails() {
  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data }) => (
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
                  <span className="record-label">Acquisition Date:</span>
                  <span className="record-value">
                    {data
                      ? new Date(data.acquisitionDate).toLocaleDateString()
                      : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Depreciation Date:</span>
                  <span className="record-value">
                    {data
                      ? new Date(data.depreciationDate).toLocaleDateString()
                      : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">User Id:</span>
                  <span className="record-value">
                    {data ? (
                      <Link to={`/users/${data.userId}`}>{data.userId}</Link>
                    ) : (
                      "Loading..."
                    )}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <span className="record-value">
                    {data ? data.status : "Loading..."}
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

export default DeviceDetails;
