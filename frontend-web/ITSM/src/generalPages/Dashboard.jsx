/* eslint-disable no-unused-vars */
import "../assets/GeneralLP.css";
import NavigationLP from "../components/NavigationLP.jsx";
import "../assets/MainPanel.css";
import MainPanel from "../components/MainPanel.jsx";

function Dashboard() {
  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({ data, openRecord, isLoading }) => (
          <div>{/*nic tu nie ma co sie gapisz*/}</div>
        )}
      </MainPanel>
    </>
  );
}

export default Dashboard;
