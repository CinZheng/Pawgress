import React, { useState, useEffect } from "react";
import {
  Container,
  TextField,
  MenuItem,
  Button,
  Typography,
  CircularProgress,
  Alert,
  Box,
} from "@mui/material";
import axiosInstance from "../axios";

const SensorDataForm = () => {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    sensorType: "Accelerometer",
    unit: "",
    averageValue: "",
    recordedDate: new Date().toISOString(),
    dogProfileId: "",
  });

  const [dogProfiles, setDogProfiles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const sensorTypes = [
    "Accelerometer",
    "Gyroscope",
    "Temperature",
    "GPS",
    "Loopsnelheid",
    "Other",
  ];

  useEffect(() => {
    const fetchDogProfiles = async () => {
      setLoading(true);
      try {
        const response = await axiosInstance.get("/api/DogProfile");
        setDogProfiles(response.data);
        setLoading(false);
      } catch (err) {
        console.error("Fout bij ophalen DogProfiles:", err);
        setError("Kan DogProfiles niet ophalen. Probeer het later opnieuw.");
        setLoading(false);
      }
    };

    fetchDogProfiles();
  }, []);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (!formData.name || !formData.dogProfileId) {
      setError("Naam en DogProfile ID zijn verplicht.");
      return;
    }

    try {
      await axiosInstance.post("/api/DogSensorData", formData);
      alert("Sensor data toegevoegd!");
    } catch (err) {
      console.error("Fout bij toevoegen sensordata:", err);
      setError("Er is een fout opgetreden bij het opslaan van sensordata.");
    }
  };

  if (loading) return <CircularProgress />;

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Sensor Data Toevoegen
      </Typography>
      {error && <Alert severity="error">{error}</Alert>}
      <form onSubmit={handleSubmit}>
        <TextField
          name="name"
          label="Naam"
          fullWidth
          margin="normal"
          value={formData.name}
          onChange={handleChange}
          required
        />
        <TextField
          name="description"
          label="Beschrijving"
          fullWidth
          margin="normal"
          value={formData.description}
          onChange={handleChange}
        />
        <TextField
          name="sensorType"
          select
          label="Sensor Type"
          fullWidth
          margin="normal"
          value={formData.sensorType}
          onChange={handleChange}
        >
          {sensorTypes.map((type) => (
            <MenuItem key={type} value={type}>
              {type}
            </MenuItem>
          ))}
        </TextField>
        <TextField
          name="unit"
          label="Eenheid"
          fullWidth
          margin="normal"
          value={formData.unit}
          onChange={handleChange}
        />
        <TextField
          name="averageValue"
          label="Gemiddelde Waarde"
          type="number"
          fullWidth
          margin="normal"
          value={formData.averageValue}
          onChange={handleChange}
        />
        <TextField
          name="recordedDate"
          label="Opnamedatum"
          type="datetime-local"
          fullWidth
          margin="normal"
          value={formData.recordedDate}
          onChange={handleChange}
        />
        <TextField
          name="dogProfileId"
          select
          label="DogProfile"
          fullWidth
          margin="normal"
          value={formData.dogProfileId}
          onChange={handleChange}
          required
        >
          {dogProfiles.map((profile) => (
            <MenuItem key={profile.dogProfileId} value={profile.dogProfileId}>
              {profile.name}
            </MenuItem>
          ))}
        </TextField>
        <Box textAlign="center" mt={3}>
          <Button type="submit" variant="contained" color="primary">
            Opslaan
          </Button>
        </Box>
      </form>
    </Container>
  );
};

export default SensorDataForm;
