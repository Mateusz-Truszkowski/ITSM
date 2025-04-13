import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./assets/App.css";
import LandingPage from "./pages/LandingPage.jsx";
import Tickets from "./pages/Tickets.jsx";
import TicketDetails from "./pages/TicketDetails.jsx";
import Users from "./pages/Users.jsx";
import UserDetails from "./pages/UserDetails.jsx";
import Services from "./pages/Services.jsx";
import ServiceDetails from "./pages/ServiceDetails.jsx";
import Devices from "./pages/Devices.jsx";
import DeviceDetails from "./pages/DeviceDetails.jsx";
import Login from "./pages/Login.jsx";
import PasswordReset from "./pages/PasswordReset.jsx";
import Dashboard from "./pages/Dashboard.jsx";
import PasswordResetFill from "./pages/PasswordResetFill.jsx";
import LoggedOut from "./pages/LoggedOut.jsx";

function App() {
  return (
    <main>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LandingPage />} /> {/* Strona główna */}
          <Route path="/tickets" element={<Tickets />} />
          <Route path="/tickets/:ticketId" element={<TicketDetails />} />
          <Route path="/users" element={<Users />} />
          <Route path="/users/:userId" element={<UserDetails />} />
          <Route path="/services" element={<Services />} />
          <Route path="/services/:serviceId" element={<ServiceDetails />} />
          <Route path="/devices" element={<Devices />} />
          <Route path="/devices/:deviceId" element={<DeviceDetails />} />
          <Route path="/PasswordReset" element={<PasswordReset />} />
          <Route path="/PasswordResetFill" element={<PasswordResetFill />} />
          <Route path="/login" element={<Login />} /> {/* Strona logowania */}
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/logged-out" element={<LoggedOut />} />
        </Routes>
      </BrowserRouter>
    </main>
  );
}

export default App;
