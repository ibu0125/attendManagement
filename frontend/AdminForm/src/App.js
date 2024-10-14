import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import AdminForm from "./Compornent/AdminForm";
import CompanyRegister from "./Compornent/CompanyRegister";
import QR from "./Compornent/QR";

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/admin/:id" element={<AdminForm />} />
          <Route path="/" element={<CompanyRegister />} />
          <Route path="/QRcode/:id" element={<QR />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
