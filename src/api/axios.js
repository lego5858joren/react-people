import axios from "axios";

const baseURL = process.env.NODE_ENV === "production"
  ? "https://people-api-fed4affrabfrawen.centralus-01.azurewebsites.net"
  : "http://localhost:7014";

export default axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
});