Imports System.Collections.ObjectModel
Imports System.ComponentModel


Public Class OptionsPageControlViewModel
    Implements INotifyPropertyChanged


    Private cgUseCurrentHash As Boolean
    Private cgUseCurrentBranch As Boolean


    Public Sub New()
        GitHubEnterpriseUrls = New ObservableCollection(Of ServerUrlModel)
        BitbucketServerUrls = New ObservableCollection(Of ServerUrlModel)
    End Sub


    Public Property UseCurrentHash As Boolean
        Get
            Return cgUseCurrentHash
        End Get

        Set
            If cgUseCurrentHash <> Value Then
                cgUseCurrentHash = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(UseCurrentHash)))
            End If
        End Set
    End Property


    Public Property UseCurrentBranch As Boolean
        Get
            Return cgUseCurrentBranch
        End Get

        Set
            If cgUseCurrentBranch <> Value Then
                cgUseCurrentBranch = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(UseCurrentBranch)))
            End If
        End Set
    End Property


    Public ReadOnly Property GitHubEnterpriseUrls As ObservableCollection(Of ServerUrlModel)


    Public ReadOnly Property BitbucketServerUrls As ObservableCollection(Of ServerUrlModel)


    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

End Class
