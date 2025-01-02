import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const DogsPage = () => {
  const [dogs, setDogs] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDogs = async () => {
      try {
        const response = await axiosInstance.get("/api/DogProfile");
        setDogs(response.data);
      } catch (error) {
        console.error("Fout bij ophalen hondenprofielen:", error);
      }
    };

    fetchDogs();
  }, []);

  return (
    <Grid container spacing={2} padding={2}>
      <Grid item xs={12}>
            <Typography variant="h4" gutterBottom>
              Honden
            </Typography>
          </Grid>
      {dogs.map((dog) => (
        <Grid item xs={12} sm={6} md={4} key={dog.dogProfileId}>
          <Card
            onClick={() => navigate(`/dogprofiles/${dog.dogProfileId}`)}
            style={{ cursor: "pointer" }}
          > 
            <CardContent>
              <Typography variant="h6">{dog.name}</Typography>
              <Typography variant="body2">Ras: {dog.breed}</Typography>
              <Typography variant="body2">
                Geboortedatum: {new Date(dog.dateOfBirth).toLocaleDateString()}
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );
};

export default DogsPage;
