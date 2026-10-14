using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using System.Data;
using SAMSCommonLayer.Classes;
using System.Text;
using System.Collections;

public partial class Forms_frmDelieveryOrderAdd : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly CustomerDataController _customerCtrl = new CustomerDataController();
    readonly OrderEntryController oeCtrl = new OrderEntryController();
    
    private static int _mCustomerTypeId;
    private static int _mCustomerVolClassId;

    DataTable _dtFreeSku;

    private void loadInvoice()
    {
        DataTable dt = oeCtrl.SelectPendingOrder2(Convert.ToInt64(Session["invoiceId"].ToString()), 2);
        if (dt.Rows.Count > 0)
        {
            ddlDistributor.SelectedValue = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
            txtTotalAmount.Text = string.Format("{0:0.00}", dt.Rows[0]["TOTAL_NET_AMOUNT"]);

            DrpBusinessType.SelectedValue = dt.Rows[0]["BUSINESS_TYPE_ID"].ToString();

            LoadCustomers();
            ddlCustomer.SelectedValue = dt.Rows[0]["CUSTOMER_ID"].ToString();

            txtDeliveredTo.Text = dt.Rows[0]["DELIVERED_TO"].ToString();
            txtDeliveredThrough.Text = dt.Rows[0]["DELIVERED_THROUGH"].ToString();
            _mCustomerTypeId = int.Parse(dt.Rows[0]["CHANNEL_TYPE_ID"].ToString());
            _mCustomerVolClassId = int.Parse(dt.Rows[0]["VOLUME_CLASS_ID"].ToString());

            hfIsGstReg.Value = dt.Rows[0]["isGstRegister"].ToString();
            rdbInvoiceType.SelectedValue = dt.Rows[0]["INVOICE_TYPE"].ToString();
            txtInvoiceDiscount.Text = string.Format("{0:0.00}", dt.Rows[0]["discount"]);
            

             DataTable dtCustomer = _customerCtrl.GetCustomerByID(Convert.ToInt32(ddlDistributor.SelectedValue), Convert.ToInt64(ddlCustomer.SelectedValue),1);
            if (dtCustomer == null)
            {
              
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Activate Customer first!');", true);
                return;
            }


            hfGst.Value = dt.Rows[0]["GstPage"].ToString();

            //------------------------------------------------------------------------\\

            rdbDiscountType.SelectedValue = Convert.ToString((dt.Rows[0]["DISCOUNT_TYPE"].ToString()));
         
            txtRemarks.Text = (dt.Rows[0]["remarks"].ToString());

            orderSku = oeCtrl.SelectOrderDetail(Convert.ToInt32(ddlDistributor.SelectedValue), long.Parse(Session["invoiceId"].ToString()), 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()), 2);
        }   
    }
  
    protected void Calculate()
    {
        try
        {

            decimal ObjGrossSales = 0;
            decimal ObjStandardDiscount = 0;
            decimal ObjDiscountPrice = 0;

            decimal ObjExtraDiscount = 0;
            decimal ProductStdDist = 0;
            decimal TotalAmount = 0;
            decimal TotalAmount2 = 0;

           

            DataTable dt = new DataTable();

            dt = (DataTable)Session["orderSku"];

            foreach (DataRow dr in dt.Rows)
            {
                dr["standardDiscount"] = 0;
                dr["standardDiscountPer"] = 0;
                dr["extraDiscount"] = 0;
                
                dr["amount"] = decimal.Parse(_dc.chkNull_0(dr["price"].ToString())) * int.Parse(dr["CurrentRcdQty"].ToString());

                dr["discountedPrice"] = 0;

                TotalAmount2 += decimal.Parse(dr["amount"].ToString());
            }
            decimal gstAmount = CalculateGst(TotalAmount2, Convert.ToDecimal(_dc.chkNull_0(hfGst.Value)));
               txtTotalAmount.Text = string.Format("{0:0.00}", TotalAmount2 + gstAmount);
           

            PromotionCollections_Controller Arpc = (PromotionCollections_Controller)Session["Arpc"];
           

            if (dt.Rows.Count > 0)
            {
                if (Arpc != null)
                {
                    SKUGroupController GroupCtl = new SKUGroupController();

                    ArrayList ARlistOffer = oeCtrl.GetPromotionOffers(Arpc, _mCustomerVolClassId, _mCustomerTypeId, dt, false);

                    for (int i = 0; i < ARlistOffer.Count; i++)
                    {
                        PromoOffers_Collection pofferCol = (PromoOffers_Collection)ARlistOffer[i];

                        if (pofferCol.SKU_ID > 0)
                        {
                            #region slab on SKU
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (int.Parse(dr["skuId"].ToString()) == pofferCol.SKU_ID)
                                {

                                    if (pofferCol.Is_And == true)
                                    {
                                        if (pofferCol.Offer_Value > 0)
                                        {
                                            dr["extraDiscount"] = decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())) + pofferCol.Offer_Value;

                                        }
                                        if (pofferCol.Discount > 0)
                                        {
                                            dr["extraDiscount"] = decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100) * (decimal.Parse(dr["amount"].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        if (pofferCol.Offer_Value > 0)
                                        {
                                            dr["standardDiscount"] = decimal.Parse(_dc.chkNull_0(dr["standardDiscount"].ToString())) + pofferCol.Offer_Value;
                                        }
                                        if (pofferCol.Discount > 0)
                                        {
                                            dr["standardDiscountPer"] = decimal.Parse(_dc.chkNull_0(dr["standardDiscountPer"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
                                        }
                                    }
                                   
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region slab on SKU Group

                            foreach (DataRow dr in dt.Rows)
                            {
                                if (GroupCtl.ExistsInGroup(Constants.IntNullValue, pofferCol.Group_ID, int.Parse(dr["skuId"].ToString())))
                                {


                                    if (pofferCol.Is_And == true)
                                    {
                                        if (pofferCol.Offer_Value > 0)
                                        {
                                            dr["extraDiscount"] = decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())) + pofferCol.Offer_Value;
                                        }
                                        if (pofferCol.Discount > 0)
                                        {
                                            dr["extraDiscount"] = decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())) + ((decimal.Parse(pofferCol.Discount.ToString())) / 100) * (decimal.Parse(dr["amount"].ToString()) - decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())));
                                        }
                                    }
                                    else
                                    {
                                        if (pofferCol.Offer_Value > 0)
                                        {
                                            dr["standardDiscount"] = decimal.Parse(_dc.chkNull_0(dr["standardDiscount"].ToString())) + pofferCol.Offer_Value;
                                        }
                                        if (pofferCol.Discount > 0)
                                        {
                                            dr["standardDiscountPer"] = decimal.Parse(_dc.chkNull_0(dr["standardDiscountPer"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
                                        }
                                    }
                                }
                               


                            }
                            #endregion
                        }

                    }
                }
                #region Recalculate Products

                foreach (DataRow dr in dt.Rows)
                {
                    ProductStdDist = decimal.Parse(_dc.chkNull_0(dr["standardDiscountPer"].ToString())) * (decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) - decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString())));
                    if (ProductStdDist > 0)
                    {
                        dr["standardDiscount"] = decimal.Parse(_dc.chkNull_0(dr["standardDiscountPer"].ToString())) * (decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())));
                        dr["discountedPrice"] = (decimal.Parse(_dc.chkNull_0(dr["standardDiscount"].ToString())) + decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString()))) / int.Parse(_dc.chkNull_0(dr["CurrentRcdQty"].ToString()));
                        dr["discountedPrice"] = (decimal.Parse(_dc.chkNull_0(dr["price"].ToString())) - decimal.Parse(dr["discountedPrice"].ToString()));
                    }
                    ObjGrossSales += decimal.Parse(_dc.chkNull_0(dr["amount"].ToString()));
                    ObjStandardDiscount += decimal.Parse(_dc.chkNull_0(dr["standardDiscount"].ToString()));
                    ObjExtraDiscount += decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString()));
                  

                    #region GST Calculation
                    
                    decimal TempAmount = decimal.Parse(_dc.chkNull_0(dr["amount"].ToString()));
                    decimal TempDiscount = decimal.Parse(_dc.chkNull_0(dr["standardDiscount"].ToString())) + decimal.Parse(_dc.chkNull_0(dr["extraDiscount"].ToString()));
                    
                    dr["netAmount"] = (TempAmount  - TempDiscount);
                   
                    #endregion
                    TotalAmount += (TempAmount - TempDiscount);
                }
              
                //dt = (DataTable)Session["orderSku"];
                 gstAmount = CalculateGst(TotalAmount, Convert.ToDecimal(_dc.chkNull_0(hfGst.Value)));
                 hfGstAmount.Value = Convert.ToString(gstAmount);
                 hfStandardDiscount.Value = Convert.ToString(ObjStandardDiscount);
                 hfExtraDiscount.Value = Convert.ToString(ObjExtraDiscount);

                 txtTotalAmount.Text = string.Format("{0:0.00}",  TotalAmount + gstAmount);
               
                Session.Add("orderSku",dt);
             
                #endregion
            }
          
        }

        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtGrossAmount.Attributes.Add("readonly", "readonly");
            txtProductDiscount.Attributes.Add("readonly", "readonly");
            txtTotalAmount.Attributes.Add("readonly", "readonly");

            CreateTable();
            
            LoadDistributors();
            LoadBusinessType();

            if (Session["invoiceId"] != null)
            {
                loadInvoice();

                Session.Add("orderSku", orderSku);
                EnableDisableController(false);
            }
            else
            {
                EnableDisableController(true);
                LoadCustomers();
            }
            LoadSKUDetail();
           // LoadPromotion();

            
            
            LoadGird();
        }
    }

    #region Load

    private void LoadDistributors()
    {
        DistributorController distCtrl = new DistributorController();
        DataTable dt = distCtrl.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                clsWebFormUtil.FillDropDownList(ddlDistributor, dt, 0, 2);
            }
        }
    }
    private void LoadBusinessType()
    {
        SLASHCodesController _customerCtrl = new SLASHCodesController();
        DataTable dt = _customerCtrl.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerTypeBusiness, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillDropDownList(DrpBusinessType, dt, 0, 2, true);
    }

    private void LoadCustomers()
    {
        ddlCustomer.Items.Clear();

        if (ddlDistributor.Items.Count > 0)
        {
            DataTable dtCustomer = _customerCtrl.SelectPrincipalCustomer(int.Parse(ddlDistributor.SelectedValue), int.Parse(DrpBusinessType.SelectedValue), Constants.IntNullValue, Constants.IntNullValue);
            
            clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");

            Session.Add("dtCustomer", dtCustomer);

            ddlCustomer_SelectedIndexChanged(null,null);
        }
    }
    private void CustomerLedger()
    {
        lblLedgerBalance.Text = "0.00";
        lblOpeningType.Text = "DR";
        if (ddlCustomer.Items.Count > 0)
        {
            decimal Opening = 0;
            DataTable dt = _customerCtrl.SelectCustomerCreditBalance(long.Parse(ddlCustomer.SelectedValue), Constants.IntNullValue, Convert.ToInt32(ddlDistributor.SelectedValue), Constants.Advance_PaymentOrder_id);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Opening = Opening + decimal.Parse(_dc.chkNull_0(dt.Rows[0][0].ToString()));

                    if (Session["invoiceId"] != null)
                    {
                        lblLedgerBalance.Text = string.Format("{0:0,0.00}", Opening + Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)));
                    }
                    else
                    {
                        lblLedgerBalance.Text = string.Format("{0:0,0.00}", Opening);
                    }
                    lblOpeningType.Text = Convert.ToDecimal(lblLedgerBalance.Text) < 0 ? " DR" : " CR";
                    if (Convert.ToDecimal(lblLedgerBalance.Text) < 0)
                    {
                        lblLedgerBalance.Text = Convert.ToString(Convert.ToDecimal(lblLedgerBalance.Text) * -1);
                    }
                }
            }
        }
    }
   
    private bool CreditLimit()
    {
        if (!chkLimit.Checked)
        {
            decimal ledgerBal = decimal.Parse(_dc.chkNull_0(lblLedgerBalance.Text));
            //decimal SaleLimit = decimal.Parse(_dc.chkNull_0(lblSaleLimit.Text));
            if (decimal.Parse(_dc.chkNull_0(txtTotalAmount.Text)) > ledgerBal)//+ SaleLimit)
            {
                return false;
            }
            return true;
        }
        return true;
    }
 
    private void LoadPromotion()
    {

        OrderEntryController orderController = new OrderEntryController();
        PromotionCollections_Controller Arpc = orderController.LoadSchemes(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), Constants.IntNullValue, Convert.ToDateTime(this.Session["CurrentWorkDate"]));
        Session.Add("Arpc", Arpc);

    }

    #endregion

    private void EnableDisableController(bool CValue)
    {
        if (CValue == true)
        {
            ddlCustomer.Enabled = true;
            ddlDistributor.Enabled = true;
            DrpBusinessType.Enabled = true;
        }
        else
        {
            ddlCustomer.Enabled = false;
            ddlDistributor.Enabled = false;
            DrpBusinessType.Enabled = false;
        }
    }
   

    #region Detail Panel

    DataTable orderSku = new DataTable();
    private static int RowNo;
    //private void LoadGird()
    //{
    //    int TotalValue = 0;
    //    //decimal TotalAmount = 0;
    //    int TotalSale = 0;
    //    orderSku = (DataTable)Session["orderSku"];
    //    grdOrder.DataSource = orderSku;
    //    grdOrder.DataBind();

    //    if (Session["invoiceId"] == null)
    //    {
    //        foreach (DataRow dr in orderSku.Rows)
    //        {
    //            TotalValue += int.Parse(dr["qty"].ToString());
    //            TotalSale += int.Parse(dr["RcdQty"].ToString());
    //        }

    //        if (TotalSale == TotalValue)
    //        {

    //            btnSaveOrder.Enabled = false;
    //            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Could not Dispatch against that Order.');", true);
    //            // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
    //            //  "alert('Could not Dispatch against that Order.'); window.location='" +
    //            //    Request.ApplicationPath + "/Forms/frmSaleOrderView.aspx?LevelType=3&TopID=26&LevelID=97';", true);
    //            return;
    //        }
    //    }
    //    else
    //    {
    //    int i = 0;
    //    foreach (GridViewRow gvr in grdOrder.Rows)
    //    {

    //        TextBox txtCurrentRecQty = gvr.FindControl("txtCurrentRecQty") as TextBox;
    //        if (txtCurrentRecQty.Text.Length > 0)
    //        {
    //            TotalSale += int.Parse(txtCurrentRecQty.Text);

    //        }
    //        i += 1;
    //        }
    //    txtTotalQty.Text = TotalSale.ToString();
    //     //   txtTotalAmount.Text = string.Format("{0:0.00}", TotalAmount);
    //    }
    //}

    private void CreateTable()
    {
        orderSku = new DataTable();
        orderSku.Columns.Add("orderDetailId", typeof(long));
        orderSku.Columns.Add("skuId", typeof(int));
        orderSku.Columns.Add("skuName", typeof(string));
        orderSku.Columns.Add("price", typeof(decimal));
        orderSku.Columns.Add("qty", typeof(int));
        orderSku.Columns.Add("prevQty", typeof(int));
        orderSku.Columns.Add("discount", typeof(decimal));
        orderSku.Columns.Add("discountType", typeof(int));
        orderSku.Columns.Add("amount", typeof(decimal));
        orderSku.Columns.Add("netAmount", typeof(decimal));
        Session.Add("orderSku", orderSku);

    }
    private bool CheckDublicateSKU()
    {
        orderSku = (DataTable)Session["orderSku"];
        DataRow[] foundRows = orderSku.Select("skuId  = '" + ddlSKus.SelectedValue + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }

        return false;
    }
    private void LoadSKUDetail()
    {
        if (ddlDistributor.Items.Count > 0)
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable Dtsku_Price = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddlDistributor.SelectedValue), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillDropDownList(ddlSKus, Dtsku_Price, "SKU_ID", "SkuPriceDetail", true);
            Session.Add("Dtsku_Price", Dtsku_Price);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

            DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKus.SelectedValue + "'");

            if (foundRows.Length > 0)
            {
                orderSku = (DataTable)Session["orderSku"];

                decimal price = Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["TRADE_PRICE"].ToString()));

                if (Convert.ToDecimal(_dc.chkNull_0(txtPrice.Text)) >= price)
                {
                    price = Convert.ToDecimal(_dc.chkNull_0(txtPrice.Text));

                    if (btnSave.Text == "Add")
                    {
                        if (CheckDublicateSKU())
                        {

                            DataRow dr = orderSku.NewRow();
                            dr["skuId"] = foundRows[0]["SKU_ID"];
                            dr["skuName"] = foundRows[0]["SKU_NAME"]; //AS SKUnMAE
                            dr["price"] = price;
                            dr["qty"] = int.Parse(_dc.chkNull_0(txtQuantity.Text));


                            dr["discountType"] = int.Parse(rbDiscountType.SelectedValue);
                            dr["amount"] = price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));

                            if (rbDiscountType.SelectedValue == "1")
                            {
                                dr["discount"] = (decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) * (decimal.Parse(_dc.chkNull_0(txtDiscount.Text)) / 100));
                                dr["netAmount"] = decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) - decimal.Parse(_dc.chkNull_0(dr["discount"].ToString()));
                            }
                            else
                            {
                                dr["discount"] = decimal.Parse(_dc.chkNull_0(txtDiscount.Text));
                                dr["netAmount"] = decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) - decimal.Parse(_dc.chkNull_0(txtDiscount.Text));
                            }
                            orderSku.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + ddlSKus.SelectedItem.Text + " Already Exists ');", true);

                            return;
                        }
                    }
                    else if (btnSave.Text == "Update")
                    {

                        DataRow dr = orderSku.Rows[RowNo];
                        dr["skuId"] = foundRows[0]["SKU_ID"];
                        dr["skuName"] = foundRows[0]["SKU_NAME"];
                        dr["price"] = price;
                        dr["qty"] = int.Parse(_dc.chkNull_0(txtQuantity.Text));


                        dr["discountType"] = int.Parse(rbDiscountType.SelectedValue);
                        dr["amount"] = price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));

                        if (rbDiscountType.SelectedValue == "1")
                        {
                            dr["discount"] = (decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) * (decimal.Parse(_dc.chkNull_0(txtDiscount.Text)) / 100));
                            dr["netAmount"] = decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) - decimal.Parse(_dc.chkNull_0(dr["discount"].ToString()));
                        }
                        else
                        {
                            dr["discount"] = decimal.Parse(_dc.chkNull_0(txtDiscount.Text));
                            dr["netAmount"] = decimal.Parse(_dc.chkNull_0(dr["amount"].ToString())) - decimal.Parse(_dc.chkNull_0(txtDiscount.Text));
                        }

                    }
                    Session.Add("orderSku", orderSku);
                    ClearDetail();
                    LoadGird();

                    ScriptManager.GetCurrent(Page).SetFocus(ddlSKus);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('entered price could not less than actual price');", true);
                    ScriptManager.GetCurrent(Page).SetFocus(txtPrice);
                }
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Item Select');", true);
                ScriptManager.GetCurrent(Page).SetFocus(ddlSKus);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' some error occurred');", true);
        }
    }

    protected void grdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RowNo = e.NewEditIndex;

        ddlSKus.SelectedValue = grdOrder.Rows[RowNo].Cells[0].Text;
        txtPrice.Text = grdOrder.Rows[RowNo].Cells[2].Text;
        txtQuantity.Text = grdOrder.Rows[RowNo].Cells[3].Text;

        rbDiscountType.SelectedValue = grdOrder.Rows[RowNo].Cells[6].Text;

        if (rbDiscountType.SelectedValue == "1")
        {
            txtDiscount.Text =Convert.ToString((decimal.Parse(_dc.chkNull_0(grdOrder.Rows[RowNo].Cells[5].Text)) * 100) / decimal.Parse(grdOrder.Rows[RowNo].Cells[4].Text));
        }
        else
        {
            txtDiscount.Text = grdOrder.Rows[RowNo].Cells[5].Text;
        }
        txtAmount.Text = grdOrder.Rows[RowNo].Cells[7].Text;

        ddlSKus.Enabled = false;
        txtQuantity.Focus();

        btnSave.Text = "Update";

    }
    protected void grdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        orderSku = (DataTable)Session["orderSku"];
        if (orderSku.Rows.Count > 0)
        {
            orderSku.Rows.RemoveAt(e.RowIndex);
            Session.Add("orderSku", orderSku);

            LoadGird();
        }
    }

    private void LoadGird()
    {
        int TotalValue = 0;
        decimal TotalAmount = 0;
        decimal TotalDiscount = 0;
        decimal TotalNetAmount = 0;

        orderSku = (DataTable)Session["orderSku"];

        grdOrder.DataSource = orderSku;
        grdOrder.DataBind();

        foreach (DataRow dr in orderSku.Rows)
        {
            TotalValue += int.Parse(dr["qty"].ToString());
            TotalAmount += decimal.Parse(dr["amount"].ToString());
            TotalNetAmount += decimal.Parse(dr["netAmount"].ToString());
            TotalDiscount += decimal.Parse(dr["discount"].ToString());
        }
        txtTotalQty.Text = TotalValue.ToString();
        txtGrossAmount.Text = string.Format("{0:0.00}", TotalAmount);
        txtProductDiscount.Text = string.Format("{0:0.00}", TotalDiscount);

        if (Convert.ToInt32(rdbDiscountType.SelectedValue) == 0)
        {
            txtTotalAmount.Text = string.Format("{0:0.00}", TotalNetAmount -  ((TotalNetAmount) * (Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)) / 100)));
        }
        else
        {
            txtTotalAmount.Text = string.Format("{0:0.00}", TotalNetAmount - Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)));
        }
    }
    private void ClearDetail()
    {

        txtQuantity.Text = "";
        txtDiscount.Text = "";
        txtPrice.Text = "";
        txtAmount.Text = "";
        ddlSKus.Enabled = true;
        btnSave.Text = "Add";

    }

    private bool CheckStock(int Qty,int prevQty, int SkuId, string SkuName)
    {
        
            PurchaseController mController = new PurchaseController();
            DataTable dtstock = mController.SelectSKUClosingStock(int.Parse(ddlDistributor.SelectedValue), SkuId);
            if (dtstock.Rows.Count > 0)
            {
                //if (ViewState["PrevRcdQty"] != null)
                //{
                //    if (int.Parse(dtstock.Rows[0][0].ToString()) + int.Parse(ViewState["PrevRcdQty"].ToString()) < Qty)
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + SkuName + " Current Stock is " + dtstock.Rows[0][0].ToString() + "');", true);
                //        return false;
                //    }
                //}
                //else
                //{
                    if ((int.Parse(dtstock.Rows[0][0].ToString())+ prevQty) < Qty)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + SkuName + " Current Stock is " + dtstock.Rows[0][0].ToString() + "');", true);
                        return false;
                    }
                //}
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + SkuName + " No Stock Found');", true);
                return false;
            }
        
        return true;
    }

   
    #endregion

    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
      //  Calculate();
        //bool flag = true;
        //int orderQty = 0;
        //int recieveQty = 0;
        //int i = 0;
        //foreach (GridViewRow gvr in grdOrder.Rows)
        //{
        //    TextBox txtRecQty = gvr.FindControl("txtRecQty") as TextBox;
        //    TextBox txtCurrentRecQty = gvr.FindControl("txtCurrentRecQty") as TextBox;
        //    if (txtCurrentRecQty.Text.Length > 0)
        //    {
        //        orderSku = (DataTable)Session["orderSku"];
        //        orderQty = int.Parse(orderSku.Rows[i]["qty"].ToString());
        //        recieveQty = Convert.ToInt32(txtRecQty.Text) + Convert.ToInt32(_dc.chkNull_0(txtCurrentRecQty.Text));
               
                
        //        if (Session["invoiceId"] != null)
        //        {
        //            ViewState["PrevRcdQty"] = Convert.ToInt32(_dc.chkNull_0(orderSku.Rows[i]["CurrentRcdQty"].ToString()));
        //        }
        //        if (CheckStock(int.Parse(_dc.chkNull_0(txtCurrentRecQty.Text)), int.Parse(orderSku.Rows[i]["skuId"].ToString()), Convert.ToString(orderSku.Rows[i]["skuName"])))
        //        {
        //            ViewState.Remove("PrevRcdQty");

        //            if (recieveQty > orderQty)
        //            {

        //                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dispatch Qty should not be greater than Order Qty.');", true);
        //                return;
        //            }
        //            i += 1;
        //        }
        //        else
        //        {
        //            flag = false;
        //            break;
                    
        //        }
        //    }
        //}
       // if (flag)//if All Item's stock is OKK
      //  {
          

            orderSku = (DataTable)Session["orderSku"];
        // _dtFreeSku = (DataTable)Session["_dtFreeSku"];

        if (orderSku.Rows.Count > 0)
        {
            foreach (DataRow dr in orderSku.Rows)
            {
                if (CheckStock(int.Parse(_dc.chkNull_0(dr["qty"].ToString())) , int.Parse(_dc.chkNull_0(dr["prevQty"].ToString())), int.Parse(dr["skuId"].ToString()), Convert.ToString(dr["skuName"].ToString())))
                {

                    if (CreditLimit())
                    {
                        long IsValidInsert = -1;

                           try
                        {
                            if (Session["invoiceId"] == null)
                            {
                                IsValidInsert = oeCtrl.Add_Invoice(Convert.ToInt32(rdbInvoiceType.SelectedValue), int.Parse(ddlDistributor.SelectedValue), 0,
                                    long.Parse(ddlCustomer.SelectedValue), 0, Convert.ToDecimal(_dc.chkNull_0(txtGrossAmount.Text)),
                                  Convert.ToDecimal(_dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(hfGstAmount.Value))
                                  , Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)), orderSku, null
                                , int.Parse(this.Session["UserId"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString())
                                , Convert.ToString(rdbDiscountType.SelectedItem.Value), txtRemarks.Text.Trim(), decimal.Parse(_dc.chkNull_0(hfGst.Value))
                                , 0, txtDeliveredTo.Text.Trim(), txtDeliveredThrough.Text.Trim());
                                if (IsValidInsert > 0)
                                {
                                    Session.Add("reportId", IsValidInsert);
                                    Response.Redirect("frmSaleOrderView.aspx?LevelID=" + Request.QueryString["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=26");

                                }
                                else
                                {

                                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
                                }
                            }

                            else
                            {


                                if (oeCtrl.Update_Invoice(Convert.ToInt32(rdbInvoiceType.SelectedValue), Convert.ToInt64(Session["invoiceId"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString())
                                , int.Parse(ddlDistributor.SelectedValue), "0", long.Parse(ddlCustomer.SelectedValue), 0,
                                Convert.ToDecimal(_dc.chkNull_0(txtGrossAmount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text))
                                , Convert.ToDecimal(_dc.chkNull_0(hfGstAmount.Value))
                                , Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)), 0, 0, orderSku, int.Parse(this.Session["UserId"].ToString())
                                , 0, 0, 0, rdbDiscountType.SelectedItem.Value, txtRemarks.Text.Trim(), decimal.Parse(_dc.chkNull_0(hfGst.Value))
                                , 0, txtDeliveredTo.Text.Trim(), txtDeliveredThrough.Text.Trim()))
                                {
                                    Session.Add("reportId", Session["invoiceId"]);
                                    Response.Redirect("frmSaleOrderView.aspx?LevelID=" + Request.QueryString["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=26");

                                }
                                else
                                {

                                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message + "');", true);
                        }

                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Check Customer Business Limit');", true);
                    }

                }
                else
                {
                    break;
                }
            }
        }
        else
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Select At least one Product');", true);
        }
     //   }
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        Session.Remove("invoiceId");

        Response.Redirect("frmSaleOrderView.aspx?LevelID=97&LevelType=3&TopID=26");
    }


    private decimal CalculateGst(decimal mTotalAmount, decimal gst)
    {
        decimal gstAmount = mTotalAmount * (gst / 100);
        return gstAmount;
    }
   
    private decimal CalculateDiscount(decimal mTotalAmount, decimal discount)
    {
        if (discount > 0)
        {
            if (rdbDiscountType.SelectedValue == "0")
            {

                 discount = mTotalAmount * (discount / 100);
                
            }
        }
        return discount;
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerLedger();

        DataTable dt = (DataTable)Session["dtCustomer"];

        DataRow[] foundRows = dt.Select("CUSTOMER_ID  = '" + ddlCustomer.SelectedValue + "'");
        hfGst.Value = "";
        if (foundRows.Length > 0)
        {
            hfGst.Value = foundRows[0]["TAX_RATE"].ToString();
           
        }
    }
    protected void DrpBusinessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomers();
    }
}