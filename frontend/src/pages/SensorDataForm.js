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
import Layout from "../components/Layout";

const SensorDataForm = () => {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    sensorType: "Accelerometer",
    unit: "",
    averageValue: 0,
    dogProfileId: "",
  });

  const [dogProfiles, setDogProfiles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  // These must match the backend enum exactly
  const sensorTypes = [
    "Accelerometer",
    "Gyroscope",
    "Temperature",
    "GPS",
    "Loopsnelheid",
    "Other"
  ];

  useEffect(() => {
    const fetchDogProfiles = async () => {
      setLoading(true);
      try {
        const response = await axiosInstance.get("/api/DogProfile");
        setDogProfiles(response.data);
      } catch (err) {
        console.error("Fout bij ophalen DogProfiles:", err);
        setError("Kan DogProfiles niet ophalen. Probeer het later opnieuw.");
      } finally {
        setLoading(false);
      }
    };

    fetchDogProfiles();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    let processedValue = value;

    if (name === 'averageValue') {
      processedValue = parseFloat(value) || 0;
    }

    if (name === 'dogProfileId') {
      processedValue = value; // Keep as string, will be converted to GUID by backend
    }

    setFormData(prev => ({ ...prev, [name]: processedValue }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (!formData.name || !formData.dogProfileId) {
      setError("Naam en DogProfile zijn verplicht.");
      return;
    }

    try {
      const dataToSend = {
        name: formData.name,
        description: formData.description,
        sensorType: formData.sensorType,
        unit: formData.unit,
        averageValue: parseFloat(formData.averageValue) || 0,
        dogProfileId: formData.dogProfileId
      };

      await axiosInstance.post("/api/DogSensorData", dataToSend);
      setSuccess("Sensor data succesvol toegevoegd!");
      // Clear form
      setFormData({
        name: "",
        description: "",
        sensorType: "Accelerometer",
        unit: "",
        averageValue: 0,
        dogProfileId: "",
      });
    } catch (err) {
      console.error("Fout bij toevoegen sensordata:", err);
      const errorMessage = err.response?.data?.errors?.DogProfile?.[0] || 
                          err.response?.data?.error || 
                          err.response?.data?.message || 
                          err.response?.data || 
                          err.message || 
                          "Er is een fout opgetreden bij het opslaan van sensordata.";
      setError(typeof errorMessage === 'string' ? errorMessage : JSON.stringify(errorMessage));
    }
  };

  if (loading) {
    return (
      <Layout>
        <Container sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
          <CircularProgress />
        </Container>
      </Layout>
    );
  }

  return (
    <Layout>
      <Container>
        <Typography variant="h4" gutterBottom>
          Sensor Data Toevoegen
        </Typography>
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}
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
            multiline
            rows={2}
          />
          <TextField
            name="sensorType"
            select
            label="Sensor Type"
            fullWidth
            margin="normal"
            value={formData.sensorType}
            onChange={handleChange}
            required
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
            required
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
    </Layout>
  );
};

export default SensorDataForm;
