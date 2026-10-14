<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmCustomerClosing.aspx.cs" Inherits="Forms_frmCustomerClosing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tbody>
                        <tr style="display:none;">
                            <td align="left">
                                <strong>
                                    Report Type</strong>
                            </td>
                            <td style="height: 25px" align="left">
                                <asp:RadioButtonList ID="rdbType" runat="server" Width="180px" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Summary" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Detail" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <strong>
                                    <asp:Label ID="lbltoLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                            </td>
                            <td style="height: 25px" align="left">
                                <asp:DropDownList ID="drpDistributor" AutoPostBack="true" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" runat="server" Width="240px" CssClass="DropList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                          <tr>
                                   
                                    <td align="left">
                                        <strong>Town</strong>
                                    </td>
                                    
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="DrpTown" runat="server" CssClass="DropList" Width="240px"
                                            OnSelectedIndexChanged="DrpTown_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                        <tr>
                            <td align="left">
                                <strong>
                                    <asp:Label ID="lblCustomer" runat="server" Width="79px" Text="Customer" CssClass="lblbox"></asp:Label></strong>
                            </td>
                            <td style="height: 25px" align="left">
                                <asp:DropDownList ID="DrpCustomer" runat="server" Width="240px" CssClass="DropList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <strong>
                                    <asp:Label ID="lblFromDate" runat="server" Width="76px" Height="13px" Text="As on Date"></asp:Label></strong>
                            </td>
                            <td style="height: 25px" align="left">
                                
                                <asp:TextBox ID="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server"
                                    Width="150px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                </asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtStartDate"
                                    PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    &nbsp;&nbsp;
    <asp:Button ID="btnPDF" runat="server" CssClass="Button" Text="View PDF" Width="90"
        OnClick="btnPDF_Click" />
    <asp:Button ID="btnExcel" runat="server" CssClass="Button" Text="View Excel" Width="90"
        OnClick="btnExcel_Click" />
    </div>
</asp:Content>
