using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaderaConsola
{
    class UploadWood
    {
        static iDB2Connection conAS400;
        //static iDB2DataReader dataTable;

        public UploadWood()
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
        public void OpenSalesQry(string orderNumber)
        {
            //Construccion del query para obtener el Order, Line, Depto, DEPTO, WIDTH, HEIGHT, BALANCE
            string Qry = "SELECT AADAT12.AADAT12.OPENSOPF3.ACBAC";

        }


    }
}
