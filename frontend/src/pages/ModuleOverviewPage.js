import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { Grid, Card, CardContent, Typography, Button, Box, Container } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const ModuleOverviewPage = () => {
  const [modules, setModules] = useState([]);
  const [progression, setProgression] = useState({});
  const navigate = useNavigate();
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchModules = async () => {
      try {
        const response = await axiosInstance.get("/api/TrainingPath");
        setModules(response.data);

        if (!isAdmin()) {
          // Fetch progression for each module
          const progressPromises = response.data.map(async (module) => {
            try {
              const progressResponse = await axiosInstance.get(
                `/api/TrainingPath/${module.trainingPathId}/progress/${userId}`
              );
              return { 
                [module.trainingPathId]: {
                  completedItems: progressResponse.data.completedItems,
                  totalItems: progressResponse.data.totalItems,
                  percentageComplete: progressResponse.data.percentageComplete,
                  status: progressResponse.data.status
                }
              };
            } catch (error) {
              console.error(`Error fetching progress for module ${module.trainingPathId}:`, error);
              return { 
                [module.trainingPathId]: {
                  completedItems: 0,
                  totalItems: 0,
                  percentageComplete: 0,
                  status: 'Not Started'
                }
              };
            }
          });

          const progressData = await Promise.all(progressPromises);
          const progressMap = progressData.reduce(
            (acc, curr) => ({ ...acc, ...curr }),
            {}
          );
          setProgression(progressMap);
        }
      } catch (error) {
        console.error("Error fetching modules or progress data:", error);
      }
    };

    fetchModules();
    setIsUserAdmin(isAdmin());
  }, [userId]);

  return (
    <Layout>
      <Container maxWidth="lg">
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Modules
          </Typography>
          {isUserAdmin && (
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate("/module-editor")}
              sx={{ mb: 2 }}
            >
              Nieuwe Module Aanmaken
            </Button>
          )}
        </Box>
        <Grid container spacing={3}>
          {modules.map((module) => (
            <Grid item xs={12} sm={6} md={4} key={module.trainingPathId}>
              <Card
                onClick={() => navigate(`/modules/${module.trainingPathId}`)}
                style={{ cursor: "pointer" }}
              >
                <CardContent>
                  <Typography variant="h6">{module.name}</Typography>
                  <Typography variant="body2">{module.description}</Typography>
                  {!isUserAdmin && progression[module.trainingPathId] && (
                    <Box sx={{ mt: 2 }}>
                      <Box sx={{ 
                        width: '100%', 
                        height: 4, 
                        bgcolor: '#e0e0e0',
                        borderRadius: 2,
                        position: 'relative'
                      }}>
                        <Box sx={{
                          width: `${progression[module.trainingPathId].percentageComplete}%`,
                          height: '100%',
                          bgcolor: 'primary.main',
                          borderRadius: 2,
                          transition: 'width 0.5s ease-in-out'
                        }} />
                      </Box>
                      <Typography variant="caption" color="textSecondary" sx={{ mt: 1, display: 'block' }}>
                        {`${progression[module.trainingPathId].completedItems} of ${progression[module.trainingPathId].totalItems} completed (${Math.round(progression[module.trainingPathId].percentageComplete)}%)`}
                      </Typography>
                      <Typography variant="caption" color="textSecondary" sx={{ display: 'block' }}>
                        Status: {progression[module.trainingPathId].status}
                      </Typography>
                    </Box>
                  )}
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Layout>
  );
};

export default ModuleOverviewPage;
