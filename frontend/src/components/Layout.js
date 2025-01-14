import React from "react";
import { Box, useMediaQuery } from "@mui/material";

function Layout({ children }) {
  const isMobile = useMediaQuery("(max-width:600px)");

  return (
    <Box
      sx={{
        minHeight: "100vh",
        backgroundColor: "#f5f5f5",
        pt: { xs: "56px", sm: "64px" },
        pb: isMobile ? "64px" : "24px",
        px: { xs: 2, sm: 3, md: 4 },
      }}
    >
      {children}
    </Box>
  );
}

export default Layout;
