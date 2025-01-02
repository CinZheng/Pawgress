import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";

const LibraryPage = () => {
  const [lessons, setLessons] = useState([]);
  const [quizzes, setQuizzes] = useState([]);
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const navigate = useNavigate();

  // Ophalen van lessen, quizzes en controleren of gebruiker admin is
  useEffect(() => {
    const fetchData = async () => {
      try {
        const lessonsResponse = await axiosInstance.get("/api/Lesson");
        const quizzesResponse = await axiosInstance.get("/api/Quiz");
        setLessons(lessonsResponse.data);
        setQuizzes(quizzesResponse.data);
      } catch (error) {
        console.error("Fout bij ophalen van data:", error);
      }
    };

    fetchData();
    setIsUserAdmin(isAdmin()); 
  }, []);

  return (
    <Grid container spacing={2} padding={2}>
      {/* Beheer knoppen voor admin */}
      {isUserAdmin && (
        <Grid item xs={12} style={{ marginBottom: "20px" }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate("/quiz-editor")}
            style={{ marginRight: "10px" }}
          >
            Nieuwe Quiz Aanmaken
          </Button>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate("/lesson-editor")}
          >
            Nieuwe Les Aanmaken
          </Button>
        </Grid>
      )}

      {/* Lessen */}
      <Grid item xs={12}>
        <Typography variant="h4" gutterBottom>
          Lessen
        </Typography>
      </Grid>
      {lessons.map((lesson) => (
        <Grid item xs={12} sm={6} md={4} key={lesson.lessonId}>
          <Card
            onClick={() => navigate(`/lessons/${lesson.lessonId}`)}
            style={{ cursor: "pointer" }}
          >
            <CardContent>
              <Typography variant="h6">{lesson.name}</Typography>
              <Typography variant="body2">
                {lesson.text || "Geen beschrijving beschikbaar."}
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      ))}

      {/* Quizzes */}
      <Grid item xs={12} style={{ marginTop: "20px" }}>
        <Typography variant="h4" gutterBottom>
          Quizzes
        </Typography>
      </Grid>
      {quizzes.map((quiz) => (
        <Grid item xs={12} sm={6} md={4} key={quiz.quizId}>
          <Card
            onClick={() => navigate(`/quiz-editor/${quiz.quizId}`)}
            style={{ cursor: "pointer" }}
          >
            <CardContent>
              <Typography variant="h6">{quiz.quizName}</Typography>
              <Typography variant="body2">
                {quiz.quizDescription || "Geen beschrijving beschikbaar."}
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );
};

export default LibraryPage;
