# FunFox Portal Development Overview:
Utilizing Visual Studio 2022 and powered by C# with dotnet core 8.0, the FunFox Portal was meticulously crafted within the given timeframe. Microsoft SQL Server 2019 serves as the sturdy backbone for data storage, seamlessly integrated with the lightweight ORM, Dapper.
The portal is structured into three distinct components, each meticulously designed for optimal functionality. 
The foundational 
# DataAccess 
module, housed within a class library, harnesses the efficiency of Dapper for nearly all database operations. With just two methods, it handles approximately 95% of database tasks, ensuring streamlined performance.
# The WebApi component(FunFoxApi), 
fortified with Microsoft Entity for endpoint security, stands as the gateway to the portal's functionalities. Endpoints are rigorously protected, with role-based access control ensuring that only authorized personnel can interact with sensitive areas. Administrators wield exclusive access to select endpoints, while authenticated users navigate through permitted sections. For newcomers, seamless signup and login processes await.
# Dotnet core Razor MVC web application(FunFoxApp)
A Razor MVC web application using dotnet core which is very simple but serves as a User Interface to consume all operations developed in **WebApi** . Its authentication mechanism is based on bearer tokens which identifies who is calling secure api.
Design of application is developed in modular approach so that any enhancements and feature change should not disturb other modules of application. E.g we can right away start writing its UI for Mobile applications we will require no change in API and class library. This web application currently developed is a demo and does not adhere to best practices.    
This collaborative effort marries cutting-edge technology with meticulous attention to detail, delivering a dynamic and secure FunFox Portal experience.



