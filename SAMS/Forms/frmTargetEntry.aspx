<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmTargetEntry.aspx.cs" Inherits="Forms_frmTargetEntry" Title="SAMS: Target Entry" %>

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
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
        <table width="100%">
            <tr>
                <td align="left">
                    
                            <table>
                                <tbody>
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
                                            <asp:DropDownList ID="DrpTargetFor" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpTargetFor_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>Customer</strong>
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpCustomer" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px">
                                            <strong>For Month</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:TextBox ID="txtFromdate" runat="server" Width="176px" Enabled="false" OnTextChanged="txtFromdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                        <td style="height: 25px"></td>
                                    </tr>

                                    <tr>
                                        <td style="height: 25px; width: 100px;">
                                            <strong>Target In CTNs</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:TextBox ID="txtTargetValue" runat="server" Width="200px" onkeypress="return onlyNumbers(this,event);"></asp:TextBox>

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
                                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate" BehaviorID="calendar3"
                                                TargetControlID="txtFromdate" OnClientShown="onCalendarShown3" OnClientHidden="onCalendarHidden3">
                                            </cc1:CalendarExtender>
                                            <%--  <asp:Button ID="btnTarget" runat="server" Font-Size="8pt" Text="View Target" ValidationGroup="vg"
                                                Width="95px" OnClick="btnTarget_Click" CssClass="Button" />--%>
                                            <asp:Button ID="btnSaveTarget" runat="server" Font-Size="8pt" Text="Save" ValidationGroup="vg"
                                                Width="95px" CssClass="Button" OnClick="btnSaveTarget_Click" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                       
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
<%--                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <fieldset style="width: 65%">
                                <table width="100%">
                                    <%--  <tr>
                                    <td style="width: 82px;">
                                        <strong>
                                            <asp:Label ID="lblskuCode" runat="server" CssClass="tblhead" Font-Bold="True" ForeColor="White"
                                                Width="300px" Text="  Product Description"></asp:Label></strong>
                                    </td>
                                    
                                    <td style="width: 70px;">
                                        <strong>
                                            <asp:Label ID="lblquantity" runat="server" CssClass="tblhead" Font-Bold="True" ForeColor="White"
                                                Height="16px" Text="Quantity" Width="70px"></asp:Label></strong>
                                    </td>
                                    <td style="width: 70px;">
                                        <strong>
                                            <asp:Label ID="lblFreeSKU" runat="server" CssClass="tblhead" Font-Bold="True" ForeColor="White"
                                                Height="16px" Text="Price" Width="70px"></asp:Label></strong>
                                    </td>
                                    <td style="width: 70px;">
                                        <strong>
                                            <asp:Label ID="Label2" runat="server" CssClass="tblhead" Font-Bold="True" ForeColor="White"
                                                Height="16px" Text="Amount" Width="70px"></asp:Label></strong>
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td style="width: 200px;">
                                       <asp:DropDownList ID="ddlSKuCde" runat="server" Width="300px">
                                                </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtBox "
                                            Width="73px" onkeypress="return onlyNumbers(this,event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnitRate" runat="server" CssClass="txtBox" Enabled="False" Width="73px"
                                            onkeypress="return onlyNumbers(this,event);">0</asp:TextBox>
                                    </td>
                                     <td>
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="txtBox" Enabled="False" Width="73px">0</asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" AccessKey="A" Font-Size="8pt" OnClick="btnSave_Click"
                                            CssClass="Button" Text="Save" ValidationGroup="vg" Width="95px" />
                                    </td>
                                </tr>--%>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel ID="Panel2" runat="server" BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px"
                                                Height="180px" ScrollBars="Vertical" Width="100%">
                                                <asp:GridView ID="GrdPurchase" runat="server" Width="99.7%" ForeColor="SteelBlue"
                                                    AutoGenerateColumns="False" BackColor="White" BorderColor="SteelBlue"
                                                     OnRowEditing="GrdPurchase_RowEditing" OnRowDeleting="GrdPurchase_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataField="TARGET_ID" HeaderText="TARGET_ID">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TARGET_FOR_ID" HeaderText="TARGET_FOR_ID">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GEO_NAME" HeaderText="Town">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer Name">
                                                            <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                                            <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Month">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Bind("TARGET_MONTH", "{0:dd-MMM-yyyy}") %>' ID="lblDate"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                                            <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UNIT_PRICE" HeaderText="CTN" DataFormatString="{0:f2}">
                                                            <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                            <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="REMARKS" HeaderText="Remarks">
                                                            <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                                            <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TARGET_MONTH" HeaderText="TARGET_MONTH">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
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

                                                    <HeaderStyle VerticalAlign="Middle" CssClass="grdHead"></HeaderStyle>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <%--    <tr>
                                    <td style="height: 12px">
                                    </td>
                                    <td style="height: 12px">
                                    </td>
                                    <td style="height: 12px">
                                        <strong>
                                            <asp:Label ID="Label3" runat="server" CssClass="tblhead" Font-Bold="True" ForeColor="White"
                                                Height="16px" Text="Total Value" Width="70px"></asp:Label></strong>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="txtBox" Width="95px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                </table>
                            </fieldset>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
                             </ContentTemplate>
                    </asp:UpdatePanel>
    </div>
</asp:Content>
