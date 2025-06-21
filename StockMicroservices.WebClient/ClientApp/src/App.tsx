import  {useState,useContext,useEffect} from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { UserAuthenticationContext } from "./context";
import {Login, Order, Orders, Stocks} from './pages';
import { UserAuthenticationService } from "./services";
import { User } from "oidc-client";
import { SignInCallback } from "./pages/signin/signincallback";
import { SignOutCallback } from "./pages/signout/signoutcallback";


function App() {
  const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext); 
  const [user, setUser] = useState<User|null>(null);

  useEffect(() => {
    async function getUserAsync() {
          const user = await userAuthenticationService?.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          if(user){
            setUser(user);
          }
        });
  }, []);

  return (
    <div className="App">
     <BrowserRouter>
        <Routes>
          <Route path="/" element={<Stocks />} />
          <Route path="orders" element={<Orders />} />
          <Route path="order/:stockId/:transactionType" element={<Order />} />
          <Route path="login" element={<Login />} />
          <Route path="signincallback" element={<SignInCallback/>}>
          </Route>
          <Route path="signoutcallback" element={<SignOutCallback/>}>
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
