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
          <div>{/* Twoja logika wyświetlania danych */}</div>
        )}
      </MainPanel>
    </>
  );
}

export default Dashboard;
