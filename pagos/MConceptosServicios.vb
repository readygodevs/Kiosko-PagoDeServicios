Public Class MConceptosServicios
    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)
    Public Refrescar As Boolean
    Dim lValorPago As Integer
    Dim lValorCobro As Integer

    Sub New(ByVal idservicio As String, ByVal descripcion As String, ByVal estatus As Boolean, ByVal cri_codigo As String,
            ByVal tiposerv As String, ByVal diasPago As Integer, ByVal diasCobro As Integer, ByVal emailP As String,
            ByVal emailK As String, ByVal archivoFTP As String, ByVal desde As Decimal, ByVal hasta As Decimal, ByVal ctaContable As String,
            ByVal sumaC As Boolean, ByVal restaCo As Boolean, ByVal proveedor As String, ByVal tienePeriodo As Boolean, ByVal diainic As String, ByVal diafin As String,
            ByVal tieneDiaCredito As Boolean, ByVal diasCredito As String, ByVal criagrupador As String)

        ' This call is required by the designer.
        InitializeComponent()
        Refrescar = False
        ' Add any initialization after the InitializeComponent() call.
        txtCRIAgrupador.Text = criagrupador
        txtCRI_Cod.Text = cri_codigo
        txtDescr.Text = descripcion
        txtIdServ.Text = idservicio
        txtTipo.Text = tiposerv
        lValorCobro = diasCobro
        lValorPago = diasPago
        txtArchivoFTP.Text = archivoFTP
        chkEstatus.Checked = estatus
        txtDesde.Value = desde
        txtHasta.Value = hasta
        txtEmailKiosko.Text = emailK
        txtEmailProv.Text = emailP
        txtCtaContable.Text = ctaContable
        chkSumaC.Checked = sumaC
        chkRestaC.Checked = restaCo
        txtProveedor.Text = proveedor



        If Not tienePeriodo Then
            grbPeriodoSemanal.Enabled = False
        End If

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

        qry = "Select * from xv_dias_semana"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        ddlDe.DisplayMember = "Dia"
        ddlDe.ValueMember = "IdDia"
        ddlDe.DataSource = dt

        qry = "Select * from xv_dias_semana"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        ddlA.DisplayMember = "Dia"
        ddlA.ValueMember = "IdDia"
        ddlA.DataSource = dt

        chkPeriodoSemanal.Checked = tienePeriodo

        chkDiasCredito.Checked = tieneDiaCredito

        If tienePeriodo Then
            grbPeriodoSemanal.Enabled = True
            ddlDe.SelectedValue = diainic
            ddlA.SelectedValue = diafin
        End If

        If Not tieneDiaCredito Then
            grbDiasCredito.Enabled = False
        Else
            txtDiasCredito.Value = diasCredito
        End If

        cmbDiasCobro.DropDownStyle = ComboBoxStyle.DropDownList
        cmbDiasPago.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub MConceptosServicios_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        qry = "Select 0 as iddiaspago,'Seleccione días' as descr union all " &
            "SELECT iddiaspago,(periodicidaddepago+' - '+diasdepago) as descr from T_DiasDePagoServicios"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        cmbDiasCobro.DisplayMember = "descr"
        cmbDiasCobro.ValueMember = "iddiaspago"
        cmbDiasCobro.DataSource = dt
        cmbDiasCobro.SelectedValue = lValorCobro

        qry = "Select 0 as iddiaspago,'Seleccione días' as descr union all " &
            "SELECT iddiaspago,(periodicidaddepago+' - '+diasdepago) as descr from T_DiasDePagoServicios"
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        cmbDiasPago.DisplayMember = "descr"
        cmbDiasPago.ValueMember = "iddiaspago"
        cmbDiasPago.DataSource = dt
        cmbDiasPago.SelectedValue = lValorPago



        Me.Text = "Modificar Concepto de Servicio"
        Me.CenterToScreen()
        Me.CenterToParent()
        Me.AutoSize = False
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            lop.Clear()
            lop.Add(sqls.CreadbParametros("@Est", chkEstatus.Checked))
            lop.Add(sqls.CreadbParametros("@Archivo", txtArchivoFTP.Text))
            lop.Add(sqls.CreadbParametros("@EmailP", txtEmailProv.Text))
            lop.Add(sqls.CreadbParametros("@EmailK", txtEmailKiosko.Text))
            lop.Add(sqls.CreadbParametros("@suma", chkSumaC.Checked))
            lop.Add(sqls.CreadbParametros("@resta", chkRestaC.Checked))
            lop.Add(sqls.CreadbParametros("@Periodo", chkPeriodoSemanal.Checked))
            lop.Add(sqls.CreadbParametros("@DiaCre", chkDiasCredito.Checked))
            lop.Add(sqls.CreadbParametros("@Inicio", ddlDe.SelectedValue))
            lop.Add(sqls.CreadbParametros("@Fin", ddlA.SelectedValue))
            lop.Add(sqls.CreadbParametros("@DiasCredito", txtDiasCredito.Value))
            lop.Add(sqls.CreadbParametros("@CriAgrupador", txtCRIAgrupador.Text))
            qry = "Update T_ConceptosDeServicios set DiasDePago=" & cmbDiasPago.SelectedValue &
                ",DiasDeCobro=" & cmbDiasCobro.SelectedValue & ",EstatusServ=@Est,ArchivoFTP=@Archivo," &
                "EmailProv=@EmailP,EmailKiosko=@EmailK,RangoDesde=" & txtDesde.Value & ",RangoHasta=" & txtHasta.Value &
                ",SumaCargo=@suma,RestaContr=@resta,TienePeriodoSem=@Periodo,TieneDiasCredito=@DiaCre,CRI_Agrupador=@CriAgrupador " &
                IIf(chkPeriodoSemanal.Checked, ",DiaInicio=@Inicio,DiaFin=@Fin ", "") &
                IIf(chkDiasCredito.Checked, ",DiasCredito=@DiasCredito ", "") &
                " where IdServicio=" & txtIdServ.Text
            sqls.ComandoSQL(cnx, qry, lop, , FuncSQL.FuncSQL.EnumTipoBase._SQL, , )

            Refrescar = True
            cnx.Close()
            btnCancelar_Click(Nothing, Nothing)
        Catch ex As Exception
            MessageBox.Show("Error al guardar el Concepto de Servicio: " & ex.Message)
        End Try

    End Sub

    Private Sub chkPeriodoSemanal_CheckedChanged(sender As Object, e As EventArgs) Handles chkPeriodoSemanal.CheckedChanged

        'RemoveHandler chkDiasCredito.CheckedChanged, AddressOf chkDiasCredito_CheckedChanged

        grbPeriodoSemanal.Enabled = False

        If chkPeriodoSemanal.Checked Then
            grbPeriodoSemanal.Enabled = True
            grbDiasCredito.Enabled = False
            txtDiasCredito.Minimum = 0
            txtDiasCredito.Value = 0
            chkDiasCredito.Checked = False
        End If

        'AddHandler chkDiasCredito.CheckedChanged, AddressOf chkDiasCredito_CheckedChanged
    End Sub

    Private Sub chkDiasCredito_CheckedChanged(sender As Object, e As EventArgs) Handles chkDiasCredito.CheckedChanged

        'RemoveHandler chkPeriodoSemanal.CheckedChanged, AddressOf chkPeriodoSemanal_CheckedChanged

        grbDiasCredito.Enabled = False

        txtDiasCredito.Minimum = 0
        txtDiasCredito.Value = 0

        If chkDiasCredito.Checked Then
            grbDiasCredito.Enabled = True
            grbPeriodoSemanal.Enabled = False
            txtDiasCredito.Minimum = 1
            txtDiasCredito.Value = 1
            ddlDe.SelectedIndex = 0
            ddlA.SelectedIndex = 0

            If chkPeriodoSemanal.Enabled Then
                chkPeriodoSemanal.Checked = False
            End If
        End If

        'AddHandler chkPeriodoSemanal.CheckedChanged, AddressOf chkPeriodoSemanal_CheckedChanged
    End Sub

    Private Sub cmbDiasPago_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDiasPago.SelectedIndexChanged

        If cmbDiasPago.SelectedValue = 0 Then Exit Sub

        qry = "SELECT tieneperiodosem from T_DiasDePagoServicios where iddiaspago=" & cmbDiasPago.SelectedValue
        dt = sqls.DevuelveDatos(cnx, qry, , , )
        chkPeriodoSemanal.Enabled = True

        If Not dt.Rows(0)(0) Then
            chkPeriodoSemanal.Enabled = False
            grbPeriodoSemanal.Enabled = False
            chkPeriodoSemanal.Checked = False
        End If
    End Sub

    Private Sub tbSecurity_KeyPress(sender As System.Object, e As System.EventArgs) Handles MyBase.KeyPress
        Dim tmp As System.Windows.Forms.KeyPressEventArgs = e
        If tmp.KeyChar = ChrW(Keys.Enter) Then
            'MessageBox.Show("Enter key")
            btnAceptar_Click(Nothing, Nothing)
        Else
            'MessageBox.Show(tmp.KeyChar)
            Me.Close()
        End If

    End Sub

End Class