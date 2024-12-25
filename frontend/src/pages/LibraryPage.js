import React, { useState, useEffect } from "react";
import { Button, Container } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";

const LibraryPage = () => {
  const navigate = useNavigate();
  const [isUserAdmin, setIsUserAdmin] = useState(false);

  useEffect(() => {
    // check of de gebruiker een admin is
    setIsUserAdmin(isAdmin());
  }, []); 

  return (
    <Container>
      <h1>Bibliotheek</h1>
      {isUserAdmin && ( // toon alleen knoppen als admin
        <div>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate("/quiz-editor")}
          >
            Quiz Editor
          </Button>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate("/lesson-editor")}
          >
            Lesson Editor
          </Button>
        </div>
      )}
    </Container>
  );
};

console.log("admin?:" + isAdmin());
console.log(localStorage.getItem("token"))

export default LibraryPage;
