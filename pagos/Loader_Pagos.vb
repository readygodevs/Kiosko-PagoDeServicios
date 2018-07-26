Imports System.Data.SqlClient
Public Class Loader_Pagos
    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=pagodeservicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public transaccion As SqlTransaction
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)
    Public LotesGenerados As String = ""
    Dim Conceptos As DataTable
    Dim InfoDynamics As DataTable
    Dim fechaIni As DateTime
    Dim fechaFin As DateTime
    Sub New(ByVal titulo As String, ByVal lConceptos As DataTable, ByVal lInfoDyn As DataTable,
            ByVal lfechaIni As DateTime, ByVal lfechaFin As DateTime)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        Me.Text = titulo
        Conceptos = lConceptos
        InfoDynamics = lInfoDyn
        LotesGenerados = ""
        fechaIni = lfechaIni
        fechaFin = lfechaFin
    End Sub
    Private Sub My_BgWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles My_BgWorker.DoWork
        Dim Debug As String = "1"

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

        If cnx.State = ConnectionState.Closed Then cnx.Open()
        transaccion = cnx.BeginTransaction
        Dim rutarchivo As String = String.Empty
        Dim qry3 As String = "Select * from T_RutasFTP where IdRuta=4"
        Dim rutaPag As DataTable = sqls.DevuelveDatos(cnx, qry3, , , , , transaccion)

        Dim qry4 As String = "Select RutaOrigen from T_RutasFTP where IdRuta=5"
        Dim TRutaCTL As DataTable = sqls.DevuelveDatos(cnx, qry4, , , , , transaccion)
        Dim RutaCTL As String = TRutaCTL.Rows(0)(0)
        rutarchivo = ""
        Dim NombreArchivo As String = ""
        Try
            For Each ServicioAPagar As DataRow In Conceptos.Rows '.Select("CRI_Codigo=76") 'recorrer servicios que corresponde pagar HOY
                'ServicioAPagar("CRI_Codigo") = ServicioAPagar("CRI_Codigo")
                qry = String.Empty
                lop.Clear()
                lop.Add(sqls.CreadbParametros("@FechaIni", fechaIni))
                lop.Add(sqls.CreadbParametros("@FechaFin", fechaFin))
                'lop.Add(sqls.CreadbParametros("@diapago", CbxServicio.Text))


                lop.Add(sqls.CreadbParametros("@idservicio", ServicioAPagar("CRI_Codigo")))

                qry = "Select TienePeriodoSem,TieneDiasCredito from T_ConceptosDeServicios where CRI_Codigo=@idservicio"


                dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
                Dim conDiaCredito As Boolean = dt.Rows(0)(1)


                If dt.Rows(0)(0) Then 'periodo semanal
                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ," &
                 "SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago  " &
                 "FROM ConciliacionServicios AS ps with (nolock) LEFT JOIN  compucaja.dbo.Servicios AS s with (nolock) ON " &
                 "ps.CRI_Codigo=s.CRI_Codigo inner join T_ConceptosDeServicios As cs on ps.cri_codigo=cs.cri_codigo " &
                 "WHERE ps.CRI_Codigo=@idservicio And conciliado=1 And " &
                 "fechapago Is null And fecha BETWEEN @FechaIni And @FechaFin " &
                 "And dbo.fn_Obtener_Fecha_PorDia(ps.Fecha, CS.DiaFin, 0) <= dbo.fn_fecha_sinhora(GETDATE())"
                ElseIf dt.Rows(0)(1) Then 'dias credito
                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ," &
                 "SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago  " &
                 "FROM ConciliacionServicios AS ps with (nolock) " &
                 "LEFT JOIN " &
                 " compucaja.dbo.Servicios AS s with (nolock) ON " &
                 " ps.CRI_Codigo=s.CRI_Codigo inner join " &
                 "T_ConceptosDeServicios As cs on ps.cri_codigo=cs.cri_codigo " &
                 "WHERE ps.CRI_Codigo=@idservicio And conciliado=1 And " &
                 "fechapago Is null " &
                 "And fecha <= DateAdd(Day, -1 * cs.DiasCredito, @FechaFin) "

                Else
                    qry = "SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ," &
                 "SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago  " &
                 "FROM ConciliacionServicios AS ps with (nolock) LEFT JOIN  compucaja.dbo.Servicios AS s with (nolock) ON " &
                 "ps.CRI_Codigo=s.CRI_Codigo  WHERE ps.CRI_Codigo=@idservicio AND conciliado=1 AND " &
                 "fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin " & IIf(ServicioAPagar("CRI_Codigo") = 16, " AND sv_codigo=7 ", "")
                End If



                'End If

                dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)

                Debug = "1.0"
                Dim maxDate As String = ""
                Dim minDate As String = ""

                If dt.Rows.Count > 0 Then
                    maxDate = dt.Compute("MAX(fecha)", "")
                    minDate = dt.Compute("MIN(fecha)", "")
                End If

                Debug = "1.1"

                'dgservicio.DataSource = dt
                Dim exApp As New Microsoft.Office.Interop.Excel.Application
                Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
                Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet
                Dim bandera As Boolean
                Dim x As Integer
                Dim y As Integer
                Dim mes As String
                Dim dia As String

                mes = DateTime.Now.ToString("MMMM")
                'dial = DateTime.Now.ToString("dddd")
                dia = Now.Day
                dia = dia - 1
                If dia = 0 Then
                    mes = DateTime.Now.AddMonths(-1).ToString("MMMM")
                    dia = DateTime.Now.AddDays(-1).Day
                End If
                If dt.Rows.Count > 0 Then
                    Debug = 5
                    If (Not String.IsNullOrEmpty(dt.Rows(0)(0))) _
                                And (dt.Rows(0)(0) IsNot Nothing) Then
                        'Añadimos el Libro al programa, y la hoja al libro
                        exLibro = exApp.Workbooks.Add
                        exHoja = exLibro.Worksheets.Add()
                        exHoja.Columns(1).NumberFormat = "@"
                        exHoja.Columns(4).NumberFormat = "@"
                        exHoja.Columns(5).NumberFormat = "#,##0.00"
                        ' ¿Cuantas columnas y cuantas filas?
                        Dim NCol As Integer = dt.Columns.Count()
                        Dim NRow As Integer = dt.Rows.Count
                        x = NCol
                        y = NRow
                        NCol = NCol - 2
                        'Aqui recorremos todas las filas, y por cada fila todas las columnas y vamos escribiendo.
                        Debug = 6
                        For i As Integer = 1 To NCol
                            exHoja.Cells.Item(3, i) = dt.Columns(i - 1).Caption.ToString
                        Next
                        Debug = 7
                        For Fila As Integer = 0 To NRow - 1
                            For Col As Integer = 0 To NCol - 1
                                exHoja.Cells.Item(Fila + 4, Col + 1) =
                                            Trim(IIf(IsDBNull(dt.Rows(Fila)(Col)), "", dt.Rows(Fila)(Col)))
                            Next
                        Next
                        Debug = 8
                        exHoja.Cells.Item(y + 4, x - 3) = "TOTAL"
                        Dim Total As Decimal = 0
                        For Each row As DataRow In dt.Rows()
                            Total += Val(row(4))
                        Next

                        exHoja.Cells.Item(y + 4, x - 2) = Format(Total, "$ #,##0.00")
                        'Titulo en negrita, Alineado al centro y que el tamaño de la columna se ajuste al texto
                        exHoja.Rows.Item(3).Font.Bold = 1
                        exHoja.Rows.Item(3).HorizontalAlignment = 3
                        exHoja.Columns.AutoFit()
                        'Aplicación visible
                        exApp.Application.Visible = True

                        Dim qry As String = "SELECT CON.CRI_Codigo,CON.descripcion FROM dbo.T_ConceptosDeServicios as CON left join T_DiasDePagoServicios as DP on " &
                                     "COn.DiasDePago=DP.IdDiasPago where DP.DiasDePago='MENSUAL' " &
                                     "and CON.EstatusServ=1 and CON.CRI_Codigo='" & ServicioAPagar("CRI_Codigo") & "'"
                        Dim esMensual As DataTable = sqls.DevuelveDatos(cnx, qry, , , , , transaccion)

                        qry = " SELECT MAX(fechapago) AS fechaultimopago " &
                 " FROM ConciliacionServicios  with (nolock) WHERE " & IIf(ServicioAPagar("CRI_Codigo") = 50 Or ServicioAPagar("CRI_Codigo") = 51, "CRI_Codigo in (50,51)", " CRI_Codigo=@idservicio ")
                        dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
                        If IsDBNull(dt.Rows(0)(0)) Then
                            qry = " SELECT MAX(fechapago) AS fechaultimopago " &
                 " FROM compucaja.dbo.pagoservicios  with (nolock) WHERE " & IIf(ServicioAPagar("CRI_Codigo") = 50 Or ServicioAPagar("CRI_Codigo") = 51, "id_servicio in (50,51)", " id_servicio=@idservicio ")
                            dt = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)
                        End If

                        '--de la fecha ultima de pago se toma el dia para el siguiente pago
                        Dim fechapagox As String = dt.Rows(0).Item("fechaultimopago").ToString
                        Dim diapagox As String() = fechapagox.Split("/")


                        NombreArchivo = ""

                        If conDiaCredito Then 'dias credito

                            NombreArchivo = rutaPag.Rows(0)("RutaOrigen") & "\" & ServicioAPagar("Descripcion") & "_del " &
                                                  Convert.ToDateTime(minDate).ToString("dd") & " al " + Convert.ToDateTime(maxDate).ToString("dd") + " de " + Convert.ToDateTime(maxDate).ToString("MMMM")
                        Else
                            If esMensual.Rows.Count = 0 Then
                                'colocar titulo para los no mensuales
                                NombreArchivo = rutaPag.Rows(0)("RutaOrigen") & "\" & ServicioAPagar("Descripcion") & "_del " &
                                                   CInt(IIf(diapagox(0) = "", 0, diapagox(0))) & " " + "al" + " " & dia & " " & mes
                            Else
                                Dim thisMonth As New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                                Dim firstDayLastMonth As DateTime
                                Dim lastDayLastMonth As DateTime

                                firstDayLastMonth = thisMonth.AddMonths(-1)
                                lastDayLastMonth = thisMonth.AddDays(-1)

                                'colocar titulo para los mensuales
                                NombreArchivo = rutaPag.Rows(0)("RutaOrigen") & "\" & ServicioAPagar("Descripcion") & "_del " &
                                                   firstDayLastMonth.Day & " " + "al" + " " & lastDayLastMonth.Day & " " &
                                                   firstDayLastMonth.ToString("MMMM")
                            End If
                        End If


                        rutarchivo = exLibro.FullName

                        If ServicioAPagar("CRI_Codigo") <> 76 Then
                            exLibro.SaveAs(NombreArchivo, , "", "", False, False)
                            exApp.Quit()
                            ReleaseCom(exApp)
                        End If
                        'exHoja = New Microsoft.Office.Interop.Excel.Worksheet

                        bandera = True
                    Else
                        bandera = False
                    End If
                Else

                    bandera = False
                End If

                Debug = 9
                If bandera = True Then
                    '*****************************************************************************
                    lop.Clear()
                    qry = ""
                    lop.Add(sqls.CreadbParametros("@FechaIni", fechaIni))
                    lop.Add(sqls.CreadbParametros("@FechaFin", fechaFin))
                    'lop.Add(sqls.CreadbParametros("@idservicio", IIf(CbxServicio.Text = "AUTOGESTION CFE", "50,51", ServicioAPagar("CRI_Codigo"))))
                    Dim direcciones As String = Nothing

                    'qry = "UPDATE ConciliacionServicios SET fechapago=GETDATE() " &
                    '              "WHERE cri_codigo in ( " & ServicioAPagar("CRI_Codigo") &
                    '              ") AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"

                    qry = "UPDATE CON SET FechaPago=GETDATE() " &
                            "FROM    dbo.ConciliacionServicios CON " &
                                    "INNER Join dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                            "WHERE   CON.cri_codigo IN ( " & ServicioAPagar("CRI_Codigo") & " ) " &
                                    "And conciliado = 1 " &
                                    "And fechapago Is NULL " &
                                    "And fecha BETWEEN @FechaIni AND @FechaFin " &
                                    "And TB.TienePeriodoSem = 0 " &
                                    "And TB.TieneDiasCredito = 0 " &
                                    "UPDATE CON SET FechaPago=GETDATE() " &
                                    "FROM    dbo.ConciliacionServicios CON " &
                                     "       INNER JOIN dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                                    "WHERE   CON.cri_codigo IN ( " & ServicioAPagar("CRI_Codigo") & " ) " &
                                     "AND conciliado = 1 " &
                                     "And fechapago Is NULL " &
                                     "And fecha BETWEEN @FechaIni AND @FechaFin " &
                                     "And TB.TienePeriodoSem = 1 " &
                                     "And TB.TieneDiasCredito = 0 " &
                                     "And dbo.fn_Obtener_Fecha_PorDia(CON.Fecha, TB.DiaFin, 0) <= dbo.fn_fecha_sinhora(GETDATE()) " &
                                    "UPDATE CON SET FechaPago=GETDATE() " &
                                    "FROM    dbo.ConciliacionServicios CON " &
                                     "       INNER JOIN dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                                    "WHERE   CON.cri_codigo IN ( " & ServicioAPagar("CRI_Codigo") & " ) " &
                                     "AND conciliado = 1 " &
                                     "And fechapago Is NULL " &
                                     "And fecha <= DateAdd(Day, -1 * TB.DiasCredito, @FechaFin) " &
                                     "And TB.TienePeriodoSem = 0 " &
                                     "And TB.TieneDiasCredito = 1 "
                    Debug = 10
                    'sqls.ComandoSQL(cnx, qry, lop, , , transaccion)
                    'llena otra vez grid con consulta vacia
                    qry = ""
                    qry = " SELECT CONVERT(CHAR(4),Tienda)+'-'+ CONVERT(CHAR(1),Estacion)+'-'+CONVERT(CHAR(10),Consecutivo) AS Folio ,SV_Nombre AS Servicio,Fecha,Referencia,Importe,Archivo,Fechapago " &
                                  " FROM ConciliacionServicios AS ps INNER JOIN  Compucaja.dbo.Servicios AS s ON ps.cri_codigo=s.CRI_Codigo  " &
                                  "WHERE ps.cri_codigo in (" & ServicioAPagar("CRI_Codigo") &
                                  ") " &
                                  "AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin "
                    Debug = 11
                    'dgservicio.DataSource = sqls.DevuelveDatos(cnx, qry, , , lop, , transaccion)


                    'Me.sumaimporte.Text = ""

                    ''generar y cargar DTA
                    Dim ctaCont As DataRow = New DataView(InfoDynamics, "CRI_Agrupador=" &
                                                         Int(ServicioAPagar("CRI_Codigo")), "", DataViewRowState.CurrentRows).ToTable().Rows(0) ' tablaServiciosPagar.Select("CRI_Agrupador=" & ServicioAPagar("CRI_Codigo") &
                    '" OR Descripcion='" & ServicioAPagar("Descripcion") & "'")(0)

                    Dim qryVal As String = "SELECT * FROM superkioscoapp.dbo.APDoc AS ad with(nolock) WHERE rtrim(InvcNbr) = rtrim('" &
                            ctaCont("Referencia") & "')"
                    Dim existeRef As DataTable = sqls.DevuelveDatos(cnx, qryVal, , , , , transaccion)
                    If existeRef.Rows.Count = 0 Then

                        Dim FileDTA As String = DateTime.Now.ToString("yyyyMMddHHmmssffff") & ".DTA"
                        'Dim filtro As New DataView(tablaServiciosPagar)
                        'filtro.RowFilter = "CuentaContable='" & ctaCont(0) & "'"
                        'Dim ServiciosCuenta As DataTable = filtro.ToTable

                        Dim Total As Decimal = ctaCont("Importe") 'ServiciosCuenta.Compute("SUM(Importe)", "")
                        Dim TipoDoc As String = "VO"
                        Dim Proveedor As String = ctaCont("Proveedor")
                        Dim FechaPago As String = DateTime.Now.ToString("MM/dd/yyyy")
                        Dim Cuenta As String = ctaCont("CuentaContable")
                        Dim Entidad As String = "00-0000"
                        Dim Referencia As String = Proveedor & ctaCont("CRI_Agrupador") & DateTime.Now.ToString("yyMMdd")
                        Dim LineaLote As String = "Batch, " & Total

                        writeDebug(LineaLote, FileDTA)
                        Dim LineaDocumento As String = "Document, " & TipoDoc & ", " & Proveedor & ", " & FechaPago & ", " &
                                     Referencia & ", " & FechaPago & ", " & Total

                        writeDebug(LineaDocumento, FileDTA)

                        Dim LineaDetalle As String = ""

                        LineaDetalle = "Transaction, " & Cuenta & ", " & Entidad & ", " & Total & ", " &
                                    (ctaCont("Descripcion").ToString().Replace("INGRESO COBROS", "")) &
                                    ", " & FechaPago
                        writeDebug(LineaDetalle, FileDTA)




                        Dim str, strLog As String
                        str = RutaCTL & "\DTA\" & FileDTA

                        strLog = IO.Path.GetDirectoryName(str) & "\" & IO.Path.GetFileNameWithoutExtension(str) & ".LOG"
                        Dim ArchivoCTL As String = RutaCTL & "\CTL\0301000.CTL"
                        Dim qrySL As String = "Select Top 1 Valorini from AlmDinamico.dbo.RENValorVig where RENClave='SL_APP' order by RVVFechaInicial desc"
                        Debug = 13
                        Dim RutaSL As DataTable = sqls.DevuelveDatos(cnx, qrySL, , , , , transaccion)
                        qryVal = "SELECT * FROM superkioscoapp.dbo.APDoc AS ad with(nolock) WHERE rtrim(InvcNbr) = rtrim('" &
                                Referencia & "') and BatNbr<>''"
                        existeRef = sqls.DevuelveDatos(cnx, qryVal, , , , , transaccion)
                        Debug = 13.1
                        Dim generarLote As Boolean = False
                        If existeRef.Rows.Count = 0 Then
                            If ServicioAPagar("CRI_Codigo") = 76 Then 'GENERAR REFERENCIA WS UDEC

                                Dim wsUdeC As New wsUcolProduccion.Service()
                                Dim referenciaPagoUdeC As String = ""
                                Debug = 13.2
                                Dim totalWS As Decimal = Decimal.Parse(ctaCont("Importe").ToString())
                                Dim response As wsUcolProduccion.respuesta = wsUdeC.generarReferencia("kiosco", "K14M32s4", totalWS)
                                If response.Respuesta = 0 Then
                                    referenciaPagoUdeC = response.txtrespuesta
                                Else
                                    'Throw New Exception("Error al generar referencia con WS de UdeC. Debug:" & Debug & ". " & response.txtrespuesta)
                                    referenciaPagoUdeC = "Error al generar Referencia UdeC, obtenerla manualmente."
                                End If
                                Debug = 13.3
                                exHoja.Cells.Item(y + 4, x - 1) = "Referencia UdeC"
                                'exHoja.Cells.Item(y + 4, x - 1).
                                exHoja.Cells.Item(y + 4, x) = referenciaPagoUdeC
                                Debug = 13.4
                                exLibro.SaveAs(NombreArchivo, , "", "", False, False)
                                exApp.Quit()
                                ReleaseCom(exApp)
                            End If
                            CargaDTA(str, strLog, ArchivoCTL, RutaSL.Rows(0)(0) & "\0301000.exe") ' VentanaPOSolomon)
                            generarLote = True
                        End If
                        'obtener lote
                        Debug = 14
                        Dim dtBat As DataTable = sqls.DevuelveDatos(cnx, "SELECT BatNbr FROM superkioscoapp.dbo.apdoc " &
                                                    "AS ad with(nolock) WHERE invcnbr = '" & Referencia &
                                                    "' order by ad.Crtd_DateTime desc", , , , , transaccion)


                        Debug = 15
                        Dim dt4 As DataTable = sqls.DevuelveDatos(cnx, "Select CRI_Codigo from T_ConceptosDeServicios where CRI_Agrupador='" &
                                                                      ctaCont("CRI_Agrupador") & "'", , , , , transaccion)
                        Dim cri_codigos As String = ""
                        For Each cod As DataRow In dt4.Rows
                            cri_codigos &= IIf(cri_codigos = "", "", ",") & "'" & cod(0) & "'"
                        Next
                        Dim cComando As New SqlCommand()
                        cComando.Connection = cnx
                        Debug = 16
                        lop.Clear()
                        lop.Add(sqls.CreadbParametros("@FechaIni", fechaIni))
                        lop.Add(sqls.CreadbParametros("@FechaFin", fechaFin))
                        qry = "Update ConciliacionServicios set fechapago=GETDATE(),ReferenciaCxP='" & Referencia & "'," &
                                "LoteCxP='" & dtBat.Rows(0)("BatNbr") & "' " &
                                "WHERE cri_codigo in ( " & cri_codigos &
                                  ") AND conciliado=1 AND fechapago is null AND fecha BETWEEN @FechaIni AND @FechaFin"

                        qry = "UPDATE CON SET fechapago=GETDATE(),ReferenciaCxP='" & Referencia & "'," &
                                "LoteCxP='" & dtBat.Rows(0)("BatNbr") & "' " &
                            "FROM    dbo.ConciliacionServicios CON " &
                                    "INNER Join dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                            "WHERE   CON.cri_codigo IN ( " & cri_codigos & " ) " &
                                    "And conciliado = 1 " &
                                    "And fechapago Is NULL " &
                                    "And fecha BETWEEN @FechaIni AND @FechaFin " &
                                    "And TB.TienePeriodoSem = 0 " &
                                    "And TB.TieneDiasCredito = 0 " &
                                    "UPDATE CON SET fechapago=GETDATE(),ReferenciaCxP='" & Referencia & "'," &
                                "LoteCxP='" & dtBat.Rows(0)("BatNbr") & "' " &
                                    "FROM    dbo.ConciliacionServicios CON " &
                                     "       INNER JOIN dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                                    "WHERE   CON.cri_codigo IN ( " & cri_codigos & " ) " &
                                     "AND conciliado = 1 " &
                                     "And fechapago Is NULL " &
                                     "And fecha BETWEEN @FechaIni AND @FechaFin " &
                                     "And TB.TienePeriodoSem = 1 " &
                                     "And TB.TieneDiasCredito = 0 " &
                                     "And dbo.fn_Obtener_Fecha_PorDia(CON.Fecha, TB.DiaFin, 0) <= dbo.fn_fecha_sinhora(GETDATE()) " &
                                     "UPDATE CON SET fechapago=GETDATE(),ReferenciaCxP='" & Referencia & "'," &
                                "LoteCxP='" & dtBat.Rows(0)("BatNbr") & "' " &
                                    "FROM    dbo.ConciliacionServicios CON " &
                                     "       INNER JOIN dbo.T_ConceptosDeServicios TB ON CON.CRI_Codigo = TB.CRI_Codigo " &
                                    "WHERE   CON.cri_codigo IN ( " & cri_codigos & " ) " &
                                     "AND conciliado = 1 " &
                                     "And fechapago Is NULL " &
                                     "And TB.TienePeriodoSem = 0 " &
                                     "And TB.TieneDiasCredito = 1 " &
                                     "And fecha <= DateAdd(Day, -1 * TB.DiasCredito, @FechaFin) "
                        If generarLote Then
                            sqls.ComandoSQL(cnx, qry, lop, , , transaccion)
                        End If

                        LotesGenerados &= IIf(LotesGenerados = "", "", ", ") & dtBat.Rows(0)("BatNbr")

                        transaccion.Commit()
                        transaccion.Dispose()
                        cnx.Close()
                        cnx.Dispose()
                        cnx = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=pagodeservicios;Data Source=192.168.100.6 ;connection timeout=0")
                        cnx.Open()
                        transaccion = cnx.BeginTransaction
                        'cnx.Close()
                        'cnx.Dispose()
                    End If
                End If

                My_BgWorker.ReportProgress(CInt((Conceptos.Rows.IndexOf(ServicioAPagar) / Conceptos.Rows.Count) * 100))
            Next
        Catch ex As Exception
            transaccion.Rollback()

            MessageBox.Show(" ERROR : " & ex.Message & ". DEBUG:" & Debug, "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error)
            My_BgWorker.ReportProgress(100)
        Finally
            transaccion.Dispose()
            cnx.Close()
            cnx.Dispose()
        End Try





    End Sub
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