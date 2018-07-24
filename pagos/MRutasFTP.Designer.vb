<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MRutasFTP
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
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.txtRutaOr = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRutaDes = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(96, 12)
        Me.txtDescripcion.MaxLength = 256
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(374, 20)
        Me.txtDescripcion.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Descripción"
        '
        'btnAceptar
        '
        Me.btnAceptar.Image = Global.pagos.My.Resources.Resources._109_AllAnnotations_Default_16x16_72
        Me.btnAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAceptar.Location = New System.Drawing.Point(365, 177)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(107, 40)
        Me.btnAceptar.TabIndex = 5
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Image = Global.pagos.My.Resources.Resources._109_AllAnnotations_Error_16x16_72
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(231, 177)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(107, 40)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtRutaOr
        '
        Me.txtRutaOr.Location = New System.Drawing.Point(96, 51)
        Me.txtRutaOr.MaxLength = 356
        Me.txtRutaOr.Multiline = True
        Me.txtRutaOr.Name = "txtRutaOr"
        Me.txtRutaOr.Size = New System.Drawing.Size(374, 48)
        Me.txtRutaOr.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Ruta origen"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 117)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Ruta destino"
        '
        'txtRutaDes
        '
        Me.txtRutaDes.Location = New System.Drawing.Point(96, 117)
        Me.txtRutaDes.MaxLength = 356
        Me.txtRutaDes.Multiline = True
        Me.txtRutaDes.Name = "txtRutaDes"
        Me.txtRutaDes.Size = New System.Drawing.Size(374, 48)
        Me.txtRutaDes.TabIndex = 3
        '
        'MRutasFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(499, 231)
        Me.Controls.Add(Me.txtRutaDes)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtRutaOr)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.txtDescripcion)
        Me.Controls.Add(Me.Label5)
        Me.Name = "MRutasFTP"
        Me.Text = "MConceptosServicios"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents txtRutaOr As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRutaDes As System.Windows.Forms.TextBox
End Class
