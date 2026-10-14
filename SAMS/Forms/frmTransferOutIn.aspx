<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmTransferOutIn.aspx.cs" Inherits="Forms_frmTransferOutIn" Title="SAMS :: Transfer In" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <div id="right_data">
         <div style="z-index: 101; left: 597px; width: 100px; position: absolute; top: 109px;
            height: 100px">
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id='messagediv' style="text-align: center">
                        <asp:ImageButton ID="ImageButton1" runat="server" Height="26px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                            Width="27px" />
                        Wait....
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>  
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tbody>
                         <tr>
                            <td align="left"></td>
                            <td style="height: 25px" align="left">
                                <strong>Transfer From</strong>
                            </td>
                            <td style="height: 25px">
                                <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                     Enabled="False" ></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td style="height: 25px; width: 100px" align="left">
                                <strong>Document No</strong>  </td>
                            <td style="height: 25px">
                                <asp:DropDownList ID="drpDocumentNo" runat="server" Width="200px" CssClass="DropList"
                                     AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td style="height: 25px" align="left">
                                <strong>Transfer to</strong>
                            </td>
                            <td style="height: 25px">
                                <asp:DropDownList ID="DrpTransferFor" runat="server" Width="200px" CssClass="DropList" Enabled="False">
                                </asp:DropDownList></td>
                        </tr>
                     
                        <tr>
                            <td style="height: 25px" align="left"></td>
                            <td style="height: 25px" align="left">
                                <strong>INV/DC No</strong></td>
                            <td style="height: 25px">
                                <asp:TextBox ID="txtDocumentNo" runat="server" Width="195px" CssClass="txtBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 25px" align="left"></td>
                            <td style="height: 25px" align="left">
                                <strong>Remarks</strong></td>
                            <td style="height: 25px">
                                <asp:TextBox ID="txtBuiltyNo" runat="server" Width="195px" CssClass="txtBox"></asp:TextBox></td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tbody>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Panel ID="Panel2" runat="server" Width="500px" 
                                    Height="200px" ScrollBars="Vertical" BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="White" CssClass="tblhead" ForeColor="SteelBlue" HorizontalAlign="Center" ShowHeader="true" Width="100%">

                                        <RowStyle ForeColor="Black" />
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                           
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Product Name">
                                                <ItemStyle CssClass="grdDetail"
                                                    Width="70%" />
                                                           <HeaderStyle CssClass="grdHead" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right"
                                                    Width="30%" />
                                                           <HeaderStyle CssClass="grdHead" />
                                            </asp:BoundField>
                                        </Columns>

                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
                &nbsp;&nbsp;
                <asp:Button AccessKey="S" ID="btnTransferIn" runat="server" Width="100px" Font-Size="8pt" Text="Transfer In"
                    UseSubmitBehavior="False" OnClick="btnTransferIn_Click" CssClass="Button"></asp:Button>
                <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="100px" Font-Size="8pt"
                    Text="Cancel" UseSubmitBehavior="False" CssClass="Button"></asp:Button>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
