Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim App As String = My.Application.Info.AssemblyName
        ' If FuncSQL.Misc.InicRegistro(App, "CreaIconos", True, "Configuracion") Then
        Dim so As String = "PROGRAMFILES"
        If IntPtr.Size = 8 Then : so = "PROGRAMFILES(x86)" : End If
        Dim MiDLL As New jm_desarrollob.jm_desarrollob
        MiDLL.CreateShortCut(App, Environment.GetEnvironmentVariable(so) & "\VersionesNET\VersionesNET.exe", True, MiDLL.EnumCarpetasEspeciales.Desktop, , App, , , Environment.GetEnvironmentVariable(so) & "\" & App & "\" & App & ".exe", Environment.GetEnvironmentVariable(so) & "\VersionesNET")
        MiDLL.CreateShortCut(App, Environment.GetEnvironmentVariable(so) & "\" & App & "\" & App & ".exe", True, MiDLL.EnumCarpetasEspeciales.Programs, "Kiosko", , , , Environment.GetEnvironmentVariable(so) & "\" & App & "\" & App & ".exe", Environment.GetEnvironmentVariable(so) & "\" & App)
        ' End If


        'Dim fecha As String = DateTime.Now.ToString("yyMMdd")
        PagosToolStripMenuItem1_Click(Nothing, Nothing)
    End Sub

    Private Sub PagosToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PagosToolStripMenuItem1.Click
        Dim frm2 As New dtfechafin
        frm2.MdiParent = Me
        ' Para mostrarlo maximizado al crear la ventana
        frm2.WindowState = FormWindowState.Maximized
        frm2.Show()
        'dtfechafin.Show()
    End Sub

    Private Sub SalirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)



        Me.Close()
    End Sub

    Private Sub ConceptosDeServiciosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConceptosDeServiciosToolStripMenuItem.Click
        Dim frm2 As New BConceptosServicios
        frm2.MdiParent = Me
        ' Para mostrarlo maximizado al crear la ventana
        frm2.WindowState = FormWindowState.Maximized
        frm2.Show()
    End Sub

    Private Sub RutasFTPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RutasFTPToolStripMenuItem.Click
        Dim frm2 As New BRutasFTP
        frm2.MdiParent = Me
        ' Para mostrarlo maximizado al crear la ventana
        frm2.WindowState = FormWindowState.Maximized
        frm2.Show()
    End Sub

    Private Sub PagosToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles PagosToolStripMenuItem2.Click
        Dim frm2 As New RptConServicios
        frm2.MdiParent = Me
        ' Para mostrarlo maximizado al crear la ventana
        frm2.WindowState = FormWindowState.Maximized
        frm2.Show()
    End Sub

    Private Sub ConciliaciónDeServiciosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConciliaciónDeServiciosToolStripMenuItem.Click
        Dim frm2 As New BConciliacion
        frm2.MdiParent = Me
        ' Para mostrarlo maximizado al crear la ventana
        frm2.WindowState = FormWindowState.Maximized
        frm2.Show()
    End Sub
End Class