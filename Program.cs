using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaderaConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese una orden: ");
            string order = Console.ReadLine();

            void calculoCrates(double width, double height, int cantidad)
            {
                //Medidas de las tablas
                int MedidaTabla96 = 96;
                int MedidaBarrote144 = 144;
                int MedidaBarrote120 = 120;
                int MedidaBarrote96 = 96;
                //Inicializacion de las madera utilizada
                int tabla96 = 0;
                int barrote144 = 0;
                int barrote120 = 0;
                int barrote96 = 0;
                //Definicion de constantes
                double anchoTabla96 = 6.1;
                int tablasPorDefecto = 5;

                //Seleccionar dept
                Console.WriteLine("Ingrese el dept");
                string dept = Console.ReadLine();
                switch (dept)
                {
                    case "J09":
                        //Calcular las cantidadad de tablas puestas en el ancho del crate 
                        tabla96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(width / anchoTabla96), 0)) + tablasPorDefecto;
                        
                        if (height <= (MedidaTabla96 / 6))
                        {
                            tabla96 = tabla96 / 6;
                        }
                        else
                        {
                            if (height <= (MedidaTabla96 / 5))
                            {
                                tabla96 = tabla96 / 5;
                            }
                            else
                            {
                                if (height <= (MedidaTabla96 / 4))
                                {
                                    tabla96 = tabla96 / 4;
                                }
                                else
                                {
                                    if (height <= (MedidaTabla96 / 3))
                                    {
                                        tabla96 = tabla96 / 3;
                                    }
                                    else
                                    {
                                        if (height <= (MedidaTabla96 / 2))
                                        {
                                            tabla96 = tabla96 / 2;
                                        }
                                        else
                                        {
                                            //Se usa la tabla 96 entera

                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "J07":
                        //
                        break;
                    case "J08":
                        //
                        break;
                    case "J06":
                        //
                        break;
                    case "J15":
                        //
                        break;
                    case "J19":
                        //
                        break;
                    default:
                        //
                        break;
                }
                //barrotes solo de la base para el montacargas
                if(width <= MedidaBarrote120)
                {
                    if (width <= (MedidaBarrote120 / 3))
                    {
                        barrote120 = 1;
                    }
                    else
                    {
                        if (width <= (MedidaBarrote120 / 2))
                        {
                            barrote120 = 2;
                        }
                        else
                        {
                            barrote120 = 3;
                        }
                    }
                }
                else
                {
                    barrote144 = 3;
                }

                Console.WriteLine("Barrote del 120: " + barrote120);
                Console.WriteLine("Tabla del 96: " + tabla96);
            }

            //using (MaderaContext db = new MaderaContext())
            //{
            //    var ordersList = db.OpenSales.Where(x => x.Order == order).ToList();
            //    foreach (var item in ordersList)
            //    {
            //        //Console.WriteLine(item.Dept.Trim() + " " + item.Order.Trim() + " " + item.Line + " " + item.Width.Trim() + " " + item.Height.Trim() + " " + item.SectW.Trim() + " " + item.SectH.Trim() + " " + item.TotalSect );
            //    }
            //    OpenSale orderCal = new OpenSale();
            //    List<double> lineaWidth = new List<double>();
            //    List<double> lineaHeight = new List<double>();
            //    foreach (var item in ordersList)
            //    {
            //        //Rellenar las lineas con height en 0
            //        if (Convert.ToDouble(item.Height) == 0)
            //        {
            //            item.Width = item.Width + 1;
            //            item.Height = item.Width;
            //        }
            //        Convert.ToDouble(item.Width);
            //        Convert.ToDouble(item.Height);
            //        Convert.ToDouble(item.SectW);
            //    }
            //}
            calculoCrates(10, 10, 1);
            Console.ReadKey();
        }
    }
}
