# AspNetCoreWebApi

This web api was started as the template for web APIs in visual studio.
The only nugget package that gets imported out-of-the box is the Swashbuckler,
so we use the open API standard to document the API (swagger).
It is basically a new endpoint with a web ui documenting all the enpoints of an api.

_LaunchSettings.json_ is an important file in the template. It is used to setup the local environment for development.
It is not deployed and it contains profile settings.
An http profile and a https profile that is used by kestrel server to launch the project.
Another profile is IIS server to launch the project using the iis.
The profile dictates the url the profile runs on and the port.

## Solution structure

_AppSettings.json_ contains application settings with some default configuration, like logging. By default, it also create AppSettings.Development.json that allows us to override some default settings just for our development environment.
We have a Controllers folder with a default controller (part of the mvc part).
CityInfo.API.http can be removed. It is used to test the api but we can do that with postman.
Program.cs contains the startup the application, in this case a web application that needs to be hosted.
The builder can be used for that. It is here that dependency injection gets configured as well.
The last section configures the API middlewares.
The way the middleware gets configured dictates how the requests get processed.
The middleware added result in a request pipeline. The order on which these middleware are added matters:
![](requestPipeline.png)

The result of the middleware processing is a response.
Middleware that can be added are, for example, authentication middlewares or diagnostics middlewares.
Each component in the pipeline chooses to pass the request to the next component on the pipeline or not.
On example of that is the authentication middleware. If authentication is not successful, the request does not get processed.
In _Program.cs_ we also observe code that checks on which environment the application is running.
Asp.Net, by default, contains the environments: Development, Staging and Production.
The application host provides a way of accessing the application environment.
The profile dictates the url and port the application runs on.

## MVC

Mode view controller is the most common used pattern to develop web apis.
Unlike with front end applications, we do not have a view within a web api.
The MVC pattern focus heavenly in the spearation of concertes, improves testability and promotes code reuse.
MVC is the pattern used in the presentation layer, not on the other layers of the application.
Parts of MVC:

1. Model: represents application data and/or business logic and rules
1. View: representation of the data.
1. Controller: handles user input and translates that to model data the view can work with.
1. The controller chooses the view and provides it with data.

In an API our view does not exist or it is the representation of data, like json to be sent over the wire.
The MVC architecture:
![](doc/MVC.png)

We introduced our cities controller that inherits from controller base (controller has more stuff not needed in case of a web api)
THe Api controller attribute is not strictly necessary but it improves dev experience while developing APIs (errors and validations).

## Routing

Routing matches a request URI to an action on a controller.
The prefered way to setup this on ASP.net is to use endpoit routing.
Endpoint routing can be configured in the middleware.
We can inject pieces of middleware that know which enpoint to select.
There are two ways of mapping endponits: convention based or attribute based. Attribute base is the preferred way.
By calling MapControllers no conventions are applied and we can just use attributes no map actions:
![](doc/attribute%20routing.png)

The Route attribute can be applied at the controller level class, which defines a base route to the controller itself and avoids some repetition on all actions.
If we use [Route("api/[Controller]")] we basically map the url to the controller name. In our case the cities controller will have the url .../api/cities.
Be careful with this approach because renaming the controller would change it to the clients.
Each controller action can have the more specific URL parts and parameters. Parameters are used in atributtes with curly braces: "{cityId}"

## Status Codes

Status codes are a very important piece of public APIs.
Status codes are the only thing the client has in order to know if the request worked out as expected.
200 Code means the request went ok. Sometimes is common for people to send 200 if something went wrong (like the routing was ok but an entity was not found).
500 means internal server error. Do not use this one if the client made a mistake.
There are several levels of status codes:

1. Level 100: international status codes, almost never used. Pure informational
1. Level 200: success status codes.
   1.200: success request
   1.201: resource created
   1.204: no content. Example delete: it was successfull but returned no content.
1. Level 300: redirection status codes.
1. Level 400: client mistakes.
   1. Bad request: 400
   1. 401: Unauthorized. No Authentication
   1. 403: Forbidden. Authentication succeded but authorization did not.
   1. 404: requested resource
   1. 409: conflict. This is meant for scenarios where concurrency issues happen (two parallel requests updating the same resource at the same time).
1. Level 500: server mistakes

