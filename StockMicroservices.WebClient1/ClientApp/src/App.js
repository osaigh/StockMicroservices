import Home from "./pages/home/Home";
import { SignInCallback } from "./pages/signin/signincallback";
import {SignOutCallback} from "./pages/signout/signoutcallback";
import {BrowserRouter, Routes, Route} from "react-router-dom"



function App() {

  return (
    <div className="App" >
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="signincallback" exact element={<SignInCallback/>}>

          </Route>
          <Route path="signoutcallback" exact element={<SignOutCallback/>}>

          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
