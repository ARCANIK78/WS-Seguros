﻿Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class WebService1
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function
    <WebMethod()>
    Public Function ObtenerLugares() As db.TLugaresDataTable
        Dim adap As New dbTableAdapters.TLugaresTableAdapter
        Dim ds As New db.TLugaresDataTable
        adap.Fill(ds)
        Return ds
    End Function
    <WebMethod>
    Public Function RegistrarNuevoAfiliado(CI As String, nombre As String, paterno As String, materno As String, FechaNacimiento As Date, sexo As String, segurosID As String) As String
        Try
            Dim adap As New dbTableAdapters.TPersonasTableAdapter
            adap.Insert(CI, nombre, paterno, materno, FechaNacimiento, sexo, "ALTA")
            Dim adapa As New dbTableAdapters.TAfiliacionesTableAdapter
            adapa.Insert(CI, Date.Now, segurosID, "ALTA")
            Return "Afiliado registrado correctamente"
        Catch ex As Exception
            Return "Error: no se pudo registrar al nuevo afiliado"
        End Try
    End Function
    <WebMethod()> Public Function RegistrarBajas(CI As String) As String
        Try
            Dim adap As New dbTableAdapters.TAfiliacionesTableAdapter
            Dim afiliacionID As String
            afiliacionID = adap.ObtenerID(CI)
            Dim ds As New db.TAfiliacionesDataTable
            adap.ObtenerAfiliaciones(ds, afiliacionID)
            If ds.Item(0).Estado = "ALTA" Then
                adap.Insert(CI, Date.Now, ds.Item(0).SegurosID, "BAJA")
                Dim adapp As New dbTableAdapters.TPersonasTableAdapter
                adapp.ActualizarEstado("BAJA", CI)
                Return ("se Dio de baja el Afiliado Correctamente")
                MsgBox("Se Dio de baja")
            Else
                Return "El Afiliado ya se encuentra con estado de baja"
                MsgBox("EL afiliado ya se encuentra con estado de baja")
            End If

        Catch ex As Exception
            Return "Error: no se pudo dar de baja al afiliado"
            MsgBox("Error: no se pudo dat de baja al afiliado")
        End Try
    End Function
End Class
