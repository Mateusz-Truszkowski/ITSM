import { BrowserRouter, Routes, Route } from 'react-router-dom'
import './assets/App.css'
import Home from './pages/Home.jsx'
import Tickets from './pages/Tickets.jsx'
import Users from './pages/Users.jsx'
import Services from './pages/Services.jsx'
import Devices from './pages/Devices.jsx'

function App() {
  return (
    <main>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />}/>
          <Route path="/tickets" element={<Tickets />}/>
          <Route path="/users" element={<Users />}/>
          <Route path="/services" element={<Services />}/>
          <Route path="/devices" element={<Devices />}/>
        </Routes>
      </BrowserRouter>
    </main>
  )
}

export default App
