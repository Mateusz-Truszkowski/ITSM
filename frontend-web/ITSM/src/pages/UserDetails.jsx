import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";

function UserDetails() {
  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, openRecord, isLoading }) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <h1 className="record-details-header">User Details</h1>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <span className="record-value">
                    {data ? data.name : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Surname:</span>
                  <span className="record-value">
                    {data ? data.surname : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Email:</span>
                  <span className="record-value">
                    {data ? data.email : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Created:</span>
                  <span className="record-value">
                    {data
                      ? new Date(data.creationDate).toLocaleDateString()
                      : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Group:</span>
                  <span className="record-value">
                    {data ? data.group : "Loading..."}
                  </span>
                </div>
                <div className="record-field">
                  <span className="record-label">Occupation:</span>
                  <span className="record-value">
                    {data ? data.occupation : "Loading..."}
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

export default UserDetails;
