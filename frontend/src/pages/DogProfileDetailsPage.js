import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  Container,
  Typography,
  Button,
  Box,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Card,
  CardContent,
  Grid,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";
import PetsIcon from "@mui/icons-material/Pets";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';

const DogProfileDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [dogProfile, setDogProfile] = useState(null);
  const [notes, setNotes] = useState([]);
  const [selectedNote, setSelectedNote] = useState(null);
  const [newNote, setNewNote] = useState({ description: "", tag: "" });
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [addNoteDialogOpen, setAddNoteDialogOpen] = useState(false);
  const [noteDeleteDialogOpen, setNoteDeleteDialogOpen] = useState(false);
  const [isFavorite, setIsFavorite] = useState(false);
  const userId = localStorage.getItem("userId");
  const [sensorData, setSensorData] = useState([]);
  const [sensorTypeFilter, setSensorTypeFilter] = useState("all");
  const [searchQuery, setSearchQuery] = useState("");
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const adminCheck = isAdmin();
        setIsUserAdmin(adminCheck);
        
        const profileResponse = await axiosInstance.get(`/api/DogProfile/${id}`);
        setDogProfile(profileResponse.data);
        
        const notesResponse = await axiosInstance.get(`/api/Note/dogprofile/${id}`);
        setNotes(notesResponse.data);
        
        const favoriteResponse = await axiosInstance.get(`/api/DogProfile/${id}/favorite/${userId}`);
        setIsFavorite(favoriteResponse.data.isFavorite);

        if (adminCheck) {
          const sensorResponse = await axiosInstance.get(`/api/DogSensorData/dog/${id}`);
          setSensorData(Array.isArray(sensorResponse.data) ? sensorResponse.data : []);
        }
      } catch (error) {
        setError(error.message);
      }
    };

    fetchData();
  }, [id, userId]);

  if (error) {
    return <Typography color="error">Error: {error}</Typography>;
  }

  if (!dogProfile) {
    return <Typography>Hondenprofiel wordt geladen...</Typography>;
  }

  const handleAddOrEditNote = async () => {
    try {
      const note = {
        description: selectedNote?.description || newNote.description,
        tag: selectedNote?.tag || newNote.tag,
        dogProfileId: id,
        userId: localStorage.getItem("userId"),
        date: new Date().toISOString(),
      };

      if (selectedNote?.noteId) {
        // Update bestaande notitie
        await axiosInstance.put(`/api/Note/${selectedNote.noteId}`, note);
      } else {
        // Nieuwe notitie maken
        await axiosInstance.post("/api/Note", note);
      }

      setAddNoteDialogOpen(false);
      const updatedNotes = await axiosInstance.get(`/api/Note/dogprofile/${id}`);
      setNotes(updatedNotes.data);
      setSelectedNote(null);
    } catch (error) {
      console.error("Fout bij toevoegen of bewerken van notitie:", error);
    }
  };

  const handleDeleteNote = async () => {
    try {
      if (selectedNote?.noteId) {
        await axiosInstance.delete(`/api/Note/${selectedNote.noteId}`);
        const updatedNotes = await axiosInstance.get(`/api/Note/dogprofile/${id}`);
        setNotes(updatedNotes.data);
      }
      setNoteDeleteDialogOpen(false);
      setSelectedNote(null);
    } catch (error) {
      console.error("Fout bij verwijderen notitie:", error);
    }
  };

  const openEditDialog = (note) => {
    setSelectedNote(note);
    setAddNoteDialogOpen(true);
  };

  const openDeleteDialog = (note) => {
    setSelectedNote(note);
    setNoteDeleteDialogOpen(true);
  };

  const handleDeleteDogProfile = async () => {
    try {
      await axiosInstance.delete(`/api/DogProfile/${id}`);
      setDeleteDialogOpen(false);
      navigate("/dogprofiles");
    } catch (error) {
      console.error("Fout bij verwijderen hondenprofiel:", error);
    }
  };

  const handleToggleFavorite = async () => {
    try {
      const response = await axiosInstance.post(`/api/DogProfile/${id}/favorite/${userId}`);
      setIsFavorite(response.data.isFavorite);
    } catch (error) {
      console.error("Error toggling favorite:", error);
    }
  };

  const handleDeleteSensorData = async (sensorDataId) => {
    if (window.confirm('Weet je zeker dat je deze sensordata wilt verwijderen?')) {
      try {
        await axiosInstance.delete(`/api/DogSensorData/${sensorDataId}`);
        setSensorData(sensorData.filter(data => data.dogSensorDataId !== sensorDataId));
      } catch (err) {
        console.error('Error deleting sensor data:', err);
      }
    }
  };

  const filteredSensorData = sensorData
    .filter(data => sensorTypeFilter === "all" || data.sensorType === sensorTypeFilter)
    .filter(data => 
      data.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      data.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      data.unit?.toLowerCase().includes(searchQuery.toLowerCase())
    );

  const uniqueSensorTypes = ["all", ...new Set(sensorData.map(data => data.sensorType))];
  
  return (
    <Layout>
      <Container maxWidth="md">
        <Box sx={{ mt: 4, mb: 6 }}>
          {/* Profile Header */}
          <Box sx={{ mb: 4 }}>
            <Typography variant="h4" gutterBottom>
              {dogProfile?.name}
            </Typography>
            {dogProfile?.description && (
              <Typography 
                variant="body1" 
                sx={{ 
                  mb: 3,
                  color: 'text.secondary',
                  fontStyle: 'italic'
                }}
              >
                {dogProfile.description}
              </Typography>
            )}
          </Box>

          {/* Main Content */}
          <Paper 
            elevation={1} 
            sx={{ 
              p: 4, 
              mb: 4,
              backgroundColor: '#fff',
            }}
          >
            {/* Dog Details */}
            <Box sx={{ mb: 4 }}>
              <Typography variant="h5" gutterBottom>Details</Typography>
              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <Typography variant="body1">
                    <strong>Ras:</strong> {dogProfile?.breed || "Onbekend"}
                  </Typography>
                  <Typography variant="body1">
                    <strong>Leeftijd:</strong> {dogProfile?.age || "Onbekend"}
                  </Typography>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="body1">
                    <strong>Geslacht:</strong> {dogProfile?.gender || "Onbekend"}
                  </Typography>
                  <Typography variant="body1">
                    <strong>Gewicht:</strong> {dogProfile?.weight ? `${dogProfile.weight} kg` : "Onbekend"}
                  </Typography>
                </Grid>
              </Grid>
            </Box>

            {/* Notes Section */}
            {notes && notes.length > 0 && (
              <Box sx={{ mb: 4 }}>
                <Typography variant="h5" gutterBottom>Notities</Typography>
                {notes.map((note) => (
                  <Box key={note.id} sx={{ mb: 2 }}>
                    <Typography variant="body1">{note.text}</Typography>
                    <Typography variant="caption" color="text.secondary">
                      {new Date(note.creationDate).toLocaleDateString()}
                    </Typography>
                  </Box>
                ))}
              </Box>
            )}

            {/* Sensor Data Section (Admin Only) */}
            {isUserAdmin && sensorData && sensorData.length > 0 && (
              <Box sx={{ mb: 4 }}>
                <Typography variant="h5" gutterBottom>Sensor Data</Typography>
                {uniqueSensorTypes.map(type => (
                  <Box key={type} sx={{ mb: 3 }}>
                    <Typography variant="h6" gutterBottom>{type}</Typography>
                    {filteredSensorData
                      .filter(data => data.sensorType === type)
                      .map(data => (
                        <Box key={data.id} sx={{ mb: 2 }}>
                          <Typography variant="body1">
                            {data.name}: {data.averageValue} {data.unit}
                          </Typography>
                          <Typography variant="caption" color="text.secondary">
                            {new Date(data.creationDate).toLocaleDateString()}
                          </Typography>
                        </Box>
                      ))}
                  </Box>
                ))}
              </Box>
            )}
          </Paper>

          {/* Action Buttons */}
          <Box sx={{ display: "flex", gap: 2 }}>
            <Button
              variant="outlined"
              onClick={() => navigate("/dogprofiles")}
            >
              Terug naar Overzicht
            </Button>
            {isUserAdmin && (
              <Button
                variant="contained"
                color="primary"
                onClick={() => navigate(`/dogprofile-editor/${id}`)}
              >
                Bewerken
              </Button>
            )}
          </Box>
        </Box>
      </Container>
    </Layout>
  );
};

export default DogProfileDetailsPage;
