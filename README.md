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






