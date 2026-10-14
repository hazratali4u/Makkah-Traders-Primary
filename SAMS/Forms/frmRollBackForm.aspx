<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmRollBackForm.aspx.cs" Inherits="Forms_frmRollBackForm" Title="SAMS :: Rollback Transaction" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <div style="z-index: 101; left: 534px; width: 100px; position: absolute; top: 256px; height: 100px">
                            <asp:Panel ID="Panel2" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" Height="26px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                            Width="27px" />
                                        Wait Update.......
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>

                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <strong>
                                                <label style="width: 104px;">
                                                    Transaction Type</label></strong>
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpDocumentType" runat="server" Width="200px">
                                                <asp:ListItem Value="1">Sales Invoice</asp:ListItem>
                                                <%-- <asp:ListItem Value="2">Sales Return</asp:ListItem>--%>
                                                <asp:ListItem Value="3">Realize Cheques</asp:ListItem>
                                                <asp:ListItem Value="4">Opening Credit</asp:ListItem>
                                                <asp:ListItem Value="5">Online/Cash Receipts</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>
                                                <label>
                                                    Location</label></strong>
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>
                                                <label>
                                                    Legend</label></strong>
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:DropDownList ID="DrpLenged" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td align="left">
                                            <strong>From Date</strong></td>

                                        <td style="height: 25px" align="left">
                                            <asp:TextBox ID="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server" Width="180px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                    </tr>
                                    <tr>

                                        <td align="left">
                                            <strong>To Date</strong></td>
                                        <td style="height: 25px" align="left">
                                            <asp:TextBox ID="txtEndDate" onkeyup="BlockEndDateKeyPress()" runat="server" Width="180px" CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                    </tr>
                                    <tr>

                                        <td align="left"></td>
                                        <td align="left"></td>
                                        <td style="height: 25px" align="left">
                                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate" TargetControlID="txtStartDate">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate" TargetControlID="txtEndDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnGetOrder" OnClick="btnGetOrder_Click" runat="server" Width="100px"
                                                Font-Size="8pt" Text="Get Data" CssClass="Button" />
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:Button ID="btnPost" runat="server" Font-Size="8pt" Text="Rollback" Width="100px"
                                                OnClick="btnPost_Click" CssClass="Button" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset; width: 650px; border-bottom: silver thin inset">
                                    <tbody>
                                        <tr>
                                            <td style="height: 21px" align="left" colspan="5">
                                                <asp:Panel ID="Panel1" runat="server" Width="750px" Height="280px" ScrollBars="Vertical">
                                                    <asp:GridView ID="GrdOrder" runat="server" Width="730px" ForeColor="SteelBlue" DataKeyNames="Document_ID"
                                                        HorizontalAlign="Center" BorderColor="SteelBlue" BackColor="White" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer Id">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChbInvoice" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" Width="4%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="Code">
                                                                <ItemStyle CssClass="grdDetail" Width="8%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name">
                                                                <ItemStyle CssClass="grdDetail" Width="18%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MANUAL_INVOICE_ID" HeaderText="Document Id">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="10%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Document Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="12%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TOTAL_AMOUNT" DataFormatString="{0:F2}" HeaderText="Amount">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="12%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DISCOUNT_AMOUNT" DataFormatString="{0:F2}" HeaderText="Discount">
                                                                <ItemStyle CssClass="HidePanel" HorizontalAlign="Right" Width="8%" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SCHEME_AMOUNT" DataFormatString="{0:F2}" HeaderText="Scheme">
                                                                <ItemStyle CssClass="HidePanel" HorizontalAlign="Right" Width="8%" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="GST_AMOUNT" DataFormatString="{0:F2}" HeaderText="GST Amount">
                                                                <ItemStyle CssClass="HidePanel" HorizontalAlign="Right" Width="10%" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TOTAL_NET_AMOUNT" DataFormatString="{0:F2}" HeaderText="Net Amount">
                                                                <ItemStyle CssClass="HidePanel" HorizontalAlign="Right" Width="15%" />
                                                                <HeaderStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grdHead" Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:GridView>
                                                    <asp:GridView ID="GrdCheque" runat="server" Visible="False" Width="730px" ForeColor="SteelBlue"
                                                        CssClass="gridRow2" HorizontalAlign="Center" BorderColor="SteelBlue" BackColor="White"
                                                        AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="CHEQUE_PROCESS_ID" HeaderText="CHEQUE_PROCESS_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer Id">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle CssClass="grdDetail" Width="4%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChbInvoice" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="Code">
                                                                <ItemStyle CssClass="grdDetail" Width="10%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name">
                                                                <ItemStyle CssClass="grdDetail" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Voucher_No" HeaderText="Voucher No" Visible="False">
                                                                <ItemStyle CssClass="grdDetail" Width="4%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_NO" HeaderText="Cheque No">
                                                                <ItemStyle CssClass="grdDetail" Width="15%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_DATE" HeaderText="Cheque Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                                <ItemStyle CssClass="grdDetail" Width="12%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_AMOUNT" DataFormatString="{0:F2}" HeaderText="Cheque Amount">
                                                                <ItemStyle CssClass="grdDetail" Width="12%" HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="grdHead" Height="25px"></HeaderStyle>
                                                    </asp:GridView>

                                                    <asp:GridView ID="GridOpeningCredit" runat="server" Visible="false" Width="99.7%" ForeColor="SteelBlue"
                                                        AutoGenerateColumns="False" BackColor="White" BorderColor="SteelBlue">

                                                        <Columns>
                                                            <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle CssClass="grdDetail" Width="4%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChbInvoice" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer">
                                                                <ItemStyle CssClass="grdDetail" Width="30%"></ItemStyle>
                                                                <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Invoice Date">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Bind("DOCUMENT_DATE", "{0:dd-MMM-yyyy}") %>' ID="Label1"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                                                <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SALE_INVOICE_ID" HeaderText="Invoice No">
                                                                <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                                                <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Amount" DataFormatString="{0:f2}">
                                                                <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                                <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LEGEND_ID" HeaderText="LEGEND_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="DOCUMENT_DATE">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SALE_INVOICE_MASTER_ID" HeaderText="SALE_INVOICE_MASTER_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="REMARKS">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BUSINESS_TYPE">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>

                                                            <asp:CommandField ShowEditButton="True" ShowHeader="True" Visible="false">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="8%" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        CommandName="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="8%" />

                                                            </asp:TemplateField>

                                                        </Columns>

                                                        <HeaderStyle VerticalAlign="Middle" CssClass="grdHead"></HeaderStyle>
                                                    </asp:GridView>

                                                    <asp:GridView ID="GridOnlineCash" runat="server" Width="733px" ForeColor="SteelBlue" CssClass="gridRow2"
                                                        BorderColor="SteelBlue" HorizontalAlign="Center"
                                                        BackColor="White" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Voucher_no" HeaderText="Voucher_no">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="voucher_type_id" HeaderText="voucher_type_id">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Document_no" HeaderText="Document_no">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ACCOUNT_HEAD_ID" HeaderText="ACCOUNT_HEAD_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle CssClass="grdDetail" Width="4%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChbInvoice" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer">
                                                                <ItemStyle CssClass="grdDetail" Width="26%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ACCOUNT_NAME" HeaderText="Bank Name">
                                                                <ItemStyle CssClass="grdDetail" Width="12%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Bind("Ledger_date", "{0:dd-MMM-yyyy}") %>' ID="Label1"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" Width="7%"></ItemStyle>
                                                                <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Balance" DataFormatString="{0:F2}" HeaderText="Amount">
                                                                <ItemStyle CssClass="grdDetail" Width="7%" HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="tblhead"></HeaderStyle>
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
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
