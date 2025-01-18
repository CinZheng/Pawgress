import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import axiosInstance from "../axios";
import { marked } from "marked";
import Layout from "../components/Layout";
import DeleteIcon from "@mui/icons-material/Delete";
import { isAdmin } from "../utils/auth";
import { useNotification } from "../context/NotificationContext";

const LessonEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    markdownContent: "",
    image: "",
    video: "",
    tag: "",
  });
  const [loading, setLoading] = useState(false);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const lessonId = searchParams.get("id");
  const [isUserAdmin] = useState(isAdmin());
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const { showNotification } = useNotification();

  useEffect(() => {
    const fetchLesson = async () => {
      if (!lessonId) return;

      setLoading(true);
      try {
        const response = await axiosInstance.get(`/api/Lesson/${lessonId}`);
        setFormData({
          name: response.data.name || "",
          markdownContent: response.data.markdownContent || "",
          image: response.data.image || "",
          video: response.data.video || "",
          tag: response.data.tag || "",
        });
      } catch (error) {
        console.error("Fout bij ophalen van les:", error);
        showNotification("Kon lesgegevens niet ophalen", "error");
      } finally {
        setLoading(false);
      }
    };

    fetchLesson();
  }, [lessonId, showNotification]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleMarkdownChange = (value) => {
    setFormData({ ...formData, markdownContent: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.name.trim()) {
      showNotification("Naam is verplicht", "error");
      return;
    }

    try {
      if (lessonId) {
        await axiosInstance.put(`/api/Lesson/${lessonId}`, formData);
        showNotification("Les succesvol bijgewerkt!", "success");
      } else {
        await axiosInstance.post("/api/Lesson", formData);
        showNotification("Les succesvol aangemaakt!", "success");
        navigate("/lessons");
      }
    } catch (error) {
      console.error("Fout bij opslaan van les:", error);
      showNotification("Er is een fout opgetreden bij het opslaan van de les", "error");
    }
  };

  const handleDeleteLesson = async () => {
    try {
      await axiosInstance.delete(`/api/Lesson/${lessonId}`);
      showNotification("Les succesvol verwijderd!", "success");
      navigate("/lessons");
    } catch (err) {
      console.error("Error deleting lesson:", err);
      const errorMessage = err.response?.data?.error || "Er is een fout opgetreden bij het verwijderen van de les";
      showNotification(errorMessage, "error");
    }
    setDeleteDialogOpen(false);
  };

  if (loading) {
    return (
      <Layout>
        <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
          <CircularProgress />
          <Typography variant="h6" sx={{ marginTop: 2 }}>
            Les wordt geladen...
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
            {lessonId ? "Les Bewerken" : "Nieuwe Les"}
          </Typography>
          {isUserAdmin && lessonId && (
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
          onSubmit={handleSubmit}
          sx={{ display: "flex", flexDirection: "column", gap: 3 }}
        >
          <TextField
            label="Titel"
            name="name"
            variant="outlined"
            required
            value={formData.name}
            onChange={handleInputChange}
          />
          <Typography variant="h6">Markdown Content</Typography>
          <ReactQuill
            theme="snow"
            value={formData.markdownContent}
            onChange={handleMarkdownChange}
          />
          <TextField
            label="Afbeelding URL"
            name="image"
            variant="outlined"
            value={formData.image}
            onChange={handleInputChange}
          />
          <TextField
            label="Video URL"
            name="video"
            variant="outlined"
            value={formData.video}
            onChange={handleInputChange}
          />
          <TextField
            label="Tags"
            name="tag"
            variant="outlined"
            value={formData.tag}
            onChange={handleInputChange}
          />
          <Typography variant="h5" gutterBottom>
            Preview
          </Typography>
          <Box
            sx={{
              border: "1px solid #ccc",
              borderRadius: "4px",
              padding: "16px",
              backgroundColor: "#f9f9f9",
            }}
          >
            <Typography variant="h6">{formData.name}</Typography>
            <div
              dangerouslySetInnerHTML={{
                __html: marked(formData.markdownContent || ""),
              }}
            ></div>
            {formData.image && (
              <img
                src={formData.image}
                alt="Preview"
                style={{ maxWidth: "100%", marginTop: "8px" }}
              />
            )}
            {formData.video && (
              <iframe
                src={formData.video}
                title="Video Preview"
                style={{ width: "100%", height: "300px", marginTop: "8px" }}
              ></iframe>
            )}
            <Typography variant="body2" style={{ marginTop: "8px" }}>
              {formData.tag}
            </Typography>
          </Box>
          <Button type="submit" variant="contained" color="primary">
            {lessonId ? "Les Bijwerken" : "Les Aanmaken"}
          </Button>
        </Box>

        {/* Delete Confirmation Dialog */}
        <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
          <DialogTitle>Les Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>
              Weet je zeker dat je deze les wilt verwijderen? Dit kan niet ongedaan worden gemaakt.
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
              Annuleren
            </Button>
            <Button onClick={handleDeleteLesson} color="error">
              Verwijderen
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    </Layout>
  );
};

export default LessonEditorPage;
