# Project Title

StockMicroservices

## Description

A project demonstrating various web technologies and techniques. This StockMicroservices application takes the Stock application and implements it with a microservice architecture. This application is comprised of Identity Server for authentication, a React-based web client and a web-api all running on containers. It leverages Docker, Ocelot API Gateway, RabbitMq for messaging.

## List of projects

### StockMicroservices.IdentityServer

* An ASP.Net Core server for authentication using Identity Server 4. Protects the API resources defined

### StockMicroservices.API

* An ASP.Net Core Web API project. Contains all stock data 

### StockMicroservices.API.Tests

* Unit and Integration tests for the Stock.API.

### StockMicroservices.WebClient

* A react based web client that authenticates the user using the Stock.IdentityServer and retrieves data from the Stock.API

### StockMicroservices.StockMarketUpdater

* A c# console application that periodically updates the Stocks data.


## How to Run

Ensure that you have .NET 6.0, Node js and Docker Desktop installed. 

* Select docker-compose from the 'Startup Projects' toolbar and run 'Docker Compose' or within the solution directory, open a command prompt and run 'docker-compose up'.
* Navigate to a browser window and go to the address 'http://localhost:44100'. This opens the React web client

To run the kubernetes example, ensure you have installed and configured a minikube cluster. 

* Navigate to the solution directory and open a command prompt
* Start the minibube cluster with 'minikube start'
* Run the command 'kubectl apply -f k8'. This will create all the necessary deployments, services, secrets and configmap objects.
* To stop run the command 'kubectl delete -f k8'. To stop the cluster run 'minikube stop' 





