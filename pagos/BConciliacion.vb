Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Public Class BConciliacion

    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)

    Private Sub ActualizarGrid()
        qry = "SELECT Tienda,Estacion,Consecutivo,Folio,LoteCxP,ReferenciaCxP,Servicio,Referencia,Fecha,cast(Importe as varchar(20)) Importe,Conciliado,Pagado,FechaPago,CRI_Codigo from vw_ConciliacionServ " &
            "where FechaReal between '" & dtpDesde.Value.ToString("yyyy-MM-dd") & " 00:00:00' and '" &
            dtpHasta.Value.ToString("yyyy-MM-dd") & " 23:59:59' " &
            IIf(cmbServicio.SelectedValue = "", "", " AND CRI_Codigo in " &
                IIf((New List(Of String)(New String() {"50", "51"})).Contains(cmbServicio.SelectedValue), "('50','51')",
                    "('" & cmbServicio.SelectedValue & "')")) & IIf(txtFolio.Text <> "", " AND folio like '%" & txtFolio.Text & "%'", "")
        dt = New DataTable
        dt = sqls.DevuelveDatos(cnx, qry, , , )

        grvConceptos.DataSource = dt
        grvConceptos.Splits(0).DisplayColumns("CRI_Codigo").Visible = False

        grvConceptos.Splits(0).DisplayColumns("Tienda").Width = 50
        grvConceptos.Splits(0).DisplayColumns("Estacion").Width = 50
        grvConceptos.Splits(0).DisplayColumns("Consecutivo").Width = 80
        grvConceptos.Splits(0).DisplayColumns("Conciliado").Width = 80
        grvConceptos.Splits(0).DisplayColumns("Pagado").Width = 80
        grvConceptos.Splits(0).DisplayColumns("Servicio").Width = 150

        'grvConceptos.Columns("Servicio").FilterText = "*" & "76" & "*"
        For Each col As C1.Win.C1TrueDBGrid.C1DataColumn In grvConceptos.Columns
            col.FilterOperator = "like"
        Next
        'grvConceptos.Columns("Servicio").FilterOperator = "like"


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

        Me.Text = "Conciliación de Servicios"

        qry = "Select * from " &
"(Select '' as a ,'SELECCIONE UN SERVICIO' as b,1 sortby " &
        "UNION ALL " &
"SELECT CRI_Codigo as a,descripcion as b,2 sortby  FROM dbo.T_ConceptosDeServicios where EstatusServ=1) dum " &
"order by sortby,b"
        dt = sqls.DevuelveDatos(cnx, qry)
        cmbServicio.DataSource = dt
        cmbServicio.DisplayMember = "b"
        cmbServicio.ValueMember = "a"
        cmbServicio.DropDownStyle = ComboBoxStyle.DropDownList

        ActualizarGrid()

    End Sub

    Private Sub grvConceptos_MouseDown(sender As Object, e As MouseEventArgs) Handles grvConceptos.MouseDown
        grvConceptos.Bookmark = (grvConceptos.RowBookmark(grvConceptos.RowContaining(e.Y)))
        If e.Button = MouseButtons.Right Then
            Dim rowindex, colindex As Integer

            grvConceptos.CellContaining(e.X, e.Y, rowindex, colindex)
            If rowindex > -1 And colindex > -1 Then
                cmsMenu.Show(grvConceptos, e.Location)
                cmsMenu.Show(Cursor.Position)
            End If
        End If
    End Sub


    Private Sub dtpDesde_ValueChanged(sender As Object, e As EventArgs) Handles dtpDesde.ValueChanged, dtpHasta.ValueChanged
        ActualizarGrid()
    End Sub
    Private Sub Eliminar_Registro()
        If grvConceptos.Columns("Conciliado").CellValue(grvConceptos.Bookmark) = "NO" Then
            Dim CRI_Codigo As String = grvConceptos.Columns("CRI_Codigo").CellValue(grvConceptos.Bookmark)
            Dim Tienda As String = grvConceptos.Columns("Tienda").CellValue(grvConceptos.Bookmark)
            Dim Estacion As String = grvConceptos.Columns("Estacion").CellValue(grvConceptos.Bookmark)
            Dim Consec As String = grvConceptos.Columns("Consecutivo").CellValue(grvConceptos.Bookmark)
            Dim Fecha As String = grvConceptos.Columns("Fecha").CellValue(grvConceptos.Bookmark)
            Dim Referencia As String = grvConceptos.Columns("Referencia").CellValue(grvConceptos.Bookmark)

            qry = "Delete from conciliacionservicios where cri_codigo=@cricod and Tienda=@tienda and " &
                "Consecutivo=@consec and Fecha=@fecha and referencia=@refr and Estacion=@est"

            If cnx.State = ConnectionState.Closed Then cnx.Open()
            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = cnx
            sqlComm.CommandText = qry
            sqlComm.Parameters.Add("@cricod", SqlDbType.Int).Value = CRI_Codigo
            sqlComm.Parameters.Add("@tienda", SqlDbType.Int).Value = Tienda
            sqlComm.Parameters.Add("@est", SqlDbType.Int).Value = IIf(Estacion = "", 0, Estacion)
            sqlComm.Parameters.Add("@consec", SqlDbType.Int).Value = Consec
            sqlComm.Parameters.Add("@fecha", SqlDbType.DateTime).Value = Fecha
            sqlComm.Parameters.Add("@refr", SqlDbType.VarChar).Value = Referencia
            sqlComm.ExecuteNonQuery()
            cnx.Close()
            'ActualizarGrid()
            grvConceptos.Delete()
            'grvConceptos.Rows.RemoveAt(grvConceptos.CurrentRow.Index)
        Else
            MessageBox.Show("El registro no puede eliminarse, ya fue conciliado.", "Información")
        End If
    End Sub


    Private Sub EliminarRegistroToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EliminarRegistroToolStripMenuItem.Click
        Eliminar_Registro()
    End Sub

    Private Sub cmbServicio_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbServicio.SelectedValueChanged
        Try
            ActualizarGrid()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        ActualizarGrid()
    End Sub

    Private Sub grvConceptos_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles grvConceptos.MouseDoubleClick
        'Dim rowindex, colindex As Integer

        'grvConceptos.CellContaining(e.X, e.Y, rowindex, colindex)
        'If rowindex >= 0 And colindex >= 0 Then

        'End If
        If grvConceptos.Columns("Conciliado").CellValue(grvConceptos.Bookmark) = "NO" Then
            Using frm As New UpdConServicio(cnx,
                                    grvConceptos.Columns("CRI_Codigo").CellValue(grvConceptos.Bookmark),
                                    grvConceptos.Columns("Tienda").CellValue(grvConceptos.Bookmark),
                                    grvConceptos.Columns("consecutivo").CellValue(grvConceptos.Bookmark),
                                    grvConceptos.Columns("Fecha").CellValue(grvConceptos.Bookmark),
                                    grvConceptos.Columns("referencia").CellValue(grvConceptos.Bookmark))
                frm.ShowDialog()
                If frm.exito Then
                    ActualizarGrid()
                End If
                frm.Dispose()
            End Using
        End If
    End Sub
End Class