using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System.Linq;

/// <summary>
/// Form To Add Targets
/// </summary>
public partial class Forms_frmTargetEntry : System.Web.UI.Page
{
    private static long TargetId;
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
            LoadTown();
            LoadCustomer();
            LoadGird();
            //   LoadSKUDetail();
            //    btnSave.Attributes.Add("onclick", "return ValidateForm();");
            txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("MMM-yyyy");
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
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }
    
   
    
    //private void LoadSector()
    //{
    //    GeoHierarchyController GControler = new GeoHierarchyController();
    //   // DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
    //  //  DataTable dt = mController.SelectSkuHierarchy(Constants.SKUBrand, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()), 2);
    //    DataTable dt = GControler.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, true, Constants.IntNullValue, Constants.Territory, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(this.Session["CompanyId"].ToString()));

    //    clsWebFormUtil.FillDropDownList(DrpTargetFor, dt,"GEO_ID", "GEO_NAME", true);
    //}
    private void LoadTown()
    {
        if (drpDistributor.Items.Count > 0)
        {
            DrpTargetFor.Items.Clear();
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(drpDistributor.SelectedValue.ToString()));
           // DrpTargetFor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(DrpTargetFor, dt, 0, 1);

            //DistributorAreaController mController = new DistributorAreaController();
            //DataTable dt = mController.SelectDist_Area(Constants.LongNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, null, null);
            //DrpTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));    
            //clsWebFormUtil.FillDropDownList(DrpRoute, dt, 0, 6,true);    
        }
    }

    //private void LoadSKUDetail()
    //{
    //    SKUPriceDetailController PController = new SKUPriceDetailController();
        
    //        DataTable Dtsku_Price = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 2, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
    //        clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuDetail", true);
    //        Session.Add("Dtsku_Price", Dtsku_Price);
        
    //}
    
    private void LoadGird()
    {
        //if (txtFromdate.Text.Length > 1 && drpDistributor.Items.Count > 0 && DrpTargetFor.Items.Count > 0)
        //{
        //DateTime dtMonthFrom = new DateTime();
        //DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        //dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);

        //decimal TotalValue = 0;
        if (drpDistributor.Items.Count > 0 || drpDistributor.Items.Count > 0)
        {
            TargetController TG = new TargetController();
            DataTable dt = TG.SelectTarget2(int.Parse(this.Session["UserId"].ToString()), int.Parse(DrpTargetFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpCustomer.SelectedValue.ToString()));
            GrdPurchase.DataSource = dt;
            GrdPurchase.DataBind();
        }
        else
        {
            GrdPurchase.DataSource = null;
            GrdPurchase.DataBind();
        }
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        TotalValue += decimal.Parse(dr["AMOUNT"].ToString());

        //    }
        //  //  txtTotalAmt.Text = decimal.Round(TotalValue, 2).ToString();

        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some Selection is Wrong');", true);
        //}
    }
    
    private void ClearAll()
    {

        //txtUnitRate.Text = "";
        //txtQuantity.Text = "";
        //btnSave.Text = "Save";
        //ddlSKuCde.Enabled = true;
        txtRemarks.Text = "";
        txtTargetValue.Text = "";
        btnSaveTarget.Text = "Save";
        drpDistributor.Enabled = true;

    }
    
    
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        // LoadSKUDetail();
        LoadTown();
        LoadGird();

    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    TargetController TG = new TargetController();
    //    DataTable Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
    //    DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");

    //    DateTime dtMonthFrom = new DateTime();
    //    DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
    //    dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);

    //    if (btnSave.Text == "Save")
    //    {

    //        TG.InsertTarget(int.Parse(drpDistributor.SelectedValue.ToString()), dtMonthFrom,0, int.Parse(DrpTargetFor.SelectedValue.ToString())
    //           , int.Parse(foundRows[0]["SKU_ID"].ToString()), decimal.Parse(foundRows[0]["TRADE_PRICE"].ToString()), int.Parse(txtQuantity.Text), 0, int.Parse(this.Session["UserId"].ToString()));

    //    }
    //    else
    //    {

    //        TG.UpdateTarget(TargetId, int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtUnitRate.Text), int.Parse(txtQuantity.Text), true);
    //    }

    //    LoadGird();
    //    ClearAll();
    //    ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
    //}

    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        //DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dtCustomer = mController.SelectAllAssignedCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, int.Parse(DrpTargetFor.SelectedValue.ToString()), 4);
            clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, 0, 3, false);
        }
    }
    protected void DrpTargetFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
        LoadGird();
    }
    
    protected void btnTarget_Click(object sender, EventArgs e)
    {
        LoadGird();
    }




    protected void btnSaveTarget_Click(object sender, EventArgs e)
    {
        TargetController TG = new TargetController();
          DateTime dtMonthFrom = new DateTime();
          DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
          dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        if (btnSaveTarget.Text == "Save")
      {

                string T_targerid=    TG.InsertTarget(int.Parse(drpDistributor.SelectedValue.ToString()), dtMonthFrom,0, int.Parse(DrpTargetFor.SelectedValue.ToString())
                       ,0, int.Parse(txtTargetValue.Text), 0, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(this.Session["UserId"].ToString()),txtRemarks.Text);
            if (T_targerid == "-1")
            {
                
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Target is already set for this Customer for the selected month');", true);
                return;
            }
        }
                else
                {
            try
            {
             
                TG.UpdateTarget(TargetId, int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtTargetValue.Text), 0, true, txtRemarks.Text,long.Parse(DrpTargetFor.SelectedValue.ToString()), dtMonthFrom);
                //Response.Redirect("~/Forms/frmTargetEntry.aspx");
            }
            catch (Exception ex)
            {
                string s=ex.StackTrace;
            }
                }

                LoadGird();
                ClearAll();
        
    }


    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGird();
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
            txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("MMM-yyyy");
            return;
        }
        //LoadDocumentDetail();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(sender as LinkButton).NamingContainer;
        
    }

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        TargetId = long.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text);
        LoadTown();
        DrpTargetFor.SelectedValue = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
        LoadCustomer();
        DrpCustomer.SelectedValue = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        txtTargetValue.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtRemarks.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        drpDistributor.Enabled = false;
        btnSaveTarget.Text = "Update";
        txtFromdate.Text = Convert.ToDateTime(GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text).ToString("MMM-yyyy");
        //  txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[3].Text;
        // txtUnitRate.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        // btnSave.Text = "Update";
        // ddlSKuCde.Enabled = false;
    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        TargetController TG = new TargetController();
        TG.UpdateTarget(long.Parse(GrdPurchase.Rows[e.RowIndex].Cells[0].Text), int.Parse(drpDistributor.SelectedValue.ToString()), Constants.DecimalNullValue, Constants.IntNullValue, false, null, Constants.LongNullValue, Constants.DateNullValue);
        this.LoadGird();
    }
}
