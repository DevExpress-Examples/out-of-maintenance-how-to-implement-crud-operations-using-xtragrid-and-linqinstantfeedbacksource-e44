' Developer Express Code Central Example:
' How to implement CRUD operations using XtraGrid and LinqInstantFeedbackSource
' 
' This example demonstrates how to implement the Create, Update and Delete
' operations using LinqInstantFeedbackSource.
' This example works with the
' standard SQL Northwind database.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4499


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports DevExpress.LookAndFeel

Namespace LinqInstantFeedback
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread> _
		Shared Sub Main()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			DevExpress.Skins.SkinManager.EnableFormSkins()
			DevExpress.UserSkins.BonusSkins.Register()
			UserLookAndFeel.Default.SetSkinStyle("Blue")
			Application.Run(New Form1())
		End Sub
	End Class
End Namespace