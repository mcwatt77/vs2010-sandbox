Class Application
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Dim logger As New TextWriterTraceListener("C:\AppLogs\application.log")

    Private Sub Application_DispatcherUnhandledException(sender As Object, e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        logger.WriteLine(e.Exception.Message)
    End Sub

    Private Sub Application_Startup(sender As Object, e As System.Windows.StartupEventArgs) Handles Me.Startup
        logger.WriteLine("Application Start")
        Trace.AutoFlush = True
        Trace.Listeners.Add(logger)
    End Sub

    Private Sub Application_Exit(sender As Object, e As System.Windows.ExitEventArgs) Handles Me.Exit
        logger.WriteLine("Application Exit")
        logger.Close()
        Trace.Listeners.Remove(logger)
    End Sub


End Class
