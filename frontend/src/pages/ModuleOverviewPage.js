import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
  Container,
  Typography,
  Box,
  Grid,
  Card,
  CardContent,
  Button,
} from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const ModuleOverviewPage = () => {
  const [modules, setModules] = useState([]);
  const [progression, setProgression] = useState({});
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const navigate = useNavigate();
  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchModules = async () => {
      try {
        const response = await axiosInstance.get("/api/TrainingPath");
        setModules(response.data);

        if (!isUserAdmin && userId) {
          const progressPromises = response.data.map((module) =>
            axiosInstance
              .get(`/api/TrainingPath/${module.trainingPathId}/progress/${userId}`)
              .then((res) => ({
                moduleId: module.trainingPathId,
                progress: res.data,
              }))
              .catch(() => null)
          );

          const progressResults = await Promise.all(progressPromises);
          const progressMap = {};
          progressResults.forEach((result) => {
            if (result) {
              progressMap[result.moduleId] = result.progress;
            }
          });
          setProgression(progressMap);
        }
      } catch (error) {
        console.error("Error fetching modules:", error);
      }
    };

    fetchModules();
    setIsUserAdmin(isAdmin());
  }, [userId, isUserAdmin]);

  return (
    <Layout>
      <Box sx={{ mt: 4, mb: 6 }}>
        <Container maxWidth="xl">
          <Grid container spacing={3} sx={{ px: { xs: 2, sm: 0 } }}>
            {/* Admin Button */}
            {isUserAdmin && (
              <Grid item xs={12} sx={{ mb: 2 }}>
                <Button
                  variant="contained"
                  sx={{
                    backgroundColor: '#663366',
                    '&:hover': {
                      backgroundColor: '#4a2649'
                    }
                  }}
                  onClick={() => navigate("/module-editor")}
                >
                  Nieuwe Module Aanmaken
                </Button>
              </Grid>
            )}

            <Grid item xs={12}>
              <Typography 
                variant="h4" 
                sx={{ 
                  mb: 3,
                  color: '#663366',
                  fontWeight: 'bold'
                }}
              >
                Modules
              </Typography>
            </Grid>

            {modules.map((module) => (
              <Grid item xs={12} key={module.trainingPathId}>
                <Card 
                  onClick={() => navigate(`/modules/${module.trainingPathId}`)}
                  sx={{
                    cursor: "pointer",
                    backgroundColor: '#663366',
                    borderRadius: '12px',
                    maxWidth: '100%',
                    '&:hover': {
                      backgroundColor: '#4a2649',
                      transform: 'scale(1.01)',
                      transition: 'all 0.2s ease-in-out'
                    }
                  }}
                >
                  <CardContent sx={{ p: 3 }}>
                    <Typography 
                      variant="h5" 
                      sx={{ 
                        color: 'white',
                        fontWeight: 'bold',
                        mb: 1
                      }}
                    >
                      {module.name}
                    </Typography>
                    <Typography 
                      variant="body1" 
                      sx={{ 
                        color: 'white',
                        opacity: 0.9,
                        fontStyle: 'italic',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        display: '-webkit-box',
                        WebkitLineClamp: 1,
                        WebkitBoxOrient: 'vertical',
                        lineHeight: '1.5em',
                        height: '1.5em'
                      }}
                    >
                      {module.description}
                    </Typography>

                    {!isUserAdmin && progression[module.trainingPathId] && (
                      <Box sx={{ mt: 2 }}>
                        <Box sx={{ 
                          width: '100%', 
                          height: 4, 
                          bgcolor: 'rgba(255, 255, 255, 0.2)',
                          borderRadius: 2,
                          position: 'relative'
                        }}>
                          <Box sx={{
                            width: `${progression[module.trainingPathId].percentageComplete}%`,
                            height: '100%',
                            bgcolor: '#FFD700',
                            borderRadius: 2,
                            transition: 'width 0.5s ease-in-out'
                          }} />
                        </Box>
                        <Typography 
                          variant="caption" 
                          sx={{ 
                            mt: 1, 
                            display: 'block',
                            color: 'white'
                          }}
                        >
                          {`${progression[module.trainingPathId].completedItems} van ${progression[module.trainingPathId].totalItems} voltooid (${Math.round(progression[module.trainingPathId].percentageComplete)}%)`}
                        </Typography>
                        <Typography 
                          variant="caption" 
                          sx={{ 
                            display: 'block',
                            color: 'white'
                          }}
                        >
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
      </Box>
    </Layout>
  );
};

export default ModuleOverviewPage;
