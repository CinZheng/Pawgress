import React, { useState } from "react";
import { Container, Typography, TextField, Button, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../axios";
import Layout from "../components/Layout";
import { useNotification } from "../context/NotificationContext";

const RegisterPage = () => {
  const [formData, setFormData] = useState({});
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post("/api/Auth/register", formData);
      showNotification("Registratie succesvol! Je kunt nu inloggen.", "success");
      setTimeout(() => navigate("/login"), 2000);
    } catch (error) {
      showNotification("Registratie mislukt. Probeer het opnieuw.", "error");
    }
  };

  return (
    <Layout>
      <Container maxWidth="sm">
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Registreren
          </Typography>
        </Box>
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
    </Layout>
  );
};

export default RegisterPage;
