import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "../api/axios";

function ProfilePage() {
  const { id } = useParams();
  const [person, setPerson] = useState(null);

  useEffect(() => {
    axios.get(`/people/${id}`)
      .then(res => setPerson(res.data))
      .catch(err => console.error("Error loading profile:", err));
  }, [id]);

  if (!person) return <p>Loading profile...</p>;

  return (
    <div style={{ padding: "2rem", fontFamily: "Arial" }}>
      <h2>{person.firstName}'s Profile</h2>
      {person.imageUrl && <img src={person.imageUrl} alt="Profile" style={{
         width: "200px", 
         marginBottom: "1rem", 
         height: "200px",
         objectFit:"cover",
         borderRadius:"10px",
         border: "1px solid #ccc",
         marginBottom: "1rem"}} />}
      <p><strong>Age:</strong> {person.age}</p>
      <p><strong>Affiliation:</strong> {person.affiliation}</p>
      <p><strong>Bio:</strong> {person.bio}</p>
    </div>
  );
}

export default ProfilePage;
