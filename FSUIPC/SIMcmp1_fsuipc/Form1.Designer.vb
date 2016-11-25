<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnFSUIPC_Connect = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox5 = New System.Windows.Forms.CheckBox()
        Me.CheckBox6 = New System.Windows.Forms.CheckBox()
        Me.CheckBox7 = New System.Windows.Forms.CheckBox()
        Me.CheckBox8 = New System.Windows.Forms.CheckBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.txtOilPress = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtVacuum = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFuel = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtVolts = New System.Windows.Forms.TextBox()
        Me.lblVacuum = New System.Windows.Forms.Label()
        Me.lblLFuel = New System.Windows.Forms.Label()
        Me.lblRFuel = New System.Windows.Forms.Label()
        Me.lblOilPress = New System.Windows.Forms.Label()
        Me.lblMVolts = New System.Windows.Forms.Label()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.cmbCOMa = New System.Windows.Forms.ComboBox()
        Me.btnCOMaDisc = New System.Windows.Forms.Button()
        Me.TextCOMaReturn = New System.Windows.Forms.TextBox()
        Me.chkCOMaAuto = New System.Windows.Forms.CheckBox()
        Me.SerialTimer = New System.Windows.Forms.Timer(Me.components)
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnFSUIPC_Disconnect = New System.Windows.Forms.Button()
        Me.lblAVolts = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkReadyToFly = New System.Windows.Forms.CheckBox()
        Me.lblReadyToFly = New System.Windows.Forms.Label()
        Me.txtCredits = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'btnFSUIPC_Connect
        '
        Me.btnFSUIPC_Connect.Location = New System.Drawing.Point(12, 1)
        Me.btnFSUIPC_Connect.Name = "btnFSUIPC_Connect"
        Me.btnFSUIPC_Connect.Size = New System.Drawing.Size(114, 23)
        Me.btnFSUIPC_Connect.TabIndex = 0
        Me.btnFSUIPC_Connect.Text = "Connect FSUIPC"
        Me.btnFSUIPC_Connect.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(24, 55)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(56, 17)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.Text = "COM1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(24, 79)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(56, 17)
        Me.CheckBox2.TabIndex = 2
        Me.CheckBox2.Text = "COM2"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(24, 103)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(56, 17)
        Me.CheckBox3.TabIndex = 3
        Me.CheckBox3.Text = "BOTH"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Location = New System.Drawing.Point(24, 127)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(54, 17)
        Me.CheckBox4.TabIndex = 4
        Me.CheckBox4.Text = "NAV1"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.Location = New System.Drawing.Point(24, 151)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(54, 17)
        Me.CheckBox5.TabIndex = 5
        Me.CheckBox5.Text = "NAV2"
        Me.CheckBox5.UseVisualStyleBackColor = True
        '
        'CheckBox6
        '
        Me.CheckBox6.AutoSize = True
        Me.CheckBox6.Location = New System.Drawing.Point(24, 175)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(50, 17)
        Me.CheckBox6.TabIndex = 6
        Me.CheckBox6.Text = "MKR"
        Me.CheckBox6.UseVisualStyleBackColor = True
        '
        'CheckBox7
        '
        Me.CheckBox7.AutoSize = True
        Me.CheckBox7.Location = New System.Drawing.Point(24, 199)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(50, 17)
        Me.CheckBox7.TabIndex = 7
        Me.CheckBox7.Text = "DME"
        Me.CheckBox7.UseVisualStyleBackColor = True
        '
        'CheckBox8
        '
        Me.CheckBox8.AutoSize = True
        Me.CheckBox8.Location = New System.Drawing.Point(90, 55)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.Size = New System.Drawing.Size(75, 17)
        Me.CheckBox8.TabIndex = 8
        Me.CheckBox8.Text = "GPS/NAV"
        Me.CheckBox8.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(7, 243)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(320, 44)
        Me.TextBox1.TabIndex = 9
        Me.TextBox1.TabStop = False
        Me.TextBox1.Visible = False
        '
        'txtOilPress
        '
        Me.txtOilPress.Location = New System.Drawing.Point(277, 120)
        Me.txtOilPress.Name = "txtOilPress"
        Me.txtOilPress.Size = New System.Drawing.Size(40, 20)
        Me.txtOilPress.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(232, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Oil Pressure (PSI)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(119, 104)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Vacuum"
        '
        'txtVacuum
        '
        Me.txtVacuum.Location = New System.Drawing.Point(168, 120)
        Me.txtVacuum.Name = "txtVacuum"
        Me.txtVacuum.Size = New System.Drawing.Size(40, 20)
        Me.txtVacuum.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(119, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "  L     Fuel    R %"
        '
        'txtFuel
        '
        Me.txtFuel.Location = New System.Drawing.Point(168, 182)
        Me.txtFuel.Name = "txtFuel"
        Me.txtFuel.Size = New System.Drawing.Size(46, 20)
        Me.txtFuel.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(233, 147)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "VOLTS (Master)"
        '
        'txtVolts
        '
        Me.txtVolts.Location = New System.Drawing.Point(277, 163)
        Me.txtVolts.Name = "txtVolts"
        Me.txtVolts.Size = New System.Drawing.Size(40, 20)
        Me.txtVolts.TabIndex = 12
        '
        'lblVacuum
        '
        Me.lblVacuum.AutoSize = True
        Me.lblVacuum.BackColor = System.Drawing.Color.Yellow
        Me.lblVacuum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVacuum.Location = New System.Drawing.Point(123, 123)
        Me.lblVacuum.Name = "lblVacuum"
        Me.lblVacuum.Size = New System.Drawing.Size(32, 13)
        Me.lblVacuum.TabIndex = 24
        Me.lblVacuum.Text = "3.56"
        '
        'lblLFuel
        '
        Me.lblLFuel.AutoSize = True
        Me.lblLFuel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLFuel.Location = New System.Drawing.Point(123, 166)
        Me.lblLFuel.Name = "lblLFuel"
        Me.lblLFuel.Size = New System.Drawing.Size(21, 13)
        Me.lblLFuel.TabIndex = 25
        Me.lblLFuel.Text = "99"
        '
        'lblRFuel
        '
        Me.lblRFuel.AutoSize = True
        Me.lblRFuel.Location = New System.Drawing.Point(165, 166)
        Me.lblRFuel.Name = "lblRFuel"
        Me.lblRFuel.Size = New System.Drawing.Size(19, 13)
        Me.lblRFuel.TabIndex = 26
        Me.lblRFuel.Text = "30"
        '
        'lblOilPress
        '
        Me.lblOilPress.AutoSize = True
        Me.lblOilPress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOilPress.Location = New System.Drawing.Point(233, 123)
        Me.lblOilPress.Name = "lblOilPress"
        Me.lblOilPress.Size = New System.Drawing.Size(32, 13)
        Me.lblOilPress.TabIndex = 27
        Me.lblOilPress.Text = "4.22"
        '
        'lblMVolts
        '
        Me.lblMVolts.AutoSize = True
        Me.lblMVolts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMVolts.Location = New System.Drawing.Point(233, 166)
        Me.lblMVolts.Name = "lblMVolts"
        Me.lblMVolts.Size = New System.Drawing.Size(28, 13)
        Me.lblMVolts.TabIndex = 28
        Me.lblMVolts.Text = "28v"
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 115000
        Me.SerialPort1.PortName = "COM7"
        '
        'cmbCOMa
        '
        Me.cmbCOMa.FormattingEnabled = True
        Me.cmbCOMa.Location = New System.Drawing.Point(189, 27)
        Me.cmbCOMa.Name = "cmbCOMa"
        Me.cmbCOMa.Size = New System.Drawing.Size(60, 21)
        Me.cmbCOMa.TabIndex = 53
        '
        'btnCOMaDisc
        '
        Me.btnCOMaDisc.Location = New System.Drawing.Point(255, 28)
        Me.btnCOMaDisc.Name = "btnCOMaDisc"
        Me.btnCOMaDisc.Size = New System.Drawing.Size(72, 20)
        Me.btnCOMaDisc.TabIndex = 54
        Me.btnCOMaDisc.Text = "btnCOMaDisc"
        Me.btnCOMaDisc.UseVisualStyleBackColor = True
        '
        'TextCOMaReturn
        '
        Me.TextCOMaReturn.Location = New System.Drawing.Point(189, 55)
        Me.TextCOMaReturn.Name = "TextCOMaReturn"
        Me.TextCOMaReturn.Size = New System.Drawing.Size(138, 20)
        Me.TextCOMaReturn.TabIndex = 55
        Me.TextCOMaReturn.Text = "TextCOMaReturn"
        '
        'chkCOMaAuto
        '
        Me.chkCOMaAuto.AutoSize = True
        Me.chkCOMaAuto.Location = New System.Drawing.Point(255, 5)
        Me.chkCOMaAuto.Name = "chkCOMaAuto"
        Me.chkCOMaAuto.Size = New System.Drawing.Size(48, 17)
        Me.chkCOMaAuto.TabIndex = 56
        Me.chkCOMaAuto.Text = "Auto"
        Me.chkCOMaAuto.UseVisualStyleBackColor = True
        '
        'SerialTimer
        '
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.BackColor = System.Drawing.Color.Blue
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Enabled = False
        Me.RadioButton1.ForeColor = System.Drawing.Color.Blue
        Me.RadioButton1.Location = New System.Drawing.Point(92, 78)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton1.TabIndex = 57
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.UseVisualStyleBackColor = False
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Enabled = False
        Me.RadioButton2.Location = New System.Drawing.Point(112, 78)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton2.TabIndex = 58
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Enabled = False
        Me.RadioButton3.Location = New System.Drawing.Point(132, 78)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton3.TabIndex = 59
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "SIMCMP1"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(93, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'StartTimer
        '
        '
        'btnFSUIPC_Disconnect
        '
        Me.btnFSUIPC_Disconnect.Location = New System.Drawing.Point(12, 24)
        Me.btnFSUIPC_Disconnect.Name = "btnFSUIPC_Disconnect"
        Me.btnFSUIPC_Disconnect.Size = New System.Drawing.Size(114, 23)
        Me.btnFSUIPC_Disconnect.TabIndex = 60
        Me.btnFSUIPC_Disconnect.Text = "Disconnect FSUIPC"
        Me.btnFSUIPC_Disconnect.UseVisualStyleBackColor = True
        '
        'lblAVolts
        '
        Me.lblAVolts.AutoSize = True
        Me.lblAVolts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAVolts.Location = New System.Drawing.Point(233, 189)
        Me.lblAVolts.Name = "lblAVolts"
        Me.lblAVolts.Size = New System.Drawing.Size(25, 13)
        Me.lblAVolts.TabIndex = 61
        Me.lblAVolts.Text = "28v"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(275, 189)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 62
        Me.Label4.Text = "AV Volts"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(142, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 63
        Me.Label6.Text = "SIMAV8 20161122"
        '
        'chkReadyToFly
        '
        Me.chkReadyToFly.AutoSize = True
        Me.chkReadyToFly.Location = New System.Drawing.Point(122, 209)
        Me.chkReadyToFly.Name = "chkReadyToFly"
        Me.chkReadyToFly.Size = New System.Drawing.Size(89, 17)
        Me.chkReadyToFly.TabIndex = 64
        Me.chkReadyToFly.Text = "Ready To Fly"
        Me.chkReadyToFly.UseVisualStyleBackColor = True
        '
        'lblReadyToFly
        '
        Me.lblReadyToFly.AutoSize = True
        Me.lblReadyToFly.Location = New System.Drawing.Point(88, 210)
        Me.lblReadyToFly.Name = "lblReadyToFly"
        Me.lblReadyToFly.Size = New System.Drawing.Size(28, 13)
        Me.lblReadyToFly.TabIndex = 65
        Me.lblReadyToFly.Text = "RTF"
        '
        'txtCredits
        '
        Me.txtCredits.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCredits.Enabled = False
        Me.txtCredits.Location = New System.Drawing.Point(7, 239)
        Me.txtCredits.Multiline = True
        Me.txtCredits.Name = "txtCredits"
        Me.txtCredits.Size = New System.Drawing.Size(320, 48)
        Me.txtCredits.TabIndex = 66
        Me.txtCredits.TabStop = False
        Me.txtCredits.Text = "Credits..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Pete Dowson, FSUIPC, http://www.schiratti.com/dowson.html" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Paul Henty" & _
    ", FSUIPC dot Net Client 2.4"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 292)
        Me.Controls.Add(Me.txtCredits)
        Me.Controls.Add(Me.lblReadyToFly)
        Me.Controls.Add(Me.chkReadyToFly)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblAVolts)
        Me.Controls.Add(Me.btnFSUIPC_Disconnect)
        Me.Controls.Add(Me.RadioButton3)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.chkCOMaAuto)
        Me.Controls.Add(Me.TextCOMaReturn)
        Me.Controls.Add(Me.btnCOMaDisc)
        Me.Controls.Add(Me.cmbCOMa)
        Me.Controls.Add(Me.lblMVolts)
        Me.Controls.Add(Me.lblOilPress)
        Me.Controls.Add(Me.lblRFuel)
        Me.Controls.Add(Me.lblLFuel)
        Me.Controls.Add(Me.lblVacuum)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtVolts)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtFuel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtVacuum)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtOilPress)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.CheckBox8)
        Me.Controls.Add(Me.CheckBox7)
        Me.Controls.Add(Me.CheckBox6)
        Me.Controls.Add(Me.CheckBox5)
        Me.Controls.Add(Me.CheckBox4)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.btnFSUIPC_Connect)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form1"
        Me.TopMost = True
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnFSUIPC_Connect As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents txtOilPress As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtVacuum As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFuel As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtVolts As System.Windows.Forms.TextBox
    Friend WithEvents lblVacuum As System.Windows.Forms.Label
    Friend WithEvents lblLFuel As System.Windows.Forms.Label
    Friend WithEvents lblRFuel As System.Windows.Forms.Label
    Friend WithEvents lblOilPress As System.Windows.Forms.Label
    Friend WithEvents lblMVolts As System.Windows.Forms.Label
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents cmbCOMa As System.Windows.Forms.ComboBox
    Friend WithEvents btnCOMaDisc As System.Windows.Forms.Button
    Friend WithEvents TextCOMaReturn As System.Windows.Forms.TextBox
    Friend WithEvents chkCOMaAuto As System.Windows.Forms.CheckBox
    Friend WithEvents SerialTimer As System.Windows.Forms.Timer
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents StartTimer As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnFSUIPC_Disconnect As System.Windows.Forms.Button
    Friend WithEvents lblAVolts As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkReadyToFly As System.Windows.Forms.CheckBox
    Friend WithEvents lblReadyToFly As System.Windows.Forms.Label
    Friend WithEvents txtCredits As System.Windows.Forms.TextBox

End Class
