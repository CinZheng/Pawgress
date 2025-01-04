import React, { useState, useEffect } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Alert,
  CircularProgress,
} from "@mui/material";
import { useSearchParams, useNavigate } from "react-router-dom";
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
  const [loading, setLoading] = useState(false);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const dogId = searchParams.get("id");

  // Fetch details if editing an existing dog
  useEffect(() => {
    const fetchDog = async () => {
      if (!dogId) return;

      setLoading(true);
      try {
        const response = await axiosInstance.get(`/api/DogProfile/${dogId}`);
        setFormData({
          name: response.data.name || "",
          breed: response.data.breed || "",
          dateOfBirth: response.data.dateOfBirth || "",
          image: response.data.image || "",
        });
      } catch (err) {
        console.error("Fout bij ophalen hondenprofiel:", err);
        setError("Kon hondenprofiel niet ophalen.");
      } finally {
        setLoading(false);
      }
    };

    fetchDog();
  }, [dogId]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const validateDogProfile = () => {
    if (!formData.name.trim()) {
      setError("De naam is verplicht.");
      return false;
    }
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!validateDogProfile()) return;

    setLoading(true);
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
      console.error("Fout bij opslaan hondenprofiel:", err);
      setError("Er is een fout opgetreden bij het opslaan van het hondenprofiel.");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
        <CircularProgress />
        <Typography variant="h6" sx={{ marginTop: 2 }}>
          Hondenprofiel wordt geladen...
        </Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      {/* Alerts bovenaan */}
      {message && (
        <Alert severity="success" sx={{ marginBottom: 2 }}>
          {message}
        </Alert>
      )}
      {error && (
        <Alert severity="error" sx={{ marginBottom: 2 }}>
          {error}
        </Alert>
      )}

      <Typography variant="h4" gutterBottom>
        {dogId ? "Hondenprofiel Bewerken" : "Nieuw Hondenprofiel Toevoegen"}
      </Typography>
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{ display: "flex", flexDirection: "column", gap: 3 }}
      >
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
      </Box>
    </Container>
  );
};

export default DogProfileEditorPage;
