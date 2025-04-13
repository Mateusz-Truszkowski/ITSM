import "../assets/GeneralLP.css";
import NavigationLP from "../components/NavigationLP.jsx";
import "../assets/MainPanel.css";
import MainPanel from "../components/MainPanel.jsx";

function LoggedOut() {
  return (
    <>
      <NavigationLP />
      <MainPanel>
        {({}) => (
          <div className="unauthorized">
            <p className="unauthorized-message">
              You have been logged out since your token has expired.
            </p>
            <p className="unauthorized-login">
              PLEASE{" "}
              <Link to="/login">
                <span className="message-login">LOG IN</span>
              </Link>
            </p>
          </div>
        )}
      </MainPanel>
    </>
  );
}

export default LoggedOut;
