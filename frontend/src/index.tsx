import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import "./i18n"; // initialize i18n before rendering
import { triggerGlobalError } from "./context/ErrorContext";


const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
window.onerror = function (message, source, lineno, colno, error) {
  console.error("Global error:", { message, source, lineno, colno, error });
  triggerGlobalError("Unhandled Exception", message.toString());
  return true;
  // Optionally log or show UI
};

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
