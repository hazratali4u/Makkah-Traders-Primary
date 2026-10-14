using SAMSCommonLayer.Classes;
using SAMSDataAccessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAMSDatabaseLayer.InputClasses
{
   public class spSelecttblPURCHASE_ORDER_DETAIL
    {
        #region Private Members
        private string sp_Name = "spSelecttblPURCHASE_ORDER_DETAIL";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;



        private long m_PURCHASEORDER_ID;
        #endregion


        #region Public Properties

        public long PURCHASEORDER_ID
        {
            set
            {
                m_PURCHASEORDER_ID = value;
            }
            get
            {
                return m_PURCHASEORDER_ID;
            }
        }



        public IDbConnection Connection
        {
            set
            {
                m_connection = value;
            }
            get
            {
                return m_connection;
            }
        }
        public IDbTransaction Transaction
        {
            set
            {
                m_transaction = value;
            }
            get
            {
                return m_transaction;
            }
        }
        #endregion


        #region Constructor
        public spSelecttblPURCHASE_ORDER_DETAIL()
        {
            //m_DISTRIBUTOR_ID = Constants.IntNullValue;
            //m_TYPE_ID = Constants.IntNullValue;
            //m_SOLD_TO = Constants.IntNullValue;
            //m_SOLD_FROM = Constants.IntNullValue;
            //m_USER_ID = Constants.IntNullValue;
            //m_POSTING = Constants.IntNullValue;
            //TOTAL_AMOUNT = Constants.DecimalNullValue;
            //m_DOCUMENT_DATE = Constants.DateNullValue;
            //m_TIME_STAMP = Constants.DateNullValue;
            //m_LAST_UPDATE = Constants.DateNullValue;
            //m_PURCHASE_MASTER_ID = Constants.LongNullValue;
            //m_ORDER_NUMBER = null;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_Name;
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {


            }
        }


        public IDataReader ExecuteReader()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp_Name;
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollection(ref command);
                IDataReader dr = command.ExecuteReader();
                return dr;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }


        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp_Name;
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollection(ref command);
                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {


            }
        }


        public string ExecuteScalar()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp_Name;
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollection(ref command);
                object o;
                o = command.ExecuteScalar();


                return o.ToString();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }


        public void FirstReader(IDataReader dr)
        {
            if (dr.Read())
            {
                //m_DISTRIBUTOR_ID = Convert.ToInt32(dr["DISTRIBUTOR_ID"]);
                //m_TYPE_ID = Convert.ToInt32(dr["TYPE_ID"]);
                //m_SOLD_TO = Convert.ToInt32(dr["SOLD_TO"]);
                //m_SOLD_FROM = Convert.ToInt32(dr["SOLD_FROM"]);
                //m_USER_ID = Convert.ToInt32(dr["USER_ID"]);
                //m_POSTING = Convert.ToInt32(dr["POSTING"]);
                //m_TOTAL_AMOUNT = Convert.ToDecimal(dr["TOTAL_AMOUNT"]);
                //m_DOCUMENT_DATE = Convert.ToDateTime(dr["DOCUMENT_DATE"]);
                //m_TIME_STAMP = Convert.ToDateTime(dr["TIME_STAMP"]);
                //m_LAST_UPDATE = Convert.ToDateTime(dr["LAST_UPDATE"]);
                m_PURCHASEORDER_ID = Convert.ToInt64(dr["PURCHASEORDER_ID"]);
                //m_ORDER_NUMBER = Convert.ToString(dr["ORDER_NUMBER"]);
            }
        }


        public void GetParameterCollection(ref IDbCommand cmd)
        {
            IDataParameterCollection pparams = cmd.Parameters;
            IDataParameter parameter;

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PURCHASEORDER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_PURCHASEORDER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PURCHASEORDER_ID;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}
