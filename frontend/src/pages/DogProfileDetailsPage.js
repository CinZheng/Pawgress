import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axiosInstance from "../axios";
import { Typography, Container } from "@mui/material";

const DogDetailsPage = () => {
  const { id } = useParams();
  const [dog, setDog] = useState(null);

  useEffect(() => {
    const fetchDogDetails = async () => {
      try {
        const response = await axiosInstance.get(`/api/DogProfile/${id}`);
        setDog(response.data);
      } catch (error) {
        console.error("Fout bij ophalen hondenprofiel details:", error);
      }
    };

    fetchDogDetails();
  }, [id]);

  if (!dog) return <Typography>Hondenprofiel wordt geladen...</Typography>;

  return (
    <Container>
      <Typography variant="h4">{dog.name}</Typography>
      <Typography variant="body1">Ras: {dog.breed}</Typography>
      <Typography variant="body1">
        Geboortedatum: {new Date(dog.dateOfBirth).toLocaleDateString()}
      </Typography>
      {/* notities */}
    </Container>
  );
};

export default DogDetailsPage;
