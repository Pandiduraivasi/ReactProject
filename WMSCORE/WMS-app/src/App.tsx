import { useState } from 'react'
import "./App.css";
import { Routes, Route } from "react-router-dom";
import Login from "../src/pages/Login/Login";
import { useSession } from './components/shared/sessionContext';
import './App.css'

function App() {
  const [loginSucceed, setLoginSucceed] = useState<boolean>(false);

  // Use the useSession hook
  const { sessionId } = useSession();
  return (
    <>
      <div>
      <div className="w-full">
        <Routes>
          {!loginSucceed && (
            <Route
              path="/"
              element={<Login  setLoginSucceed={setLoginSucceed} />}
            />
          )}
          {/* {loginSucceed && <Route path="/UserLogin" element={<UserLogin />} />}
          <Route path="/postlogin" element={<PostLogin />} /> */}
        </Routes>
      </div>
      {/* <TopBar/> */}
      {/* <ResetPasswordForm sessionId={sessionId} /> */}
    </div>
    </>
  )
}

export default App
