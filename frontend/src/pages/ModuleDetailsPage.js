import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axiosInstance from "../axios";
import { Typography, Container } from "@mui/material";

const ModuleDetailsPage = () => {
  const { id } = useParams();
  const [module, setModule] = useState(null);

  useEffect(() => {
    const fetchModuleDetails = async () => {
      try {
        const response = await axiosInstance.get(`/api/TrainingPath/${id}`);
        setModule(response.data);
      } catch (error) {
        console.error("Fout bij ophalen module details:", error);
      }
    };

    fetchModuleDetails();
  }, [id]);

  if (!module) return <Typography>Module wordt geladen...</Typography>;

  return (
    <Container>
      <Typography variant="h4">{module.name}</Typography>
      <Typography variant="body1">{module.description}</Typography>
      {/*extra details lessen of quizzes */}
    </Container>
  );
};

export default ModuleDetailsPage;
