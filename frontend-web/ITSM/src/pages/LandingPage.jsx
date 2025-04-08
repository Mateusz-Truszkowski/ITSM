import "../assets/FooterLP.css";
import "../assets/HeaderLP.css";
import "../assets/GeneralLP.css";
import NavigationLP from "../components/NavigationLP";

function LandingPage() {
  return (
    <>
      <NavigationLP />
      <div className="slogan">
        We are just simply <span className="buzzword">THE BEST</span>
      </div>
      <section className="itsm-section">
        <div className="itsm-card-1">
          <div className="itsm-number">01</div>
          <div className="itsm-title">Incident Management</div>
          <div className="itsm-description">
            Quickly restore normal service operations and minimize disruption to
            business functions.
          </div>
        </div>
        <div className="itsm-card">
          <div className="itsm-number">02</div>
          <div className="itsm-title">Service Request Management</div>
        </div>
        <div className="itsm-card">
          <div className="itsm-number">03</div>
          <div className="itsm-title">Problem Management</div>
        </div>
        <div className="itsm-card">
          <div className="itsm-number">04</div>
          <div className="itsm-title">Change Management</div>
        </div>
        <div className="itsm-card">
          <div className="itsm-number">05</div>
          <div className="itsm-title">Knowledge Management</div>
        </div>
        <div className="itsm-card">
          <div className="itsm-number">06</div>
          <div className="itsm-title">Asset & Configuration Management</div>
        </div>
      </section>
    </>
  );
}

export default LandingPage;
