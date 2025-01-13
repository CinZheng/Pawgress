import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  Button,
  Box,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,

} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Layout from '../components/Layout';
import axiosInstance from '../axios';
import SensorDataForm from './SensorDataForm';

const DogSensorDataPage = () => {
  const { dogProfileId } = useParams();
  const navigate = useNavigate();
  const [sensorData, setSensorData] = useState([]);
  const [dogProfile, setDogProfile] = useState(null);
  const [openAddDialog, setOpenAddDialog] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch dog profile details
        const profileResponse = await axiosInstance.get(`/api/DogProfile/${dogProfileId}`);
        setDogProfile(profileResponse.data);

        // Fetch sensor data for this dog
        const sensorResponse = await axiosInstance.get(`/api/DogSensorData/dog/${dogProfileId}`);
        setSensorData(sensorResponse.data);
      } catch (err) {
        console.error('Error fetching data:', err);
        setError('Er is een fout opgetreden bij het ophalen van de gegevens.');
      }
    };

    fetchData();
  }, [dogProfileId]);

  const handleDelete = async (sensorDataId) => {
    if (window.confirm('Weet je zeker dat je deze sensordata wilt verwijderen?')) {
      try {
        await axiosInstance.delete(`/api/DogSensorData/${sensorDataId}`);
        setSensorData(sensorData.filter(data => data.dogSensorDataId !== sensorDataId));
      } catch (err) {
        console.error('Error deleting sensor data:', err);
        setError('Er is een fout opgetreden bij het verwijderen van de sensordata.');
      }
    }
  };

  const handleAddSuccess = (newData) => {
    setSensorData([...sensorData, newData]);
    setOpenAddDialog(false);
  };

  return (
    <Layout>
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        {dogProfile && (
          <Box sx={{ mb: 4 }}>
            <Typography variant="h4" gutterBottom>
              Sensordata voor {dogProfile.name}
            </Typography>
            <Button
              variant="contained"
              startIcon={<AddIcon />}
              onClick={() => setOpenAddDialog(true)}
              sx={{ mb: 2 }}
            >
              Nieuwe Sensordata Toevoegen
            </Button>
          </Box>
        )}

        {error && (
          <Typography color="error" sx={{ mb: 2 }}>
            {error}
          </Typography>
        )}

        <Grid container spacing={3}>
          {sensorData.map((data) => (
            <Grid item xs={12} sm={6} md={4} key={data.dogSensorDataId}>
              <Card>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                    <Typography variant="h6" gutterBottom>
                      {data.name}
                    </Typography>
                    <IconButton 
                      size="small" 
                      onClick={() => handleDelete(data.dogSensorDataId)}
                      sx={{ color: 'error.main' }}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                  <Typography variant="body2" color="textSecondary" gutterBottom>
                    Type: {data.sensorType}
                  </Typography>
                  <Typography variant="body2" gutterBottom>
                    {data.description}
                  </Typography>
                  <Typography variant="body1">
                    {data.averageValue} {data.unit}
                  </Typography>
                  <Typography variant="body2" color="textSecondary">
                    Gemeten op: {new Date(data.recordedDate).toLocaleDateString()}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
          {sensorData.length === 0 && (
            <Grid item xs={12}>
              <Typography variant="body1" color="textSecondary">
                Nog geen sensordata beschikbaar voor deze hond.
              </Typography>
            </Grid>
          )}
        </Grid>

        <Dialog open={openAddDialog} onClose={() => setOpenAddDialog(false)} maxWidth="sm" fullWidth>
          <DialogTitle>Nieuwe Sensordata Toevoegen</DialogTitle>
          <DialogContent>
            <SensorDataForm 
              dogProfileId={dogProfileId} 
              onSuccess={handleAddSuccess}
              onCancel={() => setOpenAddDialog(false)}
            />
          </DialogContent>
        </Dialog>
      </Container>
    </Layout>
  );
};

export default DogSensorDataPage; 