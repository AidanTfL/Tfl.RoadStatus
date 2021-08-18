Feature: GetRoadStatus
Returns the Road Status for one or more specified major roads using real TfL open data feeds.

# These Acceptance tests use the real Tfl Api. You must have a working internet connection to run these.

# Also, if it's your first time running these tests, you'll need to activate Specflow:
# https://stackoverflow.com/questions/68388780/exception-specflow-plus-shared-services-activation-activationexception

@HappyPath
Scenario: OutputsRoadDisplayName
	Given a valid roadID <roadID> is specified
	When the client is run
	Then the road displayName <displayName> should be displayed

	Examples:
		| roadID						| displayName               |
		| A1							| A1                        |
		| A10							| A10                       |
		| A12							| A12                       |
		| A13							| A13                       |
		| A2							| A2                        |
		| A20							| A20                       |
		| A205							| South Circular (A205)     |
		| A21							| A21                       |
		| A23							| A23                       |
		| A24							| A24                       |
		| A3							| A3                        |
		| A316							| A316                      |
		| A4							| A4                        |
		| A40							| A40                       |
		| A406							| North Circular (A406)     |
		| A41							| A41                       |
		| bishopsgate%20cross%20route	| Bishopsgate Cross Route   |
		| blackwall%20tunnel			| Blackwall Tunnel          |
		| city%20route					| City Route                |
		| farringdon%20cross%20route	| Farringdon Cross Route    |
		| inner%20ring					| Inner Ring                |
		| southern%20river%20route		| Southern River Route      |
		| western%20cross%20route		| Western Cross Route		|


Scenario: OutputsRoadStatusSeverity
	Given a valid roadID <roadID> is specified
	When the client is run
	Then the road statusSeverity <statusSeverity> should be displayed
	
	Examples:
		| roadID						| statusSeverity	|
		| A1							| regex not empty	|
		| A10							| regex not empty	|
		| A12							| regex not empty	|
		| A13							| regex not empty	|
		| A2							| regex not empty	|
		| A20							| regex not empty	|
		| A205							| regex not empty	|
		| A21							| regex not empty	|
		| A23							| regex not empty	|
		| A24							| regex not empty	|
		| A3							| regex not empty	|
		| A316							| regex not empty	|
		| A4							| regex not empty	|
		| A40							| regex not empty	|
		| A406							| regex not empty	|
		| A41							| regex not empty	|
		| bishopsgate%20cross%20route	| regex not empty	|
		| blackwall%20tunnel			| regex not empty	|
		| city%20route					| regex not empty	|
		| farringdon%20cross%20route	| regex not empty	|
		| inner%20ring					| regex not empty	|
		| southern%20river%20route		| regex not empty	|
		| western%20cross%20route		| regex not empty	|
												   

Scenario: OutputsRoadStatusSeverityDescription
	Given a valid roadID <roadID> is specified
	When the client is run
	Then the road statusSeverityDescription <statusSeverityDescription> should be displayed

	Examples:
		| roadID						| statusSeverityDescription |
		| A1							| regex not empty           |
		| A10							| regex not empty           |
		| A12							| regex not empty           |
		| A13							| regex not empty           |
		| A2							| regex not empty           |
		| A20							| regex not empty           |
		| A205							| regex not empty           |
		| A21							| regex not empty           |
		| A23							| regex not empty           |
		| A24							| regex not empty           |
		| A3							| regex not empty           |
		| A316							| regex not empty           |
		| A4							| regex not empty           |
		| A40							| regex not empty           |
		| A406							| regex not empty           |
		| A41							| regex not empty           |
		| bishopsgate%20cross%20route	| regex not empty           |
		| blackwall%20tunnel			| regex not empty           |
		| city%20route					| regex not empty           |
		| farringdon%20cross%20route	| regex not empty           |
		| inner%20ring					| regex not empty           |
		| southern%20river%20route		| regex not empty           |
		| western%20cross%20route		| regex not empty           |


Scenario: OutputsInformativeErrors
	Given an invalid roadID <roadID> is specified
	When the client is run
	Then the application should return an informative error <error>

	Examples:
		| roadID				| error |
		|						| TfL.RoadStatus.ConsoleUI 1.0.0			|
		| RoadWithoutTraffic	| RoadWithoutTraffic is not a valid road	|


Scenario: TerminatesWithNonZeroExitCode
	Given an invalid roadID <roadID> is specified
	When the client is run
	Then the application should exit with a non-zero System Error code <errorCode>

	Examples:
		| roadID				| errorCode  |
		|						| 2			 |
		| RoadWithoutTraffic	| 1			 |