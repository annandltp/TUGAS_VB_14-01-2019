Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Form_Login

    Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Length = 0 Or TextBox2.Text.Length = 0 Then Exit Sub

        Dim pass As String
        If TextBox2.Text.Trim.Length > 0 Then
            Using md5Hash As MD5 = MD5.Create()
                pass = MHash.GetMd5Hash(md5Hash, TextBox2.Text.Trim) 'memubat password enskripsi

                If MHash.VerifyMd5Hash(md5Hash, TextBox2.Text.Trim, pass) Then
                    Console.WriteLine("Hash sama")
                Else
                    Console.WriteLine("Hash berbeda")
                End If
            End Using
        Else
            pass = Nothing
        End If

        sql = "select username, pwd from admin where username = '" & TextBox1.Text.Trim & "' and " & _
                "pwd = '" & pass & "'" 'memanggil database untuk login

        MProgress.showProgress(ProgressBar1)
        MKoneksi.open()
        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))
        MProgress.hideProgress(ProgressBar1)

        If dt.Rows.Count > 0 Then
            Form_Menu.Show() 'jika benar maka akan pindah ke FMenu
            Me.Hide()
        Else
            MsgBox("Data tak ditemukan", vbOKOnly + vbExclamation, "Pesan") 'menampilkan pesan error
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
    End Sub

    Private Sub FLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class