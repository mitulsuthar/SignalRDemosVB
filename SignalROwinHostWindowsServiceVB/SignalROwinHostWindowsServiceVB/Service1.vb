Imports System.Threading
Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hubs
Imports Microsoft.Owin.Hosting
Imports Owin

Public Class Service1
    Private stopping As Boolean
    Private stoppedEvent As ManualResetEvent
    'global variable context to update clients from everywhere in the service.
    Dim context As IHubContext = GlobalHost.ConnectionManager.GetHubContext(Of MyHub)()
    Dim url As String = "http://localhost:8080"
    Public Sub New()
        InitializeComponent()

        Me.stopping = False
        Me.stoppedEvent = New ManualResetEvent(False)
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Log a service start message to the Application log.
        Me.EventLog1.WriteEntry("Service is in OnStart.")
        Dim url As String = "http://localhost:8080/"
        Using WebApplication.Start(Of Startup)(url)
            
        End Using
        ' Queue the main service function for execution in a worker thread.
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ServiceWorkerThread))
    End Sub


    ''' <summary>
    ''' The method performs the main function of the service. It runs on a 
    ''' thread pool worker thread.
    ''' </summary>
    ''' <param name="state"></param>
    Private Sub ServiceWorkerThread(ByVal state As Object)
        ' Periodically check if the service is stopping.
        Do While Not Me.stopping
            ' Perform main service function here...
            Dim context As IHubContext = GlobalHost.ConnectionManager.GetHubContext(Of MyHub)()
            For x As Integer = 0 To 100
                System.Threading.Thread.Sleep(3000)
                context.Clients.All.addMessage(x.ToString())
            Next
            Thread.Sleep(2000)  ' Simulate some lengthy operations.
        Loop

        ' Signal the stopped event.
        Me.stoppedEvent.Set()
    End Sub

    Protected Overrides Sub OnStop()
        ' Log a service stop message to the Application log.
        Me.EventLog1.WriteEntry("Service is in OnStop.")

        ' Indicate that the service is stopping and wait for the finish of 
        ' the main service function (ServiceWorkerThread).
        Me.stopping = True
        Me.stoppedEvent.WaitOne()
    End Sub
End Class
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