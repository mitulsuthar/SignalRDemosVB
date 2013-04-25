Imports Microsoft.AspNet.SignalR.Client.Hubs
Imports Microsoft.AspNet.SignalR
Module Module1

    Sub Main()
        Dim connection = New HubConnection("http://localhost:8080")

        Dim myHub = connection.CreateHubProxy("myHub")

        connection.Start().Wait()
        Console.ForegroundColor = ConsoleColor.Yellow
        myHub.Invoke(Of String)("chatter", "Hi!! Server") _
        .ContinueWith(
            Sub(task)
                If task.IsFaulted Then
                    Console.WriteLine("Could not Invoke the server method Chatter: {0}", _
                                      task.Exception.GetBaseException())
                Else
                    Console.WriteLine("Success calling chatter method")
                End If
            End Sub)

        myHub.On(Of String)("addMessage", _
            Sub(param)
                Console.WriteLine("Client receiving value from server: {0}", param.ToString())
            End Sub)
        Console.ReadLine()
    End Sub
End Module
