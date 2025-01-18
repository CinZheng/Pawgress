import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Route, Routes, useLocation, Navigate } from "react-router-dom";
import { NotificationProvider } from "./context/NotificationContext";
import QuizEditorPage from "./pages/QuizEditorPage";
import LessonEditorPage from "./pages/LessonEditorPage";
import LessonOverviewPage from "./pages/LessonOverviewPage";
import LessonDetailsPage from "./pages/LessonDetailsPage";
import Navbar from "./components/Navbar";
import ModuleOverviewPage from "./pages/ModuleOverviewPage";
import ModuleDetailsPage from "./pages/ModuleDetailsPage";
import DogprofileOverviewPage from "./pages/DogProfileOverviewPage";
import DogprofileDetailsPage from "./pages/DogProfileDetailsPage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import PrivateRoute from "./components/PrivateRoute";
import LibraryPage from "./pages/LibraryPage";
import { isAuthenticated } from "./utils/auth";
import { isAdmin } from "./utils/auth";
import DogProfileEditorPage from "./pages/DogProfileEditorPage";
import SensorDataForm from "./pages/SensorDataForm";
import QuizPage from "./pages/QuizPage";
import QuizDetailsPage from "./pages/QuizDetailsPage";
import ModuleEditorPage from "./pages/ModuleEditorPage";
import ModuleResultPage from "./pages/ModuleResultPage";
import UserProfilePage from "./pages/UserProfilePage";
import UserProfileEditorPage from "./pages/UserProfileEditorPage";
import DogSensorDataPage from './pages/DogSensorDataPage';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    // Initial check
    setIsLoggedIn(isAuthenticated());

    // Set up an interval to check authentication status
    const checkAuthStatus = () => {
      const authStatus = isAuthenticated();
      setIsLoggedIn(authStatus);
    };

    // Check auth status every 1 second
    const interval = setInterval(checkAuthStatus, 1000);

    // Cleanup interval on unmount
    return () => clearInterval(interval);
  }, []);

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    setIsLoggedIn(false);
  };

  return (
    <NotificationProvider>
      <Router>
        <Navbar isLoggedIn={isLoggedIn} onLogout={handleLogout} />
        <Routes>
          {/* Root route redirect */}
          <Route path="/" element={
            isLoggedIn ? <Navigate to="/modules" replace /> : <Navigate to="/login" replace />
          } />

          {/* Openbare routes */}
          <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
          <Route path="/register" element={<RegisterPage />} />

          {/* Beveiligde routes */}
          <Route
            path="/library"
            element={
              <PrivateRoute>
                <LibraryPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/profile"
            element={
              <PrivateRoute>
                <UserProfilePage />
              </PrivateRoute>
            }
          />
          <Route
            path="/profile/edit"
            element={
              <PrivateRoute>
                <UserProfileEditorPage />
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
            path="/modules/:id/result"
            element={
              <PrivateRoute>
                <ModuleResultPage />
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
          <Route
            path="/lessons"
            element={
              <PrivateRoute>
                <LessonOverviewPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/lessons/:id"
            element={
              <PrivateRoute>
                <LessonDetailsPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/quizzes/:id"
            element={
              <PrivateRoute>
                <QuizDetailsPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/quiz/:id"
            element={
              <PrivateRoute>
                <QuizPage />
              </PrivateRoute>
            }
          />

          {/* Beveiligde routes alleen voor admins */}
          <Route
            path="/quiz-editor"
            element={
              <PrivateRoute>
                {isAdmin() ? (
                  <QuizEditorPage />
                ) : (
                  <div>U heeft geen toegang tot deze pagina.</div>
                )}
              </PrivateRoute>
            }
          />
          <Route
            path="/lesson-editor"
            element={
              <PrivateRoute>
                {isAdmin() ? (
                  <LessonEditorPage />
                ) : (
                  <div>U heeft geen toegang tot deze pagina.</div>
                )}
              </PrivateRoute>
            }
          />
          <Route
            path="/module-editor"
            element={
              <PrivateRoute>
                {isAdmin() ? (
                  <ModuleEditorPage />
                ) : (
                  <div>U heeft geen toegang tot deze pagina.</div>
                )}
              </PrivateRoute>
            }
          />
          <Route
            path="/dogprofile-editor"
            element={
              <PrivateRoute>
                {isAdmin() ? (
                  <DogProfileEditorPage />
                ) : (
                  <div>U heeft geen toegang tot deze pagina.</div>
                )}
              </PrivateRoute>
            }
          />
          <Route
            path="/sensor-data-form"
            element={
              <PrivateRoute>
                {isAdmin() ? (
                  <SensorDataForm />
                ) : (
                  <div>U heeft geen toegang tot deze pagina.</div>
                )}
              </PrivateRoute>
            }
          />
          <Route path="/dogprofiles/:dogProfileId/sensor-data" element={<DogSensorDataPage />} />

          {/* Catch all route */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </NotificationProvider>
  );
}

export default App;
