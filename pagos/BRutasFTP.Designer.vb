<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BRutasFTP
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
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnCrear = New System.Windows.Forms.Button()
        Me.grvConceptos = New System.Windows.Forms.DataGridView()
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnModificar
        '
        Me.btnModificar.Image = Global.pagos.My.Resources.Resources._126_Edit_16x16_72
        Me.btnModificar.Location = New System.Drawing.Point(51, 12)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(33, 32)
        Me.btnModificar.TabIndex = 1
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnCrear
        '
        Me.btnCrear.Image = Global.pagos.My.Resources.Resources.crear
        Me.btnCrear.Location = New System.Drawing.Point(12, 12)
        Me.btnCrear.Name = "btnCrear"
        Me.btnCrear.Size = New System.Drawing.Size(33, 32)
        Me.btnCrear.TabIndex = 2
        Me.btnCrear.UseVisualStyleBackColor = True
        '
        'grvConceptos
        '
        Me.grvConceptos.Location = New System.Drawing.Point(12, 59)
        Me.grvConceptos.Name = "grvConceptos"
        Me.grvConceptos.Size = New System.Drawing.Size(809, 436)
        Me.grvConceptos.TabIndex = 0
        '
        'BRutasFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(833, 525)
        Me.ControlBox = False
        Me.Controls.Add(Me.grvConceptos)
        Me.Controls.Add(Me.btnCrear)
        Me.Controls.Add(Me.btnModificar)
        Me.Name = "BRutasFTP"
        Me.Text = "BConceptosServicios"
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnModificar As System.Windows.Forms.Button
    Friend WithEvents btnCrear As System.Windows.Forms.Button
    Friend WithEvents grvConceptos As System.Windows.Forms.DataGridView
End Class