Asp.net core has a few helper methods to allow us to return responses with the proper error codoes in case of errors or successfull responses.
Responses are returned as _ActionResults_ or _ActionResults<T>_.
But what to do with child resources?

In our example, we have points of interest that are dependent on the city objects: there is not a point of interest without a city.
Notice the route of the point of interest controller. Because it makes no sense to have a point of interest without a city, the points of interest controller reflects that on its URL.
The base route of the controller itself is already reflecting that by asking for a city id.

We can also add additional information or details to problems we encounter. We can do that by using _Builder.Services.AddProblemDetails()_.
This is suitable for most cases.
In this example we added server name that can be usefull if we are executing in a multi server environment.

## Content negotiation

Content negotiation is the selection of the best representation for a given response when there are multiple representations available.
When we develop a public API, not all clients might be able to deal with a json representation, for example. They might request a xml representation.
It comes in the request headers as the media type: application/json, application/xml, etc:
![](doc/askforxmlcontent.png)

Asp.net supports that via output formatters.
The support is implemented by ObjectResult. All action results inherit from ObjectResult. Our models get wrapped by these objects that support content negotiation. We just need to worry about configuring the correct formatter.
The default is json formatter.
We can configure and change the default output formatter. When we add controllers, if we specify several output formatters, the default one is the one that was added first.
The services collection has some methods to add the most standard output formatters out of the box to the IoC container.
The same can be said about input formaters.

In the Ioc container, where we the _AddControllers_ method, we can setup the option _ReturnHttpNotAcceptable_ to true. This will return an error code 406 telling the client that the request is not acceptable (if the client in the headers asks form xml and the api does not support it for example).
See file content as well.

## Manipulating resources

Data can be passed to an API by various means. Via query parameters or via request bodies.
We can apply binding source attributes that tell the model binding engine where to find the binding source. They tell ASP.Net core where to find a certain value.
Multiple such attributes exist:

1. [FromBody] : request body
1. [FromForm] : form in the request body.
1. [FromHeader] : request header
1. [FromQuery]: query string parameters
1. [FromRoute]: Route data from the current request.
1. [FromServices]: services injected as action parameters
1. [AsParameters]: method parameters

By default, the asp.net core runtime attempts to use the complex object model binder when there are no attributes, which pull data from value providers in a defined order.
When using the [ApiController] attributte [FromBody] is inferred for complex types.
[FromForm] is inferred for action paramters of type IFormFile and IFormFileCollection.
[FromRoute] inferred for any action parameter name, macthing a parameter in the route template.
[FromQuery] inferred for any action paremeters.

Binding source attributtes can be then applied when there is the need to override default behavior.
In our _Post_, in the points of interest controller, a few key things to take a look at:

We return a _CreatedAtRoutResult_. When we return this type, we send the route at which this object can be fetched by using the http get method.
It basically returns in the reponse header a location: the url of the object we've just created:
![](doc/requestResponsePostmanHeaders.png)

Look also in the request header now, the content type. It tells the API how to deserialize the object.

## Validate input

When using the _ApiController_ attribute at controller level, asp.net returns error code 400 if validation fails.
Asp.net returns 400 if the request body is empty automatically.
Other custom validations can be made with data annotations and fluent validation.
Data annotations are attributes and asp.net provides attributes for very common validations, like email, phone, number ranges, required values, etc.
The use of data annotations is ok for simple use cases.
For big applications Fluent validation must be considered: annotations mix rules with models, so it does not follow the principle of a good separation of concerns.

## Updating a resource

There are full updates or partial updates.
Full updates are done with HTTP put and partial updates with HTTP Patch and json documents.
Put means all fields of the objects will be updated, patch means we can send only the change set over the wire.
The patch needs to contain the fields that need to be updated and its values.

To check: json Patch (RFC 6902): describes a document structure for expressing a sequence of operations to apply to a json document.
It will list all the properties that need update and the values.

When doing the patch we need to pass the model state and verify manually if the model state is valid after applying the patch document.
The document might have properties that are not in our model for example.

## Logging and exception handling

Loggers can be injected via dependency injection. Logging providers can be configured to write logs into a file, to azure, etc. By default asp.net loggs to the console. Logging configurations are present in the _appsettings.json_ and can be configured per environment.

