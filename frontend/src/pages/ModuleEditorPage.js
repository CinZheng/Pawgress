import React, { useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Alert,
} from "@mui/material";
import axiosInstance from "../axios";

const ModuleEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
  });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async () => {
    setMessage("");
    setError("");

    if (!formData.name.trim()) {
      setError("De naam is verplicht.");
      return;
    }

    try {
      await axiosInstance.post("/api/TrainingPath", formData);
      setMessage("Module succesvol aangemaakt!");
    } catch (err) {
      console.error("Fout bij opslaan module:", err);
      setError("Er is een fout opgetreden bij het opslaan van de module.");
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Nieuwe Module Aanmaken
      </Typography>
      {message && <Alert severity="success">{message}</Alert>}
      {error && <Alert severity="error">{error}</Alert>}
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          gap: "16px",
          marginTop: "16px",
        }}
      >
        <TextField
          label="Naam"
          name="name"
          variant="outlined"
          value={formData.name}
          onChange={handleInputChange}
        />
        <TextField
          label="Beschrijving"
          name="description"
          variant="outlined"
          multiline
          rows={4}
          value={formData.description}
          onChange={handleInputChange}
        />
        <Button variant="contained" color="primary" onClick={handleSubmit}>
          Opslaan
        </Button>
      </Box>
    </Container>
  );
};

export default ModuleEditorPage;
