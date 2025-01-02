import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Typography, Button, Avatar, Container } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";

const ProfilePage = () => {
  const [user, setUser] = useState(null);
  const [isUserAdmin, setIsUserAdmin] = useState(false); // Check of gebruiker admin is
  const navigate = useNavigate();

  useEffect(() => {
    // Haal userId uit JWT-token
    const userId = localStorage.getItem("userId");
    if (!userId) {
      console.error("Geen userId gevonden.");
      return;
    }

    // Ophalen gebruiker
    const fetchUser = async () => {
      try {
        const response = await axiosInstance.get(`/api/User/${userId}`);
        setUser(response.data);
        setIsUserAdmin(isAdmin()); // Controleer of de gebruiker admin is
      } catch (error) {
        console.error("Fout bij ophalen gebruiker:", error);
      }
    };

    fetchUser();
  }, []);

  // Uitlog functie
  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    navigate("/login");
  };

  if (!user) return <Typography>Profiel wordt geladen...</Typography>;

  return (
    <Container style={{ textAlign: "center", padding: "20px" }}>
      <Avatar
        alt={user.username}
        src={user.userPicture}
        sx={{ width: 100, height: 100, margin: "auto" }}
      />
      <Typography variant="h4" gutterBottom>
        {user.username}
      </Typography>
      <Button
        variant="contained"
        color="primary"
        onClick={() => alert("Bewerk profiel functionaliteit komt eraan!")}
        style={{ marginRight: "10px" }}
      >
        Bewerk profiel
      </Button>
      <Button onClick={handleLogout} variant="outlined" color="secondary">
        Uitloggen
      </Button>
    </Container>
  );
};

export default ProfilePage;
