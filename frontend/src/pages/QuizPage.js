import React, { useState, useEffect } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import {
  Container,
  Typography,
  Button,
  Box,
  Alert,
  TextField,
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
  const [userAnswer, setUserAnswer] = useState("");
  const [savedAnswer, setSavedAnswer] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const userId = localStorage.getItem("userId");

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

  useEffect(() => {
    const fetchSavedAnswer = async () => {
      if (!questions[currentQuestionIndex]) return;
      
      try {
        const response = await axiosInstance.get(
          `/api/Quiz/questions/${questions[currentQuestionIndex].quizQuestionId}/answer/${userId}`
        );
        setSavedAnswer(response.data);
        setUserAnswer(response.data.userAnswer);
        setShowAnswer(true);
      } catch (err) {
        if (err.response?.status !== 404) {
          console.error("Error fetching saved answer:", err);
        }
        setSavedAnswer(null);
        setUserAnswer("");
        setShowAnswer(false);
      }
      setIsEditing(false);
    };

    fetchSavedAnswer();
  }, [currentQuestionIndex, questions, userId]);

  const handleNextQuestion = () => {
    setShowAnswer(false);
    setUserAnswer("");
    setSavedAnswer(null);
    setIsEditing(false);
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

  const handleSubmitAnswer = async () => {
    if (!userAnswer.trim()) return;

    try {
      const response = await axiosInstance.post(
        `/api/Quiz/questions/${questions[currentQuestionIndex].quizQuestionId}/answer`,
        {
          userId: userId,
          answer: userAnswer
        }
      );
      setSavedAnswer(response.data);
      setShowAnswer(true);
      setIsEditing(false);
    } catch (err) {
      console.error("Error saving answer:", err);
      setError("Kon antwoord niet opslaan.");
    }
  };

  const handleEditAnswer = () => {
    setIsEditing(true);
    setShowAnswer(false);
  };

  if (loading) {
    return (
      <Layout>
        <Container maxWidth="lg">
          <Box sx={{ textAlign: "center" }}>
            <Typography variant="h6">Quiz wordt geladen...</Typography>
          </Box>
        </Container>
      </Layout>
    );
  }

  if (error) {
    return (
      <Layout>
        <Container maxWidth="lg">
          <Alert severity="error">{error}</Alert>
        </Container>
      </Layout>
    );
  }

  if (!questions.length) {
    return (
      <Layout>
        <Container maxWidth="lg">
          <Typography variant="h6">Geen vragen beschikbaar voor deze quiz.</Typography>
        </Container>
      </Layout>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <Layout>
      <Container maxWidth="lg">
        <Typography variant="h4" component="h1" gutterBottom>
          Quiz
        </Typography>
        <Box
          sx={{
            border: "1px solid #ccc",
            borderRadius: "4px",
            padding: 3,
            backgroundColor: "#f9f9f9",
            mb: 4,
          }}
        >
          <Typography variant="h5" gutterBottom>
            Vraag {currentQuestionIndex + 1} van {questions.length}
          </Typography>
          <Typography variant="body1" sx={{ mt: 2 }}>
            {currentQuestion.questionText}
          </Typography>
          {currentQuestion.mediaUrl && (
            <Box sx={{ mt: 3 }}>
              <img
                src={currentQuestion.mediaUrl}
                alt="Media"
                style={{ maxWidth: "100%" }}
              />
            </Box>
          )}

          {(!showAnswer || isEditing) && (
            <Box sx={{ mt: 3 }}>
              <TextField
                fullWidth
                label="Jouw antwoord"
                variant="outlined"
                value={userAnswer}
                onChange={(e) => setUserAnswer(e.target.value)}
              />
              <Button
                variant="contained"
                color="primary"
                onClick={handleSubmitAnswer}
                sx={{ mt: 2 }}
                disabled={!userAnswer.trim()}
              >
                {savedAnswer ? "Antwoord Bijwerken" : "Antwoord Indienen"}
              </Button>
            </Box>
          )}

          {showAnswer && !isEditing && (
            <Box sx={{ mt: 3 }}>
              <Typography variant="body1" sx={{ mt: 1 }}>
                Jouw antwoord: {savedAnswer?.userAnswer}
              </Typography>
              <Typography variant="body1" sx={{ mt: 1 }}>
                Het antwoord moeten de volgende punten bevatten: {currentQuestion.correctAnswer}
              </Typography>
              <Button
                variant="outlined"
                color="primary"
                onClick={handleEditAnswer}
                sx={{ mt: 2 }}
              >
                Antwoord Aanpassen
              </Button>
            </Box>
          )}
        </Box>

        <Box sx={{ display: "flex", gap: 2, justifyContent: "center" }}>
          {showAnswer && !isEditing && (
            <Button
              variant="contained"
              color="primary"
              size="large"
              onClick={handleNextQuestion}
            >
              {currentQuestionIndex + 1 < questions.length ? "Volgende Vraag" : "Quiz Afronden"}
            </Button>
          )}
        </Box>
      </Container>
    </Layout>
  );
};

export default QuizPage;
