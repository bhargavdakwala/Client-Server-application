# Getting Started.

Due to time constraint, here I have used minimal toolstack. For advance level we can use gRPC and front-end react.js technology and for TDD we can include unit test project.
1) I am using Window application with ".NET Core 3.1" and Web application using ".NET Framwork 4.7.2".
2) Both application is using TcpClient and NetworkStream for sending and received messages.
3) Application works in two direction.
	a) Web Application(Server) to Window Application(Client) : In order to Data synchronization from Web application(Server) to Window application(client) once data is submitted through TextBox please click on "Refresh" button in Window application(Client).
	b) "Refresh Data" is for Data synchronization.
	c) Window Application(Client) to Web Application(Server) : Before click on "Sending" button in window application - Click on "Refresh" button in Web Application(Server).
	


# Window Application(Client):

1) Window application build on ".NET Core 3.1".
2) Window application(client) also send and received text message.

# Web Application(Server):

1) First Run web application and then type message in textbox.
2) Click on Submit button and run window application.
3) Once run window application then it will received message and insert message into SQLlite database.
4) Click on "Refresh Data" button and it will show you send and received data list with time.
5) Web application build in ".NET Framwork 4.7.2".
6) Here we have used PORT NO = "5000" and SERVER_IP = "127.0.0.1". You can change accordingly.
7) I have used TcpClient and NetworkStream for sending and received messages.


#Database

1) I have used SQLite database for storing data.

# .NET and other dll used in Backend 
 
- Net Core 3.1
- SQLlite
- C#
- Bootstrap
- Logger for Error Handling


# other information for configuration
