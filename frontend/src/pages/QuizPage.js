import React, { useState, useEffect } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import {
  Container,
  Typography,
  Button,
  Box,
  Alert,
} from "@mui/material";
import axiosInstance from "../axios";
import Layout from "../components/Layout";

const QuizPage = () => {
  const { id } = useParams(); 
  const navigate = useNavigate();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const moduleId = queryParams.get("moduleId");
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
      if (moduleId) {
        navigate(`/modules/${moduleId}`);
      } else {
        navigate("/library");
      }
    }
  };

  if (loading) {
    return (
      <Layout>
        <Container maxWidth="md">
          <Box sx={{ mt: 4 }}>
            <Typography>Quiz wordt geladen...</Typography>
          </Box>
        </Container>
      </Layout>
    );
  }

  if (error) {
    return (
      <Layout>
        <Container maxWidth="md">
          <Box sx={{ mt: 4 }}>
            <Alert severity="error">{error}</Alert>
          </Box>
        </Container>
      </Layout>
    );
  }

  if (!questions.length) {
    return (
      <Layout>
        <Container maxWidth="md">
          <Box sx={{ mt: 4 }}>
            <Typography>Geen vragen beschikbaar voor deze quiz.</Typography>
          </Box>
        </Container>
      </Layout>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <Layout>
      <Container maxWidth="md">
        <Box sx={{ mt: 4, mb: 6 }}>
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
        </Box>
      </Container>
    </Layout>
  );
};

export default QuizPage;
