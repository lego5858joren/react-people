import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "../api/axios";

function LoginPage({ setUser }) { // accept setUser here
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      const response = await axios.post("/auth/login", {
        username,
        password
      });

      console.log("✅ Login success:", response.data);
      localStorage.setItem("loggedInUser", username);
      setUser(username); // update React state

      if (username === "admin") {
        navigate("/admin");
      } else {
        navigate("/user");
      }
    } catch (err) {
      console.error("❌ Login failed:", err.response?.data || err.message);
      alert("Login failed: Invalid username or password");
    }
  };

  return (
    <div style={{ padding: "1rem" }}>
      <h2>Login</h2>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <br />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <br />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
}

export default LoginPage;
