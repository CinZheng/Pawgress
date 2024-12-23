import React from "react";
import { AppBar, Toolbar, Typography, BottomNavigation, BottomNavigationAction } from "@mui/material";
import { useMediaQuery } from "@mui/material";
import { Link, useLocation } from "react-router-dom";
import HomeIcon from "@mui/icons-material/Home";
import PetsIcon from "@mui/icons-material/Pets";
import LibraryBooksIcon from "@mui/icons-material/LibraryBooks";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";

function Navbar() {
  const isMobile = useMediaQuery("(max-width:600px)"); // maakt responsive als het mobviele apparaat is
  const location = useLocation();

  const navigationItems = [
    { label: "Modules", icon: <HomeIcon />, path: "/modules" },
    { label: "Honden", icon: <PetsIcon />, path: "/dogs" },
    { label: "Bibliotheek", icon: <LibraryBooksIcon />, path: "/library" },
    { label: "Profiel", icon: <AccountCircleIcon />, path: "/profile" },
  ];

  if (isMobile) {
    // Mobiele navigatie
    return (
      <BottomNavigation
        value={location.pathname}
        showLabels
        style={{ position: "fixed", bottom: 0, width: "100%" }}
      >
        {navigationItems.map((item) => (
          <BottomNavigationAction
            key={item.label}
            label={item.label}
            icon={item.icon}
            component={Link}
            to={item.path}
            value={item.path}
          />
        ))}
      </BottomNavigation>
    );
  }

  // Desktop navigatie
  return (
    <AppBar position="static">
      <Toolbar>
        {navigationItems.map((item) => (
          <Typography
            key={item.label}
            variant="h6"
            component={Link}
            to={item.path}
            style={{ color: "white", textDecoration: "none", marginRight: 20 }}
          >
            {item.label}
          </Typography>
        ))}
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
