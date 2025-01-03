import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
  Container,
  Typography,
  Button,
  Box,
  TextField,
} from "@mui/material";
import axiosInstance from "../axios";

const QuizPage = () => {
  const { id } = useParams();
  const [questions, setQuestions] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [userAnswer, setUserAnswer] = useState("");
  const [results, setResults] = useState(null);

  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const response = await axiosInstance.get(`/api/Quiz/${id}/questions`);
        setQuestions(response.data);
      } catch (error) {
        console.error("Fout bij het ophalen van quizvragen:", error);
      }
    };

    fetchQuestions();
  }, [id]);

  const handleAnswerSubmit = () => {
    if (questions[currentIndex].correctAnswer === userAnswer) {
      alert("Correct!");
    } else {
      alert("Onjuist.");
    }

    if (currentIndex + 1 < questions.length) {
      setCurrentIndex(currentIndex + 1);
      setUserAnswer("");
    } else {
      setResults("Quiz voltooid!");
    }
  };

  if (results) {
    return (
      <Container>
        <Typography variant="h4">Resultaat</Typography>
        <Typography>{results}</Typography>
      </Container>
    );
  }

  if (!questions.length) {
    return <Typography>Quiz wordt geladen...</Typography>;
  }

  const currentQuestion = questions[currentIndex];

  return (
    <Container>
      <Typography variant="h4">{currentQuestion.text}</Typography>
      {currentQuestion.mediaUrl && (
        <Box>
          <img
            src={currentQuestion.mediaUrl}
            alt="media"
            style={{ maxWidth: "100%", marginTop: "16px" }}
          />
        </Box>
      )}
      <TextField
        label="Uw antwoord"
        fullWidth
        value={userAnswer}
        onChange={(e) => setUserAnswer(e.target.value)}
        margin="normal"
      />
      <Button variant="contained" onClick={handleAnswerSubmit}>
        Bevestig Antwoord
      </Button>
    </Container>
  );
};

export default QuizPage;
