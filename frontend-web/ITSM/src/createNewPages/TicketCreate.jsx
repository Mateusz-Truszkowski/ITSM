// TO FINISH

import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import NavigationLP from "../components/NavigationLP";
import MainPanel from "../components/MainPanel";
import "../assets/RecordDetails.css";
import { Link } from "react-router-dom";
import { fetchTicket } from "../hooks/tickets";
import { fetchUser } from "../hooks/users";
import { useCheckTokenValidity } from "../global";

function TicketCreate() {
  const checkToken = useCheckTokenValidity();

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
  }, []);

  const saveRecord = () => {};

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="record-details-wrapper">
            <div className="record-details-container">
              <div className="record-details-header">
                <h1>Create new ticket</h1>
                <button className="edit-button" onClick={saveRecord}>
                  Save
                </button>
              </div>
              <div className="record-fields">
                <div className="record-field">
                  <span className="record-label">Name:</span>
                  <input className="record-value-edit" type="text" />
                </div>
                <div className="record-field">
                  <span className="record-label">Description:</span>
                  <textarea className="record-value-edit" type="text" />
                </div>
                <div className="record-field">
                  <span className="record-label">Created:</span>
                  <input className="record-value-edit" type="date" />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Date:</span>
                  <input className="record-value-edit" type="date" />
                </div>
                <div className="record-field">
                  <span className="record-label">Solution Description:</span>
                  <textarea className="record-value-edit" />
                </div>
                <div className="record-field">
                  <span className="record-label">Priority:</span>
                  <input className="record-value-edit" type="number" />
                </div>
                <div className="record-field">
                  <span className="record-label">Type:</span>
                  <select className="record-value-edit">
                    <option value="Bug">Bug</option>
                    <option value="Performance">Performance</option>
                    <option value="Support">Support</option>
                    <option value="Feature Request">Feature Request</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Status:</span>
                  <select className="record-value-edit">
                    <option value="Open">Open</option>
                    <option value="In Progress">In Progress</option>
                    <option value="Closed">Closed</option>
                  </select>
                </div>
                <div className="record-field">
                  <span className="record-label">Service ID:</span>
                  <input className="record-value-edit" type="number" />
                </div>
                <div className="record-field">
                  <span className="record-label">Assignee:</span>
                  <input className="record-value-edit" type="number" />
                </div>
                <div className="record-field">
                  <span className="record-label">Requester:</span>
                  <input className="record-value-edit" type="number" />
                </div>
              </div>
            </div>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default TicketCreate;
