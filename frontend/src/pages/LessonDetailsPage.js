import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Button, Box, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { marked } from "marked";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const LessonDetailsPage = () => {
  const { id } = useParams(); // lesson id van route
  const navigate = useNavigate();
  const [lesson, setLesson] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false); // delete confirmation
  const [isUserAdmin, setIsUserAdmin] = useState(false); // check of gebruiker admin is

  useEffect(() => {
    const fetchLesson = async () => {
      try {
        const response = await axiosInstance.get(`/api/Lesson/${id}`);
        setLesson(response.data);
      } catch (error) {
        console.error("Error fetching lesson:", error);
      }
    };

    fetchLesson();

    setIsUserAdmin(isAdmin());
  }, [id]);

  const handleDelete = async () => {
    try {
      await axiosInstance.delete(`/api/Lesson/${id}`);
      setDeleteDialogOpen(false);
      navigate("/lessons");
    } catch (error) {
      console.error("Error deleting lesson:", error);
    }
  };

  if (!lesson) {
    return <Typography>Les wordt geladen...</Typography>;
  }

  return (
    <Layout>
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        {lesson.name}
      </Typography>
      <Box
        sx={{
          border: "1px solid #ccc",
          borderRadius: "4px",
          padding: "16px",
          backgroundColor: "#f9f9f9",
        }}
      >
        <div dangerouslySetInnerHTML={{ __html: marked(lesson.markdownContent || "") }}></div>
        {lesson.image && <img src={lesson.image} alt="Lesson" style={{ maxWidth: "100%", marginTop: "16px" }} />}
        {lesson.video && (
          <iframe
            src={lesson.video}
            title="Lesson Video"
            style={{ width: "100%", height: "300px", marginTop: "16px" }}
          ></iframe>
        )}
        <Typography variant="body2" sx={{ marginTop: "8px" }}>
          Tags: {lesson.tag || "Geen tags"}
        </Typography>
      </Box>

      {isUserAdmin && ( // Knoppen alleen zichtbaar als gebruiker admin is
        <Box sx={{ display: "flex", gap: 2, marginTop: 2 }}>
          <Button variant="contained" color="primary" onClick={() => navigate(`/lesson-editor?id=${id}`)}>
            Bewerken
          </Button>
          <Button variant="contained" color="error" onClick={() => setDeleteDialogOpen(true)}>
            Verwijderen
          </Button>
        </Box>
      )}

      {/* Confirmation Dialog for Delete */}
      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Les Verwijderen</DialogTitle>
        <DialogContent>
          <Typography>Weet je zeker dat je deze les wilt verwijderen?</Typography>
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

export default LessonDetailsPage;
