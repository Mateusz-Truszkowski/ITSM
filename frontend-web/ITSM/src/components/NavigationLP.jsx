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
        <div className="navigation-item">
          <Link to="/">About</Link>
        </div>
        <div className="navigation-item">
          <Link to="/">Contact</Link>
        </div>
        <div className="navigation-item">
          <Link to="/login">Login</Link> | <Link to="/">Register</Link>
        </div>
      </div>
    </section>
  );
}

export default NavigationLP;
