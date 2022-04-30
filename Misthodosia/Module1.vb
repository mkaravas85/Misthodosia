Imports MySql.Data.MySqlClient
Module Module1
    Public mysqlcon As MySqlConnection
    Public command As MySqlCommand
    Public reader As MySqlDataReader
    Public queryerror As Boolean = False
    Public constr = "server=192.168.1.10;userid=signet;password=enapass;database=sigmix;port=33953;charset=utf8"
    'Public constr = "server=192.168.55.10;userid=signet2;password=enapass;database=sigmix;port=3306;charset=utf8"
    Public Sub runquery(ByVal query As String)
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr

        Try
            mysqlcon.Open()
            command = New MySqlCommand(query, mysqlcon)
            reader = command.ExecuteReader
            mysqlcon.Close()
        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message, "Παρουσιάστηκε σφάλμα!", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub

End Module
