# OAuth2 and OpendIdConnect

## A brief history
In the past, most applications were easy to secure. They were mainly desktop and we could use windows authentication. Even for web applications running on intranet. 
They were easy to secure because they were not service based.
Mostly, they would run under the same domain, so an authentication cookie could be shared.
Even when wcf was implemented, server to server communication was using SAML 2.0 for the exchange of authorization and authentication data. 
It is still widely used in enterprise environments. It is not a very good fit for modern application architectures.

Now the world is different. Apps are usually not under the same domain, apis communicate with each other, with many integrations with other systems, etc.
Not that long ago, on every request, username and password was being sent: bad idea. Now we send tokens in every request.

First, people developed home-grown token services. A login endpoint that would take user name and password and generate the token. It is a better approach but it still sends credentials.
Why reivent the weel? we then would need to implement token validation and signing, separate authentication and authorization, for each application, etc.. A lot of mistakes and maintnance issues can happen.

So we look for an identity provider central to all applications. It is the reponsibility of the identity provider to verify the users are who they say they are, and to provide proof to other applications.
This responsibility shouldn't be on the clients.
So the identity provider is a central place for IAM, that stands for identity and access management:
![](doc/Iam.PNG)

It would be impossible to manage without a central identity provider. Changing encryptions algorithms for passwords: we change in one place. 
Some apps might require multi factor authentication and some not, etc. 

## OAuth2 and OpendIdConnect
When big problems like this emerge, standards are born: OAuth2 and OpendIdConnect.
OAuth2 is an open protocol to allow secure authorization in a simple and standardized way for web, mobile and desktop applications.
Oauth2 is all about authorization (we can request an access token to gain access to an API).
OAuth2 defines how a client application can securely achieve authorization:
we need this because all apps are not hosted equal (client mvc app runs totally on server side, while and angular app runs on the client and thus it cannot be trusted).
So the OAuth2 standard defines how to use the standard endpoints: token exprirantion, etc.
Oauth2 defines how to obtain tokens to access the API, not how to sign on a user or a client app. For that we have OpendIdConnect.

OpendIdConnect is a simple identiy layer on top of the OAuth2 protocol. 
A client application can request an identity token (next to an access token).
The identity token can be used to sign into a client application, while that same application uses the access token to access the API. 
OpendIdConnect is the superior protocol: it supersedes and extends OAuth2. Once we deal with users, we use OpendIdConnect.

## Authentication with OpendIdConnect
With Openid connect we are focusing on the identiy, which means we are focused on identiy tokens and not access tokens.
The client application asks the identity provider for proof of identity, so it can use it and rely on it.
Usually, a client app redirects the user to the identiy provider application, where the user needs to prove identity (like username and password, for example).
The identity provider generates an identity token, signs it and sends it to the client application.
The client verifies it and derives claims from it, like an authentication cookie, for example in asp.net core app. 
In an mvc app, the browser then sends that cookie on each request.
OpendIdConnect has several types of flows. It may depend on the app (web server app, client app etc).

For that there are public clients and confifential clients:
![](doc/public_confidential_client.PNG)

A flow is a set of HTTP requests and responses that determine how an authorization code and/or token are safely delivered to clients.
When we log in into an app, we are often redirected to other identity providers (google, microsoft, etc). There are more than just visible redirects.

Different client types or requirements can lead to different flows. Flows use endpoints (at the level of the Idp and at the level of the client).
1. The first endpoint at, IDP level, is the Authorization endpoint. It is used by the client application to obtain authentication and or authorization via redirection.
OpendIdConnect requires TLS! Token are not encrypted.
2. The second enpoint is the redirection endpoint or callback endpoint at client level (this client is redirected to the authorization endpoint, at the level of IDP, and then the IDP redirects back to the client)
It is used by the IDP  to return the authorization code and tokens.
3. Token endpoint (IDP level). Client applications can programatically request token via http post without redirection. It can autheticate confidential clients and not public clients.

Three main flows:
1. Authorization code flow: it is considered (or its variations) the best practice flow at the moment.
2. Implicit: deprecated
3. hybrid: deprecated

For both confidential clients, Authorization code flow plus PKCE should be used together. PKCE stands for Proof Key of Code Exchange.
Actually, PKCE is recommended for confidential clients and mandatory for public clients.

The authorization code flow derives its name from the fact that it returns an authorization code from the authorization endpoint.
The authorization code  is a short-lived single use credential, used to verify that the user who logged in, at level of IDP, is the same one that started the flow of the client application.
Tokens are returned from the token endpoint.
Because public clients cannot safely store their credentials, long lived access is restricted.
Consensus is moving away from handling security at the client in favor of the server (like for example the use of backend-for front end patterns).
Choosing the wrong flow may open security holes. Or wrong decisions on a flow (how to deliver tokens, where to include the claims, etc).

What is a good idea, changes over time. Security changes fast and we should keep up with it. And, a lot of approaches can work and most are not a good idea.

## Setting up identiy server
Creating an identity provider is an important part of creating an application. It is also something teams should avoid doing it from scratch. In this link, https://openid.net/specs/openid-connect-core-1_0.html, we can see the specification of OpendIdConnect.
It is very complex and it is recommned to use existing solutions like, KeyCloak, Duende Identity server, etc.
In our example, we will use identity server: it is part of .net foundation and certified by the OpenId fondation. The code is public and it is free for development and testing.
 For production, it depends on companies revenue.
 Duende has a set of templates we can sart with, by installing them with CLI.

