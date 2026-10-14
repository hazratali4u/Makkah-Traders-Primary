using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using System.Data;
using SAMSCommonLayer.Classes;

public partial class Forms_frmSaleReturn : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly CustomerDataController _customerCtrl = new CustomerDataController();
    readonly OrderEntryController oeCtrl = new OrderEntryController();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreateTable();
            LoadDistributors();
            this.LoadTown();
            LoadBusinessType();
            LoadCustomers();
            txtGrossAmount.Attributes.Add("readonly", "readonly");
            txtProductDiscount.Attributes.Add("readonly", "readonly");
            txtTotalAmount.Attributes.Add("readonly", "readonly");
            if (Convert.ToString(Session["SaleReturnId"]) != "")
            {
                DataTable dtOrder = oeCtrl.SelectSaleReturn(Convert.ToInt64(Session["SaleReturnId"]),Convert.ToInt32(ddlDistributor.SelectedValue),1, int.Parse(Session["UserID"].ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));
                ddlDistributor.SelectedValue = dtOrder.Rows[0]["DISTRIBUTOR_ID"].ToString();
                ddlBusinessType.SelectedValue = dtOrder.Rows[0]["BUSINESS_TYPE_ID"].ToString();
                this.LoadTown();
                this.ddlTown.SelectedValue = dtOrder.Rows[0]["TownId"].ToString();
                LoadCustomers();
                ddlCustomer.SelectedValue = dtOrder.Rows[0]["CUSTOMER_ID"].ToString();
                txtInvoiceDiscount.Text = String.Format("{0:0.00}", dtOrder.Rows[0]["EXTRA_DISCOUNT_AMOUNT"].ToString());                
                hfGst.Value = dtOrder.Rows[0]["gstPge"].ToString();
                rdbDiscountType.SelectedValue = Convert.ToString((dtOrder.Rows[0]["DISCOUNT_TYPE"].ToString()));
                txtRemarks.Text = (dtOrder.Rows[0]["remarks"].ToString());
                ExistenOrderDetail(Convert.ToInt64(Session["SaleReturnId"]));
                EnableDisableController(false);
            }
            CustomerLedger();
            LoadSKUDetail();
        }
    }

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

    private void LoadTown()
    {
        ddlTown.Items.Clear();
        if (ddlDistributor.Items.Count > 0)
        {
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(ddlDistributor.SelectedValue.ToString()));
            ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 1);
        }
    }
    protected void ddlDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadTown();
        LoadCustomers();
        CustomerLedger();
        LoadSKUDetail();
        ddlCustomer.Focus();
    }
    
    private void LoadBusinessType()
    {
        SLASHCodesController _customerCtrl = new SLASHCodesController();
        DataTable dt = _customerCtrl.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerTypeBusiness, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillDropDownList(ddlBusinessType, dt, 0, 2, true);
    }
    protected void ddlBusinessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomers();
        CustomerLedger();
    }
    private void LoadCustomers()
    {
        ddlCustomer.Items.Clear();
        if (ddlDistributor.Items.Count > 0)
        {
            DataTable dtCustomer = _customerCtrl.SelectPrincipalCustomer(int.Parse(ddlDistributor.SelectedValue), int.Parse(ddlBusinessType.SelectedValue), 4, Convert.ToInt32(ddlTown.SelectedValue),Constants.IntNullValue,Constants.IntNullValue, int.Parse(Session["UserId"].ToString()));
            if (dtCustomer.Rows.Count > 0)
            {
                clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");
                hfGst.Value = dtCustomer.Rows[0]["TAX_RATE"].ToString();
                Session.Add("dtCustomer", dtCustomer);
            }
        }
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerLedger();

        if (Session["dtCustomer"] != null)
        {
            DataTable dt = (DataTable)Session["dtCustomer"];
            DataRow[] foundRows = dt.Select("CUSTOMER_ID  = '" + ddlCustomer.SelectedValue + "'");
            if (foundRows.Length > 0)
            {
                hfGst.Value = foundRows[0]["TAX_RATE"].ToString();
               
            }
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

                    if (Session["SaleReturnId"] != null)
                    {
                        lblLedgerBalance.Text = string.Format("{0:0,0.00}", Opening - Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)));
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

    #region Detail Panel

    DataTable orderSku = new DataTable();
    private static int RowNo;

    private void LoadSKUDetail()
    {
        ddlSKus.Items.Clear();

        if (ddlDistributor.Items.Count > 0)
        {
           
                    SKUPriceDetailController PController = new SKUPriceDetailController();
                    DataTable Dtsku_Price = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddlDistributor.SelectedValue), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
                    clsWebFormUtil.FillDropDownList(ddlSKus, Dtsku_Price, "SKU_ID", "SkuPriceDetail", true);
                    Session.Add("Dtsku_Price", Dtsku_Price);
           
        }
    }
    private void CreateTable()
    {
        orderSku = new DataTable();
        orderSku.Columns.Add("orderDetailId", typeof(long));
        orderSku.Columns.Add("skuId", typeof(int));
        orderSku.Columns.Add("skuName", typeof(string));
        orderSku.Columns.Add("price", typeof(decimal));
        orderSku.Columns.Add("qty", typeof(int));
        orderSku.Columns.Add("Quantity", typeof(int));
        orderSku.Columns.Add("UNITS_IN_CASE", typeof(int));
        orderSku.Columns.Add("originalQty", typeof(int));
        orderSku.Columns.Add("discount", typeof(decimal));
        orderSku.Columns.Add("discountType", typeof(int));
        orderSku.Columns.Add("AMOUNT", typeof(decimal));
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
            
            DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKus.SelectedValue + "'");

            decimal price = Convert.ToDecimal(_dc.chkNull_0(txtPrice.Text));

            if (price > 0)
            {
                if (foundRows.Length > 0)
                {
                    orderSku = (DataTable)Session["orderSku"];
                    if (btnSave.Text == "Add")
                    {
                        if (CheckDublicateSKU())
                        {

                            DataRow dr = orderSku.NewRow();
                            dr["skuId"] = foundRows[0]["SKU_ID"];
                            dr["skuName"] = foundRows[0]["SKU_NAME"]; //AS SKUnMAE
                            dr["price"] = price;
                            dr["qty"] = int.Parse(_dc.chkNull_0(txtQuantity.Text));
                            dr["Quantity"] = int.Parse(_dc.chkNull_0(txtQuantityUnit.Text));
                            dr["UNITS_IN_CASE"] = int.Parse(foundRows[0]["UNITS_IN_CASE"].ToString());
                            dr["discountType"] = int.Parse(rbDiscountType.SelectedValue);
                            dr["amount"] = price * ((Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(_dc.chkNull_0(txtQuantity.Text)))) + Convert.ToDecimal(_dc.chkNull_0(txtQuantityUnit.Text)));
                            //dr["amount"] = price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));

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
                        dr["Quantity"] = int.Parse(_dc.chkNull_0(txtQuantityUnit.Text));
                        dr["UNITS_IN_CASE"] = int.Parse(foundRows[0]["UNITS_IN_CASE"].ToString());

                        dr["discountType"] = int.Parse(rbDiscountType.SelectedValue);
                        dr["amount"] = price * ((Convert.ToDecimal(_dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(_dc.chkNull_0(txtQuantity.Text)))) + Convert.ToDecimal(_dc.chkNull_0(txtQuantityUnit.Text)));
                        //dr["amount"] = price * decimal.Parse(_dc.chkNull_0(txtQuantity.Text));

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
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Item Select');", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Item Price!');", true);

            }
            EnableDisableController(false);
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
        txtQuantityUnit.Text = grdOrder.Rows[RowNo].Cells[4].Text;
        rbDiscountType.SelectedValue = grdOrder.Rows[RowNo].Cells[7].Text;

        if (rbDiscountType.SelectedValue == "1")
        {
            txtDiscount.Text = Convert.ToString((decimal.Parse(_dc.chkNull_0(grdOrder.Rows[RowNo].Cells[6].Text)) * 100) / decimal.Parse(grdOrder.Rows[RowNo].Cells[5].Text));
        }
        else
        {
            txtDiscount.Text = grdOrder.Rows[RowNo].Cells[6].Text;
        }
        txtAmount.Text = grdOrder.Rows[RowNo].Cells[8].Text;
        ddlSKus.Enabled = false;
        txtPrice.Focus();
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
        int TotalValueUnit = 0;
        decimal TotalAmount = 0;
        decimal TotalDiscount = 0;
        decimal TotalNetAmount = 0;

        orderSku = (DataTable)Session["orderSku"];

        grdOrder.DataSource = orderSku;
        grdOrder.DataBind();

        foreach (DataRow dr in orderSku.Rows)
        {
            TotalValue += int.Parse(dr["qty"].ToString());
            TotalValueUnit += int.Parse(dr["Quantity"].ToString());
            TotalAmount += decimal.Parse(dr["amount"].ToString());
            TotalNetAmount += decimal.Parse(dr["netAmount"].ToString());
            TotalDiscount += decimal.Parse(dr["discount"].ToString());
        }
        txtTotalQty.Text = TotalValue.ToString();
        txtTotalUnit.Text = TotalValueUnit.ToString();
        txtGrossAmount.Text = string.Format("{0:0.00}", TotalAmount);
        txtProductDiscount.Text = string.Format("{0:0.00}", TotalDiscount);

        if (Convert.ToInt32(rdbDiscountType.SelectedValue) == 0)
        {
            txtTotalAmount.Text = string.Format("{0:0.00}", TotalNetAmount - ((TotalNetAmount) * (Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)) / 100)));
        }
        else
        {
            txtTotalAmount.Text = string.Format("{0:0.00}", TotalNetAmount - Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)));
        }
    }
    private void ClearDetail()
    {

        txtQuantity.Text = "";
        txtQuantityUnit.Text = "";
        txtDiscount.Text = "";
        txtPrice.Text = "";
        txtAmount.Text = "";
        ddlSKus.Enabled = true;
        btnSave.Text = "Add";

    }
    #endregion

    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        try
        {
            orderSku = (DataTable)Session["orderSku"];
            if (orderSku.Rows.Count > 0)
            {
                if (Convert.ToString(Session["SaleReturnId"]) == "")
                {
                    long flag = oeCtrl.Add_SaleReturn(int.Parse(ddlDistributor.SelectedValue),long.Parse(ddlCustomer.SelectedValue)
                    , Convert.ToInt32(txtTotalQty.Text), orderSku, int.Parse(Session["UserId"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), 1
                    , Convert.ToDecimal(_dc.chkNull_0(hfGst.Value)),Convert.ToDecimal(txtGrossAmount.Text), Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)), Convert.ToInt32(rdbDiscountType.SelectedItem.Value), txtRemarks.Text.Trim());
                    if (flag > 0)
                    {
                        Session.Add("reportId", flag);
                        Response.Redirect("frmSaleReturnView.aspx?LevelID=" + Request["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID="+ Request.QueryString["TopID"].ToString());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' some error occurred');", true);
                    }
                }
                else
                {
                    bool flag = oeCtrl.Update_SaleReturn(Convert.ToInt64(Session["SaleReturnId"]), int.Parse(ddlDistributor.SelectedValue), long.Parse(ddlCustomer.SelectedValue)
                     , Convert.ToInt32(txtTotalQty.Text), orderSku, int.Parse(Session["UserId"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), 1
                     , Convert.ToDecimal(_dc.chkNull_0(hfGst.Value)), Convert.ToDecimal(txtGrossAmount.Text), Convert.ToDecimal(_dc.chkNull_0(txtInvoiceDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtProductDiscount.Text)), Convert.ToDecimal(_dc.chkNull_0(txtTotalAmount.Text)), Convert.ToInt32(rdbDiscountType.SelectedItem.Value), txtRemarks.Text.Trim());
                    
                    if (flag)
                    {

                        Session.Add("reportId", Session["SaleReturnId"]);
                        Response.Redirect("frmSaleReturnView.aspx?LevelID=" + Request["LevelID"].ToString() + "&LevelType=" + Request.QueryString["LevelType"].ToString() + "&TopID=" + Request.QueryString["TopID"].ToString());
                    }
                    else
                    {

                       ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' some error occurred');", true);
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' At least one SKU must enter');", true);

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' some error occurred');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        CreateTable();
        LoadGird();
        ClearDetail();
        Response.Redirect("frmSaleReturnView.aspx?LevelID=103&LevelType=3&TopID=26");
    }
 

    #region Update Panel
   
    private void ExistenOrderDetail(long OrderId)
    {
        orderSku = oeCtrl.SelectSaleReturnDetail(Convert.ToInt32(ddlDistributor.SelectedValue), OrderId, Constants.DateNullValue);
        Session.Add("orderSku", orderSku);
        LoadGird();

    }
  
    private void EnableDisableController(bool CValue)
    {
        if (CValue == true)
        {
            ddlDistributor.Enabled = true;
            ddlBusinessType.Enabled = true;
            ddlCustomer.Enabled = true;
            
        }
        else
        {

            ddlDistributor.Enabled = false;
            ddlBusinessType.Enabled = false;
            ddlCustomer.Enabled = false;
        }
    }
    private void ClearMasterALL()
    {

        EnableDisableController(true);
        Session.Remove("orderSku");
        CreateTable();

        LoadGird();
       

        RowNo = 0;
        
        ScriptManager.GetCurrent(Page).SetFocus(ddlCustomer);
    }

    #endregion

    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadCustomers();
    }
}