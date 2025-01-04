import React, { useState, useEffect } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Card,
  CardContent,
  IconButton,
  Alert,
  CircularProgress,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useNavigate, useLocation } from "react-router-dom";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";

const QuizEditorPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const quizId = queryParams.get("id");

  const [quiz, setQuiz] = useState({
    quizName: "",
    quizDescription: "",
  });
  const [questions, setQuestions] = useState([]);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!isAdmin()) {
      navigate("/library");
      return;
    }

    const fetchQuiz = async () => {
      if (!quizId) return;

      setLoading(true);
      try {
        const response = await axiosInstance.get(`/api/Quiz/${quizId}`);
        setQuiz({
          quizName: response.data.quizName,
          quizDescription: response.data.quizDescription,
        });

        if (response.data.quizQuestions) {
          setQuestions(
            response.data.quizQuestions.map((q) => ({
              questionText: q.questionText,
              correctAnswer: q.correctAnswer,
              mediaUrl: q.mediaUrl || "",
              quizQuestionId: q.quizQuestionId,
            }))
          );
        }
      } catch (err) {
        console.error("Fout bij ophalen quiz:", err);
        setError("Kon quizgegevens niet ophalen.");
      } finally {
        setLoading(false);
      }
    };

    fetchQuiz();
  }, [quizId, navigate]);

  const handleQuizChange = (e) => {
    setQuiz({ ...quiz, [e.target.name]: e.target.value });
  };

  const handleQuestionChange = (index, field, value) => {
    const updatedQuestions = [...questions];
    updatedQuestions[index][field] = value;
    setQuestions(updatedQuestions);
  };

  const addQuestion = () => {
    setQuestions([...questions, { questionText: "", correctAnswer: "", mediaUrl: "" }]);
  };

  const removeQuestion = (index) => {
    const updatedQuestions = questions.filter((_, i) => i !== index);
    setQuestions(updatedQuestions);
  };

  const validateQuiz = () => {
    if (!quiz.quizName.trim()) {
      setError("De titel van de quiz is verplicht.");
      return false;
    }

    for (const question of questions) {
      if (!question.questionText.trim()) {
        setError("Elke vraag moet een vraagtekst hebben.");
        return false;
      }
      if (!question.correctAnswer.trim()) {
        setError("Elke vraag moet een correct antwoord hebben.");
        return false;
      }
    }

    return true;
  };

  const handleSubmit = async () => {
    setMessage("");
    setError("");

    if (!validateQuiz()) return;

    try {
      setLoading(true);
      if (quizId) {
        // Bijwerken van bestaande quiz
        await axiosInstance.put(`/api/Quiz/${quizId}`, {
          ...quiz,
          quizQuestions: questions,
        });
        setMessage("Quiz succesvol bijgewerkt!");
      } else {
        // Nieuwe quiz aanmaken
        const response = await axiosInstance.post("/api/Quiz", {
          ...quiz,
          quizQuestions: questions,
        });
        setMessage("Quiz succesvol aangemaakt!");
        navigate(`/quizzes/${response.data.quizId}`);
      }
    } catch (err) {
      console.error("Fout bij opslaan quiz:", err);
      setError("Er is een fout opgetreden bij het opslaan van de quiz.");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
        <CircularProgress />
        <Typography variant="h6" sx={{ marginTop: 2 }}>
          Quiz wordt geladen...
        </Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      {/* Alerts bovenaan */}
      {message && (
        <Alert severity="success" sx={{ marginBottom: 2 }}>
          {message}
        </Alert>
      )}
      {error && (
        <Alert severity="error" sx={{ marginBottom: 2 }}>
          {error}
        </Alert>
      )}

      <Typography variant="h4" gutterBottom>
        {quizId ? "Quiz Bewerken" : "Nieuwe Quiz Aanmaken"}
      </Typography>
      <Box
        component="form"
        onSubmit={(e) => {
          e.preventDefault();
          handleSubmit();
        }}
        sx={{ display: "flex", flexDirection: "column", gap: 3 }}
      >
        <TextField
          label="Titel"
          name="quizName"
          variant="outlined"
          required
          value={quiz.quizName}
          onChange={handleQuizChange}
        />
        <TextField
          label="Beschrijving"
          name="quizDescription"
          variant="outlined"
          multiline
          rows={3}
          value={quiz.quizDescription}
          onChange={handleQuizChange}
        />
        <Typography variant="h5">Vragen</Typography>
        {questions.map((question, index) => (
          <Card key={index} sx={{ marginBottom: 2 }}>
            <CardContent>
              <TextField
                label={`Vraag ${index + 1}`}
                fullWidth
                margin="normal"
                value={question.questionText}
                onChange={(e) => handleQuestionChange(index, "questionText", e.target.value)}
              />
              <TextField
                label="Correct Antwoord"
                fullWidth
                margin="normal"
                value={question.correctAnswer}
                onChange={(e) => handleQuestionChange(index, "correctAnswer", e.target.value)}
              />
              <TextField
                label="Media URL (optioneel)"
                fullWidth
                margin="normal"
                value={question.mediaUrl}
                onChange={(e) => handleQuestionChange(index, "mediaUrl", e.target.value)}
              />
              <IconButton onClick={() => removeQuestion(index)} color="error">
                <DeleteIcon />
              </IconButton>
            </CardContent>
          </Card>
        ))}
        <Button variant="contained" onClick={addQuestion}>
          Vraag Toevoegen
        </Button>
        <Button type="submit" variant="contained" color="primary">
          {quizId ? "Bijwerken" : "Toevoegen"}
        </Button>
      </Box>
    </Container>
  );
};

export default QuizEditorPage;
