<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmDocumentPrinting.aspx.cs" Inherits="Forms_frmDocumentPrinting" Title="SAMS :: Print Sale Document" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script language="JavaScript" type="text/javascript">

        function pageLoad() {
            $("select").searchable();
        }
    </script>
    <div id="right_data">

        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                              <asp:HiddenField runat="server" ID="hfCustomerCode" />
                            <asp:HiddenField runat="server" ID="hfAddress" />
                            <asp:HiddenField runat="server" ID="hfOpeningBal" />
                            <table>
                                <tbody>
                                    <tr>
                                        
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr style="display: none;">
                                        <td style="height: 23px" align="right" colspan="5">
                                            <asp:RadioButtonList ID="rblCustomerType" runat="server" Width="300px" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="-1">All</asp:ListItem>
                                                <asp:ListItem Value="1">Registered</asp:ListItem>
                                                <asp:ListItem Value="0">UnRegistered</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td align="left" style="width: 95px;">
                                            <strong>Document Type</strong>
                                        </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpLedgerType" runat="server" Width="200px">
                                                <asp:ListItem Value="1">Invoice</asp:ListItem>
                                                <asp:ListItem Value="3">Sale Return</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td align="left"></td>
                                        <td align="left">
                                            <strong>Location</strong>
                                        </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td align="left"></td>
                                        <td align="left">
                                            <strong>City</strong>
                                        </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlTown" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="ddlTown_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td align="left"></td>
                                        <td align="left">
                                            <strong>Customer</strong>
                                        </td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpCustomer" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td align="left"></td>
                                        <td align="left">
                                            <strong>From Date
                                            </strong>
                                        </td>
                                        <td align="left"></td>
                                        <td align="left" style="height: 25px">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                                Width="175px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td align="left"></td>
                                        <td align="left">
                                            <strong>To Date
                                            </strong>
                                        </td>
                                        <td align="left"></td>
                                        <td align="left" style="height: 25px">
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"
                                                Width="175px"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;
                                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                                TargetControlID="txtStartDate">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                                TargetControlID="txtEndDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
                        <asp:Button ID="btnViewPDF" Style="margin-left: 98px;" runat="server" Width="90" Text="View PDF" OnClick="btnViewPDF_Click"
                            CssClass="Button" />
                    <asp:Button ID="btnViewExcel" runat="server" Width="90" Text="View Excel" OnClick="btnViewExcel_Click"
                        CssClass="Button" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
