using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Nwuram.Framework.Data;
using Nwuram.Framework.Settings.User;
using Nwuram.Framework.Settings.Connection;

namespace CashReportNew
{
    public class Procedures : SqlProvider
    {
        private ArrayList parameters = new ArrayList();

        public Procedures(string server, string database, string username, string password, string appName)
             : base(server, database, username, password, appName)
        {
        }

        public DataTable GetDepartments()
        {
            parameters.Clear();
            return executeProcedure("CashReport.GetDepartments", new string[] { }, new DbType[] { }, parameters);
        }

        public  DataTable GetCashData(DateTime date_start, DateTime date_end, int id_otdel, int? terminal, long? user_id, int? doc_start, int? doc_end, int? sum_start, int? sum_end, long? count_start, long? count_end)
        {
            parameters.Clear();
            parameters.AddRange(new object[] { date_start, date_end, id_otdel, terminal != null ? (object)terminal : (object)DBNull.Value,
                                                                     user_id != null ? (object)user_id : (object)DBNull.Value,
                                                                     doc_start != null ? (object)doc_start : (object)DBNull.Value,
                                                                     doc_end != null ? (object)doc_end : (object)DBNull.Value,
                                                                     sum_start != null ? (object)sum_start : (object)DBNull.Value,
                                                                     sum_end != null ? (object)sum_end : (object)DBNull.Value,
                                                                     count_start != null ? (object)count_start : (object)DBNull.Value,
                                                                     count_end != null ? (object)count_end : (object)DBNull.Value});
            return executeProcedure("CashReport.GetCashData", new string[] { "@date_start", "@date_end", "@id_otdel", "@terminal", "@user_id", "@doc_start", "@doc_end", "@sum_start", "@sum_end", "@count_start", "@count_end" },
                                                                  new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int32, DbType.Int32, DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 }, parameters);
        }

        public DataTable GetGroups(int id_otdel)
        {
            parameters.Clear();
            parameters.Add(id_otdel);
            return executeProcedure("CashReport.GetGroups", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, parameters);
        }

        public DataTable GetRealizData(DateTime date_start, DateTime date_end, int id_otdel, int id_grp1, string listTerminal)
        {
            parameters.Clear();
            parameters.AddRange(new object[] { date_start, date_end, id_otdel, id_grp1, listTerminal });
            return executeProcedure("CashReport.GetRealizData", new string[] { "@date_start", "@date_end", "@id_otdel", "@id_grp1","@terminal_list" },
                new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int32, DbType.Int32,DbType.String }, parameters);
        }

        public DataTable GetTerminals(DateTime date)
        {
            parameters.Clear();
            //parameters.Add(date);
            return executeProcedure("CashReport.GetTerminals", new string[] { }, new DbType[] { }, parameters);
        }

        public DataTable GetKassirNames(DateTime date)
        {
            parameters.Clear();
            //parameters.Add(date);
            return executeProcedure("CashReport.GetKassirNames", new string[] {  }, new DbType[] { }, parameters);
        }

        public DataTable GetInvGroups(int id_otdel)
        {
            parameters.Clear();
            parameters.Add(id_otdel);
            return executeProcedure("CashReport.GetInvGroups", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, parameters);
        }

        public DataTable GetRealizForCompare(DateTime date_start, DateTime date_end, int id_otdel)
        {
            parameters.Clear();
            parameters.AddRange(new object[] { date_start, date_end, id_otdel });
            return executeProcedure("CashReport.GetRealizForCompare", new string[] { "@date_start", "@date_end", "@id_otdel" }, new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int32 }, parameters);
        }

        public DataTable GetGoods(int id_otdel)
        {
            parameters.Clear();
            parameters.Add(id_otdel);
            return executeProcedure("CashReport.GetGoods", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, parameters);
        }

        public DataTable GetDocReport(DateTime date, int terminal, int doc_id)
        {
            parameters.Clear();
            parameters.AddRange(new object[] { date, terminal, doc_id });
            return executeProcedure("CashReport.GetDocReport", new string[] { "@date", "@terminal", "@doc_id" }, new DbType[] { DbType.DateTime, DbType.Int32, DbType.Int32 }, parameters);
        }

        public DataTable GetScanReport(DateTime date)
        {
            parameters.Clear();
            parameters.Add(date);
            return executeProcedure("CashReport.GetScanReport", new string[] { "@date" }, new DbType[] { DbType.DateTime }, parameters);
        }

        //NEW 10.08.2018
        public string GetTillList()
        {
            string ret = null;
            DataTable t;
            if (((t = executeCommand("exec [cashreport].[SelectTillNumbers]")) == null) || (t.Rows.Count == 0))
                ret = "";
            else
                ret = t.Rows[0][0].ToString().Trim();
            return ret;
        }

        public DataTable SelectWholeSales(DateTime date_from, DateTime date_to, int id_dep)
        {
            parameters.Clear();
            parameters.Add(date_from);
            parameters.Add(date_to);
            parameters.Add(id_dep);
            return executeProcedure("[CashReport].[SelectWholeSales]", new string[] { "@date_from", "@date_to", "@id_dep" }, new DbType[] { DbType.DateTime, DbType.DateTime,DbType.Int32 }, parameters);
        }

        public DataTable getLegalEntities(DateTime date)
        {
            parameters.Clear();
            parameters.Add(date);
            return executeProcedure("[CashReport].[getLegalEntities]", new string[] { "@date" }, new DbType[] { DbType.DateTime }, parameters);
        }
    }
}
