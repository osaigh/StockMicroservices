import React from "react";
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import { Home } from "./pages";
import "bootstrap/dist/css/bootstrap.min.css";

import SignInCallback from "./pages/signincallback";
import SignOutCallback from "./pages/signoutcallback";

export default function App() {
    console.log("In App.js routing");
    return (
        <Router>
        <Switch>
        <Route path="/" exact>
        <Home />
        </Route>
        <Route path="/signincallback" exact>
        <SignInCallback />
        </Route>
        <Route path="/signoutcallback" exact>
        <SignOutCallback />
        </Route>
        </Switch>
        </Router>
);
}