<%@ Page Title="SAMS :: GRN Q.A" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPurchaseQA.aspx.cs" Inherits="Forms_frmPurchaseQA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <div id="right_data">
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="width: 8%" align="right">
                                    <strong>
                                        <asp:Label ID="lblfromLocation" runat="server" Text="Purchase For:"></asp:Label></strong>
                                </td>
                                <td style="width: 1%">
                                </td>
                                <td style="width: 24%">
                                    <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 7%">
                                    <strong style="font-size: medium;">Vendor:</strong>
                                </td>
                                <td style="width: 28%">
                                    <asp:Label ID="lblVendorName" runat="server" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 7%">
                                    <strong style="font-size: medium;">GRN No:</strong>
                                </td>
                                <td style="width: 20%">
                                    <asp:Label runat="server" ID="lblNo" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel2" runat="server" Width="975px" Height="300px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="SteelBlue" HorizontalAlign="Center" Width="100%" OnRowDataBound="GrdPurchase_RowDataBound">
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle CssClass="grdDetail" Width="200px" />
                                                                <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="Market Name">
                                                                <ItemStyle CssClass="grdDetail" Width="200px" />
                                                                <HeaderStyle  CssClass="grdHead" HorizontalAlign="Left"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Order Qty">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                                 <HeaderStyle  CssClass="grdHead" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RcdQty" HeaderText="GRN Qty">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px" />
                                                                 <HeaderStyle  CssClass="grdHead" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="NG Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtngQty" runat="server" Width="75px" Text='<%# Eval("ngQty")%>'
                                                                        OnTextChanged="txtngQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                        FilterType="Custom" ValidChars="0123456789" TargetControlID="txtngQty">
                                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                                                 <HeaderStyle  CssClass="grdHead" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="InvQty" HeaderText="Inventory Qty">
                                                                   <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="75px"/>
                                                                 <HeaderStyle  CssClass="grdHead" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtremarks" runat="server" Width="220px" Text='<%# Eval("remarks")%>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" />
                                                                 <HeaderStyle  CssClass="grdHead" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exp Date" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtExpDate" runat="server" Width="75%" Text='<%# Eval("ExpDate")%>'
                                                                        onkeyup="BlockStartDateKeyPress()" MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ibExpDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                        Width="16px" />
                                                                    <ajaxToolkit:CalendarExtender ID="ceExpDate" runat="server" Format="dd-MMM-yyyy"
                                                                        PopupButtonID="ibExpDate" TargetControlID="txtExpDate">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PRICE">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="techId" >
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grdHead" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td style="vertical-align: top; display: none">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <strong>DC #</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDCNo"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>DC Date</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDCDate"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                            </asp:ImageButton>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" EnableViewState="False"
                                                                Format="dd-MMM-yyyy" PopupButtonID="ImgToDate" TargetControlID="txtDCDate">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Driver Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDriverName"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Vehicle No</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtVehicleNo"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="90%">
                    <tr>
                    <td align="right">
                         <strong>Total INV Quantity: </strong>
                        <asp:Label ID="txtTotalQuantity" runat="server"></asp:Label>
                    </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table>
                <tr>
                    <td colspan="2">
                        <strong>Remarks</strong>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="923px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Button AccessKey="S" ID="btnSaveDocument" runat="server" Width="119px" Font-Size="8pt"
                            Text="Save Document" UseSubmitBehavior="False" OnClick="btnSaveDocument_Click"
                            CssClass="Button" />
                        <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="120px" Font-Size="8pt"
                            Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="Button" />
                       
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
