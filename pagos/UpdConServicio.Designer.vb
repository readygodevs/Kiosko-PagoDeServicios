<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdConServicio
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ddlCriCodigo = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtEstacion = New System.Windows.Forms.NumericUpDown()
        Me.txtTienda = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtConsecutivo = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtHora = New System.Windows.Forms.TextBox()
        Me.txtReferencia = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtImporte = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkConciliado = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtFolio = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.hdnCriCodigo = New System.Windows.Forms.TextBox()
        Me.hdnTienda = New System.Windows.Forms.TextBox()
        Me.hdnReferencia = New System.Windows.Forms.TextBox()
        Me.hdnConsecutivo = New System.Windows.Forms.TextBox()
        Me.hdnFecha = New System.Windows.Forms.TextBox()
        CType(Me.txtEstacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTienda, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtConsecutivo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtImporte, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Servicio"
        '
        'ddlCriCodigo
        '
        Me.ddlCriCodigo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlCriCodigo.FormattingEnabled = True
        Me.ddlCriCodigo.Location = New System.Drawing.Point(86, 13)
        Me.ddlCriCodigo.Name = "ddlCriCodigo"
        Me.ddlCriCodigo.Size = New System.Drawing.Size(348, 21)
        Me.ddlCriCodigo.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(212, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Estación"
        '
        'txtEstacion
        '
        Me.txtEstacion.Location = New System.Drawing.Point(288, 46)
        Me.txtEstacion.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.txtEstacion.Name = "txtEstacion"
        Me.txtEstacion.Size = New System.Drawing.Size(111, 20)
        Me.txtEstacion.TabIndex = 3
        Me.txtEstacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTienda
        '
        Me.txtTienda.Location = New System.Drawing.Point(86, 46)
        Me.txtTienda.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.txtTienda.Name = "txtTienda"
        Me.txtTienda.Size = New System.Drawing.Size(111, 20)
        Me.txtTienda.TabIndex = 5
        Me.txtTienda.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Tienda"
        '
        'txtConsecutivo
        '
        Me.txtConsecutivo.Location = New System.Drawing.Point(86, 82)
        Me.txtConsecutivo.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.txtConsecutivo.Name = "txtConsecutivo"
        Me.txtConsecutivo.Size = New System.Drawing.Size(111, 20)
        Me.txtConsecutivo.TabIndex = 7
        Me.txtConsecutivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Consecutivo"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(215, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Fecha"
        '
        'dtpFecha
        '
        Me.dtpFecha.CustomFormat = "yyyy/MM/dd HH:mm:ss"
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFecha.Location = New System.Drawing.Point(288, 84)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(146, 20)
        Me.dtpFecha.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 117)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Hora"
        '
        'txtHora
        '
        Me.txtHora.Location = New System.Drawing.Point(86, 117)
        Me.txtHora.MaxLength = 10
        Me.txtHora.Name = "txtHora"
        Me.txtHora.Size = New System.Drawing.Size(111, 20)
        Me.txtHora.TabIndex = 11
        '
        'txtReferencia
        '
        Me.txtReferencia.Location = New System.Drawing.Point(288, 117)
        Me.txtReferencia.MaxLength = 256
        Me.txtReferencia.Name = "txtReferencia"
        Me.txtReferencia.Size = New System.Drawing.Size(146, 20)
        Me.txtReferencia.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(215, 117)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Referencia"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 151)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(42, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Importe"
        '
        'txtImporte
        '
        Me.txtImporte.DecimalPlaces = 2
        Me.txtImporte.Location = New System.Drawing.Point(86, 149)
        Me.txtImporte.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(111, 20)
        Me.txtImporte.TabIndex = 15
        Me.txtImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(215, 151)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Conciliado"
        '
        'chkConciliado
        '
        Me.chkConciliado.AutoSize = True
        Me.chkConciliado.Location = New System.Drawing.Point(288, 150)
        Me.chkConciliado.Name = "chkConciliado"
        Me.chkConciliado.Size = New System.Drawing.Size(15, 14)
        Me.chkConciliado.TabIndex = 17
        Me.chkConciliado.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(13, 189)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(29, 13)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Folio"
        '
        'txtFolio
        '
        Me.txtFolio.Location = New System.Drawing.Point(86, 186)
        Me.txtFolio.MaxLength = 20
        Me.txtFolio.Name = "txtFolio"
        Me.txtFolio.Size = New System.Drawing.Size(111, 20)
        Me.txtFolio.TabIndex = 19
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(14, 222)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 13)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Obs."
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(86, 219)
        Me.txtObservaciones.MaxLength = 256
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(348, 53)
        Me.txtObservaciones.TabIndex = 21
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(359, 291)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 22
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(274, 291)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 23
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'hdnCriCodigo
        '
        Me.hdnCriCodigo.Location = New System.Drawing.Point(13, 278)
        Me.hdnCriCodigo.MaxLength = 10
        Me.hdnCriCodigo.Name = "hdnCriCodigo"
        Me.hdnCriCodigo.Size = New System.Drawing.Size(45, 20)
        Me.hdnCriCodigo.TabIndex = 24
        Me.hdnCriCodigo.Visible = False
        '
        'hdnTienda
        '
        Me.hdnTienda.Location = New System.Drawing.Point(74, 278)
        Me.hdnTienda.MaxLength = 10
        Me.hdnTienda.Name = "hdnTienda"
        Me.hdnTienda.Size = New System.Drawing.Size(45, 20)
        Me.hdnTienda.TabIndex = 25
        Me.hdnTienda.Visible = False
        '
        'hdnReferencia
        '
        Me.hdnReferencia.Location = New System.Drawing.Point(134, 278)
        Me.hdnReferencia.MaxLength = 10
        Me.hdnReferencia.Name = "hdnReferencia"
        Me.hdnReferencia.Size = New System.Drawing.Size(45, 20)
        Me.hdnReferencia.TabIndex = 26
        Me.hdnReferencia.Visible = False
        '
        'hdnConsecutivo
        '
        Me.hdnConsecutivo.Location = New System.Drawing.Point(195, 278)
        Me.hdnConsecutivo.MaxLength = 10
        Me.hdnConsecutivo.Name = "hdnConsecutivo"
        Me.hdnConsecutivo.Size = New System.Drawing.Size(45, 20)
        Me.hdnConsecutivo.TabIndex = 27
        Me.hdnConsecutivo.Visible = False
        '
        'hdnFecha
        '
        Me.hdnFecha.Location = New System.Drawing.Point(195, 304)
        Me.hdnFecha.MaxLength = 10
        Me.hdnFecha.Name = "hdnFecha"
        Me.hdnFecha.Size = New System.Drawing.Size(45, 20)
        Me.hdnFecha.TabIndex = 28
        Me.hdnFecha.Visible = False
        '
        'UpdConServicio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 331)
        Me.Controls.Add(Me.hdnFecha)
        Me.Controls.Add(Me.hdnConsecutivo)
        Me.Controls.Add(Me.hdnReferencia)
        Me.Controls.Add(Me.hdnTienda)
        Me.Controls.Add(Me.hdnCriCodigo)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.txtObservaciones)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtFolio)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.chkConciliado)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtImporte)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtReferencia)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtHora)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtConsecutivo)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtTienda)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtEstacion)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ddlCriCodigo)
        Me.Controls.Add(Me.Label1)
        Me.Name = "UpdConServicio"
        Me.Text = "Modificar Pago Servicio"
        CType(Me.txtEstacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTienda, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtConsecutivo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtImporte, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ddlCriCodigo As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtEstacion As NumericUpDown
    Friend WithEvents txtTienda As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents txtConsecutivo As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents txtHora As TextBox
    Friend WithEvents txtReferencia As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtImporte As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents chkConciliado As CheckBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtFolio As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents btnAceptar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents hdnCriCodigo As TextBox
    Friend WithEvents hdnTienda As TextBox
    Friend WithEvents hdnReferencia As TextBox
    Friend WithEvents hdnConsecutivo As TextBox
    Friend WithEvents hdnFecha As TextBox
End Class
