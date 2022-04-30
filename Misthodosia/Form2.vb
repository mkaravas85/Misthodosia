Imports MySql.Data.MySqlClient

Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.case1 = 1 Then
            loadgrid("select * from pay_person where afm like '%" & Form1.TextBox21.Text & "%'")
        End If
        If Form1.case1 = 2 Then
            loadgrid("select * from pay_person where eponumo like '%" & Form1.TextBox22.Text & "%'")
        End If

        If Form1.case1 = 3 Then
            loadgrid("select * from pay_person where afm like '%" & Form1.TextBox19.Text & "%'")
        End If
        If Form1.case1 = 4 Then
            loadgrid("select * from pay_person where eponumo like '%" & Form1.TextBox20.Text & "%'")
        End If
        If Form1.case1 = 5 Or Form1.case1 = 6 Then
            loadgrid("select * from pay_person")
        End If
        If DataGridView1.RowCount = 2 Then
            Button1.PerformClick()

            Exit Sub

        End If
        If DataGridView1.RowCount = 1 Then
            MessageBox.Show("Δε βρέθηκαν στοιχεία")
            Me.Close()
        End If
        DataGridView1.Columns(3).Width = 167
        DataGridView1.Columns(3).HeaderText = "Ονομα"
        DataGridView1.Columns(2).HeaderText = "Επώνυμο"
        DataGridView1.Columns(1).HeaderText = "ΑΦΜ"
        DataGridView1.Columns(4).Visible = False
        DataGridView1.Columns(5).Visible = False

        DataGridView1.Columns(6).Visible = False

        DataGridView1.Columns(0).Visible = False
        Me.Focus()
    End Sub
    Public Sub loadgrid(ByVal query As String)
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr
        Dim sda As New MySqlDataAdapter
        Dim dt As New DataTable
        Dim bs As New BindingSource
        Try
            mysqlcon.Open()
            command = New MySqlCommand(query, mysqlcon)
            sda.SelectCommand = command
            sda.Fill(dt)
            bs.DataSource = dt
            DataGridView1.DataSource = bs
            sda.Update(dt)
            mysqlcon.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Button1.PerformClick()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Form1.case1 = 1 Or Form1.case1 = 2 And DataGridView1.Rows.Count = 2 Then
            Form1.TextBox1.Text = DataGridView1.Rows(0).Cells(1).Value.ToString
            Form1.person = DataGridView1.Rows(0).Cells(0).Value
            Form1.TextBox2.Text = DataGridView1.Rows(0).Cells(2).Value.ToString
            Form1.TextBox3.Text = DataGridView1.Rows(0).Cells(3).Value.ToString
            Form1.TextBox5.Text = DataGridView1.Rows(0).Cells(4).Value.ToString
            Form1.TextBox4.Text = DataGridView1.Rows(0).Cells(5).Value.ToString
            Form1.ComboBox6.Text = DataGridView1.Rows(0).Cells(6).Value.ToString
        End If
        If Form1.case1 = 1 Or Form1.case1 = 2 And DataGridView1.Rows.Count > 2 Then
            Form1.case1 = 5
        End If
        If Form1.case1 = 3 Or Form1.case1 = 4 And DataGridView1.Rows.Count = 2 Then
            Form1.person = DataGridView1.Rows(0).Cells(0).Value
            Form1.TextBox7.Text = DataGridView1.Rows(0).Cells(2).Value.ToString & " " & DataGridView1.Rows(0).Cells(3).Value.ToString
        End If
        If Form1.case1 = 3 Or Form1.case1 = 4 And DataGridView1.Rows.Count > 2 Then
            Form1.case1 = 6
        End If
        If Form1.case1 = 5 Then
            Form1.TextBox1.Text = DataGridView1.Rows(0).Cells(1).Value.ToString
            Form1.person = DataGridView1.CurrentRow.Cells(0).Value
            Form1.TextBox2.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString
            Form1.TextBox3.Text = DataGridView1.CurrentRow.Cells(3).Value.ToString
            Form1.TextBox5.Text = DataGridView1.CurrentRow.Cells(4).Value.ToString
            Form1.TextBox4.Text = DataGridView1.CurrentRow.Cells(5).Value.ToString
            Form1.ComboBox6.Text = DataGridView1.CurrentRow.Cells(6).Value.ToString
        End If
        If Form1.case1 = 6 Then
            Form1.TextBox7.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString & " " & DataGridView1.CurrentRow.Cells(3).Value.ToString
            Form1.person = DataGridView1.CurrentRow.Cells(0).Value
        End If

        Form1.case1 = 0
        Me.Dispose()
        Me.Close()
    End Sub
End Class