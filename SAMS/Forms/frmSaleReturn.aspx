<%@ Page Title="SAMS :: Sales Return" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmSaleReturn.aspx.cs" Inherits="Forms_frmSaleReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadPage" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script type="text/javascript" src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function pageLoad() {
            $("select").searchable();
            SelectPrice();
        }
        function SelectPrice() {

            var unitprice = $('#<%=ddlSKus.ClientID %> option:selected').text();
            var unitprice2 = unitprice.split(':');    
            document.getElementById("<%= txtPrice.ClientID %>").value = unitprice2[1];
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
    </script>
    <div id="right_data">
        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 10%; height: 100px">
            &nbsp;<asp:Panel ID="Panel21" runat="server">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                            Width="31px" />
                        Wait Update
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </div>
        <asp:UpdatePanel ID="pnlMaster" runat="server">
            <ContentTemplate>
                <table cellpadding="2px">
                    <tr>
                        <td style="width: 70px;">
                            <strong>
                                <label>Location</label>
                            </strong>
                        </td>
                        <td style="width: 265px;">
                            <asp:DropDownList ID="ddlDistributor" runat="server" Width="220px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDistributor_SelectedIndexChanged" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px;">
                            <strong>
                                <label>Town</label>
                            </strong>
                        </td>
                        <td style="width: 265px;">
                            <asp:DropDownList ID="ddlTown" runat="server" Width="220px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>
                                <label>
                                    Division</label>
                            </strong>
                        </td>
                        <td style="width: 265px;">
                            <asp:DropDownList ID="ddlBusinessType" runat="server" Width="220px"
                                OnSelectedIndexChanged="ddlBusinessType_SelectedIndexChanged" AutoPostBack="true" TabIndex="2">
                            </asp:DropDownList>
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
                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="220px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" TabIndex="3" >
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Remarks </strong>
                        </td>

                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="10" Rows="1"
                                Width="350px" TabIndex="4"></asp:TextBox>
                        </td>

                    </tr>
                </table>
                <asp:HiddenField runat="server" ID="hfGst" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="pnlDetail" runat="server">
            <ContentTemplate>
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
                                                                <tr>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td align="right">
                                                                        <strong>Discount In:</strong>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:RadioButtonList ID="rbDiscountType" runat="server" Width="105px" Height="1px"
                                                                            RepeatDirection="Horizontal" TabIndex="5">
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
                                                                        <asp:Label ID="Label3" runat="server" Width="74px" Text="Price" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblquantity" runat="server" Width="74px" Text="Carton" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" Width="74px" Text="Unit" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label4" runat="server" Width="74px" Text="Discount" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label5" runat="server" Width="74px" Text="Net Amount" CssClass="lblDetail"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlSKus" runat="server" Width="300px" TabIndex="6"
                                                                            onchange="SelectPrice();">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPrice" runat="server" Width="70px" TabIndex="7"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtQuantity" runat="server" Width="70px" TabIndex="8"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtQuantityUnit" runat="server" Width="70px" TabIndex="8"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDiscount" runat="server" Width="70px" TabIndex="9"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAmount" runat="server" Width="70px" ReadOnly="true" Enabled="false"></asp:TextBox>

                                                                    </td>
                                                                    <td>
                                                                        <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="85px"
                                                                            Font-Size="8pt" Text="Add" ValidationGroup="vg" CssClass="Button" TabIndex="10" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="7">
                                                                        <asp:Panel ID="Panel2" runat="server" Width="790px" Height="180px" ScrollBars="Vertical"
                                                                            BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                                            <asp:GridView ID="grdOrder" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderColor="White" ForeColor="SteelBlue" HorizontalAlign="Center" OnRowDeleting="grdOrder_RowDeleting"
                                                                                OnRowEditing="grdOrder_RowEditing" ShowHeader="False" Width="100%">
                                                                                <RowStyle ForeColor="Black" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="skuId" HeaderText="SKU_ID">
                                                                                        <HeaderStyle CssClass="HidePanel" />
                                                                                        <ItemStyle CssClass="HidePanel" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="skuName" HeaderText="SKU Name">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="300px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:f2}">
                                                                                        <ItemStyle CssClass="grdDetail" Width="65px" HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="qty" HeaderText="Carton">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Quantity" HeaderText="Unit">
                                                                                        <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
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
                                                                                                Text="Delete"></asp:LinkButton>
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
                                                    <td style="font-size: 15px; width: 65%">
                                                        <asp:Label ID="lblLedgerBalance" runat="server" Text="0.00"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblOpeningType" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Gross Amount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrossAmount" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Product Discount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductDiscount" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Discount Type</strong>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdbDiscountType" runat="server" Width="130px"
                                             RepeatDirection="Horizontal" onchange="CalculateAmount();" TabIndex="11">
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
                                            onkeypress="return onlyDotsAndNumbers(this,event);" TabIndex="12">

                                        </asp:TextBox>
                                    </td>
                                </tr>

                                <tr style="display: none;">
                                    <td>
                                        <strong>Total Quantity</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalQty" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalUnit" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Net Amount</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalAmount" runat="server"></asp:TextBox>
                                    </td>
                                    <td></td>
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
                    <asp:Button AccessKey="S" ID="btnSaveOrder" runat="server" Width="100px" Font-Size="8pt"
                        TabIndex="13" Text="Save" UseSubmitBehavior="False" OnClick="btnSaveOrder_Click"
                        CssClass="Button" />
                    <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="100px" Font-Size="8pt"
                        TabIndex="14" Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click"
                        CssClass="Button" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
