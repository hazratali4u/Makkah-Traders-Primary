using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSCommonLayer.Classes;
using SAMSBusinessLayer.Classes;
using System.Data;
using System.Linq;

public partial class Forms_frmSaleReturnView : System.Web.UI.Page
{

    readonly OrderEntryController oeCtrl = new OrderEntryController();
    private DataTable m_SKUDt;
    bool ViewAccess;
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewAccess = ValidateScreen();
        if (ViewAccess == false)
        {
            Response.Redirect("../Forms/AccessDenied.aspx");
        }
        if (!Page.IsPostBack)
        {
            ltrlNew.Text = "<a href='frmSaleReturn.aspx?LevelID=" + Request["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=26'>Add Sale Return</a>";

            Session.Remove("SaleReturnId");
            Session.Remove("dtSaleReturn");

            if (Session["reportId"] != null)
            {
                GetInvoice(Convert.ToInt64(Session["reportId"].ToString()));
            }
            Session.Remove("reportId");

            LoadData();
            LoadGrid();
        }
    }
    public bool ValidateScreen()
    {
        RoleManagementController mController = new RoleManagementController();
        int RoleID = (int)Session["RoleID"];
        DataTable dt = mController.GetAssignUnAssignModule(RoleID, Constants.IntNullValue, Constants.IntNullValue, 2);
        var PageURL = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
        string M_Key = Convert.ToString(PageURL.Replace("_", "/"));
        string[] reqArray = M_Key.Split('/');
        M_Key = reqArray[reqArray.Count() - 1];
        DataRow[] foundRows = dt.Select("MODULE_KEY  = '" + M_Key.Trim() + "'");
        if (foundRows.Length > 0)
        {
            return true;
        }
        return false;
    }
    private void LoadData()
    {
        DataTable dtSaleReturn = oeCtrl.SelectSaleReturn(Constants.LongNullValue,Convert.ToInt32(Session["Distributor_id"].ToString()), 1, int.Parse(Session["UserID"].ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));        
        Session.Add("dtSaleReturn", dtSaleReturn);
    }
    private void LoadGrid()
    {
        m_SKUDt = (DataTable)Session["dtSaleReturn"];
        if (m_SKUDt != null)
        {

            switch (ddSearchType.SelectedIndex)
            {

                case 1:
                    m_SKUDt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    m_SKUDt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 3:
                    m_SKUDt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    m_SKUDt.DefaultView.RowFilter = "CUSTOMER_NAME" + " like '%" + "" + "%'";
                    break;
            }

            grdSaleOrder.DataSource = m_SKUDt.DefaultView;
            grdSaleOrder.DataBind();
        }
    }
    protected void grdSaleOrder_RowCommand(object sender, GridViewCommandEventArgs e)

    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int index = gvr.RowIndex;

            if (e.CommandName == "Edt")
            {

                Session.Add("SaleReturnId", grdSaleOrder.Rows[index].Cells[0].Text);

                Response.Redirect("frmSaleReturn.aspx?LevelID=" + Request["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID="+ Request.QueryString["TopID"].ToString());
            }
            else if (e.CommandName == "Del")
            {
                bool flag = oeCtrl.Update_SaleReturn(Convert.ToInt64(grdSaleOrder.Rows[index].Cells[0].Text), int.Parse(Session["Distributor_Id"].ToString()),
                    true, int.Parse(Session["UserId"].ToString()), 2);

                if (flag)
                {

                    LoadData();

                    LoadGrid();

                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Delete Successfully');", true);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
                }

            }
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }

    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid();

    }
    private void GetInvoice(long sId)
    {
        try
        {
            DocumentPrintController DPrint = new DocumentPrintController();
            DataTable dt = DPrint.SelectReportTitle(Convert.ToInt32(Session["Distributor_id"].ToString()));

            DataSet ds = null;

            ds = oeCtrl.SelectOrderforView(sId, 2);
            //if (ds.Rows.Count <= 0) return;

            SAMSBusinessLayer.Reports.CrpSaleReturnPrinting crpOrder = new SAMSBusinessLayer.Reports.CrpSaleReturnPrinting();
            crpOrder.SetDataSource(ds);
            crpOrder.Refresh();

            crpOrder.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString().ToUpper());
            crpOrder.SetParameterValue("CompanyAddress", dt.Rows[0]["ADDRESS1"].ToString());
            crpOrder.SetParameterValue("ContactNo", dt.Rows[0]["CONTACT_NUMBER"].ToString());

            Session.Add("CrpReport", crpOrder);
            Session.Add("ReportType", 0);
            Session.Add("CrpReport", crpOrder);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);


        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
}