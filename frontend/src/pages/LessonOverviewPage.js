import React, { useEffect, useState } from "react";
import { Grid, Card, CardContent, Typography, Button, Container } from "@mui/material";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../axios";

const LessonOverviewPage = () => {
  const [lessons, setLessons] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchLessons = async () => {
      try {
        const response = await axiosInstance.get("/api/Lesson"); // API call voor lessen
        setLessons(response.data);
      } catch (error) {
        console.error("Fout bij ophalen lessen:", error);
      }
    };

    fetchLessons();
  }, []);

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Overzicht van Lessen
      </Typography>
      { /* <Button
        variant="contained"
        color="primary"
        style={{ marginBottom: "20px" }}
        onClick={() => navigate("/lesson-editor")}
      >
        Nieuwe Les Aanmaken
      </Button> */ }
      <Grid container spacing={2}>
        {lessons.map((lesson) => (
          <Grid item xs={12} sm={6} md={4} key={lesson.lessonId}>
            <Card
              onClick={() => navigate(`/lessons/${lesson.lessonId}`)}
              style={{ cursor: "pointer" }}
            >
              <CardContent>
                <Typography variant="h6">{lesson.name}</Typography>
                <Typography variant="body2">{lesson.text || "Geen beschrijving beschikbaar."}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default LessonOverviewPage;
