# Project Title

StockMicroservices

## Description

A project demonstrating various web technologies and techniques. This StockMicroservices application takes the Stock application and implements it with a microservice architecture. This application is comprised of Identity Server for authentication, a React-based web client and a web-api all running on containers. It leverages Docker, Ocelot API Gateway, RabbitMq for messaging. It also uses Mongo Db for database persistence as well as Mongo-Express. The API from the Stock application has been re-implemented with a backing store of Mongo-Db. The controllers remain unchanged because the data access layer as been abstracted away with the repository pattern. 

## List of projects

### StockMicroservices.IdentityServer

* An ASP.Net Core server for authentication using Identity Server 4. Protects the API resources defined

### StockMicroservices.API

* An ASP.Net Core Web API project. Contains all stock data 

### StockMicroservices.API.Tests

* Unit and Integration tests for the Stock.API.

### StockMicroservices.WebClient

* A React based web client that authenticates the user using the Stock.IdentityServer and retrieves data from the Stock.API

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
* Wait for about 5 minutes for all the pods to be 'Ready' since kubernetes will attempt to pull all the images from docker. To check the status of the pods, run 'kubectl get pods'
* Once all the pods are running, you need to create a proxy to the application running within the cluster. You will need to create three proxies as follows. Open three command prompts.
* Mongo-Express - on the first command prompt, run  'kubectl port-forward svc/mongo-express 8081' . This binds the local port 8081 on your machine to the port 8081 of the service mongo-express
* IdentityServer - on the second command prompt, run  'kubectl port-forward svc/stockidentityserver 44401' . This binds the local port 44401 on your machine to the port 44401 of the service stockidentityserver
* ApiGateway - on the third command prompt, run 'kubectl port-forward svc/stockapigateway 44405' . This binds the local port 44405 on your machine to the port 44405 of the service stockapigateway
* StockWebClient(react) - on the fourth command prompt, 'run kubectl port-forward svc/stockwebclient 44100' . This binds the local port 44100 on your machine to the port 44100 of the service stockwebclient
* Navigate again to a browser and go to the address 'http://localhost:44100'
* To stop run the command 'kubectl delete -f k8'. To stop the cluster run 'minikube stop' 
* Close all the command prompts





