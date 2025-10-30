import { useTranslation } from "react-i18next";
import { useLoading } from "../context/LoadingContext";
import { useError } from "../context/ErrorContext";

function delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
}

const Dashboard = () => {
  const { t } = useTranslation();
  const { startLoading, stopLoading } = useLoading();
  const { setError } = useError();


  const loadData = async () => {
    startLoading();
    await delay(5000);
    setError({message: "I sharted", title: "Oh no"});
    stopLoading();
  }

  return (
  <div>
    <h1 className="text-2xl font-bold mb-4">{t("menu.dashboard")}</h1>
    <p>{t("This is the dashboard view. Display summaries, widgets, etc.")}</p>
    <button onClick={loadData} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 mx-4 rounded">Load Me</button>
  </div>
  );
};

export default Dashboard;
