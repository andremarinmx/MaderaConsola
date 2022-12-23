using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaderaConsola
{
    class MacPacConnect
    {
        static iDB2Connection conAS400;
        //static iDB2DataReader dataTable;
        static DataTable dt;

        public MacPacConnect()
        {
            string User = "bdcadmin";
            string Pass = "ruskin";

            try
            {
                conAS400 = new iDB2Connection("DataSource = 10.46.144.70;UserID =" + User + "; Password =" + Pass + "; DataCompression = True;");
                conAS400.Open();
            }
            catch
            {
                return;
            }
        }
        public DataTable OpenSalesQry(string orderNumber)
        {
            //Construccion del query para obtener el Order, Line, Depto, DEPTO, WIDTH, HEIGHT, BALANCE
            string Qry = "SELECT SLONO AS ORDER, SLLNNO AS LINE, ACBAC AS DEPTO, WIDTH, HEIGHT, BALANCE FROM AADAT12.OPENSOPF3 WHERE SLONO = '" + orderNumber + "' ";
            try
            {
                iDB2Command command = conAS400.CreateCommand();
                command.CommandText = Qry;
                iDB2DataReader readerIBM = command.ExecuteReader();
                dt = new DataTable();
                dt.Load(readerIBM);
                conAS400.Close();
                return dt;
            }
            catch
            {
                return dt;
            }
        }
    }
}
