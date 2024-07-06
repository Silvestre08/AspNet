# AspNetCoreWebApi

This web api was started as the template for web APIs in visual studio.
The only nugget package that gets imported out-of-the box is the Swashbuckler,
so we use the open API standard to document the API (swagger).

LaunchSettings.json is an important file. It is used to setup the local environment for development.
It is not deployed and it contains profile settings.
An http profile and a https profile that is used by kestrel server to launch the project.
Another profile is IIS server to launch using the iis.
The profile is dictate the url the profile runs and the port.

## Solution structure

App settings.json contains application settings as default just some level configuration
Controllers (part of the mvc part)
CityInfo.API.http can be removed. It is used to test the api but we can do that with postman.
Program.cs contains the startup the application, in this case a web application that needs to be hosted.
The builder can be used from that. It is here that dependency injection gets configured.
The last section configures the API middlewares.
The way the middleware gets configured dictates how the requests get processed.
The middleware added result in a request pipeline. The order on which these middleware are added matters:
![](requestPipeline.png)
The result of the middleware processing is a response.
Middleware that can be added are for example authentication middlewares or diagnostics.
Each component in the pipeline chooses to pass the request to the next component on the pipeline or not.
On example of that is the authentication middleware. If authentication is not successful, the request dooes not get processed.

In program cs we also observe code that checks on which environment the application is running.
the default environments are Development,Staging and Production.
The application host provides a way of accessing the application environment
The profile dictates the url and port the application runs on.

# MVC

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

Picture:

We introduce our cities controller that inherits from controller base (controller has more stuff not needed in case of a web api.)
THe Api controller attribute is not strictly necessary but it improves dev experience while developing APIs (errors and validations)

Routing
Routing matches a request URI to an action on a controller.
The prefered way to setup this on ASP.net is to use endpoit routing.
Endpoint routing can be configured in the middleware.
We can inject pieces of middleware that know which enpoint to select.
There are two ways of mapping endponits: convention based or attribute based. Attribute base is the preferred way.
By calling MapControllers no conventions are applied and we can just use attributes no map actions. Attributes:
pi.

The Route attribute can be applied at the controller level class, which defines a base route to the controller itself and avoids some repetition on all actions.
if we use [Route("api/[Controller]")] we basically map the url to the controller name. In our case the cities controller will have the url .../api/cities.
Be careful with this approach because renaming the controller would change it to the clients.
Each controller action can specific the aditional URL parts and parameters. Parameters are used in atributtes with curly braces.
Pic

# Status Codes

Status codes are a very important piece of public APIs.
Status codes are the only thing the client has in order to know if the request worked out as expected.
200 Code means the request went ok. Sometimes is common for people to send 200 if something went wrong (like the routing was ok but an entity was not found)
500 means internal server error. Do not use this one if the client made a mistake.
There are several level of status codes:

1. Level 100: international status codes, almos never used. Informational
1. Level 200: success status codes.
   1.200: bd request
   1.201: resource created
   1.204: no content. Example delete: it was successfull but returned no content.
1. Level 300: redirection status codes.
1. Level 400: client mistakes.
   1. Bad request: 400
   1. 401: Unauthorized. No Authentication
   1. 403: Forbidden. Authentication succeded bit authorization did not.
   1. 404: requested resource
   1. 409: conflict. This is meant for scenarios where concurrency issues happen (two parallel requests updating the same resource at the same time)
1. Level 500: server mistakes

Asp.net core has a few helper methods to allow us to return responses with the proper error codoes in case of errors or successfull responses.
Responses are returned as ActionResults or ActionResults<T>.
But what to do with child resources?

In our example we have points of interest that are dependent on the city objects: there is not point of interest without a city.
Notice the rout of the point of interest controller. Because it makes no sense to have a point of interest without a city, the points of interest controller reflects that on its URL.
The base route of the controller itself is already reflecting that.

We can also add additional information or details to problems we encounter. We can do that by using Builder.Services.AddProblemDetails().
This is suitable for most cases.
In this example we added server name that can be usefull if we are executing in a multi server environment.

# Content negotiation

Content negotiation is the selection of the best representation for a given response when there are multiple representations available.
When we develop a public API, not all clients might be able to deal with a json representation, for example. They might request a xml representation and so on.
It comes in the request headers as the media type: application/json, application/xml, etc.
Asp.net supports that via output formatters.
The support is implemented by ObjectResult. All action results inherit from ObjectResult. Our models get wrapped by these objects that support content negotiation. We just need to worry about configuring the corret formater.
The default is json formater.
We can configure and change the default output formatter. When we add controllers, if we specify several output formatters, the default one is the one that was added first.
The services collection has some methods to add the most standard input formatters out of the box to the IoC container.
The same as input formaters.

