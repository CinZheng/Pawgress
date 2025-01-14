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

function Navbar({ isLoggedIn }) {
  const isMobile = useMediaQuery("(max-width:600px)");
  const location = useLocation();
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    navigate("/login");
  };

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
          zIndex: 1100,
          backgroundColor: "white",
          borderTop: "1px solid rgba(0, 0, 0, 0.12)",
          height: "64px",
          boxShadow: "0px -2px 4px rgba(0, 0, 0, 0.1)"
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
            sx={{
              minWidth: 0,
              padding: "6px 0",
              '& .MuiBottomNavigationAction-label': {
                fontSize: '0.75rem'
              }
            }}
          />
        ))}
      </BottomNavigation>
    );
  }

  return (
    <AppBar 
      position="fixed" 
      sx={{ 
        backgroundColor: "white",
        color: "text.primary",
        boxShadow: "0px 2px 4px rgba(0, 0, 0, 0.1)"
      }}
    >
      <Toolbar sx={{ minHeight: { xs: "56px", sm: "64px" } }}>
        <Box sx={{ display: 'flex', gap: 3, flexGrow: 1 }}>
          {navigationItems.map((item) => (
            <Typography
              key={item.path}
              component={Link}
              to={item.path}
              sx={{
                color: location.pathname === item.path ? "primary.main" : "text.primary",
                textDecoration: "none",
                display: "flex",
                alignItems: "center",
                gap: 1,
                fontSize: "0.9rem",
                fontWeight: location.pathname === item.path ? 600 : 400,
                '&:hover': {
                  color: "primary.main"
                }
              }}
            >
              {item.icon}
              {item.label}
            </Typography>
          ))}
        </Box>
        <Button
          onClick={handleLogout}
          startIcon={<LogoutIcon />}
          sx={{
            color: "text.primary",
            '&:hover': {
              color: "primary.main"
            }
          }}
        >
          Uitloggen
        </Button>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
