import React from "react";
import { Navigate } from "react-router-dom";

const ProtectedRoute = ({ children, id }) => {
  const token = localStorage.getItem("token");
  console.log("Is Authenticated:", token);

  if (!token) {
    return <Navigate to="/" />;
  }

  return children;
};

export default ProtectedRoute;
