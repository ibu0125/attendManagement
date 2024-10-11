import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import AdminForm from "./Compornent/AdminForm";
import CompanyRegister from "./Compornent/CompanyRegister";

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/admin" element={<AdminForm />} />
          <Route path="/" element={<CompanyRegister />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
