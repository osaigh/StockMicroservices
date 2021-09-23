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





