Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports System.IO.Compression
Imports System.IO.Packaging
Public Class Form1
    Public case1 As Integer = 0
    Public person As Integer = 0
    Dim id As Integer = 0
    Dim protifora As Boolean = False
    Dim lathosafm As Boolean = False
    Dim count, j As Integer
    Public keno, zero, output As String
    Dim lst2, lst3, lst4 As New List(Of String)()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.BackColor = System.Drawing.Color.CornflowerBlue
        Button2.Enabled = False
        Button3.Enabled = False

    End Sub
    Private Sub toupper(gpb As GroupBox)
        Dim ctl As Control
        For Each ctl In gpb.Controls
            If TypeOf ctl Is TextBox Then
                ctl.Text = ctl.Text.ToUpper
            End If
        Next
    End Sub
    Private Sub checkafm(tb As TextBox)
        If tb.Text.Length <> 9 Or IsNumeric(tb.Text) = False Then
            MessageBox.Show("ΛΑΘΟΣ ΑΦΜ !!!")
            lathosafm = True
            Exit Sub
        Else
            Dim a As Integer = Integer.Parse(tb.Text(0))
            Dim b As Integer = Integer.Parse(tb.Text(1))
            Dim c As Integer = Integer.Parse(tb.Text(2))
            Dim d As Integer = Integer.Parse(tb.Text(3))
            Dim f As Integer = Integer.Parse(tb.Text(4))
            Dim g As Integer = Integer.Parse(tb.Text(5))
            Dim h As Integer = Integer.Parse(tb.Text(6))
            Dim i As Integer = Integer.Parse(tb.Text(7))
            Dim j As Integer = Integer.Parse(tb.Text(8))
            Dim check As Integer = (2 * i + 4 * h + 8 * g + 16 * f + 32 * d + 64 * c + 128 * b + 256 * a) Mod (11)
            If (check <> j) Then
                If ((check <> 10) Or (j <> 0)) Then
                    MessageBox.Show("ΛΑΘΟΣ ΑΦΜ !!!")
                    lathosafm = True
                    Exit Sub
                End If
            End If
        End If
        lathosafm = False
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        checkafm(TextBox1)
        If lathosafm = True Then
            Exit Sub
        End If
        toupper(GroupBox1)
        If Button1.BackColor = Color.CornflowerBlue Then
            If String.IsNullOrWhiteSpace(TextBox1.Text) = False And String.IsNullOrWhiteSpace(TextBox2.Text) = False And String.IsNullOrWhiteSpace(TextBox3.Text) = False Then
                mysqlcon = New MySqlConnection
                'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
                mysqlcon.ConnectionString = constr

                Try
                    mysqlcon.Open()
                    command = New MySqlCommand("SELECT afm FROM pay_person WHERE afm='" & TextBox1.Text & "'", mysqlcon)
                    reader = command.ExecuteReader
                    If reader.HasRows Then
                        MessageBox.Show("Yπάρχει εγγραφή με το ίδιο ΑΦΜ")
                        Exit Sub
                    End If
                    mysqlcon.Close()
                Catch ex As Exception
                    queryerror = True
                    MessageBox.Show(ex.Message)

                End Try
                If mysqlcon.State = ConnectionState.Open Then
                    mysqlcon.Close()
                End If
                If String.IsNullOrWhiteSpace(ComboBox6.Text) Then
                    ComboBox6.SelectedItem = "00"
                End If

                runquery("INSERT INTO sigmix.pay_person (afm, eponumo, onoma, amka, fathername, children) VALUES ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox5.Text & "','" & TextBox4.Text & "','" & ComboBox6.Text & "')")

            Else
                MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")

                Exit Sub
            End If

            If queryerror = False Then
                MessageBox.Show("Επιτυχής Καταχώρηση!")
                Button4.PerformClick()
            End If
            If queryerror = True Then
                queryerror = False
                Exit Sub
            End If
        End If
        If Button2.BackColor = Color.CornflowerBlue Then
            If String.IsNullOrWhiteSpace(TextBox1.Text) = False And String.IsNullOrWhiteSpace(TextBox2.Text) = False And String.IsNullOrWhiteSpace(TextBox3.Text) = False Then
                runquery("UPDATE pay_person SET afm='" & TextBox1.Text & "',eponumo='" & TextBox2.Text & "',onoma='" & TextBox3.Text & "',amka='" & TextBox5.Text & "',fathername='" & TextBox4.Text & "',children='" & ComboBox6.SelectedItem.ToString & "' WHERE person='" & person & "'")
            Else
                MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")

                Exit Sub
            End If

            If queryerror = False Then
                MessageBox.Show("Επιτυχής Καταχώρηση!")
                Button4.PerformClick()
            End If
            If queryerror = True Then
                queryerror = False
                Exit Sub
            End If
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Ctl As Control
        For Each Ctl In TabPage1.Controls
            If TypeOf Ctl Is Button Then
                Ctl.Enabled = True
                Ctl.BackColor = DefaultBackColor
            End If

        Next
        For Each Ctl In GroupBox1.Controls
            If TypeOf Ctl Is TextBox Then
                Ctl.Text = ""
            End If
        Next
        person = 0
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button2.BackColor = Color.CornflowerBlue
        Button1.Enabled = False
        Button3.Enabled = False
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If String.IsNullOrWhiteSpace(TextBox21.Text) = False Then
            case1 = 1
        End If
        If String.IsNullOrWhiteSpace(TextBox22.Text) = False Then
            case1 = 2
        End If
        If String.IsNullOrWhiteSpace(TextBox21.Text) And String.IsNullOrWhiteSpace(TextBox22.Text) Then
            case1 = 5
        End If
        Form2.Show()
    End Sub
    Private Sub frmCustomerDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox21.Focused Or TextBox22.Focused Then
                Button13.PerformClick()
            End If
            If TextBox19.Focused Or TextBox20.Focused Then

                Button12.PerformClick()
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        runquery("DELETE FROM pay_person WHERE person='" & person & "'")
        Button4.PerformClick()
        person = 0
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Button7.BackColor = Color.CornflowerBlue
        Button8.Enabled = False
        Button9.Enabled = False
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox10.ReadOnly = True
        TextBox18.Text = Date.Today.Year
        TextBox23.Text = Date.Today.Year
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr

        Try
            mysqlcon.Open()
            command = New MySqlCommand("SELECT * FROM sigmix.pay_stoixeia", mysqlcon)
            reader = command.ExecuteReader
            If reader.HasRows Then
                While reader.Read

                    TextBox15.Text = reader.Item("afm")
                    TextBox16.Text = reader.Item("activity")
                    TextBox17.Text = reader.Item("eponumia")
                End While
            Else
                protifora = True
            End If
            mysqlcon.Close()
        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message)

        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
        If TextBox15.Text = "" Then
            Button15.Enabled = False
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim Ctl As Control
        For Each Ctl In TabPage2.Controls
            If TypeOf Ctl Is Button Then
                Ctl.Enabled = True
                Ctl.BackColor = DefaultBackColor
            End If

        Next
        For Each Ctl In GroupBox2.Controls
            If TypeOf Ctl Is TextBox Then
                Ctl.Text = ""
            End If
        Next
        ComboBox5.Text = ""
        TextBox24.Text = ""
        TextBox23.ReadOnly = False
        ComboBox3.Enabled = True
        case1 = 0
        TextBox19.Text = ""
        TextBox20.Text = ""
        TextBox7.Text = ""
        id = 0
        person = 0
        TextBox23.Text = Date.Today.Year
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If String.IsNullOrWhiteSpace(ComboBox5.Text) Then

            If String.IsNullOrWhiteSpace(TextBox19.Text) = False Then
                case1 = 3
            End If
            If String.IsNullOrWhiteSpace(TextBox20.Text) = False Then
                case1 = 4
            End If
            If String.IsNullOrWhiteSpace(TextBox20.Text) And String.IsNullOrWhiteSpace(TextBox19.Text) Then
                case1 = 6
            End If
            Form2.ShowDialog()
        Else
            mysqlcon = New MySqlConnection
            'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
            mysqlcon.ConnectionString = constr

            Try
                mysqlcon.Open()
                command = New MySqlCommand("SELECT * FROM pay_econ WHERE person='" & person & "' AND minas='" & ComboBox5.SelectedItem & "' AND etos='" & TextBox24.Text & "'", mysqlcon)
                reader = command.ExecuteReader
                If reader.HasRows Then
                    While reader.Read
                        id = reader.Item("id")
                        TextBox8.Text = reader.Item("akatharistes")
                        TextBox9.Text = reader.Item("kratiseis")
                        TextBox10.Text = reader.Item("kathares")
                        TextBox11.Text = reader.Item("foros")
                        TextBox12.Text = reader.Item("eisfora")
                        TextBox13.Text = reader.Item("xartosimo")
                        TextBox14.Text = reader.Item("oga")
                        ComboBox3.SelectedItem = reader.Item("minas")
                        ComboBox4.SelectedItem = reader.Item("eidos_apod")
                    End While
                    ComboBox3.Enabled = False
                End If

                mysqlcon.Close()
            Catch ex As Exception
                queryerror = True
                MessageBox.Show(ex.Message)

            End Try
            If mysqlcon.State = ConnectionState.Open Then
                mysqlcon.Close()
            End If
        End If
        TextBox8.Focus()

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim akatharistes, kratiseis, kathares, foros, eisfora, xartosimo, oga As String
        If person = 0 Then
            MessageBox.Show("Δεν έχετε επιλέξει φορολογούμενο")
            Exit Sub
        End If
        If Button7.BackColor = Color.CornflowerBlue Then
            If String.IsNullOrWhiteSpace(TextBox8.Text) = False And String.IsNullOrWhiteSpace(TextBox9.Text) = False And String.IsNullOrWhiteSpace(TextBox10.Text) = False And String.IsNullOrWhiteSpace(ComboBox3.SelectedItem) = False And String.IsNullOrWhiteSpace(ComboBox4.SelectedItem) = False Then
                mysqlcon = New MySqlConnection
                'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
                mysqlcon.ConnectionString = constr

                Try
                    mysqlcon.Open()
                    command = New MySqlCommand("SELECT person,minas FROM pay_econ WHERE person='" & person & "' and minas='" & ComboBox3.SelectedItem & "' and etos='" & TextBox23.Text & "'", mysqlcon)
                    reader = command.ExecuteReader
                    If reader.HasRows Then
                        MessageBox.Show("Yπάρχει εγγραφή με το ίδιο ΑΦΜ για τον επιλεγμέμο μήνα.")
                        Exit Sub
                    End If

                    mysqlcon.Close()
                Catch ex As Exception

                    MessageBox.Show(ex.Message)

                End Try
                If mysqlcon.State = ConnectionState.Open Then
                    mysqlcon.Close()
                End If
                If String.IsNullOrWhiteSpace(TextBox11.Text) Then
                    TextBox11.Text = 0.00
                End If
                If String.IsNullOrWhiteSpace(TextBox12.Text) Then
                    TextBox12.Text = 0.00
                End If
                If String.IsNullOrWhiteSpace(TextBox13.Text) Then
                    TextBox13.Text = 0.00
                End If
                If String.IsNullOrWhiteSpace(TextBox14.Text) Then
                    TextBox14.Text = 0.00
                End If
                TextBox10.Text = TextBox8.Text - TextBox9.Text
                todecimalvalue(TextBox8, akatharistes)
                todecimalvalue(TextBox9, kratiseis)
                todecimalvalue(TextBox10, kathares)
                todecimalvalue(TextBox11, foros)
                todecimalvalue(TextBox12, eisfora)
                todecimalvalue(TextBox13, xartosimo)
                todecimalvalue(TextBox14, oga)

                runquery("INSERT INTO sigmix.pay_econ (person, minas, akatharistes, kratiseis, kathares, eidos_apod, foros, eisfora,xartosimo,oga,etos) VALUES ('" & person & "','" & ComboBox3.SelectedItem.ToString & "','" & akatharistes & "','" & kratiseis & "','" & kathares & "','" & ComboBox4.SelectedItem.ToString & "','" & foros & "','" & eisfora & "','" & xartosimo & "','" & oga & "','" & TextBox23.Text & "')")
                If queryerror = False Then
                    MessageBox.Show("Επιτυχής Καταχώρηση!")
                    Button10.PerformClick()
                End If
                If queryerror = True Then
                    queryerror = False
                    Exit Sub
                End If

            Else
                MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")

                Exit Sub
            End If

        End If
        If Button8.BackColor = Color.CornflowerBlue Then
            TextBox23.ReadOnly = False
            ComboBox3.Enabled = True
            TextBox10.Text = TextBox8.Text - TextBox9.Text
            todecimalvalue(TextBox8, akatharistes)
            todecimalvalue(TextBox9, kratiseis)
            todecimalvalue(TextBox10, kathares)
            todecimalvalue(TextBox11, foros)
            todecimalvalue(TextBox12, eisfora)
            todecimalvalue(TextBox13, xartosimo)
            todecimalvalue(TextBox14, oga)
            If String.IsNullOrWhiteSpace(TextBox8.Text) = False And String.IsNullOrWhiteSpace(TextBox9.Text) = False And String.IsNullOrWhiteSpace(TextBox10.Text) = False And String.IsNullOrWhiteSpace(ComboBox3.SelectedItem) = False And String.IsNullOrWhiteSpace(ComboBox4.SelectedItem) = False Then
                runquery("UPDATE sigmix.pay_econ SET akatharistes='" & akatharistes & "',kratiseis='" & kratiseis & "',kathares='" & kathares & "',eidos_apod='" & ComboBox4.SelectedItem.ToString & "',foros='" & foros & "',eisfora='" & eisfora & "',xartosimo='" & xartosimo & "',oga='" & oga & "' WHERE id='" & id & "'")
                If queryerror = False Then
                    MessageBox.Show("Επιτυχής Καταχώρηση!")
                    Button10.PerformClick()
                End If
                If queryerror = True Then
                    queryerror = False
                    Exit Sub
                End If
            Else
                MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")

                Exit Sub
            End If

        End If

    End Sub
    Private Sub todecimalvalue(tb As TextBox, ByRef ex As String)
        If tb.Text.Contains(",") Then
            ex = tb.Text.Replace(",", ".")
        Else
            ex = tb.Text
        End If

    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        TextBox24.Text = Date.Now.Year
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        TextBox23.ReadOnly = True
        Button8.BackColor = Color.CornflowerBlue
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        If String.IsNullOrWhiteSpace(TextBox15.Text) Or String.IsNullOrWhiteSpace(TextBox16.Text) Or String.IsNullOrWhiteSpace(TextBox17.Text) Then
            MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")
            Exit Sub
        End If

        checkafm(TextBox15)
        If lathosafm = True Then
            Exit Sub
        End If
        toupper(GroupBox6)
        If Button15.Enabled = False Then
            runquery("INSERT INTO sigmix.pay_stoixeia (afm,eponumia,activity) VALUES ('" & TextBox15.Text & "', '" & TextBox17.Text & "','" & TextBox16.Text & "')")
            protifora = False
        ElseIf Button15.BackColor = Color.CornflowerBlue Then
            runquery("UPDATE sigmix.pay_stoixeia SET afm='" & TextBox15.Text & "', eponumia='" & TextBox17.Text & "', activity='" & TextBox16.Text & "'")
        End If
        If queryerror = False Then
            MessageBox.Show("Επιτυχής Καταχώρηση!")
            Button10.PerformClick()
        End If
        If queryerror = True Then
            queryerror = False
            Exit Sub
        End If
        Button15.Enabled = True
        Button15.BackColor = DefaultBackColor
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Button15.BackColor = Color.CornflowerBlue
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If String.IsNullOrWhiteSpace(TextBox18.Text) Or String.IsNullOrWhiteSpace(ComboBox2.SelectedItem) Then
            MessageBox.Show("Υπάρχουν ασυμπλήρωτα υποχρεωτικά πεδία")
            Exit Sub
        End If
        Try
            FolderBrowserDialog1.Description = "Επιλέξτε φάκελο προορισμού"
            FolderBrowserDialog1.ShowDialog()

            Dim path As String = FolderBrowserDialog1.SelectedPath

            Dim day1 As DateTime = Today 'DateTimePicker1.Value
            Dim Day As String = "yyyyMMdd"

            Dim lst1 As New List(Of String)()

            lst1.Add("0JL10    ")
            'Dim day1 As String = DateTimePicker1.Value.ToString.da
            lst1.Add(day1.ToString(Day))
            lst1.Add(TextBox18.Text)

            gapfill(127, 0)
            lst1.Add(keno)
            Dim win1253 As System.Text.Encoding
            win1253 = System.Text.Encoding.GetEncoding(1253)

            Using zipToOpen As FileStream = New FileStream(path & "\JL10.zip", FileMode.Create)
                Using archive As ZipArchive = New ZipArchive(zipToOpen, ZipArchiveMode.Create)
                    Dim readmeEntry As ZipArchiveEntry = archive.CreateEntry("JL10.asc")
                    Using writer As StreamWriter = New StreamWriter(readmeEntry.Open(), win1253)
                        'file1 = My.Computer.FileSystem.OpenTextFileWriter(path & "\JL10.asc", False, win1253)
                        writer.WriteLine(lst1(0) & lst1(1) & lst1(2) & lst1(3)) '& vbCrLf)
                        fetchcompany()
                        Dim eponumia As String

                        gapfill(30, lst2(3).Length)
                        eponumia = lst2(3) & keno ' SOS ΕΔΩ ΕΧΩ ΑΦΗΣΕΙ ΚΕΝΑ ΚΑΙ ΓΙΑ ΤΑ ΠΕΔΙΑ ΟΝΟΜΑ ΠΑΤΡΩΝΥΜΟ
                        Dim activity As String
                        gapfill(47, lst2(4).Length) ' SOS ΕΔΩ ΕΧΩ ΑΦΗΣΕΙ ΚΕΝΑ ΚΑΙ ΓΙΑ ΤΑ ΠΕΔΙΑ ΠΟΛΗ,ΟΔΟΣ,ΑΡΙΘΜΟΣ. TO ΤΚ EINAI TA 5 MHDENIKA PARAKATO
                        activity = lst2(4) & keno
                        gapfill(49, 0)
                        writer.WriteLine(lst2(0) & lst2(1) & eponumia & "0" & lst2(2) & activity & "00000" & ComboBox2.SelectedItem.ToString & keno)
                        fetchsums()
                        Dim akatharistes, kratiseis, kathares, foros, eisfora, xartosimo, oga As String
                        lst3(1) = lst3(1).Replace(",", "")
                        lst3(1) = lst3(1).Replace(".", "")
                        zerofill(16, lst3(1).Length)
                        akatharistes = zero & lst3(1)

                        lst3(2) = lst3(2).Replace(",", "")
                        lst3(2) = lst3(2).Replace(".", "")
                        zerofill(16, lst3(2).Length)
                        kratiseis = zero & lst3(2)

                        lst3(3) = lst3(3).Replace(",", "")
                        lst3(3) = lst3(3).Replace(".", "")
                        zerofill(16, lst3(3).Length)
                        kathares = zero & lst3(3)

                        lst3(4) = lst3(4).Replace(",", "")
                        lst3(4) = lst3(4).Replace(".", "")
                        zerofill(15, lst3(4).Length)
                        foros = zero & lst3(4)

                        lst3(5) = lst3(5).Replace(",", "")
                        lst3(5) = lst3(5).Replace(".", "")
                        zerofill(15, lst3(5).Length)
                        eisfora = zero & lst3(5)

                        lst3(6) = lst3(6).Replace(",", "")
                        lst3(6) = lst3(6).Replace(".", "")
                        zerofill(14, lst3(6).Length)
                        xartosimo = zero & lst3(6)

                        lst3(7) = lst3(7).Replace(",", "")
                        lst3(7) = lst3(7).Replace(".", "")
                        zerofill(13, lst3(7).Length)
                        oga = zero & lst3(7)
                        Dim filler As String = "000000000000000"
                        gapfill(27, 0)
                        writer.WriteLine(lst3(0) & akatharistes & kratiseis & kathares & filler & foros & eisfora & xartosimo & oga & keno)

                        Dim eponumo, onoma, fathername, amka, children, eidos_apod, akatharistes1, kratiseis1, kathares1, foros1, eisfora1, xartosimo1, oga1 As String
                        mysqlcon = New MySqlConnection
                        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
                        mysqlcon.ConnectionString = constr
                        Dim sda As New MySqlDataAdapter
                        Dim dt As New DataTable
                        Dim bs As New BindingSource
                        Try
                            mysqlcon.Open()
                            command = New MySqlCommand("SELECT afm,eponumo,onoma,fathername,amka,children,pay_econ.eidos_apod,pay_econ.akatharistes,pay_econ.kratiseis,pay_econ.kathares,pay_econ.foros,pay_econ.eisfora,pay_econ.xartosimo,pay_econ.oga FROM sigmix.pay_person JOIN sigmix.pay_econ ON pay_econ.person=pay_person.person WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
                            sda.SelectCommand = command
                            sda.Fill(dt)
                            bs.DataSource = dt

                            sda.Update(dt)

                            mysqlcon.Close()
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try
                        If mysqlcon.State = ConnectionState.Open Then
                            mysqlcon.Close()
                        End If
                        For i = 0 To dt.Rows.Count - 1
                            Dim lst5 As New List(Of String)
                            lst5.Add("3")
                            lst5.Add(dt.Rows(i).Item(0))
                            lst5.Add(" ")

                            eponumo = dt.Rows(i).Item(1)
                            gapfill(18, eponumo.Length)
                            eponumo = eponumo & keno
                            lst5.Add(eponumo)

                            onoma = dt.Rows(i).Item(2)
                            gapfill(9, onoma.Length)
                            onoma = onoma & keno
                            lst5.Add(onoma)

                            fathername = dt.Rows(i).Item(3)
                            gapfill(3, fathername.Length)
                            fathername = fathername & keno
                            lst5.Add(fathername)

                            amka = dt.Rows(i).Item(4)
                            gapfill(11, amka.Length)
                            amka = amka & keno
                            lst5.Add(amka)

                            lst5.Add(dt.Rows(i).Item(5))
                            lst5.Add(dt.Rows(i).Item(6))

                            akatharistes1 = dt.Rows(i).Item(7)
                            akatharistes1 = akatharistes1.Replace(",", "")
                            akatharistes1 = akatharistes1.Replace(".", "")
                            zerofill(11, akatharistes1.Length)
                            akatharistes1 = zero & akatharistes1
                            lst5.Add(akatharistes1)

                            kratiseis1 = dt.Rows(i).Item(8)
                            kratiseis1 = kratiseis1.Replace(",", "")
                            kratiseis1 = kratiseis1.Replace(".", "")
                            zerofill(10, kratiseis1.Length)
                            kratiseis1 = zero & kratiseis1
                            lst5.Add(kratiseis1)

                            kathares1 = dt.Rows(i).Item(9)
                            kathares1 = kathares1.Replace(",", "")
                            kathares1 = kathares1.Replace(".", "")
                            zerofill(11, kathares1.Length)
                            kathares1 = zero & kathares1
                            lst5.Add(kathares1)

                            lst5.Add("0") ' EΝΔΕΙΞΗ ΑΛΛΟΔΑΠΟΥ Η ΜΗ
                            lst5.Add("  ")
                            lst5.Add("0000000")   'filler + forologikos suntelestis

                            foros1 = dt.Rows(i).Item(10)
                            foros1 = foros1.Replace(",", "")
                            foros1 = foros1.Replace(".", "")
                            zerofill(10, foros1.Length)
                            foros1 = zero & foros1
                            lst5.Add(foros1)

                            eisfora1 = dt.Rows(i).Item(11)
                            eisfora1 = eisfora1.Replace(",", "")
                            eisfora1 = eisfora1.Replace(".", "")
                            zerofill(10, eisfora1.Length)
                            eisfora1 = zero & eisfora1
                            lst5.Add(eisfora1)

                            xartosimo1 = dt.Rows(i).Item(12)
                            xartosimo1 = xartosimo1.Replace(",", "")
                            xartosimo1 = xartosimo1.Replace(".", "")
                            zerofill(9, xartosimo1.Length)
                            xartosimo1 = zero & xartosimo1
                            lst5.Add(xartosimo1)

                            oga1 = dt.Rows(i).Item(13)
                            oga1 = oga1.Replace(",", "")
                            oga1 = oga1.Replace(".", "")
                            zerofill(8, oga1.Length)
                            oga1 = zero & oga1
                            lst5.Add(oga1)

                            lst5.Add("00000000/0000")
                            output = (lst5(0) & lst5(1) & lst5(2) & lst5(3) & lst5(4) & lst5(5) & lst5(6) & lst5(7) & lst5(8) & lst5(9) & lst5(10) & lst5(11) & lst5(12) & lst5(13) & lst5(14) & lst5(15) & lst5(16) & lst5(17) & lst5(18) & lst5(19))

                            writer.WriteLine(output)
                        Next
                    End Using
                End Using
            End Using
            ' file1.Close()
            lst3.Clear()
            'Dim filetocompress As New FileInfo(path & "\JL10.asc")
            'Using originalFileStream As FileStream = filetocompress.OpenRead()

            '    Using compressedFileStream As FileStream = File.Create(path & "\JL10.ZIP")
            '        Using compressionStream As New GZipStream(compressedFileStream, CompressionMode.Compress)

            '            originalFileStream.CopyTo(compressionStream)
            '        End Using
            '    End Using
            'End Using
            MessageBox.Show("Το αρχείο δημιουργήθηκε")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If protifora = False Then
            Button6.Enabled = True
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        runquery("DELETE FROM pay_econ where id='" & id & "'")
        If queryerror = False Then
            MessageBox.Show("Επιτυχής διαγραφή")
        End If
        Button10.PerformClick()
        id = 0
    End Sub
    Private Sub TextBox9_Leave(sender As Object, e As EventArgs) Handles TextBox9.Leave
        If String.IsNullOrWhiteSpace(TextBox8.Text) = False And String.IsNullOrWhiteSpace(TextBox9.Text) = False Then

            TextBox10.Text = TextBox8.Text - TextBox9.Text
        End If
    End Sub
    Private Sub fetchcompany()
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr
        Try
            mysqlcon.Open()
            command = New MySqlCommand("SELECT * FROM sigmix.pay_stoixeia", mysqlcon)
            reader = command.ExecuteReader
            lst2.Add("1")
            lst2.Add(TextBox23.Text)
            While reader.Read
                lst2.Add(reader.GetValue(1))
                lst2.Add(reader.GetValue(2))
                lst2.Add(reader.GetValue(3))
            End While

            mysqlcon.Close()
        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message)

        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
    Private Sub gapfill(fieldlength As Integer, actuallength As Integer)
        keno = ""
        For i = 1 To fieldlength - actuallength
            keno = keno & " "

        Next
    End Sub

    Private Sub fetchsums()
        mysqlcon = New MySqlConnection
        mysqlcon.ConnectionString = constr

        Try
            mysqlcon.Open()
            command = New MySqlCommand("SELECT SUM(akatharistes) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            lst3.Add("2")

            While reader.Read
                lst3.Add(reader.GetValue(0))
            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(kratiseis) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(kathares) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(foros) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(eisfora) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(xartosimo) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()

            command = New MySqlCommand("SELECT SUM(oga) FROM sigmix.pay_econ WHERE minas='" & ComboBox2.SelectedItem & "' AND etos='" & TextBox18.Text & "'", mysqlcon)
            reader = command.ExecuteReader
            While reader.Read
                lst3.Add(reader.GetValue(0))

            End While
            reader.Close()
            mysqlcon.Close()
        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message)

        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
    Private Sub zerofill(fieldlength As Integer, actuallength As Integer)
        zero = ""
        For i = 1 To fieldlength - actuallength
            zero = zero & "0"

        Next
    End Sub
    End Class
