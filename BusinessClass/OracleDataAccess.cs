using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Data;
using System.Linq;

namespace MobileReimbursement.BusinessClass
{
    public class OracleDataAccess
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string connectionstring, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            try
            {
                using (OracleConnection connObj = new OracleConnection(connectionstring))
                {
                    OracleCommand cmdObj = connObj.CreateCommand();
                    try
                    {
                        cmdObj.CommandText = commandText;
                        cmdObj.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            commandParameters.ToList().ForEach(x => cmdObj.Parameters.Add(x));
                        }
                        connObj.Open();
                        OracleDataAdapter adapterObj = new OracleDataAdapter(cmdObj);
                        adapterObj.Fill(ds);
                        connObj.Close();
                        connObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connObj.Close();
                        connObj.Dispose();
                    }
                }
            }
            catch
            {
            }
            return ds;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string connectionstring, CommandType commandType, string commandText)
        {
            DataSet ds = new DataSet();
            try
            {
                using (OracleConnection connObj = new OracleConnection(connectionstring))
                {
                    OracleCommand cmdObj = connObj.CreateCommand();
                    try
                    {
                        cmdObj.CommandText = commandText;
                        cmdObj.CommandType = commandType;
                        connObj.Open();
                        OracleDataAdapter adapterObj = new OracleDataAdapter(cmdObj);
                        adapterObj.Fill(ds);
                        connObj.Close();
                        connObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connObj.Close();
                        connObj.Dispose();
                    }
                }
            }
            catch
            {
            }
            return ds;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQueryTimeOut(string connectionstring, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            int count = 0;
            try
            {
                using (OracleConnection connObj = new OracleConnection(connectionstring))
                {
                    OracleCommand cmdObj = connObj.CreateCommand();
                    try
                    {
                        cmdObj.CommandText = commandText;
                        cmdObj.CommandType = commandType;
                        for (int i = 0; i < commandParameters.Count(); i++)
                        {
                            cmdObj.Parameters.Add(commandParameters[i]);
                        }
                        connObj.Open();
                        count = cmdObj.ExecuteNonQuery();
                        connObj.Close();
                        connObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connObj.Close();
                        connObj.Dispose();
                    }
                }
            }
            catch
            {
            }
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string connectionstring, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            int count = 0;
            try
            {
                using (OracleConnection connObj = new OracleConnection(connectionstring))
                {
                    OracleCommand cmdObj = connObj.CreateCommand();
                    try
                    {
                        cmdObj.CommandText = commandText;
                        cmdObj.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            for (int i = 0; i < commandParameters.Count(); i++)
                            {
                                cmdObj.Parameters.Add(commandParameters[i]);
                            }
                        }
                        connObj.Open();
                        count = cmdObj.ExecuteNonQuery();
                        connObj.Close();
                        connObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connObj.Close();
                        connObj.Dispose();
                    }
                }
            }
            catch
            {
            }
            return count;
        }

        /// <summary>
        /// Return Multiple Reference Cursor.
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDatasetRefCursor(string connectionstring, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            int count = 0;
            try
            {
                using (OracleConnection connObj = new OracleConnection(connectionstring))
                {
                    OracleCommand cmdObj = connObj.CreateCommand();
                    try
                    {
                        cmdObj.CommandText = commandText;
                        cmdObj.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            for (int i = 0; i < commandParameters.Count(); i++)
                            {
                                cmdObj.Parameters.Add(commandParameters[i]);
                            }
                        }
                        connObj.Open();
                        count = cmdObj.ExecuteNonQuery();
                        if (commandParameters != null)
                        {
                            for (int i = 0; i < commandParameters.Count(); i++)
                            {
                                if (commandParameters[i].OracleDbType.Equals(OracleDbType.RefCursor))
                                {
                                    OracleDataReader dr = ((OracleRefCursor)commandParameters[i].Value).GetDataReader();
                                    DataTable dt = new DataTable();
                                    dt.TableName = commandParameters[i].ParameterName;
                                    dt.Load(dr);
                                    ds.Tables.Add(dt);
                                }
                            }
                        }

                        connObj.Close();
                        connObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connObj.Close();
                        connObj.Dispose();
                    }
                }
            }
            catch
            {
            }
            return ds;
        }
    }
}