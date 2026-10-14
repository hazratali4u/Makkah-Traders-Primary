using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;
using System.Linq;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmPurchaseEntry : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly DataControl dc = new DataControl();
    private static int RowNo;
    private static int PrivouseQty, FreePrivousQty;
    DataTable PurchaseSKU;
    private static int mCustomerTypeId;
    private static int mCustomerVolClassId;
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

            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtFromdate_CalendarExtender.StartDate = Configuration.SystemCurrentDateTime.AddDays(-15);
            txtFromdate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtFromdate.Attributes.Add("readonly", "readonly");
            if (txtFreightAmount.Text == "")
            {
                txtFreightAmount.Text = "0";
            }            
            this.LoadPrincipal();
            this.LoadDistributor();
            this.LoadTown();
            this.LoadPromotion();
            this.LoadSKUDetail();
            this.CreatTable();
            this.GetDocumentNo();
            this.LoadCustomer();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
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
    private void CreatTable()
    {
        PurchaseSKU = new DataTable();
        PurchaseSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchaseSKU.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_Code", typeof(string));
        PurchaseSKU.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKU.Columns.Add("PACKSIZE", typeof(string));
        PurchaseSKU.Columns.Add("BATCH_NO", typeof(string));
        PurchaseSKU.Columns.Add("PRICE", typeof(decimal));
        PurchaseSKU.Columns.Add("DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("TRADE_PRICE", typeof(decimal));
        PurchaseSKU.Columns.Add("Quantity", typeof(int));
        PurchaseSKU.Columns.Add("QuantityCtn", typeof(int));
        PurchaseSKU.Columns.Add("FREE_SKU", typeof(int));
        PurchaseSKU.Columns.Add("AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("UNITS_IN_CASE", typeof(int));
        PurchaseSKU.Columns.Add("STANDARD_DISCOUNT_PER", typeof(decimal));
        this.Session.Add("PurchaseSKU", PurchaseSKU);
    }

    #region Load

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(2, Constants.IntNullValue, Constants.LongNullValue, int.Parse(this.Session["UserId"].ToString()), 0);

        clsWebFormUtil.FillDropDownList(this.drpDocumentNo, dt, 0, 24,false);
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }
    private void LoadPrincipal()
    {
        hfIsPromotion.Value = "0";

        DataTable dtPromotion = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(drpPrincipal, dtPromotion, 0, 1, true);



        if (dtPromotion.Rows.Count > 0)
        {
            Session.Add("dtPromotion", dtPromotion);

            hfIsPromotion.Value = dtPromotion.Rows[0]["Is_ManualDiscount"].ToString();
        }
        enableDisable();
    }
    private void LoadPromotion()
    {
        try
        {
            PromotionCollections_Controller Arpc = new PromotionCollections_Controller();
            if (hfIsPromotion.Value == "0")
            {
                OrderEntryController orderController = new OrderEntryController();
                Arpc = orderController.LoadSchemes(Convert.ToInt32(drpDistributor.SelectedValue), Constants.IntNullValue, Convert.ToDateTime(Session["CurrentWorkDate"]));
               
            }
            Session.Add("Arpc", Arpc);
        }
        catch (Exception)
        {

            throw;
        }
    }
 

    private void LoadTown()
    {
        ddlTown.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            GeoHierarchyController gController = new GeoHierarchyController();
            DataTable dt = gController.SelectGeoHierarchy(int.Parse(drpDistributor.SelectedValue.ToString()));
          //  ddlTown.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(ddlTown, dt, 0, 1);
        }
    }
    private void LoadCustomer()
    {
        CustomerDataController _customerCtrl = new CustomerDataController();
        ddlCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dtCustomer = _customerCtrl.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, 3, Convert.ToInt32(ddlTown.SelectedValue), Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()));
            clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CustomerTown");
            Session.Add("dtCustomer", dtCustomer);
        }
    }


    private void LoadSKUDetail()
    {
        if (drpPrincipal.Items.Count > 0)
        {
            DataTable Dtsku_Price = PController.SelectDataPrice(int.Parse(drpPrincipal.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillDropDownList(ddlSKuCde, Dtsku_Price, "SKU_ID", "SkuUName", true);
            this.Session.Add("Dtsku_Price", Dtsku_Price);

            //DataTable dtskuPrice = (DataTable)Session["Dtsku_Price"];
            //DataRow[] foundRows = dtskuPrice.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
            //if (foundRows.Length > 0)
            //{
            //    decimal mTradePrice = Math.Round(Convert.ToDecimal(foundRows[0]["DISTRIBUTOR_PRICE"]), 2);
            //    txtCurrentRate.Text = Convert.ToString(mTradePrice);
            //}
        }
    }

    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
            this.LoadCustomer();
            ddlCustomer.SelectedValue = dt.Rows[0]["CustomerID"].ToString();
            drpPrincipal.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
            
            LoadTown();
            ddlTown.SelectedValue = dt.Rows[0]["TOWN_ID"].ToString();
            txtDocumentNo.Text = dt.Rows[0][2].ToString();
            txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            txtRemakrs.Text = dt.Rows[0]["REMARKS"].ToString();
            txtTransporter.Text = dt.Rows[0]["TRANSPORTER"].ToString();
            txtVehicle.Text = dt.Rows[0]["VEHICLE_NO"].ToString();
            txtGatePass.Text = dt.Rows[0]["GATE_PASS"].ToString();
            txtDispatchTo.Text = dt.Rows[0]["DISPATCH_TO"].ToString();
            txtFromdate.Text = DateTime.Parse(dt.Rows[0]["DOCUMENT_DATE"].ToString()).ToString("dd-MMM-yyyy");
            txtGrossAmount.Text = string.Format("{0:0.00}",Convert.ToDecimal(dt.Rows[0]["TOTAL_AMOUNT"].ToString()));
            txtDiscount2.Text = string.Format("{0:0.00}", Convert.ToDecimal(dt.Rows[0]["DISCOUNT"].ToString()));
            txtFreightAmount.Text = string.Format("{0:0.00}", Convert.ToDecimal(dt.Rows[0]["FREIGHT_AMOUNT"].ToString()));
            PurchaseSKU = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGrid();
            if (pnlSummary.Visible)
            {
                decimal SchemeValue = 0;
                
                foreach (DataRow dr in PurchaseSKU.Rows)
                {
                    //  GrossAmount += Convert.ToDecimal(dr["Amount"]);
                    // Discount += Convert.ToDecimal(dr["Discount"]) * 100 / Convert.ToDecimal(dr["Amount"]);
                    SchemeValue += Convert.ToDecimal(dr["FREE_SKU"]);
                }
               // txtGrossAmount.Text = String.Format("{0:0.00}", GrossAmount);
               // txtDiscount2.Text = String.Format("{0:0.00}", Discount);
                txtSchemeValue.Text = string.Format("{0:0.00}", SchemeValue);
                
                txtNetValue.Text = string.Format("{0:0.00}", Convert.ToDecimal(txtGrossAmount.Text) - (Convert.ToDecimal(txtDiscount2.Text) + SchemeValue) + Convert.ToDecimal(txtFreightAmount.Text));
            }
        }
    }
    private void LoadGrid()
    {
        int TotalValue = 0;
        int TotalCtn = 0;
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        GrdPurchase.DataSource = PurchaseSKU;
        GrdPurchase.DataBind();
        foreach (DataRow dr in PurchaseSKU.Rows)
        {
            TotalValue += int.Parse(dr["Quantity"].ToString());
            TotalCtn += int.Parse(dr["QuantityCtn"].ToString());
        }
        txtTotalQuantity.Text = TotalValue.ToString();
        txtTotalCtn.Text = TotalCtn.ToString();
    }

    #endregion

    #region Index Change

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            this.CreatTable();
            this.Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGrid();
            this.ClearAll();
            txtQuantity.Enabled = true;
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            ddlCustomer.Enabled = true;
        }
        else
        {
            txtBuiltyNo.Text = "";
            txtDocumentNo.Text = "";
            txtRemakrs.Text = "";
            txtGatePass.Text = "";
            txtTransporter.Text = "";
            txtVehicle.Text = "";
            txtQuantity.Enabled = false;
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            ddlCustomer.Enabled = false;
            ddlTown.Enabled = false;
            this.LoadDocumentDetail();
            this.LoadSKUDetail();
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadPromotion();
        this.LoadTown();
        this.LoadCustomer();
    }
    protected void drpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPromotion = (DataTable)this.Session["dtPromotion"];
        if (dtPromotion.Rows.Count > 0)
        {
            DataRow[] foundRows = dtPromotion.Select("Company_Id  = '" + drpPrincipal.SelectedValue + "'");

            if (foundRows.Length > 0)
            {
                hfIsPromotion.Value = foundRows[0]["Is_ManualDiscount"].ToString();
                enableDisable();
            }
        }
        LoadSKUDetail();
    }
    protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadCustomer();
    }
    protected void ChbFreeSKU_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChbFreeSKU.Checked == true)
        //{
        //    lblSchemeValue.Enabled = true;
        //    txtScheme.Enabled = true;
        //}
        //else
        //{
        //    txtScheme.Text = "0";
        //    lblSchemeValue.Enabled = false;
        //    txtScheme.Enabled = false;
        //}
    }

    #endregion

    #region Grid 

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RowNo = e.NewEditIndex;
        ddlSKuCde.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        txtCtn.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtDiscount.Text =Convert.ToString((Convert.ToDecimal(GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text.ToString()) * 100) / Convert.ToDecimal(GrdPurchase.Rows[e.NewEditIndex].Cells[10].Text));
        PrivouseQty = int.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text);
        FreePrivousQty = int.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text);
        txtScheme.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        txtCurrentRate.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        ddlSKuCde.Enabled = false;
        txtCtn.Focus();
        btnSave.Text = "Update Sku";
    }
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            PurchaseSKU.Rows.RemoveAt(e.RowIndex);
            this.Session.Add("PurchaseSKU", PurchaseSKU);
            this.LoadGrid();
        }

        if (pnlSummary.Visible)
        {
            decimal FreightAmount = 0;
            decimal SchemeValue = 0;
            decimal GrossAmount = 0;
            decimal Discount = 0;
            foreach (DataRow dr in PurchaseSKU.Rows)
            {
                GrossAmount += Convert.ToDecimal(dr["Amount"]);
                Discount += Convert.ToDecimal(dr["Amount"]) / 100 * Convert.ToDecimal(dr["Discount"]);
                SchemeValue += Convert.ToDecimal(dr["FREE_SKU"]);
                FreightAmount = Convert.ToDecimal(txtFreightAmount.Text);
            }
            txtGrossAmount.Text = String.Format("{0:0.00}", GrossAmount);
            txtDiscount2.Text = String.Format("{0:0.00}", Discount);
            txtSchemeValue.Text = String.Format("{0:0.00}", SchemeValue);
            txtFreightAmount.Text = String.Format("{0:0.00}", FreightAmount);
            txtNetValue.Text = String.Format("{0:0.00}", GrossAmount - Discount - SchemeValue - FreightAmount);
        }
    }

    #endregion

    #region Click Operations


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtFreightAmount.Text == "")
        {
            txtFreightAmount.Text = "0";
        }

        DataTable Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_ID  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length > 0)
        {
            decimal Price = 0;
            Price = Convert.ToDecimal(dc.chkNull_0(txtCurrentRate.Text));
            if (btnSave.Text == "Add Sku")
            {
                if (CheckDublicateSKU())
                {
                    DataRow dr = PurchaseSKU.NewRow();
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    dr["BATCH_NO"] = "N/A";
                    dr["TRADE_PRICE"] = foundRows[0]["TRADE_PRICE"];
                    dr["PRICE"] = Price;

                    dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                    dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtCtn.Text));
                    dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
                    dr["AMOUNT"] = Price * ((Convert.ToDecimal(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(dc.chkNull_0(txtCtn.Text)))) + Convert.ToDecimal(dc.chkNull_0(txtQuantity.Text)));

                    if (hfIsPromotion.Value == "0")
                    {
                        dr["FREE_SKU"] = 0;
                        dr["DISCOUNT"] = 0;
                    }
                    else
                    {
                        dr["FREE_SKU"] = int.Parse(dc.chkNull_0(txtScheme.Text));
                        dr["DISCOUNT"] = Convert.ToDecimal(dr["AMOUNT"]) / 100 * Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text));
                    }

                   PurchaseSKU.Rows.Add(dr);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('  " + ddlSKuCde.SelectedItem.Text + " Already Exists ');", true);
                    return;
                }
            }
            else if (btnSave.Text == "Update Sku")
            {
                DataRow dr = PurchaseSKU.Rows[RowNo];
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                dr["BATCH_NO"] = "N/A";
                dr["TRADE_PRICE"] = foundRows[0]["TRADE_PRICE"];
                
                dr["PRICE"] = Price;
                dr["Quantity"] = int.Parse(dc.chkNull_0(txtQuantity.Text));
                dr["QuantityCtn"] = int.Parse(dc.chkNull_0(txtCtn.Text));
                dr["UNITS_IN_CASE"] = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
                dr["AMOUNT"] = Price * ((Convert.ToDecimal(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString())) * (Convert.ToDecimal(dc.chkNull_0(txtCtn.Text)))) + Convert.ToDecimal(dc.chkNull_0(txtQuantity.Text)));

                if (hfIsPromotion.Value == "0")
                {
                    dr["FREE_SKU"] = 0;
                    dr["DISCOUNT"] = 0;
                }
                else
                {
                    dr["FREE_SKU"] = int.Parse(dc.chkNull_0(txtScheme.Text));
                    dr["DISCOUNT"] = Convert.ToDecimal(dr["AMOUNT"]) / 100 * Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text));
                }
            }
            if (pnlSummary.Visible)
            {
                decimal FreightAmount = 0;
                decimal SchemeValue = 0;
                decimal GrossAmount = 0;
                decimal Discount = 0;
                foreach (DataRow dr in PurchaseSKU.Rows)
                {
                    GrossAmount += Convert.ToDecimal(dr["Amount"]);
                    if (hfIsPromotion.Value == "0")
                    {
                        Discount += Convert.ToDecimal(dr["Amount"]) / 100 * Convert.ToDecimal(dr["Discount"]);
                    }
                    else
                    {
                        Discount +=  Convert.ToDecimal(dr["Discount"]);
                    }
                    SchemeValue += Convert.ToDecimal(dr["FREE_SKU"]);
                    FreightAmount = Convert.ToDecimal(txtFreightAmount.Text);
                }
                txtGrossAmount.Text = String.Format("{0:0.00}", GrossAmount);
                txtDiscount2.Text = String.Format("{0:0.00}", Discount);
                txtSchemeValue.Text = String.Format("{0:0.00}", SchemeValue);
                txtFreightAmount.Text = String.Format("{0:0.00}", FreightAmount);
                txtNetValue.Text = String.Format("{0:0.00}", GrossAmount - Discount - SchemeValue - FreightAmount);
            }

            this.Session.Add("PurchaseSKU", PurchaseSKU);
            this.ClearAll();
            DisAbaleOption(true);
            btnCalculate_Click(null, null);
            //this.LoadGrid();
            ScriptManager.GetCurrent(Page).SetFocus(ddlSKuCde);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Wrong SKU Select');", true);
        }
    }
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        if (hfIsPromotion.Value == "0")
        {
            DataControl dc = new DataControl();
            OrderEntryController or = new OrderEntryController();
            SKUGroupController GroupCtl = new SKUGroupController();
            DataTable dt = (DataTable)this.Session["PurchaseSKU"];

            foreach (DataRow dr in dt.Rows)
            {
                dr["DISCOUNT"] = 0;
                dr["STANDARD_DISCOUNT_PER"] = 0;
            }

            DataTable dtCustomer = (DataTable)this.Session["dtCustomer"];
            DataRow[] foundRows = dtCustomer.Select("CUSTOMER_ID  = '" + ddlCustomer.SelectedValue + "'");

            mCustomerTypeId = int.Parse(foundRows[0]["channelType"].ToString());
            mCustomerVolClassId = int.Parse(foundRows[0]["volumeId"].ToString());

            PromotionCollections_Controller Arpc = (PromotionCollections_Controller)Session["Arpc"];
            if (dt.Rows.Count > 0)
            {
                System.Collections.ArrayList ARlistOffer = or.GetPromotionOffers(Arpc, mCustomerVolClassId, mCustomerTypeId, dt, false);
                for (int i = 0; i < ARlistOffer.Count; i++)
                {
                    PromoOffers_Collection pofferCol = (PromoOffers_Collection)ARlistOffer[i];
                    if (pofferCol.Is_Claimable)
                    {
                        if (pofferCol.SKU_ID > 0)
                        {
                            #region slab on SKU
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (int.Parse(dr["SKU_ID"].ToString()) == pofferCol.SKU_ID)
                                {
                                    if (pofferCol.Offer_Value > 0)
                                    {
                                        dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString())) + pofferCol.Offer_Value;
                                    }
                                    if (pofferCol.Discount > 0)
                                    {
                                        dr["STANDARD_DISCOUNT_PER"] = decimal.Parse(dc.chkNull_0(dr["STANDARD_DISCOUNT_PER"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
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
                                if (GroupCtl.ExistsInGroup(Constants.IntNullValue, pofferCol.Group_ID, int.Parse(dr["SKU_ID"].ToString())))
                                {
                                    if (pofferCol.Is_And == true)
                                    {
                                        if (pofferCol.Offer_Value > 0)
                                        {
                                            dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString())) + pofferCol.Offer_Value;
                                        }
                                        if (pofferCol.Discount > 0)
                                        {
                                            dr["STANDARD_DISCOUNT_PER"] = decimal.Parse(dc.chkNull_0(dr["STANDARD_DISCOUNT_PER"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (pofferCol.SKU_ID > 0)
                        {
                            #region slab on SKU
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (int.Parse(dr["SKU_ID"].ToString()) == pofferCol.SKU_ID)
                                {
                                    if (pofferCol.Offer_Value > 0)
                                    {
                                        dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString())) + pofferCol.Offer_Value;
                                    }
                                    if (pofferCol.Discount > 0)
                                    {
                                        dr["STANDARD_DISCOUNT_PER"] = decimal.Parse(dc.chkNull_0(dr["STANDARD_DISCOUNT_PER"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
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
                                if (GroupCtl.ExistsInGroup(Constants.IntNullValue, pofferCol.Group_ID, int.Parse(dr["SKU_ID"].ToString())))
                                {
                                    if (pofferCol.Offer_Value > 0)
                                    {
                                        dr["DISCOUNT"] = decimal.Parse(dc.chkNull_0(dr["DISCOUNT"].ToString())) + pofferCol.Offer_Value;
                                    }
                                    if (pofferCol.Discount > 0)
                                    {
                                        dr["STANDARD_DISCOUNT_PER"] = decimal.Parse(dc.chkNull_0(dr["STANDARD_DISCOUNT_PER"].ToString())) + (decimal.Parse(pofferCol.Discount.ToString()) / 100);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }

                #region Recalulate Products

                foreach (DataRow dr in dt.Rows)
                {
                    dr["DISCOUNT"] = Convert.ToDecimal(dr["DISCOUNT"]) + (decimal.Parse(dc.chkNull_0(dr["STANDARD_DISCOUNT_PER"].ToString())) * (decimal.Parse(dc.chkNull_0(dr["AMOUNT"].ToString()))));
                }
                #endregion
            }
            
        }
        LoadGrid();
    }
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            this.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }
        else
        {
            SAMSCommonLayer.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
            if (PurchaseSKU.Rows.Count > 0)
            {
                DistributorController mDayClose = new DistributorController();
                DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()));
                if (dt.Rows.Count > 0)
                {
                    if (CalculatePurchase(DateTime.Parse(txtFromdate.Text)))
                    {
                        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
                        PurchaseSKU.Rows.Clear();
                        this.Session.Add("PurchaseSKU", PurchaseSKU);
                        this.LoadGrid();
                        this.GetDocumentNo();
                        drpDistributor.Enabled = true;
                        drpPrincipal.Enabled = true;
                        this.ClearAll();
                        txtBuiltyNo.Text = "";
                        txtDocumentNo.Text = "";
                        txtRemakrs.Text = "";
                        txtGatePass.Text = "";
                        txtTransporter.Text = "";
                        txtVehicle.Text = "";
                        txtDispatchTo.Text = "";
                        DisAbaleOption(false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Wrong Location');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' At least one SKU must enter');", true);
            }
            clearAmount();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisAbaleOption(false);
        this.CreatTable();
        this.LoadGrid();
        this.ClearAll();
        txtDocumentNo.Text = "";
        txtBuiltyNo.Text = "";
        txtDocumentNo.Text = "";
        txtRemakrs.Text = "";
        txtGatePass.Text = "";
        txtTransporter.Text = "";
        txtVehicle.Text = "";
    }

    #endregion
    
    private void clearAmount()
    {
        txtGrossAmount.Text = "";
        txtDiscount2.Text = "";
        txtSchemeValue.Text = "";
        txtFreightAmount.Text = "";
        txtNetValue.Text = "";
    }
    
    private bool CheckDublicateSKU()
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = PurchaseSKU.Select("SKU_Id  = '" + ddlSKuCde.SelectedValue + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }

        return false;

    }
    
    private bool CalculatePurchase(DateTime MWorkDate)
    {
        decimal mTotalAmount = 0;

        PurchaseController mController = new PurchaseController();
        DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKU"];
        foreach (DataRow dr in dtPurchaseDetail.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

        }
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            bool mResult = mController.InsertSalesPurchase2(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, 2
            , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, decimal.Parse(dc.chkNull_0(txtFreightAmount.Text)), false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(this.Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), decimal.Parse(dc.chkNull_0(txtDiscount2.Text)), decimal.Parse(dc.chkNull_0(txtSchemeValue.Text)), Convert.ToInt64(ddlCustomer.SelectedValue)
            ,txtRemakrs.Text,txtTransporter.Text,txtVehicle.Text,txtGatePass.Text,txtDispatchTo.Text,int.Parse(ddlTown.SelectedValue.ToString()),txtDriverCellNo.Text,txtCourier.Text, ChkSaleOnly.Checked);
            return mResult;
        }
        else
        {
            bool mResult = mController.UpdateSalesPurchase(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, 2
            , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, decimal.Parse(dc.chkNull_0(txtFreightAmount.Text)), false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(this.Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), decimal.Parse(dc.chkNull_0(txtDiscount2.Text)), decimal.Parse(dc.chkNull_0(txtSchemeValue.Text)), Convert.ToInt32(ddlCustomer.SelectedValue)
            , txtRemakrs.Text, txtTransporter.Text, txtVehicle.Text, txtGatePass.Text, txtDispatchTo.Text, txtDriverCellNo.Text, txtCourier.Text, ChkSaleOnly.Checked);
            return mResult;
        }
    }
    
    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
            ddlCustomer.Enabled = false;
        }
        else
        {
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            ddlCustomer.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    private void enableDisable()
    {
        if (hfIsPromotion.Value == "0")
        {
            txtScheme.Text = "0";
            lblSchemeValue.Enabled = false;
            txtScheme.Enabled = false;
            txtDiscount.Text = "0";
            Label10.Enabled = false;
            txtDiscount.Enabled = false;
        }
        else
        {
            txtDiscount.Text = "0";

            Label10.Enabled = true;
            txtDiscount.Enabled = true;

            lblSchemeValue.Enabled = true;
            txtScheme.Enabled = true;
        }
    }

    private void ClearAll()
    {
        txtQuantity.Text = "";
        txtCtn.Text = "";
        txtScheme.Text = "0";
        txtCurrentRate.Text = "0.00";
        txtDiscount.Text = "";
        ddlSKuCde.Enabled = true;
        btnSave.Text = "Add Sku";
        PrivouseQty = 0;
        FreePrivousQty = 0;
    }
    private bool IsDayClosed()
    {
        bool flag = false;
        DistributorController DistrCtl = new DistributorController();
        DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(drpDistributor.SelectedValue), 3);
        if (Convert.ToDateTime(Session["CurrentWorkDate"]) <= Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
        {
            flag = false;
        }
        else
        {
            flag = true;
        }

        return flag;
    }
}