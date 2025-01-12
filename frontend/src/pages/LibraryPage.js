import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import {
  Grid,
  Card,
  CardContent,
  Typography,
  Button,
  TextField,
  Checkbox,
  FormControlLabel,
  FormGroup,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";

const LibraryPage = () => {
  const [lessons, setLessons] = useState([]);
  const [quizzes, setQuizzes] = useState([]);
  const [filteredLessons, setFilteredLessons] = useState([]);
  const [filteredQuizzes, setFilteredQuizzes] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [lessonTags, setLessonTags] = useState([]);
  const [selectedTags, setSelectedTags] = useState([]);
  const [lessonSort, setLessonSort] = useState(null);
  const [quizSort, setQuizSort] = useState(null);
  const [isUserAdmin, setIsUserAdmin] = useState(false);
  const navigate = useNavigate();

  // Fetch lessons, quizzes, and check if the user is admin
  useEffect(() => {
    const fetchData = async () => {
      try {
        const lessonsResponse = await axiosInstance.get("/api/Lesson", {
          headers: { "Cache-Control": "no-cache" },
        });
        const quizzesResponse = await axiosInstance.get("/api/Quiz", {
          headers: { "Cache-Control": "no-cache" },
        });
    
        console.log("Lessons Response:", lessonsResponse.data);
        console.log("Quizzes Response:", quizzesResponse.data);
    
        setLessons(lessonsResponse.data);
        setQuizzes(quizzesResponse.data);
        setFilteredLessons(lessonsResponse.data);
        setFilteredQuizzes(quizzesResponse.data);

        // Extract unique tags for lesson filters
        const tags = [
          ...new Set(lessonsResponse.data.map((lesson) => lesson.tag)),
        ].filter(Boolean);
        setLessonTags(tags);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
    setIsUserAdmin(isAdmin());
  }, []);

  // Filter lessons based on search query and selected tags/sort
  useEffect(() => {
    const lowerCaseQuery = searchQuery?.toLowerCase() || ""; // Safeguard for null or undefined

    let resultLessons = lessons.filter((lesson) =>
      lesson.name?.toLowerCase().includes(lowerCaseQuery) // Safeguard for null lesson.name
    );

    // Apply tag filter
    if (selectedTags.length > 0) {
      resultLessons = resultLessons.filter((lesson) =>
        selectedTags.includes(lesson.tag)
      );
    }

    // Apply sorting
    if (lessonSort === "alphabetical") {
      resultLessons.sort((a, b) => a.name?.localeCompare(b.name)); // Safeguard for null lesson.name
    } else if (lessonSort === "creationDate") {
      resultLessons.sort(
        (a, b) => new Date(a.creationDate) - new Date(b.creationDate)
      );
    }

    setFilteredLessons(resultLessons);
  }, [searchQuery, lessons, selectedTags, lessonSort]);

  // Filter quizzes based on search query and selected sort
  useEffect(() => {
    const lowerCaseQuery = searchQuery?.toLowerCase() || ""; // Safeguard for null or undefined

    let resultQuizzes = quizzes.filter((quiz) =>
      quiz.quizName?.toLowerCase().includes(lowerCaseQuery) // Safeguard for null quiz.quizName
    );

    // Apply sorting
    if (quizSort === "alphabetical") {
      resultQuizzes.sort((a, b) => a.Name?.localeCompare(b.Name)); // Safeguard for null quiz.quizName
    } else if (quizSort === "creationDate") {
      resultQuizzes.sort(
        (a, b) => new Date(a.creationDate) - new Date(b.creationDate)
      );
    }

    setFilteredQuizzes(resultQuizzes);
  }, [searchQuery, quizzes, quizSort]);

  // Handle tag selection
  const handleTagChange = (tag) => {
    setSelectedTags((prev) =>
      prev.includes(tag) ? prev.filter((t) => t !== tag) : [...prev, tag]
    );
  };

  return (
    <Layout>
      <Grid container spacing={2} padding={2}>
        {/* Admin buttons */}
        {isUserAdmin && (
          <Grid item xs={12} style={{ marginBottom: "20px" }}>
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate("/quiz-editor")}
              style={{ marginRight: "10px" }}
            >
              Nieuwe Quiz Aanmaken
            </Button>
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate("/lesson-editor")}
            >
              Nieuwe Les Aanmaken
            </Button>
          </Grid>
        )}

        {/* Search input */}
        <Grid item xs={12} style={{ marginBottom: "20px" }}>
          <TextField
            label="Zoek op titel"
            variant="outlined"
            fullWidth
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)} // Update search query dynamically
          />
        </Grid>

        {/* Lesson Filters */}
        <Grid item xs={12}>
          <Accordion>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="lesson-filters-content"
              id="lesson-filters-header"
            >
              <Typography variant="h6">Filter lessen</Typography>
            </AccordionSummary>
            <AccordionDetails>
              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle1">Filter op tags</Typography>
                  <FormGroup>
                    {lessonTags.map((tag) => (
                      <FormControlLabel
                        key={tag}
                        control={
                          <Checkbox
                            checked={selectedTags.includes(tag)}
                            onChange={() => handleTagChange(tag)}
                          />
                        }
                        label={tag}
                      />
                    ))}
                  </FormGroup>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle1">Sorteer lessen</Typography>
                  <FormGroup>
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={lessonSort === "alphabetical"}
                          onChange={() =>
                            setLessonSort(
                              lessonSort === "alphabetical"
                                ? null
                                : "alphabetical"
                            )
                          }
                        />
                      }
                      label="Alfabetisch"
                    />
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={lessonSort === "creationDate"}
                          onChange={() =>
                            setLessonSort(
                              lessonSort === "creationDate"
                                ? null
                                : "creationDate"
                            )
                          }
                        />
                      }
                      label="Op aanmaakdatum"
                    />
                  </FormGroup>
                </Grid>
              </Grid>
            </AccordionDetails>
          </Accordion>
        </Grid>

        {/* Lessons */}
        <Grid item xs={12}>
          <Typography variant="h4" gutterBottom>
            Lessen
          </Typography>
        </Grid>
        {filteredLessons.map((lesson) => (
          <Grid item xs={12} sm={6} md={4} key={lesson.id}>
            <Card
              onClick={() => navigate(`/lessons/${lesson.id}`)}
              style={{ cursor: "pointer" }}
            >
              <CardContent>
                <Typography variant="h6">{lesson.name}</Typography>
                <Typography variant="body2">
                  {lesson.text || "Geen beschrijving beschikbaar."}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}

        {/* Quiz Filters */}
        <Grid item xs={12} style={{ marginTop: "20px" }}>
          <Accordion>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="quiz-filters-content"
              id="quiz-filters-header"
            >
              <Typography variant="h6">Sorteer quizzes</Typography>
            </AccordionSummary>
            <AccordionDetails>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={quizSort === "alphabetical"}
                      onChange={() =>
                        setQuizSort(
                          quizSort === "alphabetical" ? null : "alphabetical"
                        )
                      }
                    />
                  }
                  label="Alfabetisch"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={quizSort === "creationDate"}
                      onChange={() =>
                        setQuizSort(
                          quizSort === "creationDate" ? null : "creationDate"
                        )
                      }
                    />
                  }
                  label="Op aanmaakdatum"
                />
              </FormGroup>
            </AccordionDetails>
          </Accordion>
        </Grid>

        {/* Quizzes */}
        <Grid item xs={12} style={{ marginTop: "20px" }}>
          <Typography variant="h4" gutterBottom>
            Quizzes
          </Typography>
        </Grid>
        {filteredQuizzes.map((quiz) => (
          <Grid item xs={12} sm={6} md={4} key={quiz.id}>
            <Card
              onClick={() => navigate(`/quizzes/${quiz.id}`)}
              style={{ cursor: "pointer" }}
            >
              <CardContent>
                <Typography variant="h6">{quiz.quizName}</Typography>
                <Typography variant="body2">
                  {quiz.quizDescription || "Geen beschrijving beschikbaar."}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Layout>
  );
};

export default LibraryPage;
