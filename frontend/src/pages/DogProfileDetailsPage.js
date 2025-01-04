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
} from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const DogProfileDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [dogProfile, setDogProfile] = useState(null);
  const [notes, setNotes] = useState([]);
  const [selectedNote, setSelectedNote] = useState(null); // Voor geselecteerde notitie
  const [newNote, setNewNote] = useState({ description: "", tag: "" });
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [addNoteDialogOpen, setAddNoteDialogOpen] = useState(false);
  const [noteDeleteDialogOpen, setNoteDeleteDialogOpen] = useState(false); // Notitie-verwijder dialoog

  useEffect(() => {
    const fetchDogProfile = async () => {
      try {
        const response = await axiosInstance.get(`/api/DogProfile/${id}`);
        setDogProfile(response.data);
      } catch (error) {
        console.error("Fout bij ophalen hondenprofiel:", error);
      }
    };

    const fetchNotes = async () => {
      try {
        const response = await axiosInstance.get(`/api/Note/dogprofile/${id}`);
        setNotes(response.data);
      } catch (error) {
        console.error("Fout bij ophalen notities:", error);
      }
    };

    fetchDogProfile();
    fetchNotes();
    setIsUserAdmin(isAdmin());
  }, [id]);

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

  if (!dogProfile) {
    return <Typography>Hondenprofiel wordt geladen...</Typography>;
  }

  return (
    <Layout>
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
