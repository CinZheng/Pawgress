import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, Alert } from "@mui/material";
import axiosInstance from "../axios";

const ModuleDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [module, setModule] = useState(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchModule = async () => {
      try {
        const response = await axiosInstance.get(`/api/TrainingPath/${id}`);
        setModule(response.data);
      } catch (err) {
        console.error("Fout bij ophalen module:", err);
        setError("Kon modulegegevens niet ophalen.");
      }
    };

    fetchModule();
  }, [id]);

  const handleNext = () => {
    if (currentIndex + 1 < module.lessons.length + module.quizzes.length) {
      setCurrentIndex(currentIndex + 1);
    } else {
      navigate(`/modules/${id}/result`);
    }
  };

  if (!module) {
    return (
      <Container maxWidth="md">
        <Typography variant="h6">Module wordt geladen...</Typography>
      </Container>
    );
  }

  const currentItem =
    currentIndex < module.lessons.length
      ? module.lessons[currentIndex]
      : module.quizzes[currentIndex - module.lessons.length];

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Module: {module.name}
      </Typography>
      <Typography variant="body1" gutterBottom>
        {module.description}
      </Typography>
      {error && <Alert severity="error">{error}</Alert>}
      <Box
        sx={{
          border: "1px solid #ccc",
          borderRadius: "4px",
          padding: "16px",
          marginTop: "16px",
        }}
      >
        {currentIndex < module.lessons.length ? (
          <Box>
            <Typography variant="h5">Les: {currentItem.name}</Typography>
            <Typography variant="body1">{currentItem.text}</Typography>
          </Box>
        ) : (
          <Box>
            <Typography variant="h5">Quiz: {currentItem.quizName}</Typography>
            <Button
              variant="contained"
              color="secondary"
              onClick={() => navigate(`/quiz/${currentItem.quizId}`)}
            >
              Start Quiz
            </Button>
          </Box>
        )}
      </Box>
      <Button
        variant="contained"
        color="primary"
        onClick={handleNext}
        sx={{ marginTop: "16px" }}
      >
        {currentIndex + 1 < module.lessons.length + module.quizzes.length
          ? "Volgende"
          : "Terug naar Overzicht"}
      </Button>
    </Container>
  );
};

export default ModuleDetailsPage;
