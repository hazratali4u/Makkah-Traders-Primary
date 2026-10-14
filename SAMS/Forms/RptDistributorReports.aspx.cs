using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Reports;

/// <summary>
/// Form For Sales & Closing Stock Report
/// </summary>
public partial class Forms_RptDistributorReports : System.Web.UI.Page
{
    static string Closinglocation = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           // DistributorType();
            LoadAssingned();
            LoadPrincipal();
            LoadSkus();
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
           
        }
        if (DrpReportType.SelectedValue == "4")
        {
            lblEndDate.Visible = false;
            txtEndDate.Visible = false;
            ibnEndDate.Visible = false;
            Label3.Text = "Date";
        }
    }
    
    private void DistributorType()
    {
        DistributorController dController = new DistributorController();
        DataTable dt = dController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }
    
    private void LoadAssingned()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Closinglocation += dr[0].ToString() + ',';
            }
        }

        //if (ddDistributorType.Items.Count > 0)
        //{
        //    drpDistributor.Items.Clear();    
        //    UserController mUserController = new UserController();
        //    DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), int.Parse(ddDistributorType.SelectedValue.ToString()), 1, int.Parse(Session["CompanyId"].ToString()));
        //    drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        //    clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 1);
        //}
    }
    
    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(DrpPrincipal, m_dt, 0, 1);
    }

    private void LoadSkus()
    {
        SkuController mSKUController = new SkuController();
        // DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        DataTable m_SKUDt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(DrpPrincipal.SelectedValue), int.Parse(Session["CompanyId"].ToString()));

        clsWebFormUtil.FillListBox(cblItem, m_SKUDt, "SKU_ID", "skuDetail", true);



        foreach (ListItem li in cblItem.Items)
        {
            li.Selected = true;
        }
    }
    
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssingned();
    }

    protected void cbAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in cblItem.Items)
        {
            li.Selected = cbAll.Checked;
        }
       
    }

    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSkus();
    }

    private void showReport(int type)
    {
        string SKuIDs = string.Empty;

        foreach (ListItem li in cblItem.Items)
        {
            if (li.Selected)
            {
                SKuIDs += li.Value + ",";
            }

        }
        DocumentPrintController mDocumentPrntControl = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        try
        {
            DataSet ds = RptSaleCtl.GetRegionSaleDetail(chkElimination.Checked, Closinglocation
                , int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text)
                , DateTime.Parse(txtEndDate.Text), Constants.IntNullValue, DrpReportType.SelectedIndex, DrpUnitType.SelectedIndex
                , int.Parse(Session["UserId"].ToString()), SKuIDs);

            DataTable dt = mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

            RegionWiseSaleReport CrpReport = new RegionWiseSaleReport();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            CrpReport.SetParameterValue("fromDate", txtStartDate.Text);
            CrpReport.SetParameterValue("todate", txtEndDate.Text);
            CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("Supplier", DrpPrincipal.SelectedItem.Text);
            CrpReport.SetParameterValue("ReportTitle", "Daily " + DrpReportType.SelectedItem.Text + " Report");
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", type);
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

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {

        showReport(0);

    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }
}
