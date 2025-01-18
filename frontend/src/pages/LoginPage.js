import React, { useState } from "react";
import { Container, Typography, TextField, Button, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../axios";
import Layout from "../components/Layout";
import { useNotification } from "../context/NotificationContext";

const LoginPage = ({ onLogin }) => {
  const [formData, setFormData] = useState({});
  const navigate = useNavigate();
  const { showNotification } = useNotification();

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
      showNotification("Succesvol ingelogd!", "success");
      navigate("/modules");
    } catch (error) {
      showNotification("Ongeldige inloggegevens", "error");
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
