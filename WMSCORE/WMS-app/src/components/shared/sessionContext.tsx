import React, { createContext, useContext, useState, ReactNode } from "react";

// Define a type for the session context
interface SessionContextType {
  sessionId: string;
  setSessionId: React.Dispatch<React.SetStateAction<string>>;
}

// Create the context with an explicit default value (can be `undefined` or an empty object)
const SessionContext = createContext<SessionContextType | undefined>(undefined);

// Type the SessionProvider's props to specify `children` is a `ReactNode`
interface SessionProviderProps {
  children: ReactNode;
}

export const SessionProvider = ({ children }: SessionProviderProps): React.ReactElement => {
  const [sessionId, setSessionId] = useState<string>("");

  return (
    <SessionContext.Provider value={{ sessionId, setSessionId }}>
      {children}
    </SessionContext.Provider>
  );
};

// Type the `useSession` hook, ensuring the context is correctly typed
export const useSession = (): SessionContextType => {
  const context = useContext(SessionContext);
  if (!context) {
    throw new Error("useSession must be used within a SessionProvider");
  }
  return context;
};
