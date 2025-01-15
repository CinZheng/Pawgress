import React from "react";
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  BottomNavigation, 
  BottomNavigationAction,
  useMediaQuery,
  Box,
  Button
} from "@mui/material";
import { Link, useLocation, useNavigate } from "react-router-dom";
import HomeIcon from "@mui/icons-material/Home";
import PetsIcon from "@mui/icons-material/Pets";
import LibraryBooksIcon from "@mui/icons-material/LibraryBooks";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import DataSensorIcon from '@mui/icons-material/DataUsage';
import LogoutIcon from '@mui/icons-material/Logout';
import { isAdmin } from "../utils/auth";

function Navbar({ isLoggedIn, onLogout }) {
  const isMobile = useMediaQuery("(max-width:600px)");
  const location = useLocation();
  const navigate = useNavigate();

  const navigationItems = [
    { label: "Modules", icon: <HomeIcon />, path: "/modules" },
    { label: "Honden", icon: <PetsIcon />, path: "/dogprofiles" },
    { label: "Bibliotheek", icon: <LibraryBooksIcon />, path: "/library" },
    { label: "Profiel", icon: <AccountCircleIcon />, path: "/profile" },
    ...(isAdmin() ? [{ label: "Sensordata", icon: <DataSensorIcon />, path: "/sensor-data-form" }] : []),
  ];

  if (!isLoggedIn) return null;

  if (isMobile) {
    return (
      <BottomNavigation
        value={location.pathname}
        showLabels
        sx={{
          position: "fixed",
          bottom: 0,
          left: 0,
          right: 0,
          zIndex: 1000,
          backgroundColor: "white",
          borderTop: "1px solid rgba(0, 0, 0, 0.12)"
        }}
      >
        {navigationItems.map((item) => (
          <BottomNavigationAction
            key={item.path}
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

  return (
    <AppBar position="static" color="primary">
      <Toolbar sx={{ gap: 2 }}>
        <Box sx={{ display: 'flex', gap: 2, flexGrow: 1 }}>
          {navigationItems.map((item) => (
            <Typography
              key={item.path}
              component={Link}
              to={item.path}
              sx={{
                color: "white",
                textDecoration: "none",
                display: "flex",
                alignItems: "center",
                gap: 1
              }}
            >
              {item.icon}
              {item.label}
            </Typography>
          ))}
        </Box>
        <Button
          color="inherit"
          onClick={onLogout}
          startIcon={<LogoutIcon />}
        >
          Uitloggen
        </Button>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
