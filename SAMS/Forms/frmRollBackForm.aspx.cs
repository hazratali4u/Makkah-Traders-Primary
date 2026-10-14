using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// Form To Rollback Order, Invoice, Sale Return And Realized Cheque
/// </summary>
public partial class Forms_frmRollBackForm : System.Web.UI.Page
{
    readonly LedgerController LController = new LedgerController();
    bool ViewAccess;
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewAccess = ValidateScreen();
        if (ViewAccess == false)
        {
            Response.Redirect("../Forms/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadDistributor();
          //  LoadOrderBooker();
            LoadLegend();

            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

        }
    }
    public bool ValidateScreen()
    {
        RoleManagementController mController = new RoleManagementController();
        int RoleID = (int)Session["RoleID"];
        DataTable dt = mController.GetAssignUnAssignModule(RoleID, Constants.IntNullValue, Constants.IntNullValue, 2);
        var PageURL = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
        string M_Key = Convert.ToString(PageURL.Replace("_", "/"));
        string[] reqArray = M_Key.Split('/');
        M_Key = reqArray[reqArray.Count() - 1];
        DataRow[] foundRows = dt.Select("MODULE_KEY  = '" + M_Key.Trim() + "'");
        if (foundRows.Length > 0)
        {
            return true;
        }
        return false;
    }
    #region Load

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }
    private void LoadLegend()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable m_dt = or.SelectLegend();
        clsWebFormUtil.FillDropDownList(DrpLenged, m_dt, 0, 2, true);
    }
    
    private void LoadRollbackDocument()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable dtOrder = or.SelectRollBackDocument(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue,
           Constants.IntNullValue, int.Parse(DrpDocumentType.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
        GrdOrder.DataSource = dtOrder;
        GrdOrder.DataBind();
    } 

    private void LoadRollbackCheque()
    {
        ChequeEntryController CController = new ChequeEntryController();
        DataTable dt = CController.SelectChequeEntryRollBack(Constants.Cheque_Clear , DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue,0);
        GrdCheque.DataSource = dt;
        GrdCheque.DataBind();       
    }

    #endregion

    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedValue == "3")
        {
            foreach (GridViewRow dr in GrdCheque.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    ChequeEntryController CController = new ChequeEntryController();
                    DataControl dc = new DataControl();
                    CController.RollbackChequeEntry(0, int.Parse(drpDistributor.SelectedValue.ToString()), dr.Cells[6].Text, long.Parse(dr.Cells[0].Text), dr.Cells[5].Text);
                }
            }
            LoadRollbackCheque();
        }
        else if (DrpDocumentType.SelectedValue == "4")
        {
            int Distibutor = int.Parse(drpDistributor.SelectedValue);
            foreach (GridViewRow dr in GridOpeningCredit.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(Configuration.ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand command = new SqlCommand("UPDATE SALE_INVOICE_MASTER SET IS_DELETED = 1 WHERE SALE_ORDER_ID  = -1 AND SALE_INVOICE_MASTER.IS_DELETED  = 0 AND (SALE_INVOICE_MASTER.SOLD_TO = " + dr.Cells[0].Text + " OR " + dr.Cells[0].Text + "IS NULL) AND MANUAL_INVOICE_ID ='" + dr.Cells[4].Text + "'", con))
                            {
                                command.ExecuteNonQuery();
                            }
                            using (SqlCommand command2 = new SqlCommand("UPDATE LEDGER SET IS_DELETED = 1 WHERE DISTRIBUTOR_ID  =" + Distibutor + " AND IS_DELETED  = 0 AND (CUSTOMER_ID =" + dr.Cells[0].Text + " OR " + dr.Cells[0].Text + "IS NULL) AND MANUAL_DOCUMENT_NO ='" + dr.Cells[4].Text + "'", con))
                            {
                                command2.ExecuteNonQuery();
                            }
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            LoadOpeningCredit();
        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            DataControl dc = new DataControl();
            foreach (GridViewRow dr in GridOnlineCash.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    long invoiceId = Convert.ToInt64(GridOnlineCash.Rows[dr.RowIndex].Cells[3].Text.Replace("&nbsp;", Constants.LongNullValue.ToString()));

                    if (LController.DeleteCashBankTransction(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(dc.chkNull_0(GridOnlineCash.Rows[dr.RowIndex].Cells[2].Text)),
                 int.Parse(dc.chkNull_0(GridOnlineCash.Rows[dr.RowIndex].Cells[1].Text)), invoiceId, decimal.Parse(dc.chkNull_0(GridOnlineCash.Rows[dr.RowIndex].Cells[9].Text))))
                    {
                        //SelectCreditInvoice();
                        //LoadGrid();
                        //SetTableSorter();
                       
                        //ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record roll back successfully.')", true);
                        //LoadOnlineCash();
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred.')", true);
                    }
                }
            }
            LoadOnlineCash();
        }
        else
        {
            foreach (GridViewRow dr in GrdOrder.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    OrderEntryController ORD = new OrderEntryController();
                    DataControl dc = new DataControl();
                    ORD.UpdateRollBackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["Document_ID"]), int.Parse(DrpDocumentType.SelectedValue.ToString()), int.Parse(DrpLenged.SelectedValue.ToString()), Convert.ToInt64(dr.Cells[4].Text));
                }
            }
            LoadRollbackDocument();
        }
    }
    private void LoadOpeningCredit()
    {
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController cdc = new CustomerDataController();
            DataTable dt = cdc.SelectOpeningCreditForRollBack(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.LongNullValue, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            GridOpeningCredit.DataSource = dt;
            GridOpeningCredit.DataBind();
        }
    }
    protected void btnGetOrder_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedValue == "3")
        {
            LoadRollbackCheque();
            GridOnlineCash.Visible = false;
            GrdOrder.Visible = false;
            GridOpeningCredit.Visible = false;
            GrdCheque.Visible = true;
        }
        else if (DrpDocumentType.SelectedValue == "4")
        {
            GridOnlineCash.Visible = false;
            GridOpeningCredit.Visible = true;
            GrdOrder.Visible = false;
            GrdCheque.Visible = false;
            LoadOpeningCredit();
        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            GridOnlineCash.Visible = true;
            GridOpeningCredit.Visible = false;
            GrdOrder.Visible = false;
            GrdCheque.Visible = false;
            LoadOnlineCash();
        }
        else
        {
            GridOnlineCash.Visible = false;
            GrdOrder.Visible = true;
            GrdCheque.Visible = false;
            GridOpeningCredit.Visible = false;
            LoadRollbackDocument();
        }
    }

    private void LoadOnlineCash()
    {
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dt = LController.SelectBankCashTransction2(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue,
                       DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text),1);
            Session.Add("dt", dt);
            GridOnlineCash.DataSource = dt;
            GridOnlineCash.DataBind();
        }
        else
        {
            //txttotalAmount.Text = "0.00";
        }
    }

}
