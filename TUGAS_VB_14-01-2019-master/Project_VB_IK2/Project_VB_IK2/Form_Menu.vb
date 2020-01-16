Public Class Form_Menu

    Private Sub AdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminToolStripMenuItem.Click
        Form_Admin.Show()
        Form_Admin.MdiParent = Me
    End Sub

    Private Sub PenjualanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenjualanToolStripMenuItem.Click
        Form_Penjualan.Show()
        Form_Penjualan.MdiParent = Me
    End Sub

    Private Sub BarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem.Click
        Form_Barang.Show()
        Form_Barang.MdiParent = Me
    End Sub

    Private Sub LaporanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanToolStripMenuItem.Click
        FCRPrint.Show()
        FCRPrint.MdiParent = Me
    End Sub


    Private Sub OpenAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Normal
        Next
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.Close()
        Next
    End Sub

    Private Sub MinimizeAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        End
    End Sub
End Class