import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, Grid } from "@mui/material";
import axiosInstance from "../axios";

const ModuleResultPage = () => {
  const { id } = useParams(); // Module ID
  const navigate = useNavigate();
  const [module, setModule] = useState(null);
  const [progress, setProgress] = useState({ completed: 0, total: 0 });

  useEffect(() => {
    const fetchModuleData = async () => {
      try {
        const response = await axiosInstance.get(`/api/TrainingPath/${id}`);
        const moduleData = response.data;

        const totalItems = moduleData.lessons.length + moduleData.quizzes.length;
        const completedItems = moduleData.lessons.filter((l) => l.completed).length +
          moduleData.quizzes.filter((q) => q.completed).length;

        setModule(moduleData);
        setProgress({ completed: completedItems, total: totalItems });
      } catch (error) {
        console.error("Fout bij ophalen moduledata:", error);
      }
    };

    fetchModuleData();
  }, [id]);

  if (!module) {
    return (
      <Container maxWidth="md" sx={{ textAlign: "center", marginTop: "50px" }}>
        <Typography variant="h6">Samenvatting wordt geladen...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Samenvatting van Module: {module.name}
      </Typography>
      <Typography variant="body1" gutterBottom>
        {module.description}
      </Typography>

      <Box
        sx={{
          border: "1px solid #ccc",
          borderRadius: "4px",
          padding: "16px",
          marginTop: "16px",
        }}
      >
        <Typography variant="h6" gutterBottom>
          Resultaat
        </Typography>
        <Typography variant="body1">
          Voltooide items: {progress.completed}/{progress.total}
        </Typography>
      </Box>

      <Grid container spacing={2} sx={{ marginTop: "16px" }}>
        <Grid item xs={12} sm={6}>
          <Button
            variant="contained"
            fullWidth
            color="primary"
            onClick={() => navigate("/modules")}
          >
            Terug naar Modules
          </Button>
        </Grid>
        <Grid item xs={12} sm={6}>
          <Button
            variant="contained"
            fullWidth
            color="secondary"
            onClick={() => navigate(`/modules/${id}`)}
          >
            Bekijk Module Opnieuw
          </Button>
        </Grid>
      </Grid>
    </Container>
  );
};

export default ModuleResultPage;
