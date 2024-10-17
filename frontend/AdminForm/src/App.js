import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import AdminForm from "./Compornent/AdminForm";
import CompanyRegister from "./Compornent/CompanyRegister";
import QR from "./Compornent/QR";
import ProtectedRoute from "./Compornent/ProtectRoute";

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/" element={<CompanyRegister />} />
          <Route
            path="/admin/:id"
            element={
              <ProtectedRoute>
                <AdminForm />
              </ProtectedRoute>
            }
          />
          <Route
            path="/QRcode/:id"
            element={
              <ProtectedRoute>
                <QR />
              </ProtectedRoute>
            }
          />
          {/* <Route path="/admin/:id" element={<AdminForm />} /> */}

          {/* <Route path="/QRcode/:id" element={<QR />} /> */}
        </Routes>
      </div>
    </Router>
  );
}

export default App;
