import { useEffect, useState } from "react";
import { Product } from "../backendmodels";
import { getProduct } from "../api/products";
import { useError } from "../context/ErrorContext";
import { useLoading } from "../context/LoadingContext";

const Settings = () => {
  const [product, setProduct] = useState<Product | null>(null);
  const { setError } = useError();
  const { startLoading, stopLoading } = useLoading();

  useEffect(() => {
    async function fetchProduct() {
      try {
        startLoading();
        setProduct(await getProduct(1));
        stopLoading();
      } catch (err) {
        if (err instanceof Error) setError({title: "Fehler beim Laden des Produkts", message: err.message})
      }
          
    }

    fetchProduct();
  }, []);

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Settings</h1>
      <p>{product?.name}</p>
    </div>
  );
};

export default Settings;
