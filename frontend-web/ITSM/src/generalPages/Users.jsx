import React, { useEffect, useState } from "react";
import "../assets/GeneralLP.css";
import "../assets/MainPanel.css";
import "../assets/Users.css";
import NavigationLP from "../components/NavigationLP.jsx";
import MainPanel from "../components/MainPanel";
import { fetchUsers,fetchUsersReport } from "../hooks/users.js";
import { useCheckTokenValidity } from "../global";
import { useNavigate } from "react-router-dom";

function Users() {
  const [users, setUsers] = useState([]);
  const checkToken = useCheckTokenValidity();
  const [isLoading, setIsLoading] = useState(true);

  const displayUsers = async () => {
    try {
      const usersData = await fetchUsers();

      if (usersData === null) {
        throw new Error("error fetching users");
      }
      setUsers(usersData);
      setIsLoading(false);
    } catch (error) {
      console.log("Error occured: " + error);
    }
  };
  const MakeReport = async () => {
    try {
      const Data = await fetchUsersReport();

      if (Data === null) {
        throw new Error("Report creating error");
      }
    } catch (error) {
      console.log("Error occured: " + error);
    }
  };
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    const isTokenValid = checkToken(token);

    isTokenValid;
    displayUsers();
  }, []);

  const navigate = useNavigate();
  const CreateUser = () => {
  navigate("/users/create");
};


  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ openRecord }) => (
          <div className="records-container">
            <h2 className="records-header">Users</h2>
            <button className="report-button" onClick={CreateUser}>
             New
            </button>
            <button className="report-button" onClick={MakeReport}>
              Download report
            </button>
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
            {users.length === 0 && !isLoading && (
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
