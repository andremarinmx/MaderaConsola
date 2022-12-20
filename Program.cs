using MaderaConsola.Properties;
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

                //Inicialización de las madera utilizada
                int tarima48x48 = 0;
                int tarima40x48 = 0;
                int tarima32x32 = 0;
                int tarima34x56 = 0;

                //Madera crates diferente
                int barrote120Diferente = 0;
                int barrote96Diferente = 0;
                int barrote144Diferente = 0;
                int tabla96Diferente = 0;

                //Definicion de constantes
                double anchoTabla96 = 6.1;
                int tablasPorDefecto = 5;

                //Seleccionar dept
                string dept;

                //Cantidad de crates
                int cantidadCrates = 0;

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

                //Lista de dept para las órdenes combinadas
                List<string> deptList = new List<string>();

                //Máximos de heigth y width
                double maxHeight = 0;
                double maxWidth = 0;

                //Grosor de el crate diferente en caso de que se necesite
                double grosorCrateDiferente = 0;

                //Constante de grosor de una pieza sin motor
                int grosorPiezaSola = 5;

                //Simulación de queries
                string acero;
                string motorOtubos;

                //Variables de la sumatoria de los crates normales más los crates diferentes
                int barrote120Total = 0;
                int barrote96Total = 0;
                int barrote144Total = 0;
                int tabla96Total = 0;

                //----------------------------------------CÁLCULO DE LOS CRATES, HIGHT Y WIDHT----------------------------------------------------//
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
                    deptList.Add(item.Depto);
                }
                //Encontrar los valores más grandes los cuales van a determinar el anchoCrate de los crates
                maxHeight = heightList.Max();
                maxWidth = widthList.Max();

                Console.WriteLine("¿Es de acero? SI/NO ");
                acero = Console.ReadLine();

                Console.WriteLine("¿Tiene motor o tubos? SI/NO");
                motorOtubos = Console.ReadLine();

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

                //----------------------------------------CÁLCULO DE LA MADERA PARA CRATE NORMAL----------------------------------------------------//
                //Departamentos con órdenes de aluminio
                if (deptList.Contains("J15") || deptList.Contains("J07"))
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
                        case string d when deptList.Contains("J19"):
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
                        case string d when deptList.Contains("J09"):
                            //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                            tabla96 = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;
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
                            //Órdenes grandes
                            if (maxHeight > 28 && maxWidth > 28)
                            {
                                //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                if (acero == "SI")
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
                                            barrote96 = barrote96 + 8;
                                            //Tablas en diagonal. 6
                                            tabla96 = tabla96 + 16; //Doble diagonal
                                        }
                                    }
                                }
                                else //Órdenes que no son de acero
                                {
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
                            }
                            else //En ordenes pequeñas, solo se construye el la base
                            {
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
                                            barrote96 = barrote96 + 2;
                                        }
                                    }
                                }
                            }
                            break;
                        //Depts J01, J02, J03, J04, EZB Y C04 se utilizan los mismos pallets
                        case "J01":
                        case "J02":
                        case "J03":
                        case "J04":
                        case "EZB":
                        case "C04":
                        case "496":
                            if (maxWidth <= 25 )
                            {
                                //En este pallet caben 250 piezas máximo
                                tarima32x32 = piezas / 250 + (piezas % 250 > 0 ? 1 : 0);
                                cantidadCrates = 0;
                                cantidadCratesDiferentes = 0;
                            }
                            else
                            {
                                if (maxWidth <= 35)
                                {
                                    //En este pallet caben 200 piezas máximo
                                    tarima40x48 = piezas / 200 + (piezas % 200 > 0 ? 1 : 0);
                                    cantidadCrates = 0;
                                    cantidadCratesDiferentes = 0;
                                }
                                else
                                {
                                    if (maxWidth <= 45)
                                    {
                                        //En este pallet caben 150 piezas máximo
                                        tarima48x48 = piezas / 150 + (piezas % 150 > 0 ? 1 : 0);
                                        cantidadCrates = 0;
                                        cantidadCratesDiferentes = 0;
                                    }
                                    else
                                    {
                                        if (maxWidth <= 55)
                                        {
                                            //En este pallet caben 100 piezas máximo
                                            tarima34x56 = piezas / 100 + (piezas % 100 > 0 ? 1 : 0);
                                            cantidadCrates = 0;
                                            cantidadCratesDiferentes = 0;
                                        }
                                        else
                                        {
                                            //En este pallet por default
                                            tarima40x48 = piezas / 250 + (piezas % 250 > 0 ? 1 : 0);
                                            cantidadCrates = 0;
                                            cantidadCratesDiferentes = 0;
                                        }
                                    }
                                }
                            }
                            break;
                        case "J06":
                            if(maxWidth >= 48 )
                            {
                                tarima40x48 = piezas / 150 + (piezas % 150 > 0 ? 1 : 0);
                                cantidadCrates = 0;
                                cantidadCratesDiferentes = 0;
                            }
                            else
                            {
                                tarima34x56 = piezas / 150 + (piezas % 150 > 0 ? 1 : 0);
                                cantidadCrates = 0;
                                cantidadCratesDiferentes = 0;
                            }
                            break;
                        default:
                            Console.WriteLine("Departamento no encontrado");
                            cantidadCrates = 0;
                            cantidadCratesDiferentes = 0;
                            break;
                    }
                }

                //----------------------------------------------------CALCULO CRATE DIFERENTE----------------------------------------------//

                if (cantidadCratesDiferentes >= 1)
                {
                    //Departamentos con órdenes de aluminio
                    if (deptList.Contains("J15") || deptList.Contains("J07"))
                    {
                        //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                        tabla96Diferente = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                        //División de las tablas para saber si van completas o partidas. 2
                        if (grosorCrateDiferente <= (MedidaBarrote96 / 8))
                        {
                            tabla96Diferente = tabla96Diferente / 8;
                        }
                        else
                        {
                            if (grosorCrateDiferente <= (MedidaBarrote96 / 7))
                            {
                                tabla96Diferente = tabla96Diferente / 7;
                            }
                            else
                            {
                                if (grosorCrateDiferente <= (MedidaBarrote96 / 6))
                                {
                                    tabla96Diferente = tabla96Diferente / 6;
                                }
                                else
                                {
                                    if (grosorCrateDiferente <= (MedidaBarrote96 / 5))
                                    {
                                        tabla96Diferente = tabla96Diferente / 5;
                                    }
                                    else
                                    {
                                        if (grosorCrateDiferente <= (MedidaBarrote96 / 4))
                                        {
                                            tabla96Diferente = tabla96Diferente / 4;
                                        }
                                        else
                                        {
                                            if (grosorCrateDiferente <= (MedidaBarrote96 / 3))
                                            {
                                                tabla96Diferente = tabla96Diferente / 3;
                                            }
                                            else
                                            {
                                                if (grosorCrateDiferente <= (MedidaBarrote96 / 2))
                                                {
                                                    tabla96Diferente = tabla96Diferente / 2;
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
                        }
                        //Barrotes para la base más soportes de lo alto de lo ancho. 3

                        if (anchoCrate <= MedidaBarrote120)
                        {
                            if (anchoCrate <= (MedidaBarrote120 / 3))
                            {
                                barrote120Diferente = barrote120Diferente + 1;
                                barrote96Diferente = barrote96Diferente + 1;
                            }
                            else
                            {
                                if (anchoCrate <= (MedidaBarrote120 / 2))
                                {
                                    barrote120Diferente = barrote120Diferente + 3;
                                    barrote96Diferente = barrote96Diferente + 2;
                                }
                                else
                                {
                                    barrote120Diferente = barrote120Diferente + 5;
                                    //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                    barrote96Diferente = barrote96Diferente + 3;
                                }
                            }
                        }
                        else
                        {
                            barrote144Diferente = 5;
                            barrote96Diferente = barrote96Diferente + 3;
                        }
                        //Barrotes a los verticales de los lados del ancho. 5
                        if (altoCrate <= (MedidaBarrote120 / 3))
                        {
                            barrote96Diferente = barrote96Diferente + 1;
                            tabla96Diferente = tabla96Diferente + 2;
                        }
                        else
                        {
                            if (altoCrate <= (MedidaBarrote120 / 2))
                            {
                                barrote96Diferente = barrote96Diferente + 3;
                                tabla96Diferente = tabla96Diferente + 3;
                            }
                            else
                            {
                                barrote96Diferente = barrote96Diferente + 6;
                                //Tablas en diagonal. 6
                                tabla96Diferente = tabla96Diferente + 6;
                            }
                        }
                    }
                    else
                    {
                        switch (dept)
                        {
                            //Órdenes de acero
                            case string d when deptList.Contains("J19"):
                                //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                                tabla96Diferente = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;

                                //División de las tablas para saber si van completas o partidas. 2
                                if (grosorCrateDiferente <= (MedidaBarrote96 / 8))
                                {
                                    tabla96Diferente = tabla96Diferente / 8;
                                }
                                else
                                {
                                    if (grosorCrateDiferente <= (MedidaBarrote96 / 7))
                                    {
                                        tabla96Diferente = tabla96Diferente / 7;
                                    }
                                    else
                                    {
                                        if (grosorCrateDiferente <= (MedidaBarrote96 / 6))
                                        {
                                            tabla96Diferente = tabla96Diferente / 6;
                                        }
                                        else
                                        {
                                            if (grosorCrateDiferente <= (MedidaBarrote96 / 5))
                                            {
                                                tabla96Diferente = tabla96Diferente / 5;
                                            }
                                            else
                                            {
                                                if (grosorCrateDiferente <= (MedidaBarrote96 / 4))
                                                {
                                                    tabla96Diferente = tabla96Diferente / 4;
                                                }
                                                else
                                                {
                                                    if (grosorCrateDiferente <= (MedidaBarrote96 / 3))
                                                    {
                                                        tabla96Diferente = tabla96Diferente / 3;
                                                    }
                                                    else
                                                    {
                                                        if (grosorCrateDiferente <= (MedidaBarrote96 / 2))
                                                        {
                                                            tabla96Diferente = tabla96Diferente / 2;
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
                                }
                                //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                if (anchoCrate <= MedidaBarrote120)
                                {
                                    if (anchoCrate <= (MedidaBarrote120 / 3))
                                    {
                                        barrote120Diferente = barrote120Diferente + 1;
                                        barrote96Diferente = barrote96Diferente + 1;
                                    }
                                    else
                                    {
                                        if (anchoCrate <= (MedidaBarrote120 / 2))
                                        {
                                            barrote120Diferente = barrote120Diferente + 3;
                                            barrote96Diferente = barrote96Diferente + 2;
                                        }
                                        else
                                        {
                                            barrote120Diferente = barrote120Diferente + 5;
                                            //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                            barrote96Diferente = barrote96Diferente + 3;
                                        }
                                    }
                                }
                                else
                                {
                                    barrote144Diferente = 5;
                                    barrote96Diferente = barrote96Diferente + 3;
                                }
                                //Barrotes a los verticales de los lados del ancho. 5
                                if (altoCrate <= (MedidaBarrote120 / 3))
                                {
                                    barrote96Diferente = barrote96Diferente + 1;
                                    tabla96Diferente = tabla96Diferente + 4;
                                }
                                else
                                {
                                    if (altoCrate <= (MedidaBarrote120 / 2))
                                    {
                                        barrote96Diferente = barrote96Diferente + 3;
                                        tabla96Diferente = tabla96Diferente + 6;
                                    }
                                    else
                                    {
                                        barrote96Diferente = barrote96Diferente + 6;
                                        //Tablas en diagonal. 6
                                        tabla96Diferente = tabla96Diferente + 12;
                                    }
                                }
                                break;
                            case string d when deptList.Contains("J09"):
                                //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                                tabla96Diferente = Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto;
                                //División de las tablas para saber si van completas o partidas. 2
                                if (grosorCrateDiferente <= (MedidaBarrote96 / 7))
                                {
                                    tabla96Diferente = tabla96Diferente / 7;
                                }
                                else
                                {
                                    if (grosorCrateDiferente <= (MedidaBarrote96 / 6))
                                    {
                                        tabla96Diferente = tabla96Diferente / 6;
                                    }
                                    else
                                    {
                                        if (grosorCrateDiferente <= (MedidaBarrote96 / 5))
                                        {
                                            tabla96Diferente = tabla96Diferente / 5;
                                        }
                                        else
                                        {
                                            if (grosorCrateDiferente <= (MedidaBarrote96 / 4))
                                            {
                                                tabla96Diferente = tabla96Diferente / 4;
                                            }
                                            else
                                            {
                                                if (grosorCrateDiferente <= (MedidaBarrote96 / 3))
                                                {
                                                    tabla96Diferente = tabla96Diferente / 3;
                                                }
                                                else
                                                {
                                                    if (grosorCrateDiferente <= (MedidaBarrote96 / 2))
                                                    {
                                                        tabla96Diferente = tabla96Diferente / 2;
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
                                //Órdenes grandes
                                if (maxHeight > 28 && maxWidth > 28)
                                {
                                    //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                    if (acero == "SI")
                                    {
                                        if (anchoCrate <= MedidaBarrote120)
                                        {
                                            if (anchoCrate <= (MedidaBarrote120 / 3))
                                            {
                                                barrote120Diferente = barrote120Diferente + 1;
                                                barrote96Diferente = barrote96Diferente + 1;
                                            }
                                            else
                                            {
                                                if (anchoCrate <= (MedidaBarrote120 / 2))
                                                {
                                                    barrote120Diferente = barrote120Diferente + 2;
                                                    barrote96Diferente = barrote96Diferente + 2;
                                                }
                                                else
                                                {
                                                    barrote120Diferente = barrote120Diferente + 3;
                                                    //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                                    barrote96Diferente = barrote96Diferente + 3;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            barrote144Diferente = 3;
                                            barrote96Diferente = barrote96Diferente + 3;
                                        }
                                        //Barrotes a los verticales de los lados del ancho. 5
                                        if (altoCrate <= (MedidaBarrote120 / 3))
                                        {
                                            barrote96Diferente = barrote96Diferente + 1;
                                            tabla96Diferente = tabla96Diferente + 4;
                                        }
                                        else
                                        {
                                            if (altoCrate <= (MedidaBarrote120 / 2))
                                            {
                                                barrote96Diferente = barrote96Diferente + 3;
                                                tabla96Diferente = tabla96Diferente + 6;
                                            }
                                            else
                                            {
                                                barrote96Diferente = barrote96Diferente + 8;
                                                //Tablas en diagonal. 6
                                                tabla96Diferente = tabla96Diferente + 16; //Doble diagonal
                                            }
                                        }
                                    }
                                    else //Órdenes que no son de acero
                                    {
                                        //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                        if (anchoCrate <= MedidaBarrote120)
                                        {
                                            if (anchoCrate <= (MedidaBarrote120 / 3))
                                            {
                                                barrote120Diferente = barrote120Diferente + 1;
                                                barrote96Diferente = barrote96Diferente + 1;
                                            }
                                            else
                                            {
                                                if (anchoCrate <= (MedidaBarrote120 / 2))
                                                {
                                                    barrote120Diferente = barrote120Diferente + 3;
                                                    barrote96Diferente = barrote96Diferente + 2;
                                                }
                                                else
                                                {
                                                    barrote120Diferente = barrote120Diferente + 5;
                                                    //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                                    barrote96Diferente = barrote96Diferente + 3;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            barrote144Diferente = 5;
                                            barrote96Diferente = barrote96Diferente + 3;
                                        }
                                        //Barrotes a los verticales de los lados del ancho. 5
                                        if (altoCrate <= (MedidaBarrote120 / 3))
                                        {
                                            barrote96Diferente = barrote96Diferente + 1;
                                            tabla96Diferente = tabla96Diferente + 2;
                                        }
                                        else
                                        {
                                            if (altoCrate <= (MedidaBarrote120 / 2))
                                            {
                                                barrote96Diferente = barrote96Diferente + 3;
                                                tabla96Diferente = tabla96Diferente + 3;
                                            }
                                            else
                                            {
                                                barrote96Diferente = barrote96Diferente + 6;
                                                //Tablas en diagonal. 6
                                                tabla96Diferente = tabla96Diferente + 6;
                                            }
                                        }
                                    }
                                }
                                else //En ordenes pequeñas, solo se construye el la base
                                {
                                    //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                    if (anchoCrate <= MedidaBarrote120)
                                    {
                                        if (anchoCrate <= (MedidaBarrote120 / 3))
                                        {
                                            barrote120Diferente = barrote120Diferente + 1;
                                            barrote96Diferente = barrote96Diferente + 1;
                                        }
                                        else
                                        {
                                            if (anchoCrate <= (MedidaBarrote120 / 2))
                                            {
                                                barrote120Diferente = barrote120Diferente + 2;
                                                barrote96Diferente = barrote96Diferente + 2;
                                            }
                                            else
                                            {
                                                barrote120Diferente = barrote120Diferente + 3;
                                                //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                                barrote96Diferente = barrote96Diferente + 3;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    cantidadCratesDiferentes = 0;
                }

                //Madera utilizada multiplicada por la cantidad de crates normales
                if (cantidadCrates == 0)
                {
                    tabla96 = 0;
                    barrote96 = 0;
                    barrote120 = 0;
                    barrote144 = 0;
                }
                else
                {
                    tabla96 *= cantidadCrates;
                    barrote96 *= cantidadCrates;
                    barrote120 *= cantidadCrates;
                    barrote144 *= cantidadCrates;
                }

                //Suma de toda la madera
                barrote120Total = barrote120 + barrote120Diferente;
                barrote96Total = barrote96 + barrote96Diferente;
                barrote144Total = barrote144 + barrote144Diferente;
                tabla96Total = tabla96 + tabla96Diferente;

                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Max W: " + maxWidth);
                Console.WriteLine("Max H: " + maxHeight);
                Console.WriteLine("Ancho: " + anchoCrate);
                Console.WriteLine("Alto: " + altoCrate);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Grosor: " + grosorCrate);
                Console.WriteLine("Barrote del 144: " + barrote144);
                Console.WriteLine("Barrote del 120: " + barrote120);
                Console.WriteLine("Barrote del 96: " + barrote96);
                Console.WriteLine("Tabla del 96: " + tabla96);
                Console.WriteLine("Cantidad de Crates: " + cantidadCrates);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Grosor Diferente: " + grosorCrateDiferente);
                Console.WriteLine("Barrote del 144 diferente: " + barrote144Diferente);
                Console.WriteLine("Barrote del 120 diferente: " + barrote120Diferente);
                Console.WriteLine("Barrote del 96 diferente: " + barrote96Diferente);
                Console.WriteLine("Tabla del 96 diferente: " + tabla96Diferente);
                Console.WriteLine("Cantidad de Crates Diferentes: " + cantidadCratesDiferentes);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Barrote del 144 total: " + barrote144Total);
                Console.WriteLine("Barrote del 120 total: " + barrote120Total);
                Console.WriteLine("Barrote del 96 total:  " + barrote96Total);
                Console.WriteLine("Tabla del 96 total:    " + tabla96Total);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Tarimas de 48x48: " + tarima48x48);
                Console.WriteLine("Tarimas de 34x56: " + tarima34x56);
                Console.WriteLine("Tarimas de 32x32: " + tarima32x32);
                Console.WriteLine("Tarimas de 40x48: " + tarima40x48);

                Console.ReadKey();
            }
        }
    }
}