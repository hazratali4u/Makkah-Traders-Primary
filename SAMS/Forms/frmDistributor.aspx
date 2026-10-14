<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmDistributor.aspx.cs" Inherits="Forms_frmDistributor" Title="SAMS :: New Location" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $('#<%=txtgstno.ClientID %>').mask("99-99-9999-999-99");
        }

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtDistributorName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Distributor Name');
                return false;
            }
            return true;
        }
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div>
                    <table>
                        <tr>
                            <td>

                                <table>
                                    <tbody>

                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label3" runat="server" Width="83px" Text="Company" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="DrpCompanyName" runat="server" Width="209px" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="lbldesignationID" runat="server" Width="80px" Text="Location Type"
                                                        CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="ddDistributorType" runat="server" Width="208px" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" Width="77px" Text="Code" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 200px">
                                                <asp:TextBox ID="txtDistributorCode" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                            <td style="height: 25px"></td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lblNickName" runat="server" Width="69px" Text="Name" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtDistributorName" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label4" runat="server" Width="96px" Text="Contact Person" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 200px">
                                                <asp:TextBox ID="txtcontactperson" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                            <td style="height: 25px"></td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lblPhNo" runat="server" Width="76px" Text="Phone No" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtPhoneNo" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblAddress2" runat="server" Width="80px" Text="Address" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px" colspan="4">
                                                <asp:TextBox ID="txtAddress1" runat="server" Width="532px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 24px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblAddress1" runat="server" Width="91px" Text="Email Address" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 200px; height: 24px">
                                                <asp:TextBox ID="txtAddress2" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                            <td style="height: 24px"></td>
                                            <td style="height: 24px" align="left">
                                                <strong>Fax</strong>
                                            </td>
                                            <td style="height: 24px">
                                                <asp:TextBox ID="txtFax" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <asp:CheckBox ID="cbRegistered" runat="server" Width="97px" Text="Is Registered"
                                                    AutoPostBack="True" Font-Bold="true" OnCheckedChanged="cbRegistered_CheckedChanged"></asp:CheckBox>

                                            </td>
                                            <td style="width: 200px; height: 26px">
                                                <asp:TextBox ID="txtgstno" runat="server" Width="200px" CssClass="txtBox " Enabled="False"></asp:TextBox>

                                            </td>
                                            <td style="height: 25px"></td>
                                            <td style="height: 25px" align="left">
                                                <asp:CheckBox ID="chkIsActive" runat="server" Font-Bold="true" Width="97px" Text="Is Active"
                                                    Checked="True"></asp:CheckBox>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 143px; height: 32px" align="left"></td>
                                            <td style="width: 200px; height: 32px">
                                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="84px" Font-Size="8pt"
                                                    Text="Save" ValidationGroup="vg" CssClass="Button" />
                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Width="83px"
                                                    Font-Size="8pt" Text="Cancel" CssClass="Button" />
                                            </td>
                                            <td style="width: 1px; height: 32px"></td>
                                            <td style="height: 32px">
                                                <strong></td>
                                            <td style="width: 237px; height: 32px"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>

                                        <td>
                                            <table class="tblhead" width="970px">
                                                <tbody>
                                                    <tr>
                                                        <td style="color: White; font-weight: bold; width: 170px;">
                                                            <asp:Label ID="Label10" runat="server" Width="153px" Text="Select Searching Type"></asp:Label>
                                                        </td>
                                                        <td style="width: 170px; height: 21px">
                                                            <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                                                <asp:ListItem Value="SKU_code">All Records</asp:ListItem>
                                                                <asp:ListItem Value="Distributor_Code">Location Code</asp:ListItem>
                                                                <asp:ListItem Value="Distributor_Name">Location Name</asp:ListItem>
                                                                <asp:ListItem Value="Contact_Person">Contact Person</asp:ListItem>
                                                                <asp:ListItem Value="CONTACT_NUMBER">Contact No</asp:ListItem>
                                                                <asp:ListItem Value="TYPENAME">Location Type</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 224px; height: 21px">
                                                            <asp:TextBox ID="txtSeach" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                                        </td>
                                                        <td style="height: 21px; width: 250px">
                                                            <asp:Button ID="btnFilter" runat="server" Width="85px" Font-Size="8pt" Text="Filter" OnClick="btnFilter_Click" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td colspan="2">
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="200" Width="970px">
                                                <asp:GridView ID="GridDistributor" runat="server" Width="99.9%" ForeColor="SteelBlue"
                                                    CssClass="gridRow2" AutoGenerateColumns="False" BackColor="White" BorderColor="White"
                                                    OnPageIndexChanging="GridDistributor_PageIndexChanging" OnRowEditing="GridDistributor_RowEditing"
                                                    HorizontalAlign="Center">
                                                    <Columns>
                                                        <asp:BoundField DataField="DISTRIBUTOR_ID">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IS_REGISTERED">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SUBZONE_ID">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ADDRESS2">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DISTRIBUTOR_CODE" HeaderText="Code">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Name">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TYPENAME" HeaderText="Type">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ADDRESS1" HeaderText="Address" ItemStyle-Width="200px">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Cantact Person">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CONTACT_NUMBER" HeaderText="Contact Number">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GST_NUMBER" HeaderText="Gst Number">
                                                            <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COMPANY_ID">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FAX">
                                                            <ItemStyle CssClass="HidePanel" />
                                                            <HeaderStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ISDELETED" HeaderText="Status">
                                                            <ItemStyle Width="65px" HorizontalAlign="Left" BorderStyle="Solid"
                                                                BorderWidth="1px" BorderColor="Silver"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:CommandField ShowEditButton="True">
                                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Center"></ItemStyle>
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
