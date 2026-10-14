using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using CrystalDecisions.CrystalReports.Engine;
public partial class Forms_rptDailyCollection : System.Web.UI.Page
{

    CustomerDataController CustCtl = new CustomerDataController();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadBusinessType();
            LoadRegion();
            LoadSector();
            LoadArea();
            LoadData();
            
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");

            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, false);
        }
    }
    private void LoadArea()
    {
      
        DrpRoute.Items.Clear();
        GeoHierarchyController gController = new GeoHierarchyController();
        if (drpSector.Items.Count > 0 && drpDistributor.Items.Count > 0)
        {
            DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpSector.SelectedValue), null, null, true, Constants.IntNullValue, Constants.Town, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(Session["CompanyId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DrpRoute.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(DrpRoute, dt, "GEO_ID", "GEO_NAME", false);
            }
        }
    }
    private void LoadRegion()
    {

        drpRegion.Items.Clear();
        GeoHierarchyController gController = new GeoHierarchyController();
        DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, true, Constants.IntNullValue, Constants.Zone, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(this.Session["CompanyId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            drpRegion.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(drpRegion, dt, "GEO_ID", "GEO_NAME", false);
        }
    }
    private void LoadSector()
    {
        drpSector.Items.Clear();
        GeoHierarchyController gController = new GeoHierarchyController();

        if (drpRegion.Items.Count > 0 && drpDistributor.Items.Count > 0)
        {
            DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpRegion.SelectedValue), null, null, true, Constants.IntNullValue, Constants.Territory, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(Session["CompanyId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                drpSector.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(drpSector, dt, "GEO_ID", "GEO_NAME", false);
            }
        }
    }
    private void LoadData()
    {
        DrpCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0 && drpDivision.Items.Count>0 && DrpRoute.Items.Count>0 && drpSector.Items.Count > 0 && drpRegion.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
          //  DataTable dt = mController.SelectAllCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue);
            DataTable dtCustomer = mController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), int.Parse(drpDivision.SelectedValue)
                , 2, int.Parse(DrpRoute.SelectedValue), int.Parse(drpSector.SelectedValue), int.Parse(drpRegion.SelectedValue), int.Parse(Session["UserId"].ToString()));
            if (dtCustomer.Rows.Count > 0)
            {
                DrpCustomer.Items.Add(new ListItem("All", Constants.LongNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, "CUSTOMER_ID", "CUSTOMER_NAME", false);
            }
        }
    }
    private void LoadBusinessType()
    {
        SLASHCodesController _customerCtrl = new SLASHCodesController();
        DataTable dt = _customerCtrl.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerTypeBusiness, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillDropDownList(drpDivision, dt, 0, 2, true);
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRegion();
        LoadSector();
        LoadArea();
        LoadData();
    }
    protected void drpDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
    protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSector();
        LoadArea();
        LoadData();
    }
    protected void drpSector_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadArea();
        LoadData();
    }
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
    protected void rdbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbReportType.SelectedValue == "0")
        {
            reportType.Visible = false;
        }
        else
        {
            reportType.Visible = true;
        }
    }
    private void ShowReport(int p_Report_Type)
    {
        RptCustomerController RptCustomerCtl = new RptCustomerController();

        DataSet ds;
        string dsName = "";
        try
        {
            if (rdbReportType.SelectedValue == "0")
            {
                ds = RptCustomerCtl.CustomerCollectionSummary(int.Parse(drpDistributor.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()),
                         DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), Convert.ToInt32(drpReportType.SelectedValue)
                         , int.Parse(drpDivision.SelectedValue), int.Parse(drpRegion.SelectedValue), int.Parse(drpSector.SelectedValue), int.Parse(DrpRoute.SelectedValue), int.Parse(Session["UserId"].ToString()));

                dsName = "spDailyCollectionSummary";
            }
            else
            {
                ds = RptCustomerCtl.CustomerCollection(int.Parse(drpDistributor.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()),
                          DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), Convert.ToInt32(drpReportType.SelectedValue)
                          , int.Parse(drpDivision.SelectedValue), int.Parse(drpRegion.SelectedValue), int.Parse(drpSector.SelectedValue), int.Parse(DrpRoute.SelectedValue), int.Parse(Session["UserId"].ToString()));
                dsName = "spDailyCollection";
            }
            if (ds.Tables[dsName].Rows.Count > 0)
            {
                dvMsg.Visible = false;
                DocumentPrintController DPrint = new DocumentPrintController();
                DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

                ReportDocument CrpReport = new ReportDocument();

                if (rdbReportType.SelectedValue == "0")
                {
                    CrpReport = new SAMSBusinessLayer.Reports.CrpDailyCollectionSummary();
                   
                }
                else
                {
                 CrpReport = new SAMSBusinessLayer.Reports.CrpDailyCollection();
                }
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();

                CrpReport.SetParameterValue("Division", drpDivision.SelectedItem.Text);
                CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
                CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("Type", drpReportType.SelectedItem.Text);
                //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", p_Report_Type);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            else
            {
                dvMsg.Visible = true;

            }
        }
        catch (Exception ex)
        {

            dvMsg.Visible = true;
        }
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
}