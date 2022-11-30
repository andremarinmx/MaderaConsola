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
            int MedidaBarrote96 = 96;
            int MedidaBarrote120 = 120;

            //Inicializacion de las madera utilizada
            int tabla96 = 0;
            int barrote144 = 0;
            int barrote120 = 0;
            int barrote96 = 0;

            //Definicion de constantes
            double anchoTabla96 = 6.1;
            int tablasPorDefecto = 5;


            double anchoOrden = 0; //Ancho de la pieza

            //Seleccionar dept
            string dept;

            //Cantidad de crates
            int cantidadCrates;

            //Variables ancho y largo del palet individual de la orden con ciertas piezas
            double grosorCrate; //Se calcula con la sumatoria del grosor de las piezas puestas en el crate y que no supere 48 pulgadas
            double anchoCrate; //Es el width de la pieza más grande
            double altoCrate;  //Es el heigth de la pieza más grande

            Console.WriteLine("Ingrese dept");
            dept = Console.ReadLine();
            Console.WriteLine("Ingrese grosor del crate");
            grosorCrate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingrese ancho de la crate");
            anchoCrate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingrese alto de la orden");
            altoCrate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingrese la cantidad de crates");
            cantidadCrates = Convert.ToInt16(Console.ReadLine());

            //Departamentos con órdenes de aluminio
            if (dept == "J15" || dept == "J07")
            {
                //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                tabla96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                //División de las tablas para saber si van completas o partidas. 2
                if (grosorCrate <= (MedidaTabla96 / 6))
                {
                    tabla96 = tabla96 / 6;
                }
                else
                {
                    if (grosorCrate <= (MedidaTabla96 / 5))
                    {
                        tabla96 = tabla96 / 5;
                    }
                    else
                    {
                        if (grosorCrate <= (MedidaTabla96 / 4))
                        {
                            tabla96 = tabla96 / 4;
                        }
                        else
                        {
                            if (grosorCrate <= (MedidaTabla96 / 3))
                            {
                                tabla96 = tabla96 / 3;
                            }
                            else
                            {
                                if (grosorCrate <= (MedidaTabla96 / 2))
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
                //Barrotes para la base más soportes de lo alto de lo ancho. 3
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
                            barrote96 = barrote96 + 3;
                        }
                        else
                        {
                            barrote120 = barrote120 + 3;
                            //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                            barrote96 = barrote96 + 5;
                        }
                    }
                }
                else
                {
                    barrote144 = 5;
                    barrote96 = barrote96 + 3;
                }
                //Barrotes a los verticales de los lados del ancho. 5
                if (altoCrate <= (MedidaBarrote120 / 3))
                {
                    barrote96 = barrote96 + 1;
                    tabla96 = tabla96 + 2;
                }
                else
                {
                    if (altoCrate <= (MedidaBarrote120 / 2))
                    {
                        barrote96 = barrote96 + 3;
                        tabla96 = tabla96 + 3;
                    }
                    else
                    {
                        barrote96 = barrote96 + 6;
                        //Tablas en diagonal. 6
                        tabla96 = tabla96 + 6;
                    }
                }
            }
            else
            {
                switch (dept)
                {
                    //Órdenes de acero
                    case "J19":
                        //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                        barrote96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                        //División de las tablas para saber si van completas o partidas. 2
                        if (grosorCrate <= (MedidaBarrote96 / 7))
                        {
                            barrote96 = barrote96 / 7;
                        }
                        else
                        {
                            if (grosorCrate <= (MedidaBarrote96 / 6))
                            {
                                barrote96 = barrote96 / 6;
                            }
                            else
                            {
                                if (grosorCrate <= (MedidaBarrote96 / 5))
                                {
                                    barrote96 = barrote96 / 5;
                                }
                                else
                                {
                                    if (grosorCrate <= (MedidaBarrote96 / 4))
                                    {
                                        barrote96 = barrote96 / 4;
                                    }
                                    else
                                    {
                                        if (grosorCrate <= (MedidaBarrote96 / 3))
                                        {
                                            barrote96 = barrote96 / 3;
                                        }
                                        else
                                        {
                                            if (grosorCrate <= (MedidaBarrote96 / 2))
                                            {
                                                barrote96 = barrote96 / 2;
                                            }
                                            else
                                            {
                                                //Se usa la tabla 96 entera
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Barrotes para la base más soportes de lo alto de lo ancho. 3
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
                                    barrote96 = barrote96 + 3;
                                }
                                else
                                {
                                    barrote120 = barrote120 + 3;
                                    //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                    barrote96 = barrote96 + 5;
                                }
                            }
                        }
                        else
                        {
                            barrote144 = 5;
                            barrote96 = barrote96 + 2;
                        }
                        //Barrotes a los verticales de los lados del ancho. 5
                        if (altoCrate <= (MedidaBarrote120 / 3))
                        {
                            barrote96 = barrote96 + 1;
                            tabla96 = tabla96 + 4;
                        }
                        else
                        {
                            if (altoCrate <= (MedidaBarrote120 / 2))
                            {
                                barrote96 = barrote96 + 3;
                                tabla96 = tabla96 + 6;
                            }
                            else
                            {
                                barrote96 = barrote96 + 6;
                                //Tablas en diagonal. 6
                                tabla96 = tabla96 + 12;
                            }
                        }
                        break;
                    case "J09":
                        //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                        tabla96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                        //Órdenes pequeñas: medir primero las piezas y después ingresar el ancho, largo y alto de la orden ya acomodadas
                        if (altoCrate <= 20 || anchoOrden <= 20)
                        {
                            //División de las tablas para saber si van completas o partidas. 2
                            if (grosorCrate <= (MedidaBarrote96 / 7))
                            {
                                tabla96 = tabla96 / 7;
                            }
                            else
                            {
                                if (grosorCrate <= (MedidaBarrote96 / 6))
                                {
                                    tabla96 = tabla96 / 6;
                                }
                                else
                                {
                                    if (grosorCrate <= (MedidaBarrote96 / 5))
                                    {
                                        tabla96 = tabla96 / 5;
                                    }
                                    else
                                    {
                                        if (grosorCrate <= (MedidaBarrote96 / 4))
                                        {
                                            tabla96 = tabla96 / 4;
                                        }
                                        else
                                        {
                                            if (grosorCrate <= (MedidaBarrote96 / 3))
                                            {
                                                tabla96 = tabla96 / 3;
                                            }
                                            else
                                            {
                                                if (grosorCrate <= (MedidaBarrote96 / 2))
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
                            }
                            //Barrotes para la base más soportes de lo alto de lo ancho. 3
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
                                        barrote96 = barrote96 + 2;
                                    }
                                    else
                                    {
                                        barrote120 = barrote120 + 3;
                                        //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                        barrote96 = barrote96 + 3;
                                    }
                                }
                            }
                            else
                            {
                                barrote144 = 5;
                                barrote96 = barrote96 + 1;
                            }
                        }
                        else
                        {
                            //División de las tablas para saber si van completas o partidas. 2
                            if (grosorCrate <= (MedidaBarrote96 / 7))
                            {
                                tabla96 = tabla96 / 7;
                            }
                            else
                            {
                                if (grosorCrate <= (MedidaBarrote96 / 6))
                                {
                                    tabla96 = tabla96 / 6;
                                }
                                else
                                {
                                    if (grosorCrate <= (MedidaBarrote96 / 5))
                                    {
                                        tabla96 = tabla96 / 5;
                                    }
                                    else
                                    {
                                        if (grosorCrate <= (MedidaBarrote96 / 4))
                                        {
                                            tabla96 = tabla96 / 4;
                                        }
                                        else
                                        {
                                            if (grosorCrate <= (MedidaBarrote96 / 3))
                                            {
                                                tabla96 = tabla96 / 3;
                                            }
                                            else
                                            {
                                                if (grosorCrate <= (MedidaBarrote96 / 2))
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
                            }
                            //Barrotes para la base más soportes de lo alto de lo ancho. 3
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
                                        barrote96 = barrote96 + 2;
                                    }
                                    else
                                    {
                                        barrote120 = barrote120 + 3;
                                        //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                        barrote96 = barrote96 + 3;
                                    }
                                }
                            }
                            else
                            {
                                barrote144 = 5;
                                barrote96 = barrote96 + 1;
                            }
                            //Barrotes a los verticales de los lados del ancho. 5
                            if (altoCrate <= (MedidaBarrote120 / 3))
                            {
                                barrote96 = barrote96 + 1;
                                tabla96 = tabla96 + 4;
                            }
                            else
                            {
                                if (altoCrate <= (MedidaBarrote120 / 2))
                                {
                                    barrote96 = barrote96 + 3;
                                    tabla96 = tabla96 + 6;
                                }
                                else
                                {
                                    barrote96 = barrote96 + 6;
                                    //Tablas en diagonal. 6
                                    tabla96 = tabla96 + 12;
                                }
                            }
                        }
                        //
                        break;
                    case "J01":
                        //
                        break;
                    case "J02":
                        //
                        break;
                    case "J04":
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
            Console.WriteLine("Barrote del 144: " + barrote144 * cantidadCrates);
            Console.WriteLine("Barrote del 120: " + barrote120 * cantidadCrates);
            Console.WriteLine("Barrote del 96: " + barrote96 * cantidadCrates);
            Console.WriteLine("Tabla del 96: " + tabla96 * cantidadCrates);
            Console.ReadKey();
        }
    }
}