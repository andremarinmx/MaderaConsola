using IBM.Data.DB2.iSeries;
using MaderaConsola.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaderaConsola
{
    class Program
    {
        static iDB2Connection conAS400;

        public class Orders
        {
            public string Depto { get; set; }
            public string OrderNumber { get; set; }
            public int Line { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public int Balance { get; set; }
            public string MO { get; set; }
        }
        static void Main(string[] args)
        {
            //Medidas de las tablas
            int MedidaTabla96 = 96;
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
            int diferenciaTablas = 0;

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
            string motorOjackshaft;

            //Variables de la sumatoria de los crates normales más los crates diferentes
            int barrote120Total = 0;
            int barrote96Total = 0;
            int barrote144Total = 0;
            int tabla96Total = 0;

            //MacPac variables
            MacPacConnect macPac = new MacPacConnect();

            //----------------------------------------CÁLCULO DE LOS CRATES, HIGHT Y WIDHT----------------------------------------------------//
            //Ingresar orden
            //Console.WriteLine("Ingrese la orden a calcular");
            //orden = Console.ReadLine();

            List<string> ListOnlyOrders = new List<string>();
            DataTable orderListOpenSale = macPac.OrderListQry();

            foreach (DataRow row in orderListOpenSale.Rows)
            {
                for (int i = 0; i < orderListOpenSale.Columns.Count; i++)
                {
                    ListOnlyOrders.Add(row[i].ToString());
                }
            }

            foreach (string ordenItem in ListOnlyOrders)
            {
                //Obtener las información de la orden desde MacPac
                DataTable dataOrder = macPac.OpenSalesQry(ordenItem);
                // Crear una lista de objetos Orders
                List<Orders> orderList = new List<Orders>();

                if (dataOrder != null && dataOrder.Rows.Count > 0)
                {
                    // Recorre cada fila del DataTable
                    foreach (DataRow row in dataOrder.Rows)
                    {
                        var Height = row["Height"].ToString().Trim();
                        var Width = row["Width"].ToString().Trim();
                        if (Height == "" || Width == "")
                        {
                            row["Width"] = 5;
                            row["Height"] = 5;
                        }
                        // Crea un nuevo objeto Orders y asigna los valores de las columnas del DataTable a sus propiedades
                        Orders order = new Orders
                        {
                            Depto = row["Depto"].ToString(),
                            OrderNumber = row["Order"].ToString(),
                            Line = Convert.ToInt32(row["Line"]),
                            Width = Convert.ToDouble(row["Width"]),
                            Height = Convert.ToDouble(row["Height"]),
                            Balance = Convert.ToInt32(row["Balance"]),
                            MO = row["Mo"].ToString(),
                        };
                        // Agrega el objeto Orders a la lista
                        orderList.Add(order);
                    }
                }
                else
                {
                    // Orden no existe
                    continue;
                }

                dept = orderList[0].Depto; //Obtener departamento

                foreach (var item in orderList)
                {
                    Console.WriteLine(item.Depto + " " + item.OrderNumber + " " + item.Line + " " + item.Width + " " + item.Height + " " + item.Balance + " ");
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

                //Console.WriteLine("¿Es de acero? SI/NO ");
                acero = "NO";

                //Console.WriteLine("¿Tiene motor o jackshaft? SI/NO");
                motorOjackshaft = "NO";

                //Calcular órdenes grandes
                if (maxHeight > 28 || maxWidth > 28)
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

                    if (motorOjackshaft == "SI")
                    {
                        //Asignar grosor dependiendo del si tiene entre 1-3 piezas
                        if (piezas < 4)
                        {
                            cantidadCratesDiferentes = 0;
                            cantidadCrates = 0;
                            grosorCrateDiferente = 0;
                            grosorCrate = 0;
                            cantidadCratesDiferentes = 1;
                            switch (piezas)
                            {
                                case 1:
                                    grosorCrateDiferente = 12;
                                    break;
                                case 2:
                                    grosorCrateDiferente = 25;
                                    break;
                                case 3:
                                    grosorCrateDiferente = 35;
                                    break;
                            }
                        }
                        else
                        {
                            //Se tiene mas de 4 piezas y son un numero tal de piezas que no sea divisor de 4 entonces se asigna grosor al diferente crate
                            grosorCrate = 45;
                            cantidadCrates = Convert.ToInt16(decimal.Round(Convert.ToDecimal(piezas / 4), 0));
                            cantidadCratesDiferentes = 1;

                            switch (piezas % 4)
                            {
                                case 1:
                                    grosorCrateDiferente = 12;
                                    break;
                                case 2:
                                    grosorCrateDiferente = 25;
                                    break;
                                case 3:
                                    grosorCrateDiferente = 35;
                                    break;
                                default:
                                    cantidadCratesDiferentes = 0;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Asignar grosor dependiendo del si tiene entre 4-7 piezas
                        if (piezas < 8)
                        {
                            grosorCrateDiferente = 0;
                            cantidadCratesDiferentes = 0;
                            cantidadCrates = 0;
                            grosorCrate = 0;

                            switch (piezas)
                            {
                                case 1:
                                    grosorCrateDiferente = (grosorPiezaSola * 1) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 2:
                                    grosorCrateDiferente = (grosorPiezaSola * 2) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 3:
                                    grosorCrateDiferente = (grosorPiezaSola * 3) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 4:
                                    grosorCrateDiferente = (grosorPiezaSola * 4) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 5:
                                    grosorCrateDiferente = (grosorPiezaSola * 5) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 6:
                                    grosorCrateDiferente = (grosorPiezaSola * 6) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                                case 7:
                                    grosorCrateDiferente = (grosorPiezaSola * 7) + 5;
                                    cantidadCratesDiferentes = 1;
                                    break;
                            }
                        }
                        else
                        {
                            //Se tiene mas de 8 piezas y son un numero tal de piezas que no sea divisor de 8 entonces se asigna grosor al diferente crate
                            grosorCrate = 45;
                            cantidadCratesDiferentes = 1;
                            //Se colocan las piezas pegadas ya que no tienen ningun accesorio, maximo caben 8
                            cantidadCrates = Convert.ToInt16(decimal.Round(Convert.ToDecimal(piezas / 8), 0));
                            //Cálculo de las cantidades de los crates de 45 más la cantidad de crates especiales y su grosor
                            switch (piezas % 8)
                            {
                                case 1:
                                    grosorCrateDiferente = grosorPiezaSola + 5;
                                    break;
                                case 2:
                                    grosorCrateDiferente = (grosorPiezaSola * 2) + 5;
                                    break;
                                case 3:
                                    grosorCrateDiferente = (grosorPiezaSola * 3) + 5;
                                    break;
                                case 4:
                                    grosorCrateDiferente = (grosorPiezaSola * 4) + 5;
                                    break;
                                case 5:
                                    grosorCrateDiferente = (grosorPiezaSola * 5) + 5;
                                    break;
                                case 6:
                                    grosorCrateDiferente = (grosorPiezaSola * 6) + 5;
                                    break;
                                case 7:
                                    grosorCrateDiferente = (grosorPiezaSola * 7) + 5;
                                    break;
                                default:
                                    grosorCrateDiferente = 0;
                                    cantidadCratesDiferentes = 0;
                                    break;
                            }
                            // Si se entra a cualquier caso excepto al default, entonces hay que calcular la cantidad de crates diferentes
                            if (piezas % 8 == 0)
                            {
                                cantidadCratesDiferentes = piezas / 8;
                                grosorCrateDiferente = 0;
                                cantidadCratesDiferentes = 0;
                            }
                        }
                    }

                }
                //Calcular órdenes pequeñas
                else
                {
                    altoCrate = 30;
                    anchoCrate = 48;
                    grosorCrate = 48;
                    grosorCrateDiferente = 0;
                    cantidadCratesDiferentes = 0;

                    switch (piezas)
                    {
                        case int n when (n <= 10):
                            cantidadCrates = 1;
                            break;
                        case int n when (n <= 40):
                            cantidadCrates = 2;
                            break;
                        case int n when (n <= 80):
                            cantidadCrates = 3;
                            break;
                        case int n when (n <= 160):
                            cantidadCrates = 4;
                            break;
                        case int n when (n <= 320):
                            cantidadCrates = 5;
                            break;
                        case int n when (n <= 640):
                            cantidadCrates = 6;
                            break;
                        case int n when (n <= 1280):
                            cantidadCrates = 7;
                            break;
                        default:
                            cantidadCrates = 8;
                            break;
                    }
                }

                //----------------------------------------CÁLCULO DE LA MADERA PARA CRATE NORMAL----------------------------------------------------//
                //Calcular las cantidadad de tablas puestas en el ancho del crate. 1,2
                tabla96 = (Convert.ToInt16(decimal.Round(Convert.ToDecimal(anchoCrate / anchoTabla96), 0)) + tablasPorDefecto) / 2;
                //Este valor es para poder calcular la madera del crate espacial ajustando las tablas de la base por su grosor diferente.
                diferenciaTablas = tabla96;
                //Departamentos con órdenes de aluminio
                if (deptList.Contains("J15") || deptList.Contains("J07"))
                {
                    //Barrotes para la base más soportes de lo alto de lo ancho. 3
                    if (anchoCrate <= MedidaBarrote120)
                    {
                        if (anchoCrate <= (MedidaBarrote120 / 3))
                        {
                            barrote120 += 1;
                            barrote96 += 1;
                        }
                        else
                        {
                            if (anchoCrate <= (MedidaBarrote120 / 2))
                            {
                                barrote120 += 3;
                                barrote96 += 2;
                            }
                            else
                            {
                                barrote120 += 5;
                                //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                barrote96 += 3;
                            }
                        }
                    }
                    else
                    {
                        barrote144 = 5;
                        barrote96 += 3;
                    }
                    //Barrotes a los verticales de los lados del ancho. 5
                    if (altoCrate <= (MedidaBarrote120 / 3))
                    {
                        barrote96 += 1;
                        tabla96 += 2;
                    }
                    else
                    {
                        if (altoCrate <= (MedidaBarrote120 / 2))
                        {
                            barrote96 += 3;
                            tabla96 += 3;
                        }
                        else
                        {
                            barrote96 += 6;
                            //Tablas en diagonal. 6
                            tabla96 += 6;
                        }
                    }
                }
                else
                {
                    switch (dept)
                    {
                        //Órdenes de acero
                        case string d when deptList.Contains("J19"):
                            //Barrotes para la base más soportes de lo alto de lo ancho. 3
                            if (anchoCrate <= MedidaBarrote120)
                            {
                                if (anchoCrate <= (MedidaBarrote120 / 3))
                                {
                                    barrote120 += 1;
                                    barrote96 += 1;
                                }
                                else
                                {
                                    if (anchoCrate <= (MedidaBarrote120 / 2))
                                    {
                                        barrote120 += 3;
                                        barrote96 += 2;
                                    }
                                    else
                                    {
                                        barrote120 += 5;
                                        //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                        barrote96 += 3;
                                    }
                                }
                            }
                            else
                            {
                                barrote144 = 5;
                                barrote96 += 3;
                            }
                            //Barrotes a los verticales de los lados del ancho. 5
                            if (altoCrate <= (MedidaBarrote120 / 3))
                            {
                                barrote96 += 1;
                                tabla96 += 4;
                            }
                            else
                            {
                                if (altoCrate <= (MedidaBarrote120 / 2))
                                {
                                    barrote96 += 3;
                                    tabla96 += 6;
                                }
                                else
                                {
                                    barrote96 += 6;
                                    //Tablas en diagonal. 6
                                    tabla96 += 12;
                                }
                            }
                            break;
                        case string d when deptList.Contains("J09"):
                            //Órdenes grandes
                            if (maxHeight > 28 && maxWidth > 28)
                            {
                                //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                if (anchoCrate <= MedidaBarrote120)
                                {
                                    if (anchoCrate <= (MedidaBarrote120 / 3))
                                    {
                                        barrote120 += 1;
                                        barrote96 += 1;
                                    }
                                    else
                                    {
                                        if (anchoCrate <= (MedidaBarrote120 / 2))
                                        {
                                            barrote120 += 3;
                                            barrote96 += 2;
                                        }
                                        else
                                        {
                                            barrote120 += 5;
                                            //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                            barrote96 += 3;
                                        }
                                    }
                                }
                                else
                                {
                                    barrote144 = 5;
                                    barrote96 += 3;
                                }
                                //Barrotes a los verticales de los lados del ancho. 5
                                if (altoCrate <= (MedidaBarrote120 / 3))
                                {
                                    barrote96 += 1;
                                    tabla96 += 2;
                                }
                                else
                                {
                                    if (altoCrate <= (MedidaBarrote120 / 2))
                                    {
                                        barrote96 += 3;
                                        tabla96 += 3;
                                    }
                                    else
                                    {
                                        barrote96 += 6;
                                        //Tablas en diagonal. 6
                                        tabla96 += 6;
                                    }
                                }
                            }
                            if (acero == "SI")
                            {
                                tabla96 += 6;
                                barrote96 += 4;
                                barrote120 += 2;
                            }
                            else //En ordenes pequeñas, solo se construye el la base
                            {
                                //Barrotes para la base más soportes de lo alto de lo ancho. 3
                                if (anchoCrate <= MedidaBarrote120)
                                {
                                    if (anchoCrate <= (MedidaBarrote120 / 3))
                                    {
                                        barrote120 += 1;
                                        barrote96 += 1;
                                    }
                                    else
                                    {
                                        if (anchoCrate <= (MedidaBarrote120 / 2))
                                        {
                                            barrote120 += 2;
                                            barrote96 += 2;
                                        }
                                        else
                                        {
                                            barrote120 += 3;
                                            //Barrotes para lo alto del ancho más soportes para que no se mueva. 4
                                            barrote96 += 2;
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
                            if (maxWidth <= 25)
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
                            if (maxWidth >= 48)
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

                //----------------------------------------------------CALCULO CRATE DIFERENTE----------------------------------------------/
                //Solo es necesario calcular el grosor del crate ya que es lo único que cambia en el algoritmo
                //Calcular las cantidadad de tablas puestas en el ancho del crate. 1
                tabla96Diferente = Convert.ToInt16(decimal.Round(Convert.ToDecimal(grosorCrateDiferente / anchoTabla96) + tablasPorDefecto, 0));
                //Ya con las tablas calculadas, determinar si se va a usar una fracción de la tabla para colocarla en la base o la tabla entera. 2
                switch (grosorCrateDiferente)
                {
                    case double n when (n <= MedidaTabla96 / 10):
                        tabla96Diferente = tabla96Diferente / 10;
                        break;
                    case double n when (n <= MedidaTabla96 / 9):
                        tabla96Diferente = tabla96Diferente / 9;
                        break;
                    case double n when (n <= MedidaTabla96 / 8):
                        tabla96Diferente = tabla96Diferente / 8;
                        break;
                    case double n when (n <= MedidaTabla96 / 7):
                        tabla96Diferente = tabla96Diferente / 7;
                        break;
                    case double n when (n <= MedidaTabla96 / 6):
                        tabla96Diferente = tabla96Diferente / 6;
                        break;
                    case double n when (n <= MedidaTabla96 / 5):
                        tabla96Diferente = tabla96Diferente / 5;
                        break;
                    case double n when (n <= MedidaTabla96 / 4):
                        tabla96Diferente = tabla96Diferente / 4;
                        break;
                    case double n when (n <= MedidaTabla96 / 3):
                        tabla96Diferente = tabla96Diferente / 3;
                        break;
                    case double n when (n <= MedidaTabla96 / 2):
                        tabla96Diferente = tabla96Diferente / 2;
                        break;
                    default:
                        // Se usa la tabla 96 entera
                        break;
                }
                //Estas tablas son las mismas del crate normal excepto por las tablas que por el grosor cambia la cantidad
                barrote96Diferente = barrote96;
                barrote120Diferente = barrote120;
                barrote144Diferente = barrote144;
                tabla96Diferente = tabla96Diferente + (tabla96 - diferenciaTablas);
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
                if (cantidadCratesDiferentes == 0)
                {
                    tabla96Diferente = 0;
                    barrote96Diferente = 0;
                    barrote120Diferente = 0;
                    barrote144Diferente = 0;
                }

                //Suma de toda la madera
                barrote120Total = barrote120 + barrote120Diferente;
                barrote96Total = barrote96 + barrote96Diferente;
                barrote144Total = barrote144 + barrote144Diferente;
                tabla96Total = tabla96 + tabla96Diferente;

                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Cantidad de piezas: " + piezas);
                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Max W: " + maxWidth);
                //Console.WriteLine("Max H: " + maxHeight);
                //Console.WriteLine("Ancho: " + anchoCrate);
                //Console.WriteLine("Alto: " + altoCrate);
                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Grosor: " + grosorCrate);
                //Console.WriteLine("Barrote del 144: " + barrote144);
                //Console.WriteLine("Barrote del 120: " + barrote120);
                //Console.WriteLine("Barrote del 96: " + barrote96);
                //Console.WriteLine("Tabla del 96: " + tabla96);
                //Console.WriteLine("Cantidad de Crates: " + cantidadCrates);
                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Grosor Diferente: " + grosorCrateDiferente);
                //Console.WriteLine("Barrote del 144 diferente: " + barrote144Diferente);
                //Console.WriteLine("Barrote del 120 diferente: " + barrote120Diferente);
                //Console.WriteLine("Barrote del 96 diferente: " + barrote96Diferente);
                //Console.WriteLine("Tabla del 96 diferente: " + tabla96Diferente);
                //Console.WriteLine("Cantidad de Crates Diferentes: " + cantidadCratesDiferentes);
                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Barrote del 144 total: " + barrote144Total);
                //Console.WriteLine("Barrote del 120 total: " + barrote120Total);
                //Console.WriteLine("Barrote del 96 total:  " + barrote96Total);
                //Console.WriteLine("Tabla del 96 total:    " + tabla96Total);
                //Console.WriteLine("---------------------------------------");
                //Console.WriteLine("Tarimas de 48x48: " + tarima48x48);
                //Console.WriteLine("Tarimas de 34x56: " + tarima34x56);
                //Console.WriteLine("Tarimas de 32x32: " + tarima32x32);
                //Console.WriteLine("Tarimas de 40x48: " + tarima40x48);

                using (AndreTestContext db = new AndreTestContext())
                {
                    //---------------------------Guardar en la base de datos la madera utilizada por la orden---------------------------
                    DateTime dateTime = DateTime.UtcNow.Date;
                    WoodInOrder wood = new WoodInOrder();
                    wood.Orden = ordenItem;
                    wood.Table96 = tabla96Total;
                    wood.Bar120 = barrote120Total;
                    wood.Bar144 = barrote144Total;
                    wood.Bar96 = barrote96Total;
                    wood.Crates = cantidadCrates + cantidadCratesDiferentes;
                    wood.Pallet32X32 = tarima32x32;
                    wood.Pallet34X56 = tarima34x56;
                    wood.Pallet40x48 = tarima40x48;
                    wood.Pallet48x48 = tarima48x48;
                    wood.AssignmentDate = dateTime.ToString("dd/MM/yyyy");
                    db.WoodInOrders.Add(wood);
                    db.SaveChanges();

                    //------------------Guardar datos en formato MacPac-----------------------//
                    WoodOrdenLinesMacPac woodMacPac = new WoodOrdenLinesMacPac();
                    if (deptList.Contains("J07") || deptList.Contains("J09") || deptList.Contains("J15") || deptList.Contains("J19"))
                    {
                        //Dividir las la madera calculada entre las lineas de la orden
                        decimal barrote144Lineas = (decimal)barrote144Total / orderList.Count();
                        decimal barrote120Lineas = (decimal)barrote120Total / orderList.Count();
                        decimal barrote96Lineas = (decimal)barrote96Total / orderList.Count();
                        decimal tabla96Lineas = (decimal)tabla96Total / orderList.Count();

                        for (int i = 1; i <= orderList.Count(); i++)
                        {
                            woodMacPac.Line = orderList[i - 1].Line;
                            woodMacPac.MO = orderList[i - 1].MO;
                            woodMacPac.Orden = orderList[0].OrderNumber;
                            //Dividir entre los barrores entre el balance de la misma linea para que MacPac posteriormente haga la multiplicacion
                            woodMacPac.Bar144 = Convert.ToDouble(barrote144Lineas / orderList[i - 1].Balance);
                            woodMacPac.Bar120 = Convert.ToDouble(barrote120Lineas / orderList[i - 1].Balance);
                            woodMacPac.Bar96 = Convert.ToDouble(barrote96Lineas / orderList[i - 1].Balance);
                            woodMacPac.Table96 = Convert.ToDouble(tabla96Lineas / orderList[i - 1].Balance);
                            db.WoodOrdenLinesMacPacs.Add(woodMacPac);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        woodMacPac.Line = orderList[0].Line;
                        woodMacPac.Orden = orderList[0].OrderNumber;
                        woodMacPac.MO = orderList[0].MO;

                        if (tarima32x32 > 0)
                        {
                            woodMacPac.Pallet32X32 = (double)tarima32x32 / (double)orderList[0].Balance;
                        }
                        else if (tarima34x56 > 0)
                        {
                            woodMacPac.Pallet34X56 = (double)tarima34x56 / (double)orderList[0].Balance;
                        }
                        else if (tarima40x48 > 0)
                        {
                            woodMacPac.Pallet40x48 = (double)tarima40x48 / (double)orderList[0].Balance;
                        }
                        else if (tarima48x48 > 0)
                        {
                            woodMacPac.Pallet48x48 = (double)tarima48x48 / (double)orderList[0].Balance;
                        }
                        db.WoodOrdenLinesMacPacs.Add(woodMacPac);
                        db.SaveChanges();
                    }
                }
                tabla96 = 0;
                barrote144 = 0;
                barrote120 = 0;
                barrote96 = 0;
                tarima48x48 = 0;
                tarima40x48 = 0;
                tarima32x32 = 0;
                tarima34x56 = 0;
                barrote120Diferente = 0;
                barrote96Diferente = 0;
                barrote144Diferente = 0;
                tabla96Diferente = 0;
                diferenciaTablas = 0;
                cantidadCrates = 0;
                grosorCrate = 0;
                anchoCrate = 0;
                altoCrate = 0;
                piezas = 0;
                maxHeight = 0;
                maxWidth = 0;
                grosorCrateDiferente = 0;
                barrote120Total = 0;
                barrote96Total = 0;
                barrote144Total = 0;
                tabla96Total = 0;
                cantidadCratesDiferentes = 0;
                heightList.Clear(); ;
                widthList.Clear();
                deptList.Clear();
            }
            conAS400.Close();
        }
    }
}