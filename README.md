# Security Identity

A simple ASP.NET Core Identity solution which provides Authentication, Authorization and Facebook Login as external provider.

### Prerequisites

```
.NET Core Environment (ASP.NET Core 2.0, SQL Server-Connection)
```

```
Facebook APP (App-ID, App-Secret)
```

## Description

What I would like to express with this topic name is the interplay between security and identity. Microsoft delivers a huge amount of possibilities with ASP.NET Core (at this time 2.*.*) how to add an identity management to your web app. The modularity of ASP.NET Core and its Middleware components makes it rather simple to extent your web app with existing OAuth providers like Microsoft, Google, Facebook and so on. Additionally, you can easily use the given possibilities to add an individual identity management, which means you can define custom properties of your users and their roles and a lot more. Furthermore, you can decide by your own how and where should the user data been saved. So, it would be possible that an existing user which signed up about a third-party provider could extend his account information with an app specific password too.

But no identity strategy performs without the essential security issues. JWT is popular and great way to handle identity between client and server in SPA applications or RESTful scenarios used by enterprise solutions. Cookies which are used for web apps too, because the browser handle cookies fully automatically on the client side, too. But if the communication is vulnerable it would be possible to steal sensitive user information’s up to session hijacking. Everybody knows that apps which handles sensitive user information´s must be secured over a valid HTTPS communication. Conscientious people would look up at the Microsoft documentation and enforce SSL on their web app too. Requiring HTTPS over the whole app is a security best practice. Additionally, you should set the well-known so called HSTS “HTTP Security Transport Strict” to prevent protocol downgrade attacks. The modularity friendly ASP.NET Core, as said before, can easily extended with additionally security middleware components. A lightweight and very good middleware as presented by **Nate Barbettini** on the Microsoft Connect(); 2017 is NWebSec, available as nuget package. Avoid implementing such security middleware’s by your own except you know what you do. In the NWebSec package there are many more security headers implemented, as middleware’s, and you should set it up by your own solution. Trying to minimize the amount of information you give out about your server is always a good idea (e.g. delete the HTTP-Header “Server”). You could double-check your current security state online at securityheaders.io under the prerequisite that your website must be reachable over the web. 
There are many other issues your web app must avoid too like XSS, Redirect Vulnerability and so on. [OWASP](https://www.owasp.org) will give you a very good overview too. To cut a long matter short, you have to look beyond the end not just extend your web app about an arbitrary identity management.


## Authors

* **Nate Barbettini** - *Inspired* - [Little-AspNetCore-Todo](https://github.com/nbarbettini/little-aspnetcore-todo)
* **Simon Planberger** - *Initial* - [Security Identity](https://github.com/planbsim/aspnetcore-authentication)

See also the list of [contributors](https://github.com/planbsim/aspnetcore-authentication/graphs/contributors) who participated in this project.

## Acknowledgments

* Inspiration


