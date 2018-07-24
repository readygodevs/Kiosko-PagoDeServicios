Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb

Public Class dtfechafin

    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "jgonzalez@mikiosko.com.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=pagodeservicios;Data Source=192.168.100.6 ;connection timeout=0")

    Public transaccion As SqlTransaction
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)
    Public bandera As Integer = 0
    Public serviciox As String
    Public mostrarMsj As Boolean = True


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub pagos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RemoveHandler CbxServicio.TextChanged, AddressOf CbxServicio_TextChanged
        TabControl1.SelectedTab = TabControl1.TabPages(1)
        'Dim dia = DateTime.Now.ToString("dddd").ToUpper()

        My.Computer.Registry.CurrentUser.CreateSubKey("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios")
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



        'Crear carpeta pagosjuntos
        Dim qry4 As String = "Select RutaOrigen from T_RutasFTP where IdRuta=4"
        Dim TRutaPG As DataTable = sqls.DevuelveDatos(cnx, qry4, , , )
        Dim RutaPJ As String = TRutaPG.Rows(0)(0)

        If Not System.IO.Directory.Exists(RutaPJ) Then
            System.IO.Directory.CreateDirectory(RutaPJ)
        End If
        '---------------------------
        '---------------------------

        Dim dial As String
        Dim dianum As Integer
        dianum = Now.Day
        dial = UCase(DateTime.Now.ToString("dddd"))
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@dias", "%" + dial + "%"))

        qry = "SELECT DISTINCT fechapago FROM ConciliacionServicios AS ps " &
           "WHERE fechapago BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE()  AND cri_codigo in (Select cri_codigo from " &
           "T_ConceptosDeServicios as CON left join T_DiasDePagoServicios as DP on CON.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL')"
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        If dt.Rows.Count = 0 Then
            qry = "SELECT DISTINCT fechapago FROM compucaja.dbo.pagoservicios AS ps " &
           "WHERE fechapago BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE()  AND id_servicio in (Select cri_codigo from " &
           "T_ConceptosDeServicios as CON left join T_DiasDePagoServicios as DP on CON.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL')"
            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        End If

        'If dt.Rows.Count > 0 Then 'darlo de alta tabla rigo dia de pago
        '    qry = "SELECT descripcion,CRI_Codigo  FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " & _
        '        "AS td ON tc.DiasDePago = td.IdDiasPago " & _
        '  " WHERE  td.diasdepago LIKE @dias and EstatusServ=1"
        '    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        '    sqls.LlenaCombo(CbxServicio, "descripcion", "CRI_Codigo", dt)
        'Else
        '    If dianum >= 2 Then 'si no hay pagos en 30 dias atras y si ya es los primeros 5 dias para pagar el servicio
        '        MsgBox("pagar servicios mensuales (guadalupana mensual y otros)")
        qry = "SELECT descripcion,CRI_Codigo FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " &
            "AS td ON tc.DiasDePago = td.IdDiasPago WHERE  td.diasdepago LIKE @dias and EstatusServ=1 " &
      " UNION SELECT descripcion,CRI_Codigo  FROM dbo.T_ConceptosDeServicios WHERE " &
         "CRI_Codigo in " &
         "(select cri_codigo from T_ConceptosDeServicios as CON left join " &
         "T_DiasDePagoServicios as DP on COn.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL')"
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        sqls.LlenaCombo(CbxServicio, "descripcion", "CRI_Codigo", dt)
        '    End If
        'End If
        qry = "SELECT SV_Codigo AS IdServicio,SV_Nombre AS Nombre FROM compucaja.dbo.Servicios WHERE SV_Codigo IN (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,30,28,29,35,36,34,37,39,43,41,44,48,49,50,51) " &
       "UNION " &
       "SELECT SV_Codigo AS IdServicio,SV_Nombre AS Nombre FROM compucaja.dbo.Servicios  WHERE SV_Codigo=42"
        dt = sqls.DevuelveDatos(cnx, qry)
        'sqls.LlenaCombo(cbxconsulta, "Nombre", "IdServicio", dt)
        AddHandler CbxServicio.TextChanged, AddressOf CbxServicio_TextChanged

        My.Computer.Registry.CurrentUser.CreateSubKey("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios")
        Dim Correo As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios", "Correo", "")
        Dim Password As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios", "Password", "")

        'If String.IsNullOrEmpty(Correo) Then

        '    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios\DatosCorreo", "Correo", txtCorreo.Text)
        '    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios\DatosCorreo", "Password", txtPass.Text)

        'End If

        txtCorreo.Text = Correo
        txtPass.Text = Password
    End Sub

    Private Sub CbxServicio_TabStopChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbxServicio.TabStopChanged

    End Sub
#Region "CbxServicio.TextChanged"
    Private Sub CbxServicio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbxServicio.TextChanged
        If CbxServicio.Text = "" Then Exit Sub
        qry = String.Empty
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
        lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
        lop.Add(sqls.CreadbParametros("@diapago", CbxServicio.Text))
        Dim fechapagox As String
        Dim diapagox As String()

        lop.Add(sqls.CreadbParametros("@idservicio", CbxServicio.SelectedValue))
        qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ," &
             "SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago  " &
             "FROM ConciliacionServicios AS ps with (nolock) LEFT JOIN  compucaja.dbo.Servicios AS s with (nolock) ON " &
             "ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND " &
             "fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin " & IIf(CbxServicio.SelectedValue = 16, " AND sv_codigo=7 ", "")
        'End If

        dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
        dgservicio.DataSource = dt

        '---buscar en tabla de base historialkiosko dias de pagos de servicios
        qry = " Select  td.DiasDePago  " &
            " FROM dbo.T_ConceptosDeServicios AS tc with (nolock) INNER JOIN dbo.T_DiasDePagoServicios AS td  with (nolock) ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
        dgpago.DataSource = dt

        ''---buscar fecha ultima de pago de servicio
        qry = " SELECT MAX(fechapago) AS fechaultimopago " &
             " FROM ConciliacionServicios  with (nolock) WHERE " & IIf(CbxServicio.SelectedValue = 50 Or CbxServicio.SelectedValue = 51, "CRI_Codigo in (50,51)", " CRI_Codigo=@idservicio ")
        dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
        If IsDBNull(dt.Rows(0)(0)) Then
            qry = " SELECT MAX(fechapago) AS fechaultimopago " &
             " FROM compucaja.dbo.pagoservicios  with (nolock) WHERE " & IIf(CbxServicio.SelectedValue = 50 Or CbxServicio.SelectedValue = 51, "id_servicio in (50,51)", " id_servicio=@idservicio ")
            dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
        End If
        TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString
        '----poner ancho a columna
        dgpago.Columns(0).Width = 190


        '--de la fecha ultima de pago se toma el dia para el siguiente pago
        fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
        diapagox = fechapagox.Split("/")

        TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

        Dim Total As Single
        Dim col As Integer
        col = 4
        For Each row As DataGridViewRow In Me.dgservicio.Rows()
            Total += Val(row.Cells(col).Value)
        Next
        Me.sumaimporte.Text = Format(Total, "$ #,##0.00") 'Total.ToString

    End Sub
    'Private Sub CbxServicio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbxServicio.TextChanged
    '    If CbxServicio.Text = "" Then Exit Sub
    '    qry = String.Empty
    '    lop.Clear()
    '    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
    '    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
    '    lop.Add(sqls.CreadbParametros("@diapago", CbxServicio.Text))
    '    Dim fechapagox As String
    '    Dim diapagox As String()

    '    Select Case CbxServicio.Text.Substring(8, CbxServicio.Text.Length - 8)
    '        Case "COBROS CFE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 5))
    '            qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ," & _
    '                 "SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago  " & _
    '                 "FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON " & _
    '                 "ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND " & _
    '                 "fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            '---buscar en tabla de base historialkiosko dias de pagos de servicios
    '            qry = " Select  td.DiasDePago  " & _
    '                " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString
    '            '----poner ancho a columna
    '            dgpago.Columns(0).Width = 190


    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")

    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))



    '        Case "COBROS IMPUESTOS COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 7))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString


    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))


    '        Case "COBROS CIAPACOV"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 13))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps with (nolock) INNER JOIN  compucaja.dbo.Servicios AS s with (nolock) ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS CASEFON"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 14))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = "no hay" 'CInt(IIf(diapagox(0)="",0,diapagox(0)))

    '        Case "COBROS AYUNTAMIENTO COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 15))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString
    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS SEAPAL (SOLO VALLARTA)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 17))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS MEGACABLE  (SOLO ZONA COLIMA)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 16))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin  AND sv_codigo=7 "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS TELMEX"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 18))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS SKY"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 19))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS INFONAVIT"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 20))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS OROMAPAS"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 21))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS LA GUADALUPANA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 22))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS DISH"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 24))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS AYUNTAMIENTO MANZANILLO"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 26))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS AYUNTAMIENTO VILLA DE ALVAREZ"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 27))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS TELECABLE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 28))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                 " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo=@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))


    '        Case "COBROS UNIVERSIDAD VIZCAYA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 55))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,42,43,55) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,55) "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS TARJETAS SEFIDEC"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 41))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt


    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (REINSCRIPCION)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 39))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (INGLES)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 40))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            'fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            'diapagox = fechapagox.Split("/")
    '            ' TextBox1.Text = CInt(IIf(diapagox(0)="",0,diapagox(0)))

    '        Case "COBROS ANUNCIO INMOBILIARI"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 46))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))


    '        Case "COBROS PAYNET"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 44))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS UNIVERSIDAD DE LA VERA CRUZ"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 49))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS AYUNTAMIENTO DE PUERTO VALLARTA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 47))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))


    '        Case "COBROS COMAPAT"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 48))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))


    '        Case "COBROS CAPDAM"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 56))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "AUTOGESTION CFE"                   '("@dias", "%" + dial + "%"))
    '            'lop.Add(sqls.CreadbParametros("@idservicio", "(" + "50" + "," + "51" + ")"))
    '            'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,'AUTOGESTION CFE' AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps LEFT JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (50,51) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo in (50,51) "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio in (50,51)"
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "PREPAGO AUTOTRANSPORTE"                   '("@dias", "%" + dial + "%"))
    '            'lop.Add(sqls.CreadbParametros("@idservicio", "(" + "50" + "," + "51" + ")"))
    '            'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (59) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo in (59) "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS AYUNTAMIENTO MAZATLAN (PREDIAL)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 61))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS JUMAPAM"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 62))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS UNIVERSIDAD MULTITECNICA PROFESIONAL"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 65))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            diapagox = fechapagox.Split("/")
    '            TextBox1.Text = CInt(IIf(diapagox(0) = "", 0, diapagox(0)))

    '        Case "COBROS IZZI (CAPTURA MANUAL)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 67))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt

    '            qry = " Select  td.DiasDePago  " & _
    '                    " FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago WHERE descripcion=@diapago "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgpago.DataSource = dt

    '            ''---buscar fecha ultima de pago de servicio
    '            qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM dbo.ConciliacionServicios WHERE CRI_Codigo =@idservicio "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            If IsDBNull(dt.Rows(0)(0)) Then
    '                qry = " SELECT MAX(fechapago) AS fechaultimopago " & _
    '                 " FROM compucaja.dbo.pagoservicios WHERE id_servicio=@idservicio "
    '                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            End If
    '            TextBox2.Text = dt.Rows(0).Item("fechaultimopago").ToString

    '            '--de la fecha ultima de pago se toma el dia para el siguiente pago
    '            'fechapagox = dt.Rows(0).Item("fechaultimopago").ToString
    '            'diapagox = fechapagox.Split("/")
    '            'TextBox1.Text = CInt(IIf(diapagox(0)="",0,diapagox(0)))
    '        Case "UNIV VIZCAYA COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 68))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt
    '        Case "COLEGIO VIZCAYA COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 54))
    '            qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " & _
    '                  " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            dgservicio.DataSource = dt
    '    End Select
    '    'Sumar una Columna
    '    Dim Total As Single
    '    'Dim Col As Integer = Me.dgservicio.CurrentCell.ColumnIndex()
    '    Dim col As Integer
    '    col = 4
    '    For Each row As DataGridViewRow In Me.dgservicio.Rows()
    '        Total += Val(row.Cells(col).Value)
    '    Next
    '    Me.sumaimporte.Text = Format(Total, "$ #,##0.00") 'Total.ToString
    '    'dgservicio.Rows.Add()

    'End Sub
#End Region

