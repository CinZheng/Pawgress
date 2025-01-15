import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#652770',
      light: '#ba68c8',
      dark: '#7b1fa2',
      contrastText: '#fff',
    },
    secondary: {
      main: '#dc004e',
    },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: '#652770',
        },
      },
    },
  },
});

export default theme;
