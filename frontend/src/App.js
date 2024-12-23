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



function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/library" element={<div>Bibliotheek Pagina</div>} />
        <Route path="/profile" element={< ProfilePage/>} />
        <Route path="/quiz-editor" element={<QuizEditorPage />} />
        <Route path="/modules" element={<ModuleOverviewPage />} />
        <Route path="/modules/:id" element={<ModuleDetailsPage />} />
        <Route path="/dogprofiles" element={<DogprofileOverviewPage />} />
        <Route path="/dogprofiles:id" element={<DogprofileDetailsPage />} />

      </Routes>
    </Router>
  );
}

export default App;
