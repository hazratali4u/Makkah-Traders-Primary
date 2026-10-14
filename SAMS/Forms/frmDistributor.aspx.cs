using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommonLayer.Classes;    

public partial class Forms_frmDistributor : System.Web.UI.Page
{
    DistributorController mController = new DistributorController();
    private static int DistributorId;
    private static int CompanyId;

    /// <summary>
    /// Page_Load Function Populates All Combos and Grids On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GetDistributorType();
            this.LoadGird();
            this.LoadCompany();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
        }
    }

    /// <summary>
    /// Loads Companies To Comapny Combo
    /// </summary>
    private void LoadCompany()
    {
        CompanyController mCompany = new CompanyController();
        DataTable dt = mCompany.SelectCompany(Constants.IntNullValue, Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(DrpCompanyName, dt, 0, 1, true);
    }

    /// <summary>
    /// Loads Location Types To LocationType Combo
    /// </summary>
    private void GetDistributorType()
    {
        DataTable dt = mController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }

    /// <summary>
    /// Loads Locations To Location Grid
    /// </summary>
    private void LoadGird()
    {
        DataTable dtDistributor = mController.SelectAllDistributors(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue);
        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 3:
                dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 4:
                dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 5:
                dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                dtDistributor.DefaultView.RowFilter = "Distributor_name" + " like '%" + "" + "%'";
                break;
        }
        GridDistributor.DataSource = dtDistributor;
        GridDistributor.DataBind();
    }

    /// <summary>
    /// Sets/UnSets Focus To GST No. TextBox
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void cbRegistered_CheckedChanged(object sender, EventArgs e)
    {
        if (cbRegistered.Checked == true)
        {
            txtgstno.Enabled = true;
            txtgstno.Focus();
        }
        else
        {
            txtgstno.Text = "";
            txtgstno.Enabled = false;
        }
    }

    /// <summary>
    /// Saves Or Updates A Location
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                mController.InsertDistributor(int.Parse(DrpCompanyName.SelectedValue.ToString()), Constants.IntNullValue, !chkIsActive.Checked, System.DateTime.Now, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
                    , Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddDistributorType.SelectedValue.ToString()), txtcontactperson.Text.Trim(), txtPhoneNo.Text.Trim(), txtgstno.Text.Trim(),
                    txtFax.Text.Trim(), txtAddress1.Text.Trim(), txtAddress2.Text.Trim(), txtDistributorCode.Text.Trim(), txtDistributorName.Text.Trim(), null, cbRegistered.Checked, 1, int.Parse(this.Session["UserId"].ToString()));
            }
            else
            {
                mController.UpdateDistributor(int.Parse(DrpCompanyName.SelectedValue.ToString()), Constants.IntNullValue, !chkIsActive.Checked, Constants.DateNullValue, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
                , Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddDistributorType.SelectedValue.ToString()), txtcontactperson.Text.Trim(), txtPhoneNo.Text.Trim(), txtgstno.Text.Trim(),
                txtFax.Text.Trim(), txtAddress1.Text.Trim(), txtAddress2.Text.Trim(), DistributorId, txtDistributorCode.Text.Trim(), txtDistributorName.Text.Trim(), null, cbRegistered.Checked, 1, int.Parse(this.Session["UserId"].ToString()));
            }
            this.LoadGird();                            
            ClearAll();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Clears All The Fields Through ClearAll() Function.
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    /// <summary>
    /// Loads Locations To Location Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadGird();
    }

    /// <summary>
    /// Sets PageIndex Of Location Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GridDistributor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridDistributor.PageIndex = e.NewPageIndex;
        this.LoadGird();
    }

    /// <summary>
    /// Actives/DeActives A Location.
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GridDistributor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DistributorId = int.Parse(GridDistributor.Rows[e.RowIndex].Cells[0].Text);
        CompanyId = int.Parse(GridDistributor.Rows[e.RowIndex].Cells[11].Text);
        bool IsRegister = bool.Parse(GridDistributor.Rows[e.RowIndex].Cells[1].Text);
        mController.UpdateDistributor(CompanyId, Constants.IntNullValue, true, Constants.DateNullValue, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
               , Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, null, null, null, null, DistributorId, null,
               null, null, true, 1, int.Parse(this.Session["UserId"].ToString()));
        this.LoadGird();
    }

    /// <summary>
    /// Sets Location Data For Edit. This Function Runs When An Existing Location Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GridDistributor_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DistributorId = int.Parse(GridDistributor.Rows[e.NewEditIndex].Cells[0].Text);
        DrpCompanyName.SelectedValue = GridDistributor.Rows[e.NewEditIndex].Cells[11].Text;
        cbRegistered.Checked = bool.Parse(GridDistributor.Rows[e.NewEditIndex].Cells[1].Text);
        ddDistributorType.SelectedValue = GridDistributor.Rows[e.NewEditIndex].Cells[2].Text;
        txtAddress2.Text = GridDistributor.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;","");
        txtDistributorCode.Text = GridDistributor.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        txtDistributorName.Text = GridDistributor.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        txtAddress1.Text = GridDistributor.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
        txtcontactperson.Text = GridDistributor.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        txtPhoneNo.Text = GridDistributor.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
        if (GridDistributor.Rows[e.NewEditIndex].Cells[13].Text == "Active")
        {
            chkIsActive.Checked = true;
        }
        else
        {
            chkIsActive.Checked = false;
        }
        txtgstno.Text=GridDistributor.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
        txtFax.Text = GridDistributor.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;","");
        btnSave.Text = "Update";

    }    

    /// <summary>
    /// Clears All The Fields.
    /// </summary>
    private void ClearAll()
    {
        txtDistributorName.Text = "";
        txtDistributorCode.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtcontactperson.Text = "";
        txtPhoneNo.Text = "";
        txtFax.Text = "";
        txtgstno.Text = ""; 
        btnSave.Text = "Save";
        cbRegistered.Checked = false;  
    }
}