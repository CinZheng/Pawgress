import React, { useEffect, useState } from "react";
import axiosInstance from "../axios";
import { useParams } from "react-router-dom";
import { Container, Typography, Table, TableHead, TableRow, TableCell, TableBody } from "@mui/material";

const SensorDataPage = () => {
  const { id } = useParams();
  const [sensorData, setSensorData] = useState([]);

  useEffect(() => {
    const fetchSensorData = async () => {
      try {
        const response = await axiosInstance.get(`/api/DogSensorData/dogprofile/${id}`);
        setSensorData(response.data);
      } catch (error) {
        console.error("Fout bij ophalen sensordata:", error);
      }
    };

    fetchSensorData();
  }, [id]);

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Sensor Data voor Hond
      </Typography>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Sensor Naam</TableCell>
            <TableCell>Beschrijving</TableCell>
            <TableCell>Type</TableCell>
            <TableCell>Eenheid</TableCell>
            <TableCell>Gemiddelde Waarde</TableCell>
            <TableCell>Datum</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {sensorData.map((data) => (
            <TableRow key={data.dogSensorDataId}>
              <TableCell>{data.name}</TableCell>
              <TableCell>{data.description}</TableCell>
              <TableCell>{data.sensorType}</TableCell>
              <TableCell>{data.unit}</TableCell>
              <TableCell>{data.averageValue}</TableCell>
              <TableCell>{new Date(data.recordedDate).toLocaleString()}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Container>
  );
};

export default SensorDataPage;
