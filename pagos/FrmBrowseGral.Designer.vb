<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmBrowseGral
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBrowseGral))
        Me.grvArticulos = New C1.Win.C1TrueDBGrid.C1TrueDBGrid()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.grvArticulos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grvArticulos
        '
        Me.grvArticulos.AllowUpdate = False
        Me.grvArticulos.FilterBar = True
        Me.grvArticulos.GroupByCaption = "Drag a column header here to group by that column"
        Me.grvArticulos.Images.Add(CType(resources.GetObject("grvArticulos.Images"), System.Drawing.Image))
        Me.grvArticulos.Location = New System.Drawing.Point(12, 10)
        Me.grvArticulos.Name = "grvArticulos"
        Me.grvArticulos.PreviewInfo.Location = New System.Drawing.Point(0, 0)
        Me.grvArticulos.PreviewInfo.Size = New System.Drawing.Size(0, 0)
        Me.grvArticulos.PreviewInfo.ZoomFactor = 75.0R
        Me.grvArticulos.PrintInfo.PageSettings = CType(resources.GetObject("grvArticulos.PrintInfo.PageSettings"), System.Drawing.Printing.PageSettings)
        Me.grvArticulos.Size = New System.Drawing.Size(409, 385)
        Me.grvArticulos.TabIndex = 3
        Me.grvArticulos.Text = "C1TrueDBGrid1"
        Me.grvArticulos.PropBag = resources.GetString("grvArticulos.PropBag")
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(346, 401)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Aceptar"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FrmBrowseGral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 432)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.grvArticulos)
        Me.Name = "FrmBrowseGral"
        Me.Text = "Seleccione los conceptos"
        CType(Me.grvArticulos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grvArticulos As C1.Win.C1TrueDBGrid.C1TrueDBGrid
    Friend WithEvents Button1 As Button
End Class
