<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dtfechafin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dtfechafin))
        Me.label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CbxServicio = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.DtpFechaini = New System.Windows.Forms.DateTimePicker()
        Me.DtpFechafin = New System.Windows.Forms.DateTimePicker()
        Me.dgservicio = New System.Windows.Forms.DataGridView()
        Me.sumaimporte = New System.Windows.Forms.TextBox()
        Me.Garchivo = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.PagoAut = New System.Windows.Forms.Button()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.dgpago = New System.Windows.Forms.DataGridView()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btnConciliarTodo = New System.Windows.Forms.Button()
        Me.Button23 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.dtnoconciliados3 = New System.Windows.Forms.DateTimePicker()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dtnoconciliados2 = New System.Windows.Forms.DateTimePicker()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.mcambio = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tdgnoconciliados = New C1.Win.C1TrueDBGrid.C1TrueDBGrid()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.dtgtiendasf = New C1.Win.C1TrueDBGrid.C1TrueDBGrid()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtCorreo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPass = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        CType(Me.dgservicio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgpago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.tdgnoconciliados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        CType(Me.dtgtiendasf, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(46, 12)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(67, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Fecha Inicial"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(232, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Fecha Final"
        '
        'CbxServicio
        '
        Me.CbxServicio.FormattingEnabled = True
        Me.CbxServicio.Location = New System.Drawing.Point(351, 34)
        Me.CbxServicio.Name = "CbxServicio"
        Me.CbxServicio.Size = New System.Drawing.Size(217, 21)
        Me.CbxServicio.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(394, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Servicio a Pagar"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(895, 441)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(108, 38)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "salir"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'DtpFechaini
        '
        Me.DtpFechaini.CustomFormat = ""
        Me.DtpFechaini.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DtpFechaini.Location = New System.Drawing.Point(42, 33)
        Me.DtpFechaini.Name = "DtpFechaini"
        Me.DtpFechaini.Size = New System.Drawing.Size(96, 20)
        Me.DtpFechaini.TabIndex = 9
        Me.DtpFechaini.Value = New Date(2012, 3, 1, 0, 0, 0, 0)
        '
        'DtpFechafin
        '
        Me.DtpFechafin.CustomFormat = ""
        Me.DtpFechafin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DtpFechafin.Location = New System.Drawing.Point(220, 33)
        Me.DtpFechafin.Name = "DtpFechafin"
        Me.DtpFechafin.Size = New System.Drawing.Size(97, 20)
        Me.DtpFechafin.TabIndex = 10
        '
        'dgservicio
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgservicio.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgservicio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgservicio.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgservicio.Location = New System.Drawing.Point(3, 73)
        Me.dgservicio.Name = "dgservicio"
        Me.dgservicio.ReadOnly = True
        Me.dgservicio.RowHeadersVisible = False
        Me.dgservicio.Size = New System.Drawing.Size(875, 316)
        Me.dgservicio.TabIndex = 11
        '
        'sumaimporte
        '
        Me.sumaimporte.Location = New System.Drawing.Point(6, 19)
        Me.sumaimporte.Name = "sumaimporte"
        Me.sumaimporte.ReadOnly = True
        Me.sumaimporte.Size = New System.Drawing.Size(100, 20)
        Me.sumaimporte.TabIndex = 13
        '
        'Garchivo
        '
        Me.Garchivo.Location = New System.Drawing.Point(6, 62)
        Me.Garchivo.Name = "Garchivo"
        Me.Garchivo.Size = New System.Drawing.Size(100, 23)
        Me.Garchivo.TabIndex = 14
        Me.Garchivo.Text = "Genera Archivo"
        Me.Garchivo.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Location = New System.Drawing.Point(3, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1004, 423)
        Me.TabControl1.TabIndex = 17
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button6)
        Me.TabPage1.Controls.Add(Me.PagoAut)
        Me.TabPage1.Controls.Add(Me.Button16)
        Me.TabPage1.Controls.Add(Me.Button14)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.Button5)
        Me.TabPage1.Controls.Add(Me.Button4)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Controls.Add(Me.dgpago)
        Me.TabPage1.Controls.Add(Me.label1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.CbxServicio)
        Me.TabPage1.Controls.Add(Me.dgservicio)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.DtpFechaini)
        Me.TabPage1.Controls.Add(Me.DtpFechafin)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(996, 397)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Servicios por Pagar"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(884, 132)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(106, 25)
        Me.Button6.TabIndex = 25
        Me.Button6.Text = "Seleccionar Pagos"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'PagoAut
        '
        Me.PagoAut.Location = New System.Drawing.Point(884, 198)
        Me.PagoAut.Name = "PagoAut"
        Me.PagoAut.Size = New System.Drawing.Size(106, 36)
        Me.PagoAut.TabIndex = 24
        Me.PagoAut.Text = "Generar Pagos Automáticos"
        Me.PagoAut.UseVisualStyleBackColor = True
        '
        'Button16
        '
        Me.Button16.Location = New System.Drawing.Point(884, 252)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(106, 36)
        Me.Button16.TabIndex = 23
        Me.Button16.Text = "Archivo Concentrado"
        Me.Button16.UseVisualStyleBackColor = True
        Me.Button16.Visible = False
        '
        'Button14
        '
        Me.Button14.Location = New System.Drawing.Point(885, 305)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(106, 35)
        Me.Button14.TabIndex = 22
        Me.Button14.Text = "Servicios Faltantes de Pagar"
        Me.Button14.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Garchivo)
        Me.GroupBox1.Controls.Add(Me.sumaimporte)
        Me.GroupBox1.Location = New System.Drawing.Point(884, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(112, 90)
        Me.GroupBox1.TabIndex = 21
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Suma Importe"
        Me.GroupBox1.Visible = False
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(884, 355)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(106, 25)
        Me.Button5.TabIndex = 20
        Me.Button5.Text = "copiar seleccion"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(533, 15)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(35, 11)
        Me.Button4.TabIndex = 19
        Me.Button4.Text = "Button4"
        Me.ToolTip1.SetToolTip(Me.Button4, "todos los servicios")
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(156, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(45, 13)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Dia I. P."
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(157, 31)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(41, 20)
        Me.TextBox1.TabIndex = 17
        '
        'dgpago
        '
        Me.dgpago.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgpago.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgpago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgpago.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgpago.Location = New System.Drawing.Point(596, 19)
        Me.dgpago.Name = "dgpago"
        Me.dgpago.ReadOnly = True
        Me.dgpago.Size = New System.Drawing.Size(251, 48)
        Me.dgpago.TabIndex = 16
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.btnConciliarTodo)
        Me.TabPage3.Controls.Add(Me.Button23)
        Me.TabPage3.Controls.Add(Me.Button15)
        Me.TabPage3.Controls.Add(Me.Button13)
        Me.TabPage3.Controls.Add(Me.GroupBox2)
        Me.TabPage3.Controls.Add(Me.Button8)
        Me.TabPage3.Controls.Add(Me.Button7)
        Me.TabPage3.Controls.Add(Me.mcambio)
        Me.TabPage3.Controls.Add(Me.Label12)
        Me.TabPage3.Controls.Add(Me.Button1)
        Me.TabPage3.Controls.Add(Me.tdgnoconciliados)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(996, 397)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Conciliar Pagos"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnConciliarTodo
        '
        Me.btnConciliarTodo.Location = New System.Drawing.Point(918, 166)
        Me.btnConciliarTodo.Name = "btnConciliarTodo"
        Me.btnConciliarTodo.Size = New System.Drawing.Size(76, 39)
        Me.btnConciliarTodo.TabIndex = 46
        Me.btnConciliarTodo.Text = "Conciliar Todo"
        Me.btnConciliarTodo.UseVisualStyleBackColor = True
        '
        'Button23
        '
        Me.Button23.Location = New System.Drawing.Point(921, 348)
        Me.Button23.Name = "Button23"
        Me.Button23.Size = New System.Drawing.Size(72, 46)
        Me.Button23.TabIndex = 45
        Me.Button23.Text = "Conciliar Transcol"
        Me.Button23.UseVisualStyleBackColor = True
        '
        'Button15
        '
        Me.Button15.Location = New System.Drawing.Point(919, 307)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(74, 39)
        Me.Button15.TabIndex = 44
        Me.Button15.Text = "Conciliar autoG"
        Me.Button15.UseVisualStyleBackColor = True
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(382, 8)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(94, 42)
        Me.Button13.TabIndex = 43
        Me.Button13.Text = "Revisar No Conciliados Gral"
        Me.Button13.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.Button12)
        Me.GroupBox2.Controls.Add(Me.dtnoconciliados3)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.dtnoconciliados2)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(373, 50)
        Me.GroupBox2.TabIndex = 42
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Servicios"
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(273, 9)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(89, 37)
        Me.Button12.TabIndex = 41
        Me.Button12.Text = "Servicios Faltantes"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'dtnoconciliados3
        '
        Me.dtnoconciliados3.CustomFormat = ""
        Me.dtnoconciliados3.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtnoconciliados3.Location = New System.Drawing.Point(44, 25)
        Me.dtnoconciliados3.Name = "dtnoconciliados3"
        Me.dtnoconciliados3.Size = New System.Drawing.Size(97, 20)
        Me.dtnoconciliados3.TabIndex = 35
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(60, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 30
        Me.Label10.Text = "Fecha Inicial"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(180, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 13)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "Fecha Final"
        '
        'dtnoconciliados2
        '
        Me.dtnoconciliados2.CustomFormat = ""
        Me.dtnoconciliados2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtnoconciliados2.Location = New System.Drawing.Point(162, 25)
        Me.dtnoconciliados2.Name = "dtnoconciliados2"
        Me.dtnoconciliados2.Size = New System.Drawing.Size(97, 20)
        Me.dtnoconciliados2.TabIndex = 33
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(918, 267)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(76, 39)
        Me.Button8.TabIndex = 40
        Me.Button8.Text = "Conciliar"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(918, 69)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(77, 40)
        Me.Button7.TabIndex = 39
        Me.Button7.Text = "Copiar Seleccion"
        Me.Button7.UseVisualStyleBackColor = True
        Me.Button7.Visible = False
        '
        'mcambio
        '
        Me.mcambio.Location = New System.Drawing.Point(604, 27)
        Me.mcambio.Name = "mcambio"
        Me.mcambio.Size = New System.Drawing.Size(111, 20)
        Me.mcambio.TabIndex = 38
        Me.mcambio.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(610, 8)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(87, 13)
        Me.Label12.TabIndex = 37
        Me.Label12.Text = "Monto a Cambiar"
        Me.Label12.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(492, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 47)
        Me.Button1.TabIndex = 34
        Me.Button1.Text = "Consultar no Conciliados por monto"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tdgnoconciliados
        '
        Me.tdgnoconciliados.AllowUpdate = False
        Me.tdgnoconciliados.FilterBar = True
        Me.tdgnoconciliados.GroupByCaption = "Drag a column header here to group by that column"
        Me.tdgnoconciliados.Images.Add(CType(resources.GetObject("tdgnoconciliados.Images"), System.Drawing.Image))
        Me.tdgnoconciliados.Location = New System.Drawing.Point(3, 53)
        Me.tdgnoconciliados.Name = "tdgnoconciliados"
        Me.tdgnoconciliados.PreviewInfo.Location = New System.Drawing.Point(0, 0)
        Me.tdgnoconciliados.PreviewInfo.Size = New System.Drawing.Size(0, 0)
        Me.tdgnoconciliados.PreviewInfo.ZoomFactor = 75.0R
        Me.tdgnoconciliados.PrintInfo.PageSettings = CType(resources.GetObject("tdgnoconciliados.PrintInfo.PageSettings"), System.Drawing.Printing.PageSettings)
        Me.tdgnoconciliados.Size = New System.Drawing.Size(909, 341)
        Me.tdgnoconciliados.TabIndex = 0
        Me.tdgnoconciliados.Text = "C1TrueDBGrid1"
        Me.tdgnoconciliados.PropBag = resources.GetString("tdgnoconciliados.PropBag")
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Button11)
        Me.TabPage4.Controls.Add(Me.Label4)
        Me.TabPage4.Controls.Add(Me.TextBox3)
        Me.TabPage4.Controls.Add(Me.Button10)
        Me.TabPage4.Controls.Add(Me.Button9)
        Me.TabPage4.Controls.Add(Me.dtgtiendasf)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(996, 397)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Tiendas Faltantes"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(754, 157)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(98, 26)
        Me.Button11.TabIndex = 34
        Me.Button11.Text = "Copiar Seleccion"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(663, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Total"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(700, 18)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(49, 20)
        Me.TextBox3.TabIndex = 4
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(537, 7)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(103, 37)
        Me.Button10.TabIndex = 3
        Me.Button10.Text = "Tiendas Actualizadas"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(385, 6)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(107, 38)
        Me.Button9.TabIndex = 2
        Me.Button9.Text = "Tiendas Faltantes"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'dtgtiendasf
        '
        Me.dtgtiendasf.AllowUpdate = False
        Me.dtgtiendasf.FetchRowStyles = True
        Me.dtgtiendasf.FilterBar = True
        Me.dtgtiendasf.GroupByCaption = "Drag a column header here to group by that column"
        Me.dtgtiendasf.Images.Add(CType(resources.GetObject("dtgtiendasf.Images"), System.Drawing.Image))
        Me.dtgtiendasf.Location = New System.Drawing.Point(6, 50)
        Me.dtgtiendasf.Name = "dtgtiendasf"
        Me.dtgtiendasf.PreviewInfo.Location = New System.Drawing.Point(0, 0)
        Me.dtgtiendasf.PreviewInfo.Size = New System.Drawing.Size(0, 0)
        Me.dtgtiendasf.PreviewInfo.ZoomFactor = 75.0R
        Me.dtgtiendasf.PrintInfo.PageSettings = CType(resources.GetObject("dtgtiendasf.PrintInfo.PageSettings"), System.Drawing.Printing.PageSettings)
        Me.dtgtiendasf.Size = New System.Drawing.Size(745, 341)
        Me.dtgtiendasf.TabIndex = 1
        Me.dtgtiendasf.Text = "C1TrueDBGrid1"
        Me.dtgtiendasf.PropBag = resources.GetString("dtgtiendasf.PropBag")
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(141, 444)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(194, 20)
        Me.TextBox2.TabIndex = 19
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(38, 447)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(97, 13)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Fecha Ultimo Pago"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(264, 33)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(107, 34)
        Me.Button3.TabIndex = 21
        Me.Button3.Text = "Enviar Archivos"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'txtCorreo
        '
        Me.txtCorreo.Location = New System.Drawing.Point(71, 19)
        Me.txtCorreo.Name = "txtCorreo"
        Me.txtCorreo.Size = New System.Drawing.Size(151, 20)
        Me.txtCorreo.TabIndex = 22
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Correo"
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(71, 54)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPass.Size = New System.Drawing.Size(151, 20)
        Me.txtPass.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 54)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Password"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtPass)
        Me.GroupBox3.Controls.Add(Me.txtCorreo)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Button3)
        Me.GroupBox3.Location = New System.Drawing.Point(499, 438)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(386, 87)
        Me.GroupBox3.TabIndex = 26
        Me.GroupBox3.TabStop = False
        '
        'dtfechafin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(1019, 597)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "dtfechafin"
        Me.Text = "pagos"
        CType(Me.dgservicio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgpago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.tdgnoconciliados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        CType(Me.dtgtiendasf, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CbxServicio As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents DtpFechaini As System.Windows.Forms.DateTimePicker
    Friend WithEvents DtpFechafin As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgservicio As System.Windows.Forms.DataGridView
    Friend WithEvents sumaimporte As System.Windows.Forms.TextBox
    Friend WithEvents Garchivo As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents dtnoconciliados2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents tdgnoconciliados As C1.Win.C1TrueDBGrid.C1TrueDBGrid
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents dtnoconciliados3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents dgpago As System.Windows.Forms.DataGridView
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents mcambio As System.Windows.Forms.TextBox
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents dtgtiendasf As C1.Win.C1TrueDBGrid.C1TrueDBGrid
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents Button23 As System.Windows.Forms.Button
    Friend WithEvents PagoAut As System.Windows.Forms.Button
    Friend WithEvents btnConciliarTodo As System.Windows.Forms.Button
    Friend WithEvents Button6 As Button
    Friend WithEvents txtCorreo As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPass As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents GroupBox3 As GroupBox
End Class
