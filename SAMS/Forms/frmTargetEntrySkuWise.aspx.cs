using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;

public partial class Forms_frmTargetEntrySkuWise : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    private static long TargetId;
    readonly DataControl dc = new DataControl();
    private static int RowNo;
    private static int PrivouseQty, FreePrivousQty;
    DataTable PurchaseSKU;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreatTable();
            LoadDistributor();
            LoadTown();
            LoadCustomer();
            LoadSKUDetail();
            //GetDocumentNo();
            //btnSave.Attributes.Add("onclick", "return ValidateForm();");
            txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("MMM-yyyy");
            LoadDocumentDetail();
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
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
    private void LoadTown()
    {
        ddlTown.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(drpDistributor.SelectedValue.ToString()));
            //ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 1);
        }
    }
    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        //DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        if (drpDistributor.Items.Count > 0 && ddlTown.Items.Count > 0 && ddlTown.SelectedValue != Constants.IntNullValue.ToString())
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dtCustomer = mController.SelectAllAssignedCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, int.Parse(ddlTown.SelectedValue.ToString()), 4);
            clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, 0, 3, false);
        }
    }
    private void LoadSKUDetail()
    {
        DataTable Dtsku_Price = PController.SelectSKUSforPurchaseOrder(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue), int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuUName", true);
        this.Session.Add("Dtsku_Price", Dtsku_Price);
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

        //TargetController TG = new TargetController();
        //DataTable dt = TG.SelectTarget2(int.Parse(this.Session["UserId"].ToString()), int.Parse(ddlTown.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()));
        //GrdPurchase.DataSource = dt;
        //GrdPurchase.DataBind();
    }

    private void ClearAll()
    {

        //txtUnitRate.Text = "";
        //txtQuantity.Text = "";
        //btnSave.Text = "Save";
        //ddlSKuCde.Enabled = true;
        txtRemarks.Text = "";
        //txtTargetValue.Text = "";
        //txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("MMM-yyyy");
        btnAdd.Text = "Save";
        drpDistributor.Enabled = true;
        //LoadTown();
        //LoadCustomer();
        //CreatTable();
        LoadGrid();

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


    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTown();
        LoadCustomer();
        LoadDocumentDetail();

    }
    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
        LoadDocumentDetail();
    }
    
    protected void btnSaveTarget_Click(object sender, EventArgs e)
    {
        TargetController TG = new TargetController();
        DateTime dtMonthFrom = new DateTime();
        DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        if (btnAdd.Text == "Save")
        {
            string T_targerid = TG.InsertTarget(int.Parse(drpDistributor.SelectedValue.ToString()), dtMonthFrom, 0, int.Parse(ddlTown.SelectedValue.ToString())
                   , 0, 0, 0, 0, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text);
            if (T_targerid == "-1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Target is already set for this town for the selected month');", true);
                return;
            }
        }
        else
        {
            try
            {

                TG.UpdateTarget(TargetId, int.Parse(drpDistributor.SelectedValue.ToString()), 0, 0, true, txtRemarks.Text, long.Parse(ddlTown.SelectedValue.ToString()), dtMonthFrom);
            }
            catch (Exception ex)
            {
                string s = ex.StackTrace;
            }
        }

        //LoadGird();
        ClearAll();
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length > 0)
        {
            decimal Price = 0;
            if (btnAdd.Text == "Add")
            {
                if (CheckDublicateSKU())
                {
                    DataRow dr = PurchaseSKU.NewRow();
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_NAME"] = foundRows[0]["SKU_NAME"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantityUnit.Text));
                    dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                    dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
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
                dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantityUnit.Text));
                dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
            }


            this.Session.Add("PurchaseSKU", PurchaseSKU);
            txtQuantity.Text = "";
            txtQuantityUnit.Text = "";
            LoadGrid();
            ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Wrong SKU Select');", true);
        }
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
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {

        DateTime dtMonthFrom = new DateTime();
        DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        DateTime dtCurMonthFrom = new DateTime();
        DateTime dtCurMonth = Convert.ToDateTime(Session["CurrentWorkDate"]);
        dtCurMonthFrom = new DateTime(dtCurMonth.Year, dtCurMonth.Month, 1);
        if (dtMonthFrom < dtCurMonthFrom)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Targets could not enter in previous months.');", true);
            return;
        }
        SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            PurchaseController mController = new PurchaseController();
            DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKU"];
            if (hfTARGET_MASTER_ID.Value == null || hfTARGET_MASTER_ID.Value == "")
            {
                bool mResult = mController.InsertTARGET_MASTER(int.Parse(drpDistributor.SelectedValue),Constants.IntNullValue,int.Parse(ddlTown.SelectedValue), int.Parse(DrpCustomer.SelectedValue), DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text);
                //ShowReport();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record save successfully.');", true);

            }
            else
            {
                bool mResult = mController.UpdateTARGET_MASTER(long.Parse(hfTARGET_MASTER_ID.Value), int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, int.Parse(ddlTown.SelectedValue), int.Parse(DrpCustomer.SelectedValue), DateTime.Parse(txtFromdate.Text), dtPurchaseDetail, int.Parse(this.Session["UserId"].ToString()), txtRemarks.Text);
                //ShowReportPopup();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record update successfully.');", true);
            }
            PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
            PurchaseSKU.Rows.Clear();
            this.Session.Add("PurchaseSKU", PurchaseSKU);
            this.LoadGrid();
            //this.GetDocumentNo();
            //this.ClearAll();
            LoadDocumentDetail();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        //{
        //    GetDocumentNo();
        //}
        //else
        //{
        //    LoadDocumentDetail();
        //}
    }

    //private void GetDocumentNo()
    //{
    //    drpDocumentNo.Items.Clear();
    //    DateTime MWorkDate = System.DateTime.Now;
    //    PurchaseController mPurchase = new PurchaseController();
    //    drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
    //    DataTable dt = mPurchase.SelectTargetNo(Constants.IntNullValue, 0);
    //    clsWebFormUtil.FillDropDownList(this.drpDocumentNo, dt, 0, 0, false);
    //}
    private void LoadDocumentDetail()
    {
        DateTime dtMonthFrom = new DateTime();
        DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        PurchaseController mPurchase = new PurchaseController();
        if (drpDistributor.Items.Count > 0 && ddlTown.Items.Count > 0 && DrpCustomer.Items.Count > 0)
        {
            DataTable dt = mPurchase.SelectTargetNo(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(ddlTown.SelectedValue.ToString()), int.Parse(DrpCustomer.SelectedValue.ToString()), dtMonthFrom, 1);
            if (dt.Rows.Count > 0)
            {
                hfTARGET_MASTER_ID.Value = dt.Rows[0]["TARGET_MASTER_ID"].ToString();
                //drpDistributor.SelectedValue = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
                //LoadTown();
                //ddlTown.SelectedValue = dt.Rows[0]["TOWN_ID"].ToString();
                //LoadCustomer();
                //DrpCustomer.SelectedValue = dt.Rows[0]["CUSTOMER_ID"].ToString();
                txtRemarks.Text = dt.Rows[0]["REMARKS"].ToString();
                //txtFromdate.Text = DateTime.Parse(dt.Rows[0]["TARGET_MONTH"].ToString()).ToString("dd-MMM-yyyy");
                PurchaseSKU = mPurchase.SelectTargetDetail(Constants.IntNullValue, int.Parse(dt.Rows[0][0].ToString()));
                Session.Add("PurchaseSKU", PurchaseSKU);
                LoadGrid();

            }
            else
            {
                hfTARGET_MASTER_ID.Value = null;
                txtRemarks.Text = "";
                //GrdPurchase.DataSource = null;
                //GrdPurchase.DataBind();
                CreatTable();
                LoadGrid();
                //this.ClearAll();
            }
        }
        else
        {
            hfTARGET_MASTER_ID.Value = null;
            txtRemarks.Text = "";
            //GrdPurchase.DataSource = null;
            //GrdPurchase.DataBind();
            CreatTable();
            LoadGrid();
        }
        
    }

    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocumentDetail();
    }

    protected void txtFromdate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtMonthFrom = new DateTime();
        DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        DateTime dtCurMonthFrom = new DateTime();
        DateTime dtCurMonth = Convert.ToDateTime(Session["CurrentWorkDate"]);
        dtCurMonthFrom = new DateTime(dtCurMonth.Year, dtCurMonth.Month, 1);
        if (dtMonthFrom < dtCurMonthFrom)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Targets could not enter in previous months.');", true);
            return;
        }
        LoadDocumentDetail();
    }
}