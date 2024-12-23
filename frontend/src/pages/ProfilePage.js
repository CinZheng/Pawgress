import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Typography, Button, Avatar, Container } from "@mui/material";

const ProfilePage = () => {
  const [user, setUser] = useState(null);

  useEffect(() => {
    // haal userId uit JWT-token
    const userId = localStorage.getItem("userId"); // Alternatief: JWT decoderen
    if (!userId) {
      console.error("Geen userId gevonden.");
      return;
    }

    //ophalen gebruiker
    const fetchUser = async () => {
      try {
        const response = await axiosInstance.get(`/api/User/${userId}`);
        setUser(response.data);
      } catch (error) {
        console.error("Fout bij ophalen gebruiker:", error);
      }
    };

    fetchUser();
  }, []);

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
      >
        Bewerk profiel
      </Button>
    </Container>
  );
};

export default ProfilePage;
