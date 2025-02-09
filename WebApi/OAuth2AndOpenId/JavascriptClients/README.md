# Securing javascript clients
From a security point of view, javascript and client code is a challenge because the code does not live on the server, like an asp.net core app.
A server we can control and trust, but clients are way harder. An example:
An mvc application in asp.net core, even after the user logs in, we can control access at the level of controller actions. On javasctipt all the code is already on the client and it cannot be blocked. Initially this was not a problem, because javascript was used for small taks. An app would serve a page, after authorization, that had a small portion of javasctipt on it.
Today, javascript is used to build entire applications. The apps require tokens to talk to apis. There are several issues with javascript apps:
1. Cannot safely store secrets. Javascript code with secrets can be viewed by anyone with access to the machine. So client authentication flows do not make sense.
1. Token storage is precarious.We cannot store them in memory, browser, etc. Javascript is vulnerable to XSS (cross site scripting) attacks so an attacker could read our not http cookies or session storage and obtain the tokens.
1. Refresh token storage is even more dangerous. With it, you can get new tokens.

Browsers are restricting cookie usage which disables common OAuth2, Opendidconnect approaches.

The solution for all of this is to have the server handling the security. It is the backend for frontend pattern.

## Backend-for-Frontend pattern
The backend-for-Frontend (BFF) is a layer that sits between a client and a backend. It is a typical pattern for situations like a client from a web browser and a mobile client that request data from the same api. A BFF layer would transform the data to fit the ui better:
![](BFF.PNG)
A BFF can be used to handle security and session management for a frontend.
On approach is to host the javascript application in the same project that contains the API (local api approach)
The flow is started from the server and there is no code in the javascript client for the flow.
An identity token is requested and that results in a cookie. The browser makes sure the cookie is sent on every call to the api. No access tokens required, security is handled by the token itself