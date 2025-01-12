import React, { useEffect, useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Alert,
  MenuItem,
  Select,
  IconButton,
} from "@mui/material";
import { DndProvider, useDrag, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import axiosInstance from "../axios";
import { useSearchParams, useNavigate } from "react-router-dom";
import DeleteIcon from "@mui/icons-material/Delete";

const ItemType = "ITEM";

const DraggableItem = ({ item, index, moveItem, handleRemoveItem }) => {
  const [, ref] = useDrag({
    type: ItemType,
    item: { index },
  });

  const [, drop] = useDrop({
    accept: ItemType,
    hover: (draggedItem) => {
      if (draggedItem.index !== index) {
        moveItem(draggedItem.index, index);
        draggedItem.index = index;
      }
    },
  });

  return (
    <div
      ref={(node) => ref(drop(node))}
      style={{
        padding: "8px",
        border: "1px solid #ccc",
        borderRadius: "4px",
        marginBottom: "4px",
        backgroundColor: "#f9f9f9",
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      <Typography>
        {item.type === "quiz" ? item.quizName : item.name} ({item.type === "quiz" ? "Quiz" : "Lesson"})
      </Typography>
      <IconButton onClick={() => handleRemoveItem(index)}>
        <DeleteIcon />
      </IconButton>
    </div>
  );
};

const ModuleEditorPage = () => {
  const [searchParams] = useSearchParams();
  const moduleId = searchParams.get("id"); // Get ID from query params
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    items: [], // Combined list of lessons and quizzes
  });
  const [quizzes, setQuizzes] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        let orders = [];
        if (moduleId) {
          try {
            // Attempt to fetch ordered items
            const ordersResponse = await axiosInstance.get(`/api/TrainingPathItemOrder/TrainingPath/${moduleId}`);
            orders = ordersResponse.data;
          } catch (err) {
            if (err.response?.status === 404) {
              // Treat 404 as no items available
              console.warn("No items found for this training path.");
            } else {
              throw err; // Re-throw for other errors
            }
          }
  
          const moduleResponse = await axiosInstance.get(`/api/TrainingPath/${moduleId}`);
          const module = moduleResponse.data;
  
          // Fetch details for lessons and quizzes in order
          const itemsDetails = await Promise.all(
            orders.map(async (order) => {
              const { id: itemId, type } = order.trainingPathItem;
              const itemResponse = await axiosInstance.get(`/api/${type === "Lesson" ? "Lesson" : "Quiz"}/${itemId}`);
              return {
                ...itemResponse.data,
                type: type.toLowerCase(),
                order: order.order,
              };
            })
          );
  
          setFormData({
            name: module.name,
            description: module.description,
            items: itemsDetails.sort((a, b) => a.order - b.order),
          });
        }
  
        // Fetch available quizzes and lessons for the dropdown
        const [quizzesResponse, lessonsResponse] = await Promise.all([
          axiosInstance.get("/api/Quiz"),
          axiosInstance.get("/api/Lesson"),
        ]);
  
        const selectedItemIds = new Set(orders.map((o) => o.trainingPathItem.id));
  
        setQuizzes(quizzesResponse.data.filter((q) => !selectedItemIds.has(q.id)));
        setLessons(lessonsResponse.data.filter((l) => !selectedItemIds.has(l.id)));
      } catch (err) {
        console.error("Error fetching data:", err);
        setError("Error fetching module data.");
      }
    };
  
    fetchData();
  }, [moduleId]);
  

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleAddItem = (type, item) => {
    setFormData((prev) => ({
      ...prev,
      items: [...prev.items, { ...item, type, order: prev.items.length + 1 }],
    }));

    if (type === "quiz") {
      setQuizzes((prev) => prev.filter((q) => q.id !== item.id));
    } else if (type === "lesson") {
      setLessons((prev) => prev.filter((l) => l.id !== item.id));
    }
  };

  const handleRemoveItem = (index) => {
    const item = formData.items[index];

    setFormData((prev) => ({
      ...prev,
      items: prev.items.filter((_, i) => i !== index),
    }));

    if (item.type === "quiz") {
      setQuizzes((prev) => [...prev, item]);
    } else if (item.type === "lesson") {
      setLessons((prev) => [...prev, item]);
    }
  };

  const moveItem = (fromIndex, toIndex) => {
    const updatedItems = [...formData.items];
    const [movedItem] = updatedItems.splice(fromIndex, 1);
    updatedItems.splice(toIndex, 0, movedItem);

    // Update the order attribute for all items
    updatedItems.forEach((item, idx) => {
      item.order = idx + 1;
    });

    setFormData((prev) => ({
      ...prev,
      items: updatedItems,
    }));
  };

  const handleSubmit = async () => {
    setMessage("");
    setError("");

    if (!formData.name.trim()) {
      setError("Name is required.");
      return;
    }

    try {
      const payload = {
        name: formData.name,
        description: formData.description,
        lessonsQuizzes: formData.items.map((item) => ({
          id: item.id,
          type: item.type,
          order: item.order,
        })),
      };

      if (moduleId) {
        await axiosInstance.put(`/api/TrainingPath/${moduleId}`, payload);
        setMessage("Module successfully updated!");
      } else {
        await axiosInstance.post("/api/TrainingPath", payload);
        setMessage("Module successfully created!");
      }

      navigate("/modules");
    } catch (err) {
      console.error("Error saving module:", err);
      setError("An error occurred while saving the module.");
    }
  };

  return (
    <DndProvider backend={HTML5Backend}>
      <Container maxWidth="md">
        <Typography variant="h4" gutterBottom>
          {moduleId ? "Edit Module" : "Create New Module"}
        </Typography>
        {message && <Alert severity="success">{message}</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
        <Box sx={{ display: "flex", flexDirection: "column", gap: 3, marginTop: 2 }}>
          <TextField
            label="Name"
            name="name"
            variant="outlined"
            value={formData.name}
            onChange={handleInputChange}
          />
          <TextField
            label="Description"
            name="description"
            variant="outlined"
            multiline
            rows={4}
            value={formData.description}
            onChange={handleInputChange}
          />

          <Select
            label="Select a Quiz"
            displayEmpty
            fullWidth
            value=""
            onChange={(e) =>
              handleAddItem("quiz", quizzes.find((q) => q.id === e.target.value))
            }
          >
            <MenuItem value="" disabled>
              Choose a quiz
            </MenuItem>
            {quizzes.map((quiz) => (
              <MenuItem key={quiz.id} value={quiz.id}>
                {quiz.quizName}
              </MenuItem>
            ))}
          </Select>

          <Select
            label="Select a Lesson"
            displayEmpty
            fullWidth
            value=""
            onChange={(e) =>
              handleAddItem(
                "lesson",
                lessons.find((l) => l.id === e.target.value)
              )
            }
          >
            <MenuItem value="" disabled>
              Choose a lesson
            </MenuItem>
            {lessons.map((lesson) => (
              <MenuItem key={lesson.id} value={lesson.id}>
                {lesson.name}
              </MenuItem>
            ))}
          </Select>

          <Typography variant="h6" gutterBottom>
            Selected Items
          </Typography>
          <Box sx={{ border: "1px solid #ccc", borderRadius: "4px", padding: "8px" }}>
            {formData.items.map((item, index) => (
              <DraggableItem
                key={`${item.type}-${item.id}`}
                item={item}
                index={index}
                moveItem={moveItem}
                handleRemoveItem={handleRemoveItem}
              />
            ))}
          </Box>

          <Button variant="contained" color="primary" onClick={handleSubmit}>
            {moduleId ? "Update Module" : "Create Module"}
          </Button>
        </Box>
      </Container>
    </DndProvider>
  );
};

export default ModuleEditorPage;
