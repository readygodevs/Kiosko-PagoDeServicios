<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RptConServicios
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbServicio = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.chkPagados = New System.Windows.Forms.CheckBox()
        Me.chkConciliados = New System.Windows.Forms.CheckBox()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.crReporte = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbServicio)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnBuscar)
        Me.GroupBox1.Controls.Add(Me.chkPagados)
        Me.GroupBox1.Controls.Add(Me.chkConciliados)
        Me.GroupBox1.Controls.Add(Me.dtpHasta)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.dtpDesde)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1063, 56)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filtros"
        '
        'cmbServicio
        '
        Me.cmbServicio.FormattingEnabled = True
        Me.cmbServicio.Location = New System.Drawing.Point(598, 20)
        Me.cmbServicio.Name = "cmbServicio"
        Me.cmbServicio.Size = New System.Drawing.Size(304, 21)
        Me.cmbServicio.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(546, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Servicio"
        '
        'btnBuscar
        '
        Me.btnBuscar.Image = Global.pagos.My.Resources.Resources.search
        Me.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscar.Location = New System.Drawing.Point(923, 20)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscar.TabIndex = 5
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'chkPagados
        '
        Me.chkPagados.AutoSize = True
        Me.chkPagados.Location = New System.Drawing.Point(446, 19)
        Me.chkPagados.Name = "chkPagados"
        Me.chkPagados.Size = New System.Drawing.Size(68, 17)
        Me.chkPagados.TabIndex = 4
        Me.chkPagados.Text = "Pagados"
        Me.chkPagados.UseVisualStyleBackColor = True
        '
        'chkConciliados
        '
        Me.chkConciliados.AutoSize = True
        Me.chkConciliados.Location = New System.Drawing.Point(337, 19)
        Me.chkConciliados.Name = "chkConciliados"
        Me.chkConciliados.Size = New System.Drawing.Size(80, 17)
        Me.chkConciliados.TabIndex = 3
        Me.chkConciliados.Text = "Conciliados"
        Me.chkConciliados.UseVisualStyleBackColor = True
        '
        'dtpHasta
        '
        Me.dtpHasta.CustomFormat = "dd/MM/yyyy"
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpHasta.Location = New System.Drawing.Point(205, 20)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(102, 20)
        Me.dtpHasta.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(160, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Hasta"
        '
        'dtpDesde
        '
        Me.dtpDesde.CustomFormat = "dd/MM/yyyy"
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDesde.Location = New System.Drawing.Point(52, 20)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(102, 20)
        Me.dtpDesde.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Desde"
        '
        'crReporte
        '
        Me.crReporte.ActiveViewIndex = -1
        Me.crReporte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crReporte.Cursor = System.Windows.Forms.Cursors.Default
        Me.crReporte.Location = New System.Drawing.Point(13, 75)
        Me.crReporte.Name = "crReporte"
        Me.crReporte.Size = New System.Drawing.Size(1062, 446)
        Me.crReporte.TabIndex = 1
        '
        'RptConServicios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1087, 533)
        Me.ControlBox = False
        Me.Controls.Add(Me.crReporte)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "RptConServicios"
        Me.Text = "Reporte de Servicios"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkPagados As System.Windows.Forms.CheckBox
    Friend WithEvents chkConciliados As System.Windows.Forms.CheckBox
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents crReporte As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents cmbServicio As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
