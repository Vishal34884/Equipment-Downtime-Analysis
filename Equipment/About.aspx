<%@ Page Title="Analysis" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Equipment.About" %>

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
        .nowrap {
            white-space: nowrap;
        }

    </style>

    
    <br />
    <br />

    <br />

    <h1 style="align-content: center; padding-left: 300px; color: WHite"><b>EQUIPMENT DOWNTIME ANALYSIS</b></h1>
    <br />
    <!--Dropdown for Duration-->
    <b style="color: WHite;font-size:medium"">Duration</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
    <asp:DropDownList ID="SelectDateList" runat="server" Width="194px" AutoPostBack="True" OnSelectedIndexChanged="SelectDate_SelectedIndexChanged">
        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
        <asp:ListItem Text="1 Day" Value="1"></asp:ListItem>
        <asp:ListItem Text="7 Days" Value="7"></asp:ListItem>
        <asp:ListItem Text="30 Days" Value="30"></asp:ListItem>
        <asp:ListItem Text="90 Days" Value="90"></asp:ListItem>
        <asp:ListItem Text="365 Days" Value="365"></asp:ListItem>
        <asp:ListItem Text="Custom Date" Value="6"></asp:ListItem>
    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblwarning" runat="server" ForeColor="Red" Font-Bold="true" Text="Please Select Duration" Visible="false"></asp:Label>

    <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" Visible="false" Style="color: WHite" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtStartDate" runat="server" Visible="false" TextMode="Date" DateFormatString="dd/MM/yyyy"></asp:TextBox>
    <asp:Label ID="lblEndDate" runat="server" Text="End Date:" Visible="false" Style="color: WHite" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtEndDate" runat="server" Visible="false" TextMode="Date" DateFormatString="dd/MM/yyyy" OnTextChanged="txtEndDate_TextChanged" AutoPostBack="true" ></asp:TextBox>
    <asp:Label ID="lbldaterange" runat="server" ForeColor="Red" Font-Bold="true" Text="Select Valid Date Range" Visible="false"></asp:Label>
    <br />
    <br />
    <!--Dropdown for Chart Type-->
    <b style="color: White;font-size:medium"">Chart Type</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; 
     <asp:DropDownList ID="ChartTypeDropDownList" runat="server" Width="194px">
         <asp:ListItem Text="Column Chart" Value="Column"></asp:ListItem>
         <asp:ListItem Text="Pie Chart" Value="Pie"></asp:ListItem>
         <asp:ListItem Text="Line Chart" Value="Line"></asp:ListItem>
     </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
     <asp:Button ID="btnRun" runat="server" Text="Run" Width="126px" OnClick="btnRun_Click"  CssClass="btn btn-success" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Button ID="ExportButton" runat="server" Text="Export Data" OnClick="ExportButton_Click" CssClass="btn btn-primary" Width="120px" />
    <br />
    <br />
    <!--Dropdown for Equipment-->
    <b style="color: WHite;font-size:medium"">Equipment </b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            <asp:GridView ID="GridData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="94px" Width="1430px">
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
