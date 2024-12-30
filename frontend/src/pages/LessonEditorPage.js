import React, { useState } from "react";
import { Container, TextField, Button, Typography, Box, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import ReactQuill from "react-quill"; // rich text
import "react-quill/dist/quill.snow.css";
import axiosInstance from "../axios";

const LessonEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    text: "",
    image: "",
    video: "",
    tag: "",
    markdownContent: "",
  });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

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
      const response = await axiosInstance.post("/api/Lesson", formData);
      setMessage("De les is succesvol toegevoegd!");
      // Reset formulier
      setFormData({
        name: "",
        text: "",
        image: "",
        video: "",
        tag: "",
        markdownContent: "",
      });
    } catch (err) {
      setError("Er is iets fout gegaan bij het toevoegen van de les.");
      console.error(err);
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Les Toevoegen
      </Typography>
      <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
        {/* Titel */}
        <TextField
          label="Titel"
          name="name"
          variant="outlined"
          required
          value={formData.name}
          onChange={handleInputChange}
        />

        {/* Tekst */}
        <TextField
          label="Tekst"
          name="text"
          variant="outlined"
          multiline
          rows={4}
          value={formData.text}
          onChange={handleInputChange}
        />

        {/* Afbeelding */}
        <TextField
          label="Afbeelding URL"
          name="image"
          variant="outlined"
          value={formData.image}
          onChange={handleInputChange}
        />

        {/* Video */}
        <TextField
          label="Video URL"
          name="video"
          variant="outlined"
          value={formData.video}
          onChange={handleInputChange}
        />

        {/* Tags */}
        <TextField
          label="Tags"
          name="tag"
          variant="outlined"
          value={formData.tag}
          onChange={handleInputChange}
        />

        {/* Markdown Content */}
        <Typography variant="h6">Markdown Content</Typography>
        <ReactQuill theme="snow" value={formData.markdownContent} onChange={handleMarkdownChange} />

        {/* Preview */}
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
          <Typography variant="body1">{formData.text}</Typography>
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

        {/* Submit */}
        <Button type="submit" variant="contained" color="primary">
          Voeg Les Toe
        </Button>

        {/* Feedback */}
        {message && <Alert severity="success">{message}</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
      </Box>
    </Container>
  );
};

export default LessonEditorPage;
