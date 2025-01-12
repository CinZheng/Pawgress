import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, CircularProgress } from "@mui/material";
import axiosInstance from "../axios";

const ModuleResultPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [moduleData, setModuleData] = useState(null);
  const [loading, setLoading] = useState(true);
  const userId = localStorage.getItem('userId');

  useEffect(() => {
    const fetchModuleProgress = async () => {
      try {
        const [moduleResponse, progressResponse] = await Promise.all([
          axiosInstance.get(`/api/TrainingPath/${id}`),
          axiosInstance.get(`/api/TrainingPath/${id}/progress/${userId}`)
        ]);

        setModuleData({
          ...moduleResponse.data,
          progress: progressResponse.data
        });
      } catch (error) {
        console.error('Error fetching module result:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchModuleProgress();
  }, [id, userId]);

  if (loading) {
    return (
      <Container sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="sm">
      <Box sx={{ textAlign: 'center', mt: 4 }}>
        <Typography variant="h4" gutterBottom>
          Module Complete!
        </Typography>
        
        <Typography variant="h5" gutterBottom>
          {moduleData?.name}
        </Typography>

        <Typography variant="body1" sx={{ mt: 2 }}>
          Progress: {moduleData?.progress?.progress}
        </Typography>

        <Typography variant="body1" sx={{ mt: 1 }}>
          Status: {moduleData?.progress?.status}
        </Typography>

        {moduleData?.progress?.completionDate && (
          <Typography variant="body2" color="textSecondary" sx={{ mt: 1 }}>
            Completed on: {new Date(moduleData.progress.completionDate).toLocaleDateString()}
          </Typography>
        )}

        <Box sx={{ mt: 4 }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => navigate('/modules')}
            sx={{ mr: 2 }}
          >
            Back to Modules
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default ModuleResultPage;
