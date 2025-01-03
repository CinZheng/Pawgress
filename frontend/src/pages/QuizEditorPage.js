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
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import axiosInstance from "../axios";
import { useNavigate, useLocation } from "react-router-dom";

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

  // Ophalen van quizgegevens als er een quizId is
  useEffect(() => {
    const fetchQuiz = async () => {
      if (!quizId) return;

      try {
        const response = await axiosInstance.get(`/api/Quiz/${quizId}`);
        setQuiz({
          quizName: response.data.quizName,
          quizDescription: response.data.quizDescription,
        });

        // Ophalen van de vragen van de quiz
        const questionsResponse = await axiosInstance.get(`/api/Quiz/${quizId}/questions`);
        setQuestions(
          questionsResponse.data.map((q) => ({
            text: q.questionText,
            correctAnswer: q.correctAnswer,
            mediaUrl: q.mediaUrl || "",
          }))
        );
      } catch (error) {
        console.error("Fout bij ophalen quiz:", error);
      }
    };

    fetchQuiz();
  }, [quizId]);

  const handleQuizChange = (e) => {
    setQuiz({ ...quiz, [e.target.name]: e.target.value });
  };

  const handleQuestionChange = (index, field, value) => {
    const updatedQuestions = [...questions];
    updatedQuestions[index][field] = value;
    setQuestions(updatedQuestions);
  };

  const addQuestion = () => {
    setQuestions([...questions, { text: "", correctAnswer: "", mediaUrl: "" }]);
  };

  const removeQuestion = (index) => {
    const updatedQuestions = questions.filter((_, i) => i !== index);
    setQuestions(updatedQuestions);
  };

  const handleSubmit = async () => {
    try {
      if (quizId) {
        // Bijwerken van bestaande quiz
        await axiosInstance.put(`/api/Quiz/${quizId}`, quiz);

        // verwijder vragen
        await axiosInstance.delete(`/api/Quiz/${quizId}/questions`);
        for (const question of questions) {
          await axiosInstance.post(`/api/Quiz/${quizId}/add-question`, question);
        }
      } else {
        // Nieuwe quiz aanmaken
        const response = await axiosInstance.post("/api/Quiz", quiz);
        const newQuizId = response.data.quizId;

        for (const question of questions) {
          await axiosInstance.post(`/api/Quiz/${newQuizId}/add-question`, question);
        }
      }

      alert("Quiz succesvol opgeslagen!");
      navigate("/library");
    } catch (error) {
      console.error("Fout bij opslaan quiz:", error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        {quizId ? "Quiz Bewerken" : "Nieuwe Quiz Aanmaken"}
      </Typography>
      <Box>
        <TextField
          name="quizName"
          label="Quiznaam"
          fullWidth
          margin="normal"
          value={quiz.quizName}
          onChange={handleQuizChange}
        />
        <TextField
          name="quizDescription"
          label="Quizbeschrijving"
          fullWidth
          margin="normal"
          multiline
          rows={2}
          value={quiz.quizDescription}
          onChange={handleQuizChange}
        />
      </Box>
      <Typography variant="h5" gutterBottom>
        Vragen
      </Typography>
      {questions.map((question, index) => (
        <Card key={index} sx={{ marginBottom: 2 }}>
          <CardContent>
            <TextField
              label="Vraagtekst"
              fullWidth
              margin="normal"
              value={question.text}
              onChange={(e) => handleQuestionChange(index, "text", e.target.value)}
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
      <Button variant="contained" color="primary" onClick={handleSubmit}>
        Opslaan
      </Button>
    </Container>
  );
};

export default QuizEditorPage;
