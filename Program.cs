﻿using MaderaConsola.Properties;
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
            using (AndreTestContext db = new AndreTestContext())
            {
                //Medidas de las tablas
                int MedidaTabla96 = 96;
                int MedidaBarrote96 = 96;
                int MedidaBarrote120 = 120;

                //Inicialización de las madera utilizada
                int tabla96 = 0;
                int barrote144 = 0;
                int barrote120 = 0;
                int barrote96 = 0;

                //Definicion de constantes
                double anchoTabla96 = 6.1;
                int tablasPorDefecto = 5;

                //Seleccionar dept
                string dept;

                //Cantidad de crates
                int cantidadCrates;

                //Variables ancho y largo del palet individual de la orden con ciertas piezas
                double grosorCrate; //Se calcula con la sumatoria del grosor de las piezas puestas en el crate y que no supere 48 pulgadas
                double anchoCrate; //Se encuentra el valor más grande entre el w y h 
                double altoCrate;  //Al obtener el valor más grande, si se encuentra en el w entonces se busca el valor más grande del h, sino viceversa

                //Orden a calcular madera
                string orden;

                //Variable para guardar la sumaroria de todas las piezas de las línea de la orden
                int piezas = 0;

                int cantidadCratesDiferentes = 0;

                //Declaración de listas para encontrar la cantidad más grande
                List<double> widthList = new List<double>();
                List<double> heightList = new List<double>();

                //Máximos de heigth y width
                double maxHeight = 0;
                double maxWidth = 0;

                //Grosor de el crate diferente en caso de que se necesite
                double grosorCrateDiferente = 0;

                //Constante de grosor de una pieza sin motor
                int grosorPiezaSola = 5;


                //Ingresar orden
                Console.WriteLine("Ingrese la orden a calcular");
                orden = Console.ReadLine();
                var ordersList = db.OpenSalesOrders.Where(x => x.Orden == orden).ToList();
                dept = ordersList[0].Depto; //Obtener departamento

                foreach (var item in ordersList)
                {
                    Console.WriteLine(item.Depto + " " + item.Orden + " " + item.Line + " " + item.Width + " " + item.Height + " " + item.SectW + " " + item.SectH + " " + item.TotalSec + " " + item.Balance + " ");
                    //Sumar todas las piezas
                    piezas += Convert.ToInt16(item.Balance);
                    //Guardar en una lista los width de las lineas
                    widthList.Add(Convert.ToDouble(item.Width));
                    //Guardar en una lista los height de las lineas
                    heightList.Add(Convert.ToDouble(item.Height));
                }
                //Encontrar los valores más grandes los cuales van a determinar el anchoCrate de los crates
                maxHeight = heightList.Max();
                maxWidth = widthList.Max();

                //Calcular órdenes grandes
                if (maxHeight > 28 && maxWidth > 28)
                {
                    //Encontrar el valor más grande entre el width y el height para usarlo como anchoCrate y el menor como altoCrate de crate
                    if (maxWidth >= maxHeight)
                    {
                        anchoCrate = maxWidth + 10;
                        altoCrate = maxHeight + 5;
                    }
                    else
                    {
                        anchoCrate = maxHeight + 10;
                        altoCrate = maxWidth + 5;
                    }

                    //¿Tiene motor o tubos?
                    Console.WriteLine("¿Tiene motor o tubos? SI/NO");
                    string motorOtubos = Console.ReadLine();
                    //Si es que tienen motor o tubos, solo caben 4 en un crate por los espacio que se genera
                    if (motorOtubos == "SI")
                    //Determinar la cantidad de crates así como el grosor de los dos tipos distintos que pueden crear para ordenes con motor o tubos
                    {
                        cantidadCrates = Convert.ToInt16(decimal.Round(Convert.ToDecimal(piezas / 4), 0));

                        //Determinar grosor si es mayor de 4 o igual de 4 piezas será 45 el grosor del crate de 45
                        //Y si es menor no se utiliza y se usa crate especial
                        if (piezas >= 4)
                        {
                            grosorCrate = 45;
                        }
                        else
                        {
                            grosorCrate = 0;
                        }

                        //Cálculo de las cantidades de los crates de 45 más la cantidad de crates especiales y su grosor
                        cantidadCratesDiferentes = 1;
                        if ((piezas % 4) == 1)
                        {
                            grosorCrateDiferente = 12;
                        }
                        else
                        {
                            if ((piezas % 4) == 2)
                            {
                                grosorCrateDiferente = 25;
                            }
                            else
                            {
                                if ((piezas % 4) == 3)
                                {
                                    grosorCrateDiferente = 35;
                                }
                                else
                                {
                                    //Cantidad ya calculada y 45 de grosor por default
                                    cantidadCratesDiferentes = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        cantidadCratesDiferentes = 1;
                        //Cálculo de las catidades de los crates de 45 más la cantidad de crates especiales y su grosor
                        if (piezas >= 8)
                        {
                            grosorCrate = 45;
                        }
                        else
                        {
                            grosorCrate = 0;
                        }

                        //Se colocan las piezas pegadas ya que no tienen ningun accesorio, maximo caben 8
                        cantidadCrates = Convert.ToInt16(decimal.Round(Convert.ToDecimal(piezas / 8), 0));

                        //Cálculo de las cantidades de los crates de 45 más la cantidad de crates especiales y su grosor
                        if ((piezas % 8) == 1)
                        {
                            grosorCrateDiferente = grosorPiezaSola + 4;
                        }
                        else
                        {
                            if ((piezas % 8) == 2)
                            {
                                grosorCrateDiferente = (grosorPiezaSola * 2) + 4;
                            }
                            else
                            {
                                if ((piezas % 8) == 3)
                                {
                                    grosorCrateDiferente = (grosorPiezaSola * 3) + 4;
                                }
                                else
                                {
                                    if ((piezas % 8) == 4)
                                    {
                                        grosorCrateDiferente = (grosorPiezaSola * 4) + 4;
                                    }
                                    else
                                    {
                                        if ((piezas % 8) == 5)
                                        {
                                            grosorCrateDiferente = (grosorPiezaSola * 5) + 4;
                                        }
                                        else
                                        {
                                            if ((piezas % 8) == 6)
                                            {
                                                grosorCrateDiferente = (grosorPiezaSola * 6) + 4;
                                            }
                                            else
                                            {
                                                if ((piezas % 8) == 7)
                                                {
                                                    grosorCrateDiferente = (grosorPiezaSola * 7) + 4;
                                                }
                                                else
                                                {
                                                    //Cantidad ya calculada y 45 de grosor por default
                                                    cantidadCratesDiferentes = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Calcular órdenes pequeñas
                else
                {
                    altoCrate = 30;
                    anchoCrate = 40;
                    grosorCrate = 40;

                    if (piezas <= 10)
                    {
                        cantidadCrates = 1;
                    }
                    else
                    {
                        if (piezas <= 40)
                        {
                            cantidadCrates = 2;
                        }
                        else
                        {
                            if (piezas <= 80)
                            {
                                cantidadCrates = 3;
                            }
                            else
                            {
                                if (piezas <= 160)
                                {
                                    cantidadCrates = 4;
                                }
                                else
                                {
                                    if (piezas <= 320)
                                    {
                                        cantidadCrates = 5;
                                    }
                                    else
                                    {
                                        if (piezas <= 640)
                                        {
                                            cantidadCrates = 6;
                                        }
                                        else
                                        {
                                            if (piezas <= 1280)
                                            {
                                                cantidadCrates = 7;
                                            }
                                            else
                                            {
                                                cantidadCrates = 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                Console.WriteLine("¿Es de acero? SI/NO ");
                string acero = Console.ReadLine();

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
                                barrote120 = barrote120 + 3;
                                barrote96 = barrote96 + 2;
                            }
                            else
                            {
                                barrote120 = barrote120 + 5;
                                //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                barrote96 = barrote96 + 3;
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
                                        barrote120 = barrote120 + 3;
                                        barrote96 = barrote96 + 2;
                                    }
                                    else
                                    {
                                        barrote120 = barrote120 + 5;
                                        //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                        barrote96 = barrote96 + 3;
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
                            if (maxHeight > 28 && maxWidth > 28)
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
                                }//En ordenes pequeñas, solo se construye el la base
                                else
                                {
                                    barrote144 = 3;
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
                                if (acero == "NO")
                                {

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
                                        barrote144 = 3;
                                        barrote96 = barrote96 + 3;
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
                                            tabla96 = tabla96 + 16; //Doble diagonal
                                        }
                                    }
                                }
                                else
                                {
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
                                                barrote120 = barrote120 + 5;
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
                            }
                            //
                            break;
                        case "J01":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        case "J02":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        case "J04":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        case "J06":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        case "EZB":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        case "C04":
                            Console.WriteLine("Departamento requiere tarimas");
                            break;
                        default:
                            Console.WriteLine("Departamento no encontrado");
                            break;
                    }
                }

                Console.WriteLine("Max W: " + maxWidth);
                Console.WriteLine("Max H: " + maxHeight);
                Console.WriteLine("Ancho: " + anchoCrate);
                Console.WriteLine("Alto: " + altoCrate);
                Console.WriteLine("Cantidad de Crates: " + cantidadCrates);
                Console.WriteLine("Grosor: " + grosorCrate);
                Console.WriteLine("Cantidad de Crates Diferentes: " + cantidadCratesDiferentes);
                Console.WriteLine("Grosor Diferente: " + grosorCrateDiferente);
                Console.WriteLine("Barrote del 144: " + barrote144 * cantidadCrates);
                Console.WriteLine("Barrote del 120: " + barrote120 * cantidadCrates);
                Console.WriteLine("Barrote del 96: " + barrote96 * cantidadCrates);
                Console.WriteLine("Tabla del 96: " + tabla96 * cantidadCrates);
                Console.ReadKey();
            }
        }
    }
}