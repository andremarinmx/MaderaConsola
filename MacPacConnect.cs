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
            string Qry = "SELECT AADAT12.EC200M.SPONO AS ORDER, AADAT12.EC200M.SPLNNO AS LINE, MIN(AADAT12.EC200M.SLMONO) AS MO, " + 
                "AADAT12.OPENSOPF3.ACBAC AS DEPTO, AADAT12.OPENSOPF3.WIDTH, AADAT12.OPENSOPF3.HEIGHT, AADAT12.OPENSOPF3.BALANCE " + 
                "FROM AADAT12.EC200M INNER JOIN AADAT12.OPENSOPF3 " + 
                "ON AADAT12.EC200M.SPONO = AADAT12.OPENSOPF3.SLONO AND AADAT12.EC200M.SPLNNO = AADAT12.OPENSOPF3.SLLNNO " +
                "WHERE AADAT12.EC200M.SPONO = '" + orderNumber + "' " + 
                "GROUP BY AADAT12.EC200M.SPONO, AADAT12.EC200M.SPLNNO, " + "" +
                "AADAT12.OPENSOPF3.ACBAC, AADAT12.OPENSOPF3.WIDTH, AADAT12.OPENSOPF3.HEIGHT, AADAT12.OPENSOPF3.BALANCE ORDER BY AADAT12.EC200M.SPLNNO";
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
            catch (iDB2Exception ex)
            {
                Console.WriteLine("Se produjo un error: " + ex.Message);
                Console.WriteLine("Detalles del error: " + ex.StackTrace);
                return dt;
            }
        }
    }
}
