<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPurchaseEntry.aspx.cs" Inherits="Forms_frmPurchaseEntry" Title="SAMS :: Sales Invoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>

    <script type="text/javascript">

        function pageLoad() {
            $("select").searchable();
            SelectPrice();
        }
        function NetAmountTotal() {
            var g_amont = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
            var dis = document.getElementById('<%=txtDiscount2.ClientID%>').value;
            var result1 = (parseFloat(g_amont) - parseFloat(dis)).toFixed(2);
            var schm = document.getElementById('<%=txtSchemeValue.ClientID%>').value;
            var result2 = (parseFloat(result1) - parseFloat(schm)).toFixed(2);
            var frght = document.getElementById('<%=txtFreightAmount.ClientID%>').value;
            var result = (parseFloat(result2) - parseFloat(frght)).toFixed(2);
            document.getElementById('<%=txtNetValue.ClientID%>').value = result;
        }

        function SelectPrice() {

            var unitprice = $('#<%=ddlSKuCde.ClientID %> option:selected').text();
             unitprice = unitprice.substring(unitprice.indexOf(']') + 1);

             document.getElementById("<%= txtCurrentRate.ClientID %>").value = unitprice;

            document.getElementById("<%= ddlSKuCde.ClientID %>").focus();
         }
         function ValidateForm() {


             var DCNo = $('#ctl00_ctl00_mainCopy_cphPage_txtDocumentNo').val();
             if (DCNo == '') {

                 $('#valid_error_msg').html("Invoice/DC No cannot be empty");
                 $('#valid_error_msg').parent().show();

                 return false;
             }

             var Quantity = $('#ctl00_ctl00_mainCopy_cphPage_txtQuantity').val();
             var Cotton = $('#ctl00_ctl00_mainCopy_cphPage_txtCtn').val();


             if ((Quantity == '' || Quantity == 0) && (Cotton == '' || Cotton == 0)) {

                 $('#valid_error_msg').html("Quantity cannot be empty");
                 $('#valid_error_msg').parent().show();

                 return false;
             }
             //$('#valid_error_msg').html("");
             return true;

         }
         <%--var prm = Sys.WebForms.PageRequestManager.getInstance();
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
        }--%>


    </script>
    <%--<div>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <asp:ImageButton ID="ImageButton10" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                    Width="31px" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
            PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
    </div>--%>
    <div id="right_data">

        <div class='error_msg'>

            <div class='error_field' id='valid_error_msg' style="color: red">
            </div>
        </div>

        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 30%;" colspan="3">
                                            <asp:CheckBox ID="ChkSaleOnly" runat="server" Text="Enable Customer Sale Only" Checked="false"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%;" valign="top">
                                            <table width="100%">

                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="lblDocumentNo" runat="server" Text="Document No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:DropDownList ID="drpDocumentNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged"
                                                            Width="200px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="lbltoLocation" runat="server" Text="Vendor"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:DropDownList ID="drpPrincipal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPrincipal_SelectedIndexChanged"
                                                            Width="200px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="lblfromLocation" runat="server" Text="Purchase For"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:DropDownList ID="drpDistributor" runat="server" AutoPostBack="True" Width="200px"
                                                            OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label4" runat="server" Text="Town"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:DropDownList ID="ddlTown" runat="server" Width="200px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlTown_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label2" runat="server" Text="Customer"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label14" runat="server" Text="Dispatch To"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtDispatchTo" runat="server" Width="195px" MaxLength="200"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label13" runat="server" Text="Remarks"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtRemakrs" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 30%;" valign="top">
                                            <table width="100%">
                                                <tr>
                                                   <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label3" runat="server" Text="Document Date"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                      <asp:TextBox ID="txtFromdate" runat="server" MaxLength="1"
                                                    Width="150px"></asp:TextBox>
                                                 <asp:ImageButton ID="ImgBntFromCalc" runat="server" CausesValidation="False" ImageUrl="~/App_Themes/Granite/Images/date.gif" />
                                           
                                                <cc1:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                                                    PopupButtonID="ImgBntFromCalc" TargetControlID="txtFromdate">

                                                </cc1:CalendarExtender>  </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label5" runat="server" Text="Gate Pass No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtGatePass" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label6" runat="server" Text="Vehicle No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtVehicle" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label11" runat="server" Text="Builty No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtBuiltyNo" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label12" runat="server" Text="Transporter"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtTransporter" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label1" runat="server" Text="Vendor INV/DC  No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtDocumentNo" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display:none;">
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label15" runat="server" Text="Driver Cell No"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtDriverCellNo" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display:none;">
                                                    <td style="width: 35%;">
                                                        <strong>
                                                            <asp:Label ID="Label16" runat="server" Text="Courier"></asp:Label>
                                                        </strong>
                                                    </td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtCourier" runat="server" Width="195px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 40%;" valign="top">
                                            <asp:Panel ID="pnlSummary" runat="server" Width="70%" ScrollBars="None" BorderWidth="1px"
                                                BorderStyle="Groove" BorderColor="Silver" Style="padding: 10px;">
                                                <table width="100%" cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <strong>Gross Amount</strong>
                                                        </td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtGrossAmount" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <strong>Discount</strong>
                                                        </td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtDiscount2" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <strong>Scheme Value</strong>
                                                        </td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtSchemeValue" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 32%">
                                                            <strong>Freight Amount</strong>
                                                        </td>
                                                        <td style="width: 68%">
                                                            <asp:TextBox ID="txtFreightAmount" runat="server" Width="100%" onchange="NetAmountTotal();"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="fteFreightAmount" runat="server" FilterType="Custom" ValidChars="0123456789."
                                                                TargetControlID="txtFreightAmount">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%">
                                                            <strong>Net Amount</strong>
                                                        </td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtNetValue" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hfIsPromotion" runat="server" Value="0"></asp:HiddenField>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table width="100%">
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
                                                <asp:Label ID="Label8" runat="server" Width="81" Text="Ctn" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblquantity" runat="server" Width="88px" Text="Unit" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSchemeValue" runat="server" Width="88" Text="Scheme Value" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Width="88" Text="Purchase Rate" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Width="88" Text="Discount(%)" CssClass="lblDetail"></asp:Label>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlSKuCde" runat="server" Width="315px" onchange="SelectPrice();">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCtn" onfocus="SearchedCode()" runat="server" Width="80px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity" runat="server" Width="88px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScheme" runat="server" Width="88px" Enabled="false">0</asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurrentRate" runat="server" Width="88px" Text="0.00"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscount" runat="server" Width="88px" Text="0.00" Enabled="false"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtDiscount">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="90px"
                                                    Font-Size="8pt" Text="Add Sku" CssClass="Button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="7">
                                                <asp:Panel ID="Panel2" runat="server" Width="100%" Height="130px" ScrollBars="Vertical"
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
                                                            <asp:BoundField DataField="FREE_SKU" HeaderText="Free SKU">
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
                                                            </asp:BoundField>
                                                            <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                <ItemStyle CssClass="grdDetail" Width="40px" HorizontalAlign="Center" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" Width="40px" HorizontalAlign="Center" />
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
                                                    <asp:Label ID="Label7" runat="server" Height="16px" Text="Total Quantity"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalCtn" onkeyup="SearchList()" runat="server" Width="81px"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalQuantity" onkeyup="SearchList()" runat="server" Width="88px"
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
