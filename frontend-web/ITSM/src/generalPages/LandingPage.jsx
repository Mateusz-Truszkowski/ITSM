import "../assets/FooterLP.css";
import "../assets/HeaderLP.css";
import "../assets/GeneralLP.css";
import "../assets/FormLP.css";
import "../assets/Navigation.css";
import { Link } from "react-router-dom";
import { useState } from "react";
import logo from "../assets/images/logo.png";

const cardData = [
  {
    id: 1,
    title: "Incident Management",
    description:
        "Quickly restore normal service operations and minimize disruption to business functions.",
  },
  {
    id: 2,
    title: "Service Request Management",
    description:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    id: 3,
    title: "Problem Management",
    description:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    id: 4,
    title: "Change Management",
    description:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    id: 5,
    title: "Knowledge Management",
    description:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    id: 6,
    title: "Asset & Configuration Management",
    description:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
];

function LandingPage() {
  const [activeCard, setActiveCard] = useState(null);

  const toggleCard = (id) => {
    setActiveCard((prev) => (prev === id ? null : id));
  };

  return (
      <>
        <section className="header">
          <img src={logo} alt="ITSM Logo" />
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

        <div className="slogan">
          We are just simply <span className="buzzword">THE BEST</span>
        </div>

        <section className="itsm-section">
          {cardData.map((card) => (
              <div
                  key={card.id}
                  className={`itsm-card ${activeCard === card.id ? "active" : ""}`}
                  onClick={() => toggleCard(card.id)}
              >
                <div className="itsm-number">{String(card.id).padStart(2, "0")}</div>
                <div className="itsm-title">{card.title}</div>
                <div className="itsm-description">{card.description}</div>
              </div>
          ))}
        </section>
      </>
  );
}

export default LandingPage;
