Imports System.Threading.Tasks

Public Class Form_Penjualan
    Dim dtBarang As DataTable
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
            sql = "select * from penjualan"
        Else
            sql = "select * from penjualan " & _
                    "where nama_barang like '%" & cari & "%'"
        End If

        MKoneksi.open()
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

        If (ProgressBar1.Visible = True) Then MProgress.hideProgress(ProgressBar1)
    End Function

    Async Function loadBarang() As Threading.Tasks.Task

        MProgress.showProgress(ProgressBar1)

        Dim sql As String = "select id_barang, kode_barang, nama_barang, harga_beli from barang" 'tampilan di textboxt

        'ambil data table
        dtBarang = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ComboBox1.DataSource = dtBarang
        ComboBox1.DisplayMember = "nama_barang"
        ComboBox1.ValueMember = "id_barang"

        ComboBox1_SelectedIndexChanged(Nothing, Nothing)


        tempID = Nothing
        If (ProgressBar1.Visible = True) Then MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sid As String = ComboBox1.SelectedValue.ToString

        'ambil alamat yang tertapung pada datatable 
        Try
            Dim row As DataRow() = dtBarang.Select("id_barang = " & sid)

            TextBox2.Text = row(0)(1)  'vbCrLf = enter
            TextBox3.Text = row(0)(3)
        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try
    End Sub

    Private Sub Form_Penjualan_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        MKoneksi.open()
        Call loadBarang()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String
        'untuk menyimpan data
        If tempID = 0 Then
            sql = "insert into penjualan (no_transaksi, kode_barang, nama_barang, harga, jumlah, sub_total, tgl) " & _
            "values ('" & TextBox1.Text.Trim & "' , '" & TextBox2.Text.Trim & "', " & _
            "'" & ComboBox1.Text.Trim & "', " & TextBox3.Text.Trim & ", " & TextBox4.Text.Trim & ", " & TextBox5.Text.Trim & ", '" & DateTimePicker1.Value & "')"
        Else
            sql = "update penjualan set no_transaksi = '" & TextBox1.Text.Trim & "', kode_barang = '" & TextBox2.Text.Trim & "'," & _
                "nama_barang = '" & ComboBox1.Text.Trim & "', harga = " & TextBox3.Text.Trim & ", jumlah = " & TextBox4.Text.Trim & ", sub_total = " & TextBox5.Text.Trim & ", tgl = '" & DateTimePicker1.Value & "' " & _
                "where id_penjualan = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        tempID = Nothing
        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = "delete from penjualan where no_transaksi = '" & TextBox1.Text.Trim & "'" 'jika tidak menggunakan id maka arahkan ke textbox
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

    Sub hitung()
        TextBox5.Text = Val(TextBox3.Text.Trim) * Val(TextBox4.Text.Trim) 'untuk membuat penjumlahan
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        hitung()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
End Class
