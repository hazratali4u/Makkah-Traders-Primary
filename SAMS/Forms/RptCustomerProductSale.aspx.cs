using System;
using System.Data;
using System.Web.UI;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Reports;

/// <summary>
/// Form For Customer Ledger Report
/// </summary>
public partial class RptCustomerProductSale : System.Web.UI.Page
{
    CustomerDataController CustCtl = new CustomerDataController();
    
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
        drpDistributor.Items.Clear();
           DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, false);
    }
 
    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        
        if (drpDistributor.Items.Count > 0)
        {
            if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0)
            {
                CustomerDataController mController = new CustomerDataController();
                DataTable dtCustomer = mController.SelectAllCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(DrpTown.SelectedValue.ToString()));
                DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtCustomer, 0, 3, false);

            }
        }
    }

 
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTown();
        LoadCustomer();
    }



    private void ShowReport(int p_Report_Type)
    {
        if (rbType.SelectedValue == "0")// CUstomer Wise
        {
            DocumentPrintController mDocumentPrntControl = new DocumentPrintController();
            RptSaleController RptSaleCtl = new RptSaleController();
            try
            {
                DataSet ds = RptSaleCtl.CustomerWiseSaleDetail(0, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTown.SelectedValue), int.Parse(DrpCustomer.SelectedValue), int.Parse(this.Session["UserID"].ToString()), DateTime.Parse(txtStartDate.Text)
                    , DateTime.Parse(txtEndDate.Text));

                DataTable dt = mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

                CustomerWiseSaleReport CrpReport = new CustomerWiseSaleReport();
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                CrpReport.SetParameterValue("fromDate", txtStartDate.Text);
                CrpReport.SetParameterValue("todate", txtEndDate.Text);
                CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("Town", DrpTown.SelectedItem.Text);
                CrpReport.SetParameterValue("ReportTitle", "Product Wise Customer Sale");
                CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", p_Report_Type);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception)
            {

                throw;
            }
        }
        else
        {
            DocumentPrintController mDocumentPrntControl = new DocumentPrintController();
            RptSaleController RptSaleCtl = new RptSaleController();
            try
            {
                DataSet ds = RptSaleCtl.CustomerWiseSaleDetail(0, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTown.SelectedValue), Constants.IntNullValue, int.Parse(this.Session["UserID"].ToString()), DateTime.Parse(txtStartDate.Text)
                    , DateTime.Parse(txtEndDate.Text));

                DataTable dt = mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

                TownWiseSaleReport CrpReport = new TownWiseSaleReport();
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                CrpReport.SetParameterValue("fromDate", txtStartDate.Text);
                CrpReport.SetParameterValue("todate", txtEndDate.Text);
                CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("Town", DrpTown.SelectedItem.Text);
                CrpReport.SetParameterValue("ReportTitle", "Product Wise Town Sale");
                CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", p_Report_Type);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception)
            {

                throw;
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
            DrpTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(DrpTown, dt, 0, 4, false);

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

    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(rbType.SelectedValue=="0")
        {
            lblCustomer.Visible = true;
            DrpCustomer.Visible = true;
        }
        else
        {

            lblCustomer.Visible = false;
            DrpCustomer.Visible = false;
        }
    }
}