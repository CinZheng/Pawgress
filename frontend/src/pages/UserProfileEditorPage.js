import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  TextField,
  Button,
  Box,
} from '@mui/material';
import Layout from '../components/Layout';
import axiosInstance from '../axios';
import { useNotification } from '../context/NotificationContext';

const UserProfileEditorPage = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: '',
    email: '',
  });
  const { showNotification } = useNotification();
  const userId = localStorage.getItem('userId');

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axiosInstance.get(`/api/User/${userId}`);
        setFormData({
          username: response.data.username,
          email: response.data.email,
        });
      } catch (error) {
        console.error('Error fetching user data:', error);
        showNotification('Kon gebruikersgegevens niet laden', 'error');
      }
    };

    fetchUserData();
  }, [userId, showNotification]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.put(`/api/User/${userId}`, formData);
      showNotification('Profiel succesvol bijgewerkt!', 'success');
      setTimeout(() => navigate('/profile'), 1500);
    } catch (error) {
      console.error('Error updating profile:', error);
      showNotification('Kon profiel niet bijwerken', 'error');
    }
  };

  return (
    <Layout>
      <Container maxWidth="sm">
        <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
          Profiel Bewerken
        </Typography>

        <Box
          component="form"
          onSubmit={handleSubmit}
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: 3,
          }}
        >
          <TextField
            label="Gebruikersnaam"
            name="username"
            value={formData.username}
            onChange={handleInputChange}
            fullWidth
            required
          />

          <TextField
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleInputChange}
            fullWidth
            required
          />

          <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
            <Button
              type="button"
              variant="outlined"
              onClick={() => navigate('/profile')}
            >
              Annuleren
            </Button>
            <Button
              type="submit"
              variant="contained"
              color="primary"
            >
              Opslaan
            </Button>
          </Box>
        </Box>
      </Container>
    </Layout>
  );
};

export default UserProfileEditorPage; 