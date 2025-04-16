import { useState,useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {Button} from "../../components/ui/button";
import ApiService from "../../services/ApiService";
import { useSession } from '../../components/shared/sessionContext';
import axios from 'axios';

interface LoginFormProps {
    setLoginSucceed: (value: boolean) => void;
}
interface TerminalDetails {
  SessionId: string;
  terminalId: string;
  terminalArea: string;
  versionNo: string;
  environmentName: string;
  infoMessage?: string;
  ipAddress: string;
}
export default function LoginForm({ setLoginSucceed }: LoginFormProps) {
  const { sessionId, setSessionId } = useSession();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
  const [showPassword] = useState(false);
    const navigate = useNavigate();
    const [error,setError] = useState("");
    const [terminalandarea, setTerminal] = useState("TERMINAL : / ");
    const [currVersionanddatabase, setCurrVersion] = useState("VERSION : 2019-01-02 / ");
    const apiService = ApiService(sessionId);
    useEffect(() => {
      const fetchConfig = async () => {
        try {
          const response = await apiService.get<TerminalDetails>("login/GetTerminalDetails");
          setSessionId(response.SessionId);
          setTerminal('TERMINAL : '+ response.terminalId + ' / ' + response.terminalArea);
          setCurrVersion('VERSION : '+ response.versionNo + ' / ' + response.environmentName);
  
        } catch (err) {
          console.error("Failed to fetch configuration:", err);
        }
      };
      fetchConfig();
    }, []);
    const handleLogin = async (e: React.FormEvent) =>  {
        e.preventDefault();
        if (!validateCredentials()) {
            return false;          
        }
        try {
            const response = await axios.post('http://localhost:7100/api/Login/Login', {
                Username: username.trim(),
                Password: password.trim()
            },
                {
                    headers: { 'Content-Type': 'application/json' }
                });            
            console.log(response);
            const sr = response.data.success;
            console.log(sr);
            if (response.data.success) {
                // Redirect to dashboard or handle success
                setLoginSucceed(true);
                localStorage.setItem('isAuthenticated', 'true');
                navigate("/"); // Redirect to the dashboard after login
            } else {
                setError(response.data.message || 'Invalid User ID or Password');
            }
            
        }
        catch (error) {
            console.error("Error details:", error);
            setError("Login failed.");
        }
        //return true;
  };
    const validateCredentials = () => {
        if (!username.trim()) {
            setError("User Name is required.");
            return false;
        }
        if (!password.trim()) {
            setError("Password is required.");
            return false;
        }
        setError("");
        return true;
    };
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 overflow-y-hidden">
      <div className="max-w-md w-full space-y-8 p-10 bg-white rounded-xl shadow-lg">
        <div className="text-center">
          <div className="flex justify-center">
            {/* Replace with your logo */}
            <div className="text-3xl font-bold text-indigo-600">Login</div>
          </div>
        </div>

        <form className="mt-8 space-y-6">
          <div className="rounded-md shadow-sm space-y-4">
            <div className="relative">
              <input
                type="text"
                id="floating_outlined"
                className="block px-2.5 pb-2.5 pt-4 w-full text-sm text-gray-900 bg-transparent rounded-lg border border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-blue-500 focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                placeholder=" " value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
              <label
                htmlFor="floating_outlined"
                className="absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-[0] bg-white dark:bg-gray-900 px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-focus:dark:text-blue-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-1">
                User Name
              </label>
            </div>
            <div className="relative">
              <input
                type={showPassword ? "text" : "password"}
                id="floating_outlined1"
                className="block px-2.5 pb-2.5 pt-4 w-full text-sm text-gray-900 bg-transparent rounded-lg border border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-blue-500 focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                placeholder=" " value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <label
                htmlFor="floating_outlined1"
                className="absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-[0] bg-white dark:bg-gray-900 px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-focus:dark:text-blue-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-1">
                Password
              </label>
            </div>
          </div>
          {/* Error Message */}
          {error && (
            <div className="text-red-500 text-sm text-center">
              {error}
            </div>
          )}
          <div>
            <Button color="rose" className="w-full" onClick={handleLogin}>
              Login
            </Button>
          </div>
          <div className="text-red-600 text-sm text-center">
            {terminalandarea} <br/>   
            {currVersionanddatabase}          
          </div>
        </form>
      </div>
    </div>

  );
}
