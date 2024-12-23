import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:5232",
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${localStorage.getItem("token")}`, // voeg JWT-token toe als nodig
  },
});

export default axiosInstance;
