import { useLoading } from "../context/LoadingContext";

export default function LoaderOverlay() {
  const { isLoading } = useLoading();

  if (!isLoading) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white px-6 py-4 rounded shadow">
        <span className="animate-pulse">Loading...</span>
      </div>
    </div>
  );
}
