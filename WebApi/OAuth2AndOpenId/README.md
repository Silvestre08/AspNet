# OAuth2 and OpendIdConnect

## A brief history

In the past, most applications were easy to secure. They were mainly desktop applications and we could use windows authentication. Even for web applications running on intranet.
They were easy to secure because they were not service based.
Mostly, they would run under the same domain, so an authentication cookie could be shared.
Even when wcf was implemented, server to server communication was using SAML 2.0 for the exchange of authorization and authentication data.
It is still widely used in enterprise environments. It is not a very good fit for modern application architectures.

The world is now different. Apps are usually not under the same domain, apis communicate with each other, with many integrations with other systems, etc.
Not that long ago, on every request, username and password were being sent: bad idea. Now we send tokens in every request.

First, people developed home-grown token services. A login endpoint that would take username and password and generate the token. It is a better approach but it still sends credentials.
Why reivent the weel? we then would need to implement token validation and signing, separate authentication and authorization, for each application, etc.. A lot of mistakes and maintnance issues can happen.

So we look for an identity provider central to all applications. It is the reponsibility of the identity provider to verify the users are who they say they are, and to provide proof of identity to other applications.
This responsibility shouldn't be on the clients.
So the identity provider is a central place for IAM (Identity and Access Management):
![](doc/Iam.PNG)

It would be impossible to manage without a central identity provider. Changing encryptions algorithms for passwords: we change in one place.
Some apps might require multi-factor authentication and some not, etc.

## OAuth2 and OpendIdConnect

When big problems like this emerge, standards are born: OAuth2 and OpendIdConnect.
OAuth2 is an open protocol to allow secure authorization in a simple and standardized way for web, mobile and desktop applications.
Oauth2 is all about authorization (we can request an access token to gain access to an API).
OAuth2 defines how a client application can securely achieve authorization:
we need this because all apps are not hosted equal (client mvc app runs totally on server side, while and angular app runs on the client and thus it cannot be trusted).
So the OAuth2 standard defines how to use the standard endpoints: token expriration, etc.
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

For that there are public clients and confidential clients:
![](doc/public_confidential_client.PNG)

A flow is a set of HTTP requests and responses that determine how an authorization code and/or token are safely delivered to clients.
When we log in into an app, we are often redirected to other identity providers (google, microsoft, etc). There are more than these just visible redirects that happen behind the sceness.

Different client types or requirements can lead to different flows. Flows use endpoints (at the level of the Idp and at the level of the client).

1. The first endpoint, at the IDP level, is the Authorization endpoint. It is used by the client application to obtain authentication and or authorization via redirection.
   OpendIdConnect requires TLS! Tokens are not encrypted.
2. The second enpoint is the redirection endpoint or callback endpoint at client level (this client is redirected to the authorization endpoint, at the level of IDP, and then the IDP redirects back to the client).
   It is used by the IDP to return the authorization code and tokens.
3. Token endpoint (IDP level). Client applications can programatically request token via http post without redirection. It can autheticate confidential clients and not public clients.

Three main flows:

1. Authorization code flow: it is considered (or its variations) the best practice flow at the moment.
2. Implicit: deprecated
3. hybrid: deprecated

For both confidential and public clients, Authorization code flow plus PKCE should be used together. PKCE stands for Proof Key of Code Exchange.
Actually, PKCE is recommended for confidential clients and mandatory for public clients.

The authorization code flow derives its name from the fact that it returns an authorization code from the authorization endpoint.
The authorization code is a short-lived single use credential, used to verify that the user who logged in, at the level of the IDP, is the same one that started the flow of the client application.
Tokens are returned from the token endpoint.
Because public clients cannot safely store their credentials, long lived access is restricted.
Consensus is moving away from handling security at the client in favor of the server (like for example the use of backend-for front end patterns).
Choosing the wrong flow may open security holes. Or wrong decisions on a flow (how to deliver tokens, where to include the claims, etc).

What is a good idea, changes over time. Security changes fast and we should keep up with it. And, a lot of approaches can work and most are not a good idea.

## Setting up identiy server

Creating an identity provider is an important part of creating an application. It is also something teams should avoid doing it from scratch. In this link, https://openid.net/specs/openid-connect-core-1_0.html, we can see the specification of OpendIdConnect.
It is very complex and it is recommended to use existing solutions like, KeyCloak, Duende Identity server, etc.
In our example, we will use identity server: it is part of the .NET foundation and certified by the OpenId fondation. The code is public and it is free for development and testing.
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

This is coming from a config. Not being persited anywhere. It is ok to get started but of course not ok for production.
When we start the project we do not see much (no user interface yet). But we launch with a self host profile (see launch settings).
We can verify the config in the conventional well-known endpoint: https://localhost:5001/.well-known/openid-configuration
This endpoint lists supported scopes and related claims and more information. This is the endpoint, and its config, that is read by other pieces of middleware to check where the enpoints can be fosund.
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

