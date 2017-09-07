# Rental Bike Project
# 1. Design

The design has been focused on the implementation of a library that have the responsibility to store a service of a bike rental.

The library called Workflow expose a single method called Save to be used in a WebApi, Web MVC or in desktop app through MVVM.

The interface of Workflow can be used with the implementation of a injection of dependency.

The argument of the Save method is an object of type CustomerService in order to store the client and one or more bike rental type. Also a promotion for family can be specified in the argument with the related customer.

# 2. Development Practices

Following practices are used:

- DRY (don't repeat yourself)
- KISS (Keep It Simple, ...)
- SOLID (some of principles: Single responsibility, Open-closed, Liskov substitution, Interface segregation and Dependency inversion)
- Implementation oriented to TDD

In the Solution, projects folders are named with number in order to avoid circular dependency (0 cannot be referenced by 1,2,etc.)

Only one method in Workflow (Save) are used to avoid the repetition of code and an unit test can be centralized.
The entry of the workflow method are always evaluated in order to ensure the whole process.
The process apply the correct calculation based on the RentalType property of the argument.
A promotional discount are processed based on the argument sended.
The implementation of the promotion are separated of the workflow in order to flexibilize the develop of more promotion.

# 3. Test

Following batch file can be executed in order to see tests results and code coverage report.
- etc/Cover.bat

Code Coverage report are generated in following path:
- etc/GeneratedReports/index.html

Unit Test:
- src/FdvRentalBike.UnitTest
- Testing tool: Nunit & Moq

Integration Test:
- src/FdvRentalBike.IntegrationTest
- Testing tool: Nunit

# 4. Implementation
FdvRentalBike.sln -> Solutions: Visual studio 2015 or 2017
.Net Framework version: 4.6.1

