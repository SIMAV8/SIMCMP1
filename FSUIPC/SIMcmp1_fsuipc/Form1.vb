Imports FSUIPC
Imports System.Collections

Partial Public Class Form1

    ' IMPORTANT: Project Properties, Compile, Target x86 CPU (FSUIPC wrapper is 32 bit only)

    ' Constants
    Private Const AppTitle As String = "SIMcmp1_fsuipc"

    ' Register the data from FSUIPC we're wanting
    Dim RadioSwitches As Offset(Of BitArray) = New FSUIPC.Offset(Of BitArray)(&H3122, 1)   'Bitarray for radio switches
    '   ADF1,DME,MKR,NAV2,NAV1,BOTH,COM2,COM1
    '    0    1   2   3    4    5    6    7
    Dim NAVGPSSwitch As Offset(Of UInteger) = New FSUIPC.Offset(Of UInteger)(&H132C)
    ' ( L Low Fuel R, VACUUM, OIL PRESS, VOLTS).
    Dim Engine1OilPressure As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&H3B60)  ' divide by 144 for PSI
    ' FSInterrogate2std comes in the FSUIPC SDK. It allows you to check values in FS of about 1600 variables. 
    ' When FS is running, you can see what a lot of this stuff does and find the offsets
    ' Vacuum was hard for me to track down, it's "GyroSuctionHg"
    Dim GyroSuctionHg As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&HB18)
    Dim LFuelLevel As Offset(Of UInteger) = New FSUIPC.Offset(Of UInteger)(&HB7C)   '% * 128 * 65536
    Dim RFuelLevel As Offset(Of UInteger) = New FSUIPC.Offset(Of UInteger)(&HB94)   '% * 128 * 65536
    Dim MainBusVoltage As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&H2840)
    ' or, alternate maybe of 2880 Generator Alternator 1 bus volt?
    Dim AvionicsBusVoltage As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&H2850)
    'To Arduino <a1 = MasterBusVolts, <g1 = AvionicsBusvoltage
    'forget about master and avionics SWITCHES, just look at BUS VOLTAGE

    'OMI indicators
    Dim OMI_O As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBB0)           'activated when true
    Dim OMI_M As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBAE)           'activated when true
    Dim OMI_I As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBAC)           'activated when true

    'Seems awkward / only useful when FSX is still starting up
    Dim ReadyToFly As Offset(Of Byte) = New FSUIPC.Offset(Of Byte)(&H3364)

    'Global variables
    Dim gsSerIndata As String
    Dim glStartTimerEvents As Long = 0

    Private Enum RadioSwitchType        'I used numbers in the code, but COM1 would be 7 in enum
        ADF
        DME
        MKR
        NAV2
        NAV1
        BOTH
        COM2
        COM1
    End Enum

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' form load event - after launch but before drawn
        ' do setup and prep work

        ' bring in .NET SETTINGS (Project Properties, Settings)
        txtFuel.Text = My.Settings.Warn_LT_Fuel
        txtVacuum.Text = My.Settings.Warn_LT_VAC
        txtOilPress.Text = My.Settings.Alarm_LT_OilPressure
        txtVolts.Text = My.Settings.Alarm_LT_Volts

        'Set Form title
        Me.Text = AppTitle

        'Serial Port init
        SerialPort1.BaudRate = 115200    'slow down if want to (make Arduino serial speed match of course)
        SerialTimer.Interval = 100       'timer at 100 milliseconds
        'SerialTimer.Enabled = True       'enable timer

        'Get list of available COM ports from Windows
        GetAvailableCOMPorts()

        'initialize some window controls
        btnCOMaDisc.Visible = False
        TextCOMaReturn.Text = ""
        StartTimer.Interval = 1000

        'AUTO Connect? Yes, serial then timer for FSUIPC connection
        If My.Settings.COMaAuto = True Then
            If My.Settings.COMaName <> "" Then
                cmbCOMa.Text = My.Settings.COMaName
                btnCOMaDisc.Visible = True
                btnCOMaDisc.Text = "Disconnect"
                chkCOMaAuto.Checked = True
                'try and connect the serial port
                ConnectCOMa(My.Settings.COMaName)
            End If

            'Launch a timer to open FSUIPC on a delay AFTER serial, then minimize the form
            btnFSUIPC_Connect.Text = "Trying..."
            StartTimer.Enabled = True
        End If

        'Connect to FSUPIC, Actually not, the dot net serial libraries are known to be fairly unstable
        'Cannot reliably open serial comms at same time as FSUIPC, so use the start timer
        'OpenFSUIPC()
    End Sub
    Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        ' Application is unloading so call close to cleanup the UNMANAGED memory used by FSUIPC. 

        ' Close serial communications (with Arduino)
        If SerialPort1.IsOpen = True Then
            SerialPort1.WriteLine("@T0")        'turn off test mode if happens to be on
            SerialPort1.WriteLine("")           'write empty line to push out the above before closing
            SerialPort1.Close()
        End If

        'close FSUIPC connection
        FSUIPCConnection.Close()

        'Settings, save user form settings
        My.Settings.Warn_LT_Fuel = txtFuel.Text
        My.Settings.Warn_LT_VAC = txtVacuum.Text
        My.Settings.Alarm_LT_OilPressure = txtOilPress.Text
        My.Settings.Alarm_LT_Volts = txtVolts.Text
        My.Settings.COMaName = cmbCOMa.Text
        My.Settings.COMaAuto = chkCOMaAuto.Checked
        My.Settings.Save()

        'Exit app here
    End Sub
    Private Sub btnFSUIPC_Connect_Click(sender As Object, e As EventArgs) Handles btnFSUIPC_Connect.Click
        'Manually open FSUIPC
        TextBox1.Text = ""
        TextBox1.Visible = False
        OpenFSUIPC()
    End Sub

    Private Sub btnFSUIPC_Disconnect_Click(sender As Object, e As EventArgs) Handles btnFSUIPC_Disconnect.Click
        'Manually close FSUIPC
        Timer1.Enabled = False      'turn off event timer
        TextBox1.Visible = True
        btnFSUIPC_Connect.Enabled = True

        'close FSUIPC connection
        FSUIPCConnection.Close()

    End Sub

    Private Sub OpenFSUIPC()
        ' Opens FSUIPC - if all goes well then starts the timer to drive start the main application cycle.
        ' If can't open, display the error message

        Try
            ' Attempt to open a connection to FSUIPC (running on any version of Flight Sim)
            FSUIPCConnection.Open()

            ' Opened, disable the Connect button
            btnFSUIPC_Connect.Enabled = False
            btnFSUIPC_Connect.Text = "Connect"

            ' and enable the disconnect button
            btnFSUIPC_Disconnect.Enabled = True

            ' and start the timer ticking to drive the rest of the application
            Timer1.Interval = 100       '10 times a second poll FSUIPC
            Timer1.Enabled = True

        Catch ex As Exception
            ' Badness occurred - show the error message
            TextBox1.Visible = True
            TextBox1.Text = ex.Message      'no message box, use textbox on form
            'MessageBox.Show(ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' The timer handles the real-time updating of the Form.  The default group (ie, no group specified) is 
        ' processed and every Offset in the default group is updated.
        Try
            ' Process the default group
            FSUIPCConnection.Process()

            ' ReadyToFly doesn't seem incredibly useful, just showing the values
            lblReadyToFly.Text = ReadyToFly.Value
            If ReadyToFly.Value = 0 Then
                chkReadyToFly.Checked = True
            Else
                chkReadyToFly.Checked = False
            End If

            'Get radio switch values from SIM and update Arduino LEDs
            'RadioSwitches return 0=False or -1 = true
            'RadioSwitches is a BitArray of 1 byte (8bits) to offset 0x3122 
            'VB.net if else if shortcut IIf
            SerialPort1.WriteLine("=M" & IIf(RadioSwitches.Value(7), "1", "0"))
            SerialPort1.WriteLine("=N" & IIf(RadioSwitches.Value(6), "1", "0"))
            SerialPort1.WriteLine("=O" & IIf(RadioSwitches.Value(5), "1", "0"))
            SerialPort1.WriteLine("=P" & IIf(RadioSwitches.Value(4), "1", "0"))
            SerialPort1.WriteLine("=Q" & IIf(RadioSwitches.Value(3), "1", "0"))
            SerialPort1.WriteLine("=U" & IIf(RadioSwitches.Value(2), "1", "0"))
            SerialPort1.WriteLine("=R" & IIf(RadioSwitches.Value(1), "1", "0"))

            're-read the values from FSUIPC vars to update the Windows form checkboxes
            CheckBox1.Checked = RadioSwitches.Value(7)      'COM1
            CheckBox2.Checked = RadioSwitches.Value(6)      'COM2
            CheckBox3.Checked = RadioSwitches.Value(5)      'BOTH
            CheckBox4.Checked = RadioSwitches.Value(4)      'NAV1
            CheckBox5.Checked = RadioSwitches.Value(3)      'NAV2
            CheckBox6.Checked = RadioSwitches.Value(2)      'MKR
            CheckBox7.Checked = RadioSwitches.Value(1)      'DME

            'NAVGPS switch position in SIM
            'Dim NAVGPSSwitch As Offset(Of Integer) = New Offset(Of Integer)(&H132C, 1)
            CheckBox8.Checked = NAVGPSSwitch.Value
            SerialPort1.WriteLine("=l" & IIf(NAVGPSSwitch.Value = 1, "1", "0"))

            'Fetch EngineOilPressure from SIM
            'turn on panel indicator LEDs if below threshold
            'Dim Engine1OilPressure As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&H3B60)  ' Divide by 144 for psi
            lblOilPress.Text = Format(Engine1OilPressure.Value / 144, "0.00")
            If (Engine1OilPressure.Value / 144 < CDbl(txtOilPress.Text)) Then
                lblOilPress.BackColor = Color.Red
                SerialPort1.WriteLine("/F1")
            Else    'or, turn it off if above threshold
                lblOilPress.BackColor = Control.DefaultBackColor
                SerialPort1.WriteLine("/F0")
            End If

            'VOLTS measures and coloring
            'Dim MainBusVoltage As Offset(Of ULong) = New FSUIPC.Offset(Of ULong)(&H2840)    'unsigned long
            lblMVolts.Text = Format(MainBusVoltage.Value, "0.00")
            lblMVolts.BackColor = IIf(MainBusVoltage.Value < CLng(txtVolts.Text), Color.Red, Control.DefaultBackColor)
            SerialPort1.WriteLine("/R" & IIf(MainBusVoltage.Value < CLng(txtVolts.Text), "1", "0"))
            'tell arduino MasterBusVoltage is on if > 12 volts
            SerialPort1.WriteLine("<a" & IIf(MainBusVoltage.Value > 12, "1", "0"))

            'Dim AvionicsBusVoltage As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&H2840)
            lblAVolts.Text = Format(AvionicsBusVoltage.Value, "0.00")

            'tell arduino AvionicsBusVoltage is on if > 12 volts (arbitrary value, FSX seems to drop to 0 once declines to 17 volts)
            SerialPort1.WriteLine("<g" & IIf(AvionicsBusVoltage.Value > 12, "1", "0"))

            'Vacuum measures and coloring (called GyroSuctionHg in FSUIPC)
            'Dim GyroSuctionHg As Offset(Of Double) = New FSUIPC.Offset(Of Double)(&HB18)
            lblVacuum.Text = Format(GyroSuctionHg.Value, "0.00")
            lblVacuum.BackColor = IIf(GyroSuctionHg.Value < CDbl(txtVacuum.Text), Color.Yellow, Control.DefaultBackColor)
            SerialPort1.WriteLine("/N" & IIf(GyroSuctionHg.Value < CDbl(txtVacuum.Text), "1", "0"))

            'Fuel measures and coloring, left and right tanks
            'Dim LFuelLevel As Offset(Of UInteger) = New FSUIPC.Offset(Of UInteger)(&HB7C)   '% * 128 * 65536
            'Dim RFuelLevel As Offset(Of UInteger) = New FSUIPC.Offset(Of UInteger)(&HB94)   '% * 128 * 65536
            lblLFuel.Text = Format(LFuelLevel.Value / 128 / 65536 * 100, "0.00")
            lblLFuel.BackColor = IIf(CDbl(lblLFuel.Text) < txtFuel.Text, Color.Yellow, Control.DefaultBackColor)
            SerialPort1.WriteLine("/J" & IIf(CDbl(lblLFuel.Text) < txtFuel.Text, "1", "0"))

            lblRFuel.Text = Format(RFuelLevel.Value / 128 / 65536 * 100, "0.00")
            lblRFuel.BackColor = IIf(CDbl(lblRFuel.Text) < txtFuel.Text, Color.Yellow, Control.DefaultBackColor)
            SerialPort1.WriteLine("/K" & IIf(CDbl(lblRFuel.Text) < txtFuel.Text, "1", "0"))

            'OMI indicators
            'Dim OMI_O As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBAC)           'activated when true
            'Dim OMI_M As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBAE)           'activated when true
            'Dim OMI_I As Offset(Of UShort) = New FSUIPC.Offset(Of UShort)(&HBB0)
            If (OMI_O.Value Or OMI_M.Value Or OMI_I.Value) Then
                If (OMI_O.Value) Then
                    RadioButton1.BackColor = Color.Blue
                    RadioButton1.Checked = True
                    SerialPort1.WriteLine("=V1")
                End If
                If (OMI_M.Value) Then
                    RadioButton2.BackColor = Color.Red
                    RadioButton2.Checked = True
                    SerialPort1.WriteLine("=V2")
                End If
                If (OMI_I.Value) Then
                    RadioButton3.BackColor = Color.White
                    RadioButton3.Checked = True
                    SerialPort1.WriteLine("=V3")
                End If
            Else  'if none, then turn off all indicators on panel
                SerialPort1.WriteLine("=V0")
                RadioButton1.BackColor = Control.DefaultBackColor
                RadioButton1.Checked = False
                RadioButton2.BackColor = Control.DefaultBackColor
                RadioButton2.Checked = False
                RadioButton3.BackColor = Control.DefaultBackColor
                RadioButton3.Checked = False

            End If

        Catch exFSUIPC As FSUIPCException
            If exFSUIPC.FSUIPCErrorCode = FSUIPCError.FSUIPC_ERR_SENDMSG Or
                exFSUIPC.FSUIPCErrorCode = FSUIPCError.FSUIPC_ERR_NOTOPEN Then
                ' Send message error - connection to FSUIPC lost.
                ' Show message, disable the main timer loop and relight the 
                ' connection button:
                ' Also Close the broken connection.
                Timer1.Enabled = False
                btnFSUIPC_Connect.Enabled = True
                btnFSUIPC_Disconnect.Enabled = False
                FSUIPCConnection.Close()

                Me.Visible = True
                Me.WindowState = FormWindowState.Normal

                TextBox1.Visible = True
                TextBox1.Text = "The connection to FSUIPC has been lost."
                'MessageBox.Show("The connection to FSUIPC has been lost.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                ' not the disonnect error so some other baddness occured.
                ' just rethrow to halt the application
                Throw exFSUIPC
            End If

        Catch ex As Exception
            ' Sometime when the connection is lost, bad data gets returned and causes problems with some of the other lines.  
            ' This catch block just makes sure the user doesn't see any other Exceptions apart from FSUIPCExceptions.
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        'COM1 arduino switch read
        RadioSwitches.Value(7) = CheckBox1.Checked
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        'COM2 arduino switch read
        'bug maybe?  But COM2 cannot be selected if COM1 is active (but not vice-versa)
        'if checking this then uncheck COM1 THEN check COM2
        If CheckBox2.Checked Then
            CheckBox1.Checked = False
            RadioSwitches.Value(7) = CheckBox1.Checked
        End If
        RadioSwitches.Value(6) = CheckBox2.Checked
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        'BOTH
        RadioSwitches.Value(5) = CheckBox3.Checked
    End Sub
    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        'NAV1
        RadioSwitches.Value(4) = CheckBox4.Checked
    End Sub
    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        'NAV2
        RadioSwitches.Value(3) = CheckBox5.Checked
    End Sub
    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        'MKR
        RadioSwitches.Value(2) = CheckBox6.Checked
    End Sub
    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        'DME
        RadioSwitches.Value(1) = CheckBox7.Checked
    End Sub
    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        'GPS/NAV, different call...
    End Sub

    '*******************************************************************************
    '*******************************************************************************
    '*** CODE BELOW IS ALL FOR SERIAL COMMUNICATION WITH ARDUINO
    '*** Basically, SerialTimer fires every 100ms
    '*** if serial open, then poll data and see if arduino sent up anything
    '*** if it did, match code from Arduino to actions and send it over
    '*** Lots of threads on Internet about .NET serial com being very buggy
    '*** in my experience this is true, but works well enough to use stock .NET
    '*******************************************************************************
    '*******************************************************************************
    Private Sub GetAvailableCOMPorts()
        'Gather a list of all available COM ports on system and populate the combobox
        cmbCOMa.Items.Clear()
        cmbCOMa.Text = ""
        For Each sp As String In My.Computer.Ports.SerialPortNames
            cmbCOMa.Items.Add(sp)
        Next
    End Sub
    Private Sub cmbCOMa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCOMa.SelectedIndexChanged
        'if user selects something in ComboBox, try to open it
        If SerialPort1.IsOpen = True Then
            SerialPort1.Close()
        End If

        ConnectCOMa(cmbCOMa.Text)
    End Sub

    Private Sub ConnectCOMa(sPort As String)
        'try and connect to specified com port, eg COM8
        SerialTimer.Enabled = False

        'close VB serialport1 if open
        If SerialPort1.IsOpen = True Then
            SerialPort1.Close()
        End If

        'try and set to specified port
        Try
            SerialPort1.PortName = sPort
        Catch
        End Try

        'try and open specified port
        Try
            SerialPort1.Open()
            SerialPort1.DtrEnable = True        'raise DTR, arduino should give header in setup message

            'if it opens, then post the name and save it
            btnCOMaDisc.Visible = True
            btnCOMaDisc.Text = "Disconnect"
            My.Settings.COMaName = sPort
            My.Settings.Save()

            'enable the timer to poll the arduino for incoming commands
            SerialTimer.Enabled = True
        Catch ex As Exception
            TextCOMaReturn.Text = ex.Message
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnCOMaDisc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOMaDisc.Click
        'button to disconnect serial port
        On Error Resume Next        'in case Arduino unplugged, cover your butt
        SerialPort1.WriteLine("@T0")
        SerialPort1.Close()
        On Error GoTo 0

        If SerialPort1.IsOpen = False Then
            TextCOMaReturn.Text = ""
            btnCOMaDisc.Visible = False
        End If

        GetAvailableCOMPorts()
    End Sub

    Private Sub SerialTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SerialTimer.Tick
        'polls the serial port(s) every X milliseconds looking for incoming data
        Dim i As Integer, j As Integer, k As Integer

        If SerialPort1.IsOpen = True Then

            gsSerIndata = SerialPort1.ReadExisting.ToString
            If Len(gsSerIndata) > 0 Then
                TextCOMaReturn.Text = gsSerIndata   'show what's read on form
                gsSerIndata = Trim(gsSerIndata)     'trim off any junk

                'Acceleration for knobs (there aren't any in this project, but left code)
                'If an encoder knob turned quickly, data comes from Arduino in one read like
                'A56..A56..A56..A56..A56..A56..A56..A56..A56..   (.. are CRLF pairs)
                'can use that to do simulated acceleration.  count up the CRLFs in variable j
                j = 0 : k = 0
                For i = 1 To Len(gsSerIndata)
                    If Asc(Mid(gsSerIndata, i, 1)) = 13 Then
                        j = j + 1
                        If k = 0 Then k = i 'position of first CR
                    End If
                Next i

                'acceleration factor, 1=1, 2=2, 3=6, 4=8...
                If j > 2 Then
                    j = j * 3
                End If
            End If  'end if len(gsSerIndata) has something


            'if have some data and connected to FSX then process it
            'These strings come from Arduino code based on function
            'they are arbitrary, just make Arduino and Windows prog match up
            'Made them to match where possible Jim Z's Link2FS
            If k > 0 Then
                Select Case Mid(gsSerIndata, 1, k - 1)   'get first command in string ()
                    Case "A45"  'COM1 button
                        'CheckBox1.Checked = Not CheckBox1.Checked
                        RadioSwitches.Value(7) = 1      'CheckBox1.Checked     'COM1
                    Case "A46"  'COM2 button
                        'CheckBox2.Checked = Not CheckBox2.Checked
                        RadioSwitches.Value(6) = 1      'CheckBox2.Checked     'COM2
                    Case "A47"   'BOTH button
                        CheckBox3.Checked = Not CheckBox3.Checked
                        RadioSwitches.Value(5) = CheckBox3.Checked     'BOTH
                    Case "A48"   'NAV1 button
                        CheckBox4.Checked = Not CheckBox4.Checked
                        RadioSwitches.Value(4) = CheckBox4.Checked     'NAV1
                    Case "A49"   'NAV2 button
                        CheckBox5.Checked = Not CheckBox5.Checked
                        RadioSwitches.Value(3) = CheckBox5.Checked     'NAV2
                    Case "A53"   'MKR button
                        CheckBox6.Checked = Not CheckBox6.Checked
                        RadioSwitches.Value(2) = CheckBox6.Checked     'MKR
                    Case "A50"   'DME button
                        CheckBox7.Checked = Not CheckBox7.Checked
                        RadioSwitches.Value(1) = CheckBox7.Checked     'DME
                    Case "A54"   'GPSNAV button
                        'toggle it, 0 or 1
                        NAVGPSSwitch.Value = IIf(NAVGPSSwitch.Value = 1, 0, 1)
                    Case Else
                        'dont do anything
                End Select

            End If     'end if have data and FSX connected

        End If 'end if serialport1 connected

    End Sub

    Private Sub SerialPort1_ErrorReceived(sender As Object, e As IO.Ports.SerialErrorReceivedEventArgs) Handles SerialPort1.ErrorReceived
        MessageBox.Show("Unhandled Serial port error")
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Try
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Visible = False
                NotifyIcon1.Visible = True
                NotifyIcon1.Text = "SIMCMP1"
                'Don't like notifyicon, but you can turn it on if want
                'NotifyIcon1.ShowBalloonTip(0, "SIMCMP1", "Running Minimized", ToolTipIcon.Info)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Try
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
            'NotifyIcon1.Visible = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        'Windows toolbar toolstrip Exit function
        Me.Close()
    End Sub

    Private Sub StartTimer_Tick(sender As Object, e As EventArgs) Handles StartTimer.Tick
        'Startup timer

        glStartTimerEvents += 1

        'serial opens in form_load (eg tick 0)

        'try and start FSUIPC beginning at 3rd tick
        'give FSUIPC up to 30 seconds to connect (eg starting FSX)
        If glStartTimerEvents > 3 And glStartTimerEvents < 30 Then
            btnFSUIPC_Connect.Text = "Trying (" & glStartTimerEvents & ")..."
            If btnFSUIPC_Connect.Enabled = True Then    'use the button disable state as flag
                OpenFSUIPC()                            'basically trying it every second 
            Else
                'button disabled, assume FSUIPC is connected
                glStartTimerEvents = 30                 'scoot up the timer to make the minimize if happen
                btnFSUIPC_Connect.Text = "Connect"      'put button text back
            End If
        End If

        'Minimize the form when (IF) both serial and FSUIPC are open
        If glStartTimerEvents = 30 Then
            'minimize form only if both serial and FSUIPC opened
            If SerialPort1.IsOpen = True And btnFSUIPC_Connect.Enabled = False Then
                If Me.WindowState = FormWindowState.Normal Then
                    Me.WindowState = FormWindowState.Minimized
                    Me.Visible = False
                End If
            End If
        End If

        'Last tick, shut down the timer firing
        If glStartTimerEvents > 30 Then
            StartTimer.Enabled = False          'turn me off, just do once
            glStartTimerEvents = 0              'reset the counter
        End If

    End Sub

End Class

