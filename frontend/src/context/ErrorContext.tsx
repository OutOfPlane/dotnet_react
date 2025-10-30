// context/ErrorContext.tsx
import { createContext, useContext, useState } from "react";
import { useLoading } from "./LoadingContext";

type ErrorDescriptionType = {
    title: string;
    message: string;
}

type ErrorContextType = {
  error: ErrorDescriptionType;
  setError: (error: ErrorDescriptionType) => void;
  clearError: () => void;
};

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);
let globalSetError: ((title: string, message: string) => void) | null = null;

export function ErrorProvider({ children }: { children: React.ReactNode }) {
  const [error, setErrorInt] = useState<ErrorDescriptionType>({title:"", message:""});
  const { stopLoading } = useLoading();

  const clearError = () => setErrorInt({title:"", message:""});

  globalSetError = (title: string, message: string) => {
    setErrorInt({title:title, message:message}); // ✅ Assign to global reference
  }

  const setError = (error: ErrorDescriptionType) => {
    setErrorInt(error);
    if(error.message) stopLoading();
  }
  return (
    <ErrorContext.Provider value={{ error, setError, clearError }}>
      {children}
    </ErrorContext.Provider>
  );
}

export function useError() {
  const ctx = useContext(ErrorContext);
  if (!ctx) throw new Error("useError must be used within ErrorProvider");
  return ctx;
}

// ✅ Global setter accessible anywhere
export function triggerGlobalError(title: string, message: string) {
  if (globalSetError) globalSetError(title, message);
}