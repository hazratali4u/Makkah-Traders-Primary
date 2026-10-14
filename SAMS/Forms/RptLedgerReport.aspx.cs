using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Reports;
using CrystalDecisions.CrystalReports.Engine;

/// <summary>
/// Form For General Ledger Report
/// </summary>
public partial class Forms_RptLedgerReport : System.Web.UI.Page
{

    static string opType = "CR";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.LoadAccountDetail();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

   
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        this.drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }

 
    private void LoadAccountDetail()
    {
        AccountHeadController mAccountController = new AccountHeadController();
        if (drpDistributor.SelectedValue == Constants.IntNullValue.ToString())
        {
           // DataTable dtHead = mAccountController.SelectAccountHead(Constants.IntNullValue, Constants.LongNullValue, 0);
            DataTable dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue);
            clsWebFormUtil.FillDropDownList(this.ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_NAME",true);
        }
        else
        {
            DataTable dtHead = mAccountController.SelectAccountHeadLocation(Constants.IntNullValue, Constants.LongNullValue, Convert.ToInt32(drpDistributor.SelectedValue));
            clsWebFormUtil.FillDropDownList(this.ddlAccountHead, dtHead, "ACCOUNT_HEAD_ID", "ACCOUNT_NAME",true);
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadAccountDetail();
    }

    private void showReport(int reportType)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();

        DataSet ds = null;

        DataControl dc = new DataControl();
        ds = RptAccountCtl.GeneralLedger_View(int.Parse(this.Session["UserID"].ToString()), Convert.ToInt64(ddlAccountHead.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()),
                       DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), Convert.ToInt32(rdbPostType.SelectedValue));

        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        decimal OpeningValue = RptAccountCtl.GeneralLedgerOpening(Constants.IntNullValue, Convert.ToInt64(ddlAccountHead.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()),
                  DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), Convert.ToInt32(rdbPostType.SelectedValue));

        if (OpeningValue > 0)
        {
            opType = "DR";
        }
        else if (OpeningValue < 0)
        {

            opType = "CR";
        }
        else
        {
            opType = "";
        }
        CrpLedgerView CrpReport = new CrpLedgerView();

        ReportDocument subReport = CrpReport.OpenSubreport("PostCheques");
        ReportDocument subReport2 = CrpReport.OpenSubreport("PostCheques2");

        CrpReport.SetDataSource(ds);
        subReport.SetDataSource(ds);
        subReport2.SetDataSource(ds);

        CrpReport.Refresh();

        //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("Principal", "All");
        CrpReport.SetParameterValue("opType", opType);
        CrpReport.SetParameterValue("OpeningValue", OpeningValue.ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }

  
}
