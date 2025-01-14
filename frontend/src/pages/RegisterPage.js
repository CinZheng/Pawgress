import React, { useState } from "react";
import axiosInstance from "../axios";
import { Container, TextField, Button, Typography, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

const RegisterPage = () => {
  const [formData, setFormData] = useState({ email: "", password: "" });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post("/api/Auth/register", formData);
      setMessage("Registratie succesvol! Je kunt nu inloggen.");
      setTimeout(() => navigate("/login"), 2000);
    } catch (error) {
      console.error("Registratiefout:", error);
      setMessage("Er is een fout opgetreden bij het registreren.");
    }
  };

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 2,
          pt: -8
        }}
      >
        <Typography 
          variant="h2" 
          component="h1" 
          sx={{ 
            mb: 4,
            color: '#663366',
            fontWeight: 'bold',
            textAlign: 'center',
            width: '100%'
          }}
        >
          Account aanmaken
        </Typography>

        <Box
          component="form"
          onSubmit={handleSubmit}
          sx={{ 
            display: "flex", 
            flexDirection: "column", 
            gap: 3,
            width: '100%',
            maxWidth: '400px'
          }}
        >
          <Typography 
            variant="body1" 
            sx={{ 
              color: '#663366',
              fontWeight: 'medium',
              mb: -1
            }}
          >
            Email
          </Typography>
          <TextField
            name="email"
            placeholder="Type hier..."
            variant="outlined"
            type="email"
            required
            onChange={handleInputChange}
            sx={{
              backgroundColor: 'white',
              '& .MuiOutlinedInput-root': {
                '& fieldset': {
                  borderColor: '#E0E0E0',
                  borderRadius: '12px',
                },
                '&:hover fieldset': {
                  borderColor: '#663366',
                },
                '&.Mui-focused fieldset': {
                  borderColor: '#663366',
                },
              },
              '& .MuiOutlinedInput-input': {
                padding: '14px',
              },
            }}
            InputLabelProps={{ shrink: false }}
          />

          <Typography 
            variant="body1" 
            sx={{ 
              color: '#663366',
              fontWeight: 'medium',
              mb: -1
            }}
          >
            Wachtwoord
          </Typography>
          <TextField
            name="password"
            placeholder="Type hier..."
            variant="outlined"
            type="password"
            required
            onChange={handleInputChange}
            sx={{
              backgroundColor: 'white',
              '& .MuiOutlinedInput-root': {
                '& fieldset': {
                  borderColor: '#E0E0E0',
                  borderRadius: '12px',
                },
                '&:hover fieldset': {
                  borderColor: '#663366',
                },
                '&.Mui-focused fieldset': {
                  borderColor: '#663366',
                },
              },
              '& .MuiOutlinedInput-input': {
                padding: '14px',
              },
            }}
            InputLabelProps={{ shrink: false }}
          />

          <Button 
            type="submit" 
            variant="contained" 
            sx={{
              mt: 2,
              py: 2,
              backgroundColor: 'white',
              color: '#663366',
              boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.1)',
              borderRadius: '12px',
              '&:hover': {
                backgroundColor: '#f5f5f5',
                color: '#663366'
              }
            }}
          >
            Account aanmaken
          </Button>
        </Box>

        {message && (
          <Typography 
            sx={{ 
              marginTop: 2,
              color: message.includes("succesvol") ? "green" : "error.main"
            }}
          >
            {message}
          </Typography>
        )}

        <Typography 
          sx={{ 
            marginTop: 2, 
            textAlign: "center",
            color: '#663366'
          }}
        >
          Heb je al een account?{" "}
          <Button
            variant="text"
            sx={{ 
              color: '#663366',
              '&:hover': {
                backgroundColor: 'transparent',
                textDecoration: 'underline'
              }
            }}
            onClick={() => navigate("/login")}
          >
            Log hier in
          </Button>
        </Typography>
      </Box>
    </Container>
  );
};

export default RegisterPage;
