// src/components/Login.tsx
import { useState } from "react";

interface LoginProps {
  onLogin: (username: string, password: string) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onLogin(username, password);
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-sm mx-auto mt-10 p-6 border rounded shadow">
      <h2 className="text-2xl mb-4">Login</h2>
      <div className="mb-4">
        <label className="block text-sm font-medium">Username</label>
        <input
          type="text"
          className="w-full mt-1 p-2 border rounded"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-sm font-medium">Password</label>
        <input
          type="password"
          className="w-full mt-1 p-2 border rounded"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>
      <button
        type="submit"
        className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
      >
        Log In
      </button>
    </form>
  );
};

export default Login;
