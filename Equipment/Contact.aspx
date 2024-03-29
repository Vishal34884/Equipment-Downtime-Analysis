﻿<%@ Page Title="Overall Downtime Analysis" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Equipment.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!--Style-->
    <style>
        body {
            margin: 0;
            padding: 0;
        }

        #GridView1 {
            border-radius: 1rem;
        }

        .row {
            margin-left: 0px;
        }

        .col-lg-6 {
            border-radius: 1rem;
            background-color: white;
            margin-left: 0px;
        }
    </style>
    <br />
    <br />

    <br />

    <h1 style="align-content: center; padding-left: 300px; color: WHite"><b>OVERALL EQUIPMENT DOWNTIME ANALYSIS</b></h1>
    <br />
    <!--Dropdown for Time Period-->
    <b style="color: WHite;font-size:medium">Time Period</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="SelectDateList" runat="server" Width="194px" AutoPostBack="True">
        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>   <%--Select--%>
        <asp:ListItem Text="Date" Value="date"></asp:ListItem>  <%--Date--%>
        <asp:ListItem Text="Month" Value="month"></asp:ListItem>  <%--Month--%>
        <asp:ListItem Text="Year" Value="year"></asp:ListItem>  <%--Year--%>
    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblwarning" runat="server" ForeColor="Red" Text="Please Select Time Period" Visible="false"></asp:Label>

    <br />
    <br />
    <!--Dropdown for Chart Type-->
    <b style="color: White;font-size:medium"">Chart Type</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
     <asp:DropDownList ID="ChartTypeDropDownList" runat="server" Width="194px">
         <asp:ListItem Text="Column Chart" Value="Column"></asp:ListItem>  <%--Stacked Column Chart--%>
         <asp:ListItem Text="Pie Chart" Value="Pie"></asp:ListItem>  <%--Pie Chart--%>
         <asp:ListItem Text="Line Chart" Value="Line"></asp:ListItem>  <%--Line Chart--%>
     </asp:DropDownList>
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
     <asp:Button ID="btnRun" runat="server" Text="Run" Width="126px" OnClick="btnRun_Click" CssClass="btn btn-success" />
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Button ID="ExportButton" runat="server" Text="Export Data" OnClick="ExportButton_Click" CssClass="btn btn-primary" Width="120px" />
    <br />
    <br />
    <!--Dropdown for Equipment-->
    <b style="color: WHite;font-size:medium"">Equipment </b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
     <asp:DropDownList ID="EquipmentList" runat="server" Width="194px">
         <asp:ListItem Text="All Equipments" Value=""></asp:ListItem>
         <asp:ListItem Text="EQ_01" Value="EQ_01"></asp:ListItem>
         <asp:ListItem Text="EQ_02" Value="EQ_02"></asp:ListItem>
     </asp:DropDownList>
    <br />
    <br />
    <br />
    <br>
    <br />
    <div class="row">
        <!--Grid View-->
        <div class="col-lg-6 p-0 m-0" dir="ltr" style="height: 400px; width: 576px; overflow: auto; top: 0px; left: 0px; margin-right: 10px;" role="grid">
            <br />
            <asp:GridView ID="GridData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="102px" Width="588px">
                <AlternatingRowStyle BackColor="white" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="white" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="white" />
                <PagerStyle BackColor="#2461BF" ForeColor="white" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
            <br />
        </div>
        <!--Chart-->
        <div class="col-lg-6" role="grid">
            <asp:Chart ID="downtimechart" runat="server" Width="675px" Height="400px" border-radius="1rem">
                <Series>
                    <asp:Series Name="Downtime" XValueMember="EquipementName" YValueMembers="Downtime_Duration" ShadowColor="#339966" Color="#339966" CustomProperties="PieLabelStyle=Outside"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="Main"></asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="MyLegend"></asp:Legend>
                </Legends>
            </asp:Chart>
        </div>
    </div>
</asp:Content>
