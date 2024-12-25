import { jwtDecode } from "jwt-decode";

export const isAuthenticated = () => {
  const token = localStorage.getItem("token");
  if (!token) return false;

  try {
    const { exp } = jwtDecode(token);
    const currentTime = Date.now() / 1000;
    return exp > currentTime; //check of token niet is verlopen
  } catch (error) {
    console.error("JWT-token fout:", error);
    return false;
  }
};
