using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_RptNewPurchaseOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //LoadDistributor();
            //LoadTown();
            //LoadPrincipal();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
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
    private void ShowReport(int p_Type)
    {
        //if (drpDistributor.Items.Count > 0 && DrpTown.Items.Count > 0)
        //{
            //LoadCustomerOpBalance();
            DocumentPrintController DPrint = new DocumentPrintController();
            RptCustomerController RptCustomerCtl = new RptCustomerController();

            DataSet ds = null;
            {
                DataControl dc = new DataControl();
                try
                {
                    lblErrorMsg.Text = "";
                    ds = RptCustomerCtl.GetPurchaseOrder(DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),Constants.IntNullValue,0);
                    //DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
                    SAMSBusinessLayer.Reports.CrpNewPurchaseOrder CrpReport = new SAMSBusinessLayer.Reports.CrpNewPurchaseOrder();

                    CrpReport.SetDataSource(ds);
                    CrpReport.Refresh();
                    CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
                    CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
                //CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
                //CrpReport.SetParameterValue("Town_Name", DrpTown.SelectedItem.Text);
                //CrpReport.SetParameterValue("Customer", DrpPrincipal.SelectedItem.Text);
                //CrpReport.SetParameterValue("CustomerCode", hfCustomerCode.Value);
                //CrpReport.SetParameterValue("Address", "");
                ////CrpReport.SetParameterValue("Op_Balance", LoadCustomerOpBalance());
                //CrpReport.SetParameterValue("Op_Balance2", hfOpeningBal.Value);
                //CrpReport.SetParameterValue("Op_Type", opType);

                //CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
                CrpReport.SetParameterValue("CompanyName", "");
                Session.Add("CrpReport", CrpReport);
                    Session.Add("ReportType", p_Type);
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
            //}
        }
    }
}