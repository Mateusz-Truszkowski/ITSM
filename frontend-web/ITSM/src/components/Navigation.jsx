import "../assets/Navigation.css";
import { Link } from "react-router-dom";

function Navigation() {
  return (
    <nav>
      <Link to="/tickets">Zgłoszenia</Link>
      <Link to="/services">Usługi</Link>
      <Link to="/devices">Urządzenia</Link>
      <Link to="/users">Użytkownicy</Link>
    </nav>
  );
}

export default Navigation;
