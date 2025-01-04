import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  Container,
  Typography,
  Button,
  Box,
  Alert,
} from "@mui/material";
import axiosInstance from "../axios";

const QuizPage = () => {
  const { id } = useParams(); 
  const navigate = useNavigate();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchQuestions = async () => {
      setLoading(true);
      try {
        const response = await axiosInstance.get(`/api/Quiz/${id}/questions`);
        setQuestions(response.data);
      } catch (err) {
        console.error("Error fetching questions:", err);
        setError("Kon quizvragen niet ophalen.");
      } finally {
        setLoading(false);
      }
    };

    fetchQuestions();
  }, [id]);

  const handleNextQuestion = () => {
    setShowAnswer(false);
    if (currentQuestionIndex + 1 < questions.length) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    } else {
      navigate("/library");
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
        <Typography variant="h6">Quiz wordt geladen...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ marginTop: "50px" }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (!questions.length) {
    return (
      <Container maxWidth="md" sx={{ marginTop: "50px" }}>
        <Typography variant="h6">Geen vragen beschikbaar voor deze quiz.</Typography>
      </Container>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Quiz
      </Typography>
      <Box
        sx={{
          border: "1px solid #ccc",
          borderRadius: "4px",
          padding: "16px",
          backgroundColor: "#f9f9f9",
          marginBottom: "16px",
        }}
      >
        <Typography variant="h6">Vraag {currentQuestionIndex + 1}</Typography>
        <Typography variant="body1" sx={{ marginTop: 2 }}>
          {currentQuestion.questionText}
        </Typography>
        {currentQuestion.mediaUrl && (
          <Box sx={{ marginTop: 2 }}>
            <img
              src={currentQuestion.mediaUrl}
              alt="Media"
              style={{ maxWidth: "100%" }}
            />
          </Box>
        )}
        {showAnswer && (
          <Typography
            variant="body2"
            sx={{ marginTop: 2, color: "green", fontWeight: "bold" }}
          >
            Correct Antwoord: {currentQuestion.correctAnswer}
          </Typography>
        )}
      </Box>
      <Box sx={{ display: "flex", gap: 2 }}>
        {!showAnswer && (
          <Button
            variant="contained"
            color="secondary"
            onClick={() => setShowAnswer(true)}
          >
            Toon Antwoord
          </Button>
        )}
        {showAnswer && (
          <Button
            variant="contained"
            color="primary"
            onClick={handleNextQuestion}
          >
            Volgende Vraag
          </Button>
        )}
      </Box>
    </Container>
  );
};

export default QuizPage;
