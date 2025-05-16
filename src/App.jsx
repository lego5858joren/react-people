import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import AdminPage from "./pages/AdminPage";
import UserPage from "./pages/UserPage";
import ProfilePage from "./pages/ProfilePage";

function App() {
  const user = localStorage.getItem("loggedInUser");

  return (
    <Routes>
      <Route path="/" element={<LoginPage />} />
      <Route
        path="/admin"
        element={user === "admin" ? <AdminPage /> : <Navigate to="/" />}
      />
      <Route
        path="/user"
        element={user && user !== "admin" ? <UserPage /> : <Navigate to="/" />}
      />
      <Route
    path="/profile/:id"
    element={user === "admin" ? <ProfilePage /> : <Navigate to="/" />}
  />
    </Routes>
  );
}

export default App;

