Imports System.Data.SqlClient
Public Class FrmBrowseGral

    Private erpError As New ErrorProvider

    Public IdItem As String
    Public DescrItem As String
    Public cSource As DataTable
    Public Filtro As String
    Sub New(ByVal source As DataTable, ByVal lfiltro As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        IdItem = ""
        DescrItem = ""
        cSource = source
        Filtro = lfiltro
    End Sub
    Private Sub FrmLocalizaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim p As New Point
        p.X = (Screen.PrimaryScreen.Bounds.Width / 2) - (Me.Width / 2)
        p.Y = (Screen.PrimaryScreen.Bounds.Height / 2) - (Me.Height / 2)
        Me.Location = p



        grvArticulos.DataSource = cSource
        grvArticulos.Splits(0).DisplayColumns("Descr").Width = 250
        grvArticulos.Splits(0).DisplayColumns("Id").Width = 50
        grvArticulos.AllowSort = False
        grvArticulos.Columns("Id").SortDirection = C1.Win.C1TrueDBGrid.SortDirEnum.Ascending
        grvArticulos.AllowSort = True

        grvArticulos.Columns("Id").FilterText = Filtro
        grvArticulos.FilterActive = True
    End Sub

    'Private Sub grvArticulos_KeyPress(sender As Object, e As KeyEventArgs) Handles grvArticulos.KeyUp
    '    If e.KeyValue = 13 Then
    '        Try
    '            IdItem = grvArticulos.Columns("Id").CellValue(grvArticulos.Row)
    '            DescrItem = grvArticulos.Columns("Descr").CellValue(grvArticulos.Row)
    '            Me.Close()
    '        Catch ex As Exception

    '        End Try
    '    ElseIf e.KeyValue = 27 Then
    '        IdItem = ""
    '        DescrItem = ""
    '        Me.Close()
    '    End If
    'End Sub

    Private Sub FrmLocalizaciones_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'Cnx.Close()
    End Sub

    'Private Sub C1TrueDBGrid1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As _
    '            System.Windows.Forms.MouseEventArgs) Handles grvArticulos.DoubleClick
    '    If grvArticulos.PointAt(e.X, e.Y) = C1.Win.C1TrueDBGrid.PointAtEnum.AtDataArea Then

    '        IdItem = grvArticulos.Columns("Id").CellValue(grvArticulos.Row)
    '        DescrItem = grvArticulos.Columns("Descr").CellValue(grvArticulos.Row)
    '        Me.Close()

    '    End If
    'End Sub

    Private Sub FrmBrowseGral_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = "47" Then
            IdItem = ""
            DescrItem = ""
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IdItem = ""
        For Each i As Integer In grvArticulos.SelectedRows

            IdItem &= IIf(IdItem = "", "", ",") & grvArticulos.Columns("Id").CellValue(i)
        Next
        Me.Close()
    End Sub
End Class