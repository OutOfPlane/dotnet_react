import { useState } from 'react';
import { useTranslation } from "react-i18next";
import { ErrorProvider } from "./context/ErrorContext";
import { LoadingProvider } from './context/LoadingContext';

import { loginUser } from './api/auth';
import { User } from './backendmodels';

import Login from './components/Login';
import ImageLabelDropdown from "./components/ImageLabelDropdown";
import ErrorBanner from "./components/ErrorBanner";
import LoaderOverlay from './components/LoaderOverlay';

import Dashboard from "./pages/Dashboard";
import Settings from "./pages/Settings";

const allowedViewnames = ["dashboard", "settings", "base"] as const;

type ViewType = typeof allowedViewnames[number];

function strToView(str: string): ViewType | undefined {
  if ((allowedViewnames as readonly string[]).includes(str)) return str as ViewType;
  return undefined;
}

const languageOptions = [
  { value: "en", label: "EN", imageUrl: "https://flagcdn.com/w20/gb.png" },
  { value: "de", label: "DE", imageUrl: "https://flagcdn.com/w20/de.png" },
  { value: "cz", label: "CZ", imageUrl: "https://flagcdn.com/w20/cz.png" },
];

function App() {
  const { t, i18n } = useTranslation();
  const [activeView, setActiveView] = useState<ViewType | undefined>("dashboard");
  const [lang, setLangInt] = useState("de");
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [userMenu, setUserMenu] = useState<string[]>(["dashboard", "settings", "base"]);

  const onLogin = async (user: string, password: string) => {
    setCurrentUser(await loginUser(user, password));
  }

  const renderView = () => {
    if (currentUser !== null) {
      switch (activeView) {
        case "dashboard":
          return <Dashboard />;
        case "settings":
          return <Settings />;
        default:
          return <div>Unknown view</div>;
      }
    } else {
      return <Login onLogin={onLogin} />
    }

  };

  const renderMainMenu = () => {
    if (currentUser !== null) {
      const options: any[] = [];
      userMenu.forEach(element => {
        options.push(<li><button className="cursor-pointer block p-2 rounded hover:bg-gray-700 w-full text-left" onClick={() => setActiveView(strToView(element))}>{t("menu." + element)}</button></li>)
      });
      return (
        <nav className="flex flex-col justify-between p-2 flex-1 overflow-y-auto">
          <ul className="space-y-1 overflow-y-auto flex-1 min-h-20 mb-2">
            {options}
          </ul>
          <button onClick={() => setCurrentUser(null)} className="bg-red-500 hover:bg-red-700 cursor-pointer block p-2 rounded w-full">{t("controls.logout")}</button>
        </nav>
      )
    }
  }

  const setLang = (value: string) => {
    setLangInt(value);
    i18n.changeLanguage(value);
    console.log(value);
  }


  return (
    <LoadingProvider>
      <ErrorProvider>
        <ErrorBanner />
        <LoaderOverlay />
        <div className="flex h-screen">
          <aside className="flex flex-col h-screen w-64 bg-gray-800 text-white p-2 space-y-4">
            <ImageLabelDropdown className="p-2"
              items={languageOptions}
              selected={lang}
              onChange={setLang}
            />
            <div className="mb-6">
              <img
                src="./TLFI_Logo_150x77px.jpg"
                alt="Logo"
                className="w-32 h-auto mx-auto rounded"
              />
            </div>
            {renderMainMenu()}
          </aside>
          <main className='p-4'>
            {renderView()}
          </main>
        </div>

      </ErrorProvider>
    </LoadingProvider>
  );
}

export default App;