#Region "Pagos Automaticos"
    Private Sub writeDebug(ByVal line As String, ByVal nameFile As String)
        'Dim path As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim qry4 As String = "Select RutaOrigen from T_RutasFTP where IdRuta=5"
        Dim TRutaCTL As DataTable = sqls.DevuelveDatos(cnx, qry4, , , , , transaccion)
        Dim RutaCTL As String = TRutaCTL.Rows(0)(0)

        If Not System.IO.Directory.Exists(RutaCTL) Then
            System.IO.Directory.CreateDirectory(RutaCTL)
        End If
        If Not System.IO.Directory.Exists(RutaCTL & "\DTA") Then
            System.IO.Directory.CreateDirectory(RutaCTL & "\DTA")
        End If
        If Not System.IO.Directory.Exists(RutaCTL & "\CTL") Then
            System.IO.Directory.CreateDirectory(RutaCTL & "\CTL")
        End If
        Dim path As String = RutaCTL & "\DTA"
        Dim FILE_NAME As String = path & "\" & nameFile
        If System.IO.File.Exists(FILE_NAME) = False Then
            System.IO.File.Create(FILE_NAME).Dispose()
        End If

        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
        objWriter.WriteLine(line)
        objWriter.Close()
    End Sub
    Public Function ChecaSolomon() As Boolean
        Dim pList As List(Of System.Diagnostics.Process) = System.Diagnostics.Process.GetProcesses().ToList
        Dim pDynamics As System.Diagnostics.Process = pList.Where(Function(p) p.ProcessName = "MSDynamicssl" _
                                                                               And Not p.MainWindowTitle.Contains("Dynamics SL (98.000.00)"))(0)
        '_
        'And p.MachineName = "."

        If pDynamics IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub jis() 'Handles PagoAut.Click
        Using frm As New Loader_Base("Espere un momento...", 15)
            frm.ShowDialog()
            frm.Dispose()
        End Using
    End Sub
    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles PagoAut.Click
        Dim btn As Button = sender
        Dim seleccion As Boolean = (btn.Name = "Button6")
        If Not ChecaSolomon() Then
            MessageBox.Show("Pagos no realizados. Inicie sesión en Solomon.")
            Exit Sub
        End If

        'validar si existen archivos
        '***************************
        qry = "Select * from T_RutasFTP where IdRuta=1"
        Dim RutasConc As DataTable = sqls.DevuelveDatos(cnx, qry, , , )

        Dim dir_info As New DirectoryInfo(RutasConc.Rows(0)("RutaOrigen"))
        Dim fs_infos As FileInfo() = dir_info.GetFiles("*.txt")
        If fs_infos.Length > 0 Then
            If MessageBox.Show(
                String.Format("Existen archivos sin conciliar en la ruta {0}, ¿desea continuar?",
                              RutasConc.Rows(0)("RutaOrigen")), "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If



        qry = "Select * from T_RutasFTP where IdRuta=2"
        RutasConc = sqls.DevuelveDatos(cnx, qry, , , )

        dir_info = New DirectoryInfo(RutasConc.Rows(0)("RutaOrigen"))
        fs_infos = dir_info.GetFiles("*.txt")
        If fs_infos.Length > 0 Then
            If MessageBox.Show(
                String.Format("Existen archivos sin conciliar en la ruta {0}, ¿desea continuar?",
                              RutasConc.Rows(0)("RutaOrigen")), "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If

        qry = "Select * from T_RutasFTP where IdRuta=3"
        RutasConc = sqls.DevuelveDatos(cnx, qry, , , )

        dir_info = New DirectoryInfo(RutasConc.Rows(0)("RutaOrigen"))
        fs_infos = dir_info.GetFiles("*.txt")
        If fs_infos.Length > 0 Then
            If MessageBox.Show(
                String.Format("Existen archivos sin conciliar en la ruta {0}, ¿desea continuar?",
                              RutasConc.Rows(0)("RutaOrigen")), "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If
        '***************************
        '***************************

        'Dim transaccion As SqlTransaction
        'Dim cnxLocal As New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=pagodeservicios;Data Source=192.168.100.6 ;connection timeout=0")
        Dim debug As String = 0
        Try
            debug = 1
            If cnx.State = ConnectionState.Closed Then cnx.Open()
            'transaccion = cnx.BeginTransaction
            Dim tablaServiciosPagar As DataTable
            'variables mandar correo servicio en particular

            'Dim correotienda As String = "rtorres@mikiosko.mx"
            Dim direcciones(5) As String


            Dim mensaje As String = "¿Está seguro que desea pagar todos los servicios de hoy?"
            If seleccion Then
                mensaje = "¿Está seguro que desea pagar los servicios seleccionados?"
            End If
            Dim Caption As String = "CONFIRMACION DE PAGO"
            Dim boton As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim resp As DialogResult

            resp = MessageBox.Show(mensaje, Caption, boton)
            Dim qryCtaPagar As String
            If resp = System.Windows.Forms.DialogResult.Yes Then
                Dim rutaPag1 As DataTable = sqls.DevuelveDatos(cnx, "select * from T_RutasFTP where idruta=4",
                                                          , , , , )
                For Each f In Directory.GetFiles(rutaPag1.Rows(0)("RutaOrigen"))
                    File.Delete(f)
                Next
                '----------------------------
                '----------------------------
                debug = 2
                lop.Clear()
                lop.Add(sqls.CreadbParametros("@dia", "%" & DateTime.Now.ToString("dddd").ToUpper & "%"))
                Dim qry0 As String = "Select cs.CRI_Codigo,cs.Descripcion from " &
                    "T_ConceptosDeServicios as CS " &
                    "inner join T_DiasDePagoServicios as DP on cs.DiasDePago=DP.IdDiasPago " &
                    "where DP.DiasDePago like @dia and CS.EstatusServ=1 " &
                    "group by cs.CRI_Codigo,Descripcion"

                qryCtaPagar = "Select cs.Proveedor,cs.CRI_Agrupador,cs.cuentacontable,cs.Descripcion," &
                "SUM(con.Importe) as Importe,isnull(con.ReferenciaCxP,'xyz') Referencia from " &
                    "T_ConceptosDeServicios as CS " &
                    "inner join T_DiasDePagoServicios as DP on cs.DiasDePago=DP.IdDiasPago " &
                    "inner join ConciliacionServicios as CON on cs.CRI_Codigo=CON.CRI_Codigo " &
                    "where DP.DiasDePago Like @dia And CS.EstatusServ=1 And CON.fechapago Is null And CON.Conciliado=1 And " &
                    "CON.fecha BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' AND '" &
                    DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' " &
                    "and CS.TienePeriodoSem=0 and CS.TieneDiasCredito=0 " &
                    "group by cs.Proveedor,cs.CRI_Agrupador,cs.cuentacontable,cs.Descripcion,con.ReferenciaCxP " &
                    " UNION " &
                    "SELECT  cs.Proveedor , " &
                        "cs.CRI_Agrupador , " &
                        "cs.cuentacontable , " &
                        "cs.Descripcion, " &
                        "SUM(con.Importe) As Importe , " &
                        "ISNULL(con.ReferenciaCxP, 'xyz') Referencia " &
                "From T_ConceptosDeServicios As cs " &
                        "INNER Join T_DiasDePagoServicios AS DP ON cs.DiasDePago = DP.IdDiasPago " &
                        "INNER Join ConciliacionServicios AS CON ON cs.CRI_Codigo = CON.CRI_Codigo " &
                "WHERE   dp.DiasDePago Like @dia " &
                        "And cs.EstatusServ = 1 " &
                        "And CON.fechapago Is NULL " &
                        "And CON.Conciliado = 1 " &
                        "And CON.fecha BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' " &
                                      "And '" & DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' " &
                        "And CS.TienePeriodoSem = 1 " &
                        "And dbo.fn_Obtener_Fecha_PorDia(CON.Fecha, CS.DiaFin, 0) <= dbo.fn_fecha_sinhora(GETDATE()) " &
                        "and CS.TieneDiasCredito=0 " &
                "GROUP by cs.Proveedor , " &
                        "cs.CRI_Agrupador , " &
                        "cs.cuentacontable, " &
                        "cs.Descripcion, " &
                        "con.ReferenciaCxP " &
                        " UNION " &
                    "SELECT  cs.Proveedor , " &
                        "cs.CRI_Agrupador , " &
                        "cs.cuentacontable , " &
                        "cs.Descripcion, " &
                        "SUM(con.Importe) As Importe , " &
                        "ISNULL(con.ReferenciaCxP, 'xyz') Referencia " &
                "From T_ConceptosDeServicios As cs " &
                        "INNER Join T_DiasDePagoServicios AS DP ON cs.DiasDePago = DP.IdDiasPago " &
                        "INNER Join ConciliacionServicios AS CON ON cs.CRI_Codigo = CON.CRI_Codigo " &
                "WHERE   dp.DiasDePago Like @dia " &
                        "And cs.EstatusServ = 1 " &
                        "And CON.fechapago Is NULL " &
                        "And CON.Conciliado = 1 " &
                        "And CON.fecha <= DateAdd(Day, -1 * cs.DiasCredito,'" &
                        DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59') " &
                        "And CS.TienePeriodoSem = 0 " &
                        "And CS.TieneDiasCredito = 1 " &
                "GROUP by cs.Proveedor , " &
                        "cs.CRI_Agrupador , " &
                        "cs.cuentacontable, " &
                        "cs.Descripcion, " &
                        "con.ReferenciaCxP"
                If seleccion Then
                    qry0 = "Select cs.CRI_Codigo, cs.Descripcion from " &
                    "T_ConceptosDeServicios as CS " &
                    "where CS.CRI_Codigo in (" & btn.ImageKey & ")" &
                    " and CS.TieneDiasCredito=0 " &
                    "group by cs.CRI_Codigo,Descripcion"
                    qryCtaPagar = "Select cs.Proveedor,cs.CRI_Agrupador,cs.cuentacontable,cs.Descripcion," &
                "SUM(con.Importe) as Importe,isnull(con.ReferenciaCxP,'xyz') Referencia from " &
                    "T_ConceptosDeServicios as CS " &
                    "inner join ConciliacionServicios as CON on cs.CRI_Codigo=CON.CRI_Codigo " &
                    "where CS.CRI_Codigo in (" & btn.ImageKey & ") And CS.EstatusServ=1 And " &
                    " and CS.TieneDiasCredito=0 AND " &
                    "CON.fechapago Is null And CON.Conciliado=1 And " &
                    "CON.fecha BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' AND '" &
                    DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' " &
                    "group by cs.Proveedor,cs.CRI_Agrupador,cs.cuentacontable,cs.Descripcion,con.ReferenciaCxP"
                End If

                Dim qry2 As String = " SELECT fechapago AS fechaultimopago " &
                 " FROM ConciliacionServicios WHERE CRI_Codigo in " &
                    "(select cri_codigo from T_ConceptosDeServicios as CON left join T_DiasDePagoServicios as DP on " &
                     "COn.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL' and CON.TieneDiasCredito=0) and fechapago is not null and Month(fechapago)=" &
                     Now.Month & " and Year(fechapago)=" & Now.Year
                dt = sqls.DevuelveDatos(cnx, qry2, , , lop, , )

                If dt.Rows.Count = 0 And Not seleccion Then
                    If MessageBox.Show("¿Desea realizar el pago de servicios Mensuales?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        qry0 &= " UNION " &
                        "SELECT CON.CRI_Codigo,CON.descripcion FROM dbo.T_ConceptosDeServicios as CON left join T_DiasDePagoServicios as DP on " &
                             "COn.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL' " &
                             "and CON.EstatusServ=1 And CON.TieneDiasCredito=0"
                        qryCtaPagar &= " UNION " &
                        "SELECT  con.Proveedor,con.CRI_Agrupador,con.cuentacontable,con.Descripcion,SUM(cs.Importe) as Importe,isnull(CS.ReferenciaCxP,'xyz') " &
                        "FROM dbo.T_ConceptosDeServicios " &
                        "as CON left join T_DiasDePagoServicios as DP on " &
                             "COn.DiasDePago=DP.IdDiasPago inner join ConciliacionServicios as CS on con.CRI_Codigo=cs.CRI_Codigo " &
                             "where DP.DiasDePago='MENSUAL' and CON.EstatusServ=1 " &
                             "and CS.fechapago is null and CS.Conciliado=1 and CON.TieneDiasCredito=0 and " &
                        "CS.fecha BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' AND '" &
                        DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' group by con.Proveedor,con.CRI_Agrupador,con.cuentacontable,con.Descripcion,CS.ReferenciaCxP "
                    End If
                End If
                '--------------------------------------------
                '--------------HACER CUENTA POR PAGAR--------
                'obtener servicios a pagar y colocarlos en memoria
                debug = 3
                Dim dtServ As DataTable = sqls.DevuelveDatos(cnx, qry0, , , lop, , )
                tablaServiciosPagar = sqls.DevuelveDatos(cnx, "set language english " & qryCtaPagar, , , lop, , ) '

                '--------------------------------------------
                '--------------------------------------------
                debug = 4

                'Dim resumenPago As String = "PROVEEDOR" & vbTab & "AGRUPADOR" & vbTab & "CTA.CONT." & vbTab & "DESCR. E IMP."

                'For Each filaServ As DataRow In tablaServiciosPagar.Rows

                '    Dim descripcionServ = filaServ("Descripcion").ToString().Replace("INGRESO COBROS ", "")

                '    'If descripcionServ.Length <= 40 Then
                '    '    descripcionServ = descripcionServ.PadRight(40, " ")
                '    'Else
                '    '    descripcionServ = descripcionServ.Substring(0, 40)
                '    'End If

                '    resumenPago &= vbCrLf & filaServ("Proveedor") & vbTab & vbTab & filaServ("CRI_Agrupador") & vbTab & vbTab &
                '        filaServ("cuentacontable") & vbTab & vbTab & descripcionServ &
                '        " " & String.Format("{0:C}", filaServ("Importe"))
                'Next


                'If MessageBox.Show(resumenPago, "¿Está seguro que desea pagar los siguientes servicios?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then


                Dim LotesGenerados As String = ""
                Using frm As New Loader_Pagos("Generando pagos...", dtServ, tablaServiciosPagar, DtpFechaini.Value, DtpFechafin.Value)
                    frm.ShowDialog()
                    LotesGenerados = frm.LotesGenerados
                    frm.Dispose()
                End Using
                'debug = 12

                If LotesGenerados <> "" Then
                    MessageBox.Show("Proceso Terminado! Lotes Generados: " & LotesGenerados, "Pagos")
                End If

            ElseIf resp = System.Windows.Forms.DialogResult.No Then
                'Try
                '    File.Delete(rutarchivo)
                'Catch ex As Exception
                '    MsgBox(ex.Message.ToString, MsgBoxStyle.Critical)
                'End Try
                'transaccion.Rollback()


                Exit Sub
            End If
            'End If

        Catch ex As Exception
            Dim rutaPag As DataTable = sqls.DevuelveDatos(cnx, "select * from T_RutasFTP where idruta=4",
                                                          , , , , )
            'transaccion.Rollback()
            'For Each f In Directory.GetFiles(rutaPag.Rows(0)("RutaOrigen"))
            '    File.Delete(f)
            'Next
            MessageBox.Show(" ERROR : " & ex.Message & ". DEBUG:" & debug, "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error)
            bandera = False
        Finally
            cnx.Close()
        End Try
    End Sub
#End Region
    Public Sub CargaDTA(ByVal str As String, ByVal strLog As String, ByVal CTL As String, ByVal VentanaDeSololomon As String)

        'Dim MAil As New FuncSQL.EnviaMail("192.168.100.8")

        Dim RutaArchivosImportar As String = CTL

        Dim debug As String = "1"

        Try

            ' linea para llamar DTA

            If IO.File.Exists(str) Then

                Dim Ruta As String = VentanaDeSololomon

                Dim Parametros As String = " [TI]TM=2 [TI]TC=" & RutaArchivosImportar & " [TI]TD=" & str & " [TI]TO=" & strLog & " [TI]TL=1 [TI]TE=1 [TI]Minimize=Y"

                debug = "1.1"

                Dim myProcess As System.Diagnostics.Process = New System.Diagnostics.Process()

                myProcess.StartInfo.FileName = Ruta '"S:\SL\Applications\OM\0301000.exe" 'Ruta



                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal



                myProcess.StartInfo.UseShellExecute = True

                myProcess.StartInfo.Arguments = Parametros





                debug = "2"

                myProcess.Start()

                debug = "3"

                myProcess.WaitForExit()

            Else

                Throw New Exception("No se encontro el Achivo DTA:" & str)

            End If

        Catch ex As Exception

            Throw New Exception("Error al cargar DTA, Correrlo manualmente:" & str &
                                            ", DTA:" & str & ", CTL:" & CTL & ", LOG:" & strLog &
                                            ", Ventana:" & VentanaDeSololomon & ", ERROR: " & ex.Message & ", Debug:" & debug)

        End Try

    End Sub
    Private Sub Garchivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Garchivo.Click
        'variables mandar correo servicio en particular
        Dim adjunto As Attachment
        Dim email As System.Net.Mail.MailMessage
        Dim smtpMail As System.Net.Mail.SmtpClient
        Dim correotienda As String = "rtorres@mikiosko.mx"
        Dim direcciones(5) As String

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet
        Dim bandera As Boolean
        Dim x As Integer
        Dim y As Integer
        Dim mensaje As String = "hacer pago"
        Dim Caption As String = "CONFIRMACION DE PAGO"
        Dim boton As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim resp As DialogResult
        Dim mes As String
        Dim dia As String
        Dim rutarchivo As String = String.Empty
        mes = DateTime.Now.ToString("MMMM")
        'dial = DateTime.Now.ToString("dddd")
        dia = Now.Day
        dia = dia - 1

        If dgservicio.Rows.Count = 1 Then
            MsgBox("faltan datos")
        Else
            If CbxServicio.Text = "" Then
                MessageBox.Show("Seleccionar Servicio")
            Else
                If sumaimporte.Text = 0 Then
                    MessageBox.Show("no hay datos")
                Else
                    Try
                        'Añadimos el Libro al programa, y la hoja al libro
                        exLibro = exApp.Workbooks.Add
                        exHoja = exLibro.Worksheets.Add()
                        exHoja.Columns(1).NumberFormat = "@"
                        exHoja.Columns(4).NumberFormat = "@"
                        exHoja.Columns(5).NumberFormat = "#,##0.00"
                        ' ¿Cuantas columnas y cuantas filas?
                        Dim NCol As Integer = dgservicio.ColumnCount()
                        Dim NRow As Integer = dgservicio.RowCount
                        x = NCol
                        y = NRow
                        NCol = NCol - 2
                        'Aqui recorremos todas las filas, y por cada fila todas las columnas y vamos escribiendo.
                        For i As Integer = 1 To NCol
                            exHoja.Cells.Item(3, i) = dgservicio.Columns(i - 1).Name.ToString
                        Next
                        For Fila As Integer = 0 To NRow - 1
                            For Col As Integer = 0 To NCol - 1
                                exHoja.Cells.Item(Fila + 4, Col + 1) = Trim(dgservicio.Rows(Fila).Cells(Col).Value)
                            Next
                        Next
                        exHoja.Cells.Item(y + 4, x - 3) = "TOTAL"
                        exHoja.Cells.Item(y + 4, x - 2) = Me.sumaimporte.Text
                        'Titulo en negrita, Alineado al centro y que el tamaño de la columna se ajuste al texto
                        exHoja.Rows.Item(3).Font.Bold = 1
                        exHoja.Rows.Item(3).HorizontalAlignment = 3
                        exHoja.Columns.AutoFit()
                        'Aplicación visible
                        exApp.Application.Visible = True
                        exLibro.SaveAs("c:\pagosjuntos\" & CbxServicio.Text & " del " & TextBox1.Text & " " + "al" + " " & dia & " " & mes, , "", "", False, False)
                        rutarchivo = exLibro.FullName
                        exApp.Quit()
                        ReleaseCom(exApp)
                        exHoja = Nothing
                        exLibro = Nothing
                        exApp = Nothing
                        bandera = True
                    Catch ex As Exception
                        MessageBox.Show(" ERROR : " & ex.Message & " --UtilForm.ExportarGridADocumentoExcel", "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        bandera = False
                    End Try

                    If bandera = True Then
                        resp = MessageBox.Show(mensaje, Caption, boton)
                        If resp = System.Windows.Forms.DialogResult.Yes Then
                            Select Case CbxServicio.Text
                                Case "COBROS CFE"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 5))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS IMPUESTOS COLIMA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 7))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                Case "COBROS CIAPACOV"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 13))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS CASEFON"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 14))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS AYUNTAMIENTO COLIMA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 15))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS SEAPAL (SOLO VALLARTA)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 17))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS MEGACABLE  (SOLO ZONA COLIMA)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 16))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS TELMEX"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 18))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS SKY"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 19))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS INFONAVIT"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 20))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS OROMAPAS"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 21))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                Case "COBROS LA GUADALUPANA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 22))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS DISH"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 24))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS AYUNTAMIENTO MANZANILLO"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 26))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS AYUNTAMIENTO VILLA DE ALVAREZ"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 27))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""



                                Case "COBROS TELECABLE"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 28))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS UNIVERSIDAD VIZCAYA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 55))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,42,43,55) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,42,43,55) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS TARJETAS SEFIDEC"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 41))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (REINSCRIPCION)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 39))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (INGLES)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 40))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                Case "COBROS ANUNCIO INMOBILIARI"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 46))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""


                                Case "COBROS PAYNET"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 44))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""


                                Case "COBROS UNIVERSIDAD DE LA VERA CRUZ"
                                    lop.Clear()
                                    qry = ""
                                    direcciones = Nothing
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 49))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                    'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio = @idservicio "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If


                                Case "COBROS AYUNTAMIENTO DE PUERTO VALLARTA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 47))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                    '--buscar en tabla las direcciones de correo electronico
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio = @idservicio "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If


                                Case "COBROS COMAPAT"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 48))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""


                                Case "COBROS CAPDAM"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 56))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""


                                Case "AUTOGESTION CFE"
                                    lop.Clear()
                                    qry = ""
                                    direcciones = Nothing
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo in (50,51) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,'AUTOGESTION CFE' AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps LEFT JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (50,51) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                    'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio in (50,51) "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If

                                Case "PREPAGO AUTOTRANSPORTE"
                                    lop.Clear()
                                    qry = ""
                                    direcciones = Nothing
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo in (59) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (59) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""

                                    'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio in (59) "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If


                                Case "COBROS AYUNTAMIENTO MAZATLAN (PREDIAL)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 61))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                    '--buscar en tabla las direcciones de correo electronico
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio = @idservicio "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If

                                Case "COBROS JUMAPAM"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 62))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                    '--buscar en tabla las direcciones de correo electronico
                                    qry = ""
                                    qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " &
                                          " WHERE C_IdServicio = @idservicio "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    If dt.Rows(0).Item("c_enviarcorreo") Then
                                        direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                        For i = 1 To direcciones.Length
                                            'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                            email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                            adjunto = New Attachment(rutarchivo)
                                            email.Attachments.Add(adjunto)
                                            Try
                                                smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                                email.IsBodyHtml = False
                                                smtpMail.UseDefaultCredentials = False
                                                smtpMail.Port = 25
                                                smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                                smtpMail.Send(email)
                                                email.Attachments.Dispose()
                                            Catch excpt As System.Exception
                                                MessageBox.Show("correo no enviado")
                                                Debug.WriteLine(excpt.Message)
                                            End Try
                                        Next
                                        MessageBox.Show("correo enviado correctamente")
                                    End If


                                Case "COBROS UNIVERSIDAD MULTITECNICA PROFESIONAL"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 65))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                    '--buscar en tabla las direcciones de correo electronico
                                    'qry = ""
                                    'qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " & _
                                    '      " WHERE C_IdServicio = @idservicio "
                                    'dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    'If dt.Rows(0).Item("c_enviarcorreo") Then
                                    '    direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                    '    For i = 1 To direcciones.Length
                                    '        'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                    '        email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                    '        adjunto = New Attachment(rutarchivo)
                                    '        email.Attachments.Add(adjunto)
                                    '        Try
                                    '            smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                    '            email.IsBodyHtml = False
                                    '            smtpMail.UseDefaultCredentials = False
                                    '            smtpMail.Port = 25
                                    '            smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                    '            smtpMail.Send(email)
                                    '            email.Attachments.Dispose()
                                    '        Catch excpt As System.Exception
                                    '            MessageBox.Show("correo no enviado")
                                    '            Debug.WriteLine(excpt.Message)
                                    '        End Try
                                    '    Next
                                    '    MessageBox.Show("correo enviado correctamente")
                                    'End If


                                Case "COBROS IZZI (CAPTURA MANUAL)"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 67))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE ps.CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    'llena otra vez grid con consulta vacia
                                    qry = ""
                                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                          " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    dgservicio.DataSource = dt
                                    Me.sumaimporte.Text = ""
                                    '--buscar en tabla las direcciones de correo electronico
                                    'qry = ""
                                    'qry = " SELECT c_enviarcorreo,C_EmailContacto1,C_IdServicio FROM historialkiosko.dbo.T_ContactosProvServ AS tcps " & _
                                    '      " WHERE C_IdServicio = @idservicio "
                                    'dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                    'If dt.Rows(0).Item("c_enviarcorreo") Then
                                    '    direcciones = dt.Rows(0).Item("c_emailcontacto1").ToString.Split(";")
                                    '    For i = 1 To direcciones.Length
                                    '        'Establesco El Email para enviar archivo sin borrar a encargado de servicio
                                    '        email = New System.Net.Mail.MailMessage(correotienda, direcciones(i - 1), "Envio Pago de servicios", "envio archivos")
                                    '        adjunto = New Attachment(rutarchivo)
                                    '        email.Attachments.Add(adjunto)
                                    '        Try
                                    '            smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                                    '            email.IsBodyHtml = False
                                    '            smtpMail.UseDefaultCredentials = False
                                    '            smtpMail.Port = 25
                                    '            smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, "fghjmnb6")
                                    '            smtpMail.Send(email)
                                    '            email.Attachments.Dispose()
                                    '        Catch excpt As System.Exception
                                    '            MessageBox.Show("correo no enviado")
                                    '            Debug.WriteLine(excpt.Message)
                                    '        End Try
                                    '    Next
                                    '    MessageBox.Show("correo enviado correctamente")
                                    'End If

                                Case "UNIV VIZCAYA COLIMA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 68))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    dgservicio.DataSource = vbNull
                                Case "COLEGIO VIZCAYA COLIMA"
                                    lop.Clear()
                                    qry = ""
                                    lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
                                    lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
                                    lop.Add(sqls.CreadbParametros("@idservicio", 54))
                                    direcciones = Nothing
                                    qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                                          "WHERE CRI_Codigo =@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"
                                    sqls.ComandoSQL(cnx, qry, lop, , , , )
                                    dgservicio.DataSource = vbNull
                            End Select
                            '  My.Computer.FileSystem.DeleteFile(archivo, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)

                        End If
                        If resp = System.Windows.Forms.DialogResult.No Then
                            Try
                                File.Delete(rutarchivo)
                            Catch ex As Exception
                                MsgBox(ex.Message.ToString, MsgBoxStyle.Critical)
                            End Try
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub ReleaseCom(ByVal o As Object)

        Try
            ' Liberamos el objeto COM
            System.Runtime.InteropServices.Marshal.ReleaseComObject(o)

        Catch
            ' Sin implementación

        Finally
            o = Nothing

        End Try

    End Sub





    Private Sub DtpFechafin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DtpFechafin.ValueChanged
        qry = ""
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@FechaIni", DtpFechaini.Value))
        lop.Add(sqls.CreadbParametros("@FechaFin", DtpFechafin.Value))
        Select Case CbxServicio.Text
            Case "COBROS CFE"
                lop.Add(sqls.CreadbParametros("@idservicio", 5))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS IMPUESTOS COLIMA"
                lop.Add(sqls.CreadbParametros("@idservicio", 7))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS CIAPACOV"
                lop.Add(sqls.CreadbParametros("@idservicio", 13))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS CASEFON"
                lop.Add(sqls.CreadbParametros("@idservicio", 14))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS AYUNTAMIENTO COLIMA"
                lop.Add(sqls.CreadbParametros("@idservicio", 15))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS SEAPAL (SOLO VALLARTA)"
                lop.Add(sqls.CreadbParametros("@idservicio", 17))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS MEGACABLE  (SOLO ZONA COLIMA)"
                lop.Add(sqls.CreadbParametros("@idservicio", 16))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS TELMEX"
                lop.Add(sqls.CreadbParametros("@idservicio", 18))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS SKY"
                lop.Add(sqls.CreadbParametros("@idservicio", 19))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt
            Case "COBROS INFONAVIT"
                lop.Add(sqls.CreadbParametros("@idservicio", 20))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS OROMAPAS"
                lop.Add(sqls.CreadbParametros("@idservicio", 21))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS LA GUADALUPANA"
                lop.Add(sqls.CreadbParametros("@idservicio", 22))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS DISH"
                lop.Add(sqls.CreadbParametros("@idservicio", 24))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS AYUNTAMIENTO MANZANILLO"
                lop.Add(sqls.CreadbParametros("@idservicio", 26))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS AYUNTAMIENTO VILLA DE ALVAREZ"
                lop.Add(sqls.CreadbParametros("@idservicio", 27))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS TELECABLE"
                lop.Add(sqls.CreadbParametros("@idservicio", 28))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS UNIVERSIDAD VIZCAYA"
                lop.Add(sqls.CreadbParametros("@idservicio", 55))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,55) AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS TARJETAS SEFIDEC"
                lop.Add(sqls.CreadbParametros("@idservicio", 41))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (REINSCRIPCION)"
                lop.Add(sqls.CreadbParametros("@idservicio", 39))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (INGLES)"
                lop.Add(sqls.CreadbParametros("@idservicio", 40))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS ANUNCIO INMOBILIARI"
                lop.Add(sqls.CreadbParametros("@idservicio", 46))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS PAYNET"
                lop.Add(sqls.CreadbParametros("@idservicio", 44))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS UNIVERSIDAD DE LA VERA CRUZ"
                lop.Add(sqls.CreadbParametros("@idservicio", 49))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS AYUNTAMIENTO DE PUERTO VALLARTA"
                lop.Add(sqls.CreadbParametros("@idservicio", 47))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt


            Case "COBROS COMAPAT"
                lop.Add(sqls.CreadbParametros("@idservicio", 48))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt


            Case "COBROS CAPDAM"
                lop.Add(sqls.CreadbParametros("@idservicio", 56))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "AUTOGESTION CFE"
                lop.Add(sqls.CreadbParametros("@idservicio", 50))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "AUTOGESTION CFE"
                lop.Add(sqls.CreadbParametros("@idservicio", 59))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS AYUNTAMIENTO MAZATLAN (PREDIAL)"
                lop.Add(sqls.CreadbParametros("@idservicio", 61))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS JUMAPAM"
                lop.Add(sqls.CreadbParametros("@idservicio", 62))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS UNIVERSIDAD MULTITECNICA PROFESIONAL"
                lop.Add(sqls.CreadbParametros("@idservicio", 65))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

            Case "COBROS IZZI (CAPTURA MANUAL)"
                lop.Add(sqls.CreadbParametros("@idservicio", 67))
                qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                      " FROM ConciliacionServicios AS ps INNER JOIN  compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                dgservicio.DataSource = dt

        End Select

        'Sumar una Columna
        Dim Total As Single
        'Dim Col As Integer = Me.dgservicio.CurrentCell.ColumnIndex()
        Dim col As Integer
        col = 4
        For Each row As DataGridViewRow In Me.dgservicio.Rows()
            Total += Val(row.Cells(col).Value)
        Next
        Me.sumaimporte.Text = Format(Total, "$ #,##0.00") 'Total.ToString
        'dgservicio.Rows.Add()
    End Sub



    Private Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    'Private Sub cbxconsulta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    qry = ""
    '    lop.Clear()
    '    lop.Add(sqls.CreadbParametros("@FechaIni", dtxfechaini.Value))
    '    lop.Add(sqls.CreadbParametros("@FechaFin", dtxfechafin.Value))

    '    Select Case cbxconsulta.Text
    '        Case "COBROS CFE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 5))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                  "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS IMPUESTOS COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 7))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                 "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS CIAPACOV"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 13))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                 "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS CASEFON"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 14))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                 "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS AYUNTAMIENTO COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 15))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                  "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS SEAPAL (SOLO VALLARTA)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 17))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                  "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS MEGACABLE  (SOLO ZONA COLIMA)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 16))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                 "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS TELMEX"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 18))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS SKY / VETV"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 19))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS INFONAVIT"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 20))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS OROMAPAS"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 21))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS LA GUADALUPANA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 22))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)
    '        Case "COBROS DISH"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 24))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS AYUNTAMIENTO MANZANILLO"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 26))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS AYUNTAMIENTO VILLA DE ALVAREZ"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 27))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS TELECABLE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 28))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS UNIVERSIDAD VIZCAYA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 55))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,42,43,55) AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS TARJETAS SEFIDEC"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 41))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 39))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (INGLES)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 40))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS ANUNCIO INMOBILIARI"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 46))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS PAYNET"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 44))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS UNIVERSIDAD DE LA VERA CRUZ"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 49))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS AYUNTAMIENTO PUERTO VALLARTA"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 47))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS COMAPAT"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 48))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)


    '        Case "COBROS CAPDAM"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 56))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "AUTOGESTION CFE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 50))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "PREPAGO AUTOTRANSPORTE"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 59))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS AYUNTAMIENTO MAZATLAN (PREDIAL)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 61))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS JUMAPAM"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 62))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS UNIVERSIDAD MULTITECNICA PROFESIONAL"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 65))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '        Case "COBROS IZZI (CAPTURA MANUAL)"
    '            lop.Add(sqls.CreadbParametros("@idservicio", 67))
    '            qry = "SELECT DISTINCT fechapago,CRI_Codigo FROM dbo.ConciliacionServicios  " & _
    '                   "WHERE fecha BETWEEN @fechaini AND @fechafin AND CRI_Codigo=@idservicio AND fechapago IS NOT NULL "
    '            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '            sqls.LlenaCombo(cbxfechapago, "fechapago", "CRI_Codigo", dt)

    '    End Select

    'End Sub

    Private Sub cbxfechapago_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bandera = 1
    End Sub


    'Private Sub cbxfechapago_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)

    '    If bandera = 0 Then
    '    Else
    '        If cbxfechapago.Text = "" Then
    '        Else
    '            qry = ""
    '            lop.Clear()
    '            lop.Add(sqls.CreadbParametros("@fechapago", CDate(cbxfechapago.Text)))
    '            Select Case cbxconsulta.Text
    '                Case "COBROS CFE"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 5))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                          "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS IMPUESTOS COLIMA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 7))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS CIAPACOV"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 13))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS CASEFON"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 14))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS AYUNTAMIENTO COLIMA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 15))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                      "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS SEAPAL (SOLO VALLARTA)"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 17))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                        "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS MEGACABLE  (SOLO ZONA COLIMA)"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 16))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                      "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS TELMEX"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 18))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS SKY / VETV"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 19))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS INFONAVIT"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 20))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS OROMAPAS"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 21))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS LA GUADALUPANA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 22))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS DISH"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 24))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS AYUNTAMIENTO MANZANILLO"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 26))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt
    '                Case "COBROS AYUNTAMIENTO VILLA DE ALVAREZ"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 27))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt


    '                Case "COBROS TELECABLE"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 28))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS UNIVERSIDAD VIZCAYA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 55))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo in (29,30,31,32,33,34,35,36,37,38,42,43,55)"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS TARJETAS SEFIDEC"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 41))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 39))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS INSTITUTO TECNOLOGICO DE COLIMA (INGLES)"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 40))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS ANUNCIO INMOBILIARI"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 46))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt


    '                Case "COBROS PAYNET"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 44))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS UNIVERSIDAD DE LA VERA CRUZ"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 49))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS AYUNTAMIENTO PUERTO VALLARTA"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 47))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS COMAPAT"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 48))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS CAPDAM"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 56))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "AUTOGESTION CFE"
    '                    'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo in (50,51)"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt


    '                Case "PREPAGO AUTOTRANSPORTE"
    '                    'lop.Add(sqls.CreadbParametros("@idservicio", "(50,51)"))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo in (59)"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS AYUNTAMIENTO MAZATLAN (PREDIAL)"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 61))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS JUMAPAM"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 62))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS UNIVERSIDAD MULTITECNICA PROFESIONAL"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 65))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '                Case "COBROS IZZI (CAPTURA MANUAL)"
    '                    lop.Add(sqls.CreadbParametros("@idservicio", 67))
    '                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio , CRI_Codigo as Servicio,Fecha,Referencia,Importe,Archivo,Fechapago FROM dbo.ConciliacionServicios  " & _
    '                       "WHERE DATEADD(dd,DATEDIFF(dd,0,fechapago),0)=DATEADD(dd,DATEDIFF(dd,0,@fechapago),0) and CRI_Codigo=@idservicio"
    '                    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                    tdgconsulta.DataSource = dt

    '            End Select
    '            ''Sumar una Columna
    '            Dim Total As Single
    '            ''Dim Col As Integer = Me.dgservicio.CurrentCell.ColumnIndex()
    '            Dim col As Integer
    '            col = 4
    '            Dim dtsuma As DataTable = tdgconsulta.DataSource
    '            For col = 0 To dt.Rows.Count - 1
    '                Total += Val(dtsuma.Rows(col).Item("Importe"))
    '            Next
    '            Me.sumaconsulta.Text = Format(Total, "$ #,##0.00") 'Total.ToString  ' ''dgservicio.Rows.Add()

    '        End If


    '    End If
    'End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        qry = ""
        lop.Clear()
        qry = "SELECT convert(NVARCHAR(5),foltda_codigo)+'-'+convert(NVARCHAR(4),folest_codigo)+'-'+convert(NVARCHAR(4),foldoc_codigo)+'-'+ convert(NVARCHAR(15),folconsecutivo) as foliocc,ing_fecha,ing_importe,CRI_Codigo,tienda,estacion,consecutivo,referencia,importe  " &
              "FROM compucaja.dbo.Ingresos I  " &
              "INNER join compucaja.dbo.ConciliacionServicios AS pg ON CONVERT(INT, PG.TIENDA) = I.FolTda_Codigo AND pg.Referencia=i.Ing_Referencia AND pg.CRI_Codigo = i.CI_Codigo AND i.FolConsecutivo=pg.Consecutivo " &
              "WHERE i.ing_fecha >= '20130801' AND CONVERT(VARCHAR(1),pg.conciliado)='0' AND i.Ing_Importe <> CONVERT(DECIMAL(10),pg.Importe) AND Ing_Cancelado=0  "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        tdgnoconciliados.DataSource = dt
        MsgBox("proceso terminado")
        tdgnoconciliados.Splits(0).DisplayColumns(3).FetchStyle = True
    End Sub

    Private Sub tdgnoconciliados_DoubleClick(sender As Object, e As EventArgs) Handles tdgnoconciliados.DoubleClick
        qry = ""
        lop.Clear()
        If mcambio.Text <> "" Then
            lop.Add(sqls.CreadbParametros("@id_servicio1", tdgnoconciliados.Item(tdgnoconciliados.Row, "CRI_Codigo")))
            lop.Add(sqls.CreadbParametros("@estacion1", tdgnoconciliados.Item(tdgnoconciliados.Row, "estacion")))
            lop.Add(sqls.CreadbParametros("@tienda1", tdgnoconciliados.Item(tdgnoconciliados.Row, "tienda")))
            lop.Add(sqls.CreadbParametros("@consecutivo1", tdgnoconciliados.Item(tdgnoconciliados.Row, "consecutivo")))
            lop.Add(sqls.CreadbParametros("@referencia1", tdgnoconciliados.Item(tdgnoconciliados.Row, "referencia")))
            lop.Add(sqls.CreadbParametros("@importe1", Decimal.Parse(mcambio.Text)))
            '  lop.Add(sqls.CreadbParametros("@observaciones1", tdgnoconciliados.Item(tdgnoconciliados.Row, "observaciones")))
            Dim mensaje As String = "Modificar Registro"
            Dim Caption As String = "CONFIRMACION DE MODIFICACION"
            Dim boton As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim resp As DialogResult
            resp = MessageBox.Show(mensaje, Caption, boton)
            If resp = System.Windows.Forms.DialogResult.Yes Then
                qry = "update ConciliacionServicios set importe=@importe1 " &
                "WHERE CRI_Codigo=@id_servicio1 AND estacion=@estacion1 AND tienda=@tienda1 " &
                "AND consecutivo=@consecutivo1 AND referencia=@referencia1"
                sqls.ComandoSQL(cnx, qry, lop, , , , )
                mcambio.Text = ""
                'llena otra vez true dbgrid con datos actuales
                qry = ""
                lop.Clear()
                ' lop.Add(sqls.CreadbParametros("@FechaIni", dtnoconciliados3.Value))
                'lop.Add(sqls.CreadbParametros("@FechaFin", dtnoconciliados2.Value))
                'CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio
                qry = "SELECT convert(char(4),foltda_codigo)+'-'+convert(char(1),folest_codigo)+'-'+ convert(char(1),foldoc_codigo)+'-'+ convert(char(10),folconsecutivo) as foliocc,ing_fecha,ing_referencia,ing_importe,ci_codigo,CRI_Codigo,tienda,estacion,Consecutivo,referencia,importe  " &
              "FROM compucaja.dbo.Ingresos I  " &
              "INNER join compucaja.dbo.ConciliacionServicios AS pg ON CONVERT(INT, PG.TIENDA) = I.FolTda_Codigo AND pg.Referencia=i.Ing_Referencia AND pg.CRI_Codigo = i.CI_Codigo AND i.FolConsecutivo=pg.Consecutivo " &
              "WHERE i.ing_fecha >= '20130801' AND CONVERT(VARCHAR(1),pg.conciliado)='0' AND i.Ing_Importe <> CONVERT(DECIMAL(10),pg.Importe) AND Ing_Cancelado=0  "
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                tdgnoconciliados.DataSource = dt
                tdgnoconciliados.Splits(0).DisplayColumns(3).FetchStyle = True
            Else
                mcambio.Text = ""
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        qry = "Select * from T_RutasFTP where IdRuta=4"
        Dim rutaPag As DataTable = sqls.DevuelveDatos(cnx, qry, , , )

        'Dim ds As DataSet
        Dim adjunto As Attachment
        'ds = New DataSet
        'ds.ReadXml(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\COMPUCAJA\\CCADMIN\\Config.ini")
        Dim email As System.Net.Mail.MailMessage
        Dim smtpMail As System.Net.Mail.SmtpClient
        Dim correotienda As String = txtCorreo.Text '"pserviciosk@gocsa.mx" '"rtorres@mikiosko.mx" '"k" & ds.Tables(0).Rows(0)("CodigoTienda") & "@gocsa.com.mx"
        Dim directorio As New DirectoryInfo(rutaPag.Rows(0)("RutaOrigen") & "\")
        'Dim directorio As New DirectoryInfo("C:\Proyectos\KIOSKO3\Pago Servicios\ArchivosDetallados\")
        Dim directorios() As DirectoryInfo = directorio.GetDirectories
        Dim archivos() As FileInfo
        archivos = directorio.GetFiles
        If archivos.Length <= 0 Then
            MessageBox.Show("no hay archivos para enviar")
        Else
            'OBTENER LOS CORREOS DE KIOSKO
            email = New System.Net.Mail.MailMessage(correotienda, correotienda, "Envio Pago de servicios", "envio archivos embotelladora")
            qry = "Select TOP 1 EmailKiosko from T_ConceptosDeServicios where EmailKiosko<>'' and EmailKiosko is not null"
            dt = sqls.DevuelveDatos(cnx, qry, , , )
            If dt.Rows.Count > 0 Then 'ENVIO DE CORREO A CUENTAS DE KIOSKO
                For Each correoKio As String In dt.Rows(0)("EmailKiosko").ToString.Split(";")
                    email.CC.Add(correoKio)
                    'email.CC.Add("fheredia@gocsa.com.mx")
                    'email.CC.Add("jramos@gocsa.com.mx")
                    'email.CC.Add("ejimenez@gocsa.com.mx")
                    'email.CC.Add("rgutierrez@gocsa.com.mx")
                    'email.CC.Add("rtorres@mikiosko.mx")
                Next
                For Each d As FileInfo In archivos
                    adjunto = New Attachment(d.FullName)
                    email.Attachments.Add(adjunto)
                Next
                Try
                    smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                    email.IsBodyHtml = False
                    qry = "Select isnull ((Select TOP 1 FechaPago from ConciliacionServicios where " &
                        "LoteCxP<>'' and LoteCxP is not null order by FechaPago desc),'')"
                    dt = sqls.DevuelveDatos(cnx, qry, , , )
                    qry = "Select Distinct LoteCxP from ConciliacionServicios where " &
                        "LoteCxP<>'' and LoteCxP is not null and FechaPago Between '" & DateTime.Parse(dt.Rows(0)(0)).ToString("yyyy-MM-dd") &
                        " 00:00:00' and '" & DateTime.Parse(dt.Rows(0)(0)).ToString("yyyy-MM-dd") & " 23:59:59' order by LoteCxP"
                    dt = sqls.DevuelveDatos(cnx, qry, , , )
                    email.Body = "Numeros de Lote Cuenta Por Pagar:"
                    For Each lote As DataRow In dt.Rows
                        email.Body &= (IIf(email.Body = "Numeros de Lote Cuenta Por Pagar:", "", ", ")) & lote(0)
                    Next
                    smtpMail.UseDefaultCredentials = False
                    smtpMail.Port = 25
                    smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, txtPass.Text) '"fghjmnb6")
                    'smtpMail.EnableSsl = True
                    smtpMail.Send(email)
                    'MessageBox.Show("correo enviado correctamente")
                    email.Attachments.Dispose()
                    'For Each d As FileInfo In archivos
                    'File.Delete(d.FullName)
                    'Next
                Catch excpt As System.Exception
                    MessageBox.Show("Correo a oficina kiosko no enviado. Error: " & excpt.Message)

                    Debug.WriteLine(excpt.Message)
                    Exit Sub
                End Try
            End If
            'RECORRER ARCHIVOS Y ENVIAR CORREO A PROVEEDORES
            For Each d As FileInfo In archivos
                'Recuperar correos de proveedor
                lop.Clear()
                lop.Add(sqls.CreadbParametros("@serv", d.Name.Split("_")(0)))
                qry = "Select TOP 1 EmailProv from T_ConceptosDeServicios where Descripcion=@serv " &
                    "and EmailProv<>'' and EmailProv is not null"
                dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                If dt.Rows.Count > 0 Then 'ENVIO DE CORREO A CUENTAS DE KIOSKO
                    Dim correosProv As String() = dt.Rows(0)("EmailProv").ToString.Split(";")
                    email = New System.Net.Mail.MailMessage(correotienda, correosProv(0), "Envio Pago de servicios", "envio archivos a proveedor")

                    For Each correoKio As String In correosProv
                        If correoKio <> correosProv(0) Then
                            email.CC.Add(correoKio)
                        End If
                    Next
                    adjunto = New Attachment(d.FullName)
                    email.Attachments.Add(adjunto)
                    Try
                        smtpMail = New System.Net.Mail.SmtpClient("192.168.100.8")
                        email.IsBodyHtml = False
                        smtpMail.UseDefaultCredentials = False
                        smtpMail.Port = 25
                        'smtpMail.EnableSsl = True
                        smtpMail.Credentials = New System.Net.NetworkCredential(correotienda, txtPass.Text) '"fghjmnb6")
                        smtpMail.Send(email)
                        'MessageBox.Show("correo enviado correctamente")
                        email.Attachments.Dispose()
                    Catch excpt As System.Exception
                        MessageBox.Show("Correo a proveedor no enviado. Error: " & excpt.Message)

                        Debug.WriteLine(excpt.Message)
                        Exit Sub
                    End Try
                End If
                File.Delete(d.FullName)

            Next
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios", "Correo", txtCorreo.Text)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Vb and VBA Program Settings\PagoeServicios", "Password", txtPass.Text)
            MessageBox.Show("Envío de correos terminado.", "Información")
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        qry = "SELECT SV_Codigo AS IdServicio,SV_Nombre AS Nombre FROM compucaja.dbo.Servicios WHERE SV_Codigo IN (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,28,30,35,36,34,37,39,42,41,50,51) "
        '"UNION " & _
        '"SELECT SV_Codigo AS IdServicio,SV_Nombre AS Nombre FROM compucaja.dbo.Servicios  WHERE SV_Codigo=18"
        dt = sqls.DevuelveDatos(cnx, qry)
        sqls.LlenaCombo(CbxServicio, "Nombre", "IdServicio", dt)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Me.dgservicio.GetCellCount(
DataGridViewElementStates.Selected) > 0 Then
            Try
                ' Add the selection to the clipboard.
                Clipboard.SetDataObject(
                    Me.dgservicio.GetClipboardContent())
                ' Replace the text box contents with the clipboard text.
                ' Me.TextBox1.Text = Clipboard.GetText()
            Catch ex As System.Runtime.InteropServices.ExternalException
                MessageBox.Show("no contemplado")
                ' Me.TextBox1.Text = _
                '  "The Clipboard could not be accessed. Please try again."
            End Try
        End If

    End Sub

    Private Sub CbxServicio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxServicio.SelectedIndexChanged

    End Sub

    Private Sub cbxconsulta_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    'Private Sub Button6_Click(sender As Object, e As EventArgs)
    '    My.Computer.Clipboard.Clear()
    '    Dim strTemp As String
    '    Dim row As Integer
    '    Dim i As Integer
    '    strTemp = ""
    '    If tdgconsulta.SelectedRows.Count > 0 Then
    '        For i = 0 To tdgconsulta.Splits(0).DisplayColumns.Count - 1
    '            If tdgconsulta.Splits(0).DisplayColumns(i).Visible = True Then
    '                strTemp = strTemp & tdgconsulta.Columns(i).Caption & vbTab
    '            End If
    '        Next i
    '        strTemp = strTemp & vbCrLf
    '        For Each row In tdgconsulta.SelectedRows
    '            For i = 0 To tdgconsulta.Splits(0).DisplayColumns.Count - 1
    '                If tdgconsulta.Splits(0).DisplayColumns(i).Visible = True Then
    '                    strTemp = strTemp & tdgconsulta.Columns(i).CellText(row) & vbTab
    '                End If
    '            Next
    '            strTemp = strTemp & vbCrLf
    '        Next
    '        System.Windows.Forms.Clipboard.SetDataObject(strTemp, True)
    '    Else
    '    End If

    'End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        My.Computer.Clipboard.Clear()
        Dim strTemp As String
        Dim row As Integer
        row = 0
        strTemp = " "
        If tdgnoconciliados.SelectedRows.Count >= 0 Then
            For Each row In tdgnoconciliados.SelectedRows
                strTemp = tdgnoconciliados.Columns(tdgnoconciliados.Col).CellText(row)
            Next
            System.Windows.Forms.Clipboard.SetDataObject(Trim(strTemp), True)
        Else
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim mylog As String = ""
        Dim ds As New DataSet
        Dim archivos() As String
        Dim sLine As String = ""
        Dim da As New SqlCommand

        Dim fs_infos() As FileInfo
        Dim nomarch As String
        Dim debug As String = "1"
        cnx.Open()
        qry = "Select * from T_RutasFTP where IdRuta=1"
        Dim RutasConc As DataTable = sqls.DevuelveDatos(cnx, qry, , , )

        'Dim dir_info As New DirectoryInfo("C:\Proyectos\KIOSKO3\Pago Servicios\AutoGOrigen")
        Dim dir_info As New DirectoryInfo(RutasConc.Rows(0)("RutaOrigen"))
        fs_infos = dir_info.GetFiles("*.txt")
        Dim nombre As String
        nombre = " "
        debug = "2"
        Try
            Dim ccomando As New SqlCommand


            debug = "2.05"

            ccomando = New SqlCommand
            ccomando.Connection = cnx
            ccomando.CommandText = "sp_conciliar_atrasados"
            ccomando.ExecuteNonQuery()
            '
            debug = "2.1"
            For Each fs_info As FileInfo In fs_infos
                nombre = fs_info.Name
                nomarch = Mid(nombre, 1, 11)
                If nomarch = "P01.KIOSKO." Or nomarch = "P01.MEGAKIO" Then
                    'archivos = IO.Directory.GetDirectories("\\svrservices\DatosFTP\Servicios\ConciliacionServicios")
                    archivos = IO.Directory.GetDirectories(RutasConc(0)("RutaOrigen"))


                    Dim dir As String = RutasConc.Rows(0)("RutaOrigen") & "\" & fs_info.Name
                    debug = "2.2"
                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    ccomando.CommandText = "Delete from T_Conciliar"
                    ccomando.ExecuteNonQuery()

                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    debug = "3"
                    ccomando.CommandText = "exec SP_Bulk_Conciliar '" & dir & "'"
                    ccomando.ExecuteNonQuery()
                    debug = "4"
                    Dim act As String = "exec sp_Conciliar '" & nombre.Trim() & "'"
                    da = New SqlCommand(act, cnx)
                    da.ExecuteNonQuery()
                    debug = "5.1"


                    Dim sArchivoOrigen As String = RutasConc.Rows(0)("RutaOrigen") & "\" & nombre.Trim
                    Dim sRutaDestino As String = RutasConc.Rows(0)("RutaDestino") & "\" & nombre.Trim
                    My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)


                ElseIf nomarch.ToUpper().Contains("CINEPOLIS") Then 'BOLETOS DE CINE
                    debug = "6"
                    dt = sqls.DevuelveDatos(cnx,
                        "Select Top 1 CRI_Codigo from T_ConceptosDeServicios where DEscripcion like '%CINEPOLIS%'")
                    Dim CodCinepolis As String = dt.Rows(0)(0)

                    archivos = IO.Directory.GetDirectories(RutasConc(0)("RutaOrigen"))

                    Dim dir As String = RutasConc.Rows(0)("RutaOrigen") & "\" & fs_info.Name

                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    ccomando.CommandText = "Delete from T_ArchivoCine"
                    debug = "7"
                    ccomando.ExecuteNonQuery()

                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    ccomando.CommandText = "exec SP_Bulk_Cine '" & dir & "'"
                    debug = "8"
                    ccomando.ExecuteNonQuery()

                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    ccomando.CommandText = "INSERT INTO [PagoDeServicios].[dbo].[ConciliacionServicios] " &
            "([CRI_Codigo],[Estacion],[Tienda],[Consecutivo],[Fecha],[Hora],[Art_Codigo],[Referencia],[Importe],[Archivo]" &
           ",[Conciliado],[Observaciones],[Folio]) " &
            "Select " & CodCinepolis & ",isnull(i.FolEst_Codigo,0),isnull(i.FolTda_Codigo,ar.Tienda),isnull(i.FolConsecutivo,0)," &
            "ar.Fecha,'00:00:00',isnull(i.CC_ArtCodigo,''),ar.Referencia,isnull(i.CC_Ingreso,0),'" & nombre & "'," &
            "case when i.FolConsecutivo is null then 0 else 1 end,ar.Codigo," &
            "case when i.FolConsecutivo is null then NULL else (cast(i.FolTda_Codigo as varchar)+'_'+cast(i.FolEst_Codigo as varchar)" &
                                    "+'_'+cast(i.FolDoc_Codigo as varchar)+'_'+cast(i.FolConsecutivo as varchar)" &
                                    "+'_'+cast(i.Consecutivo as varchar)) end from " &
                                    "T_ArchivoCine as ar left join compucaja.dbo.CorresponsaliasComisiones as i on ar.Referencia=i.CC_TrxID where isnumeric(ar.Tienda)=1"
                    debug = "9"
                    ccomando.ExecuteNonQuery()

                    ccomando = New SqlCommand
                    ccomando.Connection = cnx
                    ccomando.CommandText = "Delete from T_ArchivoCine"
                    debug = "10"
                    ccomando.ExecuteNonQuery()

                    Dim sArchivoOrigen As String = RutasConc.Rows(0)("RutaOrigen") & "\" & nombre.Trim
                    Dim sRutaDestino As String = RutasConc.Rows(0)("RutaDestino") & "\" & nombre.Trim
                    My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)
                End If
            Next




            ' da.Connection.Close()
            'Dim mflsNuevoLog As New FileStream("c:\milogconciliacion.log", FileMode.Create, FileAccess.Write) '******
            'Dim mstwLineaLog As New StreamWriter(mflsNuevoLog)                            '******
            'mstwLineaLog.BaseStream.Seek(0, SeekOrigin.End)                                   '******
            'mstwLineaLog.WriteLine(mylog)
        Catch ex As Exception
            '     sqls.GrabaTextoEnArchivo("Error al conciliar Servicios : " + ex.Message + " - Arrchivo " + nombre.Trim + " Detalle ultima linea -  Tienda : " + linea(2).Trim & " - Estacion: " & linea(4).Trim & " - Referencia: " & linea(9).Trim & " - Servicio:" & id_serviciox.Trim & "|")
            MsgBox(serviciox & " Error de Conexión - debug: " & debug & "-" & ex.Message)
        Finally
            qry = ""
            lop.Clear()
            qry = "SELECT convert(char(4),foltda_codigo)+'-'+convert(char(1),folest_codigo)+'-'+ convert(char(1),foldoc_codigo)+'-'+ convert(char(10),folconsecutivo) as foliocc,ing_fecha,ing_referencia,ing_importe,ci_codigo,cri_codigo,tienda,estacion,Consecutivo,referencia,importe  " &
                  "FROM compucaja.dbo.Ingresos I with (nolock) " &
                  "INNER join ConciliacionServicios AS pg with (nolock) ON CONVERT(INT, PG.TIENDA) = I.FolTda_Codigo AND pg.Referencia=i.Ing_Referencia AND pg.cri_codigo = i.CI_Codigo AND i.FolConsecutivo=pg.Consecutivo " &
                  "WHERE i.ing_fecha >= '20130801' AND CONVERT(VARCHAR(1),pg.conciliado)='0' " &
                  "AND Ing_Cancelado=0  "
            'AND i.Ing_Importe <> CONVERT(DECIMAL(10),pg.Importe) 
            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
            tdgnoconciliados.DataSource = dt
            cnx.Close()

            If mostrarMsj Then MsgBox("proceso terminado")
        End Try

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim total As String
        qry = ""
        TextBox3.Text = " "
        lop.Clear()
        qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " &
              "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " &
              "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) in (1,2,3,4,5)  "
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =2  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =3  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =4  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =5  "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        dtgtiendasf.DataSource = dt
        total = dtgtiendasf.RowCount
        TextBox3.Text = total
        dtgtiendasf.Splits(0).DisplayColumns(3).FetchStyle = True
        ''Sumar una Columna
        '   Dim generado As Integer

        '   Dim row As Integer
        '   Dim i As Integer
        '   row = 3
        '   For i = 0 To dtgtiendasf.Splits(0).DisplayColumns.Count - 1
        '       If total <> "0" Then
        '           generado = dtgtiendasf.Columns(i).CellText(row)
        '       End If
        '   Next
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim total As Integer
        qry = ""
        TextBox3.Text = " "
        lop.Clear()
        qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS GeneradoT,aplicadoencentral,FechaGeneracion,FechaAplicacion  " &
              "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " &
              "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =0  "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        dtgtiendasf.DataSource = dt
        total = dtgtiendasf.RowCount
        TextBox3.Text = total
        ' dtgtiendasf.Splits(0).DisplayColumns(2).FetchStyle = True
        'Sumar una Columna
        'Dim generado As Integer
        'Dim aplicado As Integer
        'Dim x As Integer
        'Dim i As Integer
        'x = 2 'hacia abajo
        'For i = 0 To dtgtiendasf.Splits(0).DisplayColumns.Count - 1
        '    If total <> "0" Then
        '        generado = dtgtiendasf.Columns(x).CellText(i)
        '        aplicado = dtgtiendasf.Columns(x + 1).CellText(i)
        '        If generado = aplicado Then
        '            Me.dtgtiendasf.Columns(x).CellValue(i).style.backcolor = System.Drawing.ColorTranslator.FromHtml("#FF000F")
        '        End If
        '    End If
        '    x = 2
        'Next

        'Color para el fondo de la celda 
        'Me.DataGridView1.Rows(0).Cells(0).Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF000F")

        'Color para el texto de la celda 
        'Me.DataGridView1.Rows(0).Cells(0).Style.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006")




    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        My.Computer.Clipboard.Clear()
        Dim strTemp As String
        Dim row As Integer
        Dim i As Integer
        strTemp = ""
        If dtgtiendasf.SelectedRows.Count > 0 Then
            For i = 0 To dtgtiendasf.Splits(0).DisplayColumns.Count - 1
                If dtgtiendasf.Splits(0).DisplayColumns(i).Visible = True Then
                    strTemp = strTemp & dtgtiendasf.Columns(i).Caption & vbTab
                End If
            Next i
            strTemp = strTemp & vbCrLf
            For Each row In dtgtiendasf.SelectedRows
                For i = 0 To dtgtiendasf.Splits(0).DisplayColumns.Count - 1
                    If dtgtiendasf.Splits(0).DisplayColumns(i).Visible = True Then
                        strTemp = strTemp & dtgtiendasf.Columns(i).CellText(row) & vbTab
                    End If
                Next
                strTemp = strTemp & vbCrLf
            Next
            System.Windows.Forms.Clipboard.SetDataObject(strTemp, True)
        Else
        End If
    End Sub

    Private Sub cbxfechapago_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub dtgtiendasf_FetchCellStyle(sender As Object, e As C1.Win.C1TrueDBGrid.FetchCellStyleEventArgs) Handles dtgtiendasf.FetchCellStyle
        If dtgtiendasf.Columns("Generado").CellText(e.Row) = dtgtiendasf.Columns("aplicado").CellText(e.Row) Then
            e.CellStyle.BackColor = Color.Red
        End If
        If dtgtiendasf.Columns("Generado").CellText(e.Row) > dtgtiendasf.Columns("Aplicado").CellText(e.Row) Then
            e.CellStyle.BackColor = Color.Green
        End If
        If dtgtiendasf.Columns("Generado").CellText(e.Row) < dtgtiendasf.Columns("Aplicado").CellText(e.Row) Then
            e.CellStyle.BackColor = Color.Red
        End If
    End Sub

    'Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    '    Dim total As String
    '    qry = ""
    '    TextBox3.Text = " "
    '    lop.Clear()
    '    qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '         "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '         "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =1  " & _
    '         "union" & _
    '   " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '         "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '         "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =2  " & _
    '         "union" & _
    '   " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '         "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '         "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =3  " & _
    '         "union" & _
    '   " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '         "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '         "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =4  " & _
    '         "union" & _
    '   " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '         "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '         "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =5  "
    '    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '    dtgtiendasf.DataSource = dt
    '    total = dtgtiendasf.RowCount
    '    TextBox3.Text = total
    '    dtgtiendasf.Splits(0).DisplayColumns(3).FetchStyle = True
    'End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        qry = ""
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@FechaIni", dtnoconciliados3.Value))
        lop.Add(sqls.CreadbParametros("@FechaFin", dtnoconciliados2.Value))
        qry = "SELECT convert(char(4),tienda)+'-'+convert(char(1),estacion)+'-'+ convert(char(10),consecutivo) AS FOLIO,FECHA,HORA,convert(char(4),CRI_Codigo)+'-'+sv_nombre as SERVICIO,ING_CANCELADO,REFERENCIA,IMPORTE,ARCHIVO FROM ConciliacionServicios AS ps  " &
        "INNER JOIN compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo " &
        "INNER JOIN compucaja.dbo.Ingresos AS i ON FolTda_Codigo=tienda AND FolEst_Codigo=estacion AND FolConsecutivo=consecutivo AND ci_codigo=CRI_Codigo " &
        "WHERE fecha between @fechaini AND @fechafin AND conciliado=0 "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        tdgnoconciliados.DataSource = dt
        MsgBox("proceso terminado")
    End Sub


    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged

        Select Case TabControl1.Controls.Item(TabControl1.SelectedIndex).Name
            Case "TabPage1"
                Label14.Visible = True
                TextBox2.Visible = True
                Button3.Visible = True
                GroupBox3.Visible = True
            Case "TabPage2"
                ' dtxfechaini.Value = DateAdd(DateInterval.Month, -1, Today)
                Label14.Visible = False
                TextBox2.Visible = False
                Button3.Visible = False
                GroupBox3.Visible = False
            Case "TabPage3"
                dtnoconciliados3.Value = DateAdd(DateInterval.Day, -3, Today)
                Label14.Visible = False
                TextBox2.Visible = False
                Button3.Visible = False
                GroupBox3.Visible = False
            Case "TabPage4"
                Label14.Visible = False
                TextBox2.Visible = False
                Button3.Visible = False
                GroupBox3.Visible = False
        End Select
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        qry = ""
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@FechaIni", dtnoconciliados3.Value))
        lop.Add(sqls.CreadbParametros("@FechaFin", dtnoconciliados2.Value))
        qry = "SELECT convert(char(4),tienda)+'-'+convert(char(1),estacion)+'-'+ convert(char(10),consecutivo) AS FOLIO,FECHA,HORA,convert(char(4),CRI_Codigo)+'-'+sv_nombre as SERVICIO,REFERENCIA,IMPORTE,ARCHIVO FROM ConciliacionServicios AS ps " &
             "INNER JOIN compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo AND SV_Codigo=7" &
             "WHERE fecha between @fechaini AND @fechafin AND conciliado=0 " &
             "union  " &
             "SELECT convert(char(4),tienda)+'-'+convert(char(1),estacion)+'-'+ convert(char(10),consecutivo) AS FOLIO,FECHA,HORA,convert(char(4),CRI_Codigo)+'-'+sv_nombre as SERVICIO,REFERENCIA,IMPORTE,ARCHIVO FROM ConciliacionServicios AS ps " &
             "INNER JOIN compucaja.dbo.Servicios AS s ON ps.CRI_Codigo=s.CRI_Codigo AND CRI_Codigo<>16" &
             "WHERE fecha between @fechaini AND @fechafin AND conciliado=0 "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        tdgnoconciliados.DataSource = dt
        MsgBox("proceso terminado")
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim dial As String
        dial = UCase(DateTime.Now.ToString("dddd"))
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@dias", "%" + dial + "%"))
        'qry = "SELECT descripcion,CRI_Codigo,SUM(Importe)AS monto FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago INNER JOIN COMPUCAJA.dbo.ConciliacionServicios AS ps ON CRI_Codigo=CodIngresoCC " & _
        '      "WHERE  td.diasdepago LIKE @dias  AND ps.fecha BETWEEN DATEDIFF(dd,5,GETDATE()) AND GETDATE() AND conciliado=0 and fechapago is null " & _
        '      " GROUP BY CRI_Codigo,Descripcion"

        '        qry = "SELECT descripcion,tc.CRI_Codigo,SUM(Importe)AS monto " & _
        '"FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " & _
        '"AS td ON tc.DiasDePago = td.IdDiasPago INNER JOIN dbo.ConciliacionServicios " & _
        '"AS ps ON ps.CRI_Codigo=tc.CRI_Codigo WHERE  td.diasdepago LIKE @dias  AND ps.fecha " & _
        ' "       BETWEEN DateDiff(dd, 5, GETDATE()) And GETDATE() And conciliado = 1 And fechapago Is null " & _
        '"GROUP BY tc.CRI_Codigo,Descripcion"
        qry = "SELECT descripcion,tc.CRI_Codigo,SUM(Importe)AS monto " &
"FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " &
"AS td ON tc.DiasDePago = td.IdDiasPago INNER JOIN dbo.ConciliacionServicios " &
"AS ps ON ps.CRI_Codigo=tc.CRI_Codigo WHERE  td.diasdepago LIKE @dias and tc.TienePeriodoSem=0 " &
 "AND ps.fecha BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' And '" &
 DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' " &
 "AND tc.TienePeriodoSem=0 " &
 "AND tc.TieneDiasCredito=0 " &
 "And conciliado = 1 And fechapago Is null And tc.EstatusServ=1" &
"GROUP BY tc.CRI_Codigo,Descripcion " &
"union " &
"SELECT descripcion,tc.CRI_Codigo,SUM(Importe)AS monto " &
"FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " &
"AS td ON tc.DiasDePago = td.IdDiasPago INNER JOIN dbo.ConciliacionServicios " &
"AS ps ON ps.CRI_Codigo=tc.CRI_Codigo WHERE  td.diasdepago Like @dias And tc.TienePeriodoSem=1 And ps.fecha " &
 "       BETWEEN '" & DtpFechaini.Value.ToString("yyyy-MM-dd") & " 00:00:00' And '" &
 DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59' And conciliado = 1 And fechapago Is null and tc.EstatusServ=1" &
 " AND dbo.fn_Obtener_Fecha_PorDia(ps.Fecha, tc.DiaFin, 0) <= dbo.fn_fecha_sinhora(GETDATE()) " &
 "AND tc.TienePeriodoSem=1 " &
 "AND tc.TieneDiasCredito=0 " &
