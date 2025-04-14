import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./assets/App.css";
import LandingPage from "./generalPages/LandingPage.jsx";
import Tickets from "./generalPages/Tickets.jsx";
import TicketDetails from "./recordDetailsPages/TicketDetails.jsx";
import Users from "./generalPages/Users.jsx";
import UserDetails from "./recordDetailsPages/UserDetails.jsx";
import Services from "./generalPages/Services.jsx";
import ServiceDetails from "./recordDetailsPages/ServiceDetails.jsx";
import Devices from "./generalPages/Devices.jsx";
import DeviceDetails from "./recordDetailsPages/DeviceDetails.jsx";
import Login from "./generalPages/Login.jsx";
import PasswordReset from "./generalPages/PasswordReset.jsx";
import Dashboard from "./generalPages/Dashboard.jsx";
import PasswordResetFill from "./generalPages/PasswordResetFill.jsx";
import LoggedOut from "./generalPages/LoggedOut.jsx";
import DeviceDetailsEdit from "./recordEditPages/DeviceDetailsEdit.jsx";
import ServiceDetailsEdit from "./recordEditPages/ServiceDetailsEdit.jsx";
import TicketDetailsEdit from "./recordEditPages/TicketDetailsEdit.jsx";
import UserDetailsEdit from "./recordEditPages/UserDetailsEdit.jsx";
import TicketCreate from "./createNewPages/TicketCreate.jsx";

function App() {
  return (
    <main>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LandingPage />} /> {/* Strona główna */}
          <Route path="/tickets" element={<Tickets />} />
          <Route path="/tickets/:ticketId" element={<TicketDetails />} />
          <Route path="/tickets/create" element={<TicketCreate />} />
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
          <Route
            path="/devices/:deviceId/edit"
            element={<DeviceDetailsEdit />}
          />
          <Route
            path="/services/:serviceId/edit"
            element={<ServiceDetailsEdit />}
          />
          <Route
            path="/tickets/:ticketId/edit"
            element={<TicketDetailsEdit />}
          />
          <Route path="/users/:userId/edit" element={<UserDetailsEdit />} />
        </Routes>
      </BrowserRouter>
    </main>
  );
}

export default App;
