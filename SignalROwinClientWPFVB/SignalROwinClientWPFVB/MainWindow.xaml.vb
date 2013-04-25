Imports Microsoft.AspNet.SignalR.Client
Imports System.ComponentModel
Imports Microsoft.AspNet.SignalR.Client.Hubs

Partial Public Class MainWindow
    Inherits Window
    Implements INotifyPropertyChanged
    Public connection As HubConnection = New HubConnection("http://localhost:8080")
    Public myHub As IHubProxy = connection.CreateHubProxy("myHub")

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DataContext = Me
        connection = New HubConnection("http://localhost:8080")

        myHub = connection.CreateHubProxy("myHub")
        myHub.On(Of String)("addMessage", AddressOf addMessage)
    End Sub

    Async Sub btnShowSignal_Click(sender As Object, e As RoutedEventArgs)
        Await connection.Start()
        Await myHub.Invoke("Chatter", "Hello Server")
    End Sub

    Private m_updatetext As String
    Public Property UpdateText() As String
        Get
            Return m_updatetext
        End Get
        Set(ByVal value As String)
            m_updatetext = value
            RaisePropertyChanged("UpdateText")
        End Set
    End Property

    Private Sub addMessage(ByVal sValue As String)
        UpdateText += Environment.NewLine
        UpdateText += sValue
    End Sub
    Private Sub RaisePropertyChanged(prop As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class