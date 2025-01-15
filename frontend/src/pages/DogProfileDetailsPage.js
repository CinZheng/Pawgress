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
import DataSensorIcon from '@mui/icons-material/DataUsage';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';

console.log('Loading DogProfileDetailsPage file');

const DogProfileDetailsPage = () => {
  console.log('Starting DogProfileDetailsPage component');
  
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

  console.log('Initial setup complete');

  useEffect(() => {
    const fetchData = async () => {
      try {
        console.log('Starting data fetch');
        const adminCheck = isAdmin();
        console.log('Admin check result:', adminCheck);
        setIsUserAdmin(adminCheck);
        
        const profileResponse = await axiosInstance.get(`/api/DogProfile/${id}`);
        console.log('Dog profile data:', profileResponse.data);
        setDogProfile(profileResponse.data);
        
        const notesResponse = await axiosInstance.get(`/api/Note/dogprofile/${id}`);
        console.log('Notes data:', notesResponse.data);
        setNotes(notesResponse.data);
        
        const favoriteResponse = await axiosInstance.get(`/api/DogProfile/${id}/favorite/${userId}`);
        console.log('Favorite status:', favoriteResponse.data);
        setIsFavorite(favoriteResponse.data.isFavorite);

        if (adminCheck) {
          console.log('Fetching sensor data for dog:', id);
          const sensorResponse = await axiosInstance.get(`/api/DogSensorData/dog/${id}`);
          console.log('Received sensor data:', sensorResponse.data);
          setSensorData(Array.isArray(sensorResponse.data) ? sensorResponse.data : []);
        }
      } catch (error) {
        console.error('Error fetching data:', error);
        setError(error.message);
      }
    };

    fetchData();
  }, [id, userId]);

  // Log state changes
  useEffect(() => {
    console.log('State updated:', {
      isUserAdmin,
      sensorDataCount: sensorData.length,
      hasProfile: !!dogProfile,
      notesCount: notes.length,
      isFavorite
    });
  }, [isUserAdmin, sensorData, dogProfile, notes, isFavorite]);

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

  console.log('Before render, state:', { isUserAdmin, sensorData });
  
  return (
    <Layout>
      <Container maxWidth="lg">
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 4 }}>
          <Typography variant="h4" component="h1">
            {dogProfile.name}
          </Typography>
          <IconButton 
            onClick={handleToggleFavorite}
            sx={{ 
              color: isFavorite ? 'primary.main' : 'grey.400',
              transform: isFavorite ? 'scale(1.1)' : 'scale(1)',
              transition: 'all 0.2s ease-in-out',
              '&:hover': {
                transform: 'scale(1.2)',
              }
            }}
          >
            <PetsIcon />
          </IconButton>
        </Box>
        
        <Box
          sx={{
            border: "1px solid #ccc",
            borderRadius: "4px",
            padding: "16px",
            backgroundColor: "#f9f9f9",
            mb: 4
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

        <Typography variant="h5" sx={{ marginTop: 4 }}>
          Notities
        </Typography>
        <Button variant="contained" color="primary" onClick={() => openEditDialog({})} sx={{ marginBottom: 2 }}>
          Notitie Toevoegen
        </Button>
        <Grid container spacing={2}>
          {notes.map((note) => (
            <Grid item xs={12} sm={6} md={4} key={note.noteId}>
              <Card>
                <CardContent>
                  <Typography variant="subtitle1">
                    {note.userName || "Onbekend"}
                  </Typography>
                  <Typography variant="body2">{note.description}</Typography>
                    {note.tag && (
                  <Typography variant="caption" sx={{ color: "gray", display: "block", marginBottom: 1 }}>
                    Tag: {note.tag}
                  </Typography>
            )}
            {(isUserAdmin || note.userId === localStorage.getItem("userId")) && (
              <Button
                variant="text"
                color="error"
                onClick={() => openDeleteDialog(note)}
              >
                Verwijderen
              </Button>
              )}
            </CardContent>
          </Card>
            </Grid>
          ))}
        </Grid>

        {/* Sensor Data Section */}
        {console.log('About to render sensor section, isUserAdmin:', isUserAdmin)}
        {isUserAdmin ? (
          <Box sx={{ mt: 4 }}>
            {console.log('Inside sensor data section')}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Typography variant="h5">
                Sensor Data ({sensorData.length})
              </Typography>
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                onClick={() => navigate('/sensor-data-form')}
              >
                Sensordata Toevoegen
              </Button>
            </Box>
            
            {/* Filters */}
            <Box sx={{ mb: 2, display: 'flex', gap: 2 }}>
              <FormControl sx={{ minWidth: 200 }}>
                <InputLabel>Filter op Type</InputLabel>
                <Select
                  value={sensorTypeFilter}
                  onChange={(e) => setSensorTypeFilter(e.target.value)}
                  label="Filter op Type"
                >
                  {uniqueSensorTypes.map(type => (
                    <MenuItem key={type} value={type}>
                      {type === "all" ? "Alle Types" : type}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
              
              <TextField
                label="Zoeken"
                variant="outlined"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                sx={{ flexGrow: 1 }}
              />
            </Box>

            {/* Sensor Data Table */}
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Naam</TableCell>
                    <TableCell>Type</TableCell>
                    <TableCell>Beschrijving</TableCell>
                    <TableCell>Waarde</TableCell>
                    <TableCell>Eenheid</TableCell>
                    <TableCell>Datum</TableCell>
                    <TableCell>Acties</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {filteredSensorData.map((data) => (
                    <TableRow key={data.dogSensorDataId}>
                      <TableCell>{data.name}</TableCell>
                      <TableCell>{data.sensorType}</TableCell>
                      <TableCell>{data.description}</TableCell>
                      <TableCell>{data.averageValue}</TableCell>
                      <TableCell>{data.unit}</TableCell>
                      <TableCell>
                        {new Date(data.creationDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        <IconButton
                          color="error"
                          onClick={() => handleDeleteSensorData(data.dogSensorDataId)}
                          size="small"
                        >
                          <DeleteIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
            
            {filteredSensorData.length === 0 && (
              <Typography sx={{ mt: 2, textAlign: 'center', color: 'text.secondary' }}>
                Geen sensor data gevonden
              </Typography>
            )}
          </Box>
        ) : console.log('User is not admin, skipping sensor section')}

        {/* Popup for Adding/Editing Note - editing wip */}
        <Dialog open={addNoteDialogOpen} onClose={() => setAddNoteDialogOpen(false)}>
          <DialogTitle>
            {selectedNote?.noteId ? "Notitie Bewerken" : "Nieuwe Notitie Toevoegen"} 
          </DialogTitle>
          <DialogContent>
            <TextField
              label="Beschrijving"
              fullWidth
              multiline
              rows={4}
              value={selectedNote?.description || ""}
              onChange={(e) =>
                setSelectedNote({ ...selectedNote, description: e.target.value })
              }
              sx={{ marginBottom: 2 }}
            />
            <TextField
              label="Tag"
              fullWidth
              value={selectedNote?.tag || ""}
              onChange={(e) => setSelectedNote({ ...selectedNote, tag: e.target.value })}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setAddNoteDialogOpen(false)} color="secondary">
              Annuleren
            </Button>
            <Button onClick={handleAddOrEditNote} color="primary">
              {selectedNote?.noteId ? "Bewerken" : "Toevoegen"}
            </Button>
          </DialogActions>
        </Dialog>

        {/* Popup for Deleting Note */}
        <Dialog open={noteDeleteDialogOpen} onClose={() => setNoteDeleteDialogOpen(false)}>
          <DialogTitle>Notitie Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>
              Weet je zeker dat je deze notitie wilt verwijderen?
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setNoteDeleteDialogOpen(false)} color="secondary">
              Annuleren
            </Button>
            <Button onClick={handleDeleteNote} color="error">
              Verwijderen
            </Button>
          </DialogActions>
        </Dialog>

        {/* Confirmatie dialoog voor verwijderen hondenprofiel */}
        <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
          <DialogTitle>Hond Verwijderen</DialogTitle>
          <DialogContent>
            <Typography>Weet je zeker dat je dit hondenprofiel wilt verwijderen?</Typography>
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

export default DogProfileDetailsPage;
