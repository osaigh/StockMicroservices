import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./pages/home/Home";
import { SignInCallback } from "./pages/signin/signincallback";
import { SignOutCallback } from "./pages/signout/signoutcallback";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/signincallback" element={<SignInCallback />} />
          <Route path="/signoutcallback" element={<SignOutCallback />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
