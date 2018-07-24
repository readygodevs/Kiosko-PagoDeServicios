Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Public Class BConceptosServicios

    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)

    Private Sub ActualizarGrid()
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

        qry = "SELECT *,cast(EstatusServ as bit) as Estatus from T_ConceptosDeServicios"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        grvConceptos.DataSource = dt

        grvConceptos.Columns("TienePeriodoSem").Visible = False
        grvConceptos.Columns("DiaInicio").Visible = False
        grvConceptos.Columns("DiaFin").Visible = False

        grvConceptos.Columns("EmailProv").Visible = False
        grvConceptos.Columns("TieneDiasCredito").Visible = False
        grvConceptos.Columns("DiasCredito").Visible = False
        grvConceptos.Columns("EmailKiosko").Visible = False
        grvConceptos.Columns("ArchivoFTP").Visible = False
        grvConceptos.Columns("TipoServicio").Visible = False
        grvConceptos.Columns("EstatusServ").Visible = False
        grvConceptos.Columns("DiasDePago").Visible = False
        grvConceptos.Columns("DiasDeCobro").Visible = False
        grvConceptos.Columns("RangoDesde").Visible = False
        grvConceptos.Columns("RangoHasta").Visible = False
        grvConceptos.Columns("SumaCargo").Visible = False
        grvConceptos.Columns("RestaContr").Visible = False
        grvConceptos.Columns("Cuentacontable").Visible = False
        'grvConceptos.Columns("IdServicio").AllowEditing = False
        'grvConceptos.Columns("IdServicio").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        'grvConceptos.Columns("Descripcion").AllowEditing = False
        grvConceptos.Columns("Descripcion").Width = 400
        grvConceptos.Columns("Descripcion").HeaderText = "Descripción"
        'grvConceptos.Columns("CRI_Codigo").AllowEditing = False
        grvConceptos.Columns("CRI_Codigo").HeaderText = "Código Dyn."
        grvConceptos.Columns("CRI_Agrupador").HeaderText = "Agrupador"
        grvConceptos.Columns("CRI_Agrupador").Width = 70
        grvConceptos.Columns("CRI_Codigo").Width = 70
        'grvConceptos.Columns("Estatus").AllowEditing = False
        For Each col As DataGridViewColumn In grvConceptos.Columns
            col.ReadOnly = True
        Next
        grvConceptos.AllowUserToAddRows = False
    End Sub
    Private Sub BConceptosServicios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ActualizarGrid()

        Me.Text = "Conceptos de Servicios"
    End Sub

    Private Sub grvConceptos_DoubleClick(sender As Object, e As EventArgs) Handles grvConceptos.DoubleClick
        Dim IdServicio As String = grvConceptos.CurrentRow.Cells("IdServicio").Value
        Dim Descripcion As String = grvConceptos.CurrentRow.Cells("Descripcion").Value
        Dim CRI_Codigo As String = grvConceptos.CurrentRow.Cells("CRI_Codigo").Value
        Dim EstatusServ As String = grvConceptos.CurrentRow.Cells("EstatusServ").Value
        Dim TipoServ As String = grvConceptos.CurrentRow.Cells("TipoServicio").Value
        Dim DiasPago As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("DiasDePago").Value), "0",
                                     grvConceptos.CurrentRow.Cells("DiasDePago").Value)
        Dim DiasCobro As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("DiasDeCobro").Value), "0",
                                      grvConceptos.CurrentRow.Cells("DiasDeCobro").Value)
        Dim EmailProv As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("EmailProv").Value), "",
                                      grvConceptos.CurrentRow.Cells("EmailProv").Value)
        Dim EmailKiosko As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("EmailKiosko").Value), "",
                                      grvConceptos.CurrentRow.Cells("EmailKiosko").Value)
        Dim ArchivoFTP As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("ArchivoFTP").Value), "",
                                      grvConceptos.CurrentRow.Cells("ArchivoFTP").Value)
        Dim CuentaCont As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("CuentaContable").Value), "",
                                      grvConceptos.CurrentRow.Cells("CuentaContable").Value)
        Dim RangoDesde As String = grvConceptos.CurrentRow.Cells("RangoDesde").Value
        Dim RangoHasta As String = grvConceptos.CurrentRow.Cells("RangoHasta").Value
        Dim suma As Boolean = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("SumaCargo").Value), False,
                                      grvConceptos.CurrentRow.Cells("SumaCargo").Value)
        Dim resta As Boolean = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("RestaContr").Value), False,
                                      grvConceptos.CurrentRow.Cells("RestaContr").Value)
        Dim proov As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("Proveedor").Value), False,
                                      grvConceptos.CurrentRow.Cells("Proveedor").Value)

        Dim tienePeriodo As Boolean = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("TienePeriodoSem").Value), False,
                                      grvConceptos.CurrentRow.Cells("TienePeriodoSem").Value)
        Dim DiaInicio As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("DiaInicio").Value), "1",
                                     grvConceptos.CurrentRow.Cells("DiaInicio").Value)
        Dim DiaFin As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("DiaFin").Value), "1",
                                     grvConceptos.CurrentRow.Cells("DiaFin").Value)

        Dim tieneDiasCredito As Boolean = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("TieneDiasCredito").Value), False,
                                      grvConceptos.CurrentRow.Cells("TieneDiasCredito").Value)

        Dim DiasCredito As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("DiasCredito").Value), "0",
                                     grvConceptos.CurrentRow.Cells("DiasCredito").Value)

        Dim criagrupador As String = IIf(IsDBNull(grvConceptos.CurrentRow.Cells("CRI_Agrupador").Value), "",
                                     grvConceptos.CurrentRow.Cells("CRI_Agrupador").Value)

        Using mnto As New MConceptosServicios(IdServicio, Descripcion, EstatusServ, CRI_Codigo, TipoServ, DiasPago,
                                              DiasCobro, EmailProv, EmailKiosko, ArchivoFTP, RangoDesde, RangoHasta, CuentaCont,
                                              suma, resta, proov, tienePeriodo, DiaInicio, DiaFin, tieneDiasCredito, DiasCredito,
                                              criagrupador)
            mnto.ShowDialog()
            If mnto.Refrescar Then
                ActualizarGrid()
            End If
            mnto.Dispose()

        End Using
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        grvConceptos_DoubleClick(Nothing, Nothing)
    End Sub
End Class