The redirect uri is the URI of the client application where the response of the request is going to be delivered to.
We also see the scopes. We ask for the profile scope.
Response type on that request determines the flow that is used:
![](doc/responseTypes.PNG)

A response type of code means that we are going to use authorization code flow and at the same time that authorization is returned to the type via browser redirection.

![](doc/authorizationcodeflow.PNG)

1. First, the client sends a request to the Authorization endpoint, with response type code and other parameters like scopes.
2. At the IDP, the user autheticaticates with user name and password, for example: the idp can ask the user for consent. At this point the client app does not know who the user is, but the idp does.
3. The IDP sends us back to the client application via redirection or via a form post. It sends the authorization code, the response we asked for.
   This code is delivered via the URI, that is called front channel communication (visible to the browser).
4. The client then asks the token endpoint through the back channel (does not use redirection and thus is not visible to the browser). This is a server to server http request (might not apply to static apps living in the browser TO CHECK because this is MVCC APP).
5. The client sends to the token endpoint the authorization code, and other information like client id and secret.
6. At the client, token is validated. After the token is validated, the client application knows who the user is. There are libraries that do the validation for us.
   On our case, the distinction between fron-channel and back-channel communication:
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
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:5001/"; // authorization server
    options.ClientId = "imagegalleryclient"; // should match client id at the identity provider.
    options.ClientSecret = "secret";
    options.ResponseType = "code"; // authorization code flow
    // these scopes are automatically requested by the middleware
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");
    //options.CallbackPath = new PathString("signin-oidc");
    // this callback path is the same as the redirect URI that we defined at the IDP level. It is commented because this is the default. here for demo purposes.
    options.SaveTokens = true; // configure middleware to save tokens it receives from idp so we can use them.
});
```

We configured cookie as a default authentication scheme. This means that once we have an identity token, that is validated and transformed into identity claims, it will be stored in an encrypted cookie that will be analyzied by our web app.. It is where we store the credentials. Notice that the authentication default scheme matches the scheme of the cookie.
In subsequent requests, the cookie will be sent and it is this cookie our app is going to check to validate authenticated requests.
Note: an authentication scheme is a string identifier that represents a specific authentication handler in asp.net core. You can configure multiple authentication methods (e.g., cookies, JWT, or OAuth). Each method is registered with a unique scheme name. Policy enforcement: In ASP.NET Core Authorization, you can tie a specific scheme to an authorization policy.
We can see that we added openid connect scheme as well. This handler will be responsible to perform the authentication requests to the IDP. See that the scheme matches the challenge scheme. See as well that the signin scheme matches the cookie scheme defined previously. This will make sure that the claims will be stored in the cookie with the authentication scheme with the same name.
Then we make sure the request pipelie is requiring authentication:

```
app.UseAuthentication(); // order in the request pipeline matters.
app.UseAuthorization();
```

Now we need to ensure we cannot access a part of our application without an authenticated user. We can do that by decorate the controller with the authorize attribute.

After aplying all this code, once we start the apps, we will notice we will be automatically redirected to the identity provider. That is because of the authorize attribute and because we haven't logged in yet, we will immediatly be redirected, by the middleware, to the authorization endpoint of the identity provider.
By inspecting the browser we will see the authorization request being performed. But we will not see all the requests we mentioned before: because of the back channel communication, the token request is invisible to the browser.
When we inspect the logs of the identity provider we can verify a few more things going on:

1. Request to the token endpoint so the client secret was verified.
2. See the tokens being sent back to the client.

So what happened was that the middleware received an authorization code and it used that to call the token together with the secret and client id. It received an identity token and validated it, stored it and created identity claims from it and stored in an encrypted cookie.
We can verify by looking into the console of our mvc client appÇ
![](doc/identitytoken.png)

We can inspect the contents of a token on jwt.io.
One good thing to mention here is that we can ask for user consent:
![](doc/userconsent.png)
We stat that setting on the IDP, while configuring the client:

```
RequireConsent = true,
```

### Authorization code flow injection attack

Authorization code flow is vulnerable to code injection attacks. This means that an attacker got a hold of the authorization code of the victim and the code is a short term proof of identity that links the server session to the browser session. So the attacker can impersonate a user.
The way to mitigate this is to use the PKCE.
This means that on every request to the authorization endpoint, a secret is created by the client. When calling the token endpoint, the secret is verified. This mitigates the attack because the atacker does not access the secret generated by the client on every request.

The steps of the authorization code flow with PKCE are similar to the standar authorization code flow, with a few key differences.
Before calling the authorization endpoint, the client application creates a code_verifier and hashes it. It sends this hashed version (the code_challenge) to the authorization endpoint. The id stores the code. The nexgt steps are similar besides the fact that the token request will include the original code, that is going to be hashed by the identity provider and see if they match.
Summary:

![](doc/pkce1.png)

## Logout

To logout the users the proper way we need to implment the following:

1. Add the loggout button in the user interface to call the action of the controller above. (Layout cshtml)

```
        @if (User.Identity.IsAuthenticated)
        {
            <li>
                <a class="nav-link text-dark" asp-area="" asp-controller="Authentication" asp-action="Logout">Logout</a>
            </li>
        }
