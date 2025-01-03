import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Button, Box, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";

const DogProfileDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [dogProfile, setDogProfile] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [isUserAdmin, setIsUserAdmin] = useState(false);

  useEffect(() => {
    const fetchDogProfile = async () => {
      try {
        const response = await axiosInstance.get(`/api/DogProfile/${id}`);
        setDogProfile(response.data);
      } catch (error) {
        console.error("Fout bij ophalen hondenprofiel:", error);
      }
    };

    fetchDogProfile();
    setIsUserAdmin(isAdmin());
  }, [id]);

  const handleDelete = async () => {
    try {
      await axiosInstance.delete(`/api/DogProfile/${id}`);
      setDeleteDialogOpen(false);
      navigate("/dogprofiles");
    } catch (error) {
      console.error("Fout bij verwijderen hondenprofiel:", error);
    }
  };

  if (!dogProfile) {
    return <Typography>Hondenprofiel wordt geladen...</Typography>;
  }

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        {dogProfile.name}
      </Typography>
      <Box
        sx={{
          border: "1px solid #ccc",
          borderRadius: "4px",
          padding: "16px",
          backgroundColor: "#f9f9f9",
        }}
      >
        <Typography variant="body1">
          <strong>Ras:</strong> {dogProfile.breed || "Niet gespecificeerd"}
        </Typography>
        <Typography variant="body1">
          <strong>Geboortedatum:</strong> {new Date(dogProfile.dateOfBirth).toLocaleDateString()}
        </Typography>
        {dogProfile.image && (
          <img
            src={dogProfile.image}
            alt={dogProfile.name}
            style={{ maxWidth: "100%", marginTop: "16px" }}
          />
        )}
      </Box>
      {isUserAdmin && (
        <Box sx={{ display: "flex", gap: 2, marginTop: 2 }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate(`/dogprofile-editor?id=${id}`)}
          >
            Bewerken
          </Button>
          <Button
            variant="contained"
            color="error"
            onClick={() => setDeleteDialogOpen(true)}
          >
            Verwijderen
          </Button>
        </Box>
      )}
      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Hond Verwijderen</DialogTitle>
        <DialogContent>
          <Typography>Weet je zeker dat je dit hondenprofiel wilt verwijderen?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
            Annuleren
          </Button>
          <Button onClick={handleDelete} color="error">
            Verwijderen
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default DogProfileDetailsPage;
