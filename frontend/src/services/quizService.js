import axiosInstance from './axiosConfig';

export const addQuestion = (quizId, question) =>
  axiosInstance.post(`/api/Quiz/add-question`, { ...question, quizId });

export const getQuestions = (quizId) =>
  axiosInstance.get(`/api/Quiz/${quizId}/questions`);
