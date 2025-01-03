import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { Container, TextField, Button, Typography, Box, Alert } from "@mui/material";
import axiosInstance from "../axios";

const DogProfileEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    breed: "",
    dateOfBirth: "",
    image: "",
  });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const dogId = searchParams.get("id");

  // Fetch details if editing an existing dog
  useEffect(() => {
    const fetchDog = async () => {
      if (dogId) {
        try {
          const response = await axiosInstance.get(`/api/DogProfile/${dogId}`);
          setFormData({
            name: response.data.name || "",
            breed: response.data.breed || "",
            dateOfBirth: response.data.dateOfBirth || "",
            image: response.data.image || "",
          });
        } catch (error) {
          console.error("Error fetching dog profile:", error);
        }
      }
    };

    fetchDog();
  }, [dogId]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!formData.name.trim()) {
      setError("De naam is verplicht.");
      return;
    }

    try {
      if (dogId) {
        // Update existing dog profile
        await axiosInstance.put(`/api/DogProfile/${dogId}`, formData);
        setMessage("Het hondenprofiel is succesvol bijgewerkt!");
      } else {
        // Create new dog profile
        await axiosInstance.post("/api/DogProfile", formData);
        setMessage("Het hondenprofiel is succesvol toegevoegd!");
      }
      navigate("/dogprofiles");
    } catch (err) {
      setError("Er is iets fout gegaan bij het opslaan van het hondenprofiel.");
      console.error(err);
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        {dogId ? "Hondenprofiel Bewerken" : "Nieuw Hondenprofiel Toevoegen"}
      </Typography>
      <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
        <TextField
          label="Naam"
          name="name"
          variant="outlined"
          required
          value={formData.name}
          onChange={handleInputChange}
        />
        <TextField
          label="Ras"
          name="breed"
          variant="outlined"
          value={formData.breed}
          onChange={handleInputChange}
        />
        <TextField
          label="Geboortedatum"
          name="dateOfBirth"
          type="date"
          InputLabelProps={{ shrink: true }}
          variant="outlined"
          value={formData.dateOfBirth}
          onChange={handleInputChange}
        />
        <TextField
          label="Afbeelding URL"
          name="image"
          variant="outlined"
          value={formData.image}
          onChange={handleInputChange}
        />
        <Button type="submit" variant="contained" color="primary">
          {dogId ? "Bijwerken" : "Toevoegen"}
        </Button>
        {message && <Alert severity="success">{message}</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
      </Box>
    </Container>
  );
};

export default DogProfileEditorPage;
