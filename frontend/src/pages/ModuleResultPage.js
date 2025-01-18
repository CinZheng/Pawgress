import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, CircularProgress, List, ListItem, ListItemText, ListItemIcon } from "@mui/material";
import axiosInstance from "../axios";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import Layout from "../components/Layout";

const ModuleResultPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [moduleData, setModuleData] = useState(null);
  const [orderedItems, setOrderedItems] = useState([]);
  const [itemProgress, setItemProgress] = useState([]);
  const [loading, setLoading] = useState(true);
  const userId = localStorage.getItem('userId');

  useEffect(() => {
    const fetchModuleProgress = async () => {
      try {
        const [moduleResponse, progressResponse, ordersResponse] = await Promise.all([
          axiosInstance.get(`/api/TrainingPath/${id}`),
          axiosInstance.get(`/api/TrainingPath/${id}/progress/${userId}`),
          axiosInstance.get(`/api/TrainingPathItemOrder/TrainingPath/${id}`)
        ]);

        setModuleData({
          ...moduleResponse.data,
          progress: progressResponse.data
        });

        if (progressResponse.data) {
          setItemProgress(progressResponse.data.itemProgress || []);
        }

        const orders = ordersResponse.data;
        const itemsDetails = await Promise.all(
          orders.map(async (order) => {
            const { id: itemId, type } = order.trainingPathItem;
            const itemResponse = await axiosInstance.get(
              `/api/${type === "Lesson" ? "Lesson" : "Quiz"}/${itemId}`
            );
            return { ...order, details: itemResponse.data };
          })
        );

        setOrderedItems(itemsDetails.sort((a, b) => a.order - b.order));
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
      <Layout>
        <Container sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
          <CircularProgress />
        </Container>
      </Layout>
    );
  }

  return (
    <Layout>
      <Container maxWidth="md">
        <Box sx={{ textAlign: 'center', mt: 4 }}>
          <Typography variant="h4" gutterBottom>
            Module Voltooid!
          </Typography>
          
          <Typography variant="h5" gutterBottom>
            {moduleData?.name}
          </Typography>

          <Typography variant="body1" sx={{ mt: 2 }}>
            Voortgang: {moduleData?.progress?.percentageComplete}%
          </Typography>

          <Typography variant="body1" sx={{ mt: 1 }}>
            Status: {moduleData?.progress?.status}
          </Typography>

          {moduleData?.progress?.completionDate && (
            <Typography variant="body2" color="textSecondary" sx={{ mt: 1 }}>
              Voltooid op: {new Date(moduleData.progress.completionDate).toLocaleDateString()}
            </Typography>
          )}

          <Box sx={{ mt: 4, mb: 4 }}>
            <Typography variant="h6" gutterBottom>
              Module Inhoud
            </Typography>
            <List>
              {orderedItems.map((item, index) => (
                <ListItem
                  key={item.trainingPathItem.id}
                  sx={{
                    mb: 1,
                    p: 2,
                    border: "1px solid #e0e0e0",
                    borderRadius: 1,
                    display: "flex",
                    alignItems: "center"
                  }}
                >
                  <ListItemText
                    primary={
                      <Typography>
                        {index + 1}. {item.details.name || item.details.quizName}{" "}
                        <Typography component="span" color="textSecondary">
                          ({item.trainingPathItem.type})
                        </Typography>
                      </Typography>
                    }
                  />
                  {itemProgress?.find((ip) => ip.itemId === item.trainingPathItem.id)
                    ?.isCompleted && <CheckCircleIcon color="success" sx={{ ml: 1 }} />}
                </ListItem>
              ))}
            </List>
          </Box>

          <Box sx={{ mt: 4 }}>
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate('/modules')}
              sx={{ mr: 2 }}
            >
              Terug naar Modules
            </Button>
          </Box>
        </Box>
      </Container>
    </Layout>
  );
};

export default ModuleResultPage;