In the Ioc container in the add controller methods we can setup the option ReturnHttpNotAcceptable to true. This will return an error code 406 telling the client that the request is not acceptable (if the client in the headers asks form xml and the api does not support it for example)

See file content as well.

# Models

APis usually do not return the same entities as they are in its data store.
Usually, Data Transfer Objects (DTOs), are returned.

# Manipulating resources

Data can be passed to an API by various means. Via query parameters or via request bodies.
We can apply binding source attributes that tell the model binding engine where to find the binding source. They tell ASP.Net core where to find a certain value.
Multiple such attributes exist:

1. [FromBody] : request body
1. [FromForm] : form daara in the request body.
1. [FromHeader] : request header
1. [FromQuery]: query string parameters
1. [FromRoute]: Route data from the current request.
1. [FromServices]: services injected as action parameters
1. [AsParameters]: method parameters

By default, the asp.net core runtime attempts to use the complex object model binder when there are no attributes, which pull data from value providers in a defined order.
When using the [ApiController] attributte [FromBody] is inferred for complex types.
[FromForm] is inferred for action paramters of type IFormFile and IFormFileCollection.
[FromRoute] inferred for any action parameter name, macthing a parameter in the route template.
[FromQuery] inferred for any action paremeters

Binding source attributtes can be then applied when there is the need to override default behavior.
In our post in the points of interest controller, a few key things to take a look at:

we return a CreatedAtRoutResult. When we return this type, we send the route at which this object can be fetched by using the http get method.
It basically returns in the reponse header a location: the url of the object we've just created.
pic: headers
Look also in the request header now, the content type. It tells the API how to deserialize the object.

# Validate input

When using the ApiController attribute at controller level, asp.net returns error code 400 if validation fails.
Asp.net returns 400 if the request body is empty automatically.
Other custom validations can be made with data annotations nd fluent validation.
Data annotations are attributes and asp.net provides attributes for very common validations, like email, phone, number ranges, required values, etc
The use of data annotations is ok for simple use cases.
For big applications Fluent validation must be considered, annotations mix rules with models, so no follow good separation of concerns.

# Updating a resource

There are full updates or partial updates.
Full updates are done with HTTP put and partical updates with the patch and json documents. Put means all fields of the objects will be updated, patch means we can send only the change set over the wire.
The patch needs to contain the fields that need to be updated and its values.

To check: json Patch (RFC 6902): describes a document structure for expressing a sequence of operations to apply to a json document.
It will list all the properties that need update and the values.

When doing the patch we need to pass the model state and verify manually if the model state is valid after applying the patch document.
The document might have properties that are not in our model for example.

# Logging and exception handling

loggers can be injected via dependency injection. Logging providers can be configured to write logs into a file, to azure, etc. By default asp.net loggs to the console. Logging configurations are present in the appsettings.json and can be configured per environment.

Usually the prefered approach is to handle exceptions in a central place.
Asp.net core by default, in the development environment, enables the development exception page.
the developer can see the exception and the stack trace. CAREFUL: never send the exception to the client, in other environments, it exposes implementation details.

for other environments, besides the development, we can add the exception handling middleware (for that we need to add problem detials to). It makes sense to build a user friendly errors response by using the probem details object seen before. In front-end apps, it makes sense to build a nice user friendly error page.

Plenty of log providers (NLog, erilog, etc)

Other custom services can be added via dependency injection as well. There are several lifetimes:

1. Scoped: an instance per request
1. Transient: an new instance every time it is requested
1. Singleton: an instance for the whole application

# configuration

By default, the asp.net creates the file appSettings.json. This configuration is available as key value pairs in the application, by injecting the interface IConfiguration interface where needed.
In our case we introduced there default email settings.
We can vary our configuration based on the environement we are running the app. By default, asp.net core creates a second app settings file called: appSettings.Development.json, for the development environment. We can add more files to staging and production environments like: appSettings.Staging.json, etc.
The appSettings.json has the default values, that can be ovverrided in the environment specific files. So the environment specific files do not need all the configuration values in them.
In this case we changed the email address to where we send the point of interest deleted.

# security

Attention in creating files on the server.