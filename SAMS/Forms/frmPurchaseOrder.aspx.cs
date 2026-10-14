using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_frmPurchaseOrder : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly DataControl dc = new DataControl();
    private static int RowNo;
    private static int PrivouseQty, FreePrivousQty;
    DataTable PurchaseSKU;
    private static int mCustomerTypeId;
    private static int mCustomerVolClassId;
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

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtFromdate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtFromdate.Attributes.Add("readonly", "readonly");

            this.CreatTable();
            this.GetDocumentNo();
            this.LoadSKUDetail();
            //btnSave.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
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
    private void CreatTable()
    {
        PurchaseSKU = new DataTable();
        PurchaseSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchaseSKU.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_CODE", typeof(string));
        PurchaseSKU.Columns.Add("SKU_NAME", typeof(string));
        PurchaseSKU.Columns.Add("PACKSIZE", typeof(string));
        PurchaseSKU.Columns.Add("Quantity", typeof(int));
        PurchaseSKU.Columns.Add("QuantityCtn", typeof(int));
        PurchaseSKU.Columns.Add("UNITS_IN_CASE", typeof(int));
        this.Session.Add("PurchaseSKU", PurchaseSKU);
    }
    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
        DataTable dt = mPurchase.SelectPurchaseOrderNo(Constants.IntNullValue, 0);
        clsWebFormUtil.FillDropDownList(this.drpDocumentNo, dt, 0, 0, false);
    }
    private void LoadSKUDetail()
    {
        //if (drpPrincipal.Items.Count > 0)
        //{
        DataTable Dtsku_Price = PController.SelectSKUSforPurchaseOrder(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuUName", true);
        this.Session.Add("Dtsku_Price", Dtsku_Price);

        //DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
        //DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
        //if (foundRows.Length > 0)
        //{
        //    decimal mTradePrice = Math.Round(Convert.ToDecimal(foundRows[0]["DISTRIBUTOR_PRICE"]), 2);
        //    txtCurrentRate.Text = Convert.ToString(mTradePrice);
        //}
        //}
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseOrderNo(int.Parse(drpDocumentNo.SelectedValue.ToString()), 1);
        if (dt.Rows.Count > 0)
        {
            txtLocation.Text = dt.Rows[0]["LOCATION"].ToString();
            txtCustomer.Text = dt.Rows[0]["CUSTOMER"].ToString();
            txtMobile.Text = dt.Rows[0]["MOBIL_NO"].ToString();
            txtRemarks.Text = dt.Rows[0]["REMARKS"].ToString();
            txtFromdate.Text = DateTime.Parse(dt.Rows[0]["DATE"].ToString()).ToString("dd-MMM-yyyy");
            PurchaseSKU = mPurchase.SelecttblPurchaseOrderDetail(Constants.IntNullValue, int.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGrid();

        }
    }
    private void LoadGrid()
    {
        int TotalValue = 0;
        int TotalCtn = 0;
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        GrdPurchase.DataSource = PurchaseSKU;
        GrdPurchase.DataBind();
        foreach (DataRow dr in PurchaseSKU.Rows)
        {
            TotalValue += int.Parse(dr["Quantity"].ToString());
            TotalCtn += int.Parse(dr["QuantityCtn"].ToString());
        }
        txtTotalQuantity.Text = TotalValue.ToString();
        txtTotalCtn.Text = TotalCtn.ToString();
    }
    private bool CheckDublicateSKU()
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = PurchaseSKU.Select("SKU_Id  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }

        return false;

    }
    private void ClearAll()
    {
        txtQuantity.Text = "";
        txtQuantityUnit.Text = "";
        txtRemarks.Text = "";
        txtMobile.Text = "";
        txtLocation.Text = "";
        txtCustomer.Text = "";
        ddlSKuCde.Enabled = true;
        btnAdd.Text = "Add";
        PrivouseQty = 0;
        //FreePrivousQty = 0;
    }
    //private bool IsDayClosed()
    //{
    //    bool flag = false;
    //    DistributorController DistrCtl = new DistributorController();
    //    DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(drpDistributor.SelectedValue), 3);
    //    if (Convert.ToDateTime(Session["CurrentWorkDate"]) <= Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
    //    {
    //        flag = false;
    //    }
    //    else
    //    {
    //        flag = true;
    //    }

    //    return flag;
    //}
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            GetDocumentNo();
        }
        else
        {
            LoadDocumentDetail();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //if (txtFreightAmount.Text == "")
        //{
        //    txtFreightAmount.Text = "0";
        //}

        DataTable Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length > 0)
        {
            decimal Price = 0;
            //Price = Convert.ToDecimal(dc.chkNull_0(txtCurrentRate.Text));
            if (btnAdd.Text == "Add")
            {
                if (CheckDublicateSKU())
                {
                    DataRow dr = PurchaseSKU.NewRow();
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_NAME"] = foundRows[0]["SKU_NAME"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    //dr["BATCH_NO"] = "N/A";
                    //dr["TRADE_PRICE"] = foundRows[0]["TRADE_PRICE"];
                    //dr["PRICE"] = Price;

                    dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantityUnit.Text));
                    dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                    dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
                    //dr["AMOUNT"] = Price * ((Convert.ToDecimal(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(dc.chkNull_0(txtCtn.Text)))) + decimal.Parse(dc.chkNull_0(txtQuantity.Text)));

                    //if (hfIsPromotion.Value == "0")
                    //{
                    //    dr["FREE_SKU"] = 0;
                    //    dr["DISCOUNT"] = 0;
                    //}
                    //else
                    //{
                    //    dr["FREE_SKU"] = int.Parse(dc.chkNull_0(txtScheme.Text));
                    //    dr["DISCOUNT"] = Convert.ToDecimal(dr["AMOUNT"]) / 100 * Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text));
                    //}

                    PurchaseSKU.Rows.Add(dr);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('  " + ddlSKuCde.SelectedItem.Text + " Already Exists ');", true);
                    return;
                }
            }
            else if (btnAdd.Text == "Update")
            {
                DataRow dr = PurchaseSKU.Rows[RowNo];
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
                dr["SKU_NAME"] = foundRows[0]["SKU_NAME"];
                dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                //dr["BATCH_NO"] = "N/A";
                //dr["TRADE_PRICE"] = foundRows[0]["TRADE_PRICE"];

                //dr["PRICE"] = Price;
                dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantityUnit.Text));
                dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
                //dr["AMOUNT"] = Price * ((Convert.ToDecimal(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(dc.chkNull_0(txtCtn.Text)))) + decimal.Parse(dc.chkNull_0(txtQuantity.Text)));

                //if (hfIsPromotion.Value == "0")
                //{
                //    dr["FREE_SKU"] = 0;
                //    dr["DISCOUNT"] = 0;
                //}
                //else
                //{
                //    dr["FREE_SKU"] = int.Parse(dc.chkNull_0(txtScheme.Text));
                //    dr["DISCOUNT"] = Convert.ToDecimal(dr["AMOUNT"]) / 100 * Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text));
                //}
            }


            this.Session.Add("PurchaseSKU", PurchaseSKU);
            txtQuantity.Text = "";
            txtQuantityUnit.Text = "";
            //this.ClearAll();
            //DisAbaleOption(true);
            //btnCalculate_Click(null, null);
            this.LoadGrid();
            ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Wrong SKU Select');", true);
        }
    }

    #region Grid 

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RowNo = e.NewEditIndex;
        ddlSKuCde.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtQuantityUnit.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        //txtDiscount.Text = Convert.ToString((Convert.ToDecimal(GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text.ToString()) * 100) / Convert.ToDecimal(GrdPurchase.Rows[e.NewEditIndex].Cells[10].Text));
        PrivouseQty = int.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text);
        //FreePrivousQty = int.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text);
        //txtScheme.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        //txtCurrentRate.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        ddlSKuCde.Enabled = false;
        txtQuantity.Focus();
        btnAdd.Text = "Update";
    }
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            PurchaseSKU.Rows.RemoveAt(e.RowIndex);
            this.Session.Add("PurchaseSKU", PurchaseSKU);
            this.LoadGrid();
        }
        btnAdd.Text = "Add";
    }

    #endregion

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
                PurchaseController mController = new PurchaseController();
                DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKU"];
                if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
                {
                    bool mResult = mController.InserttblPurchaseOrder(txtLocation.Text, DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, txtCustomer.Text, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text, txtMobile.Text);
                    ShowReport();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record save successfully.');", true);

            }
            else
                {
                    bool mResult = mController.UpdatetblPurchaseOrder(int.Parse(drpDocumentNo.SelectedValue.ToString()), txtLocation.Text, DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, txtCustomer.Text, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text, txtMobile.Text);
                    
                    ShowReportPopup();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record update successfully.');", true);
            }
                PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
                PurchaseSKU.Rows.Clear();
                this.Session.Add("PurchaseSKU", PurchaseSKU);
                this.LoadGrid();
                this.GetDocumentNo();
                this.ClearAll();

            }
    }
    private bool CalculatePurchase()
    {
        decimal mTotalAmount = 0;
        PurchaseController mController = new PurchaseController();
        DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKU"];
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            bool mResult = mController.InserttblPurchaseOrder(txtLocation.Text, DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, txtCustomer.Text, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text, txtMobile.Text);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record save successfully.');", true);
            
            return mResult;
        }
        else
        {
            bool mResult = mController.UpdatetblPurchaseOrder(int.Parse(drpDocumentNo.SelectedValue.ToString()), txtLocation.Text, DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, txtCustomer.Text, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text, txtMobile.Text);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record update successfully.');", true);
            
            return mResult;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GetDocumentNo();
        this.CreatTable();
        this.LoadGrid();
        this.ClearAll();

    }
    private void ShowReport()
    {
        //if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0)
        //{
        DocumentPrintController DPrint = new DocumentPrintController();
        RptCustomerController RptCustomerCtl = new RptCustomerController();

        DataSet ds = null;
        {
            DataControl dc = new DataControl();
            try
            {
                ds = RptCustomerCtl.GetPurchaseOrder(Constants.DateNullValue, Constants.DateNullValue, Constants.IntNullValue, 2);
                //DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
                SAMSBusinessLayer.Reports.CrpNewPurchaseOrder CrpReport = new SAMSBusinessLayer.Reports.CrpNewPurchaseOrder();

                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                CrpReport.SetParameterValue("FromDate", DateTime.Parse(DateTime.Now.ToString()));
                CrpReport.SetParameterValue("To_date", DateTime.Parse(DateTime.Now.ToString()));
                CrpReport.SetParameterValue("CompanyName", "");
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                //lblErrorMsg.Text = ex.Message.ToString();
            }
            //}
        }
    }
    private void ShowReportPopup()
    {
        //if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0)
        //{
        DocumentPrintController DPrint = new DocumentPrintController();
        RptCustomerController RptCustomerCtl = new RptCustomerController();

        DataSet ds = null;
        {
            DataControl dc = new DataControl();
            try
            {
                ds = RptCustomerCtl.GetPurchaseOrder(Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDocumentNo.SelectedItem.Value.ToString()), 1);
                //DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
                SAMSBusinessLayer.Reports.CrpNewPurchaseOrder CrpReport = new SAMSBusinessLayer.Reports.CrpNewPurchaseOrder();

                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtFromdate.Text));
                CrpReport.SetParameterValue("To_date", DateTime.Parse(txtFromdate.Text));
                CrpReport.SetParameterValue("CompanyName", "");
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                //lblErrorMsg.Text = ex.Message.ToString();
            }
            //}
        }
    }
}