import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import QuizEditor from "./pages/QuizEditor";
import Navbar from "./components/Navbar";


function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/modules" element={<div>Modules Pagina</div>} />
        <Route path="/dogs" element={<div>Honden Pagina</div>} />
        <Route path="/library" element={<div>Bibliotheek Pagina</div>} />
        <Route path="/profile" element={<div>Profiel Pagina</div>} />
        <Route path="/quiz-editor" element={<QuizEditor />} />
      </Routes>
    </Router>
  );
}

export default App;
