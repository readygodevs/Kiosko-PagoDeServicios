<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.PagosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PagosToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguraciónToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConceptosDeServiciosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RutasFTPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConciliaciónDeServiciosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PagosToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PagosToolStripMenuItem, Me.ConfiguraciónToolStripMenuItem, Me.ReportesToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1187, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'PagosToolStripMenuItem
        '
        Me.PagosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PagosToolStripMenuItem1})
        Me.PagosToolStripMenuItem.Name = "PagosToolStripMenuItem"
        Me.PagosToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.PagosToolStripMenuItem.Text = "Pagos"
        '
        'PagosToolStripMenuItem1
        '
        Me.PagosToolStripMenuItem1.Name = "PagosToolStripMenuItem1"
        Me.PagosToolStripMenuItem1.Size = New System.Drawing.Size(106, 22)
        Me.PagosToolStripMenuItem1.Text = "pagos"
        '
        'ConfiguraciónToolStripMenuItem
        '
        Me.ConfiguraciónToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConceptosDeServiciosToolStripMenuItem, Me.RutasFTPToolStripMenuItem, Me.ConciliaciónDeServiciosToolStripMenuItem})
        Me.ConfiguraciónToolStripMenuItem.Name = "ConfiguraciónToolStripMenuItem"
        Me.ConfiguraciónToolStripMenuItem.Size = New System.Drawing.Size(95, 20)
        Me.ConfiguraciónToolStripMenuItem.Text = "Configuración"
        '
        'ConceptosDeServiciosToolStripMenuItem
        '
        Me.ConceptosDeServiciosToolStripMenuItem.Name = "ConceptosDeServiciosToolStripMenuItem"
        Me.ConceptosDeServiciosToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ConceptosDeServiciosToolStripMenuItem.Text = "Conceptos de Servicios"
        '
        'RutasFTPToolStripMenuItem
        '
        Me.RutasFTPToolStripMenuItem.Name = "RutasFTPToolStripMenuItem"
        Me.RutasFTPToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.RutasFTPToolStripMenuItem.Text = "Rutas FTP"
        '
        'ConciliaciónDeServiciosToolStripMenuItem
        '
        Me.ConciliaciónDeServiciosToolStripMenuItem.Name = "ConciliaciónDeServiciosToolStripMenuItem"
        Me.ConciliaciónDeServiciosToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ConciliaciónDeServiciosToolStripMenuItem.Text = "Conciliación de Servicios"
        '
        'ReportesToolStripMenuItem
        '
        Me.ReportesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PagosToolStripMenuItem2})
        Me.ReportesToolStripMenuItem.Name = "ReportesToolStripMenuItem"
        Me.ReportesToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.ReportesToolStripMenuItem.Text = "Reportes"
        '
        'PagosToolStripMenuItem2
        '
        Me.PagosToolStripMenuItem2.Name = "PagosToolStripMenuItem2"
        Me.PagosToolStripMenuItem2.Size = New System.Drawing.Size(106, 22)
        Me.PagosToolStripMenuItem2.Text = "Pagos"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.pagos.My.Resources.Resources._010680_0000d
        Me.ClientSize = New System.Drawing.Size(1187, 540)
        Me.Controls.Add(Me.MenuStrip1)
        Me.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Menu Pagos Version 1.8.2"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents PagosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PagosToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfiguraciónToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConceptosDeServiciosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RutasFTPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PagosToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConciliaciónDeServiciosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
