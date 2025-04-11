import React, { useEffect, useState } from "react";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";
import { fetchUsers } from "../hooks/users.js";

function Users() {
  const [users, setUsers] = useState([]);

  const displayUsers = async () => {
    const usersData = await fetchUsers();

    setUsers(usersData);
  };

  useEffect(() => {
    displayUsers();
  }, []);

  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, isLoading, openRecord }) => (
          <div className="records-container">
            <h2 className="records-header">Users</h2>
            {isLoading ? (
              <div className="loading-spinner">
                <div className="spinner"></div>
              </div>
            ) : users && users.length > 0 ? (
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
                  {users.map((user) => (
                    <tr onClick={() => openRecord(user.id)} key={user.id}>
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
