import React from "react";
import { useNavigate } from "react-router-dom";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";

function Users() {
  const navigate = useNavigate();

  const openUser = async (userId) => {
    console.log("Otwarto użytkownika " + userId);
    navigate(`/users/${userId}`);
  };

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, isLoading }) => (
          <div className="records-container">
            <h2 className="records-header">Users</h2>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : data && data.length > 0 ? (
              <table className="records-table">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Email</th>
                    <th>Created</th>
                    <th>Group</th>
                    <th>Occupation</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((user) => (
                    <tr onClick={() => openUser(user.id)} key={user.id}>
                      <td>{user.id}</td>
                      <td>{user.name}</td>
                      <td>{user.surname}</td>
                      <td>{user.email}</td>
                      <td>
                        {new Date(user.creationDate).toLocaleDateString()}
                      </td>
                      <td>{user.group}</td>
                      <td>{user.occupation}</td>
                      <td>{user.status}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : null}
            {data.length === 0 && !isLoading && (
              <div className="no-records">
                <p>No tickets available</p>
              </div>
            )}
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default Users;
