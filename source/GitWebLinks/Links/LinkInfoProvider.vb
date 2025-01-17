Imports EnvDTE


Public Class LinkInfoProvider
    Implements IAsyncInitializable
    Implements ILinkInfoProvider


    Private cgDte As DTE
    Private cgLinkInfoFinder As ILinkInfoFinder
    Private cgSolutionEvents As SolutionEvents
    Private cgCurrentLinkInfo As LinkInfo


    Public Async Function InitializeAsync(provider As IAsyncServiceProvider) As Threading.Tasks.Task _
        Implements IAsyncInitializable.InitializeAsync

        cgDte = Await provider.GetServiceAsync(Of DTE)()
        cgLinkInfoFinder = Await provider.GetServiceAsync(Of ILinkInfoFinder)()
        cgSolutionEvents = cgDte.Events.SolutionEvents

        AddHandler cgSolutionEvents.AfterClosing, AddressOf OnSolutionClosed
        AddHandler cgSolutionEvents.Opened, AddressOf OnSolutionOpened

        If cgDte.Solution.IsOpen Then
            OnSolutionOpened()
        End If
    End Function


    Private Sub OnSolutionOpened()
        ' When a new solution is created this event is raised, but the solution doesn't have a file 
        ' name at that stage, so we can't get the link info for it. But, once the project is fully 
        ' created, this event is raised again, and we can get the link info at that point.
        If Not String.IsNullOrEmpty(cgDte.Solution?.FullName) Then
            cgCurrentLinkInfo = cgLinkInfoFinder.Find(IO.Path.GetDirectoryName(cgDte.Solution.FullName))
        End If
    End Sub


    Private Sub OnSolutionClosed()
        cgCurrentLinkInfo = Nothing
    End Sub


    Public ReadOnly Property LinkInfo As LinkInfo _
        Implements ILinkInfoProvider.LinkInfo

        Get
            Return cgCurrentLinkInfo
        End Get
    End Property

End Class
