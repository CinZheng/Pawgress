import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, Alert, FormControlLabel, Checkbox } from "@mui/material";
import axiosInstance from "../axios";
import { isAdmin } from "../utils/auth";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";

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
          mt: 8, // Add top margin
          mb: 4, // Add bottom margin
          display: 'flex',
          flexDirection: 'column',
          minHeight: '80vh' // Ensure consistent minimum height
        }}
      >
        <Typography variant="h4" gutterBottom>
          Module: {moduleName}
        </Typography>
        <Typography variant="body1" gutterBottom sx={{ mb: 4 }}>
          {moduleDescription}
        </Typography>

        {/* Module Contents Section */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom>
            Module Contents
          </Typography>
          {orderedItems.map((item, index) => (
            <Box 
              key={item.trainingPathItem.id}
              sx={{ 
                display: 'flex', 
                alignItems: 'center', 
                mb: 1,
                p: 2,
                border: '1px solid #e0e0e0',
                borderRadius: 1
              }}
            >
              <Typography sx={{ flex: 1 }}>
                {index + 1}. {item.details.name || item.details.quizName}
                {' '}
                <Typography component="span" color="textSecondary">
                  ({item.trainingPathItem.type})
                </Typography>
              </Typography>
              {itemProgress?.find(ip => ip.itemId === item.trainingPathItem.id)?.isCompleted && (
                <CheckCircleIcon color="success" sx={{ ml: 1 }} />
              )}
            </Box>
          ))}
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
                bgcolor: '#e0e0e0',
                borderRadius: 5,
                position: 'relative'
              }}>
                <Box sx={{
                  width: `${progress.percentageComplete || 0}%`,
                  height: '100%',
                  bgcolor: 'primary.main',
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
            <Typography variant="h5">
              Lesson: {details.name}
              {itemProgress?.find(ip => ip.itemId === currentItem.trainingPathItem.id)?.isCompleted && (
                <CheckCircleIcon color="success" sx={{ ml: 1 }} />
              )}
            </Typography>
            <Typography variant="body1">{details.text}</Typography>
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
