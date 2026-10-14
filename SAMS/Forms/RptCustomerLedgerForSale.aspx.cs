using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_RptCustomerLedgerForSale : System.Web.UI.Page
{
    CustomerDataController CustCtl = new CustomerDataController();
    public static string opType = "CR";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadTown();
            LoadCustomer();
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
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }
    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCustomer = (DataTable)Session["dtCustomer"];
        DataRow[] foundRows = dtCustomer.Select("CUSTOMER_ID  = '" + DrpCustomer.SelectedValue + "'");

        if (foundRows.Length > 0)
        {
            hfCustomerCode.Value = foundRows[0]["CUSTOMER_CODE"].ToString();
            hfAddress.Value = foundRows[0]["ADDRESS"].ToString();
        }

    }
   
    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        hfCustomerCode.Value = "";
        hfAddress.Value = "";

        if (drpDistributor.Items.Count > 0)
        {
            DrpCustomer.Items.Clear();
            if (drpDistributor.Items.Count > 0 )
            {
                
                CustomerDataController mController = new CustomerDataController();
                DataTable dtCustomer = mController.SelectAllAssignedCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, Constants.IntNullValue,4);
                //DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, 0, 3, true);

                DataRow[] foundRows = dtCustomer.Select("CUSTOMER_ID  = '" + DrpCustomer.SelectedValue + "'");

                if (foundRows.Length > 0)
                {
                    hfCustomerCode.Value = foundRows[0]["CUSTOMER_CODE"].ToString();
                    hfAddress.Value = foundRows[0]["ADDRESS"].ToString();
                }
                Session.Add("dtCustomer", dtCustomer);
            }
        }
    }

    private decimal LoadCustomerOpBalance()
    {
        try
        {
            if (drpDistributor.Items.Count > 0 && DrpCustomer.Items.Count > 0)
            {
                CustomerDataController mController = new CustomerDataController();
                DataTable dt = mController.SelectPrincipalCustomerOpBalance(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpCustomer.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), Constants.IntNullValue);

                hfOpeningBal.Value = dt.Rows[0][0].ToString();

                if (Convert.ToDecimal(dt.Rows[0][0]) < 0)
                {
                    hfOpeningBal.Value = Convert.ToString(Math.Abs(Convert.ToDecimal(dt.Rows[0][0])));

                    opType = "DR";
                }
                else if (Convert.ToDecimal(dt.Rows[0][0]) == 0)
                {
                    opType = "DR";
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

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTown();
        LoadCustomer();
    }



    private void ShowReport(int p_Report_Type)
    {
        if (drpDistributor.Items.Count > 0 && DrpCustomer.Items.Count > 0)
        {
            LoadCustomerOpBalance();
            DocumentPrintController DPrint = new DocumentPrintController();
            RptCustomerController RptCustomerCtl = new RptCustomerController();
            var customer = DrpCustomer.SelectedItem.Text.Split('|');
            DataSet ds = null;
            {
                DataControl dc = new DataControl();
                try
                {
                    lblErrorMsg.Text = "";
                    ds = RptCustomerCtl.CustomerLedger_View2(Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpCustomer.SelectedValue.ToString()),
                                  DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));
                    DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
                    SAMSBusinessLayer.Reports.CrpCustomerLedgerForSale CrpReport = new SAMSBusinessLayer.Reports.CrpCustomerLedgerForSale();
                    //ReportDocument subReport = CrpReport.OpenSubreport("SubReport");
                    CrpReport.SetDataSource(ds);
                    //subReport.SetDataSource(ds);
                    CrpReport.Refresh();


                    CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
                    CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
                    CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                    CrpReport.SetParameterValue("Town_Name", DrpTown.SelectedItem.Text);
                    CrpReport.SetParameterValue("Customer", customer[0].ToUpper());
                    CrpReport.SetParameterValue("CustomerCode", hfCustomerCode.Value);
                    CrpReport.SetParameterValue("Address", hfAddress.Value.ToUpper());
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

    protected void DrpTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
}