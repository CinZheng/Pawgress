import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography, Button, IconButton, Box, Container } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";
import PetsIcon from "@mui/icons-material/Pets";

const DogProfileOverviewPage = () => {
  const [dogProfiles, setDogProfiles] = useState([]);
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const [favorites, setFavorites] = useState({});
  const navigate = useNavigate();
  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchDogs = async () => {
      try {
        const response = await axiosInstance.get("/api/DogProfile");
        setDogProfiles(response.data);
        
        // Fetch favorite status for each dog
        const favoriteStatuses = {};
        await Promise.all(
          response.data.map(async (dog) => {
            const favoriteResponse = await axiosInstance.get(
              `/api/DogProfile/${dog.dogProfileId}/favorite/${userId}`
            );
            favoriteStatuses[dog.dogProfileId] = favoriteResponse.data.isFavorite;
          })
        );
        setFavorites(favoriteStatuses);
      } catch (error) {
        console.error("Error fetching dog profiles:", error);
      }
    };

    fetchDogs();
    setIsUserAdmin(isAdmin());
  }, [userId]);

  const handleToggleFavorite = async (dogId, event) => {
    event.stopPropagation(); // Prevent card click when clicking the star
    try {
      const response = await axiosInstance.post(
        `/api/DogProfile/${dogId}/favorite/${userId}`
      );
      setFavorites(prev => ({
        ...prev,
        [dogId]: response.data.isFavorite
      }));
    } catch (error) {
      console.error("Error toggling favorite:", error);
    }
  };

  return (
    <Layout>
      <Container maxWidth="lg">
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Honden
          </Typography>
          {isUserAdmin && (
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate("/dogprofile-editor")}
              sx={{ mb: 2 }}
            >
              Nieuwe Hond Toevoegen
            </Button>
          )}
        </Box>
        <Grid container spacing={3}>
          {dogProfiles.map((dog) => (
            <Grid item xs={12} sm={6} md={4} key={dog.dogProfileId}>
              <Card
                onClick={() => navigate(`/dogprofiles/${dog.dogProfileId}`)}
                style={{ cursor: "pointer" }}
              >
                <CardContent>
                  <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 1 }}>
                    <Typography variant="h6">{dog.name}</Typography>
                    <IconButton
                      onClick={(e) => handleToggleFavorite(dog.dogProfileId, e)}
                      sx={{ 
                        color: favorites[dog.dogProfileId] ? 'primary.main' : 'grey.400',
                        transform: favorites[dog.dogProfileId] ? 'scale(1.1)' : 'scale(1)',
                        transition: 'all 0.2s ease-in-out',
                        '&:hover': {
                          transform: 'scale(1.2)',
                        }
                      }}
                    >
                      <PetsIcon />
                    </IconButton>
                  </Box>
                  {dog.image ? (
                    <img
                      src={dog.image}
                      alt={dog.name}
                      style={{
                        width: "100%",
                        height: "200px",
                        objectFit: "cover",
                        borderRadius: "8px",
                        marginBottom: "10px",
                      }}
                    />
                  ) : (
                    <div
                      style={{
                        width: "100%",
                        height: "200px",
                        backgroundColor: "#e0e0e0",
                        borderRadius: "8px",
                        marginBottom: "10px",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        fontSize: "14px",
                        color: "#757575",
                      }}
                    >
                      Geen afbeelding beschikbaar
                    </div>
                  )}
                  <Typography variant="body2">Ras: {dog.breed}</Typography>
                  <Typography variant="body2">
                    Geboortedatum: {new Date(dog.dateOfBirth).toLocaleDateString()}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Layout>
  );
};

export default DogProfileOverviewPage;
