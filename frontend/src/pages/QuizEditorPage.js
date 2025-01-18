import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  CircularProgress,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import axiosInstance from "../axios";
import Layout from "../components/Layout";
import { isAdmin } from "../utils/auth";
import { useNotification } from "../context/NotificationContext";

const QuizEditorPage = () => {
  const [quiz, setQuiz] = useState({
    quizName: "",
    quizDescription: "",
    quizQuestions: [],
  });
  const [loading, setLoading] = useState(false);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const quizId = searchParams.get("id");
  const [isUserAdmin] = useState(isAdmin());
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const { showNotification } = useNotification();

  useEffect(() => {
    const fetchQuiz = async () => {
      if (!quizId) return;

      setLoading(true);
      try {
        const response = await axiosInstance.get(`/api/Quiz/${quizId}`);
        setQuiz({
          quizName: response.data.quizName || "",
          quizDescription: response.data.quizDescription || "",
          quizQuestions: response.data.quizQuestions || [],
        });
      } catch (error) {
        console.error("Fout bij ophalen van quiz:", error);
        showNotification("Kon quizgegevens niet ophalen", "error");
      } finally {
        setLoading(false);
      }
    };

    fetchQuiz();
  }, [quizId, showNotification]);

  const handleQuizChange = (e) => {
    const { name, value } = e.target;
    setQuiz((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleQuestionChange = (index, field, value) => {
    const updatedQuestions = [...quiz.quizQuestions];
    updatedQuestions[index] = {
      ...updatedQuestions[index],
      [field]: value,
    };
    setQuiz((prev) => ({
      ...prev,
      quizQuestions: updatedQuestions,
    }));
  };

  const addQuestion = () => {
    setQuiz((prev) => ({
      ...prev,
      quizQuestions: [
        ...prev.quizQuestions,
        { questionText: "", correctAnswer: "", mediaUrl: "" },
      ],
    }));
  };

  const removeQuestion = (index) => {
    setQuiz((prev) => ({
      ...prev,
      quizQuestions: prev.quizQuestions.filter((_, i) => i !== index),
    }));
  };

  const handleSubmit = async () => {
    if (!quiz.quizName.trim()) {
      showNotification("Naam is verplicht", "error");
      return;
    }

    try {
      if (quizId) {
        await axiosInstance.put(`/api/Quiz/${quizId}`, quiz);
        showNotification("Quiz succesvol bijgewerkt!", "success");
      } else {
        await axiosInstance.post("/api/Quiz", quiz);
        showNotification("Quiz succesvol aangemaakt!", "success");
        navigate("/quizzes");
      }
    } catch (error) {
      console.error("Fout bij opslaan van quiz:", error);
      showNotification("Er is een fout opgetreden bij het opslaan van de quiz", "error");
    }
  };

  const handleDeleteQuiz = async () => {
    try {
      await axiosInstance.delete(`/api/Quiz/${quizId}`);
      showNotification("Quiz succesvol verwijderd!", "success");
      navigate("/quizzes");
    } catch (err) {
      console.error("Error deleting quiz:", err);
      showNotification("Er is een fout opgetreden bij het verwijderen van de quiz", "error");
    }
    setDeleteDialogOpen(false);
  };

  if (loading) {
    return (
      <Layout>
        <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
          <CircularProgress />
          <Typography variant="h6" sx={{ marginTop: 2 }}>
            Quiz wordt geladen...
          </Typography>
        </Container>
      </Layout>
    );
  }

  return (
    <Layout>
      <Container maxWidth="md">
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h4" component="h1">
            {quizId ? "Quiz Bewerken" : "Nieuwe Quiz"}
          </Typography>
          {isUserAdmin && quizId && (
            <Button
              variant="contained"
              color="error"
              startIcon={<DeleteIcon />}
              onClick={() => setDeleteDialogOpen(true)}
            >
              Verwijderen
            </Button>
          )}
        </Box>

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

          <Box sx={{ mt: 2 }}>
            <Typography variant="h6" gutterBottom>
              Vragen
            </Typography>
            {quiz.quizQuestions.map((question, index) => (
              <Box
                key={index}
                sx={{
                  border: "1px solid #ccc",
                  borderRadius: "4px",
                  p: 2,
                  mb: 2,
                  backgroundColor: "#f9f9f9",
                }}
              >
                <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
                  <Typography variant="subtitle1">Vraag {index + 1}</Typography>
                  <IconButton
                    color="error"
                    onClick={() => removeQuestion(index)}
                    size="small"
                  >
                    <DeleteIcon />
                  </IconButton>
                </Box>
                <TextField
                  label="Vraag"
                  fullWidth
                  value={question.questionText}
                  onChange={(e) =>
                    handleQuestionChange(index, "questionText", e.target.value)
                  }
                  sx={{ mb: 2 }}
                />
                <TextField
                  label="Antwoord"
                  fullWidth
                  value={question.correctAnswer}
                  onChange={(e) =>
                    handleQuestionChange(index, "correctAnswer", e.target.value)
                  }
                  sx={{ mb: 2 }}
                />
                <TextField
                  label="Media URL"
                  fullWidth
                  value={question.mediaUrl}
                  onChange={(e) =>
                    handleQuestionChange(index, "mediaUrl", e.target.value)
                  }
                />
              </Box>
            ))}
            <Button
              variant="outlined"
              onClick={addQuestion}
              sx={{ mt: 2 }}
            >
              Vraag Toevoegen
            </Button>
          </Box>

          <Button type="submit" variant="contained" color="primary">
            {quizId ? "Quiz Bijwerken" : "Quiz Aanmaken"}
          </Button>
        </Box>

        {/* Delete Confirmation Dialog */}
        <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
          <DialogTitle>Quiz Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>
              Weet je zeker dat je deze quiz wilt verwijderen? Dit kan niet ongedaan worden gemaakt.
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
              Annuleren
            </Button>
            <Button onClick={handleDeleteQuiz} color="error">
              Verwijderen
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    </Layout>
  );
};

export default QuizEditorPage;
