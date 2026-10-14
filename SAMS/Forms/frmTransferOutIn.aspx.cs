using System;
using System.Data;
using System.Web.UI;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;

/// <summary>
/// From To Transfer In Stock
/// </summary>
public partial class Forms_frmTransferOutIn : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function Populates All Combos And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           
            LoadToDistributor();
            LoadDistributor();
            GetDocumentNo();
            LoadDocumentDetail();
        }
    }

    //Load All Locations
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }

    
    private void LoadToDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpTransferFor, dt, 0, 2, true);
    }

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        PurchaseController mPurchase = new PurchaseController();

        // DataTable dt = mPurchase.SelectTransferDocumentNo(Constants.Document_Transfer_Out, Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, int.Parse(DrpTransferFor.SelectedValue), 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
         DataTable dt = mPurchase.SelectTransferDocumentNo(Constants.Document_Transfer_Out, Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, int.Parse(DrpTransferFor.SelectedValue), 0, Constants.DateNullValue);

        clsWebFormUtil.FillDropDownList(drpDocumentNo, dt, 0, 0);
    }

  
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocumentDetail();
    }

    private void LoadDocumentDetail()
    {
        try
        {
            if (drpDocumentNo.Items.Count > 0)
            {
                PurchaseController mPurchase = new PurchaseController();
                DataTable dt = mPurchase.SelectTransferDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue, int.Parse(DrpTransferFor.SelectedValue), 0, Constants.DateNullValue);
                if (dt.Rows.Count > 0)
                {
                    txtDocumentNo.Text = dt.Rows[0]["ORDER_NUMBER"].ToString();
                    txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
                    drpDistributor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
                    DataTable TransferSKU = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
                    GrdPurchase.DataSource = TransferSKU;
                    GrdPurchase.DataBind();
                    Session.Add("TransferSKU", TransferSKU);
                }
                else
                {
                    txtDocumentNo.Text = "";
                    txtBuiltyNo.Text = "";

                    drpDistributor.SelectedIndex = 0;
                    Session.Remove("TransferSKU");
                    GrdPurchase.DataSource = null;
                    GrdPurchase.DataBind();
                }
            }
            else
            {
                txtDocumentNo.Text = "";
                txtBuiltyNo.Text = "";


                drpDistributor.SelectedIndex = 0;
                Session.Remove("TransferSKU");
                GrdPurchase.DataSource = null;
                GrdPurchase.DataBind();
            }
        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
        }
    }

    
    protected void btnTransferIn_Click(object sender, EventArgs e)
    {
        PurchaseController mController = new PurchaseController();
        DataTable dtPurchaseDetail = (DataTable)this.Session["TransferSKU"];
        decimal mTotalAmount = 0;

        foreach (DataRow dr in dtPurchaseDetail.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

        }

        bool mResult = mController.InsertTransferIn(long.Parse(drpDocumentNo.SelectedValue), int.Parse(DrpTransferFor.SelectedValue.ToString()), txtDocumentNo.Text, Constants.Document_Transfer_In,
               DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(DrpTransferFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
              , mTotalAmount, false, dtPurchaseDetail, 1, txtBuiltyNo.Text, int.Parse(this.Session["UserId"].ToString()), 0, false);

        if (mResult == true)
        {
            GrdPurchase.DataSource = null;
            GrdPurchase.DataBind();

            Session.Remove("TransferSKU");

            LoadDistributor();
            GetDocumentNo();
            LoadDocumentDetail();
            txtBuiltyNo.Text = "";
            txtDocumentNo.Text = "";
        }
    }
}
