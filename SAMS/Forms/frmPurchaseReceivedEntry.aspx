<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPurchaseReceivedEntry.aspx.cs" Inherits="Forms_frmPurchaseReceivedEntry"
    Title="SAMS :: Goods Receipt Note [GRN]" %>

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
                                <td style="width: 15%" align="right">
                                    <strong>
                                        <asp:Label ID="lblDocumentNo" runat="server" Text="P.O. Ref #:"></asp:Label></strong>
                                </td>
                                <td style="width: 1%">
                                </td>
                                <td style="width: 84%">
                                    <asp:DropDownList ID="drpDocumentNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged"
                                        Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" align="right">
                                    <strong>
                                        <asp:Label ID="lblfromLocation" runat="server" Text="Purchase For:"></asp:Label></strong>
                                </td>
                                <td style="width: 1%">
                                </td>
                                <td style="width: 84%">
                                    <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" Enabled="false">
                                    </asp:DropDownList>
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
                                                <asp:Panel ID="Panel2" runat="server" Width="740px" Height="300px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="SteelBlue"  HorizontalAlign="Center" Width="100%" OnRowDataBound="GrdPurchase_RowDataBound">
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle CssClass="grdDetail"  
                                                                    Width="210px" />
                                                                    <HeaderStyle HorizontalAlign="Left"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="Market Name">
                                                                <ItemStyle CssClass="grdDetail" 
                                                                    Width="210px" />
                                                                    <HeaderStyle HorizontalAlign="Left"/>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Batch No">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtBatchNo" runat="server" Width="100%" Text='<%# Eval("BATCH_NO")%>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Order Qty">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="GRN Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRecQty" runat="server" Width="100%" Text='<%# Eval("RcdQty")%>'
                                                                    OnTextChanged="txtRecQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                        FilterType="Custom" ValidChars="0123456789." TargetControlID="txtRecQty">
                                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
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
                                                            <asp:BoundField DataField="PRICE" HeaderText="SKU Price">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" />
                                                        <PagerStyle BackColor="Transparent" />
                                                        <HeaderStyle CssClass="grdHead" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td style="vertical-align: top;">
                                                <table>
                                                    <tr style="height:28px;">
                                                        <td>
                                                            <strong>DC #</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDCNo"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:28px;">
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
                                                    <tr style="height:28px;">
                                                        <td>
                                                            <strong>Driver Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDriverName"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:28px;">
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table>
            
            <tr>
                <td>
                    <asp:CheckBox ID="chkSend" Text="Send To Q.A" runat="server" Font-Bold="true"/>
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
                        <strong>
                            <asp:Label ID="Label7" runat="server" Width="103px" Height="16px" Text="Total Quantity"></asp:Label></strong>
                        <asp:TextBox ID="txtTotalQuantity"  runat="server" Width="88px"
                            CssClass="txtBox" ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
