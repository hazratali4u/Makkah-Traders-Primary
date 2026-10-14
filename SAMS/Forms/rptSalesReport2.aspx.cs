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
public partial class Forms_rptSalesReport2 : System.Web.UI.Page
{
    SKUPriceDetailController PController = new SKUPriceDetailController();
    public static string opType = "CR";
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
            this.LoadCustomer();
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadCustomer()
    {
        ddlCustomer.Items.Clear();        
        if (drpDistributor.Items.Count > 0 && ddlDivision.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dtCustomer = mController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), int.Parse(ddlDivision.SelectedValue), Constants.IntNullValue, Convert.ToInt32(ddlTown.SelectedValue),Constants.IntNullValue,Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()));
            ddlCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");            
        }
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
    private decimal LoadCustomerOpBalance()
    {
        try
        {
            if (drpDistributor.Items.Count > 0 && ddlCustomer.Items.Count > 0)
            {
                CustomerDataController mController = new CustomerDataController();
                DataTable dt = mController.SelectPrincipalCustomerOpBalance(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(ddlCustomer.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), Constants.IntNullValue);

                hfOpeningBal.Value = dt.Rows[0][0].ToString();

                if (Convert.ToDecimal(dt.Rows[0][0]) < 0)
                {
                    hfOpeningBal.Value = Convert.ToString(Math.Abs(Convert.ToDecimal(dt.Rows[0][0])));

                    opType = "DR";
                }
                else if (Convert.ToDecimal(dt.Rows[0][0]) == 0)
                {
                    opType = "";
                }
                else
                {
                    opType = "CR";
                }
                return decimal.Parse(dt.Rows[0][0].ToString());

            }
            return 0;
        }
        catch (Exception)
        {

            throw;
        }
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
        System.Text.StringBuilder sbCustomerID = new System.Text.StringBuilder();        
        DocumentPrintController DPrint = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        if (ddlCustomer.SelectedValue == Constants.IntNullValue.ToString())
        {
            foreach (ListItem li in ddlCustomer.Items)
            {
                sbCustomerID.Append(li.Value);
                sbCustomerID.Append(",");
            }
        }
        else
        {
            sbCustomerID.Append(ddlCustomer.SelectedValue);
        }
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptSaleCtl.GetSalesCustomerWise(int.Parse(drpDistributor.SelectedValue),Constants.IntNullValue, sbCustomerID.ToString(), Convert.ToInt32(ddlTown.SelectedValue), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        SAMSBusinessLayer.Reports.CrpSalesCustomerWise CrpReport = new SAMSBusinessLayer.Reports.CrpSalesCustomerWise();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        //CrpReport.SetParameterValue("Sector", ddlSector.SelectedItem.Text);
        //CrpReport.SetParameterValue("Division", ddlDivision.SelectedItem.Text);
        CrpReport.SetParameterValue("Town", ddlTown.SelectedItem.Text);
        CrpReport.SetParameterValue("Customer", ddlCustomer.SelectedItem.Text);
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
        this.LoadCustomer();
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSector();
        this.LoadAssignTown();
        this.LoadCustomer();
    }

    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadCustomer();
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadCustomer();
    }
}
