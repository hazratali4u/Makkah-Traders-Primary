using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Reports;
using CrystalDecisions.CrystalReports.Engine;

public partial class Forms_RptTargetReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            LoadLocation();
            LoadTown();
            LoadCustomer();
            DateTime dt = (DateTime)this.Session["CurrentWorkDate"];
            txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Regions
    /// </summary>


    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>


    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    protected void LoadLocation()
    {
        try
        {
            DistributorController DController = new DistributorController();
            DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            //  this.DrpLocation.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(this.DrpLocation, dt, 0, 2);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    /// <summary>
    /// Loads Locations
    /// </summary>
    /// 
    private void LoadTown()
    {
        if (DrpLocation.Items.Count > 0)
        {
            drpTown.Items.Clear();
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(DrpLocation.SelectedValue.ToString()));
            drpTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(drpTown, dt, 0, 1);  
        }
    }
    protected void DrpRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadLocation();
    }

    /// <summary>
    /// Shows Target Vs Achievement in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ShowReport(0);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

   
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ShowReport(1);
            //SAMSBusinessLayer.Classes.DocumentPrintController DPrint = new SAMSBusinessLayer.Classes.DocumentPrintController();
            //RptSaleController RptSaleCtl = new RptSaleController();

            //DataTable dt = DPrint.SelectReportTitle(int.Parse(DrpLocation.SelectedValue.ToString()));

            //SAMSBusinessLayer.Reports.CrpVolumeSalesDetail CrpReport = new SAMSBusinessLayer.Reports.CrpVolumeSalesDetail();

            //DataSet ds = null;

            //ds = RptSaleCtl.SelectVolumeSale(int.Parse(drpTown.SelectedValue.ToString()), int.Parse(DrpLocation.SelectedValue.ToString()), Convert.ToDateTime(txtFromdate.Text + " 00:00:00"), 0, int.Parse(this.Session["UserId"].ToString()), 0);
            //CrpReport.SetDataSource(ds);
            //CrpReport.Refresh();

            //CrpReport.SetParameterValue("Location", this.DrpLocation.SelectedItem.Text.ToString());
            //CrpReport.SetParameterValue("Principal", this.drpTown.SelectedItem.Text.ToString());
            //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            //CrpReport.SetParameterValue("TargetDate", txtFromdate.Text);

            //this.Session.Add("CrpReport", CrpReport);
            //this.Session.Add("ReportType", 1);
            //string url = "'Default.aspx'";
            //string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            //Type cstype = this.GetType();
            //ClientScriptManager cs = Page.ClientScript;
            //cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    public void ShowReport(int p_Type)
    {
        DateTime dtMonthFrom = new DateTime();
        DateTime dtMonth = DateTime.Parse(txtFromdate.Text);
        dtMonthFrom = new DateTime(dtMonth.Year, dtMonth.Month, 1);
        SAMSBusinessLayer.Classes.DocumentPrintController DPrint = new SAMSBusinessLayer.Classes.DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        DataTable dt = DPrint.SelectReportTitle(int.Parse(DrpLocation.SelectedValue.ToString()));
        ReportDocument CrpReport = new ReportDocument();
        if (RbtReportType.SelectedValue == "1")
        {
            CrpReport = new SAMSBusinessLayer.Reports.CrpTargerVsSale();
        }
        else
        {
            CrpReport = new SAMSBusinessLayer.Reports.CrpTargerVsSaleSummary();
        }
        DataSet ds = null;
        ds = RptSaleCtl.SelectTargetVsSale(int.Parse(DrpLocation.SelectedValue.ToString()), int.Parse(drpTown.SelectedValue.ToString()), int.Parse(DrpCustomer.SelectedValue), dtMonthFrom, int.Parse(RbtReportType.SelectedValue));
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Location", this.DrpLocation.SelectedItem.Text.ToString());
        CrpReport.SetParameterValue("Principal", this.drpTown.SelectedItem.Text.ToString());
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("TargetDate", txtFromdate.Text);
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", p_Type);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void drpTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        if (DrpLocation.Items.Count > 0 && drpTown.Items.Count > 0 && drpTown.SelectedValue != Constants.IntNullValue.ToString())
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dtCustomer = mController.SelectAllAssignedCustomer(int.Parse(DrpLocation.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, int.Parse(drpTown.SelectedValue.ToString()), 4);
            clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, 0, 3, false);
        }
    }
}