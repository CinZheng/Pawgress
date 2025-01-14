import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, Alert, FormControlLabel, Checkbox, Paper, Card, CardContent } from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { marked } from "marked";
import RadioButtonUncheckedIcon from '@mui/icons-material/RadioButtonUnchecked';

const ModuleDetailsPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [moduleName, setModuleName] = useState("");
  const [moduleDescription, setModuleDescription] = useState("");
  const [orderedItems, setOrderedItems] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [progress, setProgress] = useState(null);
  const [itemProgress, setItemProgress] = useState([]);
  const [error, setError] = useState("");
  const [hasStarted, setHasStarted] = useState(false);
  const [isCompleted, setIsCompleted] = useState(false);
  const [progressPercentage, setProgressPercentage] = useState(0);

  const userIsAdmin = isAdmin(); // Check if the user is an admin
  const userId = localStorage.getItem("userId"); // Replace with your user ID retrieval logic

  useEffect(() => {
    const fetchModuleDetails = async () => {
      try {
        // Fetch module details (name and description)
        const moduleResponse = await axiosInstance.get(`/api/TrainingPath/${id}`);
        setModuleName(moduleResponse.data.name);
        setModuleDescription(moduleResponse.data.description);

        if (!userIsAdmin) {
          try {
            // Fetch progress
            const progressResponse = await axiosInstance.get(
              `/api/TrainingPath/${id}/progress/${userId}`
            );
            
            if (progressResponse.data) {
              setProgress(progressResponse.data);
              setItemProgress(progressResponse.data.itemProgress);
              setIsCompleted(progressResponse.data.status === "Completed");
            } else {
              setProgress({
                completedItems: 0,
                totalItems: 0,
                percentageComplete: 0,
                status: "Not Started"
              });
              setItemProgress([]);
              setIsCompleted(false);
            }
          } catch (err) {
            console.error('Error fetching progress:', err);
            setProgress({
              completedItems: 0,
              totalItems: 0,
              percentageComplete: 0,
              status: "Not Started"
            });
            setItemProgress([]);
            setIsCompleted(false);
          }
        }

        try {
          // Fetch the ordered items
          const ordersResponse = await axiosInstance.get(
            `/api/TrainingPathItemOrder/TrainingPath/${id}`
          );
          const orders = ordersResponse.data;

          // Fetch details for each training path item
          const itemsDetails = await Promise.all(
            orders.map(async (order) => {
              const { id: itemId, type } = order.trainingPathItem;
              const itemResponse = await axiosInstance.get(
                `/api/${type === "Lesson" ? "Lesson" : "Quiz"}/${itemId}`
              );
              return { ...order, details: itemResponse.data };
            })
          );

          // Sort the items by order and set the state
          setOrderedItems(
            itemsDetails.sort((a, b) => a.order - b.order)
          );
        } catch (err) {
          if (err.response?.status === 404) {
            // Handle 404 as no items found
            setOrderedItems([]);
          } else {
            throw err; // Rethrow other errors
          }
        }
      } catch (err) {
        console.error("Error fetching module details:", err);
        setError("Could not fetch module data.");
      }
    };

    fetchModuleDetails();
  }, [id, userId, userIsAdmin]);

  const handleNext = () => {
    if (!currentItem) {
      console.error('No current item found');
      setError('Error: Could not find current item');
      return;
    }

    // Move to next item if not at the end
    if (currentIndex + 1 < orderedItems.length) {
      setCurrentIndex(currentIndex + 1);
    } else {
      // If we're at the end, go to results page
      navigate(`/modules/${id}/result`);
    }
  };

  const handleBack = async () => {
    if (currentIndex > 0) {
      if (!userIsAdmin) {
        try {
          // Fetch latest progress when going back
          const progressResponse = await axiosInstance.get(
            `/api/TrainingPath/${id}/progress/${userId}`
          );
          
          if (progressResponse.data) {
            setProgress(progressResponse.data);
            setItemProgress(progressResponse.data.itemProgress || []);
          }
        } catch (error) {
          console.error('Error fetching progress:', error.response?.data || error.message);
          setError(`Failed to fetch progress: ${error.response?.data?.error || error.message}`);
        }
      }
      setCurrentIndex(currentIndex - 1);
    }
  };

  const handleItemComplete = async () => {
    if (!userIsAdmin) {
      await handleNext();
    } else {
      setCurrentIndex(currentIndex + 1);
    }
  };

  if (error) {
    return (
      <Container maxWidth="md">
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (orderedItems.length === 0) {
    return (
      <Container maxWidth="md">
        <Box sx={{ mt: 4, mb: 6 }}>
        <Typography variant="h4" gutterBottom>
          Module: {moduleName}
        </Typography>
        <Typography variant="body1" gutterBottom>
          {moduleDescription}
        </Typography>
        <Box
          sx={{
            border: "1px solid #ccc",
            borderRadius: "4px",
            padding: "16px",
            marginTop: "16px",
          }}
        >
          <Typography variant="h6" color="textSecondary">
            No lessons or quizzes yet.
          </Typography>
        </Box>

        {userIsAdmin && (
          <Button
            variant="outlined"
            color="secondary"
            onClick={() => navigate(`/module-editor?id=${id}`)}
            sx={{ marginTop: "16px" }}
          >
            Edit Module
          </Button>
        )}
        </Box>
      </Container>
    );
  }

  const currentItem = orderedItems[currentIndex];
  const { details, trainingPathItem } = currentItem;
  const isLesson = trainingPathItem.type === "Lesson";

  // Overview Page
  if (!hasStarted) {
    return (
      <Container 
        maxWidth="md" 
        sx={{ 
          mt: 4,
          mb: 4,
          display: 'flex',
          flexDirection: 'column',
          minHeight: '80vh'
        }}
      >
        {/* Module Header Card */}
        <Card 
          sx={{
            backgroundColor: '#663366',
            borderRadius: '12px',
            mb: 4
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
              {moduleName}
            </Typography>
            <Typography 
              sx={{ 
                color: 'white',
                opacity: 0.9,
                fontStyle: 'italic'
              }}
            >
              {moduleDescription}
            </Typography>
          </CardContent>
        </Card>

        {/* Module Contents Section */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom>
            Module Contents
          </Typography>
          
          {orderedItems.map((item, index) => {
            const isCompleted = itemProgress?.find(ip => ip.itemId === item.trainingPathItem.id)?.isCompleted;
            const isLesson = item.trainingPathItem.type === "Lesson";
            const backgroundColor = isCompleted ? '#f0f0f0' : 'white';
            const borderColor = isCompleted ? '#663366' : '#e0e0e0';

            const handleItemClick = () => {
              setHasStarted(true);
              setCurrentIndex(index);
            };

            return (
              <Box
                key={item.trainingPathItem.id}
                sx={{
                  mb: 2,
                  '&:last-child': { mb: 0 },
                  cursor: 'pointer'
                }}
                onClick={handleItemClick}
              >
                <Card
                  sx={{
                    backgroundColor,
                    border: `1px solid ${borderColor}`,
                    borderRadius: '12px',
                    mb: 1,
                    '&:hover': {
                      transform: 'scale(1.01)',
                      transition: 'all 0.2s ease-in-out',
                      boxShadow: 2,
                      backgroundColor: isCompleted ? '#e8e8e8' : '#f5f5f5'
                    }
                  }}
                >
                  <CardContent sx={{ p: 2 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                      <Box sx={{ flex: 1, mr: 2 }}>
                        <Typography
                          variant="subtitle1"
                          sx={{
                            color: '#663366',
                            fontWeight: 'medium',
                            mb: 0.5
                          }}
                        >
                          {isLesson ? 'Leiding geven: ' : 'Quiz: '}{item.details.name || item.details.quizName}
                        </Typography>
                        <Typography
                          variant="body2"
                          sx={{
                            color: 'text.secondary',
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                            display: '-webkit-box',
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: 'vertical'
                          }}
                        >
                          {item.details.text || item.details.quizDescription}
                        </Typography>
                      </Box>
                      <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        {isCompleted ? (
                          <CheckCircleIcon sx={{ fontSize: 24, color: '#663366' }} />
                        ) : (
                          <RadioButtonUncheckedIcon sx={{ fontSize: 24, color: '#663366' }} />
                        )}
                      </Box>
                    </Box>
                  </CardContent>
                </Card>
              </Box>
            );
          })}
        </Box>

        {/* Progress Section */}
        {!userIsAdmin && progress && (
          <Box sx={{ mb: 4 }}>
            <Typography variant="h5" gutterBottom>
              Your Progress
            </Typography>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
              <Box sx={{ 
                width: '100%', 
                height: 10, 
                bgcolor: 'rgba(255, 255, 255, 0.2)',
                borderRadius: 5,
                position: 'relative'
              }}>
                <Box sx={{
                  width: `${progress.percentageComplete || 0}%`,
                  height: '100%',
                  bgcolor: '#FFD700',
                  borderRadius: 5,
                  transition: 'width 0.5s ease-in-out'
                }} />
              </Box>
              <Typography variant="body2" color="textSecondary">
                {Math.round(progress.percentageComplete || 0)}%
              </Typography>
            </Box>
            <Typography variant="body2" sx={{ mt: 1 }}>
              {progress.completedItems || 0} of {progress.totalItems || 0} items completed
            </Typography>
            <Typography variant="body2">
              Status: {progress.status}
            </Typography>
            {progress.startDate && (
              <Typography variant="body2">
                Started: {new Date(progress.startDate).toLocaleDateString()}
              </Typography>
            )}
            {progress.completionDate && (
              <Typography variant="body2">
                Completed: {new Date(progress.completionDate).toLocaleDateString()}
              </Typography>
            )}
          </Box>
        )}

        {/* Action Buttons */}
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button
            variant="contained"
            color="primary"
            size="large"
            onClick={() => setHasStarted(true)}
            startIcon={<PlayArrowIcon />}
            sx={{
              backgroundColor: '#663366',
              '&:hover': {
                backgroundColor: '#4a2649'
              }
            }}
          >
            {progress?.status === "In Progress" ? "Continue Module" : "Start Module"}
          </Button>
          {userIsAdmin && (
            <Button
              variant="outlined"
              color="secondary"
              onClick={() => navigate(`/module-editor?id=${id}`)}
            >
              Edit Module
            </Button>
          )}
        </Box>
      </Container>
    );
  }

  // Module Content Page
  return (
    <Container 
      maxWidth="md" 
      sx={{ 
        mt: 8, // Add top margin
        mb: 4, // Add bottom margin
        display: 'flex',
        flexDirection: 'column',
        minHeight: '80vh' // Ensure consistent minimum height
      }}
    >
      {/* Current Item Content */}
      <Box sx={{ mb: 4 }}>
        {isLesson ? (
          <Box>
            <Typography variant="h5" gutterBottom>
              {details.name}
              {itemProgress?.find(ip => ip.itemId === currentItem.trainingPathItem.id)?.isCompleted && (
                <CheckCircleIcon color="success" sx={{ ml: 1 }} />
              )}
            </Typography>
            
            {/* Description */}
            {details.text && (
              <Typography 
                variant="body1" 
                sx={{ 
                  mb: 3,
                  color: 'text.secondary',
                  fontStyle: 'italic'
                }}
              >
                {details.text}
              </Typography>
            )}

            {/* Main Content */}
            <Paper 
              elevation={1} 
              sx={{ 
                p: 3, 
                mb: 3,
                backgroundColor: '#fff',
                '& img': { maxWidth: '100%', height: 'auto' }
              }}
            >
              <div dangerouslySetInnerHTML={{ __html: marked(details.markdownContent || '') }}></div>
              {details.image && (
                <Box sx={{ mt: 2 }}>
                  <img src={details.image} alt={details.name} style={{ maxWidth: '100%' }} />
                </Box>
              )}
              {details.video && (
                <Box sx={{ mt: 2 }}>
                  <iframe
                    src={details.video}
                    title="Lesson Video"
                    style={{ width: '100%', height: '400px' }}
                    frameBorder="0"
                    allowFullScreen
                  />
                </Box>
              )}
            </Paper>
          </Box>
        ) : (
          <Box>
            <Typography variant="h5">
              Quiz: {details.quizName}
              {itemProgress?.find(ip => ip.itemId === currentItem.trainingPathItem.id)?.isCompleted && (
                <CheckCircleIcon color="success" sx={{ ml: 1 }} />
              )}
            </Typography>
            <Typography variant="body1">{details.quizDescription}</Typography>
            <Button
              variant="contained"
              color="secondary"
              onClick={() => navigate(`/quiz/${details.id}?moduleId=${id}`)}
            >
              Start Quiz
            </Button>
          </Box>
        )}
      </Box>

      {/* Navigation Buttons */}
      <Box sx={{ display: "flex", justifyContent: "space-between", marginTop: "16px" }}>
        <Box sx={{ display: "flex", gap: 1 }}>
          <Button
            variant="outlined"
            color="primary"
            onClick={() => setHasStarted(false)}
            startIcon={<ArrowBackIcon />}
          >
            Back to Overview
          </Button>
          <Button
            variant="contained"
            color="primary"
            onClick={handleBack}
            disabled={currentIndex === 0}
          >
            Previous
          </Button>
        </Box>
        {!userIsAdmin && (
          <Button
            variant="contained"
            color="secondary"
            onClick={async () => {
              try {
                const requestData = {
                  trainingPathId: id,
                  score: null
                };
                
                const isCompleted = itemProgress?.find(ip => ip.itemId === currentItem.trainingPathItem.id)?.isCompleted;
                let response;
                
                if (!isCompleted) {
                  // Complete the item
                  response = await axiosInstance.post(
                    `/api/TrainingPath/items/${currentItem.trainingPathItem.id}/complete/${userId}`,
                    requestData
                  );
                } else {
                  // Uncomplete the item
                  response = await axiosInstance.delete(
                    `/api/TrainingPath/items/${currentItem.trainingPathItem.id}/complete/${userId}`,
                    { data: requestData }
                  );
                }

                if (response.data) {
                  setProgress(response.data);
                  setItemProgress(response.data.itemProgress || []);
                }
              } catch (error) {
                console.error('Error updating progress:', error.response?.data || error.message);
                setError(`Failed to update progress: ${error.response?.data?.error || error.message}`);
              }
            }}
          >
            {itemProgress?.find(ip => ip.itemId === currentItem.trainingPathItem.id)?.isCompleted 
              ? "Mark as Incomplete" 
              : "Mark as Complete"}
          </Button>
        )}
        <Button
          variant="contained"
          color="primary"
          onClick={handleNext}
        >
          {currentIndex + 1 < orderedItems.length ? "Next" : "Back to Overview"}
        </Button>
      </Box>
    </Container>
  );
};

export default ModuleDetailsPage;
