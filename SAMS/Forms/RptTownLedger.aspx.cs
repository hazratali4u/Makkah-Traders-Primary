using System;
using System.Data;
using System.Web.UI;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

/// <summary>
/// Form For Customer Ledger Report
/// </summary>
public partial class Forms_RptTownLedger : System.Web.UI.Page
{
    CustomerDataController CustCtl = new CustomerDataController();
    public static string opType = "CR";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadTown();
            LoadPrincipal();
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
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, false);
    }
   

    private decimal LoadCustomerOpBalance()
    {
        try
        {
            if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0)
            {
                CustomerDataController mController = new CustomerDataController();
                DataTable dt = mController.SelectPrincipalTownOpBalance(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTown.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), int.Parse(DrpPrincipal.SelectedValue));

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
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTown();
       
    }

    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants .IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

    private void ShowReport(int p_Report_Type)
    {
        if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0 )
        {
            LoadCustomerOpBalance();
            DocumentPrintController DPrint = new DocumentPrintController();
            RptCustomerController RptCustomerCtl = new RptCustomerController();
           
            DataSet ds = null;
            {
                DataControl dc = new DataControl();
                try
                {
                    lblErrorMsg.Text = "";
                    ds = RptCustomerCtl.TownLedger(int.Parse(DrpPrincipal.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTown.SelectedValue.ToString()),
                                  DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
                    DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
                    SAMSBusinessLayer.Reports.CrpTownLedger CrpReport = new SAMSBusinessLayer.Reports.CrpTownLedger();
                    
                    CrpReport.SetDataSource(ds);
                    CrpReport.Refresh();
                    CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
                    CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
                    CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                    CrpReport.SetParameterValue("Town_Name", DrpTown.SelectedItem.Text);
                    CrpReport.SetParameterValue("Customer", DrpPrincipal.SelectedItem.Text);
                    CrpReport.SetParameterValue("CustomerCode", hfCustomerCode.Value);
                    CrpReport.SetParameterValue("Address", "");
                    CrpReport.SetParameterValue("Op_Balance", LoadCustomerOpBalance());
                    CrpReport.SetParameterValue("Op_Balance2", hfOpeningBal.Value);
                    CrpReport.SetParameterValue("Op_Type", opType);

                    CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
                    Session.Add("CrpReport", CrpReport);
                    Session.Add("ReportType", p_Report_Type);
                    const string url = "'Default.aspx'";
                    const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;
                    cs.RegisterStartupScript(cstype, "OpenWindow", script);
                }
                catch (Exception ex)
                {
                    lblErrorMsg.Text = ex.Message.ToString();
                }
            }
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
    private void LoadTown()
    {
        if (drpDistributor.Items.Count > 0)
        {
            GeoHierarchyController gController = new GeoHierarchyController();
            DrpTown.Items.Clear();

            DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, true, Constants.IntNullValue, Constants.Town, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(DrpTown, dt, 0, 4, true);

        }
        else
        {
            DrpTown.Items.Clear();
        }

    }

   
}