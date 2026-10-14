<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="rptSalesReport2.aspx.cs"
     Inherits="Forms_rptSalesReport2" Title="SAMS :: Sales Report (Customer Wise)" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script language="JavaScript" type="text/javascript">
        function ValidateForm() {

            return true;
        }

    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ID="hfOpeningBal" />
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="70px" Text="Location" CssClass="lblbox"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList></td>
                                    </tr>                                    
                                    <tr style="display:none;">
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="70px" Text="Sector"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlSector" runat="server" Width="200px" OnSelectedIndexChanged="ddlSector_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="70px" Text="Town"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlTown" runat="server" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTown_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="lblDivision" runat="server" Width="78px" Text="Division"></asp:Label></strong> </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlDivision" runat="server" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label6" runat="server" Width="78px" Text="Customer"></asp:Label></strong> </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="76px" Height="13px" Text="From Date"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">&nbsp;<asp:TextBox ID="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server" Width="177px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="80px" Height="13px" Text="To Date"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">&nbsp;<asp:TextBox ID="txtEndDate" onkeyup="BlockEndDateKeyPress()" runat="server" Width="177px" CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left"></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtStartDate" PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="CEEndDate" runat="server" TargetControlID="txtEndDate" PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>                                    
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
        <asp:Button ID="btnViewPDF" runat="server" Style="margin-left:98px;" CssClass="Button" Width="90" Text="View PDF" OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" Width="90" CssClass="Button" Text="View Excel" OnClick="btnViewExcel_Click" /></td>
            </tr>
        </table>
        &nbsp;
        
    </div>
</asp:Content>

