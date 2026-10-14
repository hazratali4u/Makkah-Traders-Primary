<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmTargetEntrySkuWise.aspx.cs" Inherits="Forms_frmTargetEntrySkuWise" Title="SAMS: Target Entry SKU Wise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function ConfirmDelete() {
            if (confirm("Do you want to Cancel this record?") == true)
                return true;

            else {
                return false;
            }
        }

        function ValidateForm() {
            var str;



            <%--  str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Quantity');
                return false;
            }--%>
            <%--  str = document.getElementById('<%=txtFromdate.ClientID%>').value;
            if (str == null || str.length <= 1) {
                alert('Must Select Target Month');
                return false;
            }
            
            str = document.getElementById('<%=drpDistributor.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Select Distributor');
                return false;
            }--%>
           <%-- str = document.getElementById('<%=DrpTargetFor.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Select Target For');
                return false;
            }
            return true;--%>
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

        function onlyNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;

            return true;
        }
    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr style="display:none;">
                                            <td style="height: 25px; width: 80px;">
                                                <strong>
                                                    <asp:Label ID="lblDocumentNo" runat="server" Text="Document No"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDocumentNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px; width: 80px;">
                                                <strong>Location</strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDistributor" runat="server" CssClass="DropList" Width="200px"
                                                    OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>

                                        </tr>

                                        <tr>
                                            <td style="height: 25px">
                                                <strong>Town</strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="ddlTown" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px">
                                                <strong>Customer</strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="DrpCustomer" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>

                                        <tr>
                                            <td style="height: 25px">
                                                <strong>For Month</strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtFromdate" runat="server" Width="176px" OnTextChanged="txtFromdate_TextChanged" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                    Width="16px" />
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px">
                                                <strong>Remarks</strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" Height="50px" TextMode="MultiLine"></asp:TextBox>

                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 25px" valign="top"></td>
                                            <td style="width: 1px; height: 25px;" valign="top">
                                                <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate" BehaviorID="calendar3" TargetControlID="txtFromdate" OnClientShown="onCalendarShown3" OnClientHidden="onCalendarHidden3"></cc1:CalendarExtender>
                                            <asp:HiddenField ID="hfTARGET_MASTER_ID" runat="server" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table width="120%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblskuCode" runat="server" Width="100%" Text="  SKU Description" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblquantity" runat="server" Width="114px" Text="Carton" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl8" runat="server" Width="114px" Text="Unit" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <%--<td>
                                                <asp:Label ID="lblSchemeValue" runat="server" Width="88" Text="Scheme Value" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Width="88" Text="Purchase Rate" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Width="88" Text="Discount(%)" CssClass="lblDetail"></asp:Label>
                                            </td>--%>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlSKuCde" runat="server" Width="425px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity" onfocus="SearchedCode()" Style="text-align: right;" runat="server" Width="110px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantityUnit" runat="server" Width="110px"></asp:TextBox>
                                            </td>
                                            <%--<td>
                                                <asp:TextBox ID="txtScheme" runat="server" Width="88px" Enabled="false">0</asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurrentRate" runat="server" Width="88px" Text="0.00"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscount" runat="server" Width="88px" Text="0.00" Enabled="false"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtDiscount">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>--%>
                                            <td>
                                                <asp:Button AccessKey="A" ID="btnAdd" OnClick="btnAdd_Click" runat="server" Width="90px"
                                                    Font-Size="8pt" Text="Add" CssClass="Button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="7">
                                                <asp:Panel ID="Panel2" runat="server" Width="98%" Height="200px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                        OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing"
                                                        ShowHeader="False" Width="100%">
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="55px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="180px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PACKSIZE" HeaderText="Pack Size">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="65px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="QuantityCtn" HeaderText="Ctn">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Unit">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                            </asp:BoundField>
                                                            <%--<asp:BoundField DataField="FREE_SKU" HeaderText="Free SKU">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PRICE" HeaderText="PRICE">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DISCOUNT" HeaderText="DISCOUNT">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TRADE_PRICE">
                                                                <ItemStyle CssClass="HidePanel" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AMOUNT">
                                                                <ItemStyle CssClass="HidePanel" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>--%>
                                                            <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                <ItemStyle CssClass="grdDetail" Width="20px" HorizontalAlign="Center" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" Width="25px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" />
                                                        <PagerStyle BackColor="Transparent" />
                                                        <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                            VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button AccessKey="S" ID="btnSaveDocument" runat="server" Width="110px" Font-Size="8pt"
                                                    Text="Save Document" UseSubmitBehavior="False" OnClick="btnSaveDocument_Click"
                                                    CssClass="Button" />
                                                <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="110px" Font-Size="8pt"
                                                    Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="Button" />
                                                <strong>
                                                    <asp:Label ID="Label7" Style="margin-left: 110px;" runat="server" Height="16px" Text="Total Quantity"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalCtn" runat="server" Width="115px" Style="margin-left: -5px; text-align: right;"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalQuantity" runat="server" Width="115px" Style="margin-left: -5px; text-align: right;"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
