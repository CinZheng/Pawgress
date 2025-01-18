import React, { useEffect, useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  MenuItem,
  Select,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import { DndProvider, useDrag, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import axiosInstance from "../axios";
import { useSearchParams, useNavigate } from "react-router-dom";
import DeleteIcon from "@mui/icons-material/Delete";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";
import { useNotification } from "../context/NotificationContext";

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
  const moduleId = searchParams.get("id");
  const navigate = useNavigate();
  const [isUserAdmin] = useState(isAdmin());
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const { showNotification } = useNotification();

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    items: [],
  });
  const [quizzes, setQuizzes] = useState([]);
  const [lessons, setLessons] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        let orders = [];
        if (moduleId) {
          try {
            const ordersResponse = await axiosInstance.get(`/api/TrainingPathItemOrder/TrainingPath/${moduleId}`);
            orders = ordersResponse.data;
          } catch (err) {
            if (err.response?.status === 404) {
              console.warn("No items found for this training path.");
            } else {
              throw err;
            }
          }
  
          const moduleResponse = await axiosInstance.get(`/api/TrainingPath/${moduleId}`);
          const module = moduleResponse.data;
  
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
  
        const [quizzesResponse, lessonsResponse] = await Promise.all([
          axiosInstance.get("/api/Quiz"),
          axiosInstance.get("/api/Lesson"),
        ]);
  
        const selectedItemIds = new Set(orders.map((o) => o.trainingPathItem.id));
  
        setQuizzes(quizzesResponse.data.filter((q) => !selectedItemIds.has(q.id)));
        setLessons(lessonsResponse.data.filter((l) => !selectedItemIds.has(l.id)));
      } catch (err) {
        console.error("Error fetching data:", err);
        showNotification("Kon modulegegevens niet ophalen", "error");
      }
    };
  
    fetchData();
  }, [moduleId, showNotification]);

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

    updatedItems.forEach((item, idx) => {
      item.order = idx + 1;
    });

    setFormData((prev) => ({
      ...prev,
      items: updatedItems,
    }));
  };

  const handleSubmit = async () => {
    if (!formData.name.trim()) {
      showNotification("Naam is verplicht", "error");
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
        showNotification("Module succesvol bijgewerkt!", "success");
        navigate("/modules");
      } else {
        await axiosInstance.post("/api/TrainingPath", payload);
        showNotification("Module succesvol aangemaakt!", "success");
        navigate("/modules");
      }
    } catch (err) {
      console.error("Error saving module:", err);
      const errorMessage = err.response?.data?.error || 
                          err.response?.data?.message || 
                          err.response?.data || 
                          err.message || 
                          "Er is een fout opgetreden bij het opslaan van de module.";
      showNotification(typeof errorMessage === 'string' ? errorMessage : JSON.stringify(errorMessage), "error");
      return;
    }
  };

  const handleDeleteModule = async () => {
    try {
      await axiosInstance.delete(`/api/TrainingPath/${moduleId}`);
      showNotification("Module succesvol verwijderd!", "success");
      navigate("/modules");
    } catch (err) {
      console.error("Error deleting module:", err);
      const errorMessage = err.response?.data?.error || "Er is een fout opgetreden bij het verwijderen van de module";
      showNotification(errorMessage, "error");
    }
    setDeleteDialogOpen(false);
  };

  return (
    <Layout>
      <DndProvider backend={HTML5Backend}>
        <Container maxWidth="md">
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
            <Typography variant="h4" component="h1">
              {moduleId ? "Module Bewerken" : "Nieuwe Module"}
            </Typography>
            {isUserAdmin && moduleId && (
              <Button
                variant="contained"
                color="error"
                startIcon={<DeleteIcon />}
                onClick={() => setDeleteDialogOpen(true)}
              >
                Verwijderen
              </Button>
            )}
          </Box>

          <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
            <TextField
              label="Naam"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
            />
            <TextField
              label="Beschrijving"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              multiline
              rows={4}
            />

            <Box>
              <Typography variant="h6" gutterBottom>
                Items Toevoegen
              </Typography>
              <Box sx={{ display: "flex", gap: 2, mb: 2 }}>
                <Select
                  value=""
                  onChange={(e) => {
                    const selectedQuiz = quizzes.find((q) => q.id === e.target.value);
                    if (selectedQuiz) handleAddItem("quiz", selectedQuiz);
                  }}
                  displayEmpty
                  sx={{ flexGrow: 1 }}
                >
                  <MenuItem value="" disabled>
                    Selecteer een quiz
                  </MenuItem>
                  {quizzes.map((quiz) => (
                    <MenuItem key={quiz.id} value={quiz.id}>
                      {quiz.quizName}
                    </MenuItem>
                  ))}
                </Select>
                <Select
                  value=""
                  onChange={(e) => {
                    const selectedLesson = lessons.find((l) => l.id === e.target.value);
                    if (selectedLesson) handleAddItem("lesson", selectedLesson);
                  }}
                  displayEmpty
                  sx={{ flexGrow: 1 }}
                >
                  <MenuItem value="" disabled>
                    Selecteer een les
                  </MenuItem>
                  {lessons.map((lesson) => (
                    <MenuItem key={lesson.id} value={lesson.id}>
                      {lesson.name}
                    </MenuItem>
                  ))}
                </Select>
              </Box>

              <Typography variant="h6" gutterBottom>
                Module Items
              </Typography>
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
              {moduleId ? "Module Bijwerken" : "Module Aanmaken"}
            </Button>
          </Box>

          {/* Delete Confirmation Dialog */}
          <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
            <DialogTitle>Module Verwijderen</DialogTitle>
            <DialogContent>
              <Typography>
                Weet je zeker dat je deze module wilt verwijderen? Dit kan niet ongedaan worden gemaakt.
              </Typography>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
                Annuleren
              </Button>
              <Button onClick={handleDeleteModule} color="error">
                Verwijderen
              </Button>
            </DialogActions>
          </Dialog>
        </Container>
      </DndProvider>
    </Layout>
  );
};

export default ModuleEditorPage;
