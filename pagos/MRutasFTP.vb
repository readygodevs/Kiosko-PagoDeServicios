Public Class MRutasFTP
    Public sqls As FuncSQL.FuncSQL = New FuncSQL.FuncSQL(, String.Format("c:\logs\Log{0:yyyyMMdd}.txt", Today), , 1000, True, "192.168.100.8", , "rtorres@mikiosko.mx")
    Public cnx As SqlClient.SqlConnection = New SqlClient.SqlConnection("Password=sqlsolomon;Persist Security Info=True;User ID=sa;Initial Catalog=PagoDeServicios;Data Source=192.168.100.6 ;connection timeout=0")
    Public qry As String
    Public dt As DataTable
    Public lop As New List(Of Common.DbParameter)
    Public Refrescar As Boolean
    Public ModoMtn As Integer
    Public IdRuta As Integer
    

    Sub New(ByVal modo As Integer, Optional descripcion As String = "", Optional rutaO As String = "", Optional rutaD As String = "", Optional id As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()
        Refrescar = False
        ' Add any initialization after the InitializeComponent() call.
        txtDescripcion.Text = descripcion
        txtRutaOr.Text = rutaO
        txtRutaDes.Text = rutaD
        ModoMtn = modo
        IdRuta = id
    End Sub

    Private Sub MConceptosServicios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Me.Text = IIf(ModoMtn = 1, "Crear", "Modificar") & " Ruta FTP"
        Me.CenterToScreen()
        Me.CenterToParent()
        Me.AutoSize = False
        txtDescripcion.Focus()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            If String.IsNullOrEmpty(txtDescripcion.Text.ToString) Then
                Throw New Exception("El campo <Descripcion> es requerido.")
            End If
            If String.IsNullOrEmpty(txtRutaOr.Text.ToString) Then
                Throw New Exception("El campo <RutaOrigen> es requerido.")
            End If

            lop.Clear()
            lop.Add(sqls.CreadbParametros("@Descr", txtDescripcion.Text))
            lop.Add(sqls.CreadbParametros("@RutaO", txtRutaOr.Text))
            lop.Add(sqls.CreadbParametros("@RutaD", txtRutaDes.Text))
            If ModoMtn = 2 Then
                qry = "Update T_RutasFTP set Descripcion=@Descr" & _
               ",RutaOrigen=@RutaO,RutaDestino=@RutaD " & _
               " where IdRuta=" & IdRuta
            ElseIf ModoMtn = 1 Then
                qry = "insert into T_RutasFTP (Descripcion," & _
                   "RutaOrigen,RutaDestino) values " & _
                   "(@Descr,@RutaO,@RutaD)"
            End If
           
            sqls.ComandoSQL(cnx, qry, lop, , FuncSQL.FuncSQL.EnumTipoBase._SQL, , )

            Refrescar = True
            cnx.Close()
            btnCancelar_Click(Nothing, Nothing)
        Catch ex As Exception
            MessageBox.Show("Error al guardar el Concepto de Servicio: " & ex.Message)
        End Try

    End Sub
End Class