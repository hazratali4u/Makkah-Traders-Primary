using SAMSCommonLayer.Classes;
using SAMSDataAccessLayer.Classes;
using SAMSDatabaseLayer.ReportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAMSBusinessLayer.Classes
{
   public class RptPrintSaleInvoiceController
    {
        #region Constructor

        /// <summary>
        /// Constructor for RptSaleController
        /// </summary>
        public RptPrintSaleInvoiceController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Print Sale Invoice Report

        public DataSet SelectPrintInvoice(int p_Distributor_ID, int p_Principal_Id, int p_CustomerID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintSaleInvoice ObjPrint = new UspPrintSaleInvoice();
                SAMSBusinessLayer.Reports.DsReport ds = new SAMSBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.CUSTOMER_ID = p_CustomerID;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_ToDate;

                DataTable dt = ObjPrint.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspPrintSaleInvoice"].ImportRow(dr);
                }

            
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        #endregion
    }
}
