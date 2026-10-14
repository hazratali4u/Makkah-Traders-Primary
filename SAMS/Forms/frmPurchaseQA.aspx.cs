using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;

public partial class Forms_frmPurchaseQA : System.Web.UI.Page
{
    SKUPriceDetailController PController = new SKUPriceDetailController();
    DataControl dc = new DataControl();
    DataTable OrderedSKU;
    static long mPURCHASE_MASTER_ID = Constants.LongNullValue;
    static int mPURCHASE_PRINCIPAL_ID = Constants.IntNullValue;
    static bool mPrincipalType = false;
    static decimal mGst = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();

            if (Session["PurchaseQAId"].ToString() != "")
            {
               lblVendorName.Text= Session["VendId"].ToString();
               LoadDocumentDetail();
            }
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtDCDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtDCDate.Attributes.Add("readonly", "readonly");
            txtTotalQuantity.Attributes.Add("readonly", "readonly");
           

        }
    }

    #region Load

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();

        DataTable dt = mPurchase.SelectPurchaseDocumentNo(2,Constants.IntNullValue,Convert.ToInt64(Session["PurchaseQAId"].ToString()), Constants.IntNullValue,Convert.ToDateTime(Session["CURRENTWORKDATE"]),2);
            if (dt.Rows.Count > 0)
            {
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();

                if (Convert.ToInt64(dc.chkNull_0(dt.Rows[0]["PURCHASE_MASTER_ID"].ToString())) > 0)
                {
                    lblNo.Text = Session["PurchaseQAId"].ToString();
                    mPURCHASE_MASTER_ID = Convert.ToInt64(dc.chkNull_0(dt.Rows[0]["PURCHASE_MASTER_ID"].ToString()));
                    mPURCHASE_PRINCIPAL_ID = Convert.ToInt32(dc.chkNull_0(dt.Rows[0]["PRINCIPAL_ID"].ToString()));
                    mGst = Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["gst"].ToString()));
                    mPrincipalType = Convert.ToBoolean(dt.Rows[0]["vendorType"].ToString());

                    txtDCNo.Text = dt.Rows[0]["dcNo"].ToString().Replace("&nbsp;", "");
                    if (dt.Rows[0]["dcDate"].ToString() != "")
                    {
                        txtDCDate.Text = DateTime.Parse(dt.Rows[0]["dcDate"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        txtDCDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
                    }
                    txtDriverName.Text = dt.Rows[0]["driverName"].ToString().Replace("&nbsp;", "");
                    txtVehicleNo.Text = dt.Rows[0]["vehicleNo"].ToString().Replace("&nbsp;", "");
                    txtRemarks.Text = dt.Rows[0]["Builty_no"].ToString().Replace("&nbsp;", "");
                    OrderedSKU = mPurchase.SelectPurchaseOrderDetail2(long.Parse(dt.Rows[0]["PURCHASE_MASTER_ID"].ToString()), Constants.IntNullValue);
                }
               

                Session.Add("OrderedSKU", OrderedSKU);
                LoadGird();
            }
        
    }

    private void LoadGird()
    {
        int TotalValue = 0;
        OrderedSKU = (DataTable)Session["OrderedSKU"];
        
        GrdPurchase.DataSource = OrderedSKU;
        GrdPurchase.DataBind();
        foreach (DataRow dr in OrderedSKU.Rows)
        {
            
                    TotalValue += int.Parse(dr["InvQty"].ToString());
               
        }
        txtTotalQuantity.Text = TotalValue.ToString();
    }

    #endregion

    #region Click OPerations

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        DataTable PurchasedPSKU = new DataTable();
        PurchasedPSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchasedPSKU.Columns.Add("SKU_ID", typeof(int));
        PurchasedPSKU.Columns.Add("BATCH_NO", typeof(string));
        PurchasedPSKU.Columns.Add("PRICE", typeof(decimal));
        PurchasedPSKU.Columns.Add("Quantity", typeof(int));
        PurchasedPSKU.Columns.Add("FREE_SKU", typeof(int));
        PurchasedPSKU.Columns.Add("ngQty", typeof(int));
        PurchasedPSKU.Columns.Add("AMOUNT", typeof(decimal));
       
        PurchasedPSKU.Columns.Add("remarks", typeof(string));
        PurchasedPSKU.Columns.Add("techId",typeof(int));
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            TextBox txtngQty = gvr.FindControl("txtngQty") as TextBox;
           
            TextBox txtremarks = gvr.FindControl("txtremarks") as TextBox;

          //  DateTime dtExpDate = Constants.DateNullValue;

          

            DataRow dr = PurchasedPSKU.NewRow();
            dr["SKU_ID"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[0].Text));
            dr["BATCH_NO"] = "N/A";
            dr["FREE_SKU"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[4].Text));
            dr["PRICE"] = Convert.ToDecimal(dc.chkNull_0(gvr.Cells[9].Text));
            dr["techId"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[10].Text));
            dr["Quantity"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[3].Text));
            dr["ngQty"] = Convert.ToInt32(dc.chkNull_0(txtngQty.Text));
            dr["AMOUNT"] = (Convert.ToInt32(dc.chkNull_0(gvr.Cells[4].Text)) - Convert.ToInt32(dc.chkNull_0(txtngQty.Text))) * Convert.ToDecimal(dc.chkNull_0(gvr.Cells[9].Text));
            dr["remarks"]=Convert.ToString(dc.chkNull_0(txtremarks.Text));
            
            PurchasedPSKU.Rows.Add(dr);
        }


        if (PurchasedPSKU.Rows.Count > 0)
        {
            DistributorController mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()));
            if (dt.Rows.Count > 0)
            {
                if (CalculatePurchase(DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()), PurchasedPSKU))
                {
                    Session.Remove("PurchaseQAId");
                    Session.Remove("VendId");
                    Response.Redirect("frmPurchaseViewQA.aspx?LevelID=39&LevelType=3&TopID=27");
                
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Location');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' At least one SKU must enter');", true);

        }
    }

  
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmPurchaseViewQA.aspx?LevelID=39&LevelType=3&TopID=27");
    }

    #endregion


    private bool CalculatePurchase(DateTime MWorkDate, DataTable PurchasedPSKU)
    {
        bool flag = false;
        decimal mTotalAmount = 0;
        decimal mTotalNetAmount = 0;
        decimal mGstAmount = 0;

        PurchaseController mController = new PurchaseController();

        foreach (DataRow dr in PurchasedPSKU.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());
        }

        mGstAmount = (mGst / 100) * mTotalAmount;
        mTotalNetAmount = mGstAmount + mTotalAmount;

        if (mPURCHASE_MASTER_ID > 0)
        {
            
             flag = mController.UpdateGRN_QA(mPURCHASE_MASTER_ID, int.Parse(drpDistributor.SelectedValue.ToString()), "", Constants.Document_Purchase, MWorkDate
                , int.Parse(drpDistributor.SelectedValue.ToString()), mPrincipalType, mTotalAmount, mTotalNetAmount,mGstAmount, false, PurchasedPSKU, 0, txtRemarks.Text
                , int.Parse(Session["UserId"].ToString()), mPURCHASE_PRINCIPAL_ID, Session["PurchaseQAId"].ToString()
                , txtDCNo.Text.Trim(), DateTime.Parse(txtDCDate.Text), txtDriverName.Text.Trim(), txtVehicleNo.Text.Trim());
           
        }
        return flag;
    }

    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowIndex == 0)
        {
            e.Row.FindControl("txtngQty").Focus();
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtngQty = (TextBox)e.Row.FindControl("txtngQty");
            TextBox txtremarks = (TextBox)e.Row.FindControl("txtremarks");
        }

    }

    protected void txtngQty_TextChanged(object sender, EventArgs e)
    {
        DataControl _dc = new DataControl();
        TextBox txtngQty = sender as TextBox;


        if (txtngQty == null) return;
        GridViewRow item = (GridViewRow)txtngQty.NamingContainer;

        int recieveQty = 0;
        if (!string.IsNullOrEmpty(txtngQty.Text))
        {                                 
            recieveQty = int.Parse(txtngQty.Text);
        }
        DataTable order = (DataTable)Session["OrderedSKU"];
        
            int orderQty = int.Parse(order.Rows[item.RowIndex]["RcdQty"].ToString());


            if (recieveQty > orderQty)
            {
                txtngQty.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('NG Qty should not be greater than Received Qty!');", true);

            }
            else
            {
                GrdPurchase.Rows[item.RowIndex].Cells[6].Text = Convert.ToString(orderQty - int.Parse(_dc.chkNull_0(txtngQty.Text)));

                int TotalQty = 0;

                foreach (DataRow dr in order.Rows)
                {

                    TotalQty += int.Parse(dr["InvQty"].ToString());
                }

                txtTotalQuantity.Text = TotalQty.ToString();

                if (item.RowIndex + 1 < GrdPurchase.Rows.Count)
                {
                    GrdPurchase.Rows[item.RowIndex].Cells[7].Focus();
                }
                else if (item.RowIndex == GrdPurchase.Rows.Count)
                {
                    GrdPurchase.Rows[item.RowIndex].Cells[7].Focus();
                }
                else
                {
                    txtRemarks.Focus();
                }
            }
        
    }

    private void ClearAll()
    {
        txtDCNo.Text = "";
        txtDriverName.Text = "";
        txtVehicleNo.Text = "";
        txtDCDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
    }
}