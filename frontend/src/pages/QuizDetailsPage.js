import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Button, Box, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const QuizDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [isUserAdmin, setIsUserAdmin] = useState(false);

  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await axiosInstance.get(`/api/Quiz/${id}`);
        setQuiz(response.data);
      } catch (error) {
        console.error("Error fetching quiz:", error);
      }
    };

    fetchQuiz();

    setIsUserAdmin(isAdmin());
  }, [id]);

  const handleDelete = async () => {
    try {
      await axiosInstance.delete(`/api/Quiz/${id}`);
      setDeleteDialogOpen(false);
      navigate("/library");
    } catch (error) {
      console.error("Error deleting quiz:", error);
    }
  };

  if (!quiz) {
    return <Typography>Quiz wordt geladen...</Typography>;
  }

  return (
    <Layout>
      <Container maxWidth="md">
        <Typography variant="h4" gutterBottom>
          {quiz.quizName}
        </Typography>
        <Box
          sx={{
            border: "1px solid #ccc",
            borderRadius: "4px",
            padding: "16px",
            backgroundColor: "#f9f9f9",
          }}
        >
          <Typography variant="body1" sx={{ marginBottom: 2 }}>
            {quiz.quizDescription || "Geen beschrijving"}
          </Typography>
        </Box>

        {isUserAdmin && (
          <Box sx={{ display: "flex", gap: 2, marginTop: 2 }}>
            <Button variant="contained" color="primary" onClick={() => navigate(`/quiz-editor?id=${id}`)}>
              Bewerken
            </Button>
            <Button variant="contained" color="error" onClick={() => setDeleteDialogOpen(true)}>
              Verwijderen
            </Button>
          </Box>
        )}

        <Button
          variant="contained"
          color="secondary"
          sx={{ marginTop: 2 }}
          onClick={() => navigate(`/quiz/${id}`)}
        >
          Start Quiz
        </Button>

        {/* Confirmation Dialog for Delete */}
        <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
          <DialogTitle>Quiz Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>Weet je zeker dat je deze quiz wilt verwijderen?</Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
              Annuleren
            </Button>
            <Button onClick={handleDelete} color="error">
              Verwijderen
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    </Layout>
  );
};

export default QuizDetailsPage;
