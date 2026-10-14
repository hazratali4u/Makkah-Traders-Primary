<%@ Page Title="SAMS :: Delivery Order" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmDelieveryOrderAdd.aspx.cs" Inherits="Forms_frmDelieveryOrderAdd" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function pageLoad() {
            $("#<%=ddlCustomer.ClientID%>").searchable();
            $("#<%=ddlSKus.ClientID%>").searchable();
            $("#<%=DrpBusinessType.ClientID%>").searchable();

            SelectPrice();
        }
        function SelectPrice() {

            var unitprice = $('#<%=ddlSKus.ClientID %> option:selected').text();
            unitprice = unitprice.substring(unitprice.indexOf('[') + 1);
            unitprice = unitprice.substring(unitprice, unitprice.indexOf(']'));

            document.getElementById("<%= txtPrice.ClientID %>").value = unitprice;

            document.getElementById("<%= ddlSKus.ClientID %>").focus();
        }

        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;
            return true;

        }

        function CalculateAmount() {
            var GrossAmount = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
            var ProductDiscount = document.getElementById('<%=txtProductDiscount.ClientID%>').value;
            var InvoiceDiscount = document.getElementById('<%=txtInvoiceDiscount.ClientID%>').value;
            if (InvoiceDiscount == "") {
                InvoiceDiscount = 0;
            }

            if (InvoiceDiscount > 0) {
                var selectedvalue = $('#<%= rdbDiscountType.ClientID %> input:checked').val();
                if (selectedvalue == 1) {
                    document.getElementById("<%= txtTotalAmount.ClientID %>").value = ((GrossAmount - ProductDiscount) - InvoiceDiscount);
                }
                else {
                    document.getElementById("<%= txtTotalAmount.ClientID %>").value = (((GrossAmount - ProductDiscount)) - ((GrossAmount - ProductDiscount)) * (InvoiceDiscount / 100));
                }
            }
            else {
                document.getElementById("<%= txtTotalAmount.ClientID %>").value = ((GrossAmount - ProductDiscount));
            }

        }
    </script>
    <div id="right_data">
        <div style="z-index: 101; left: 60%; width: 100px; position: absolute; top: 1%; height: 80px">
            &nbsp;<asp:Panel ID="Panel21" runat="server">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                            Width="31px" />
                        Wait....
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </div>
        <asp:UpdatePanel ID="pnlMaster" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfIsGstReg" />

                <asp:HiddenField runat="server" ID="hfGst" />
                <asp:HiddenField runat="server" ID="hfInvoiceDiscount" />

                <table>
                    <tr style="display:none;">
                        <td>
                            <strong>Invoice Mode</strong>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdbInvoiceType" runat="server" Width="180px" Visible="false" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" Text="Credit" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Cash"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px;">
                            <strong>
                                <label>
                                    Location</label>
                            </strong>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDistributor" runat="server" Width="231px">
                            </asp:DropDownList>
                        </td>
                        <td style="width:5%">

                        </td>
                        <td>
                            <strong>Delivered to</strong>
                        </td>
                        <td>
                                <asp:TextBox ID="txtDeliveredTo" runat="server" Width="230px" MaxLength="70"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>

                        <td>
                            <strong>Division </strong>
                        </td>
                        <td>
                            <asp:DropDownList ID="DrpBusinessType" runat="server" Width="231px"
                                OnSelectedIndexChanged="DrpBusinessType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                         <td>
                            <strong>Delivered through</strong>
                        </td>
                        <td>
                                <asp:TextBox ID="txtDeliveredThrough" runat="server" Width="230px" MaxLength="70"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>
                                <label>
                                    Customer</label>
                            </strong>
                        </td>
                        <td>
                            <strong>
                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="231px" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Remarks </strong>
                        </td>

                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="10" Rows="1"
                                Width="350px"></asp:TextBox>
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="pnlDetail" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfStandardDiscount" Value="0" />
                <asp:HiddenField runat="server" ID="hfExtraDiscount" Value="0" />
                <asp:HiddenField runat="server" ID="hfGstAmount" Value="0" />
                <table width="100%">
                    <tr valign="top">
                        <td style="width: 65%">

                            <table>
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <fieldset style="width: 200px;">
                                                         <table>
                                                            <tbody>
                                                                <tr >
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td align="right">
                                                                        <strong>Discount In:</strong>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:RadioButtonList ID="rbDiscountType" runat="server" Width="105px" Height="1px"
                                                                            RepeatDirection="Horizontal">
                                                                            <asp:ListItem Value="1" Text="%"></asp:ListItem>
                                                                            <asp:ListItem Value="2" Text="Value" Selected="True"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 16px">
                                                                        <asp:Label ID="lblskuname" runat="server" Width="300px" Text="Product Name" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" Width="70px" Text="Price" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblquantity" runat="server" Width="70px" Text="Quantity" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label2" runat="server" Width="70px" Text="Discount" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                        <td>
                                                                            <asp:Label ID="Label3" runat="server" Width="70px" Text="Net Amount" CssClass="lblDetail"></asp:Label>
                                                                        </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlSKus" runat="server" Width="300px" onchange="SelectPrice();">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPrice" runat="server" Width="70px" ></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtQuantity" runat="server" Width="70px"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDiscount" runat="server" Width="70px"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAmount" runat="server" Width="70px" ReadOnly="true" Enabled="false"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="75px"
                                                                            Font-Size="8pt" Text="Add" ValidationGroup="vg" CssClass="Button" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="6">
                                                                        <asp:Panel ID="Panel2" runat="server" Width="690px" Height="155px" ScrollBars="Vertical"
                                                                            BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                                            <asp:GridView ID="grdOrder" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                OnRowDeleting="grdOrder_RowDeleting"
                                                                                OnRowEditing="grdOrder_RowEditing" ShowHeader="False" Width="100%">
                                                                                <RowStyle ForeColor="Black" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="skuId" HeaderText="SKU_ID">
                                                                                        <HeaderStyle CssClass="HidePanel" />
                                                                                        <ItemStyle CssClass="HidePanel" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="skuName" HeaderText="SKU Name">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="310px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:f2}">

                                                                                        <ItemStyle CssClass="grdDetail" Width="65px" HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="qty" HeaderText="Quantity">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="65px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="amount" HeaderText="Amount">
                                                                                        <HeaderStyle CssClass="HidePanel" />
                                                                                        <ItemStyle CssClass="HidePanel" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="discount" DataFormatString="{0:f2}">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="65px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="discountType">
                                                                                        <ItemStyle CssClass="HidePanel" />
                                                                                        <HeaderStyle CssClass="HidePanel" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="netAmount" DataFormatString="{0:f2}">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="65px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                                        <ItemStyle CssClass="grdDetail" Width="35px" HorizontalAlign="Center" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="Delete">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                                Text="Delete" ToolTip="Delete"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="grdDetail" Width="35px" HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                                                    VerticalAlign="Middle" />
                                                                                <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>

                                </tbody>
                            </table>

                        </td>
                        <td style="width: 24%">
                            <table style="width: 100%">

                                <tr>
                                    <td colspan="2">
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td style="width: 200px; font-size: 15px;">
                                                        <strong>Ledger Bal:</strong>
                                                    </td>
                                                    <td style="font-size: 15px; width:65%">
                                                        <asp:Label ID="lblLedgerBalance" runat="server" Text="0.00"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblOpeningType" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr style="display: none;">
                                    <td>
                                        <strong>Builty No </strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBuilty" Width="100%" runat="server"></asp:TextBox>
                                    </td>
                                </tr>


                                <tr>
                                    <td>
                                        <strong>Gross Amount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrossAmount" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Product Discount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductDiscount" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Discount Type</strong>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdbDiscountType" runat="server" Width="130px" RepeatDirection="Horizontal" onchange="CalculateAmount();">
                                            <asp:ListItem Value="0" Text="%"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Value" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Invoice Discount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceDiscount" runat="server" onkeyup="CalculateAmount();"
                                            onkeypress="return onlyDotsAndNumbers(this,event);">

                                        </asp:TextBox>
                                    </td>
                                </tr>

                                <tr style="display: none;">
                                    <td>
                                        <strong>Total Qty </strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalQty" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Net Amount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalAmount" runat="server" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <strong>
                                            <asp:CheckBox ID="chkLimit" Text="Skip Limit" Font-Size="14px" runat="server" /></strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Button AccessKey="S" ID="btnSaveOrder" runat="server" Width="90px" Font-Size="8pt"
                        Text="Save" UseSubmitBehavior="False" OnClick="btnSaveOrder_Click" CssClass="Button" />
                    <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="90px" Font-Size="8pt"
                        Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="Button" />
                </td>

            </tr>
        </table>
    </div>
</asp:Content>