'''
dotnet new install Duende.IdentityServer.Templates
'''
It comes with the following templates:
![](doc/duendeTemplates.PNG)

We have templates that come with EF core out of the box, an empty identity server, or an integration with ASP.NET Core Identity.
We are creating a new empty template to learn better how identity providers work, by executing the command:

dotnet new isempty -n Marvin.IDP

Marvin is the name of the company. We use this name instead of image gallery, because identity providers are central to applications, like seen previously.
The empty template comes with serilog for logging and handles a few things for us, like the injection of services and the configuration of the request pipeline:

'''

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true; // this means that audience clain is going to be included in tokens by default.
            })
            // identity related resources
            .AddInMemoryIdentityResources(Config.IdentityResources) // this resources map claim of user like first name, last name, etc
            .AddInMemoryApiScopes(Config.ApiScopes) //by requesting an API scope, a client can get access to an api.
            .AddInMemoryClients(Config.Clients); // for each application a client needs to be defined.
'''

This is coming from a config. Not being persited anywhere. It is ok to get started but of course not for production.
When we start the project we do not see much (no user interface yet). But we launch with a self host profile (see launch settings). 
We can the the config in the conventional well-known endpoint: https://localhost:5001/.well-known/openid-configuration
This endpoint lists supported scopes and related claims and more information. This is the endpoint, its config, that is read by other pieces of middleware to check where the enpoints can be found.
Tokens need to be signed and for that keys are needed. Identity server generates that on the fly. When going into production we want to replace that by something more persistent like a certificate.

## Add ui and users
Navigating to our identity provider project, we can call the dotnet new isui to add the ui template from duende. It adds razor pages into the project. 
We need to add related services to the IoC container but after this step, when we launch the application, we see the following ui:
![](doc/duendeui.PNG)

When we added the ui samples, a list of test users was automatically created for us.
A test user has a subject ID, that should be unique at the Identity provider level.
A test user also comes with a few claims.
Claims are information about the user: name, family names, etc.
Claims are related to scopes. 
Like we have seen, we are using the open id scope. So anytime a client requests the open id scope the user identifier claim is returned.
In order to return to a client claims like the name, they need to request the Progile scope. We need to add it to the identiy resource list:

'''
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
'''
So OpendIdConnect has a few standardized claims. So far we mentioned openid and profile:
|[](doc/profileopenidclaims.PNG)

There are more standard scope/claims mapping. Scope phone maps to phone_number, prhone_number_verified, etc. We can add our scopes as well.

## Authorization code flow
Autorhization code flow is the advised flow, with PKCE protection. First lets implement without PKCE.
All flows start with a request to the authorization endpoint. This is a simple redirection to a URI at the level of the identiy provider.
Lets see an example of such request uri:
![](doc/authorizationedpoint.PNG)

The redirect_uri is the URI of the client application where the response of the request is going to be delivered to.
We also see the scopes. We ask for the profile scope.
Response type on that request determines the flow that is used:
![](doc/responseTypes.PNG)

A response type of code means that we are going to use authorization code flow and at the same time that authorization is returned to the type via browser redirection.

![](doc/authorizationcodeflow.PNG)

First, the client send a request to the Authorization endpoint, with response type code and other parameters like scopes. 
At the IDP the user autheticaticates: the idp can ask the user for consent. 
At this point the client app does not know who the user is, but the idp does.
The IDP sends us back to the client application via redirection or form post.
It sends the authorization code, the response we asked for.
This code is delivered via the URI, that is called front channel communication (visible to the browser).
After that, the client asks the token endpoint through the back channel (does not use redirection and thus is not visible to the browser). This is a server to server http request (might not apply to static apps living in the browser TO CHECK because this is MVCC APP). 
The client sends to the token endpoint the authorization code, and other information like client id and secret. At the client, token is validated.
After the token is validated, the client application knows who the user is. There are libraries that do the validation for us.
Onm our case, the distinction between fron-channel and back-channel communication:
![](doc/fronchannel.PNG)

### Loging in with Authorization code flow
1. The first step is to configure a client that will represent our application at the IDP level:
```
 new Client[] 
     { new Client { ClientName = "Image Gallery" , 
         ClientId = "imagegalleryclient", // client app identifier
         AllowedGrantTypes = GrantTypes.Code, // authorization code flow
         RedirectUris = { "https://localhost:7184/signin-oidc" }, // client redirect uri  - signin oidc is the default but it can be configured
         AllowedScopes = 
         { 
             IdentityServerConstants.StandardScopes.OpenId,
             IdentityServerConstants.StandardScopes.Profile
         },
         ClientSecrets = { new Secret("secret".Sha256()) }
     } };
```
2. On our client application because it is a mvc app, first download Microsoft.AspNetCore.Authentication.OpenIdConnect 
3. Configure the client request pipeline: 
```

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme); 

```
We configured cookie as a default authentication scheme. This means that once we have an identity token, that is validated and transformed into identity claims, it will be stored in an encrypted cookie.
In subsequent request, the cookie will be sent and it is this cookie our app is going to check to validate authenticated requests.