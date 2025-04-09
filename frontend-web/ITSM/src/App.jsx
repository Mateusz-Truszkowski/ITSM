import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./assets/App.css";
import LandingPage from "./pages/LandingPage.jsx";
import Tickets from "./pages/Tickets.jsx";
import Users from "./pages/Users.jsx";
import Services from "./pages/Services.jsx";
import Devices from "./pages/Devices.jsx";
import Login from "./pages/Login.jsx";
import PasswordReset from "./pages/PasswordReset.jsx";

function App() {
  return (
    <main>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LandingPage />} /> {/* Strona główna */}
          <Route path="/tickets" element={<Tickets />} />
          <Route path="/users" element={<Users />} />
          <Route path="/services" element={<Services />} />
          <Route path="/devices" element={<Devices />} />
          <Route path="/PasswordReset" element={<PasswordReset />} /> {/* ResetPassword */}
          <Route path="/login" element={<Login />} /> {/* Strona logowania */}
        </Routes>
      </BrowserRouter>
    </main>
  );
}

export default App;
