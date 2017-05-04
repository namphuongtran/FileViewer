<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="MASFileViewer.view" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <span>
            <button class="btn btn-default btnBack" type="button">Back</button>
        </span>
        <span>
            <h2>Syntax Highlighting</h2>
        </span>
    </div>
    <div class="content">
        <asp:Label ID="lblContent" runat="server"></asp:Label>
    </div>
</asp:Content>

