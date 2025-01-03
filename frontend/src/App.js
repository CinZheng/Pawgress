import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Route, Routes, useLocation } from "react-router-dom";
import QuizEditorPage from "./pages/QuizEditorPage";
import LessonEditorPage from "./pages/LessonEditorPage";
import LessonOverviewPage from "./pages/LessonOverviewPage";
import LessonDetailsPage from "./pages/LessonDetailsPage";
import Navbar from "./components/Navbar";
import ModuleOverviewPage from "./pages/ModuleOverviewPage";
import ModuleDetailsPage from "./pages/ModuleDetailsPage";
import DogprofileOverviewPage from "./pages/DogProfileOverviewPage";
import DogprofileDetailsPage from "./pages/DogProfileDetailsPage";
import ProfilePage from "./pages/ProfilePage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import PrivateRoute from "./components/PrivateRoute";
import LibraryPage from "./pages/LibraryPage";
import { isAuthenticated } from "./utils/auth";
import { isAdmin } from "./utils/auth";
import DogProfileEditorPage from "./pages/DogProfileEditorPage";
import SensorDataForm from "./pages/SensorDataForm";
import SensorDataPage from "./pages/SensorDataPage";
import QuizPage from "./pages/QuizPage";
import QuizDetailsPage from "./pages/QuizDetailsPage";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(isAuthenticated());

  // Controleer de loginstatus opnieuw wanneer de component mount
  useEffect(() => {
    setIsLoggedIn(isAuthenticated());
  }, []);

  return (
    <Router>
      <NavbarWrapper isLoggedIn={isLoggedIn} />
      <Routes>
        {/* Openbare routes */}
        <Route path="/login" element={<LoginPage onLogin={() => setIsLoggedIn(true)} />} />
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
            <SensorDataForm />
          } />
          <Route 
          path="/sensor-data" 
          element={
            <SensorDataPage />
          } />
      </Routes>
    </Router>
  );
}

const NavbarWrapper = ({ isLoggedIn }) => {
  const location = useLocation(); // gebruik de hook binnen Router-context
  const hideNavbarRoutes = ["/login", "/register"];
  const shouldHideNavbar = hideNavbarRoutes.includes(location.pathname);

  if (!isLoggedIn || shouldHideNavbar) return null; // verberg Navbar op specifieke routes of als niet ingelogd

  return <Navbar />;
};

export default App;
