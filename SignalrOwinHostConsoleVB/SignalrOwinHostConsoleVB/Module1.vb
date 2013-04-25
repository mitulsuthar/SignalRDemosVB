Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hubs
Imports Microsoft.Owin.Hosting
Imports Owin
Module Module1
    Sub Main()
        Dim url As String = "http://localhost:8080/"
        Using WebApplication.Start(Of Startup)(url)
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Server running on {0}", url)
            Console.WriteLine("Press any key to start sending events to connected clients")
            Console.ReadLine()
            Dim context As IHubContext = GlobalHost.ConnectionManager.GetHubContext(Of MyHub)()
            For x As Integer = 0 To 100
                System.Threading.Thread.Sleep(3000)

                Console.WriteLine("Server Sending Value to Client X: " + x.ToString())
                context.Clients.All.addMessage(x.ToString())
            Next
            Console.ReadLine()
        End Using
    End Sub
    Public Class Startup
        Public Sub Configuration(ByVal app As IAppBuilder)
            Dim config = New HubConfiguration With {.EnableCrossDomain = True}
            app.MapHubs(config)
        End Sub
    End Class
    <HubName("myHub")> _
    Public Class MyHub
        Inherits Hub
        Public Sub Chatter(param As String)
            Console.WriteLine(param)
            Clients.All.addMessage(param)
        End Sub
    End Class
End Module