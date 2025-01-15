import React from "react";
import { useMediaQuery, Box } from "@mui/material";

const Layout = ({ children }) => {
  const isMobile = useMediaQuery("(max-width:600px)");

  return (
    <Box 
      component="main"
      sx={{ 
        paddingBottom: isMobile ? "72px" : "24px",
        minHeight: "100vh",
        paddingTop: { xs: "24px", sm: "32px" },
        paddingX: { xs: "16px", sm: "24px" },
        backgroundColor: "#f5f5f5",
        '& .MuiTypography-h4': {
          marginTop: { xs: "16px", sm: "24px" },
          marginBottom: { xs: "16px", sm: "24px" },
          fontSize: { xs: "1.75rem", sm: "2rem" },
          fontWeight: 500
        },
        '& .MuiContainer-root': {
          paddingLeft: { xs: 0, sm: "24px" },
          paddingRight: { xs: 0, sm: "24px" }
        }
      }}
    >
      {children}
    </Box>
  );
};

export default Layout;
