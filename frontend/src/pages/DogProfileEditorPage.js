import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import axiosInstance from "../axios";
import Layout from "../components/Layout";
import DeleteIcon from "@mui/icons-material/Delete";
import { isAdmin } from "../utils/auth";

const DogProfileEditorPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    breed: "",
    dateOfBirth: "",
    image: "",
  });
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const dogId = searchParams.get("id");
  const [isUserAdmin] = useState(isAdmin());
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);

  useEffect(() => {
    const fetchDogProfile = async () => {
      if (!dogId) return;

      try {
        const response = await axiosInstance.get(`/api/DogProfile/${dogId}`);
        setFormData({
          name: response.data.name || "",
          breed: response.data.breed || "",
          dateOfBirth: response.data.dateOfBirth?.split("T")[0] || "",
          image: response.data.image || "",
        });
      } catch (error) {
        console.error("Fout bij ophalen van hondenprofiel:", error);
        setError("Kon hondenprofiel niet ophalen.");
      }
    };

    fetchDogProfile();
  }, [dogId]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!formData.name.trim()) {
      setError("Naam is verplicht.");
      return;
    }

    try {
      if (dogId) {
        await axiosInstance.put(`/api/DogProfile/${dogId}`, formData);
        setMessage("Hondenprofiel succesvol bijgewerkt!");
      } else {
        await axiosInstance.post("/api/DogProfile", formData);
        setMessage("Hondenprofiel succesvol aangemaakt!");
        navigate("/dogprofiles");
      }
    } catch (error) {
      console.error("Fout bij opslaan van hondenprofiel:", error);
      setError("Er is een fout opgetreden bij het opslaan van het hondenprofiel.");
    }
  };

  const handleDeleteDogProfile = async () => {
    try {
      await axiosInstance.delete(`/api/DogProfile/${dogId}`);
      setMessage("Hondenprofiel succesvol verwijderd!");
      navigate("/dogprofiles");
    } catch (err) {
      console.error("Error deleting dog profile:", err);
      setError(err.response?.data?.error || "Er is een fout opgetreden bij het verwijderen van het hondenprofiel.");
    }
    setDeleteDialogOpen(false);
  };

  return (
    <Layout>
      <Container maxWidth="md">
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h4" component="h1">
            {dogId ? "Hondenprofiel Bewerken" : "Nieuw Hondenprofiel"}
          </Typography>
          {isUserAdmin && dogId && (
            <Button
              variant="contained"
              color="error"
              startIcon={<DeleteIcon />}
              onClick={() => setDeleteDialogOpen(true)}
            >
              Verwijderen
            </Button>
          )}
        </Box>

        {message && <Alert severity="success" sx={{ mb: 2 }}>{message}</Alert>}
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

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

          {formData.image && (
            <Box sx={{ mt: 2 }}>
              <Typography variant="h6" gutterBottom>
                Preview
              </Typography>
              <Box
                sx={{
                  border: "1px solid #ccc",
                  borderRadius: "4px",
                  p: 2,
                  backgroundColor: "#f9f9f9",
                }}
              >
                <img
                  src={formData.image}
                  alt="Preview"
                  style={{ maxWidth: "100%", marginTop: "8px" }}
                />
              </Box>
            </Box>
          )}

          <Button type="submit" variant="contained" color="primary">
            {dogId ? "Hondenprofiel Bijwerken" : "Hondenprofiel Aanmaken"}
          </Button>
        </Box>

        {/* Delete Confirmation Dialog */}
        <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
          <DialogTitle>Hondenprofiel Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>
              Weet je zeker dat je dit hondenprofiel wilt verwijderen? Dit kan niet ongedaan worden gemaakt.
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
              Annuleren
            </Button>
            <Button onClick={handleDeleteDogProfile} color="error">
              Verwijderen
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    </Layout>
  );
};

export default DogProfileEditorPage;
