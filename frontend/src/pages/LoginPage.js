import React, { useState } from "react";
import { Container, Typography, TextField, Button, Box, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../axios";
import Layout from "../components/Layout";

const LoginPage = ({ onLogin }) => {
  const [formData, setFormData] = useState({});
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axiosInstance.post("/api/Auth/login", formData);
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("userId", response.data.userId);
      onLogin();
      navigate("/modules");
    } catch (error) {
      setError("Ongeldige inloggegevens");
    }
  };

  return (
    <Layout>
      <Container maxWidth="sm">
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Inloggen
          </Typography>
        </Box>
        <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
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
            Inloggen
          </Button>
        </Box>

        {error && <Alert severity="error" sx={{ marginTop: 2 }}>{error}</Alert>}
        
        <Typography sx={{ marginTop: 2, textAlign: "center" }}>
          Nog geen account?{" "}
          <Button
            variant="text"
            color="primary"
            onClick={() => navigate("/register")}
          >
            Registreer hier
          </Button>
        </Typography>
      </Container>
    </Layout>
  );
};

export default LoginPage;
