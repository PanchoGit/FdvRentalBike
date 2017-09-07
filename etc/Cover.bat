..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"..\packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe" -targetargs:"..\src\FdvRentalBike.UnitTest\bin\Debug\FdvRentalBike.UnitTest.dll" -filter:"+[FdvRentalBike.Workflow*]* -[*]*Resource"

..\packages\ReportGenerator.2.5.11\tools\ReportGenerator.exe -reports:results.xml -targetdir:GeneratedReports