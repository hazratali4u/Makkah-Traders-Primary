<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptTargetReport.aspx.cs" Inherits="Forms_RptTargetReport" MasterPageFile="~/Forms/PageMaster.master" Title="SAMS :: Target Vs Achievement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function onCalendarShown3() {
            var cal = $find("calendar3");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
        }

        function onCalendarHidden3() {
            var cal = $find("calendar3");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
        }

        function call3(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar3");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
        <table width="100%">
            <tr>
                <td>
                    
                            <table>
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td style="width: 1px" align="left" colspan="1"></td>
                                    </tr>
                                 <tr>
                                        <td align="left"></td>
                                        <td style="width: 92px" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="48px" Text="Report Type" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="width: 1px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:RadioButtonList ID="RbtReportType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" Text="Detail" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Summary"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td style="width: 92px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="48px" Text="Location" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="width: 1px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:DropDownList ID="DrpLocation" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True"></asp:DropDownList></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td style="width: 92px" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="61px" Text="Town" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="width: 1px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:DropDownList ID="drpTown" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="drpTown_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td style="width: 92px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="61px" Text="Customer" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="width: 1px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:DropDownList ID="DrpCustomer" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True"></asp:DropDownList></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="left"></td>
                                        <td style="width: 92px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="48px" Text="Month" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtFromdate" runat="server" Width="176px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" /></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate" BehaviorID="calendar3"
                                                TargetControlID="txtFromdate" OnClientShown="onCalendarShown3" OnClientHidden="onCalendarHidden3">
                                            </cc1:CalendarExtender>
                            &nbsp; 
                            
                    &nbsp;
                    

                </td>
            </tr>
        </table>
        </ContentTemplate>
                        
                    </asp:UpdatePanel>
        <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Width="90" Text="View PDF" OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" Width="90" OnClick="btnViewExcel_Click" Text="View Excel" CssClass="Button" />
                        
    </div>
</asp:Content>
