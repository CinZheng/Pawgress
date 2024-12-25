import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import QuizEditorPage from "./pages/QuizEditorPage";
import Navbar from "./components/Navbar";
import ModuleOverviewPage from "./pages/ModuleOverviewPage";
import ModuleDetailsPage from "./pages/ModuleDetailsPage";
import DogprofileOverviewPage from "./pages/DogProfileOverviewPage";
import DogprofileDetailsPage from "./pages/DogProfileDetailsPage";
import ProfilePage from "./pages/ProfilePage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import { isAuthenticated } from "./utils/auth";
import PrivateRoute from "./components/PrivateRoute";

function App() {
  const isLoggedIn = isAuthenticated();

  return (
    <Router>
      {isLoggedIn && <Navbar />}
      <Routes>
        {/* Openbare routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        {/* Beveiligde routes */}
        <Route
          path="/library"
          element={
            <PrivateRoute>
              <div>Bibliotheek Pagina</div>
            </PrivateRoute>
          }
        />
        <Route
          path="/profile"
          element={
            <PrivateRoute>
              <ProfilePage />
            </PrivateRoute>
          }
        />
        <Route
          path="/modules"
          element={
            <PrivateRoute>
              <ModuleOverviewPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/modules/:id"
          element={
            <PrivateRoute>
              <ModuleDetailsPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/dogprofiles"
          element={
            <PrivateRoute>
              <DogprofileOverviewPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/dogprofiles/:id"
          element={
            <PrivateRoute>
              <DogprofileDetailsPage />
            </PrivateRoute>
          }
        />
      </Routes>
    </Router>
  );
}

export default App;

