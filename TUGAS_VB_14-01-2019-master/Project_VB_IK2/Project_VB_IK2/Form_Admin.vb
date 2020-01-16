Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Form_Admin
    Dim tempusername As String
    Dim lst As ListViewItem

    Private Sub kosong()
        tempusername = Nothing
        TextBox1.Text = Nothing 'untuk menghilangkan teks ketika tombol tambah dipencet
        TextBox2.Text = Nothing
        TextBox3.Text = Nothing
        ComboBox1.SelectedIndex = 0
        CheckBox1.Checked = False

    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)
        Dim sql As String

        If cari = Nothing Then 'untuk mencari data
            sql = "select * from admin"
        Else
            sql = "select * from admin " & _
                    "where username like '%" & cari & "%' or nama like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ListView1.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3)) 'untuk memnculkan teks ketika listview di klik 2 kali
            lst.SubItems.Add(dr(4))



        Next
        tempusername = Nothing
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub Form_Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql, pass As String
        'untuk menyimpan data

        If TextBox2.Text.Trim.Length > 0 Then
            Using md5Hash As MD5 = MD5.Create()
                pass = MHash.GetMd5Hash(md5Hash, TextBox2.Text.Trim)

                If MHash.VerifyMd5Hash(md5Hash, TextBox2.Text.Trim, pass) Then
                    Console.WriteLine("Hash sama")
                Else
                    Console.WriteLine("Hash berbeda")
                End If
            End Using
        Else
            pass = Nothing
        End If

        If (tempusername <> Nothing) Then
            If pass = Nothing Then
                sql = "update admin set nama = '" & TextBox3.Text.Trim & "', status = '" & ComboBox1.Text.Trim & "', aktif = '" & CheckBox1.CheckState & "' " & _
                "where username = '" & tempusername & "'"
            Else
                sql = "update admin set pwd = '" & pass & "', nama = '" & TextBox3.Text.Trim & "', status = '" & ComboBox1.Text.Trim & "', aktif = '" & CheckBox1.CheckState & "' " & _
                "where username = '" & tempusername & "'"

            End If
            
        Else
            sql = "insert into admin (username, pwd, nama, status, aktif) " & _
                "values ('" & TextBox1.Text.Trim & "' , '" & pass & "', " & _
                "'" & TextBox3.Text.Trim & "', '" & ComboBox1.Text.Trim & "', '" & CheckBox1.CheckState & "')"
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MsgBox("Hapus data " & tempusername & "?", vbYesNo, "Perhatian") = MsgBoxResult.No Then Exit Sub

        If (tempusername = Nothing) Then
            MsgBox("Tak ada data yang akan dihapus", vbOKOnly, "Perhatian")
            Exit Sub
        End If

        sql = "delete from admin where username = '" & TextBox1.Text().Trim & "'"

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1

            tempusername = .SelectedItems.Item(0).Text 'untuk memnculkan teks ketika listview di klik 2 kali
            TextBox1.Text = tempusername : TextBox1.Enabled = False
            TextBox2.Text = Nothing
            TextBox3.Text = .SelectedItems.Item(0).SubItems(2).Text
            ComboBox1.Text = .SelectedItems.Item(0).SubItems(3).Text
            CheckBox1.Checked = .SelectedItems.Item(0).SubItems(4).Text

        End With

        MsgBox("Passwordmu telah terenkripsi dengan baik",
              vbOKOnly + vbInformation,
              "Jangan mengisi text password, jika tak ingin mengubah")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call loadGrid(TextBox6.Text.Trim)
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
    End Sub
End Class