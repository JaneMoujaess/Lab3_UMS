# Lab3_UMS

## Lab3 project with complete functionalities ##

-View courses (anyone can view courses as long as they are signed in). Implemented Odata for added functionality and custom querying.

**ADMIN:**\
-Create courses.\
-Get common students to 2 teachers (I ended up optimizing the inner join query).\
(PS: I didn't have the time to relook into the gender based distribution of the courses).

**STUDENT:**\
-Enroll in class (teacherPerCourse) given the id of a class.

**TEACHER:**\
-Create time slots to manage availability.\
-Teach courses (basically assign a course to a time slot using their respective ids).


## Multitenancy ##
-Shared database, shared schema approach (added a branchTenantId table to the database and added the necessary foreign keys referencing it to the relative tables).\
-Above mentioned methods are only valid for users' specific tenantIds (branchIds).\
ex: Admins of branch 2 can only create courses for branch 2. Similarly, students of branch 1 are only capable of enrolling in courses of branch 1, etc.

## Authentication and Authorizatiom ##
*User is able to signup/signin through Firebase.\
*Authentication/Authorization handled by JWT with custom claim (userID) that allows:\
-the implementation of multitenancy based on that user's tenant.\
-Role based access based on that user's role and managed by custom policies.\
Also note that retrieval of userID is implemented in a middleware to ensure correct interception of requests.

## Microservices implementation ##
This application functions as a publisher. Anytime a student enrolls in a course, a Notification object is sent to the queue. The handling of the notification is the responsibility of another application (Lab3_UMS_Consumer).\
The consumer is listening to the event of publishing to the queue. Only when that event is triggered does the consumer add the notification object accordingly to a separate NotificationCenter database.

## Loosely coupled application ##
Everything implemented so far allows for a loosely coupled application (services, CQRS, microservices).

## Additional features ##
*Global exception handling implemented.\
*Custom Exceptions with appropriate status codes and messages.
