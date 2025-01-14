import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import {
  Grid,
  Card,
  CardContent,
  Typography,
  Button,
  TextField,
  Container,
  Box,
  FormControl,
  Select,
  MenuItem,
  Chip,
  CardActions,
  Fab,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { isAdmin } from "../utils/auth";
import Layout from "../components/Layout";
import AddIcon from "@mui/icons-material/Add";

const LibraryPage = () => {
  const navigate = useNavigate();
  const [lessons, setLessons] = useState([]);
  const [quizzes, setQuizzes] = useState([]);
  const [filteredLessons, setFilteredLessons] = useState([]);
  const [filteredQuizzes, setFilteredQuizzes] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [lessonTags, setLessonTags] = useState([]);
  const [selectedTags, setSelectedTags] = useState([]);
  const [lessonSort, setLessonSort] = useState("name-asc");
  const [quizSort, setQuizSort] = useState("name-asc");
  const isUserAdmin = isAdmin();

  useEffect(() => {
    const fetchContent = async () => {
      try {
        const [lessonsResponse, quizzesResponse] = await Promise.all([
          axiosInstance.get("/api/Lesson"),
          axiosInstance.get("/api/Quiz")
        ]);
        setLessons(lessonsResponse.data);
        setQuizzes(quizzesResponse.data);
        
        // Extract unique tags from lessons
        const uniqueTags = [...new Set(lessonsResponse.data
          .map(lesson => lesson.tag)
          .filter(Boolean))];
        setLessonTags(uniqueTags);
      } catch (error) {
        console.error("Error fetching content:", error);
      }
    };

    fetchContent();
  }, []);

  useEffect(() => {
    // Filter and sort lessons
    let filtered = lessons.filter(lesson => {
      const matchesSearch = (lesson.name || '').toLowerCase().includes(searchQuery.toLowerCase()) ||
        (lesson.text || '').toLowerCase().includes(searchQuery.toLowerCase());
      const matchesTags = selectedTags.length === 0 || (lesson.tag && selectedTags.includes(lesson.tag));
      return matchesSearch && matchesTags;
    });

    filtered.sort((a, b) => {
      switch (lessonSort) {
        case "name-asc":
          return a.name.localeCompare(b.name);
        case "name-desc":
          return b.name.localeCompare(a.name);
        case "date-asc":
          return new Date(a.creationDate) - new Date(b.creationDate);
        case "date-desc":
          return new Date(b.creationDate) - new Date(a.creationDate);
        default:
          return 0;
      }
    });

    setFilteredLessons(filtered);
  }, [lessons, searchQuery, selectedTags, lessonSort]);

  useEffect(() => {
    // Filter and sort quizzes
    let filtered = quizzes.filter(quiz => {
      const matchesSearch = (quiz.quizName || '').toLowerCase().includes(searchQuery.toLowerCase()) ||
        (quiz.quizDescription || '').toLowerCase().includes(searchQuery.toLowerCase());
      return matchesSearch;
    });

    filtered.sort((a, b) => {
      switch (quizSort) {
        case "name-asc":
          return a.quizName.localeCompare(b.quizName);
        case "name-desc":
          return b.quizName.localeCompare(a.quizName);
        case "date-asc":
          return new Date(a.creationDate) - new Date(b.creationDate);
        case "date-desc":
          return new Date(b.creationDate) - new Date(a.creationDate);
        default:
          return 0;
      }
    });

    setFilteredQuizzes(filtered);
  }, [quizzes, searchQuery, quizSort]);

  const handleTagChange = (event) => {
    setSelectedTags(event.target.value);
  };

  const handleLessonSortChange = (event) => {
    setLessonSort(event.target.value);
  };

  const handleQuizSortChange = (event) => {
    setQuizSort(event.target.value);
  };

  return (
    <Layout>
      <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
        {/* Title */}
        <Typography 
          variant="h4" 
          sx={{ 
            color: '#663366',
            fontWeight: 'bold',
            mb: 4 
          }}
        >
          Bibliotheek
        </Typography>

        {/* Search and Filter Section */}
        <Box sx={{ mb: 4 }}>
          <TextField
            fullWidth
            variant="outlined"
            placeholder="Zoeken..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            sx={{
              mb: 2,
              '& .MuiOutlinedInput-root': {
                borderRadius: '12px',
                '& fieldset': {
                  borderColor: '#e0e0e0',
                },
                '&:hover fieldset': {
                  borderColor: '#663366',
                },
                '&.Mui-focused fieldset': {
                  borderColor: '#663366',
                }
              }
            }}
          />
          <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
            <FormControl sx={{ minWidth: 120 }}>
              <Select
                multiple
                value={selectedTags}
                onChange={handleTagChange}
                displayEmpty
                renderValue={(selected) => {
                  if (selected.length === 0) {
                    return 'Tags';
                  }
                  return selected.join(', ');
                }}
                sx={{ 
                  borderRadius: '12px',
                  '& .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#e0e0e0',
                  },
                  '&:hover .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#663366',
                  },
                  '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#663366',
                  }
                }}
              >
                {lessonTags.map((tag) => (
                  <MenuItem key={tag} value={tag}>{tag}</MenuItem>
                ))}
              </Select>
            </FormControl>
            <FormControl sx={{ minWidth: 150 }}>
              <Select
                value={lessonSort}
                onChange={handleLessonSortChange}
                displayEmpty
                sx={{ 
                  borderRadius: '12px',
                  '& .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#e0e0e0',
                  },
                  '&:hover .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#663366',
                  },
                  '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                    borderColor: '#663366',
                  }
                }}
              >
                <MenuItem value="name-asc">Naam (A-Z)</MenuItem>
                <MenuItem value="name-desc">Naam (Z-A)</MenuItem>
                <MenuItem value="date-asc">Datum (Oud-Nieuw)</MenuItem>
                <MenuItem value="date-desc">Datum (Nieuw-Oud)</MenuItem>
              </Select>
            </FormControl>
          </Box>
        </Box>

        {/* Lessons Section */}
        <Typography 
          variant="h5" 
          sx={{ 
            color: '#663366',
            fontWeight: 'medium',
            mb: 2 
          }}
        >
          Lessen
        </Typography>
        <Grid container spacing={3} sx={{ mb: 4 }}>
          {filteredLessons.map((lesson) => (
            <Grid item xs={12} sm={6} md={4} key={lesson.id}>
              <Card 
                sx={{
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  borderRadius: '12px',
                  transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
                  '&:hover': {
                    transform: 'scale(1.02)',
                    boxShadow: 3,
                  }
                }}
              >
                <CardContent sx={{ flexGrow: 1, p: 3 }}>
                  <Typography 
                    variant="h6" 
                    component="h2" 
                    gutterBottom
                    sx={{ 
                      color: '#663366',
                      fontWeight: 'medium'
                    }}
                  >
                    {lesson.name}
                  </Typography>
                  <Typography 
                    variant="body2" 
                    color="text.secondary"
                    sx={{
                      mb: 2,
                      overflow: 'hidden',
                      textOverflow: 'ellipsis',
                      display: '-webkit-box',
                      WebkitLineClamp: 2,
                      WebkitBoxOrient: 'vertical',
                      fontStyle: 'italic'
                    }}
                  >
                    {lesson.text || 'Geen beschrijving beschikbaar'}
                  </Typography>
                  {lesson.tag && (
                    <Chip 
                      label={lesson.tag}
                      size="small"
                      sx={{ 
                        backgroundColor: '#f0f0f0',
                        borderRadius: '8px',
                        '& .MuiChip-label': {
                          color: '#663366'
                        }
                      }}
                    />
                  )}
                </CardContent>
                <CardActions sx={{ p: 2, pt: 0 }}>
                  <Button
                    size="small"
                    onClick={() => navigate(`/lessons/${lesson.id}`)}
                    sx={{
                      color: '#663366',
                      '&:hover': {
                        backgroundColor: 'rgba(102, 51, 102, 0.04)'
                      }
                    }}
                  >
                    Bekijk Les
                  </Button>
                  {isUserAdmin && (
                    <Button
                      size="small"
                      onClick={() => navigate(`/lesson-editor?id=${lesson.id}`)}
                      sx={{
                        color: '#663366',
                        '&:hover': {
                          backgroundColor: 'rgba(102, 51, 102, 0.04)'
                        }
                      }}
                    >
                      Bewerken
                    </Button>
                  )}
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* Quizzes Section */}
        <Typography 
          variant="h5" 
          sx={{ 
            color: '#663366',
            fontWeight: 'medium',
            mb: 2 
          }}
        >
          Quizzes
        </Typography>
        <Box sx={{ mb: 2 }}>
          <FormControl sx={{ minWidth: 150 }}>
            <Select
              value={quizSort}
              onChange={handleQuizSortChange}
              displayEmpty
              sx={{ 
                borderRadius: '12px',
                '& .MuiOutlinedInput-notchedOutline': {
                  borderColor: '#e0e0e0',
                },
                '&:hover .MuiOutlinedInput-notchedOutline': {
                  borderColor: '#663366',
                },
                '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                  borderColor: '#663366',
                }
              }}
            >
              <MenuItem value="name-asc">Naam (A-Z)</MenuItem>
              <MenuItem value="name-desc">Naam (Z-A)</MenuItem>
              <MenuItem value="date-asc">Datum (Oud-Nieuw)</MenuItem>
              <MenuItem value="date-desc">Datum (Nieuw-Oud)</MenuItem>
            </Select>
          </FormControl>
        </Box>
        <Grid container spacing={3}>
          {filteredQuizzes.map((quiz) => (
            <Grid item xs={12} sm={6} md={4} key={quiz.id}>
              <Card 
                sx={{
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  borderRadius: '12px',
                  transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
                  '&:hover': {
                    transform: 'scale(1.02)',
                    boxShadow: 3,
                  }
                }}
              >
                <CardContent sx={{ flexGrow: 1, p: 3 }}>
                  <Typography 
                    variant="h6" 
                    component="h2" 
                    gutterBottom
                    sx={{ 
                      color: '#663366',
                      fontWeight: 'medium'
                    }}
                  >
                    {quiz.quizName}
                  </Typography>
                  <Typography 
                    variant="body2" 
                    color="text.secondary"
                    sx={{
                      mb: 2,
                      overflow: 'hidden',
                      textOverflow: 'ellipsis',
                      display: '-webkit-box',
                      WebkitLineClamp: 2,
                      WebkitBoxOrient: 'vertical',
                      fontStyle: 'italic'
                    }}
                  >
                    {quiz.quizDescription || 'Geen beschrijving beschikbaar'}
                  </Typography>
                  <Chip 
                    label="Quiz"
                    size="small"
                    sx={{ 
                      backgroundColor: '#e6f3ff',
                      borderRadius: '8px',
                      '& .MuiChip-label': {
                        color: '#0066cc'
                      }
                    }}
                  />
                </CardContent>
                <CardActions sx={{ p: 2, pt: 0 }}>
                  <Button
                    size="small"
                    onClick={() => navigate(`/quizzes/${quiz.id}`)}
                    sx={{
                      color: '#663366',
                      '&:hover': {
                        backgroundColor: 'rgba(102, 51, 102, 0.04)'
                      }
                    }}
                  >
                    Bekijk Quiz
                  </Button>
                  {isUserAdmin && (
                    <Button
                      size="small"
                      onClick={() => navigate(`/quiz-editor?id=${quiz.id}`)}
                      sx={{
                        color: '#663366',
                        '&:hover': {
                          backgroundColor: 'rgba(102, 51, 102, 0.04)'
                        }
                      }}
                    >
                      Bewerken
                    </Button>
                  )}
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* Add Buttons for Admins */}
        {isUserAdmin && (
          <Box sx={{ position: 'fixed', bottom: 16, right: 16, display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Fab
              color="primary"
              aria-label="add lesson"
              onClick={() => navigate('/lesson-editor')}
              sx={{
                bgcolor: '#663366',
                '&:hover': {
                  bgcolor: '#4a2649'
                }
              }}
            >
              <AddIcon />
            </Fab>
            <Fab
              color="primary"
              aria-label="add quiz"
              onClick={() => navigate('/quiz-editor')}
              sx={{
                bgcolor: '#663366',
                '&:hover': {
                  bgcolor: '#4a2649'
                }
              }}
            >
              <AddIcon />
            </Fab>
          </Box>
        )}
      </Container>
    </Layout>
  );
};

export default LibraryPage;
