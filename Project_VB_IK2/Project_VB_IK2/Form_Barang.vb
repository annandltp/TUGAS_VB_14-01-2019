Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Form_Barang
    Dim tempID As Integer
    Dim lst As ListViewItem

    Private Sub kosong()
        tempID = Nothing
        TextBox1.Text = Nothing 'untuk menghilangkan teks ketika tombol tambah dipencet
        TextBox2.Text = Nothing
        ComboBox1.SelectedIndex = 0
        TextBox3.Text = Nothing
        TextBox4.Text = Nothing
        TextBox5.Text = Nothing
    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)
        Dim sql As String

        If cari = Nothing Then 'untuk mencari data
            sql = "select * from barang"
        Else
            sql = "select * from barang " & _
                    "where nama_barang like '%" & cari & "%' or kode_barang like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ListView1.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3)) 'untuk memnculkan teks ketika listview di klik 2 kali
            lst.SubItems.Add(dr(4))
            lst.SubItems.Add(dr(5))
            lst.SubItems.Add(dr(6))

        Next
        tempID = Nothing
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub Form_Barang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String
        'untuk menyimpan data
        If tempID = 0 Then
            sql = "insert into barang (kode_barang, nama_barang, satuan, harga_beli, stock, harga_jual) " & _
            "values ('" & TextBox1.Text.Trim & "' , '" & TextBox2.Text.Trim & "', " & _
            "'" & ComboBox1.Text.Trim & "', '" & TextBox3.Text.Trim & "', '" & TextBox4.Text.Trim & "', '" & TextBox5.Text.Trim & "')"
        Else
            sql = "update barang set kode_barang = '" & TextBox1.Text.Trim & "', nama_barang = '" & TextBox2.Text.Trim & "' , " & _
                "satuan = '" & ComboBox1.Text.Trim & "', " & _
                "harga_beli = '" & TextBox3.Text.Trim & "', stock = '" & TextBox4.Text.Trim & "', harga_jual = '" & TextBox5.Text.Trim & "'" & _
                "where id_barang = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = "delete from barang where id_barang = " & tempID  'jika tidak menggunakan id maka arahkan ke textbox
        'untuk menghapus data
        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1

            tempID = .SelectedItems.Item(0).Text
            TextBox1.Text = .SelectedItems.Item(0).SubItems(1).Text 'untuk memnculkan teks ketika listview di klik 2 kali
            TextBox2.Text = .SelectedItems.Item(0).SubItems(2).Text
            ComboBox1.Text = .SelectedItems.Item(0).SubItems(3).Text
            TextBox3.Text = .SelectedItems.Item(0).SubItems(4).Text
            TextBox4.Text = .SelectedItems.Item(0).SubItems(5).Text
            TextBox5.Text = .SelectedItems.Item(0).SubItems(6).Text

        End With

    End Sub





    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call loadGrid(TextBox6.Text.Trim)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class