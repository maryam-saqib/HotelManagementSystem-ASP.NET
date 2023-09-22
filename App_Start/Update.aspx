<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="WebApplication1.Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="color: aliceblue;">Hotel Management System</h2>
        <br />
        <div class="search-container" style="margin-top: 20px;">
            <label for="txtSearch" style="color: aliceblue;">Search:</label>
            <asp:TextBox ID="txtSearch" runat="server" style="border: 1px solid #ccc; padding: 5px; border-radius: 5px;"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" style="background-color: #2196F3; color: aliceblue; border: none; padding: 10px 20px; border-radius: 5px; cursor: pointer;" />
        </div>
        <br />
        <br />
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing">
            <Columns>
                <asp:BoundField DataField="Branch_ID" HeaderText="Hotel ID" ReadOnly="true" />
                <asp:TemplateField HeaderText="Location">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLocation" runat="server" Text='<%# Bind("Location") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phone Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Bind("PhoneNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPhoneNumber" runat="server" Text='<%# Bind("PhoneNumber") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Button"/>
            </Columns>
            <HeaderStyle BackColor="#2196F3" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F2F2F2" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <div class="error-container">
            <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <style>
                body {
            background-color: #1F1F1F;
            
        }

       
        #container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 50vh;
            flex-wrap: wrap;
        }

        h2 {
          color: #fff;
          font-size: 36px;
          margin-bottom: 20px;
          text-align: center;
        }

        .search-container {
          display: flex;
          align-items: center;
          justify-content: center;
          margin-bottom: 20px;
        }

        label[for="txtSearch"] {
          color: #fff;
          margin-right: 10px;
        }

        #txtSearch {
          border: none;
          padding: 10px;
          border-radius: 5px;
          background-color: rgba(255, 255, 255, 0.8);
        }

        #btnSearch {
          background-color: #2B2B2B;
          color: #fff;
          border: none;
          padding: 10px 20px;
          border-radius: 5px;
          cursor: pointer;
          margin-left: 10px;
        }

        table {
          border-collapse: collapse;
          width: 100%;
          margin-bottom: 20px;
        }

        th,
        td {
          text-align: left;
          padding: 8px;
        }

        th {
          background-color: #2196F3;
          color: #fff;
          font-weight: bold;
        }

        tr:nth-child(even) {
          background-color: #f2f2f2;
        }

        /* Update edit field styles */
        .edit-mode {
          background-color: rgba(255, 255, 255, 0.8);
        }

        .edit-mode input {
          border: none;
          padding: 5px;
          border-radius: 5px;
        }

        /* Update error label styles */
        .error-container {
          text-align: center;
        }

        #lblError {
          color: red;
          margin-top: 10px;
        }

    </style>
</asp:Content>
