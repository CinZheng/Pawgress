import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";

const ModulesPage = () => {
  const [modules, setModules] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchModules = async () => {
      try {
        const response = await axiosInstance.get("/api/TrainingPath");
        setModules(response.data);
      } catch (error) {
        console.error("Fout bij ophalen modules:", error);
      }
    };

    fetchModules();
  }, []);

  return (
    <Layout>
    <Grid container spacing={2} padding={2}>
      <Grid item xs={12}>
        <Typography variant="h4" gutterBottom>
          Modules
        </Typography>
      </Grid>
      {modules.map((module) => (
        <Grid item xs={12} sm={6} md={4} key={module.trainingPathId}>
          <Card
            onClick={() => navigate(`/modules/${module.trainingPathId}`)}
            style={{ cursor: "pointer" }}
          >
            <CardContent>
              <Typography variant="h6">{module.name}</Typography>
              <Typography variant="body2">{module.description}</Typography>
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
    </Layout>
  );
};

export default ModulesPage;
