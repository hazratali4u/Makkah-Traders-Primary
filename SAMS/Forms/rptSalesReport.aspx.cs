using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Reports;

/// <summary>
/// Form For  SKU Price History Report
/// </summary>
public partial class Forms_rptSalesReport : System.Web.UI.Page
{
    SKUPriceDetailController PController = new SKUPriceDetailController();

    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadDistributor();
            this.LoadSector();
            this.LoadAssignTown();
            this.LoadDivision();
            this.LoadPrincipal();
            this.LoadSKUDetail();
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }

    /// <summary>
    /// Shows SKU Price History in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows SKU Price History in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }

    /// <summary>
    /// Shows SKU Price History Report in Excel Or PDF
    /// </summary>
    /// <param name="p_Report_Type">ReportType</param>
    private void ShowReport(int p_Report_Type)
    {
        System.Text.StringBuilder sbTownID = new System.Text.StringBuilder();        
        DocumentPrintController DPrint = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        if (ddlTown.SelectedValue == Constants.IntNullValue.ToString())
        {
            foreach (ListItem li in ddlTown.Items)
            {
                sbTownID.Append(li.Value);
                sbTownID.Append(",");
            }
        }
        else
        {
            sbTownID.Append(ddlTown.SelectedValue);
        }
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptSaleCtl.GetSalesProductWise(int.Parse(drpDistributor.SelectedValue),Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, sbTownID.ToString(), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        SAMSBusinessLayer.Reports.CrpSalesProductWise CrpReport = new SAMSBusinessLayer.Reports.CrpSalesProductWise();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        //CrpReport.SetParameterValue("Sector", ddlSector.SelectedItem.Text);
        //CrpReport.SetParameterValue("Division", ddlDivision.SelectedItem.Text);
        CrpReport.SetParameterValue("Town", ddlTown.SelectedItem.Text);
        //CrpReport.SetParameterValue("Vendor", DrpPrincipal.SelectedItem.Text);
        //CrpReport.SetParameterValue("Item", DrpSKU.SelectedItem.Text);
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("ToDate", txtEndDate.Text);
        //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        this.Session.Add("ReportType", p_Report_Type);
        this.Session.Add("CrpReport", CrpReport);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    /// <summary>
    /// Loads SKU Categories
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSKUDetail();
    }

    /// <summary>
    /// Loads SKUS To SKU Combo
    /// </summary>
    private void LoadSKUDetail()
    {
        DrpSKU.Items.Clear();
        SkuController mContoller = new SkuController();
        DataTable dt = mContoller.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(DrpPrincipal.SelectedValue), int.Parse(Session["CompanyId"].ToString()));
        DrpSKU.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpSKU, dt, 0, 18, false);
    }

    /// <summary>
    /// Loads SKU Categories To Category Combo
    /// </summary>
    private void LoadDivision()
    {
        SLASHCodesController _customerCtrl = new SLASHCodesController();
        DataTable dt = _customerCtrl.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerTypeBusiness, null, Constants.IntNullValue, bool.Parse("True"));
        ddlDivision.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(ddlDivision, dt, 0, 2);
    }

    protected void LoadSector()
    {
        GeoHierarchyController gController = new GeoHierarchyController();
        DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue,Constants.IntNullValue, Constants.IntNullValue, null, null, true, Constants.IntNullValue, Constants.Territory, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(this.Session["CompanyId"].ToString()));        
        ddlSector.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.ddlSector, dt, "GEO_ID", "GEO_NAME");
    }

    private void LoadAssignTown()
    {
        this.ddlTown.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            DistributorTownController gController = new DistributorTownController();
            DataTable dt = gController.SelectAssignTown(int.Parse(ddlSector.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), 1);
            ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(this.ddlTown, dt, 0, 1);
        }
    }
    
    protected void ddlSector_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadAssignTown();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSector();
        this.LoadAssignTown();
    }
}
