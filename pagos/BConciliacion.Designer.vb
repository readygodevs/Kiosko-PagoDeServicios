<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BConciliacion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BConciliacion))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtFolio = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbServicio = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmsMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EliminarRegistroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grvConceptos = New C1.Win.C1TrueDBGrid.C1TrueDBGrid()
        Me.GroupBox1.SuspendLayout()
        Me.cmsMenu.SuspendLayout()
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnBuscar)
        Me.GroupBox1.Controls.Add(Me.txtFolio)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cmbServicio)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.dtpHasta)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.dtpDesde)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1049, 56)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filtros"
        '
        'btnBuscar
        '
        Me.btnBuscar.Image = Global.pagos.My.Resources.Resources.search
        Me.btnBuscar.Location = New System.Drawing.Point(880, 20)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(35, 23)
        Me.btnBuscar.TabIndex = 12
        Me.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtFolio
        '
        Me.txtFolio.Location = New System.Drawing.Point(745, 20)
        Me.txtFolio.Name = "txtFolio"
        Me.txtFolio.Size = New System.Drawing.Size(119, 20)
        Me.txtFolio.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(709, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Folio"
        '
        'cmbServicio
        '
        Me.cmbServicio.FormattingEnabled = True
        Me.cmbServicio.Location = New System.Drawing.Point(387, 20)
        Me.cmbServicio.Name = "cmbServicio"
        Me.cmbServicio.Size = New System.Drawing.Size(304, 21)
        Me.cmbServicio.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(335, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Servicio"
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
        'cmsMenu
        '
        Me.cmsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EliminarRegistroToolStripMenuItem})
        Me.cmsMenu.Name = "ContextMenuStrip1"
        Me.cmsMenu.Size = New System.Drawing.Size(173, 26)
        '
        'EliminarRegistroToolStripMenuItem
        '
        Me.EliminarRegistroToolStripMenuItem.Name = "EliminarRegistroToolStripMenuItem"
        Me.EliminarRegistroToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.EliminarRegistroToolStripMenuItem.Text = "Eliminar Registro..."
        '
        'grvConceptos
        '
        Me.grvConceptos.AllowUpdate = False
        Me.grvConceptos.FilterBar = True
        Me.grvConceptos.GroupByCaption = "Drag a column header here to group by that column"
        Me.grvConceptos.Images.Add(CType(resources.GetObject("grvConceptos.Images"), System.Drawing.Image))
        Me.grvConceptos.Location = New System.Drawing.Point(12, 74)
        Me.grvConceptos.Name = "grvConceptos"
        Me.grvConceptos.PreviewInfo.Location = New System.Drawing.Point(0, 0)
        Me.grvConceptos.PreviewInfo.Size = New System.Drawing.Size(0, 0)
        Me.grvConceptos.PreviewInfo.ZoomFactor = 75.0R
        Me.grvConceptos.PrintInfo.PageSettings = CType(resources.GetObject("grvConceptos.PrintInfo.PageSettings"), System.Drawing.Printing.PageSettings)
        Me.grvConceptos.Size = New System.Drawing.Size(1162, 439)
        Me.grvConceptos.TabIndex = 2
        Me.grvConceptos.Text = "C1TrueDBGrid1"
        Me.grvConceptos.PropBag = resources.GetString("grvConceptos.PropBag")
        '
        'BConciliacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1186, 525)
        Me.ControlBox = False
        Me.Controls.Add(Me.grvConceptos)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "BConciliacion"
        Me.Text = "BConceptosServicios"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.cmsMenu.ResumeLayout(False)
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmsMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EliminarRegistroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbServicio As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFolio As System.Windows.Forms.TextBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents grvConceptos As C1.Win.C1TrueDBGrid.C1TrueDBGrid
End Class
