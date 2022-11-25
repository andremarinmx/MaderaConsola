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
            //Medidas de las tablas
            int MedidaTabla96 = 96;
            int MedidaBarrote120 = 120;

            //Inicializacion de las madera utilizada
            int tabla96 = 0;
            int barrote144 = 0;
            int barrote120 = 0;
            int barrote96 = 0;

            //Definicion de constantes
            double anchoTabla96 = 6.1;
            int tablasPorDefecto = 5;

            //Medidas de las piezas más grandes
            double anchoOrden = 0;
            double altoOrden = 0;
            double grosorOrden = 0;

            //Variables entrantes
            double width;
            double height;
            int cantidad;

            //Seleccionar dept
            string dept;

            //Variables ancho y largo del palet individual de la orden con ciertas piezas
            double anchoCrate;
            double largoCrate;

            Console.WriteLine("Ingrese dept");
            dept = Console.ReadLine();
            Console.WriteLine("Ingrese ancho del crate");
            anchoCrate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingrese largo de la crate");
            largoCrate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingrese alto de la orden");
            altoOrden = Convert.ToDouble(Console.ReadLine());

            //Departamentos con órdenes de aluminio
            if(dept == "J15" || dept == "J07")
            {
                //Calcular las cantidadad de tablas puestas en el ancho del crate 
                tabla96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                if (largoCrate <= (MedidaTabla96 / 6))
                {
                    tabla96 = tabla96 / 6;
                }
                else
                {
                    if (largoCrate <= (MedidaTabla96 / 5))
                    {
                        tabla96 = tabla96 / 5;
                    }
                    else
                    {
                        if (largoCrate <= (MedidaTabla96 / 4))
                        {
                            tabla96 = tabla96 / 4;
                        }
                        else
                        {
                            if (largoCrate <= (MedidaTabla96 / 3))
                            {
                                tabla96 = tabla96 / 3;
                            }
                            else
                            {
                                if (largoCrate <= (MedidaTabla96 / 2))
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
                //barrotes de la base para el montacargas más soportes de lo alto de lo ancho
                if (anchoCrate <= MedidaBarrote120)
                {
                    if (anchoCrate <= (MedidaBarrote120 / 3))
                    {
                        barrote120 = barrote120 + 1;
                        barrote96 = barrote96 + 1;
                    }
                    else
                    {
                        if (anchoCrate <= (MedidaBarrote120 / 2))
                        {
                            barrote120 = barrote120 + 2;
                            barrote96 = barrote96 + 1;
                        }
                        else
                        {
                            barrote120 = barrote120 + 3;
                            barrote96 = barrote96 + 2;
                        }
                    }
                }
                else
                {
                    barrote144 = 5;
                }
                //barrotes a los lados del ancho
                if (altoOrden <= (MedidaBarrote120 / 3))
                {
                    barrote96 = barrote96 + 1;
                    tabla96 = tabla96 + 2;
                }
                else
                {
                    if (altoOrden <= (MedidaBarrote120 / 2))
                    {
                        barrote96 = barrote96 + 3;
                        tabla96 = tabla96 + 3;
                    }
                    else
                    {
                        barrote96 = barrote96 + 6;
                        tabla96 = tabla96 + 6;
                    }
                }
            }
            else
            {
                switch (dept)
                {
                    case "J19":
                        //
                        break;
                    case "J09":
                        //
                        break;
                    case "J06":
                        //
                        break;
                    case "EZB":
                        //
                        break;
                    case "C04":
                        //
                        break;
                    default:
                        Console.WriteLine("Departamento no encontrado");
                        break;
                }
            }
            Console.WriteLine("Barrote del 144: " + barrote144);
            Console.WriteLine("Barrote del 120: " + barrote120);
            Console.WriteLine("Barrote del 96: " + barrote96);
            Console.WriteLine("Tabla del 96: " + tabla96);
            Console.ReadKey();
        }
    }
}