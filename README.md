SwaggerBasicAuth

Recently, I had a chance to work on a new ASP.NET Core web API project from scratch, and I have integrated Swagger for API documentation using Swashbuckle.AspNetCore library.
I've tried to secure access to Swagger UI page ("/swagger") and Swagger document endpoint ("/swagger/v1/swagger.json"), and I haven't found any "out of the box" way of doing that, so I wrote simple 
middleware for securing those urls using basic http authentication.

Core files

- SwaggerBasicAuthMiddleware.cs
	- core logic for performing basic http authentication if requests are for Swagger related urls
- SwaggerBasicAuthExtensions.cs 
	- extension method used for middleware registration and configuration
- SwaggerBasicAuthConfiguration.cs
	- configuration object that contains swagger url and credentials for authentication
