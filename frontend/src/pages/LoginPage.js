import React, { useState } from "react";
import axiosInstance from "../axios";
import { Container, TextField, Button, Typography, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
  const [formData, setFormData] = useState({ email: "", password: "" });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axiosInstance.post("/api/Auth/login", formData);
      const { token, userId } = response.data;
      localStorage.setItem("token", token);
      localStorage.setItem("userId", userId);
      setMessage("Inloggen succesvol!");

       // Redirect naar profielpagina
      navigate("/profile");
    } catch (error) {
      console.error("Inlogfout:", error);
      setMessage("Ongeldige inloggegevens.");
    }
  };

  return (
    <Container maxWidth="sm">
      <Typography variant="h4" gutterBottom>
        Inloggen
      </Typography>
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
      {message && <Typography sx={{ marginTop: 2 }}>{message}</Typography>}
      {/* Navigatie naar register */}
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
  );
};

export default LoginPage;
