<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="RptDistributorReports.aspx.cs" 
    Inherits="Forms_RptDistributorReports" Title="SAMS :: Sales & Closing Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
        function pageLaod()
        {
            checkDocument();
        }
        function checkDocument() {
            var Id = document.getElementById("<%=DrpReportType.ClientID %>");
            var selectedValue = Id.value;

            if (selectedValue == "4") {
                document.getElementById('<%= lblEndDate.ClientID %>').style.display = "none";
                document.getElementById('<%= txtEndDate.ClientID %>').style.display = "none";
                document.getElementById('<%= ibnEndDate.ClientID %>').style.display = "none";
                document.getElementById('<%= Label3.ClientID %>').innerHTML = "Date";
            } else {
                document.getElementById('<%= lblEndDate.ClientID %>').style.display = "block";
                document.getElementById('<%= txtEndDate.ClientID %>').style.display = "block";
                document.getElementById('<%= ibnEndDate.ClientID %>').style.display = "block";
                document.getElementById('<%= Label3.ClientID %>').innerHTML = "From Date";
            }
        }

    </script>
    <div id="right_data">
        <div>
            <asp:UpdateProgress ID="UpdateProgress" runat="server">
                <ProgressTemplate>
                    <asp:ImageButton ID="ImageButton10" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                        Width="31px" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
                PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="left" colspan="4">
                                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label1" runat="server" CssClass="lblbox" Text="Report Type" Width="78px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td style="height: 25px" align="left">
                                        <asp:DropDownList ID="DrpReportType" runat="server" Width="200px" CssClass="DropList" onchange="checkDocument()">
                                            <asp:ListItem Value="1" Selected="True">Sales</asp:ListItem>
                                            <asp:ListItem Value="2">Sales Return</asp:ListItem>
                                            <asp:ListItem Value="2">Damage</asp:ListItem>
                                            <asp:ListItem Value="4">Closing Stock</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="display: none;">
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label5" runat="server" CssClass="lblbox" Text="Value Type" Width="78px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="DrpUnitType" runat="server" Width="200px" CssClass="DropList">
                                            <asp:ListItem>Pieces</asp:ListItem>
                                            <asp:ListItem>Carton</asp:ListItem>
                                            <asp:ListItem>Value</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="display: none;">
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label7" runat="server" CssClass="lblbox" Text="Location Type" Width="101px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="ddDistributorType" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="ddDistributorType_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Location" Width="66px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label6" runat="server" CssClass="lblbox" Text="Supplier" Width="78px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList"
                                            OnSelectedIndexChanged="DrpPrincipal_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td >
                                                    <asp:CheckBox ID="cbAll" runat="server" Text="Products" BorderColor="Silver"
                                                        BorderWidth="1" Checked="true" AutoPostBack="True"
                                                        OnCheckedChanged="cbAll_CheckedChanged" />
                                                    <asp:Panel ID="Panel3" runat="server" Width="300px" Height="250px" ScrollBars="Vertical"
                                                        BorderStyle="Groove" BorderWidth="1px">
                                                        <asp:CheckBoxList ID="cblItem" runat="server" Width="90%">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label3" runat="server" Height="13px" Text="From Date" Width="80px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">&nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                         Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" /></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="lblEndDate" runat="server" Height="13px" Text="To Date" Width="80px"></asp:Label></strong></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">&nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"
                                         Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" /></td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        </td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                      <asp:CheckBox ID="chkElimination" runat="server" Text="Zero Elimination" BorderColor="Silver"
                                                        BorderWidth="1" Checked="true" />
                                        </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left"></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="ibtnStartDate" TargetControlID="txtStartDate">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                            TargetControlID="txtEndDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                             <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Width="90" Text="View PDF" OnClick="btnViewPDF_Click" />
                            <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Width="90" Text="View Excel" OnClick="btnViewExcel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnViewPDF"/>
                            <asp:PostBackTrigger ControlID="btnViewExcel"/>
                        </Triggers>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
                 </td>
            </tr>
        </table>
        
        
    </div>
</asp:Content>
