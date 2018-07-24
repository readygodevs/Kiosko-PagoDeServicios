Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Public Class BRutasFTP

    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)

    Private Sub ActualizarGrid()
        qry = "SELECT * from T_RutasFTP"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        grvConceptos.DataSource = dt
        grvConceptos.Columns("IdRuta").Visible = False
        
        grvConceptos.Columns("Descripcion").ReadOnly = True
        'grvConceptos.Columns("Descripcion").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        grvConceptos.Columns("Descripcion").Width = 150
        grvConceptos.Columns("Descripcion").HeaderText = "Descripción"

        grvConceptos.Columns("RutaOrigen").ReadOnly = True
        'grvConceptos.Columns("RutaOrigen").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        grvConceptos.Columns("RutaOrigen").Width = 300
        grvConceptos.Columns("RutaOrigen").HeaderText = "Ruta Origen"

        grvConceptos.Columns("RutaDestino").ReadOnly = True
        'grvConceptos.Columns("RutaDestino").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        grvConceptos.Columns("RutaDestino").Width = 300
        grvConceptos.Columns("RutaDestino").HeaderText = "Ruta Destino"

        grvConceptos.AllowUserToAddRows = False
    End Sub
    Private Sub BConceptosServicios_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        My.Computer.Registry.CurrentUser.CreateSubKey("PagoeServiciosCnx")
        Dim candenaCnx As String =
            My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios",
                                          "Conexion",
                                          "")

        If String.IsNullOrEmpty(candenaCnx) Then
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios",
                                          "Conexion",
                                          "Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=pagodeservicios;Data Source=192.168.100.6 ;connection timeout=0")

            candenaCnx =
            My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios",
                                          "Conexion",
                                          "")
        End If

        cnx.ConnectionString = candenaCnx

        ActualizarGrid()

        Me.Text = "Rutas FTP"
    End Sub

    Private Sub grvConceptos_DoubleClick(sender As Object, e As EventArgs) Handles grvConceptos.DoubleClick
        Dim Descripcion As String = grvConceptos.CurrentRow.Cells("Descripcion").Value
        Dim RutaO As String = grvConceptos.CurrentRow.Cells("RutaOrigen").Value

        Dim RutaD As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("RutaDestino").Value), "", _
                                     grvConceptos.CurrentRow.Cells("RutaDestino").Value)
        Dim ID As String = grvConceptos.CurrentRow.Cells("IdRuta").Value



        Using mnto As New MRutasFTP(2, Descripcion, RutaO, RutaD, ID)
            mnto.ShowDialog()
            If mnto.Refrescar Then
                ActualizarGrid()
            End If
            mnto.Dispose()

        End Using
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        grvConceptos_DoubleClick(Nothing, Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Using mnto As New MRutasFTP(1)
            mnto.ShowDialog()
            If mnto.Refrescar Then
                ActualizarGrid()
            End If
            mnto.Dispose()

        End Using
    End Sub

    
End Class