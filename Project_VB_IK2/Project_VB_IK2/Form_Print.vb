Imports CrystalDecisions.Shared

Public Class FCRPrint

    Private Sub FCRPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sql As String
        Dim dtDari, dtSampai As DataTable

        cbx_dari.DataSource = Nothing : cbx_dari.Items.Clear()
        cbx_sampai.DataSource = Nothing : cbx_sampai.Items.Clear()

        If ComboBox1.SelectedItem = "Barang" Then
            sql = "select id_barang, nama_barang from barang order by id_barang asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "id_barang"
                .DisplayMember = "nama_barang"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "id_barang"
                .DisplayMember = "nama_barang"
            End With
        ElseIf ComboBox1.SelectedItem = "Admin" Then
            sql = "select username, nama from admin order by username asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "username"
                .DisplayMember = "nama"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "username"
                .DisplayMember = "nama"
            End With
        ElseIf ComboBox1.SelectedItem = "Penjualan" Then
            sql = "select id_penjualan, no_transaksi from penjualan order by id_penjualan asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "id_penjualan"
                .DisplayMember = "no_transaksi"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "id_penjualan"
                .DisplayMember = "no_transaksi"
            End With
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dari, sampai As String
        Dim lokasiDatabase As String = Application.StartupPath() & "\DB.mdb"
        Dim Eng As CrystalDecisions.CrystalReports.Engine.Table
        Dim DbInfo As TableLogOnInfo

        dari = cbx_dari.SelectedValue.ToString
        sampai = cbx_sampai.SelectedValue.ToString

        If ComboBox1.SelectedItem = "Barang" Then
            Dim report As New CRBarang
            ''Dim xdari() As String = cbx_dari.SelectedItem.ToString.Split(" - ")
            ''Dim xsampai() As String = cbx_sampai.SelectedItem.ToString.Split(" - ")
            ''Dim dari1 As String = xdari(0)
            ''Dim sampai1 As String = xsampai(0)

            report.RecordSelectionFormula = "{barang.id_barang} >= " &
                cbx_dari.SelectedValue.ToString & " and {barang.id_barang} <= " & cbx_sampai.SelectedValue.ToString

            'set lokasi database

            DbInfo = New TableLogOnInfo
            For Each Eng In report.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            CrystalReportViewer1.ReportSource = report
            CrystalReportViewer1.RefreshReport()

        ElseIf ComboBox1.SelectedItem = "Admin" Then
            Dim report1 As New CRAdmin
            ''Dim xdari() As String = cbx_dari.SelectedItem.ToString.Split(" - ")
            ''Dim xsampai() As String = cbx_sampai.SelectedItem.ToString.Split(" - ")
            ''Dim dari1 As String = xdari(0)
            ''Dim sampai1 As String = xsampai(0)

            report1.RecordSelectionFormula = "{admin.username} >= '" &
                cbx_dari.SelectedValue.ToString & "' and {admin.username} <= '" & cbx_sampai.SelectedValue.ToString & "'"

            DbInfo = New TableLogOnInfo
            For Each Eng In report1.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            CrystalReportViewer1.ReportSource = report1
            CrystalReportViewer1.RefreshReport()


        ElseIf ComboBox1.SelectedItem = "Penjualan" Then
            Dim report1 As New CRPenjualan
            ''Dim xdari() As String = cbx_dari.SelectedItem.ToString.Split(" - ")
            ''Dim xsampai() As String = cbx_sampai.SelectedItem.ToString.Split(" - ")
            ''Dim dari1 As String = xdari(0)
            ''Dim sampai1 As String = xsampai(0)

            report1.RecordSelectionFormula = "{penjualan.id_penjualan} >= " &
                cbx_dari.SelectedValue.ToString & " and {penjualan.id_penjualan} <= " & cbx_sampai.SelectedValue.ToString

            DbInfo = New TableLogOnInfo
            For Each Eng In report1.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            CrystalReportViewer1.ReportSource = report1
            CrystalReportViewer1.RefreshReport()
        End If

    End Sub
End Class