
Public Class Loader_Base
    Dim m_CountTo As Integer
    Sub New(ByVal titulo As String, ByVal conteo As Integer)

        ' Esta llamada es exigida por el diseñador.
        m_CountTo = conteo
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        Me.Text = titulo
    End Sub
    Private Sub My_BgWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles My_BgWorker.DoWork



        For i As Integer = 1 To m_CountTo

            '' Has the background worker be told to stop?

            'If My_BgWorker.CancellationPending Then

            '    ' Set Cancel to True

            '    e.Cancel = True

            '    Exit For

            'End If
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 Second
            ' Report The progress of the Background Worker.
            My_BgWorker.ReportProgress(CInt((i / m_CountTo) * 100))
        Next
    End Sub
    Private Sub My_BgWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles My_BgWorker.ProgressChanged
        Me.ProgressBar1.Value = e.ProgressPercentage
    End Sub
    Private Sub My_BgWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles My_BgWorker.RunWorkerCompleted

        If e.Cancelled Then

            'Me.Lbl_Status.Text = "Cancelled"

        Else

            'Me.Lbl_Status.Text = "Completed"
            Me.Close()
        End If

    End Sub

    Private Sub Loader_Base_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim p As New Point
        p.X = (Screen.PrimaryScreen.Bounds.Width / 2) - (Me.Width / 2)
        p.Y = (Screen.PrimaryScreen.Bounds.Height / 2) - (Me.Height / 2)
        Me.Location = p
        My_BgWorker.RunWorkerAsync()
    End Sub
End Class