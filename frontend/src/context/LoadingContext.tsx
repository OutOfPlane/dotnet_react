import { createContext, useContext, useState, useRef } from "react";

type LoadingContextType = {
  isLoading: boolean;
  startLoading: () => void;
  stopLoading: () => void;
};

const LoadingContext = createContext<LoadingContextType | undefined>(undefined);
const SHOW_DELAY = 500; // ms

export const LoadingProvider = ({ children }: { children: React.ReactNode }) => {
  const [isLoading, setLoading] = useState(false);
  const delayTimer = useRef<ReturnType<typeof setTimeout> | null>(null);

  const startLoading = () => {
    if(delayTimer.current == null){
        delayTimer.current = setTimeout(() => setLoading(true), SHOW_DELAY);
    }
  }
  const stopLoading = () => {
    if(delayTimer.current !== null){
        clearTimeout(delayTimer.current);
        delayTimer.current = null;
    } 
    setLoading(false);
  }

  return (
    <LoadingContext.Provider value={{ isLoading, startLoading, stopLoading }}>
      {children}
    </LoadingContext.Provider>
  );
};

export function useLoading() {
  const ctx = useContext(LoadingContext);
  if (!ctx) throw new Error("useLoading must be used inside LoadingProvider");
  return ctx;
}
