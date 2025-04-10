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
<<<<<<< HEAD
        {({ data, openRecord, isLoading }) => (
          <div>{/*nic tu nie ma co sie gapisz*/}</div>
        )}
=======
        {({ data, openRecord, isLoading }) => <div>{/* co sie paczysz */}</div>}
>>>>>>> BugFixes
      </MainPanel>
    </>
  );
}

export default Dashboard;