```

2. Add an authentication controller to our mvc app.

```
    public class AuthenticationController : Controller
    {
        [Authorize]
        public async Task Logout()
        {
            // to logout the application must clear the cookie.
            // the scheme name must match the name we used to configure the authentication middleware.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirects to the IDP linked to the scheme
            // OpenIdConnectDefaults.AuthenticationScheme so it clears the session
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

    }
```

3. The above actions logs out the user from our MVC app but without the lines that sign out of the open id authentication scheme, the user would not be logged out from the identity provider and as such the login screen would not be shown again.

4. Implement the redirect after signing out. To not get stuck at the logout page of the IDP, we need to configure it and register the post logout redirects:

```
// config of the client at the idp level
PostLogoutRedirectUris = { "https://localhost:7184/signout-callback-oidc" },

```

On the mvc app side signout-callback-oidc is the default value used by opend id connect so nothing there needed.

See the logging of the idp of a session end. We see the redirect and a token hint. The token hint helps to do confirm the user that originated the request and prevent redirect attacks.
![](doc/end%20sessionidp.png)

```
    public static readonly bool AutomaticRedirectAfterSignOut = true; // this redirects automatically after logout.
```

## User info endpoint

By default, identity server does not include identiy claims safe for the user identifier in the identity token (we do not want to configure like that):

1. In some flows the identity token might be returned from the authorization endpoint directly (the token becomes bigger).
2. Decreases the potential of attack.

![](doc/claimsinidentitytoken.png)

So how the we obtain the user information?
There is a user info endpoint we can use to request additional claims. It requires an access token with scopes related to the claims that must be returned: if we want the profile information, the access token must contain the profile scope.
The access tokens and refresh tokens can b returned from the token endpoint as well. In our flow an access token is delivered together with an identity token.
So the flow revised (omitting the first part of the authorization code):
![](doc/accessTokenandIdentity.png)
![](doc/accessTokenandIdentity2.png)
To get the middleware of out MVc app to call the user infor endpoint is a matter of setting this when we add the openid connect to the IoC :

```
    options.GetClaimsFromUserInfoEndpoint = true;
```

If we inspect the token we do not see the claims but if we see the output of our mvc app and the idp, we can verify that there were requests to the user info endpoint and we can verify the information:
![](doc/claimsidentity.png)

Let's inspect the identity token and see what its fields mean.
The format of an identity token is JWT:
![](doc/identityTokendecripted.png)

1. The sub is the user identifier or "subject". it is always returned when used openidconnect.
2. Iss means the issuer of the idenity token: URI of the identity provider.
3. aud stands for audience, the audience for this token. In our case the client application.
4. The nexrt four items represnet the seconds passed since January first 1970. Iat issued at. exp: expiration of the token.
5. amr: authentication methods used.
   ![](doc/identityTokendecripted2.png)
   This can have other values like a one time password or a multi-factor authentication.
6. nonce: number only used once. Generated at client level and it is sent back ffrom the IDP. It can be checked during token validation and helps prevent cross-site request forgery attacks.
7. at_hash is a number used to link an access token to this specific identity token

Depending on the idp used, extra claims can come with the identity token

## Working with claims

The identity claims allows us to show specific information in the web app. They are also important for authorization.
To make sure the claim types stay the same as they come from the identity provider, we can add this in the program.cs:

```
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
```

The middleware of openidconnect filter out some claims before creating the claims idenity and storing it in a cookie. Most likely claims that are not that useful. We are allowed to get the claims we want and get rid of the ones that are not necessary (keep the cookie smaller as it can be). See the example of how to configure the middleware to not filter out the audience claim, and to remove a claim from the claims identity:

```
// done in addOpenIdConnect method
options.ClaimActions.Remove("aud"); // remove a filter
  options.ClaimActions.DeleteClaim("idp");
```

## Rbac

So far we have seen authentication: the process to determine who a user is. Lets dive into authorization: the process of determining what a user is allowed to do.
One way to do that is use Role-Base access control. A role has a set of permissions that tell us what a user is/is not allowed to do.
There is also another way like attribute based access control (preferred over rbac and to see later).
To enable RBAC we need:
1. define new claim "role" and add to test users
2. Add a new identity resource. Role scope is not standard of OpendIdConnect. So when a client asks for this scope, the defined claims for this scope need to be returned.
```

            new IdentityResource("roles", "Your role(s)", new [] { "roles" }),
```
3. Add roles to the allowed scope list of the client application:
```
  AllowedScopes = 
  { 
      IdentityServerConstants.StandardScopes.OpenId,
      IdentityServerConstants.StandardScopes.Profile,
      "roles",
  },
```
4. On the client app ask for this additional scope and appkly the mapping from the claim to the claims identity:
```
options.Scope.Add("roles");
    options.ClaimActions.MapJsonKey("role", "role");
```