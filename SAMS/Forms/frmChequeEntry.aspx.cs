using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System.Linq;

/// <summary>
/// Form To Add, Edit Cheques
/// </summary>
public partial class Forms_frmChequeEntry : System.Web.UI.Page
{
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
            DrpStatus.Items.Add(new ListItem("Cheque Received", "527"));
            DrpStatus.Items.Add(new ListItem("Cheque Deposit", "528"));
            DrpStatus.Items.Add(new ListItem("Cheque Realize", "529"));
            DrpStatus.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatus.Items.Add(new ListItem("Cheque Cancel", "560"));
            LoadAccountHead();
            LoadDistributor();
            this.LoadTown();
            LoadData();
            SelectCreditInvoice();
            LoadReceviedCheque();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            LoadPaymentRecieved();
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

    private void LoadPaymentRecieved()
    {
        ChequeEntryController CController = new ChequeEntryController();
        LedgerController LController = new LedgerController();


        if (drpDistributor.Items.Count > 0)
        {
            DataSet dsReceived = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, 21,
                DateTime.Parse(Session["CurrentWorkDate"].ToString()));

            DataTable dtRealized = dsReceived.Tables[2];

            if (dtRealized.Rows.Count > 0)
            {
                lblAmount.Text = string.Format("{0:0,0.00}", Convert.ToDecimal(dtRealized.Rows[0][0].ToString()));

            }

        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    private void LoadTown()
    {
        ddlTown.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(drpDistributor.SelectedValue.ToString()));
            ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 1);
        }
    }

    private void LoadData()
    {
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        DrpCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            if (DrpChequeType.SelectedIndex == 1)
            {
                DataTable dtCustomer = mController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, 3, Convert.ToInt32(ddlTown.SelectedValue),Constants.IntNullValue,Constants.IntNullValue, int.Parse(Session["UserId"].ToString()));
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");
            }
            else
            {
                LedgerController LedgerCtl = new LedgerController();
                DataTable dtCredit = LedgerCtl.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), Convert.ToInt32(ddlTown.SelectedValue), Constants.LongNullValue, 1);
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtCredit, 0, 1, true);
            }
        }
    }

    private void ClearAll()
    {
        txtChequeNo.Text = "";
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = "";
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtSlipNo.Text = "";
        txtRemarks.Text = "";

    }


    private void LoadReceviedCheque()
    {
        ChequeEntryController CController = new ChequeEntryController();
        if (DrpStatus.SelectedValue.ToString() != Constants.Cheque_Clear.ToString())
        {
            DataTable dt = CController.SelectChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), 0, DrpChequeType.SelectedIndex);
            Session.Add("dt", dt);
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
            decimal chqAmount = 0;
            if (dt != null)
            {
                foreach (DataRow gvr in dt.Rows)
                {
                    chqAmount += Convert.ToDecimal(gvr["CHEQUE_AMOUNT"]);
                }
                lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
            }
        }
        else
        {
            ///Load Cash Realization Detail
            if (Convert.ToInt32(DrpStatus.SelectedValue) < 530)
            {
                DataSet ds = CController.SelectChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), DrpChequeType.SelectedIndex);
                DataTable dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {


                    decimal realizeAmount = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        realizeAmount += Convert.ToDecimal(dr[2].ToString());
                    }
                    lblTotalAmount.Text = string.Format("{0:0.00}", realizeAmount);
                }
            }
            ////////////////////////////////////////////////
            DataTable dt = CController.SelectChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), 0, DrpChequeType.SelectedIndex);
            Session.Add("dt", dt);
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();

            foreach (GridViewRow gvr in GrdOrder.Rows)
            {
                LinkButton row = gvr.FindControl("btnEdit") as LinkButton;
                row.Enabled = false;
            }

        }
    }

    private void checkDuplication()
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("CUSTOMER_ID = '" + DrpCustomer.SelectedValue + "' and CHEQUE_NO='" + txtChequeNo.Text + "'");

        if (foundRows.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Cheque No Already exist against this customer!');", true);
        }
    }

    private void SelectCreditInvoice()
    {
        LedgerController CDC = new LedgerController();
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        if (DrpCustomer.Items.Count > 0 && DrpChequeType.SelectedIndex != 1)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, long.Parse(DrpCustomer.SelectedValue.ToString()), 0);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
        }
    }

    private void LoadAccountHead()
    {
        SAMSCommonLayer.Classes.Configuration.GetAccountHead();
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, long.Parse(SAMSCommonLayer.Classes.Configuration.BankDefaultType));
        clsWebFormUtil.FillDropDownList(DrpBankAccount, dt, 0, 4, true);
    }

    #endregion

    private void ChequeRealization(string payeeName)
    {
        LedgerController LController = new LedgerController();
        string MaxDocumentId = LController.SelectLedgerMaxDocumentId(Constants.Bank_Voucher, int.Parse(drpDistributor.SelectedValue.ToString()), 0);
        Session.Add("VoucherNo", MaxDocumentId);

        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        decimal realizeAmount = 0;
        //string remarks = "Chq# " + txtChequeNo.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        string remarks = txtRemarks.Text;
        if (DrpChequeType.SelectedIndex == 0)
        {

            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    string manualNo = "0";

                    if (Convert.ToString(dr.Cells[5].Text) == "opng")
                    {
                        manualNo = "opng";
                    }
                    if (decimal.Parse(dr.Cells[3].Text) >= OfferAmount)
                    {
                        realizeAmount += OfferAmount;

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), int.Parse(Configuration.AccountReceivable), int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                        DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks, DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, payeeName);

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                        DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks, DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, payeeName);

                        OfferAmount = decimal.Parse(dr.Cells[3].Text) - OfferAmount;
                        LController.UpdateSaleInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount);
                        break;
                    }
                    else if (decimal.Parse(dr.Cells[3].Text) <= OfferAmount)
                    {
                        realizeAmount += decimal.Parse(dr.Cells[3].Text);
                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), int.Parse(Configuration.AccountReceivable), int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(dr.Cells[3].Text),
                        DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks, DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "Group Payment");

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(dr.Cells[3].Text), 0,
                        DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks, DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), manualNo, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "Group Payment");

                        OfferAmount = OfferAmount - decimal.Parse(dr.Cells[3].Text);
                        LController.UpdateSaleInvoice(long.Parse(dr.Cells[1].Text), int.Parse(drpDistributor.SelectedValue.ToString()), 0);
                    }
                }
            }
            if (realizeAmount < decimal.Parse(txtAmount.Text))
            {

                LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), int.Parse(Configuration.AccountReceivable), int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                                      DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks , DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                                      txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "Group Payment");

                LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                DateTime.Parse(Session["CurrentWorkDate"].ToString()), remarks , DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "Group Payment");

            }
        }
        else
        {
            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), int.Parse(Configuration.AccountReceivable), int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(txtAmount.Text),
                DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtChequeNo.Text + "-Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");

            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtAmount.Text), 0,
                DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtChequeNo.Text + "-Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");
        }

    }

    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
        CController.DeleteChequeEntry(long.Parse(GrdOrder.Rows[e.RowIndex].Cells[0].Text));
        LoadReceviedCheque();
    }

    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;
            LoadData();
            DrpCustomer.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[1].Text;
            txtChequeNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[3].Text;
            txtBankName.Text = GrdOrder.Rows[e.NewEditIndex].Cells[4].Text;
            txtStartDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[5].Text;
            txtReceivedDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[6].Text;
            txtAmount.Text = GrdOrder.Rows[e.NewEditIndex].Cells[8].Text;
            txtSlipNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[9].Text;
            txtRemarks.Text = GrdOrder.Rows[e.NewEditIndex].Cells[10].Text;
            DrpBankAccount.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[13].Text;            
            btnSave.Text = "Update";
            SelectCreditInvoice();
            DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);
            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                foreach (DataRow dbr in dt.Rows)
                {
                    if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        chRelized.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    #region Sel/Index Change
    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

        SelectCreditInvoice();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadTown();
        LoadData();
        SelectCreditInvoice();
        LoadReceviedCheque();
    }

    protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {
            LoadReceviedCheque();
        }
    }
    protected void DrpChequeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
        SelectCreditInvoice();
        LoadReceviedCheque();
        ClearAll();
    }


    #endregion

    #region Click 

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ChequeEntryController CController = new ChequeEntryController();
            DateTime ChequeDate;
            try
            {
                if (txtStartDate.Text.Length == 10)
                {
                    ChequeDate = DateTime.Parse(ConvertDate.British_To_American(txtStartDate.Text));
                }
                else
                {
                    ChequeDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
                return;
            }
            if (DrpChequeType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
            {
                int InvoiceCount = Constants.IntNullValue;
                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        InvoiceCount++;
                        break;
                    }
                }
                if (InvoiceCount == Constants.IntNullValue)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must Select Invoice');", true);
                    return;
                }
            }
            if (btnSave.Text == "Save")
            {
                checkDuplication();
                if (DrpStatus.SelectedIndex == 0)
                {
                    HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedValue.ToString()), 0, long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate,
                        DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue, decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Now, DrpChequeType.SelectedIndex, txtSlipNo.Text, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()), 0
                        , Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue, Constants.IntNullValue);
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                        }
                    }
                }
                PrintReport(long.Parse(HFChqueProcessId.Value));
            }
            else
            {
                if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Pending && txtReceivedDate.Text == DateTime.Parse(Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), 0, long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
                       decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString())
                       , Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue, Constants.IntNullValue);
                    CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                        }
                    }
                    PrintReport(long.Parse(HFChqueProcessId.Value));
                }
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Deposit)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.DateNullValue,
                         Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString())
                         , Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue, Constants.IntNullValue);
                }
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Cancel)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()),
                                                     Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString())
                                                    , Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue, Constants.IntNullValue);
                }
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Clear)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()),
                                                     Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString())
                                                     , Constants.DecimalNullValue, Constants.ShortNullValue, Constants.DecimalNullValue, Constants.IntNullValue);
                    CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);
                    int Count = 0;
                    string PayeeName = "";
                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                            Count++;
                            if (Count > 1)
                            {
                                PayeeName = "Group Payment"; //in case multiple invoices selection
                                break;
                            }
                        }
                    }

                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {

                        }
                    }
                    ChequeRealization(PayeeName);
                    InsertGL();

                    Session.Remove("VoucherNo");
                    LoadData();
                    SelectCreditInvoice();
                }
            }
            ClearAll();
            LoadReceviedCheque();
            LoadPaymentRecieved();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];
        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 3:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 4:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                break;
        }
        GrdOrder.DataSource = dt.DefaultView;
        GrdOrder.DataBind();
    }

    #endregion

    private void InsertGL()
    {

        LedgerController LController = new LedgerController();

        Configuration.GetAccountHead();
        DataTable dtVoucher = new DataTable();
        dtVoucher.Columns.Add("LEDGER_ID", typeof(long));
        dtVoucher.Columns.Add("ACCOUNT_HEAD_ID", typeof(long));
        dtVoucher.Columns.Add("DEBIT", typeof(decimal));
        dtVoucher.Columns.Add("CREDIT", typeof(decimal));
        dtVoucher.Columns.Add("REMARKS", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));

        DataRow dr = dtVoucher.NewRow();
        dr["LEDGER_ID"] = Constants.LongNullValue.ToString();
        dr["ACCOUNT_HEAD_ID"] = Convert.ToInt64(DrpBankAccount.SelectedValue);
        dr["REMARKS"] = DrpBankAccount.SelectedItem.Text + " received from " + DrpCustomer.SelectedItem.Text;
        dr["DEBIT"] = decimal.Parse(txtAmount.Text);
        dr["CREDIT"] = 0;
        dr["Principal_Id"] = 0;
        dtVoucher.Rows.Add(dr);
        ///Credit Side Entry
        DataRow dr1 = dtVoucher.NewRow();
        dr1["LEDGER_ID"] = Constants.LongNullValue.ToString();
        dr1["ACCOUNT_HEAD_ID"] = Configuration.AccountReceivable;

        dr1["DEBIT"] = 0;
        dr1["CREDIT"] = decimal.Parse(txtAmount.Text);
        dr1["Principal_Id"] = 0;
        dr1["REMARKS"] = DrpBankAccount.SelectedItem.Text + " received from " + DrpCustomer.SelectedItem.Text;
        dtVoucher.Rows.Add(dr1);



        string MaxDocumentId = LController.SelectMaxVoucherId(Constants.Bank_Voucher, Convert.ToInt32(drpDistributor.SelectedValue), Convert.ToDateTime(Session["CurrentWorkDate"]));

        LController.Add_Voucher(Convert.ToInt32(drpDistributor.SelectedValue), 0, MaxDocumentId, Constants.Bank_Voucher, Convert.ToDateTime(Session["CurrentWorkDate"]), Constants.Bank_Deposit, Session["VoucherNo"].ToString(), "Defualt " + DrpBankAccount.SelectedItem.Text + " Voucher", Convert.ToDateTime(Session["CurrentWorkDate"]), txtChequeNo.Text, dtVoucher, Convert.ToInt32(Session["UserID"]), txtSlipNo.Text, Constants.DateNullValue, true);

    }


    private void PrintReport(long p_Ledger_Id)
    {

        // long DocumentID = long.Parse(Session["VoucherNo"].ToString());
        // int invoiceID = int.Parse(Session["VoucherNo"].ToString());
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        SAMSBusinessLayer.Reports.crpLedgerPrintChequeEntry CrpReport = new SAMSBusinessLayer.Reports.crpLedgerPrintChequeEntry();

        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        ds = RptAccountCtl.SelectCustomerRelizationForPrint(int.Parse(drpDistributor.SelectedValue.ToString()), p_Ledger_Id, Constants.LongNullValue, Constants.DateNullValue, Constants.Bank_Voucher, 1);
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("CompanyAddress", dt.Rows[0]["Address1"].ToString());
        CrpReport.SetParameterValue("Cell", dt.Rows[0]["CONTACT_NUMBER"].ToString());
        CrpReport.SetParameterValue("Fax", "");


        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);




    }

    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
        SelectCreditInvoice();
        LoadReceviedCheque();
    }
}