import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { Container, TextField, Button, Typography, Box, Alert } from "@mui/material";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import axiosInstance from "../axios";
import { marked } from "marked";

const LessonEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    markdownContent: "",
    image: "",
    video: "",
    tag: "",
  });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const lessonId = searchParams.get("id");

  // fetch lesson details if editing an existing lesson
  useEffect(() => {
    const fetchLesson = async () => {
      if (lessonId) {
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
          console.error("Error fetching lesson:", error);
        }
      }
    };

    fetchLesson();
  }, [lessonId]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleMarkdownChange = (value) => {
    setFormData({ ...formData, markdownContent: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!formData.name.trim()) {
      setError("De titel is verplicht.");
      return;
    }

    try {
      if (lessonId) {
        // update existing lesson
        await axiosInstance.put(`/api/Lesson/${lessonId}`, formData);
        setMessage("De les is succesvol bijgewerkt!");
      } else {
        // create new lesson
        await axiosInstance.post("/api/Lesson", formData);
        setMessage("De les is succesvol toegevoegd!");
      }
      navigate("/library");
    } catch (err) {
      setError("Er is iets fout gegaan bij het opslaan van de les.");
      console.error(err);
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        {lessonId ? "Les Bewerken" : "Nieuwe Les Toevoegen"}
      </Typography>
      <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
        <TextField
          label="Titel"
          name="name"
          variant="outlined"
          required
          value={formData.name}
          onChange={handleInputChange}
        />
        <Typography variant="h6">Markdown Content</Typography>
        <ReactQuill theme="snow" value={formData.markdownContent} onChange={handleMarkdownChange} />
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
          <div dangerouslySetInnerHTML={{ __html: marked(formData.markdownContent || "") }}></div>
          {formData.image && <img src={formData.image} alt="Preview" style={{ maxWidth: "100%", marginTop: "8px" }} />}
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
          {lessonId ? "Bijwerken" : "Toevoegen"}
        </Button>
        {message && <Alert severity="success">{message}</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
      </Box>
    </Container>
  );
};

export default LessonEditorPage;
