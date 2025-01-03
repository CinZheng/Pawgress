import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";

const DogProfileOverviewPage = () => {
  const [dogProfile, setDogs] = useState([]);
  const [isUserAdmin, setIsUserAdmin] = useState(false);
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
    setIsUserAdmin(isAdmin());
  }, []);

  return (
    <Grid container spacing={2} padding={2}>
      {isUserAdmin && (
        <Grid item xs={12} style={{ marginBottom: "20px" }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate("/dogprofile-editor")}
          >
            Nieuwe Hond Toevoegen
          </Button>
        </Grid>
      )}
      {dogProfile.map((dog) => (
        <Grid item xs={12} sm={6} md={4} key={dog.dogProfileId}>
          <Card
            onClick={() => navigate(`/dogprofiles/${dog.dogProfileId}`)}
            style={{ cursor: "pointer" }}
          >
            <CardContent>
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

export default DogProfileOverviewPage;
