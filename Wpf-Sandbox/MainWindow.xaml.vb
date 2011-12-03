Class MainWindow 

    Private LastClick As DateTime = DateTime.MinValue

    Private Sub DefaultButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles DefaultButton.Click
        Dim btn As Button = sender
        Dim now As DateTime = DateTime.Now
        Dim diff As TimeSpan = TimeSpan.Zero
        If LastClick <> DateTime.MinValue Then
            diff = (now - LastClick)
        End If
        LastClick = now
        Dim msg = String.Format("{1} Elapsed: {2}", btn.Name, now, diff)
        Trace.WriteLine(msg)
    End Sub

End Class
