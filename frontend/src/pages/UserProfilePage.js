import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  Button,
  Box,
  LinearProgress,
  Divider,
  Avatar,
  useMediaQuery,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import PetsIcon from '@mui/icons-material/Pets';
import LogoutIcon from '@mui/icons-material/Logout';
import Layout from '../components/Layout';
import axiosInstance from '../axios';

const UserProfilePage = () => {
  const navigate = useNavigate();
  const [favoriteDogs, setFavoriteDogs] = useState([]);
  const [modules, setModules] = useState([]);
  const [userProfile, setUserProfile] = useState(null);
  const userId = localStorage.getItem('userId');
  const isMobile = useMediaQuery('(max-width:600px)');

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    navigate('/login');
  };

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        // Fetch user profile
        const profileResponse = await axiosInstance.get(`/api/User/${userId}`);
        setUserProfile(profileResponse.data);

        // Fetch favorite dogs
        const dogsResponse = await axiosInstance.get(`/api/User/${userId}/favorites`);
        setFavoriteDogs(dogsResponse.data);

        // Fetch modules progress
        const modulesResponse = await axiosInstance.get(`/api/User/${userId}/modules`);
        setModules(modulesResponse.data);
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };

    fetchUserData();
  }, [userId]);

  return (
    <Layout>
      <Container maxWidth="lg">
        {/* User Info Section */}
        <Box sx={{ 
          display: 'flex', 
          flexDirection: 'column', 
          alignItems: 'center',
          mb: 6 
        }}>
          <Avatar
            sx={{
              width: 120,
              height: 120,
              mb: 3,
              bgcolor: 'primary.main',
              fontSize: '3rem'
            }}
          >
            {userProfile?.username?.charAt(0)?.toUpperCase()}
          </Avatar>
          <Typography variant="h4" component="h1" gutterBottom>
            {userProfile?.username}
          </Typography>
          <Typography variant="body1" color="textSecondary" gutterBottom>
            {userProfile?.email}
          </Typography>
          <Box sx={{ display: 'flex', flexDirection: isMobile ? 'column' : 'row', gap: 2, width: isMobile ? '100%' : 'auto' }}>
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={() => navigate('/profile/edit')}
              fullWidth={isMobile}
            >
              Profiel Bewerken
            </Button>
            {isMobile && (
              <Button
                variant="outlined"
                color="secondary"
                startIcon={<LogoutIcon />}
                onClick={handleLogout}
                fullWidth
              >
                Uitloggen
              </Button>
            )}
          </Box>
        </Box>

        {/* Favorite Dogs Section */}
        <Typography variant="h5" component="h2" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PetsIcon />
          Favoriete Honden
        </Typography>
        <Grid container spacing={3} sx={{ mb: 4 }}>
          {favoriteDogs.map((dog) => (
            <Grid item xs={12} sm={6} md={4} key={dog.dogProfileId}>
              <Card 
                onClick={() => navigate(`/dogprofiles/${dog.dogProfileId}`)}
                sx={{ 
                  cursor: 'pointer',
                  '&:hover': { transform: 'scale(1.02)', transition: 'transform 0.2s' }
                }}
              >
                <CardContent>
                  {dog.image && (
                    <img
                      src={dog.image}
                      alt={dog.name}
                      style={{
                        width: '100%',
                        height: '160px',
                        objectFit: 'cover',
                        borderRadius: '4px',
                        marginBottom: '8px'
                      }}
                    />
                  )}
                  <Typography variant="h6">{dog.name}</Typography>
                  <Typography variant="body2" color="textSecondary">
                    {dog.breed}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
          {favoriteDogs.length === 0 && (
            <Grid item xs={12}>
              <Typography variant="body1" color="textSecondary">
                Je hebt nog geen favoriete honden.
              </Typography>
            </Grid>
          )}
        </Grid>

        <Divider sx={{ my: 4 }} />

        {/* Modules Section */}
        <Typography variant="h5" component="h2" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PetsIcon />
          Mijn Modules
        </Typography>
        <Grid container spacing={3}>
          {modules.map((module) => (
            <Grid item xs={12} sm={6} md={4} key={module.trainingPathId}>
              <Card 
                onClick={() => navigate(`/modules/${module.trainingPathId}`)}
                sx={{ 
                  cursor: 'pointer',
                  '&:hover': { transform: 'scale(1.02)', transition: 'transform 0.2s' }
                }}
              >
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    {module.name}
                  </Typography>
                  <Box sx={{ mb: 2 }}>
                    <LinearProgress 
                      variant="determinate" 
                      value={module.percentageComplete || 0}
                      sx={{ height: 8, borderRadius: 4 }}
                    />
                    <Typography variant="body2" color="textSecondary" sx={{ mt: 1 }}>
                      {Math.round(module.percentageComplete || 0)}% voltooid
                    </Typography>
                  </Box>
                  <Typography variant="body2" color="textSecondary">
                    Status: {module.status}
                  </Typography>
                  {module.startDate && (
                    <Typography variant="body2" color="textSecondary">
                      Gestart op: {new Date(module.startDate).toLocaleDateString()}
                    </Typography>
                  )}
                </CardContent>
              </Card>
            </Grid>
          ))}
          {modules.length === 0 && (
            <Grid item xs={12}>
              <Typography variant="body1" color="textSecondary">
                Je hebt nog geen modules gestart.
              </Typography>
            </Grid>
          )}
        </Grid>
      </Container>
    </Layout>
  );
};

export default UserProfilePage; 