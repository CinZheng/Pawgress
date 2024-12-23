import React, { useState, useEffect } from "react";
import axios from "axios";
import { TextField, Button, List, ListItem, Typography, Box } from "@mui/material";

const QuizEditor = ({ quizId }) => {
  const [questions, setQuestions] = useState([]);
  const [newQuestion, setNewQuestion] = useState({
    questionText: "",
    correctAnswer: "",
    mediaUrl: "",
  });
  const [error, setError] = useState("");

  // basis URL voor de API
  const baseURL = "http://localhost:5232";

  // haal bestaande vragen op
  useEffect(() => {
    axios
      .get(`${baseURL}/api/Quiz/${quizId}/questions`)
      .then((response) => {
        setQuestions(response.data);
      })
      .catch((error) => {
        console.error("Fout bij het ophalen van vragen:", error);
      });
  }, [quizId]);

  // voeg een nieuwe vraag toe
  const handleAddQuestion = () => {
    // validatie
    if (!newQuestion.questionText || !newQuestion.correctAnswer) {
      setError("Vraagtekst en Correct Antwoord zijn verplicht.");
      return;
    }

    setError("");
    axios
      .post(`${baseURL}/api/Quiz/add-question`, {
        ...newQuestion,
        quizId: quizId,
      })
      .then((response) => {
        setQuestions([...questions, newQuestion]);
        setNewQuestion({
          questionText: "",
          correctAnswer: "",
          mediaUrl: "",
        });
      })
      .catch((error) => {
        console.error("Fout bij het toevoegen van de vraag:", error);
      });
  };

  return (
    <Box sx={{ padding: 4 }}>
      <Typography variant="h4" gutterBottom>
        Quiz Editor
      </Typography>

      {/* Formulier voor nieuwe vraag */}
      <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, marginBottom: 4 }}>
        <TextField
          label="Vraagtekst"
          value={newQuestion.questionText}
          onChange={(e) => setNewQuestion({ ...newQuestion, questionText: e.target.value })}
          required
        />
        <TextField
          label="Correct Antwoord"
          value={newQuestion.correctAnswer}
          onChange={(e) => setNewQuestion({ ...newQuestion, correctAnswer: e.target.value })}
          required
        />
        <TextField
          label="Media URL"
          value={newQuestion.mediaUrl}
          onChange={(e) => setNewQuestion({ ...newQuestion, mediaUrl: e.target.value })}
        />
        {error && (
          <Typography color="error" variant="body2">
            {error}
          </Typography>
        )}
        <Button variant="contained" onClick={handleAddQuestion}>
          Voeg Vraag Toe
        </Button>
      </Box>

      {/* Lijst van bestaande vragen */}
      <Typography variant="h5" gutterBottom>
        Bestaande Vragen
      </Typography>
      <List>
        {questions.map((q, index) => (
          <ListItem key={index}>
            {index + 1}. {q.questionText}
          </ListItem>
        ))}
      </List>
    </Box>
  );
};

export default QuizEditor;