"GROUP BY tc.CRI_Codigo,Descripcion " &
"union " &
"SELECT descripcion,tc.CRI_Codigo,SUM(Importe)AS monto " &
"FROM dbo.T_ConceptosDeServicios AS tc INNER JOIN dbo.T_DiasDePagoServicios " &
"AS td ON tc.DiasDePago = td.IdDiasPago INNER JOIN dbo.ConciliacionServicios " &
"AS ps ON ps.CRI_Codigo=tc.CRI_Codigo WHERE  td.diasdepago LIKE @dias " &
"and ps.fecha <= DateAdd(Day, -1 * tc.DiasCredito,'" &
                        DtpFechafin.Value.ToString("yyyy-MM-dd") & " 23:59:59') " &
 " And conciliado = 1 And fechapago Is null and tc.EstatusServ=1" &
 "AND tc.TienePeriodoSem=0 " &
 "AND tc.TieneDiasCredito=1 " &
"GROUP BY tc.CRI_Codigo,Descripcion "



        'SELECT *
        'FROM 
        '(SELECT descripcion,CRI_Codigo,SUM(Importe)AS monto 
        'FROM HistorialKiosko.dbo.T_ConceptosDeServicios AS tc 
        'INNER JOIN HistorialKiosko.dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago 
        'INNER JOIN COMPUCAJA.dbo.ConciliacionServicios AS ps ON CRI_Codigo=CodIngresoCC 
        'WHERE  td.diasdepago LIKE @dias  AND ps.fecha BETWEEN DATEDIFF(dd,5,GETDATE()) AND GETDATE() AND conciliado=0 and fechapago is null 
        'GROUP BY CRI_Codigo,Descripcion) AS SERVICIOSX 
        'INNER JOIN (SELECT MAX(fechapago) AS fechaultimopago,CRI_Codigo 
        'FROM dbo.ConciliacionServicios WHERE CRI_Codigo=5
        'GROUP BY CRI_Codigo) AS pagox ON SERVICIOSX.CRI_Codigo=pagox.CRI_Codigo






        dt = sqls.DevuelveDatos(cnx, qry, , , lop) '"set language english " &
        dgservicio.DataSource = dt
    End Sub


    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        '  Dim mylog As String
        Dim ds As New DataSet
        Dim archivos() As String
        Dim sLine As String = ""
        Dim da As New SqlCommand
        Dim linea() As String
        Dim primeral As String
        Dim id_serviciox As String
        Dim fs_infos() As FileInfo
        Dim nomarch As String
        Dim mmcomision As Decimal
        Dim servicio As String
        Dim referencia As String
        Dim tipopago As String
        Dim tienda As String
        Dim fecha1 As String
        Dim fpago As String
        tienda = " "
        qry = "Select * from T_RutasFTP where IdRuta=2"
        Dim RutasAutoG As DataTable = sqls.DevuelveDatos(cnx, qry, , , )
        'Dim dir_info As New DirectoryInfo("\\svrservices\DatosFTP\IUSA")
        'RutasAutoG.Rows(0)("RutaOrigen") = "\\192.168.100.167\c$\Users\rtorres\Google Drive\SERVICIOS KIOSKO\iusa"
        Dim dir_info As New DirectoryInfo(RutasAutoG.Rows(0)("RutaOrigen"))


        fs_infos = dir_info.GetFiles("*.txt")

        qry = ""
        lop.Clear()
        qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " &
              "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " &
              "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,'" &
              dtnoconciliados2.Value.ToString("yyyy-MM-dd") & "') in (1,2,3,4,5)  "
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =2  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =3  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =4  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =5  "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        If dt.Rows.Count > 20 Then  'numero de tiendas faltantes 
            MsgBox("faltan tiendas")
        Else
            If fs_infos.Count = 0 Then
                MsgBox("no hay archivos para procesar")
            Else
                Dim nombre As String
                nombre = " "
                cnx.Open()
                Try
                    For Each fs_info As FileInfo In fs_infos
                        nombre = fs_info.Name
                        nomarch = Mid(nombre, 1, 6)
                        If nomarch = "kiosko" Then
                            archivos = IO.Directory.GetDirectories(RutasAutoG.Rows(0)("RutaOrigen"))
                            Dim dir As String = RutasAutoG.Rows(0)("RutaOrigen") & "\" & fs_info.Name
                            Dim objReader As New StreamReader(dir.ToString)
                            Do
                                sLine = objReader.ReadLine()
                                id_serviciox = " "
                                If Not sLine Is Nothing Then
                                    primeral = Mid(sLine, 1, 1)
                                    If primeral = "f" Or primeral = "F" Or primeral = " " Then
                                    Else
                                        linea = sLine.Split(",")

                                        'If linea(5) = "M10092791" Then
                                        '    primeral = "jix"
                                        'End If

                                        fpago = Mid(linea(0), 1, 4)
                                        'referencia = Mid(linea(5), 2, 7)
                                        referencia = Mid(linea(5), 2, linea(5).Length - 1)
                                        tipopago = Mid(linea(0), 6, 5)

                                        If linea(4).Substring(0, 4) = "1969" Then
                                            Dim a As String = ""
                                            a = ""

                                        End If

                                        If fpago = "PAGO" Then

                                            id_serviciox = "50"
                                            lop.Clear()
                                            lop.Add(sqls.CreadbParametros("@referencia", "%" + referencia + "%"))
                                            qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Doc,FolEst_Codigo as estacion,FolConsecutivo as consecutivo," &
                                                "Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha,Ing_Contribucion " &
                                                  "FROM Compucaja.dbo.Ingresos AS i " &
                                                  " WHERE ing_trxid LIKE @referencia AND ci_codigo=50 AND ing_fecha BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE() "
                                            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                        ElseIf fpago = "CARG" Then
                                            'mmcomision = linea(6) - 2.54
                                            id_serviciox = "50"
                                            lop.Clear()
                                            lop.Add(sqls.CreadbParametros("@referencia", "%" + referencia + "%"))
                                            qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Doc,FolEst_Codigo as estacion,FolConsecutivo as consecutivo," &
                                                "Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha,Ing_Contribucion " &
                                                  "FROM Compucaja.dbo.Ingresos AS i " &
                                                  " WHERE ing_trxid LIKE @referencia AND ci_codigo=50 AND ing_fecha BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE() "
                                            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                        ElseIf fpago = "TARJ" Then
                                            'mmcomision = linea(6) - 5.8
                                            id_serviciox = "51"
                                            lop.Clear()
                                            lop.Add(sqls.CreadbParametros("@referencia", "%" + referencia + "%"))
                                            qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Doc,FolEst_Codigo as estacion,FolConsecutivo as consecutivo," &
                                                "Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha,Ing_Contribucion " &
                                                  "FROM Compucaja.dbo.Ingresos AS i " &
                                                  " WHERE ing_trxid LIKE @referencia AND ci_codigo=51 AND ing_fecha BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE()"
                                            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                        ElseIf fpago = "CXX" Then
                                            'mmcomision = linea(6) - 2.54
                                            id_serviciox = "50"
                                            lop.Clear()
                                            lop.Add(sqls.CreadbParametros("@referencia", "%" + referencia + "%"))
                                            qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Doc,FolEst_Codigo as estacion,FolConsecutivo as consecutivo," &
                                                "Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha,Ing_Contribucion " &
                                                  "FROM Compucaja.dbo.Ingresos AS i " &
                                                  " WHERE ing_trxid LIKE @referencia AND ci_codigo=50 AND ing_fecha BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE() "
                                            dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                        Else
                                            MsgBox("este pago no lo encuentro en compucaja: " & sLine)
                                            servicio = "autogestion"
                                            Log(sLine, servicio)
                                        End If

                                        If dt.Rows.Count >= 1 Then
                                            'mylog = mylog & linea(2).Trim & "|" & linea(4).Trim & "|" & linea(9).Trim & "|" & "|" & id_serviciox.Trim & vbCrLf
                                            Dim dt2 As New DataTable
                                            Dim qry2 As String = "Select isnull((Select RestaContr from T_ConceptosDeServicios where CRI_Codigo='" &
                                                id_serviciox & "'),0)"
                                            dt2 = sqls.DevuelveDatos(cnx, qry2, , , )

                                            mmcomision = linea(6) - IIf(dt2.Rows(0)(0), dt.Rows(0)("Ing_Contribucion"), 0)
                                            If linea(6) <= dt.Rows(0)("Importe") Then 'Si el importe del proveedor es igual al de compucaja conciliar

                                                Dim cmd As New SqlCommand("X_SP_IntegrarServiciosA", cnx)
                                                cmd.CommandType = CommandType.StoredProcedure
                                                cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
                                                cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = dt.Rows(0).Item("estacion")
                                                cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = dt.Rows(0).Item("consecutivo")
                                                cmd.Parameters.Add("@importe", SqlDbType.Money).Value = mmcomision ' dt.Rows(0).Item("importe")
                                                cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = dt.Rows(0).Item("referencia")
                                                cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows(0).Item("fecha"))
                                                cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(dt.Rows(0).Item("fecha"), "HH:mm:ss")
                                                cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
                                                cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
                                                cmd.Parameters.Add("@Documento", SqlDbType.VarChar, 10).Value = dt.Rows(0).Item("Doc")
                                                cmd.Parameters.Add("@msj", SqlDbType.BigInt)
                                                cmd.Parameters("@msj").Direction = ParameterDirection.Output
                                                cmd.ExecuteNonQuery()
                                                If cmd.Parameters("@msj").Value = 1 Then
                                                    'mylog = mylog & linea(2).Trim & "|" & linea(4).Trim & "|" & linea(9).Trim & "|" & "|" & nombre.Trim & vbCrLf
                                                End If
                                            Else
                                                Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
                                                cmd.CommandType = CommandType.StoredProcedure
                                                cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
                                                cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = dt.Rows(0).Item("estacion")
                                                cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = dt.Rows(0).Item("consecutivo")
                                                cmd.Parameters.Add("@importe", SqlDbType.Money).Value = dt.Rows(0).Item("importe")
                                                cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = dt.Rows(0).Item("referencia")
                                                cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows(0).Item("fecha"))
                                                cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(dt.Rows(0).Item("fecha"), "HH:mm:ss")
                                                cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
                                                cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
                                                cmd.Parameters.Add("@msj", SqlDbType.BigInt)
                                                cmd.Parameters("@msj").Direction = ParameterDirection.Output
                                                cmd.ExecuteNonQuery()
                                            End If
                                        Else
                                            'MsgBox("no encuento este registro" & sLine)
                                            servicio = "autogestion"
                                            'Log(sLine, servicio)
                                            Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
                                            cmd.CommandType = CommandType.StoredProcedure
                                            cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = linea(4).Substring(0, 4)
                                            cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = 0
                                            cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = 0
                                            cmd.Parameters.Add("@importe", SqlDbType.Money).Value = linea(6) ' dt.Rows(0).Item("importe")
                                            cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = referencia
                                            Dim fecha As DateTime = Convert.ToDateTime((Mid(linea(2), 1, 4) & "-" &
                                                                                        Mid(linea(2), 5, 2) & "-" &
                                                                                        Mid(linea(2), 7, 2)) & " " & linea(3))
                                            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = fecha
                                            cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(fecha, "HH:mm:ss")
                                            cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
                                            cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
                                            cmd.Parameters.Add("@msj", SqlDbType.BigInt)
                                            cmd.Parameters("@msj").Direction = ParameterDirection.Output
                                            cmd.ExecuteNonQuery()
                                        End If
                                    End If
                                End If
                            Loop Until sLine Is Nothing
                            objReader.Close()
                            Dim sArchivoOrigen As String = RutasAutoG.Rows(0)("RutaOrigen") & "\" & nombre.Trim
                            Dim sRutaDestino As String = RutasAutoG.Rows(0)("RutaDestino") & "\" & nombre.Trim
                            My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)

                        ElseIf nomarch = "boletoscine" Then
                            qry = "Select * from T_RutasFTP where IdRuta=1"
                            RutasAutoG = sqls.DevuelveDatos(cnx, qry, , , )

                            archivos = IO.Directory.GetDirectories(RutasAutoG.Rows(0)("RutaOrigen"))
                            Dim dir As String = RutasAutoG.Rows(0)("RutaOrigen") & "\" & fs_info.Name
                            Dim objReader As New StreamReader(dir.ToString)
                            Do
                                sLine = objReader.ReadLine()
                                id_serviciox = " "
                                If Not sLine Is Nothing Then
                                    primeral = Mid(sLine, 1, 1)
                                    If primeral = "f" Or primeral = "F" Or primeral = " " Then
                                    Else
                                        linea = sLine.Split(",")
                                        tienda = linea(1)
                                        referencia = linea(2)
                                        fecha1 = linea(0)
                                        lop.Clear()
                                        lop.Add(sqls.CreadbParametros("@referencia", referencia))
                                        lop.Add(sqls.CreadbParametros("@tienda", tienda))

                                        qry = "SELECT t.FolTda_Codigo as tienda,t.FolEst_Codigo as estacion,t.FolConsecutivo as consecutivo,T_ImporteTotal as importe,DT_TrxID AS referencia,t.t_Fecha as fecha " &
                                              "FROM Compucaja.dbo.Tickets AS t " &
                                              "INNER JOIN Compucaja.dbo.DetallesTicket AS dt ON t.FolTda_Codigo = dt.FolTda_Codigo AND t.FolEst_Codigo = dt.FolEst_Codigo AND t.FolDoc_Codigo = dt.FolDoc_Codigo AND t.FolConsecutivo = dt.FolConsecutivo " &
                                              "WHERE t.FolTda_Codigo=@tienda AND dt.DT_TrxID=@referencia"
                                        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
                                        id_serviciox = "241"
                                        If dt.Rows.Count = 1 Then
                                            'mylog = mylog & linea(2).Trim & "|" & linea(4).Trim & "|" & linea(9).Trim & "|" & "|" & id_serviciox.Trim & vbCrLf
                                            Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
                                            cmd.CommandType = CommandType.StoredProcedure
                                            cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
                                            cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = dt.Rows(0).Item("estacion")
                                            cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = dt.Rows(0).Item("consecutivo")
                                            cmd.Parameters.Add("@importe", SqlDbType.Money).Value = dt.Rows(0).Item("importe")
                                            cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = dt.Rows(0).Item("referencia")
                                            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows(0).Item("fecha"))
                                            cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(dt.Rows(0).Item("fecha"), "HH:mm:ss")
                                            cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
                                            cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
                                            cmd.Parameters.Add("@msj", SqlDbType.BigInt)
                                            cmd.Parameters("@msj").Direction = ParameterDirection.Output
                                            cmd.ExecuteNonQuery()
                                            If cmd.Parameters("@msj").Value = 1 Then
                                                'mylog = mylog & linea(2).Trim & "|" & linea(4).Trim & "|" & linea(9).Trim & "|" & "|" & nombre.Trim & vbCrLf
                                            End If
                                        Else
                                            MsgBox("este registro no lo encontre")
                                            Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
                                            cmd.CommandType = CommandType.StoredProcedure
                                            cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = tienda 'dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
                                            cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = 7
                                            cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = 11
                                            cmd.Parameters.Add("@importe", SqlDbType.Money).Value = 14
                                            cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = referencia 'dt.Rows(0).Item("referencia")
                                            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(fecha1)
                                            cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(fecha1, "HH:mm:ss")
                                            cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
                                            cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
                                            cmd.Parameters.Add("@msj", SqlDbType.BigInt)
                                            cmd.Parameters("@msj").Direction = ParameterDirection.Output
                                            cmd.ExecuteNonQuery()
                                            If cmd.Parameters("@msj").Value = 1 Then

                                            End If
                                        End If
                                    End If
                                End If
                            Loop Until sLine Is Nothing
                            objReader.Close()
                            Dim sArchivoOrigen As String = RutasAutoG.Rows(0)("RutaOrigen") & "\" & nombre.Trim
                            Dim sRutaDestino As String = RutasAutoG.Rows(0)("RutaDestino") & "\" & nombre.Trim
                            My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)
                        End If
                    Next
                Catch ex As Exception
                    ' sqls.GrabaTextoEnArchivo("Error al conciliar Servicios : " + ex.Message + " - Arrchivo " + nombre.Trim + " Detalle ultima linea -  Tienda : " + linea(2).Trim & " - Estacion: " & linea(4).Trim & " - Referencia: " & linea(9).Trim & " - Servicio:" & id_serviciox.Trim & "|")
                    ' MsgBox.show(ex)
                Finally
                    cnx.Close()
                    If mostrarMsj Then MessageBox.Show("Proceso Terminado")
                End Try
                'Process.Start("notepad.exe", "c:\milogconciliacion.log")       'abrir archivo despues del proceso
            End If
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet
        Dim bandera As Boolean
        Dim x As Integer
        Dim y As Integer
        Dim mensaje As String = "hacer Archivo Concentrado"
        Dim Caption As String = "CONFIRMACION DE PAGO"
        Dim boton As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim resp As DialogResult
        Dim mes As String
        Dim dia As String
        Dim rutarchivo As String = String.Empty
        mes = DateTime.Now.ToString("MMMM")
        'dial = DateTime.Now.ToString("dddd")
        dia = Now.Day
        dia = dia - 1
        Dim dial As String
        dial = UCase(DateTime.Now.ToString("dddd"))
        lop.Clear()
        lop.Add(sqls.CreadbParametros("@dias", "%" + dial + "%"))
        qry = "SELECT Descripcion,SUM(Monto),Fecha FROM   " &
            "(SELECT descripcion,tc.CRI_Codigo,SUM(Importe)AS monto   " &
             " FROM dbo.T_ConceptosDeServicios AS tc    " &
         "INNER JOIN dbo.T_DiasDePagoServicios AS td ON tc.DiasDePago = td.IdDiasPago   " &
         "INNER JOIN dbo.conciliacionservicios AS ps ON cast(tc.CRI_Codigo as bigint)=ps.cri_codigo " &
         "WHERE  td.diasdepago LIKE @dias   " &
         "AND ps.fecha BETWEEN DATEDIFF(dd,30,GETDATE()) AND GETDATE()  " &
         "AND conciliado IN (1)  " &
        "and fechapago is null    " &
         "GROUP BY tc.CRI_Codigo,Descripcion) AS SERVICIOSX   " &
         " INNER JOIN (SELECT CONVERT(NVARCHAR(64), ISNULL(MAX(fechapago),0))+' AL '+ CONVERT(NVARCHAR(64),  " &
         " CONVERT(DATETIME,DATEDIFF(dd,1,GETDATE()))) AS fecha,cri_codigo    " &
       " FROM dbo.conciliacionservicios " &
        "     GROUP BY cri_codigo) AS pagox ON SERVICIOSX.cri_codigo=pagox.cri_codigo " &
         "    GROUP BY descripcion,fecha "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
        dgservicio.DataSource = dt
        If dt.Rows.Count = 0 Then
            MsgBox("faltan datos")
        Else
            resp = MessageBox.Show(mensaje, Caption, boton)
            If resp = System.Windows.Forms.DialogResult.Yes Then
                'MessageBox.Show("voy hacer pago concentrado")
                Try
                    'Añadimos el Libro al programa, y la hoja al libro
                    exLibro = exApp.Workbooks.Add
                    exHoja = exLibro.Worksheets.Add()
                    exHoja.Columns(1).NumberFormat = "@"
                    exHoja.Columns(4).NumberFormat = "@"
                    exHoja.Columns(3).NumberFormat = "#,##0.00"
                    ' ¿Cuantas columnas y cuantas filas?
                    Dim NCol As Integer = dgservicio.ColumnCount()
                    Dim NRow As Integer = dgservicio.RowCount
                    x = NCol
                    y = NRow
                    ' NCol = NCol - 2
                    'Aqui recorremos todas las filas, y por cada fila todas las columnas y vamos escribiendo.
                    For i As Integer = 1 To NCol
                        exHoja.Cells.Item(3, i) = dgservicio.Columns(i - 1).Name.ToString
                    Next
                    For Fila As Integer = 0 To NRow - 1
                        For Col As Integer = 0 To NCol - 1
                            exHoja.Cells.Item(Fila + 4, Col + 1) = Trim(dgservicio.Rows(Fila).Cells(Col).Value)
                        Next
                    Next
                    'exHoja.Cells.Item(y + 4, x - 3) = "TOTAL"
                    'exHoja.Cells.Item(y + 4, x - 2) = Me.sumaimporte.Text
                    'Titulo en negrita, Alineado al centro y que el tamaño de la columna se ajuste al texto
                    exHoja.Rows.Item(3).Font.Bold = 1
                    exHoja.Rows.Item(3).HorizontalAlignment = 3
                    exHoja.Columns.AutoFit()
                    'Aplicación visible
                    exApp.Application.Visible = True
                    exLibro.SaveAs("c:\pagosjuntos\" + "pago servicios concentrado" + " al " + " " & dia & " " & mes, , "", "", False, False)
                    rutarchivo = exLibro.FullName
                    exApp.Quit()
                    ReleaseCom(exApp)
                    exHoja = Nothing
                    exLibro = Nothing
                    exApp = Nothing
                    bandera = True
                Catch ex As Exception
                    MessageBox.Show(" ERROR : " & ex.Message & " --UtilForm.ExportarGridADocumentoExcel", "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    bandera = False
                End Try
            Else
                CbxServicio.Focus()
            End If
        End If
    End Sub


    Private Sub tdgnoconciliados_FetchCellStyle(sender As Object, e As C1.Win.C1TrueDBGrid.FetchCellStyleEventArgs) Handles tdgnoconciliados.FetchCellStyle
        If tdgnoconciliados.Columns("ing_importe").CellText(e.Row) = tdgnoconciliados.Columns("importe").CellText(e.Row) Then
            e.CellStyle.BackColor = Color.Green
        End If
        If tdgnoconciliados.Columns("ing_importe").CellText(e.Row) <> tdgnoconciliados.Columns("importe").CellText(e.Row) Then
            e.CellStyle.BackColor = Color.Red
        End If
        'If dtgtiendasf.Columns("Generado").CellText(e.Row) < dtgtiendasf.Columns("Aplicado").CellText(e.Row) Then
        '    e.CellStyle.BackColor = Color.Red
        'End If
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs)
        Dim ruta As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\192.168.100.31\_out")
        Dim ruta1 As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\192.168.100.31\pdf")
        'buscarfactura(ruta)
        'buscarfactura(ruta1)
        MsgBox("ya termine de copiar las facturas")
    End Sub
    'Public Sub buscarfactura(ByVal ruta As String)
    '    Try
    '        If Directory.Exists(ruta) Then
    '            Dim directorio As New DirectoryInfo(ruta)
    '            Dim directorios() As DirectoryInfo = directorio.GetDirectories
    '            Dim archivos() As FileInfo
    '            Dim linea() As String
    '            Dim sLine As String = ""
    '            Dim nombredir As String
    '            archivos = directorio.GetFiles
    '            For Each f As DirectoryInfo In directorios
    '                nombredir = Mid(f.Name, 5, 2)
    '                If nombredir = TextBox4.Text.Trim Then
    '                    buscarfactura(f.FullName)
    '                End If
    '            Next
    '            For Each d As FileInfo In archivos
    '                sLine = d.Name
    '                If Len(sLine) >= 31 Then
    '                    linea = sLine.Split("_")
    '                    If linea(2) = "DI" Then
    '                        Dim sArchivoOrigen As String = d.FullName
    '                        Dim sRutaDestino As String = "C:\facturasherenita\" & d.Name.Trim
    '                        My.Computer.FileSystem.CopyFile(sArchivoOrigen, sRutaDestino, True)
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Catch excpt As System.Exception
    '        Debug.WriteLine(excpt.Message)
    '    End Try
    'End Sub

    'Private Sub Button18_Click(sender As Object, e As EventArgs)
    '    Dim oExcel As New OpenFileDialog
    '    oExcel.Filter = "Excel 2007 Files|*.xlsx|Excel 97-2003 Files|*.xls|Excel xlsb|*.xlsb|Excel xlsm|*.xlsm|All Files|*.*"
    '    If oExcel.ShowDialog = DialogResult.OK Then
    '        TextBox5.Text = oExcel.FileName
    '    End If
    '    DataGridView1.DataSource = MostrarDatExcel(TextBox5.Text, "Hoja1", "")

    '    'Función para mostrar el excel •‘SELECT * FROM [Hoja1$]‘)
    'End Sub
    'Private Function MostrarDatExcel(ByVal RUTA As String, ByVal HOJA As String, ByVal WHERE As String) As DataTable

    '    Dim Dst As New DataSet
    '    Dim Coneccion As String = String.Empty
    '    Dim TABLE As DataTable
    '    Dim ru As New FileInfo(RUTA)

    '    Select Case ru.Extension
    '        Case ".xls"
    '            Coneccion = "Provider=Microsoft.Jet.Oledb.4.0; data source= " & RUTA & ";Extended properties=""Excel 8.0;hdr=yes;imex=1"""
    '        Case ".xlsx"
    '            Coneccion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & RUTA & ";Extended Properties=""Excel 12.0 Xml;HDR=YES"""
    '        Case ".xlsb"
    '            Coneccion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & RUTA & ";Extended Properties=""Excel 12.0;HDR=YES"""
    '        Case ".xlsm"
    '            Coneccion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & RUTA & ";Extended Properties=""Excel 12.0 Macro;HDR=YES"""
    '    End Select

    '    Dim Cn As New OleDbConnection(Coneccion)
    '    Try
    '        Dst = New DataSet
    '        Dim Dap As New OleDbDataAdapter("Select * From [" & HOJA & "$]" & IIf(WHERE = String.Empty, "", WHERE), Cn)
    '        Cn.Open()                         '("Select * From [" & HOJA & "$]" & IIf(WHERE = String.Empty, "", WHERE), Cn) 
    '        Dap.Fill(Dst)
    '        Cn.Close()
    '        TABLE = Dst.Tables(0)
    '        TextBox6.Text = TABLE.Rows.Count
    '    Catch ex As Exception

    '        Cn.Close()
    '        MessageBox.Show(ex.Message, "Informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        TABLE = Nothing
    '    End Try

    '    Return TABLE

    'End Function

    'Private Sub Button19_Click(sender As Object, e As EventArgs)
    '    Dim codigox As String
    '    Dim minimovisual As String
    '    Dim totalreg As Integer
    '    If TextBox5.Text <> "" Then
    '        For Each row As DataGridViewRow In Me.DataGridView1.Rows()
    '            codigox = Val(row.Cells("codigo").Value)
    '            minimovisual = Val(row.Cells("MV").Value)
    '            If codigox = "0" And minimovisual = "0" Then
    '                ' MsgBox("enblanco")
    '            Else
    '                totalreg = totalreg + 1
    '                qry = ""
    '                lop.Clear()
    '                lop.Add(sqls.CreadbParametros("@codigo", codigox))
    '                lop.Add(sqls.CreadbParametros("@minimo", minimovisual))
    '                qry = "update SuperkioscoApp.dbo.xInventoryAdic  set user9=@minimo  WHERE InvtID= @codigo"
    '                sqls.ComandoSQL(cnx, qry, lop, , , , )
    '            End If
    '        Next
    '        MsgBox("proceso terminado se actualizaron  " & totalreg & " registros ")
    '    Else
    '        MsgBox("falta cargar archivo")
    '    End If
    'End Sub

    'Private Sub Button20_Click(sender As Object, e As EventArgs)
    '    Dim tienda As String
    '    Dim periodo As String
    '    Dim totalreg As Integer
    '    If TextBox5.Text <> "" Or TextBox6.Text <> "" Then
    '        For Each row As DataGridViewRow In Me.DataGridView1.Rows()
    '            tienda = Val(row.Cells("tienda").Value)
    '            periodo = Val(row.Cells("periodo").Value)
    '            If tienda = "0" And periodo = "0" Then
    '                ' MsgBox("enblanco")
    '            Else
    '                totalreg = totalreg + 1
    '                qry = ""
    '                lop.Clear()
    '                lop.Add(sqls.CreadbParametros("@tienda", tienda))
    '                lop.Add(sqls.CreadbParametros("@periodo", periodo))
    '                lop.Add(sqls.CreadbParametros("@tmaterial", TextBox7.Text))
    '                qry = "UPDATE SuperKioscoApp.dbo.ItemSite SET ReordInterval= @periodo WHERE SiteID= @tienda AND InvtID IN(SELECT InvtID FROM SuperkioscoApp.dbo.Inventory AS I WHERE MaterialType in (@tmaterial))"
    '                sqls.ComandoSQL(cnx, qry, lop, , , , )
    '            End If
    '        Next
    '    Else
    '        MsgBox("falta tipo material")
    '    End If
    '    MsgBox("proceso terminado se actualizaron  " & totalreg & " registros ")
    '    Dim sArchivoOrigen As String = TextBox5.Text

    '    Dim sRutaDestino As String = "C:\acomprascambios\cambiados\"
    '    My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)


    'End Sub

    'Private Sub Button21_Click(sender As Object, e As EventArgs)

    '    ' Create two identical or different temporary folders 
    '    ' on a local drive and add files to them.
    '    ' Then set these file paths accordingly.
    '    Dim pathA As String = "C:\directorio1"
    '    Dim pathB As String = "C:\directorio2"

    '    ' Take a snapshot of the file system. 
    '    Dim dir1 As New System.IO.DirectoryInfo(pathA)
    '    Dim dir2 As New System.IO.DirectoryInfo(pathB)

    '    Dim list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
    '    Dim list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories)

    '    ' Create the FileCompare object we'll use in each query
    '    Dim myFileCompare As New FileCompare

    '    ' This query determines whether the two folders contain
    '    ' identical file lists, based on the custom file comparer
    '    ' that is defined in the FileCompare class.
    '    ' The query executes immediately because it returns a bool.
    '    Dim areIdentical As Boolean = list1.SequenceEqual(list2, myFileCompare)
    '    If areIdentical = True Then
    '        ' MsgBox("las dos carpetas traen lo mismo")
    '        TextBox8.Text = TextBox8.Text & "las dos carpetas traen lo mismo" & vbCrLf & vbCrLf
    '    Else
    '        ' MsgBox("las dos carpetas no son iguales")
    '        TextBox8.Text = TextBox8.Text & "las dos carpetas no son iguales" & vbCrLf & vbCrLf
    '    End If

    '    ' Find common files in both folders. It produces a sequence and doesn't execute
    '    ' until the foreach statement.
    '    Dim queryCommonFiles = list1.Intersect(list2, myFileCompare)
    '    If queryCommonFiles.Count() > 0 Then
    '        'MsgBox("los siguientes archivos estan en las dos carpetas:")
    '        TextBox8.Text = TextBox8.Text & "los siguientes archivos estan en las dos carpetas:" & vbCrLf & vbCrLf
    '        For Each fi As System.IO.FileInfo In queryCommonFiles
    '            'MsgBox(fi.FullName)
    '            TextBox8.Text = TextBox8.Text & fi.Name & vbCrLf
    '        Next
    '        TextBox8.Text = TextBox8.Text & vbCrLf & vbCrLf
    '    Else
    '        'MsgBox("no hay archivos comunes en las dos carpetas")
    '        TextBox8.Text = TextBox8.Text & "no hay archivos comunes en las dos carpetas" & vbCrLf
    '    End If
    '    ' Find the set difference between the two folders.
    '    ' For this example we only check one way.
    '    Dim queryDirAOnly = list1.Except(list2, myFileCompare)
    '    'MsgBox("archivos que estan en dirA pero no en dirB:")
    '    TextBox8.Text = TextBox8.Text & "archivos que estan en dirA pero no en dirB:" & vbCrLf & vbCrLf
    '    For Each fi As System.IO.FileInfo In queryDirAOnly
    '        'Console.WriteLine(fi.FullName)
    '        TextBox8.Text = TextBox8.Text & fi.Name & vbCrLf
    '    Next
    '    TextBox8.Text = TextBox8.Text & vbCrLf & vbCrLf
    '    ' Keep the console window open in debug mode
    '    'MsgBox("Press any key to exit.")
    '    'Console.ReadKey()
    'End Sub
    ' This implementation defines a very simple comparison
    ' between two FileInfo objects. It only compares the name
    ' of the files being compared and their length in bytes.
    Public Class FileCompare
        Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo)
        Public Function Equals1(ByVal x As System.IO.FileInfo, ByVal y As System.IO.FileInfo) _
            As Boolean Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).Equals
            If (x.Name = y.Name) And (x.Length = y.Length) Then
                Return True
            Else
                Return False
            End If
        End Function

        ' Return a hash that reflects the comparison criteria. According to the 
        ' rules for IEqualityComparer(Of T), if Equals is true, then the hash codes must
        ' also be equal. Because equality as defined here is a simple value equality, not
        ' reference identity, it is possible that two or more objects will produce the same
        ' hash code.
        Public Function GetHashCode1(ByVal fi As System.IO.FileInfo) _
            As Integer Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).GetHashCode
            Dim s As String = fi.Name & fi.Length
            Return s.GetHashCode()
        End Function
    End Class

    'Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
    '    Dim ds As New DataSet
    '    Dim archivos() As String
    '    Dim sLine As String = ""
    '    Dim da As New SqlCommand
    '    Dim linea() As String
    '    Dim primeral As String
    '    Dim id_serviciox As String
    '    Dim fs_infos() As FileInfo
    '    Dim nomarch As String
    '    ' Dim mmcomision As Decimal
    '    Dim mylog As String
    '    Dim referencia As String
    '    Dim servicio As String
    '    Dim tienda As String
    '    '  Dim fecha1 As String
    '    tienda = " "
    '    mylog = ""
    '    qry = "Select * from T_RutasFTP where IdRuta=3"
    '    Dim RutasTransCol As DataTable = sqls.DevuelveDatos(cnx, qry, , , )
    '    Dim dir_info As New DirectoryInfo(RutasTransCol.Rows(0)("RutaOrigen"))
    '    fs_infos = dir_info.GetFiles("*.txt")
    '    qry = ""
    '    lop.Clear()
    '    qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '          "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '          "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =1  " & _
    '          "union" & _
    '    " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '          "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '          "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =2  " & _
    '          "union" & _
    '    " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '          "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '          "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =3  " & _
    '          "union" & _
    '    " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '          "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '          "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =4  " & _
    '          "union" & _
    '    " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
    '          "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
    '          "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =5  "
    '    dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '    If dt.Rows.Count > 5 Then
    '        MsgBox("faltan tiendas")
    '    Else

    '        If fs_infos.Count = 0 Then
    '            MsgBox("no hay archivos para procesar")
    '        Else
    '            Dim nombre As String
    '            nombre = " "
    '            cnx.Open()
    '            Try
    '                Dim i As Integer = 0
    '                For Each fs_info As FileInfo In fs_infos
    '                    nombre = fs_info.Name
    '                    nomarch = Mid(nombre, 1, 5)
    '                    If nomarch = "trans" Then
    '                        archivos = IO.Directory.GetDirectories(RutasTransCol.Rows(0)("RutaOrigen"))
    '                        Dim dir As String = RutasTransCol.Rows(0)("RutaOrigen") & "\" & fs_info.Name
    '                        Dim objReader As New StreamReader(dir.ToString)
    '                        Do
    '                            Try
    '                                sLine = objReader.ReadLine()
    '                                id_serviciox = " "
    '                                If Not sLine Is Nothing Then
    '                                    primeral = Mid(sLine, 1, 1)
    '                                    If primeral = "f" Or primeral = "F" Or primeral = " " Then
    '                                    Else
    '                                        linea = sLine.Split("|")
    '                                        referencia = linea(0) + "_" + linea(5)
    '                                        tienda = linea(2)
    '                                        id_serviciox = "59"
    '                                        lop.Clear()
    '                                        lop.Add(sqls.CreadbParametros("@referencia", "%" + referencia + "%"))
    '                                        lop.Add(sqls.CreadbParametros("@tienda", tienda))
    '                                        'qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Documento,FolEst_Codigo as estacion,FolConsecutivo as consecutivo,Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha " & _
    '                                        '      "FROM Compucaja.dbo.Ingresos AS i " & _
    '                                        '      " WHERE ing_referencia LIKE @referencia AND ci_codigo=59 AND ing_fecha BETWEEN DATEDIFF(dd,120,GETDATE()) AND GETDATE() and foltda_codigo=@tienda "

    '                                        qry = "SELECT FolTda_Codigo as tienda,FolDoc_Codigo as Documento,FolEst_Codigo as estacion,FolConsecutivo as consecutivo," & _
    '                                            "Ing_Importe as importe,Ing_Referencia as referencia,Ing_Fecha as fecha," & _
    '                                            "(case when isnull(Con.RestaContr,0)=1 then (ing_importe-ing_contribucion) " & _
    '                                            "else (ing_importe) end) as ImporteReal " & _
    '                                              "FROM Compucaja.dbo.Ingresos AS i WITH (NOLOCK) left join " & _
    '                                              "PagoDeServicios.dbo.T_ConceptosDeServicios as Con WITH (NOLOCK) " & _
    '                                              "on i.ci_codigo=con.cri_codigo " & _
    '                                              " WHERE ing_referencia LIKE @referencia AND ci_codigo=59 AND ing_fecha BETWEEN DATEDIFF(dd,120,GETDATE()) AND GETDATE() and foltda_codigo=@tienda "
    '                                        dt = sqls.DevuelveDatos(cnx, qry, , , lop)
    '                                        If dt.Rows.Count = 1 Then
    '                                            'qry = "Select isnull(RestaContr,0) as RestaContr from " & _
    '                                            '    "T_ConceptosDeServicios where CRI_Codigo='" & _
    '                                            '    id_serviciox & "'"
    '                                            'Dim dt2 As DataTable = sqls.DevuelveDatos(cnx, qry, , )
    '                                            'If dt2.Rows(0)("RestaContr") Then
    '                                            '    qry = "SELECT (ing_importe-ing_contribucion) " & _
    '                                            '      "FROM Compucaja.dbo.Ingresos AS i where " & _
    '                                            '      " Ing_TrxId='" & linea(5) & "' and CI_codigo=59"
    '                                            'Else
    '                                            '    qry = "SELECT ing_importe " & _
    '                                            '      "FROM Compucaja.dbo.Ingresos AS i where " & _
    '                                            '      " Ing_TrxId='" & linea(5) & "' and CI_codigo=59"
    '                                            'End If
    '                                            'dt2 = sqls.DevuelveDatos(cnx, qry, , )
    '                                            'validar que el importe del proveedor, sea igual al de compucaja.
    '                                            'éste importe ya viene restado de la contribucion en el txt
    '                                            If linea(11) <= dt.Rows(0)("ImporteReal") Then
    '                                                Dim cmd As New SqlCommand("X_SP_IntegrarServiciosA", cnx)
    '                                                cmd.CommandType = CommandType.StoredProcedure
    '                                                cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
    '                                                cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = dt.Rows(0).Item("estacion")
    '                                                cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = dt.Rows(0).Item("consecutivo")
    '                                                cmd.Parameters.Add("@importe", SqlDbType.Money).Value = linea(11) ' dt.Rows(0).Item("importe")
    '                                                cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = dt.Rows(0).Item("referencia")
    '                                                cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows(0).Item("fecha"))
    '                                                cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(dt.Rows(0).Item("fecha"), "HH:mm:ss")
    '                                                cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
    '                                                cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
    '                                                cmd.Parameters.Add("@Documento", SqlDbType.VarChar).Value = dt.Rows(0).Item("Documento")
    '                                                cmd.Parameters.Add("@msj", SqlDbType.BigInt)
    '                                                cmd.Parameters("@msj").Direction = ParameterDirection.Output
    '                                                cmd.ExecuteNonQuery()
    '                                                If cmd.Parameters("@msj").Value = 1 Then
    '                                                    '  mylog = mylog & linea(2).Trim & "|" & linea(4).Trim & "|" & linea(9).Trim & "|" & "|" & nombre.Trim & vbCrLf
    '                                                End If
    '                                            Else
    '                                                Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
    '                                                cmd.CommandType = CommandType.StoredProcedure
    '                                                cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = dt.Rows(0).Item("tienda") 'dt.Rows(x).Item("ip")) 
    '                                                cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = dt.Rows(0).Item("estacion")
    '                                                cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = dt.Rows(0).Item("consecutivo")
    '                                                cmd.Parameters.Add("@importe", SqlDbType.Money).Value = linea(11) ' dt.Rows(0).Item("importe")
    '                                                cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = dt.Rows(0).Item("referencia")
    '                                                cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows(0).Item("fecha"))
    '                                                cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Format(dt.Rows(0).Item("fecha"), "HH:mm:ss")
    '                                                cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
    '                                                cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
    '                                                cmd.Parameters.Add("@msj", SqlDbType.BigInt)
    '                                                cmd.Parameters("@msj").Direction = ParameterDirection.Output
    '                                                cmd.ExecuteNonQuery()
    '                                            End If

    '                                        Else
    '                                            'MsgBox("no encuento este registro" & sLine)
    '                                            servicio = "transporte"
    '                                            'Log(sLine, servicio) 'Escribe en log de errores y no inserta en la base de datos
    '                                            Dim cmd As New SqlCommand("X_SP_IntegrarServicios", cnx)
    '                                            cmd.CommandType = CommandType.StoredProcedure
    '                                            cmd.Parameters.Add("@tienda", SqlDbType.BigInt).Value = linea(2)
    '                                            cmd.Parameters.Add("@estacion", SqlDbType.BigInt).Value = 0 'linea(4)
    '                                            cmd.Parameters.Add("@consecutivo", SqlDbType.BigInt).Value = 0
    '                                            cmd.Parameters.Add("@importe", SqlDbType.Money).Value = linea(11) ' dt.Rows(0).Item("importe")
    '                                            cmd.Parameters.Add("@referencia", SqlDbType.NChar, 256).Value = referencia
    '                                            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = linea(0)
    '                                            cmd.Parameters.Add("@Hora", SqlDbType.NChar, 10).Value = Mid(linea(0), 11, 8)
    '                                            cmd.Parameters.Add("@Archivo", SqlDbType.NChar, 70).Value = fs_info.Name
    '                                            cmd.Parameters.Add("@Id_servicio", SqlDbType.BigInt).Value = id_serviciox.Trim
    '                                            cmd.Parameters.Add("@msj", SqlDbType.BigInt)
    '                                            cmd.Parameters("@msj").Direction = ParameterDirection.Output
    '                                            cmd.ExecuteNonQuery()
    '                                        End If
    '                                    End If
    '                                End If

    '                            Catch ex As Exception
    '                                '  sqls.GrabaTextoEnArchivo("Error al conciliar Servicios : " + ex.Message + " - Arrchivo " + nombre.Trim + " Detalle ultima linea -  Tienda : " + " - Estacion: " & " - Referencia: " & " - Servicio:" & "|")
    '                                MsgBox(ex.Message)
    '                            End Try
    '                            i = i + 1
    '                            Me.Text = "Cargando registro: " & i '& " de " & fs_infos.Length
    '                            My.Application.DoEvents()
    '                        Loop Until sLine Is Nothing
    '                        objReader.Close()
    '                        objReader.Close()
    '                        Dim sArchivoOrigen As String = RutasTransCol.Rows(0)("RutaOrigen") & "\" & nombre.Trim
    '                        Dim sRutaDestino As String = RutasTransCol.Rows(0)("RutaDestino") & "\" & nombre.Trim
    '                        My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)

    '                    End If

    '                Next
    '            Catch ex As Exception
    '                'sqls.GrabaTextoEnArchivo("Error al conciliar Servicios : " + ex.Message + " - Arrchivo " + nombre.Trim + " Detalle ultimtnmam linea -  Tienda : " + linea(2).Trim & " - Estacion: " & linea(4).Trim & " - Referencia: " & linea(9).Trim & " - Servicio:" & id_serviciox.Trim & "|")
    '                MsgBox(ex)
    '            Finally
    '                cnx.Close()
    '                If mostrarMsj Then MessageBox.Show("Proceso Terminado")
    '            End Try
    '            'Process.Start("notepad.exe", "c:\milogconciliacion.log")       'abrir archivo despues del proceso
    '        End If
    '    End If
    '    Me.Text = "Version 1.1"
    'End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        Dim ds As New DataSet
        Dim sLine As String = ""
        Dim da As New SqlCommand
        Dim fs_infos() As FileInfo
        Dim nomarch As String
        ' Dim mmcomision As Decimal
        Dim mylog As String
        Dim tienda As String
        '  Dim fecha1 As String
        tienda = " "
        mylog = ""
        qry = "Select * from T_RutasFTP where IdRuta=3"
        Dim RutasTransCol As DataTable = sqls.DevuelveDatos(cnx, qry, , , )
        Dim dir_info As New DirectoryInfo(RutasTransCol.Rows(0)("RutaOrigen"))
        fs_infos = dir_info.GetFiles("*.txt")
        qry = ""
        lop.Clear()
        qry = " SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " &
              "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " &
              "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) in (1,2,3,4,5) "
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =2  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =3  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =4  " & _
        '      "union" & _
        '" SELECT  Tda_Codigo,Tda_Nombre AS Nombre,GeneradoEnTienda AS Generado,aplicadoencentral as Aplicado,FechaGeneracion,FechaAplicacion  " & _
        '      "FROM compucaja.dbo.XV_ActualizacionDisCon AS xadc  " & _
        '      "WHERE AplicadoEnCentral IS NOT NULL  AND Tda_Codigo NOT IN (1400,1201,3140) AND DATEDIFF(dd,fechaaplicacion,GETDATE()) =5  "
        dt = sqls.DevuelveDatos(cnx, qry, , , lop)



        If dt.Rows.Count > 5 Then
            MsgBox("faltan tiendas")
        Else

            If fs_infos.Count = 0 Then
                MsgBox("no hay archivos para procesar")
            Else

                Dim nombre As String
                nombre = " "
                cnx.Open()

                Dim ccomando As New SqlCommand
                ccomando.Connection = cnx
                ccomando.CommandText = "delete from T_Archivo"
                ccomando.ExecuteNonQuery()
                Try
                    Dim i As Integer = 0
                    For Each fs_info As FileInfo In fs_infos
                        nombre = fs_info.Name
                        nomarch = Mid(nombre, 1, 5)
                        If nomarch = "trans" Then
                            ccomando = New SqlCommand
                            ccomando.Connection = cnx
                            ccomando.CommandText = "exec SP_Bulk_TransCol '" & RutasTransCol.Rows(0)("RutaOrigen") & "\" & fs_info.Name & "'"
                            ccomando.ExecuteNonQuery()

                            qry = "INSERT INTO ConciliacionServicios ([CRI_Codigo]" &
                                   ",[Estacion]" &
                                   ",[Tienda]" &
                                   ",[Consecutivo]" &
                                   ",[Fecha]" &
                                   ",[Hora]" &
                                   ",[Referencia]" &
                                   ",[Importe]" &
                                   ",[Archivo]" &
                                   ",[Conciliado]" &
                                   ",[Folio]) " &
                                   "select 59," &
                                   "isnull(i.FolEst_Codigo,0)," &
                                    "isnull(i.FolTda_Codigo,ar.tienda)," &
                                    "isnull(i.folconsecutivo,0)," &
                                    "isnull(i.Ing_Fecha,ar.fecha)," &
                                    "(case when i.Ing_Fecha is null then SUBSTRING(ar.fecha,11,10) else convert(nvarchar(MAX), i.ing_fecha, 24) end)," &
                                    "ar.referencia," &
                                    "ar.cantidad," &
                                    "'" & fs_info.Name & "'," &
                                    "case when i.FolConsecutivo IS null then 0 " &
                                    "Else " &
                                     "   (case when ar.cantidad= " &
                                      "      (case when isnull(Con.RestaContr,0)=1 then " &
                                       "     (i.ing_importe-i.ing_contribucion) else (i.ing_importe) end) " &
                                        "then 1 else 0 end)" &
                                    "end," &
                                    "(case when i.FolConsecutivo IS null then NULL else " &
                                    "(CAST(i.foltda_codigo as varchar)+'_'+CAST(i.FolEst_Codigo as varchar)+'_'+" &
                                    "CAST(i.FolDoc_Codigo as varchar)+'_'+CAST(i.folconsecutivo as varchar)) end)" &
                                    " " &
                                    "from Compucaja.dbo.Ingresos AS i with (nolock) right outer join T_Archivo as AR with (nolock) " &
                                    "on i.foltda_codigo=ar.tienda and i.Ing_TrxId=ar.referencia " &
                                    "Left Join " &
                                    "PagoDeServicios.dbo.T_ConceptosDeServicios as Con WITH (NOLOCK) on i.ci_codigo=con.cri_codigo " &
                                    " left join ConciliacionServicios as MAS WITH (NOLOCK) on ar.tienda=mas.tienda " &
                                    "and i.folconsecutivo=mas.consecutivo and i.ing_fecha=mas.fecha and ar.referencia=mas.referencia " &
                                    "where ((i.Ing_Fecha BETWEEN '20140101' AND GETDATE()) or " &
                                    "(i.Ing_Fecha is null))  " &
                                    "and (i.CI_Codigo=59 or i.CI_Codigo is null) and mas.CRI_Codigo is null and " &
                                    "ar.Tienda IN (SELECT SiteId FROM SuperKioscoApp.dbo.Site) "

                            ccomando = New SqlCommand
                            ccomando.Connection = cnx
                            ccomando.CommandText = qry
                            ccomando.ExecuteNonQuery()

                            ccomando = New SqlCommand
                            ccomando.Connection = cnx
                            ccomando.CommandText = "delete from T_Archivo"
                            ccomando.ExecuteNonQuery()


                            Dim sArchivoOrigen As String = RutasTransCol.Rows(0)("RutaOrigen") & "\" & nombre.Trim
                            Dim sRutaDestino As String = RutasTransCol.Rows(0)("RutaDestino") & "\" & nombre.Trim
                            My.Computer.FileSystem.MoveFile(sArchivoOrigen, sRutaDestino, True)

                        End If

                    Next
                Catch ex As Exception
                    'sqls.GrabaTextoEnArchivo("Error al conciliar Servicios : " + ex.Message + " - Arrchivo " + nombre.Trim + " Detalle ultimtnmam linea -  Tienda : " + linea(2).Trim & " - Estacion: " & linea(4).Trim & " - Referencia: " & linea(9).Trim & " - Servicio:" & id_serviciox.Trim & "|")
                    MsgBox(ex.Message)
                Finally
                    cnx.Close()
                    If mostrarMsj Then MessageBox.Show("Proceso Terminado")
                End Try
                'Process.Start("notepad.exe", "c:\milogconciliacion.log")       'abrir archivo despues del proceso
            End If
        End If
        Me.Text = "Version 1.1"
    End Sub
    Public Sub Log(ByVal sMsg As String, ByVal servicio As String)
        'Dim Ruta As String = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6)
        Dim oSW As New StreamWriter("C:\milogconciliacion.log", True)
        'Dim scomando As String = String.Empty
        oSW.WriteLine(servicio & " -- " & Now & " ===> " & sMsg)
        oSW.Flush()
        oSW.Close()
    End Sub

    Private Sub btnConciliarTodo_Click(sender As Object, e As EventArgs) Handles btnConciliarTodo.Click
        mostrarMsj = False
        Button8_Click(Nothing, Nothing)
        Button15_Click(Nothing, Nothing)
        Button23_Click(Nothing, Nothing)
        mostrarMsj = True
        MessageBox.Show("Proceso Terminado")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim source As DataTable = sqls.DevuelveDatos(cnx, "Select CRI_Codigo Id,Descripcion Descr from T_ConceptosDeServicios where EstatusServ=1",
                           FuncSQL.FuncSQL.EnumFormatoSalida.DataTable_)
        Using frm As New FrmBrowseGral(source, "")
            frm.ShowDialog()
            Button6.ImageKey = frm.IdItem
            Button24_Click(Button6, Nothing)
            Button6.ImageKey = ""
            frm.Dispose()
        End Using
    End Sub
End Class