Usually the prefered approach is to handle exceptions in a central place.
Asp.net core by default, in the development environment, enables the development exception page.
The developer can see the exception and the stack trace in the browser. CAREFUL: never send the exception to the client, in other environments, it exposes implementation details to the consumers, that can use that information for attacks.

In other environments, besides the development, we can add the exception handling middleware (for that we need to add problem detials too). It makes sense to build a user friendly errors response by using the probem details object seen before. In front-end apps, it makes sense to build a nice user friendly error page.

Plenty of log providers (NLog, erilog, etc). They implement the ILogger interface. Adding loggers to our controllers and services we just need to inject ILogger<T>, being T the component type.

Other custom services can be added via dependency injection as well. There are several lifetimes:

1. Scoped: an instance per request
1. Transient: an new instance every time it is requested
1. Singleton: an instance for the whole application

Dependency injection is a broader topic, that deserves its repository and discussion of its own.

## Configuration

By default, the asp.net creates the file _appSettings.json_. This configuration is available as key value pairs in the application, by injecting the interface IConfiguration where needed. There is also the option patterns that is worthy a check.
In our case we introduced there default email settings.
We can vary our configuration based on the environement we are running the app. By default, asp.net core creates a second app settings file called: appSettings.Development.json, for the development environment. We can add more files to staging and production environments like: appSettings.Staging.json, etc.
The appSettings.json has the default values, that can be ovverriden in the environment specific files. So the environment specific files do not need all the configuration values in them.
In this case we changed the email address to where we send the point of interest deleted.

## Models

Models represent data and behavior. Data is stored in a database in form of tables which is not very object oriented friendly. It is then very common to use ORMa (Object-Relational Mapping) that let us manipulate data from a database using an object-oriented paradigm and fetch the data as entities, or, our models. We can work with object instead of sql statements.
Within the .net realm, EF core is a widely used ORM that supports SQL Server, Postgres, etc.

Our entitties can use data annotation attributes to properties that can tell EF if a property is for example a key. If the property is name "Id" EF core also recognizes it as a key, due to the naming conventions in place.
In our example we use data annotations, but fluent APIs are also possible to achieve the same results. Data annotations configure the database constraints as well: if we do not specify a max lenght of a string property,the database will use varchar max instead of limiting the column size.
The DTOs do not have some of these annotations, because they are not needed to store the data in the database.

This is a web api trainning, so in order to dig deep into EF go here:.
A note on returning objects: APis usually do not return the same entities as they are in its data store.
Usually, Data Transfer Objects (DTOs), are returned. In this demo we introduced the AutoMapper to map entities to DTOS and the repository pattern, to abstract the DBContext and the data access completely. Here are the advantages of the repository pattern:
![](doc/repositoryPattern.png)

Some notes on gettings resources: _IActionResult_ is more appropiate when more than one Dto type can be return. Check the GetCityById with or without points of interests (dtos are different).

## Security

On the demo of last sections, we've just seen how to configure EF core with SQL Lite. In order to connect to a database, we need a connection string. Connections strings are considered sensitive data.
We may be tempted to store such values as configuration data in the AppSettings. That is ok for development environments, but not ok at all for production environments because connection strings have server names, combinations of user and password, etc.
There are several safe locations: azure key vault (tto far for thos course), environment variable.
Environment variable is a variable is a value that is set outside of the program, but on the operating system (so never on the source control system).
Imagining our windows laptop is the production server we can setup an environment variable there. Go to windows menu -> Edit System Variables -> NewSystemVariable.
Visual studio overrides appSettings with environment variables. So in order to keep developing we need to remove it.

SQL injection is an attack in which malicious code is inserted into strings that are later passed to an instance of a database server for parsing and execution.

Here is an example, where a client sends this string to filter for city name:
![](doc/sqlinjection.png)

That gets translated into two sql statements, as we can see. The script is dropping a table, meaning the app would loose data.
Also, other statements could be executed, like query sensitive user data.
Code that constructs SQL statements should be reviewed for injection vulnerabilities.
The best way to protect agains SQL injection is by encapsulating and parameterizing SQL commands. This way the input text is treated as paremeter and not a SQL command:
![](doc/sqlinjectionsolved.png)
EF core gives almost this out of the box. Most of the queries are done as linq statements.

Attention in creating files on the server.
