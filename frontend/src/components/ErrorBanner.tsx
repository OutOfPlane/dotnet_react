// components/ErrorBanner.tsx
import { useError } from "../context/ErrorContext";
import { useTranslation } from "react-i18next";

export default function ErrorBanner() {
    const { error, clearError } = useError();
    const { t } = useTranslation();

    if (!error.title) return null;

    return (
        <div className="fixed inset-0 z-50 bg-black bg-opacity-75">
            <div className="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-red-100 border border-red-400 text-red-700 px-4 pr-16 py-2 rounded mb-4 flex-row justify-between items-center">
                <h1 className="text-2xl">{t(error.title)}</h1>
                <span>{t(error.message)}</span>
                <button onClick={clearError} className="absolute text-red-700 font-bold px-2 top-2 right-2">✕</button>
            </div>
        </div>
    );
}
