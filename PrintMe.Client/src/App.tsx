import { useState } from "react";
// import reactLogo from './assets/react.svg'
// import viteLogo from '/vite.svg'
import "./App.css";
import Header from "./components/Header/Header.tsx";

// import MyComponent from './MyComponent.tsx'
import MainPage from "./components/MainPage/MainPage.tsx";
import PseudoLogin from "./components/Header/PseudoLogin.tsx";

function App() {
  const [isLogined, setIsLogined] = useState<boolean>(false);

  return (
    <>
      <Header isLogined={isLogined} />
      <PseudoLogin onClick={setIsLogined} />
      <MainPage />
    </>
  );
}

export default App;
