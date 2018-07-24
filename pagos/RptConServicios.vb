Public Class RptConServicios
    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)

    Private Sub RptConServicios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        crReporte.ShowCloseButton = False
        crReporte.ShowGroupTreeButton = False
        crReporte.ShowParameterPanelButton = False
        crReporte.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None

        crReporte.ShowRefreshButton = False

        qry = "Select '' as a ,'SELECCIONE UN SERVICIO' as b UNION ALL " &
            "SELECT CRI_Codigo as a,descripcion as b FROM dbo.T_ConceptosDeServicios where EstatusServ=1"
        dt = sqls.DevuelveDatos(cnx, qry)
        cmbServicio.DataSource = dt
        cmbServicio.DisplayMember = "b"
        cmbServicio.ValueMember = "a"
        cmbServicio.DropDownStyle = ComboBoxStyle.DropDownList
        btnBuscar_Click(Nothing, Nothing)
    End Sub
    Public Sub RefrescarReporte()

        dt = sqls.DevuelveDatos(cnx, qry, , , )
        Dim reporte As New CRConServicios
        reporte.SetDataSource(dt)
        If dt.Rows.Count > 0 Then
            reporte.SetParameterValue("Total", dt.Compute("SUM(Importe)", ""))
        Else
            reporte.SetParameterValue("Total", 0)
        End If

        crReporte.ReportSource = reporte
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        qry = "SELECT * from vw_ConciliacionServ where FechaReal >='" & dtpDesde.Value.ToString("yyyy-MM-dd") & " 00:00:00' and " & _
            " Fechareal<= '" & dtpHasta.Value.ToString("yyyy-MM-dd") & " 23:59:59' AND Conciliado=" & IIf(chkConciliados.Checked, "'SI'", "'NO'") & _
            " AND Pagado=" & IIf(chkPagados.Checked, "'SI'", "'NO'") & _
            IIf(cmbServicio.SelectedValue = "", "", " AND CRI_Codigo in " & _
                IIf((New List(Of String)(New String() {"50", "51"})).Contains(cmbServicio.SelectedValue), "('50','51')", _
                    "('" & cmbServicio.SelectedValue & "')"))
        RefrescarReporte()
    End Sub
End Class