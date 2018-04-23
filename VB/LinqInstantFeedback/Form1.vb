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
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraEditors
Imports System.Linq
Imports System.Data.Linq


Namespace LinqInstantFeedback
	Partial Public Class Form1
		Inherits XtraForm
		Public Sub New()
			InitializeComponent()
			AddHandler linqInstantFeedbackSource1.GetQueryable, AddressOf linqInstantFeedbackSource1_GetQueryable

			AddHandler gridView1.AsyncCompleted, AddressOf gridView1_AsyncCompleted
			oldRowsCount = gridView1.DataRowCount
			gridControl.DataSource = linqInstantFeedbackSource1
		End Sub

		Private Sub gridView1_AsyncCompleted(ByVal sender As Object, ByVal e As EventArgs)
			If customerToEdit IsNot Nothing AndAlso gridView1.DataRowCount > oldRowsCount Then
				For i As Integer = 0 To gridView1.DataRowCount - 1
					If customerToEdit.CustomerID = gridView1.GetRowCellValue(i, gridView1.Columns("CustomerID")).ToString() Then
						gridView1.FocusedRowHandle = i
						oldRowsCount = gridView1.DataRowCount
						Exit For
					End If
				Next i
			End If
		End Sub
		Private nwdContext As New NorthwindDataContext()
		Private customerToEdit As Customer
		Private f1 As EditForm
		Private oldRowsCount As Integer = 0
		Private Sub linqInstantFeedbackSource1_GetQueryable(ByVal sender As Object, ByVal e As DevExpress.Data.Linq.GetQueryableEventArgs)
			e.QueryableSource = nwdContext.Customers
		End Sub
		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
			customerToEdit = CreateCustomer()
			oldRowsCount = gridView1.DataRowCount
			EditCustomer(customerToEdit, "NewCustomer", AddressOf CloseNewCustomerHandler)
		End Sub
		Private Function CreateCustomer() As Customer
			Dim idString As String
			Dim newCustomer = New Customer()
			Do
				idString = GenerateCustomerID()
				If (Not String.IsNullOrEmpty(idString)) Then
					newCustomer.CustomerID = idString
					Exit Do
				End If
			Loop
			Return newCustomer
		End Function

		Private Function GenerateCustomerID() As String
			Const IDLength As Integer = 5
			Dim result = String.Empty
			Dim rnd = New Random()
            Dim collisionFlag As Boolean = False
            Dim j
            For j = 0 To IDLength - 1
                result += Convert.ToChar(rnd.Next(65, 90))
            Next j
			For i As Integer = 0 To gridView1.DataRowCount - 1
				If result Is gridView1.GetRowCellValue(i, gridView1.Columns("CustomerID")).ToString() Then
					collisionFlag = True
					Exit For
				End If
			Next i
			If collisionFlag Then
				Return String.Empty
			Else
				Return result
			End If
		End Function
		Private Sub button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
            Dim query = (From
                                               Customer In
                                               nwdContext.Customers
                                                        Where
                                                        Customer.CustomerID = GetCustomerIDByRowHandle(gridView1.FocusedRowHandle)
                                                        Select
                                                        Customer).ToList()

            customerToEdit = query(0)
			EditCustomer(customerToEdit, "EditInfo", AddressOf CloseEditCustomerHandler)
		End Sub
		Private Sub EditCustomer(ByVal customer As Customer, ByVal windowTitle As String, ByVal closedDelegate As FormClosingEventHandler)
			f1 = New EditForm(customer) With {.Text = windowTitle}
			AddHandler f1.FormClosing, closedDelegate
			f1.ShowDialog()
		End Sub
		Private Function GetCustomerIDByRowHandle(ByVal rowHandle As Integer) As String
			Return CStr(gridView1.GetRowCellValue(rowHandle, "CustomerID"))
		End Function
		Private Sub CloseEditCustomerHandler(ByVal sender As Object, ByVal e As EventArgs)
			If (CType(sender, EditForm)).DialogResult = System.Windows.Forms.DialogResult.OK Then
				Try
					nwdContext.SubmitChanges()
					nwdContext.Refresh(RefreshMode.OverwriteCurrentValues, nwdContext.Customers)
					linqInstantFeedbackSource1.Refresh()

				Catch ex As Exception
					HandleExcepton(ex)
				End Try
			End If
			customerToEdit = Nothing
		End Sub
		Private Sub CloseNewCustomerHandler(ByVal sender As Object, ByVal e As FormClosingEventArgs)
			If (CType(sender, EditForm)).DialogResult = System.Windows.Forms.DialogResult.OK Then
				Try
					nwdContext.Customers.InsertOnSubmit(customerToEdit)
					nwdContext.SubmitChanges()
					nwdContext.Refresh(RefreshMode.OverwriteCurrentValues, nwdContext.Customers)
					linqInstantFeedbackSource1.Refresh()

				Catch ex As Exception
					HandleExcepton(ex)
				End Try
			Else
				customerToEdit = Nothing
			End If
		End Sub

		Private Sub HandleExcepton(ByVal ex As Exception)
			MessageBox.Show(ex.Message)
		End Sub

		Private Sub button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton3.Click
			DeleteCustomer(gridView1.FocusedRowHandle)
		End Sub
		Private Sub DeleteCustomer(ByVal focusedRowHandle As Integer)
			If focusedRowHandle < 0 Then
				Return
			End If
			If MessageBox.Show("Do you really want to delete the selected customer?", "Delete Customer", MessageBoxButtons.OKCancel) <> System.Windows.Forms.DialogResult.OK Then
				Return
			End If
            Dim query = (From
                                                          Customer In
                                                          nwdContext.Customers
                                                                   Where
                                                                   Customer.CustomerID = GetCustomerIDByRowHandle(gridView1.FocusedRowHandle)
                                                                   Select
                                                                   Customer).ToList()

            customerToEdit = query(0)
            nwdContext.Customers.DeleteOnSubmit(query(0))
			Try
				nwdContext.SubmitChanges()
				nwdContext.Refresh(RefreshMode.OverwriteCurrentValues, nwdContext.Customers)
			Catch ex As Exception
				HandleExcepton(ex)
			End Try
			linqInstantFeedbackSource1.Refresh()
			customerToEdit = Nothing
		End Sub

	End Class



End Namespace