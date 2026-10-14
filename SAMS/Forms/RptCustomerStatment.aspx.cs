using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;

/// <summary>
/// Form For  Party Statement Report
/// </summary>
public partial class Forms_RptCustomerStatment : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDistributor();
            LoadBusinessType();
            LoadCustomer();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }

    

    private void LoadCustomer()
    {
        drpCustomer.Items.Clear();

        if (drpDistributor.Items.Count > 0 && DrpBusinessType.Items.Count > 0)
        {
            CustomerDataController mController = new CustomerDataController();
           // DataTable dt = mController.SelectAllCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, Convert.ToInt32(ddlChannelType.SelectedValue), 3);
            DataTable dtCustomer = mController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpBusinessType.SelectedValue), Constants.IntNullValue, Constants.IntNullValue);

            if (dtCustomer.Rows.Count > 0)
            {
                drpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                
               // clsWebFormUtil.FillDropDownList(drpCustomer, dtCustomer, "CUSTOMER_ID", "customerDetail");
            }
            clsWebFormUtil.FillDropDownList(drpCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");
        }
    }
    //private void LoadChannelType()
    //{
    //    SLASHCodesController mController = new SLASHCodesController();
    //    DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerChannelType, null, Constants.IntNullValue, bool.Parse("True"));
    //    ddlChannelType.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
    //    clsWebFormUtil.FillDropDownList(ddlChannelType, dt, 0, 2, false);
    //}
    private void LoadBusinessType()
    {
        SLASHCodesController _customerCtrl = new SLASHCodesController();
        DataTable dt = _customerCtrl.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerTypeBusiness, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillDropDownList(DrpBusinessType, dt, 0, 2, true);
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }

    protected void DrpBusinessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }

    private void showReport(int pType)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptCustomerController RptCustomerCtl = new RptCustomerController();

        DataSet ds = null;




        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        DataControl dc = new DataControl();
        ds = RptCustomerCtl.SelectPartystatment(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpCustomer.SelectedValue),
            int.Parse(DrpBusinessType.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        SAMSBusinessLayer.Reports.CrpCustomerStatment CrpReport = new SAMSBusinessLayer.Reports.CrpCustomerStatment();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("From_Date", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_Date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Customer", drpCustomer.SelectedItem.Text);
        CrpReport.SetParameterValue("Channel", DrpBusinessType.SelectedItem.Text);
        //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("ReportType", pType);
        Session.Add("CrpReport", CrpReport);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }

    /// <summary>
    /// Shows  Party Statement in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }
}
