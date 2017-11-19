Imports LibGit2Sharp
Imports System.IO

Public Class VisualStudioTeamServicesHandlerTests

    Public Class NameProperty

        <Fact()>
        Public Sub ReturnsVisualStudioTeamServices()
            Assert.Equal("Visual Studio Team Services", CreateHandler().Name)
        End Sub

    End Class


    Public Class IsMatchMethod

        <Theory()>
        <MemberData(NameOf(GetRemotes), MemberType:=GetType(VisualStudioTeamServicesHandlerTests))>
        Public Sub MatchesVisualStudioTeamServicesServers(remote As String)
            Dim handler As VisualStudioTeamServicesHandler


            handler = CreateHandler()

            Assert.True(handler.IsMatch(remote))
        End Sub


        <Fact()>
        Public Sub DoesNotMatchOtherServerUrls()
            Dim handler As VisualStudioTeamServicesHandler


            handler = CreateHandler()

            Assert.False(handler.IsMatch("https://codeplex.com/foo/bar.git"))
        End Sub

    End Class


    Public Class MakeUrlMethod

        <Theory()>
        <MemberData(NameOf(GetRemotes), MemberType:=GetType(VisualStudioTeamServicesHandlerTests))>
        Public Sub CreatesCorrectLinkFromRemoteUrl(remote As String)
            Using dir As New TempDirectory
                Dim handler As VisualStudioTeamServicesHandler
                Dim info As GitInfo
                Dim fileName As String


                info = New GitInfo(dir.FullPath, remote)
                fileName = Path.Combine(dir.FullPath, "src\file.cs")
                handler = CreateHandler()

                Using LinkHandlerHelpers.InitializeRepository(dir.FullPath)
                End Using

                Assert.Equal(
                    "https://foo.visualstudio.com/_git/MyRepo?path=%2Fsrc%2Ffile.cs&version=GBmaster",
                    handler.MakeUrl(info, fileName, Nothing)
                )
            End Using
        End Sub


        <Fact()>
        Public Sub CreatesCorrectLinkWithSingleLineSelection()
            Using dir As New TempDirectory
                Dim handler As VisualStudioTeamServicesHandler
                Dim info As GitInfo
                Dim fileName As String


                info = New GitInfo(dir.FullPath, "ssh://foo@vs-ssh.visualstudio.com:22/_ssh/MyRepo")
                fileName = Path.Combine(dir.FullPath, "src\file.cs")
                handler = CreateHandler()

                Using LinkHandlerHelpers.InitializeRepository(dir.FullPath)
                End Using

                Assert.Equal(
                    "https://foo.visualstudio.com/_git/MyRepo?path=%2Fsrc%2Ffile.cs&version=GBmaster&line=2",
                    handler.MakeUrl(info, fileName, New LineSelection(2, 2))
                )
            End Using
        End Sub


        <Fact()>
        Public Sub CreatesCorrectLinkWithMultiLineSelection()
            Using dir As New TempDirectory
                Dim handler As VisualStudioTeamServicesHandler
                Dim info As GitInfo
                Dim fileName As String


                info = New GitInfo(dir.FullPath, "ssh://foo@vs-ssh.visualstudio.com:22/_ssh/MyRepo")
                fileName = Path.Combine(dir.FullPath, "src\file.cs")
                handler = CreateHandler()

                Using LinkHandlerHelpers.InitializeRepository(dir.FullPath)
                End Using

                Assert.Equal(
                    "https://foo.visualstudio.com/_git/MyRepo?path=%2Fsrc%2Ffile.cs&version=GBmaster&line=1&lineEnd=3",
                    handler.MakeUrl(info, fileName, New LineSelection(1, 3))
                )
            End Using
        End Sub


        <Fact()>
        Public Sub UsesCurrentBranch()
            Using dir As New TempDirectory
                Dim handler As VisualStudioTeamServicesHandler
                Dim info As GitInfo
                Dim fileName As String


                info = New GitInfo(dir.FullPath, "ssh://foo@vs-ssh.visualstudio.com:22/_ssh/MyRepo")
                fileName = Path.Combine(dir.FullPath, "src\file.cs")
                handler = CreateHandler()

                Using repo = LinkHandlerHelpers.InitializeRepository(dir.FullPath)
                    LibGit2Sharp.Commands.Checkout(repo, repo.CreateBranch("feature/work"))
                End Using

                Assert.Equal(
                    "https://foo.visualstudio.com/_git/MyRepo?path=%2Fsrc%2Ffile.cs&version=GBfeature%2Fwork",
                    handler.MakeUrl(info, fileName, Nothing)
                )
            End Using
        End Sub

    End Class


    Public Shared Iterator Function GetRemotes() As IEnumerable(Of Object())
        Yield {"https://foo.visualstudio.com/_git/MyRepo"}
        Yield {"ssh://foo@vs-ssh.visualstudio.com:22/_ssh/MyRepo"}
    End Function


    Private Shared Function CreateHandler() As VisualStudioTeamServicesHandler
        Return New VisualStudioTeamServicesHandler()
    End Function

End Class