using CrystalDecisions.CrystalReports.Engine;
using SAMSBusinessLayer.Classes;
using SAMSBusinessLayer.Reports;
using SAMSCommonLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_RptPrintSaleInvoice : System.Web.UI.Page
{
    public static string opType = "CR";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
    private void LoadTown()
    {
        ddlTown.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));

            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, true, Constants.IntNullValue, Constants.Town, Constants.IntNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 4, false);
            //clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 1);
            //ddlTown.SelectedValue = "0";
        }
    }

    /// <summary>
    /// Loads Customers To Customer Combo
    /// </summary>
    //private void LoadCustomer()
    //{
    //    DrpCustomer.Items.Clear();
    //    DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
    //    if (drpDistributor.Items.Count > 0 && ddlTown.Items.Count > 0 && ddlTown.SelectedValue != Constants.IntNullValue.ToString())
    //    {
    //        CustomerDataController mController = new CustomerDataController();
    //        DataTable dt = mController.SelectAllCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(ddlTown.SelectedValue.ToString()));
    //        clsWebFormUtil.FillDropDownList(DrpCustomer, dt, 0, 3, false);

    //    }
    //}

    private void LoadCustomer()
    {
        DrpCustomer.Items.Clear();
        DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        if (drpDistributor.Items.Count > 0 && ddlTown.Items.Count > 0 && ddlTown.SelectedValue != Constants.IntNullValue.ToString())
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.SelectAllCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(ddlTown.SelectedValue.ToString()));
            clsWebFormUtil.FillDropDownList(DrpCustomer, dt, 0, 3, false);

        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTown();
    }

    /// <summary>
    /// Loads Customers
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
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

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Gets Print Sale Document And Shows Either in Excel or PDF
    /// </summary>
    /// <param name="p_ReportType">Type</param>
    private void ShowReport(int p_ReportType)
    {
        //LoadCustomerOpBalance();
        
        RptPrintSaleInvoiceController RptSaleCtl = new RptPrintSaleInvoiceController();
        DocumentPrintController DPrint = new DocumentPrintController();
        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        if (dt.Rows.Count > 0)
        {
            DataControl dc = new DataControl();
            ds = RptSaleCtl.SelectPrintInvoice(int.Parse(drpDistributor.SelectedValue.ToString()),Convert.ToInt32(ddlTown.SelectedValue), Convert.ToInt32(DrpCustomer.SelectedValue), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            ReportDocument CrpReport = new ReportDocument();
            CrpReport = new CrpPrintSaleInvoice();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("CUSTOMER", DrpCustomer.SelectedItem.Text);
            CrpReport.SetParameterValue("PRINCIPAL", ddlTown.SelectedItem.Text);
            CrpReport.SetParameterValue("PRINTED BY", Session["UserName2"].ToString());
            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", p_ReportType);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    /// <summary>
    /// Shows Print Sale Document in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }
}