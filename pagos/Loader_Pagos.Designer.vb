<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Loader_Pagos
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.My_BgWorker = New System.ComponentModel.BackgroundWorker()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'My_BgWorker
        '
        Me.My_BgWorker.WorkerReportsProgress = True
        Me.My_BgWorker.WorkerSupportsCancellation = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 12)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(470, 23)
        Me.ProgressBar1.TabIndex = 0
        '
        'Loader_Base
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(494, 48)
        Me.ControlBox = False
        Me.Controls.Add(Me.ProgressBar1)
        Me.MaximizeBox = False
        Me.Name = "Loader_Base"
        Me.Text = "Loader_Base"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents My_BgWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ProgressBar1 As ProgressBar
End Class
