import axios from "axios";

export default axios.create({
    baseURL: "https://localhost:7014",
    headers: {
        "Content-Type": "application/json",

    }
});