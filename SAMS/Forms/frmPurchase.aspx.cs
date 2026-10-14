using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Reports;

public partial class Forms_frmPurchase : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly DataControl dc = new DataControl();
    
    DataTable PurchaseSKU;

    /// <summary>
    /// Page_Load Function Populates All Combos On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                LoadPrincipal();
                LoadDistributor();
                VendorLedger();
                LoadSKUDetail();
                CreatTable();
                GetDocumentNo();
                btnSave.Attributes.Add("onclick", "return ValidateForm();");
                Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];

                txtNetAmount.Attributes.Add("readonly", "readonly");
                txtGrossAmount.Attributes.Add("readonly", "readonly");
                txtTotalQuantity.Attributes.Add("readonly", "readonly");
                txtRate.Attributes.Add("disabled", "disabled");
                txtProductDiscount.Attributes.Add("readonly","readonly");
            }
            catch (Exception)
            {
                
                throw;
            }

        }
    }

    private void CreatTable()
    {
        PurchaseSKU = new DataTable();
        PurchaseSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchaseSKU.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKU.Columns.Add("Quantity", typeof(int));
        PurchaseSKU.Columns.Add("prevQty", typeof(int));
        PurchaseSKU.Columns.Add("PRICE", typeof(decimal));
        PurchaseSKU.Columns.Add("DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("DISCOUNT_TYPE", typeof(int));
        PurchaseSKU.Columns.Add("AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("NET_AMOUNT", typeof(decimal));
        Session.Add("PurchaseSKU", PurchaseSKU);

    }

    #region Check_Duplication

    //Stop replication for ref no
    private bool IsBillBookNoExist()
    {
        bool flag = false;
        if ((long.Parse(drpDocumentNo.SelectedValue) == Constants.LongNullValue || Session["hfBillBookNo"].ToString() != txtRefNo.Text) && txtRefNo.Text.Trim().Length > 0)
        {
            OrderEntryController OEC = new OrderEntryController();
            DataTable dtBillBookNo = OEC.SelectBillBookNo(int.Parse(drpDistributor.SelectedValue), txtRefNo.Text, 0);
            if (dtBillBookNo.Rows.Count > 0)
            {
                flag = true;
            }
        }
        return flag;
    }

    private bool CheckDublicateSKU()
    {

        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
        DataRow[] foundRows = PurchaseSKU.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }

        return false;

    }
    #endregion

    #region Load
    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();

        PurchaseController mPurchase = new PurchaseController();

        try
        {
            DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.Document_Purchase, Constants.IntNullValue, Constants.LongNullValue, int.Parse(this.Session["UserId"].ToString()), 0);
            drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(this.drpDocumentNo, dt, 0, 0);
        }
        catch (Exception)
        {
            
            throw;
        }

    }

    private void LoadPrincipal()
    {
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(drpPrincipal, m_dt, 0, 1, true);
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    private void LoadDocumentDetail()
    {
        try
        {
            PurchaseController mPurchase = new PurchaseController();
            DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue);
            if (dt.Rows.Count > 0)
            {

                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();//Purchase For this Location
                txtInvoiceNo.Text = dt.Rows[0]["INVOICE_NO"].ToString();
                txtRemarks.Text = dt.Rows[0]["BUILTY_NO"].ToString();
              //  txtProductDiscount.Text = string.Format("{0,0:0.00}", dt.Rows[0]["PRODUCT_DISCOUNT"]);
                txtInvoiceDiscount.Text = string.Format("{0,0:0.00}", dt.Rows[0]["DISCOUNT"]);
                rdbDiscountType.SelectedValue= dt.Rows[0]["VehicleNo"].ToString();

                PurchaseSKU = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
                Session.Add("PurchaseSKU", PurchaseSKU);

                LoadGird();
            }
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }

    private void LoadGird()
    {
        int TotalQty = 0;
        decimal GrossAmount = 0;
        decimal TotalDiscount = 0;
        decimal TotalNetAmount = 0;

        PurchaseSKU = (DataTable)Session["PurchaseSKU"];

        GrdPurchase.DataSource = PurchaseSKU;
        GrdPurchase.DataBind();

        foreach (DataRow dr in PurchaseSKU.Rows)
        {
            TotalQty += Convert.ToInt32(dr["Quantity"]);
            GrossAmount += Convert.ToDecimal(dr["AMOUNT"]);
            TotalNetAmount += decimal.Parse(dr["NET_AMOUNT"].ToString());
            TotalDiscount += decimal.Parse(dr["DISCOUNT"].ToString());

        }

        txtTotalQuantity.Text = TotalQty.ToString();
        txtGrossAmount.Text = Math.Round(GrossAmount, 2).ToString();
        txtProductDiscount.Text = string.Format("{0:0.00}", TotalDiscount);
        txtNetAmount.Text = string.Format("{0:0.00}", TotalNetAmount - Convert.ToDecimal(dc.chkNull_0(txtInvoiceDiscount.Text)));

    }

    private void LoadSKUDetail()
    {
        SkuController SKUCtl = new SkuController();
        try
        {
            if (drpPrincipal.Items.Count > 0)
            {
             //   DataTable Dtsku_Price = SKUCtl.SelectSkuData(Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(drpPrincipal.SelectedValue));
                DataTable Dtsku_Price = PController.SelectDataPrice(Convert.ToInt32(drpPrincipal.SelectedValue), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 2, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
                clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuDetail", true);

                if (Dtsku_Price != null)
                {
                    //clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, 0, 4, true);
                    clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuDetail", true);

                    Session.Add("Dtsku_Price", Dtsku_Price);
                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    #endregion

    #region Grid Operations

    /// <summary>
    /// Sets Dcoment Detail Data For Edit. This Function Runs When An Existing Document Detail Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

        hfRowNo.Value =Convert.ToString(e.NewEditIndex);
        ddlSKuCde.SelectedValue = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        
            txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
            txtRate.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[3].Text;
           
         txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;

        rbDiscountType.SelectedValue = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;

        if (rbDiscountType.SelectedValue == "1")
        {
            txtDiscount.Text = Convert.ToString((decimal.Parse(dc.chkNull_0(GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text)) * 100) / decimal.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text));
        }
        else
        {
            txtDiscount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        }
        txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;


        txtQuantity.Focus();
        btnSave.Text = "Update";

    }

    /// <summary>
    /// Deletes A Document Detail
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            PurchaseSKU.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGird();
        }
    }

    #endregion

    #region Click OPerations

    //Grid Operation
    protected void btnSave_Click(object sender, EventArgs e)
    {

        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
       
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");

        // int mPackSize = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));


        if (foundRows.Length > 0)
        {
            PurchaseSKU = (DataTable)Session["PurchaseSKU"];
            if (btnSave.Text == "Add")
            {
                if (CheckDublicateSKU())
                {

                    DataRow dr = PurchaseSKU.NewRow();
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];

                    dr["SKU_Name"] = ddlSKuCde.SelectedItem.Text;
                    // dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtRate.Text));
                    dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                    dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantity.Text));

                    dr["AMOUNT"] = (Convert.ToDecimal(dr["PRICE"]) * Convert.ToInt32(dr["Quantity"]));


                    dr["DISCOUNT_TYPE"] = int.Parse(rbDiscountType.SelectedValue);


                    if (rbDiscountType.SelectedValue == "1")
                    {
                        dr["DISCOUNT"] = (decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) * (decimal.Parse(dc.chkNull_0(txtDiscount.Text)) / 100));
                        dr["NET_AMOUNT"] = decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) - decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString()));
                    }
                    else
                    {
                        dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(txtDiscount.Text));
                        dr["NET_AMOUNT"] = decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) - decimal.Parse(dc.chkNull_0(txtDiscount.Text));
                    }

                    PurchaseSKU.Rows.Add(dr);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSKuCde.SelectedItem.Text + " Already Exists ');", true);
                    ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
                    return;
                }
            }
            else if (btnSave.Text == "Update")
            {
                DataRow dr = PurchaseSKU.Rows[Convert.ToInt32(hfRowNo.Value)];
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];

                dr["SKU_Name"] = ddlSKuCde.SelectedItem.Text;
                //  dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtRate.Text));
                dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantity.Text));

                dr["AMOUNT"] = (Convert.ToDecimal(dr["PRICE"]) * Convert.ToInt32(dr["Quantity"]));

                dr["DISCOUNT_TYPE"] = int.Parse(rbDiscountType.SelectedValue);
                if (rbDiscountType.SelectedValue == "1")
                {
                    dr["DISCOUNT"] = (decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) * (decimal.Parse(dc.chkNull_0(txtDiscount.Text)) / 100));
                    dr["NET_AMOUNT"] = decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) - decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString()));
                }
                else
                {
                    dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(txtDiscount.Text));
                    dr["NET_AMOUNT"] = decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString())) - decimal.Parse(dc.chkNull_0(txtDiscount.Text));
                }

            }
            Session.Add("PurchaseSKU", PurchaseSKU);
            ClearAll();
            LoadGird();
            DisAbaleOption(true);
            ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong SKU Select');", true);

        }
       
    }

    /// <summary>
    /// Saves All Document Detail Grid Data
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];

        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            DistributorController mDayClose = new DistributorController();
            if (CalculatePurchase(DateTime.Parse(Session["CurrentWorkDate"].ToString())))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record save successfully');", true);

                PurchaseSKU = (DataTable)Session["PurchaseSKU"];
                PurchaseSKU.Rows.Clear();

                Session.Add("PurchaseSKU", PurchaseSKU);

                //if (drpDocumentNo.SelectedValue != Constants.LongNullValue.ToString())
                //{
                //    Print(Convert.ToInt64(drpDocumentNo.SelectedValue));
                //}

              
                LoadGird();
                GetDocumentNo();

                ClearAll();
                ClearMaster();
                DisAbaleOption(false);

              
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' At least one SKU must enter');", true);

        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisAbaleOption(false);
        CreatTable();
        LoadGird();
        ClearAll();
        txtRefNo.Text = "";
        txtRemarks.Text = "";
    }

    #endregion

    #region Index/Change

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            Session.Add("PurchaseSKU", PurchaseSKU);
          
            LoadGird();
            ClearAll();
            ClearMaster();

            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            
        }
        else
        {
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;

            LoadDocumentDetail();
            LoadSKUDetail();
            VendorLedger();
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAll();
        GetDocumentNo();
    }

    /// <summary>
    /// Loads Document Detail To Document Detail Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    /// 
    protected void drpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAll();
        LoadSKUDetail();
        VendorLedger();
    }

    #endregion

    private void VendorLedger()
    {
        VenderEntryController _customerCtrl = new VenderEntryController();
        lblLedgerBalance.Text = "";
        if (drpPrincipal.Items.Count > 0)
        {
            decimal Opening = 0;
            DataTable dt = _customerCtrl.SelectVendorCreditBalance(Constants.LongNullValue,int.Parse(drpPrincipal.SelectedValue), Convert.ToInt32(drpDistributor.SelectedValue), Constants.Advance_PaymentOrder_id);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Opening = Opening + decimal.Parse(dc.chkNull_0(dt.Rows[0][0].ToString()));
                    if (drpDocumentNo.SelectedValue != Constants.LongNullValue.ToString())
                    {
                        lblLedgerBalance.Text = string.Format("{0:0,0.00}", Opening + Convert.ToDecimal(dc.chkNull_0(txtNetAmount.Text)));
                    }
                    else
                    {
                        lblLedgerBalance.Text = string.Format("{0:0,0.00}", Opening);
                    }
                }
            }
            if (Convert.ToDecimal(lblLedgerBalance.Text) < 0)
            {
                lblLedgerBalance.Text=Convert.ToString(Convert.ToDecimal(lblLedgerBalance.Text) * -1);
                lblOpeningType.Text = "CR";
            }
            else if(Convert.ToDecimal(lblLedgerBalance.Text) == 0)
            {
                lblOpeningType.Text = "";
            }
            else
            {
                lblOpeningType.Text = "DR";
            }

        }
    }
    private bool CalculatePurchase(DateTime CWorkDate)
    {
        bool flag = false;

        hfClosingDate.Value = Convert.ToString(CWorkDate);

        PurchaseController mController = new PurchaseController();
       
        DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];
        

        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
             long P_ID= mController.InsertPurchase(int.Parse(drpDistributor.SelectedValue.ToString()),txtInvoiceNo.Text.Trim()
            , Constants.Document_Purchase, CWorkDate, 0, int.Parse(drpDistributor.SelectedValue.ToString())
            , Convert.ToDecimal(dc.chkNull_0(txtGrossAmount.Text)), Convert.ToDecimal(dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(dc.chkNull_0(txtInvoiceDiscount.Text))
            , Convert.ToDecimal(dc.chkNull_0(txtNetAmount.Text)), false, dtPurchaseDetail, 0, txtRemarks.Text.Trim()
            , int.Parse(this.Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue),int.Parse(rdbDiscountType.SelectedValue));

             if (P_ID > 0)
             {
                 flag = true;
               //  Print(P_ID);
             }
        }
        else
        {
            flag = mController.UpdatePurchase(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
            , txtInvoiceNo.Text.Trim(), Constants.Document_Purchase, CWorkDate, 0, int.Parse(drpDistributor.SelectedValue.ToString())
            , Convert.ToDecimal(dc.chkNull_0(txtGrossAmount.Text)), Convert.ToDecimal(dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(dc.chkNull_0(txtInvoiceDiscount.Text))
            , Convert.ToDecimal(dc.chkNull_0(txtNetAmount.Text)), false, dtPurchaseDetail, 0, txtRemarks.Text.Trim()
            , int.Parse(this.Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue), int.Parse(rdbDiscountType.SelectedValue));

        }
        
        return flag;
    }

    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
          
            txtRefNo.Enabled = false;
        }
        else
        {
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
            txtRefNo.Enabled = true;
        }
    }

    private void ClearAll()
    {

        txtQuantity.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "0.00";

        txtDiscount.Text = "";

        btnSave.Text = "Add";
    }

    private void ClearMaster()
    {
        txtRemarks.Text = "";
        txtRefNo.Text = "";
        txtNetAmount.Text = "";
        txtInvoiceNo.Text = "";
        txtInvoiceDiscount.Text = "";
    }

    private void Print(long mResult)
    {
          
    RptInventoryController _rptInventoryCtl = new RptInventoryController();
    DocumentPrintController _dPrint = new DocumentPrintController();

        var crpReport = new CrpPurchaseVoucher();

        DataTable dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue));
        DataSet ds = _rptInventoryCtl.SelectPurchaseDocument(mResult, int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, DateTime.Parse(hfClosingDate.Value), DateTime.Parse(hfClosingDate.Value), 2);

        crpReport.SetDataSource(ds);
        crpReport.Refresh();

        crpReport.SetParameterValue("DocumentType", "Purchase Voucher");
        crpReport.SetParameterValue("Principal", drpPrincipal.SelectedItem.Text);
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const  string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);  


    }
}