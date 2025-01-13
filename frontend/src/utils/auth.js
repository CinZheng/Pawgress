export const decodeToken = (token) => {
  try {
    // decodeer token met base64-decoder
    const base64Payload = token.split(".")[1];
    const decodedPayload = atob(base64Payload);
    return JSON.parse(decodedPayload);
  } catch (error) {
    console.error("Token decoding fout:", error);
    return null;
  }
};

export const isAuthenticated = () => {
  const token = localStorage.getItem("token");
  if (!token) return false;

  const decodedToken = decodeToken(token);
  if (!decodedToken) return false;

  const currentTime = Date.now() / 1000;
  return decodedToken.exp > currentTime;
};

export const isAdmin = () => {
  console.log('=== Checking Admin Status ===');
  const token = localStorage.getItem("token");
  console.log('Token exists:', !!token);
  
  if (!token) return false;

  const decodedToken = decodeToken(token);
  console.log('Decoded token:', decodedToken);
  
  if (!decodedToken) return false;

  // gebruik sleutel om rol te vinden met behulp van URI 
  const roleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
  const role = decodedToken[roleKey];
  console.log('User role:', role);
  
  const isAdminResult = role === "admin";
  console.log('Is admin result:', isAdminResult);
  
  return isAdminResult;
};
