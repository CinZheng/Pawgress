import React, { useState } from "react";
import axiosInstance from "../axios";
import { Container, TextField, Button, Typography, Box, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom"; // Import de useNavigate-hook


const RegisterPage = () => {
  const [formData, setFormData] = useState({ username: "", email: "", password: "" });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate(); // Gebruik de hook om te navigeren
  

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError(""); // reset bij nieuwe poging

    try {
      const response = await axiosInstance.post("/api/Auth/register", formData);
      setMessage(response.data); // succes bericht van server
    } catch (error) {
      console.error("Registratiefout:", error);

      
      if (error.response && error.response.data) {
        setError(error.response.data); // toon serverfoutmelding
      } else {
        setError("Er is iets misgegaan. Probeer het opnieuw.");
      }
    }
  };

  return (
    <Container maxWidth="sm">
      <Typography variant="h4" gutterBottom>
        Registreren
      </Typography>
      <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
        <TextField
          name="username"
          label="Gebruikersnaam"
          variant="outlined"
          required
          onChange={handleInputChange}
        />
        <TextField
          name="email"
          label="E-mailadres"
          variant="outlined"
          type="email"
          required
          onChange={handleInputChange}
        />
        <TextField
          name="password"
          label="Wachtwoord"
          variant="outlined"
          type="password"
          required
          onChange={handleInputChange}
        />
        <Button type="submit" variant="contained" color="primary">
          Registreren
        </Button>
      </Box>

      {/* Toon success- of foutmelding */}
      {message && <Alert severity="success" sx={{ marginTop: 2 }}>{message}</Alert>}
      {error && <Alert severity="error" sx={{ marginTop: 2 }}>{error}</Alert>}
      {/* Navigatie naar login */}
            <Typography sx={{ marginTop: 2, textAlign: "center" }}>
              Al een account geregistreerd?{" "}
              <Button
                variant="text"
                color="primary"
                onClick={() => navigate("/login")}
              >
              Log hier in
              </Button>
            </Typography>
    </Container>
  );
};

export default RegisterPage;
