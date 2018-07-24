<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BConceptosServicios
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
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.grvConceptos = New System.Windows.Forms.DataGridView()
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnEditar
        '
        Me.btnEditar.Image = Global.pagos.My.Resources.Resources._126_Edit_16x16_72
        Me.btnEditar.Location = New System.Drawing.Point(12, 12)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(33, 32)
        Me.btnEditar.TabIndex = 1
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'grvConceptos
        '
        Me.grvConceptos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grvConceptos.Location = New System.Drawing.Point(13, 51)
        Me.grvConceptos.Name = "grvConceptos"
        Me.grvConceptos.Size = New System.Drawing.Size(1038, 462)
        Me.grvConceptos.TabIndex = 2
        '
        'BConceptosServicios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1063, 525)
        Me.ControlBox = False
        Me.Controls.Add(Me.grvConceptos)
        Me.Controls.Add(Me.btnEditar)
        Me.Name = "BConceptosServicios"
        Me.Text = "BConceptosServicios"
        CType(Me.grvConceptos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents grvConceptos As System.Windows.Forms.DataGridView
End Class
