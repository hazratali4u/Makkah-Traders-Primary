<%@ Page Language="C#" MasterPageFile="~/Forms/AppMaster.master" AutoEventWireup="true" CodeFile="frmTransferIn.aspx.cs" Inherits="Forms_frmTransferIn" Title="SAMS :: Transfer In" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainCopy" runat="Server">
    <div class="container" style="background-color: white">
        <h2>&nbsp; Customer&nbsp; Builty Information</h2>
    </div>
    <div class="container">
        <table width="100%">
            <tr>
                <td style="width: 100px;"></td>
                <td align="center">
                    <div style="z-index: 101; left: 477px; width: 100px; position: absolute; top: 276px; height: 100px">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                    Width="22px" />
                                Record Update
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblmsg" runat="server" Width="175px" ForeColor="Red"></asp:Label>&nbsp;
                                        </td>
                                        <td style="height: 17px"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px;" align="left">
                                            <asp:Label ID="Label1" runat="server" Width="58px" Text="Locaton" CssClass="lblbox"></asp:Label></td>
                                        <td align="left">
                                            <asp:DropDownList ID="DrpDistributor" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged"></asp:DropDownList></td>
                                        <td style="height: 10px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="height: 25px">
                                            <asp:ListBox ID="ListCustomer" runat="server" CssClass="DropList" Height="93px" onkeyup="SelectCode(event)"
                                                Width="373px" AutoPostBack="True" OnSelectedIndexChanged="ListCustomer_SelectedIndexChanged"></asp:ListBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 100px;"></td>
            </tr>
        </table>


    </div>
    <div class="container">
        <table width="100%">
            <tr>
                <td></td>
                <td align="center" style="width: 327px" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="height: 24px" rowspan="1"></td>
                                    <td style="height: 24px">
                                        <asp:GridView ID="GrdCreditLimit" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                            PageSize="8" Width="650px">
                                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                PreviousPageText="Previous" />
                                            <RowStyle ForeColor="Black" />
                                            <Columns>
                                                <asp:BoundField DataField="SALE_INVOICE_ID" HeaderText="Invoice No">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRINCIPAL" HeaderText="Principal">
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Transpoter Info">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DrpTranspoter" runat="server" CssClass="DropList" Width="170px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="D.C  No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDCno" runat="server" CssClass="txtBox " Width="92px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="HidePanel" />
                                                    <HeaderStyle CssClass="HidePanel" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Builty No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBiltyNo" runat="server" CssClass="txtBox " Width="98px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Freight">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFreight" runat="server" CssClass="txtBox " Width="126px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="White" />
                                            <PagerStyle BackColor="Transparent" />
                                            <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                VerticalAlign="Middle" />
                                            <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                        </asp:GridView>
                                    </td>
                                    <td style="height: 24px" rowspan="1"></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnSave" runat="server" Font-Size="8pt" OnClick="btnSave_Click" Text="Save"
                                ValidationGroup="vg" Width="82px" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 100px"></td>
                <td style="width: 327px"></td>
                <td style="width: 100px"></td>
            </tr>
        </table>
    </div>

</asp:Content>
