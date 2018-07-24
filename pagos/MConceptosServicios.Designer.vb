<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MConceptosServicios
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblIdServicio = New System.Windows.Forms.Label()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.txtIdServ = New System.Windows.Forms.TextBox()
        Me.txtDescr = New System.Windows.Forms.TextBox()
        Me.txtTipo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCRI_Cod = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Estatus = New System.Windows.Forms.Label()
        Me.chkEstatus = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbDiasPago = New System.Windows.Forms.ComboBox()
        Me.cmbDiasCobro = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtArchivoFTP = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtDesde = New Telerik.WinControls.UI.RadSpinEditor()
        Me.txtHasta = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.txtEmailProv = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtEmailKiosko = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtCtaContable = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.chkSumaC = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkRestaC = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtProveedor = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.grbPeriodoSemanal = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ddlA = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.ddlDe = New System.Windows.Forms.ComboBox()
        Me.chkPeriodoSemanal = New System.Windows.Forms.CheckBox()
        Me.grbDiasCredito = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtDiasCredito = New Telerik.WinControls.UI.RadSpinEditor()
        Me.chkDiasCredito = New System.Windows.Forms.CheckBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtCRIAgrupador = New System.Windows.Forms.TextBox()
        CType(Me.txtDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbPeriodoSemanal.SuspendLayout()
        Me.grbDiasCredito.SuspendLayout()
        CType(Me.txtDiasCredito, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblIdServicio
        '
        Me.lblIdServicio.AutoSize = True
        Me.lblIdServicio.Location = New System.Drawing.Point(9, 22)
        Me.lblIdServicio.Name = "lblIdServicio"
        Me.lblIdServicio.Size = New System.Drawing.Size(57, 13)
        Me.lblIdServicio.TabIndex = 0
        Me.lblIdServicio.Text = "Id Servicio"
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(6, 99)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcion.TabIndex = 1
        Me.lblDescripcion.Text = "Descripción"
        '
        'txtIdServ
        '
        Me.txtIdServ.Enabled = False
        Me.txtIdServ.Location = New System.Drawing.Point(101, 19)
        Me.txtIdServ.Name = "txtIdServ"
        Me.txtIdServ.Size = New System.Drawing.Size(100, 20)
        Me.txtIdServ.TabIndex = 0
        '
        'txtDescr
        '
        Me.txtDescr.Enabled = False
        Me.txtDescr.Location = New System.Drawing.Point(101, 99)
        Me.txtDescr.Multiline = True
        Me.txtDescr.Name = "txtDescr"
        Me.txtDescr.Size = New System.Drawing.Size(251, 56)
        Me.txtDescr.TabIndex = 0
        '
        'txtTipo
        '
        Me.txtTipo.Enabled = False
        Me.txtTipo.Location = New System.Drawing.Point(239, 19)
        Me.txtTipo.Name = "txtTipo"
        Me.txtTipo.Size = New System.Drawing.Size(113, 20)
        Me.txtTipo.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(205, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Tipo"
        '
        'txtCRI_Cod
        '
        Me.txtCRI_Cod.Enabled = False
        Me.txtCRI_Cod.Location = New System.Drawing.Point(456, 19)
        Me.txtCRI_Cod.Name = "txtCRI_Cod"
        Me.txtCRI_Cod.Size = New System.Drawing.Size(113, 20)
        Me.txtCRI_Cod.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(386, 22)
        Me.Label2.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Código Dyn."
        '
        'Estatus
        '
        Me.Estatus.AutoSize = True
        Me.Estatus.Location = New System.Drawing.Point(613, 13)
        Me.Estatus.Name = "Estatus"
        Me.Estatus.Size = New System.Drawing.Size(42, 13)
        Me.Estatus.TabIndex = 8
        Me.Estatus.Text = "Estatus"
        '
        'chkEstatus
        '
        Me.chkEstatus.AutoSize = True
        Me.chkEstatus.Location = New System.Drawing.Point(675, 13)
        Me.chkEstatus.Name = "chkEstatus"
        Me.chkEstatus.Size = New System.Drawing.Size(15, 14)
        Me.chkEstatus.TabIndex = 1
        Me.chkEstatus.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(378, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Días de Pago"
        '
        'cmbDiasPago
        '
        Me.cmbDiasPago.FormattingEnabled = True
        Me.cmbDiasPago.Location = New System.Drawing.Point(457, 97)
        Me.cmbDiasPago.Name = "cmbDiasPago"
        Me.cmbDiasPago.Size = New System.Drawing.Size(267, 21)
        Me.cmbDiasPago.TabIndex = 4
        '
        'cmbDiasCobro
        '
        Me.cmbDiasCobro.FormattingEnabled = True
        Me.cmbDiasCobro.Location = New System.Drawing.Point(457, 134)
        Me.cmbDiasCobro.Name = "cmbDiasCobro"
        Me.cmbDiasCobro.Size = New System.Drawing.Size(266, 21)
        Me.cmbDiasCobro.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(375, 134)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Días de Cobro"
        '
        'txtArchivoFTP
        '
        Me.txtArchivoFTP.Location = New System.Drawing.Point(101, 172)
        Me.txtArchivoFTP.MaxLength = 255
        Me.txtArchivoFTP.Name = "txtArchivoFTP"
        Me.txtArchivoFTP.Size = New System.Drawing.Size(251, 20)
        Me.txtArchivoFTP.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Descripción Doc."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 251)
        Me.Label6.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Rango Desde"
        '
        'txtDesde
        '
        Me.txtDesde.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtDesde.DecimalPlaces = 2
        Me.txtDesde.Location = New System.Drawing.Point(101, 251)
        Me.txtDesde.Name = "txtDesde"
        '
        '
        '
        Me.txtDesde.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.txtDesde.RootElement.ControlBounds = New System.Drawing.Rectangle(101, 214, 100, 20)
        Me.txtDesde.RootElement.StretchVertically = True
        Me.txtDesde.ShowBorder = True
        Me.txtDesde.ShowUpDownButtons = False
        Me.txtDesde.Size = New System.Drawing.Size(133, 20)
        Me.txtDesde.TabIndex = 9
        Me.txtDesde.TabStop = False
        Me.txtDesde.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHasta
        '
        Me.txtHasta.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtHasta.DecimalPlaces = 2
        Me.txtHasta.Location = New System.Drawing.Point(457, 214)
        Me.txtHasta.Name = "txtHasta"
        '
        '
        '
        Me.txtHasta.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.txtHasta.RootElement.ControlBounds = New System.Drawing.Rectangle(457, 214, 100, 20)
        Me.txtHasta.RootElement.StretchVertically = True
        Me.txtHasta.ShowBorder = True
        Me.txtHasta.ShowUpDownButtons = False
        Me.txtHasta.Size = New System.Drawing.Size(134, 20)
        Me.txtHasta.TabIndex = 10
        Me.txtHasta.TabStop = False
        Me.txtHasta.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(375, 214)
        Me.Label7.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(70, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Rango Hasta"
        '
        'btnAceptar
        '
        Me.btnAceptar.Image = Global.pagos.My.Resources.Resources._109_AllAnnotations_Default_16x16_72
        Me.btnAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAceptar.Location = New System.Drawing.Point(632, 331)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(107, 40)
        Me.btnAceptar.TabIndex = 12
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Image = Global.pagos.My.Resources.Resources._109_AllAnnotations_Error_16x16_72
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(498, 331)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(107, 40)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtEmailProv
        '
        Me.txtEmailProv.Location = New System.Drawing.Point(101, 213)
        Me.txtEmailProv.MaxLength = 255
        Me.txtEmailProv.Name = "txtEmailProv"
        Me.txtEmailProv.Size = New System.Drawing.Size(251, 20)
        Me.txtEmailProv.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 212)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Email Prov."
        '
        'txtEmailKiosko
        '
        Me.txtEmailKiosko.Location = New System.Drawing.Point(457, 179)
        Me.txtEmailKiosko.MaxLength = 255
        Me.txtEmailKiosko.Name = "txtEmailKiosko"
        Me.txtEmailKiosko.Size = New System.Drawing.Size(261, 20)
        Me.txtEmailKiosko.TabIndex = 8
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(391, 176)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(67, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Email Kiosko"
        '
        'txtCtaContable
        '
        Me.txtCtaContable.Enabled = False
        Me.txtCtaContable.Location = New System.Drawing.Point(456, 59)
        Me.txtCtaContable.Name = "txtCtaContable"
        Me.txtCtaContable.Size = New System.Drawing.Size(113, 20)
        Me.txtCtaContable.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(386, 62)
        Me.Label10.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 13)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "Cta. Contable"
        '
        'chkSumaC
        '
        Me.chkSumaC.AutoSize = True
        Me.chkSumaC.Location = New System.Drawing.Point(675, 40)
        Me.chkSumaC.Name = "chkSumaC"
        Me.chkSumaC.Size = New System.Drawing.Size(15, 14)
        Me.chkSumaC.TabIndex = 2
        Me.chkSumaC.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(590, 40)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(65, 13)
        Me.Label11.TabIndex = 25
        Me.Label11.Text = "Suma Cargo"
        '
        'chkRestaC
        '
        Me.chkRestaC.AutoSize = True
        Me.chkRestaC.Location = New System.Drawing.Point(675, 66)
        Me.chkRestaC.Name = "chkRestaC"
        Me.chkRestaC.Size = New System.Drawing.Size(15, 14)
        Me.chkRestaC.TabIndex = 3
        Me.chkRestaC.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(590, 66)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(66, 13)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "Resta Contr."
        '
        'txtProveedor
        '
        Me.txtProveedor.Enabled = False
        Me.txtProveedor.Location = New System.Drawing.Point(456, 248)
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(113, 20)
        Me.txtProveedor.TabIndex = 28
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(375, 248)
        Me.Label13.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 13)
        Me.Label13.TabIndex = 29
        Me.Label13.Text = "Proveedor"
        '
        'grbPeriodoSemanal
        '
        Me.grbPeriodoSemanal.Controls.Add(Me.Label15)
        Me.grbPeriodoSemanal.Controls.Add(Me.ddlA)
        Me.grbPeriodoSemanal.Controls.Add(Me.Label14)
        Me.grbPeriodoSemanal.Controls.Add(Me.ddlDe)
        Me.grbPeriodoSemanal.Enabled = False
        Me.grbPeriodoSemanal.Location = New System.Drawing.Point(9, 295)
        Me.grbPeriodoSemanal.Name = "grbPeriodoSemanal"
        Me.grbPeriodoSemanal.Size = New System.Drawing.Size(175, 76)
        Me.grbPeriodoSemanal.TabIndex = 30
        Me.grbPeriodoSemanal.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 47)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(14, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "A"
        '
        'ddlA
        '
        Me.ddlA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlA.FormattingEnabled = True
        Me.ddlA.Location = New System.Drawing.Point(39, 44)
        Me.ddlA.Name = "ddlA"
        Me.ddlA.Size = New System.Drawing.Size(121, 21)
        Me.ddlA.TabIndex = 3
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(12, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(21, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "De"
        '
        'ddlDe
        '
        Me.ddlDe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlDe.FormattingEnabled = True
        Me.ddlDe.Location = New System.Drawing.Point(39, 12)
        Me.ddlDe.Name = "ddlDe"
        Me.ddlDe.Size = New System.Drawing.Size(121, 21)
        Me.ddlDe.TabIndex = 1
        '
        'chkPeriodoSemanal
        '
        Me.chkPeriodoSemanal.AutoSize = True
        Me.chkPeriodoSemanal.Location = New System.Drawing.Point(9, 277)
        Me.chkPeriodoSemanal.Name = "chkPeriodoSemanal"
        Me.chkPeriodoSemanal.Size = New System.Drawing.Size(106, 17)
        Me.chkPeriodoSemanal.TabIndex = 0
        Me.chkPeriodoSemanal.Text = "Periodo Semanal"
        Me.chkPeriodoSemanal.UseVisualStyleBackColor = True
        '
        'grbDiasCredito
        '
        Me.grbDiasCredito.Controls.Add(Me.Label16)
        Me.grbDiasCredito.Controls.Add(Me.txtDiasCredito)
        Me.grbDiasCredito.Enabled = False
        Me.grbDiasCredito.Location = New System.Drawing.Point(193, 295)
        Me.grbDiasCredito.Name = "grbDiasCredito"
        Me.grbDiasCredito.Size = New System.Drawing.Size(143, 76)
        Me.grbDiasCredito.TabIndex = 33
        Me.grbDiasCredito.TabStop = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 20)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(30, 13)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "Días"
        '
        'txtDiasCredito
        '
        Me.txtDiasCredito.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtDiasCredito.Location = New System.Drawing.Point(72, 13)
        Me.txtDiasCredito.Name = "txtDiasCredito"
        '
        '
        '
        Me.txtDiasCredito.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.txtDiasCredito.RootElement.ControlBounds = New System.Drawing.Rectangle(72, 13, 100, 20)
        Me.txtDiasCredito.RootElement.StretchVertically = True
        Me.txtDiasCredito.ShowBorder = True
        Me.txtDiasCredito.ShowUpDownButtons = False
        Me.txtDiasCredito.Size = New System.Drawing.Size(65, 20)
        Me.txtDiasCredito.TabIndex = 34
        Me.txtDiasCredito.TabStop = False
        Me.txtDiasCredito.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkDiasCredito
        '
        Me.chkDiasCredito.AutoSize = True
        Me.chkDiasCredito.Location = New System.Drawing.Point(193, 277)
        Me.chkDiasCredito.Name = "chkDiasCredito"
        Me.chkDiasCredito.Size = New System.Drawing.Size(85, 17)
        Me.chkDiasCredito.TabIndex = 35
        Me.chkDiasCredito.Text = "Días Crédito"
        Me.chkDiasCredito.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(10, 59)
        Me.Label17.MaximumSize = New System.Drawing.Size(0, 100)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(77, 13)
        Me.Label17.TabIndex = 37
        Me.Label17.Text = "CRI Agrupador"
        '
        'txtCRIAgrupador
        '
        Me.txtCRIAgrupador.Location = New System.Drawing.Point(101, 52)
        Me.txtCRIAgrupador.Name = "txtCRIAgrupador"
        Me.txtCRIAgrupador.Size = New System.Drawing.Size(100, 20)
        Me.txtCRIAgrupador.TabIndex = 36
        '
        'MConceptosServicios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 387)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.txtCRIAgrupador)
        Me.Controls.Add(Me.chkDiasCredito)
        Me.Controls.Add(Me.grbDiasCredito)
        Me.Controls.Add(Me.grbPeriodoSemanal)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtProveedor)
        Me.Controls.Add(Me.chkRestaC)
        Me.Controls.Add(Me.chkPeriodoSemanal)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.chkSumaC)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtCtaContable)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtEmailKiosko)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtEmailProv)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.txtHasta)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDesde)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtArchivoFTP)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbDiasCobro)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbDiasPago)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkEstatus)
        Me.Controls.Add(Me.Estatus)
        Me.Controls.Add(Me.txtCRI_Cod)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTipo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDescr)
        Me.Controls.Add(Me.txtIdServ)
        Me.Controls.Add(Me.lblDescripcion)
        Me.Controls.Add(Me.lblIdServicio)
        Me.Name = "MConceptosServicios"
        Me.Text = "MConceptosServicios"
        CType(Me.txtDesde, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHasta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbPeriodoSemanal.ResumeLayout(False)
        Me.grbPeriodoSemanal.PerformLayout()
        Me.grbDiasCredito.ResumeLayout(False)
        Me.grbDiasCredito.PerformLayout()
        CType(Me.txtDiasCredito, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblIdServicio As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents txtIdServ As System.Windows.Forms.TextBox
    Friend WithEvents txtDescr As System.Windows.Forms.TextBox
    Friend WithEvents txtTipo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCRI_Cod As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Estatus As System.Windows.Forms.Label
    Friend WithEvents chkEstatus As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbDiasPago As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDiasCobro As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtArchivoFTP As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents txtEmailProv As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtEmailKiosko As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtCtaContable As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents chkSumaC As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkRestaC As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtProveedor As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grbPeriodoSemanal As GroupBox
    Friend WithEvents Label15 As Label
    Friend WithEvents ddlA As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents ddlDe As ComboBox
    Friend WithEvents chkPeriodoSemanal As CheckBox
    Private WithEvents txtDesde As Telerik.WinControls.UI.RadSpinEditor
    Private WithEvents txtHasta As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents grbDiasCredito As GroupBox
    Friend WithEvents Label16 As Label
    Private WithEvents txtDiasCredito As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents chkDiasCredito As CheckBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtCRIAgrupador As TextBox
End Class
