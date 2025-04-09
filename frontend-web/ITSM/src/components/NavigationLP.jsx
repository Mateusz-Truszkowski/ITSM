import { Link } from "react-router-dom";
import "../assets/HeaderLP.css";
import logo from "../assets/images/logo.png";

function NavigationLP() {
  return (
    <section className="header">
      <Link className="logo-link" to="/">
        <img src={logo} alt="Logo" />
      </Link>
      <div className="navigation-bar">
      <Link to="/">
          <div className="navigation-item">
            About
          </div>
        </Link>
        <Link to="/">
          <div className="navigation-item">
            Contact
          </div>
        </Link>
        <Link to="/login">
          <div className="navigation-item">
            Login | Register
          </div>
        </Link>
      </div>
    </section>
  );
}

export default NavigationLP;
