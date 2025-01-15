import React from "react";
import { useMediaQuery } from "@mui/material";

const Layout = ({ children }) => {
  const isMobile = useMediaQuery("(max-width:600px)");

  return (
    <div style={{ paddingBottom: isMobile ? 56 : 0, minHeight: "200vh" }}>
      {children}
    </div>
  );
};

export default Layout;
