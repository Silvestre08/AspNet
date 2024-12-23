# OAuth2 and OpendIdConnect

## A brief history
In the past most applications were easy to secure. They were mainly desktop and we would use windows authentication. Even for web applications running on intranet. They were easy to secure because they were not service based.
Mostly they would run under the same domain, so an authentication cookie could be shared.
Even when wcf was implemented, server to server communication was using SAML 2.0 for the exchange of authorization and authentication data. It is still widely used in enterprise environments.
It is not a very good fit for modern application architectures.

Now the world is different. Apps are usually not under the same domain, apis communicate to each other, with many integrations, etc.
Then on every request username and password was being sent: bad idea.
Now we send tokens in every request.

First people developed home-grown token services. A login endpoint that would take user name and password and generate the token. It is a better approach but still sends credentials..
Why reivent the weel? we then would need to implement token validation and signing, separate authentication and authorization, etc.. A lot of mistakes and maintnance can happen.

So we look for an identity provider central to all applications. It is the reponsibility of the identity provider to verify the users are who they say they are and to provide proof to other applications.
It shouldnt be on the clients.
So the identity provider is a cenrtal place for IAM: identity and access management:
![](doc/Iam.PNG)

It would be impossible to manage without a central identity provider. Changing encryptions algorithms for passwords: we change in one place. 
Some apps might require multi factor authentication and some not, etc. 

## OAuth2 and OpendIdConnect
OAuth2 is an open protocol to allow secure authorization in a simple and standard method from web, mobile and desktop applications.
Oauth2 is all about authorization (we can request an access token to gain access to an API).
OAuth2 defines how a client application can securely achieve authorization:
we need this because all apps are not hosted equal (client mvc app runs totally on server side, while and angular app runs on the client and thus it cannot be trusted).
So the OAuth2 standard defines how to used the standard enpoints: token exprirantion, etc.
So Oauth2 defines how to obtain tokens to access the APi, not how to sign on a user on a client app. For that we have openid connect.

OpenID connect is a simple identiy layer on top of the OAuth2 protocol. 
A client application can request an identity token (next to an access token).
The identity token can be used to sign into a client application, while that same application uses the access token to access the API. 
OpendIdConnect is the superior protocol: it supersedes and extends OAuth2. Once we deal with users, we use OpendIdConnect.

## Authentication with OpendIdConnect
With Openid connect we are focusing on the identiy, which means we are focused on identiy tokens and not access tokens.
The client application asks the identity provider for proof of identity so it can use it and rely on it.
Usually a client app redirects the user to the identiy provider applications, where the user needs to prove identity (like username and password, for example).
The identity provider generates an identity tokens, signs it and sends it to the client application.
The client verifies it and derives claims from it, like an authentication cookie, for example in asp.net core app. 
In an mvc app, the browser sends that cookie on each request.
OpendIdConnect has several types of flows. It may depend on the app (web server app, client app etc).

For that there are public clients and confifential clients:
![](doc/public_confidential_client.PNG)

A flow is a set of HTTP requests and responses that determine how code an or tokens are safeliy delivered to clients.
When we log in into an app, we are often redirected to other identity providers (google, microsoft, etc). There are more than just visible redirects.

Different client types or requirement led to different flows. Flows use endpoints (at the level of the Idp and at the level of the client).
1. The first endpoint at IDP level is the Authorization endpoint. It is used by the client application to obtain authentication qnd or authorization via redirection.
OpendIdConnect requires TLS! Token are not encrypted.
2. The second enpoint is the redirection endpoint or callback endpoint at client level (this client redirect to the authorization endpoint at the level of IDP and the IDP redirects back to the client)
It is used by the IDP  to return code and tokens.
3. Token endpoint (IDP level). Client applications can programatically request token via http post without redirection. It can autheticate confidential clients and not public clients.

Three main flows:
1. Authorization code flow: it is considered (or its variations) the best practice flow at the moment.
2. Implicit
3. hybrid

For both confidential clients, Authorization code flow plus PKCE should be used together.
Actually it is recommended for confidential clients and obligatory for public clients.

The authorization code flow derives its name from the fact that it returns an authorization code from the authorization endpoint.
The authorization code  is a short-lived single use credential, used to verify that the user who logged in at level of IDP is the same one that started the flow of the client application.
Tokens are returned from the token endpoint.
Because public clients cannot safely store their credentials, long lived access is restricted.
Consensus is owards moving away from handling security at the client in favor of the server (BFF pattern).
Choosing the wrong flow may open security holes.

What is a good idea, changes over time. Security changes fast and we should keep up with it. And, a lot of approaches can work and most are not a good idea.