import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Container, Typography, Box, Button, Alert } from "@mui/material";
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

  const userIsAdmin = isAdmin();
  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchModuleDetails = async () => {
      try {
        const moduleResponse = await axiosInstance.get(`/api/TrainingPath/${id}`);
        setModuleName(moduleResponse.data.name);
        setModuleDescription(moduleResponse.data.description);

        if (!userIsAdmin) {
          try {
            const progressResponse = await axiosInstance.get(
              `/api/TrainingPath/${id}/progress/${userId}`
            );
            if (progressResponse.data) {
              setProgress(progressResponse.data);
              setItemProgress(progressResponse.data.itemProgress);
              setIsCompleted(progressResponse.data.status === "Completed");
            }
          } catch {
            setProgress({
              completedItems: 0,
              totalItems: 0,
              percentageComplete: 0,
              status: "Not Started",
            });
            setItemProgress([]);
            setIsCompleted(false);
          }
        }

        try {
          const ordersResponse = await axiosInstance.get(
            `/api/TrainingPathItemOrder/TrainingPath/${id}`
          );
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

          setOrderedItems(
            itemsDetails.sort((a, b) => a.order - b.order)
          );
        } catch {
          setOrderedItems([]);
        }
      } catch (err) {
        setError("Could not fetch module data.");
      }
    };

    fetchModuleDetails();
  }, [id, userId, userIsAdmin]);

  const handleNext = () => {
    if (!currentItem) {
      setError("Error: Could not find current item");
      return;
    }

    if (currentIndex + 1 < orderedItems.length) {
      setCurrentIndex(currentIndex + 1);
    } else {
      navigate(`/modules/${id}/result`);
    }
  };

  const handleBack = () => {
    if (currentIndex > 0) {
      setCurrentIndex(currentIndex - 1);
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

  if (!hasStarted) {
    return (
      <Container maxWidth="md" sx={{ mt: 8, mb: 4, minHeight: "80vh" }}>
        <Typography variant="h4" gutterBottom>
          Module: {moduleName}
        </Typography>
        <Typography variant="body1" gutterBottom>
          {moduleDescription}
        </Typography>

        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom>
            Module Contents
          </Typography>
          {orderedItems.map((item, index) => (
            <Box
              key={item.trainingPathItem.id}
              sx={{
                display: "flex",
                alignItems: "center",
                mb: 1,
                p: 2,
                border: "1px solid #e0e0e0",
                borderRadius: 1,
              }}
            >
              <Typography sx={{ flex: 1 }}>
                {index + 1}. {item.details.name || item.details.quizName}{" "}
                <Typography component="span" color="textSecondary">
                  ({item.trainingPathItem.type})
                </Typography>
              </Typography>
              {itemProgress?.find((ip) => ip.itemId === item.trainingPathItem.id)
                ?.isCompleted && <CheckCircleIcon color="success" sx={{ ml: 1 }} />}
            </Box>
          ))}
        </Box>

        <Box sx={{ display: "flex", gap: 2, justifyContent: "center" }}>
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

  return (
    <Container maxWidth="md" sx={{ mt: 8, mb: 4, minHeight: "80vh" }}>
      <Box sx={{ mb: 4 }}>
        {isLesson ? (
          <Box>
            <Typography variant="h5">
              Lesson: {details.name}
              {itemProgress?.find((ip) => ip.itemId === currentItem.trainingPathItem.id)
                ?.isCompleted && <CheckCircleIcon color="success" sx={{ ml: 1 }} />}
            </Typography>
            <Typography variant="body1">{details.text}</Typography>
          </Box>
        ) : (
          <Box>
            <Typography variant="h5">
              Quiz: {details.quizName}
              {itemProgress?.find((ip) => ip.itemId === currentItem.trainingPathItem.id)
                ?.isCompleted && <CheckCircleIcon color="success" sx={{ ml: 1 }} />}
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

      <Box
        sx={{
          display: "flex",
          flexDirection: { xs: "column", sm: "row" },
          gap: 2,
          justifyContent: { xs: "center", sm: "space-between" },
          alignItems: { xs: "center", sm: "flex-start" },
          mt: 2,
        }}
      >
        <Button
          variant="outlined"
          color="primary"
          onClick={() => setHasStarted(false)}
          startIcon={<ArrowBackIcon />}
          sx={{ width: { xs: "100%", sm: "auto" } }}
        >
          Back to Overview
        </Button>
        <Button
          variant="contained"
          color="primary"
          onClick={handleBack}
          disabled={currentIndex === 0}
          sx={{ width: { xs: "100%", sm: "auto" } }}
        >
          Previous
        </Button>
        {!userIsAdmin && (
          <Button
            variant="contained"
            color="secondary"
            onClick={async () => {
              try {
                const requestData = {
                  trainingPathId: id,
                  score: null,
                };
                const isCompleted = itemProgress?.find(
                  (ip) => ip.itemId === currentItem.trainingPathItem.id
                )?.isCompleted;
                let response;

                if (!isCompleted) {
                  response = await axiosInstance.post(
                    `/api/TrainingPath/items/${currentItem.trainingPathItem.id}/complete/${userId}`,
                    requestData
                  );
                } else {
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
                setError(`Failed to update progress: ${error.message}`);
              }
            }}
            sx={{ width: { xs: "100%", sm: "auto" } }}
          >
            {itemProgress?.find(
              (ip) => ip.itemId === currentItem.trainingPathItem.id
            )?.isCompleted
              ? "Mark as Incomplete"
              : "Mark as Complete"}
          </Button>
        )}
        <Button
          variant="contained"
          color="primary"
          onClick={handleNext}
          sx={{ width: { xs: "100%", sm: "auto" } }}
        >
          {currentIndex + 1 < orderedItems.length ? "Next" : "Back to Overview"}
        </Button>
      </Box>
    </Container>
  );
};

export default ModuleDetailsPage;
