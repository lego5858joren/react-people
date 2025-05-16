import { useEffect, useState } from "react";
import axios from "../api/axios";
import { useNavigate } from "react-router-dom";


function AdminPage() {
  const [users, setUsers] = useState([]);
  const [people, setPeople] = useState([]);
  const [newUser, setNewUser] = useState({ username: "", password: "" });
  const [newPerson, setNewPerson] = useState({ firstName: "", age: "", affiliation: "" });

  const fetchUsers = async () => {
    const res = await axios.get("/user");
    setUsers(res.data.map(u => ({ ...u, newPassword: "" })));
  };

  const fetchPeople = async () => {
    const res = await axios.get("/people");
    setPeople(res.data);
  };

  useEffect(() => {
    fetchUsers();
    fetchPeople();
  }, []);

  const handleAddUser = async () => {
    await axios.post("/user", newUser);
    setNewUser({ username: "", password: "" });
    fetchUsers();
  };

  const handleDeleteUser = async (id) => {
    await axios.delete(`/user/${id}`);
    fetchUsers();
  };

  const handleUpdateUser = async (id, newPassword) => {
    await axios.put(`/user/${id}`, newPassword, {
      headers: { "Content-Type": "application/json" }
    });
    fetchUsers();
  };

  const handleAddPerson = async () => {
    await axios.post("/people", newPerson);
    setNewPerson({ firstName: "", age: "", affiliation: "" });
    fetchPeople();
  };

  const handleDeletePerson = async (id) => {
    await axios.delete(`/people/${id}`);
    fetchPeople();
  };

  const handleUpdatePerson = async (person) => {
    await axios.put(`/people/${person.id}`, person);
    fetchPeople();
  };

  const navigate = useNavigate();

  return (
    <div style={{ padding: "2rem", fontFamily: "Arial, sans-serif", maxWidth: "700px", margin: "auto" }}>
      <h2>🛠️ Admin Dashboard</h2>

           <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "1rem" }}>
  <span>
    Logged in as <strong>{localStorage.getItem("loggedInUser")}</strong>  
  </span>
  <button
    onClick={() => {
      localStorage.removeItem("loggedInUser");
      window.location.href = "/";
    }}
    style={{ background: "#ccc", border: "none", padding: "0.5rem" }}
  >
    🚪 Logout
  </button>
</div>


      {/* Users Section */}
      <section style={{ marginBottom: "2rem" }}>
        <h3>👤 Users</h3>
        {users.map((u) => (
          <div key={u.id} style={{ marginBottom: "0.5rem" }}>
            <strong>{u.username}</strong>
            <input
              placeholder="New Password"
              type="password"
              value={u.newPassword}
              onChange={(e) =>
                setUsers(users.map(user =>
                  user.id === u.id ? { ...user, newPassword: e.target.value } : user
                ))
              }
              style={{ marginLeft: "1rem", marginRight: "0.5rem" }}
            />
            <button onClick={() => handleUpdateUser(u.id, u.newPassword)}>Update</button>
            <button onClick={() => handleDeleteUser(u.id)} style={{ marginLeft: "0.5rem" }}>Delete</button>
          </div>
        ))}
        <h4>Add User</h4>
        <input placeholder="Username" value={newUser.username} onChange={(e) => setNewUser({ ...newUser, username: e.target.value })} />
        <input placeholder="Password" type="password" value={newUser.password} onChange={(e) => setNewUser({ ...newUser, password: e.target.value })} />
        <button onClick={handleAddUser}>Add User</button>
      </section>

      <hr />

      {/* People Section */}
      <section>
        <h3>👥 People</h3>
        {people.map((p) => (
          <div key={p.id} style={{ marginBottom: "0.5rem" }}>
            <input
              value={p.firstName}
              onChange={(e) => setPeople(people.map(person => person.id === p.id ? { ...person, firstName: e.target.value } : person))}
              style={{ marginRight: "0.5rem" }}
            />
            <input
              type="number"
              value={p.age}
              onChange={(e) => setPeople(people.map(person => person.id === p.id ? { ...person, age: parseInt(e.target.value) } : person))}
              style={{ width: "60px", marginRight: "0.5rem" }}
            />
            <input
              value={p.affiliation}
              onChange={(e) => setPeople(people.map(person => person.id === p.id ? { ...person, affiliation: e.target.value } : person))}
              style={{ marginRight: "0.5rem" }}
            />
            <button onClick={() => handleUpdatePerson(p)}>Update</button>
            <button onClick={() => handleDeletePerson(p.id)} style={{ marginLeft: "0.5rem" }}>Delete</button>
            <button onClick={() => navigate(`/profile/${p.id}`)} style={{ marginLeft: "0.5rem" }}>View Profile</button>
          </div>
        ))}
        <h4>Add Person</h4>
        <input placeholder="First Name" value={newPerson.firstName} onChange={(e) => setNewPerson({ ...newPerson, firstName: e.target.value })} />
        <input placeholder="Age" type="number" value={newPerson.age} onChange={(e) => setNewPerson({ ...newPerson, age: parseInt(e.target.value) })} />
        <input placeholder="Affiliation" value={newPerson.affiliation} onChange={(e) => setNewPerson({ ...newPerson, affiliation: e.target.value })} />
        <button onClick={handleAddPerson}>Add Person</button>
      </section>
    </div>
  );
}

export default AdminPage;


