using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmPurchaseReceivedEntry: System.Web.UI.Page
{
    SKUPriceDetailController PController = new SKUPriceDetailController();
    DataControl dc = new DataControl();    
    DataTable OrderedSKU;
    static long mPURCHASE_MASTER_ID = Constants.LongNullValue;
    static int mPURCHASE_PRINCIPAL_ID = Constants.IntNullValue;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
          
            GetDocumentNo();
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtDCDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtDCDate.Attributes.Add("readonly", "readonly");
            txtTotalQuantity.Attributes.Add("readonly", "readonly");
            LoadDocumentDetail();
           
        }
    }

    #region Load

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseOrderDocumentNo2(Constants.LongNullValue, Constants.IntNullValue, 2, Constants.DateNullValue);

        drpDocumentNo.Items.Add(new ListItem("--------Select--------", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDocumentNo, dt, "PURCHASE_ORDER_ID", "VENDOR_NAME", false);
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2,true);
    }
    private void LoadDocumentDetail()
    {        
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        if (drpDocumentNo.Items.Count > 0)
        {
            if (drpDocumentNo.SelectedValue != Constants.LongNullValue.ToString())
            {
                DataTable dt = mPurchase.SelectPurchaseOrderDocumentNo2(Convert.ToInt64(drpDocumentNo.SelectedValue), Constants.IntNullValue, 2, Constants.DateNullValue);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Status"].ToString() == "PM")
                    {
                        drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();

                        if (Convert.ToInt64(dc.chkNull_0(dt.Rows[0]["PURCHASE_ORDER_ID"].ToString())) > 0)
                        {
                            mPURCHASE_MASTER_ID = Convert.ToInt64(dc.chkNull_0(dt.Rows[0]["PURCHASE_ORDER_ID"].ToString()));
                            mPURCHASE_PRINCIPAL_ID = Convert.ToInt32(dc.chkNull_0(dt.Rows[0]["SOLD_FROM"].ToString()));
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

                            OrderedSKU = mPurchase.SelectPurchaseOrderDetail2(long.Parse(dt.Rows[0]["PURCHASE_ORDER_ID"].ToString()), Constants.IntNullValue);
                        }
                    }
                    else
                    {
                        mPURCHASE_MASTER_ID = Constants.LongNullValue;
                        mPURCHASE_PRINCIPAL_ID = Convert.ToInt32(dc.chkNull_0(dt.Rows[0]["SOLD_FROM"].ToString()));
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

                        OrderedSKU = mPurchase.SelectPurchaseOrderDetail(long.Parse(dt.Rows[0][0].ToString()), Constants.IntNullValue);

                    }

                    Session.Add("OrderedSKU", OrderedSKU);
                    LoadGird();
                }
            }
        }
    }

    private void LoadGird()
    {
        int TotalValue = 0;
        OrderedSKU = (DataTable)Session["OrderedSKU"];
        GrdPurchase.DataSource = null;
        GrdPurchase.DataBind();
        GrdPurchase.DataSource = OrderedSKU;
        GrdPurchase.DataBind();
        foreach (DataRow dr in OrderedSKU.Rows)
        {
            TotalValue += int.Parse(dr["Quantity"].ToString());
        }
        txtTotalQuantity.Text = TotalValue.ToString(); 
    }

    #endregion

    #region Click OPerations

    /// <summary>
    /// Saves All Document Detail Grid Data
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        DataTable PurchasedPSKU = new DataTable();
        PurchasedPSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchasedPSKU.Columns.Add("SKU_ID", typeof(int));
        PurchasedPSKU.Columns.Add("BATCH_NO", typeof(string));
        PurchasedPSKU.Columns.Add("PRICE", typeof(decimal));
        PurchasedPSKU.Columns.Add("Quantity", typeof(int));
        PurchasedPSKU.Columns.Add("FREE_SKU", typeof(int));
        PurchasedPSKU.Columns.Add("AMOUNT", typeof(decimal));
        PurchasedPSKU.Columns.Add("TIME_STAMP",typeof(DateTime));

        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            TextBox txtRecQty = gvr.FindControl("txtRecQty") as TextBox;
            TextBox txtBatchNo = gvr.FindControl("txtBatchNo") as TextBox;
            TextBox txtExpDate = gvr.FindControl("txtExpDate") as TextBox;

            DateTime dtExpDate = Constants.DateNullValue;

            if (txtExpDate.Text.Length > 0)
            {
                try
                {
                    dtExpDate = Convert.ToDateTime(txtExpDate.Text);
                }
                catch (Exception ex)
                {                    
                }
            }

            DataRow dr = PurchasedPSKU.NewRow();
            dr["SKU_ID"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[0].Text));
            dr["BATCH_NO"] = txtBatchNo.Text;
            dr["FREE_SKU"] = Convert.ToInt32(dc.chkNull_0(txtRecQty.Text));
            dr["PRICE"] = Convert.ToDecimal(dc.chkNull_0(gvr.Cells[7].Text));
            dr["Quantity"] = Convert.ToInt32(dc.chkNull_0(gvr.Cells[4].Text));
            dr["AMOUNT"] = Convert.ToDecimal(dc.chkNull_0(gvr.Cells[7].Text)) * Convert.ToInt32(dc.chkNull_0(gvr.Cells[4].Text));
            dr["TIME_STAMP"] = dtExpDate;
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
                    LoadDistributor();
                    GetDocumentNo();
                    //drpDocumentNo_SelectedIndexChanged(null, null);
                    //LoadDocumentDetail();
                    GrdPurchase.DataSource = null;
                    GrdPurchase.DataBind();
                    ClearAll();
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('GRN saved successfully.');", true);
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

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LoadGird();
    }

    #endregion

 
    private bool CalculatePurchase(DateTime MWorkDate, DataTable PurchasedPSKU)
    {
        decimal mTotalAmount = 0;
        byte Status = 1;
        if (chkSend.Checked)
        {
            Status = 2;
        }
        PurchaseController mController = new PurchaseController();

        foreach (DataRow dr in PurchasedPSKU.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());
        }

        if (mPURCHASE_MASTER_ID > 0)
        {
            bool mResult = mController.UpdateGRN(mPURCHASE_MASTER_ID, int.Parse(drpDistributor.SelectedValue.ToString()), "", 2, MWorkDate
                , int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, PurchasedPSKU, 0, ""
                , int.Parse(Session["UserId"].ToString()),mPURCHASE_PRINCIPAL_ID, drpDocumentNo.SelectedItem.Text.ToString()
                , txtDCNo.Text.Trim(), DateTime.Parse(txtDCDate.Text), txtDriverName.Text.Trim(), txtVehicleNo.Text.Trim(), Status);
            return mResult;
        }
        else
        {
            bool mResult = mController.InsertGRN(int.Parse(drpDistributor.SelectedValue.ToString()), "", 2
            , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, PurchasedPSKU, 0, ""
            , int.Parse(Session["UserId"].ToString()),mPURCHASE_PRINCIPAL_ID, Convert.ToInt64(drpDocumentNo.SelectedValue), drpDocumentNo.SelectedItem.Text.ToString()
            , txtDCNo.Text.Trim(), DateTime.Parse(txtDCDate.Text), txtDriverName.Text.Trim(), txtVehicleNo.Text.Trim(), Status);
            return mResult;
        }
    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            OrderedSKU = new DataTable();
            Session.Add("OrderedSKU", OrderedSKU);
            LoadGird();
        }
        else
        {
            LoadDocumentDetail();
          
        }
    }


    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowIndex == 0)
        {
            e.Row.FindControl("txtRecQty").Focus();
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtRecQty = (TextBox)e.Row.FindControl("txtRecQty");

        }

    }

    protected void txtRecQty_TextChanged(object sender, EventArgs e)
    {
        DataControl _dc = new DataControl();
        TextBox txtRecQty = sender as TextBox;


        if (txtRecQty == null) return;
        GridViewRow item = (GridViewRow)txtRecQty.NamingContainer;

        int recieveQty = 0;
        if (!string.IsNullOrEmpty(txtRecQty.Text))
        {                                   //prev rec qty
            recieveQty = int.Parse(txtRecQty.Text);

        }


        DataTable order = (DataTable)Session["OrderedSKU"];
        if (recieveQty > 0)
        {
            int orderQty = int.Parse(order.Rows[item.RowIndex]["Quantity"].ToString());
            

                if (recieveQty > orderQty)
                {
                    txtRecQty.Focus();
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Received Qty should not be greater than Order Qty!');", true);

                }
                else
                {
                    //order.Rows[item.RowIndex]["RcdQty"] = Convert.ToString(int.Parse(_dc.chkNull_0(txtRecQty.Text)));
                   
                    //int TotalQty = 0;
                    //foreach (DataRow dr in order.Rows)
                    //{
                      
                    //    TotalQty += int.Parse(dr["RcdQty"].ToString());
                    //}

                    //txtTotalQuantity.Text = TotalQty.ToString();

                    if (item.RowIndex + 1 < GrdPurchase.Rows.Count)
                    {
                        GrdPurchase.Rows[item.RowIndex + 1].Cells[5].Focus();
                    }
                    else
                    {
                        txtDCNo.Focus();
                    }
                }
            }
            else
            {
                txtRecQty.Focus();
            }
        }

    private void ClearAll()
    {
        txtDCNo.Text = "";
        txtDriverName.Text = "";
        txtVehicleNo.Text = "";
        txtTotalQuantity.Text = "";
        txtDCDate.Text = SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
    }

  
}