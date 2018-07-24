Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class UpdConServicio
    Public cnx As SqlClient.SqlConnection = Nothing
    Public transaccion As SqlTransaction
    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public lop As New List(Of Common.DbParameter)
    Public exito As Boolean

    Sub New(ByVal lCnx As SqlConnection, ByVal cricod As String, ByVal tienda As String,
            ByVal consecutivo As String, ByVal fecha As String,
            ByVal referencia As String)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        cnx = lCnx
        hdnCriCodigo.Text = cricod
        hdnTienda.Text = tienda
        hdnConsecutivo.Text = consecutivo
        hdnFecha.Text = Convert.ToDateTime(fecha).ToString("yyyy-MM-dd HH:mm:ss")
        hdnReferencia.Text = referencia

        exito = False
    End Sub
    Private Sub UpdConServicio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim qry4 As String = "Select Cri_Codigo,(Cri_Codigo+' '+Descripcion) Descripcion " &
            "from T_ConceptosDeServicios"
        If cnx.State = ConnectionState.Closed Then cnx.Open()
        Dim conceptosServ As DataTable = sqls.DevuelveDatos(cnx, qry4, , , )

        ddlCriCodigo.DataSource = conceptosServ
        ddlCriCodigo.ValueMember = "Cri_Codigo"
        ddlCriCodigo.DisplayMember = "Descripcion"

        ddlCriCodigo.SelectedValue = hdnCriCodigo.Text
        txtTienda.Value = hdnTienda.Text
        txtConsecutivo.Value = hdnConsecutivo.Text
        dtpFecha.Value = hdnFecha.Text
        txtReferencia.Text = hdnReferencia.Text

        Dim qry As String = "select * from conciliacionservicios where cri_codigo=" & hdnCriCodigo.Text &
            " And Tienda=" & hdnTienda.Text & " And Consecutivo=" & hdnConsecutivo.Text &
            " And CONVERT(VARCHAR(30),Fecha,20)='" &
            dtpFecha.Value.ToString("yyyy-MM-dd HH:mm:ss") &
            "' and rtrim(Referencia)=rtrim('" & hdnReferencia.Text & "')"
        Dim servicios As DataTable = sqls.DevuelveDatos(cnx, qry, , , )

        txtEstacion.Value = servicios.Rows(0)("Estacion")
        txtHora.Text = servicios.Rows(0)("Hora")
        txtImporte.Value = servicios.Rows(0)("Importe")
        chkConciliado.Checked = servicios.Rows(0)("Conciliado")
        txtFolio.Text = IIf(servicios.Rows(0)("Folio") Is DBNull.Value, "", servicios.Rows(0)("Folio"))
        txtObservaciones.Text = IIf(servicios.Rows(0)("Observaciones") Is DBNull.Value, "", servicios.Rows(0)("Observaciones"))

        Me.CenterToScreen()
        Me.CenterToParent()
        Me.AutoSize = False
    End Sub
    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            Dim qry As String = "EXECUTE [dbo].[Upd_Conciliacion] @CRI_CodigoOLD,@TiendaOLD,@ConsecutivoOLD" &
  ",@FechaOLD,@ReferenciaOLD,@CRI_Codigo,@Estacion,@Tienda,@Consecutivo," &
  "@Fecha,@Hora,@Referencia,@Importe,@Conciliado,@Observaciones,@Folio"
            lop.Clear()
            lop.Add(sqls.CreadbParametros("@CRI_CodigoOLD", hdnCriCodigo.Text))
            lop.Add(sqls.CreadbParametros("@TiendaOLD", hdnTienda.Text))
            lop.Add(sqls.CreadbParametros("@ConsecutivoOLD", hdnConsecutivo.Text))
            lop.Add(sqls.CreadbParametros("@FechaOLD", hdnFecha.Text))
            lop.Add(sqls.CreadbParametros("@ReferenciaOLD", hdnReferencia.Text))
            lop.Add(sqls.CreadbParametros("@CRI_Codigo", ddlCriCodigo.SelectedValue))
            lop.Add(sqls.CreadbParametros("@Estacion", txtEstacion.Value))
            lop.Add(sqls.CreadbParametros("@Tienda", txtTienda.Value))
            lop.Add(sqls.CreadbParametros("@Consecutivo", txtConsecutivo.Value))
            lop.Add(sqls.CreadbParametros("@Fecha", dtpFecha.Value.ToString("yyyy-MM-dd HH:mm:ss")))
            lop.Add(sqls.CreadbParametros("@Hora", txtHora.Text))
            'lop.Add(sqls.CreadbParametros("@Art_Codigo", txt))
            lop.Add(sqls.CreadbParametros("@Referencia", txtReferencia.Text))
            lop.Add(sqls.CreadbParametros("@Importe", txtImporte.Value))
            lop.Add(sqls.CreadbParametros("@Conciliado", chkConciliado.Checked))
            lop.Add(sqls.CreadbParametros("@Observaciones", txtObservaciones.Text))
            lop.Add(sqls.CreadbParametros("@Folio", txtFolio.Text))
            sqls.ComandoSQL(cnx, qry, lop)

            exito = True
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1)
        End Try

    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class