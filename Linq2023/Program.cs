using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Linq2023
{
    internal class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();

        static List<Product> products = new List<Product>();
        static void Main(string[] args)
        {

            //DataSource();
            Filtering();
            //Ordering();
            //Grouping();
            //Grouping2();
            //Joining();
            Console.Read();
        }

        static void DataSource()
        {
            //var queryAllCustomers = from cust in context.clientes select cust;
            var queryAllCustomers = context.clientes.Select(cust => cust).ToList();
            foreach ( var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }


        static void Filtering() {
            //var queryLondonCustomers = from cust in context.clientes
            // where cust.Ciudad == "Londres"
            // select cust;

            var queryLondonCustomers = context.clientes.Where(cust => cust.Ciudad == "Londres").Select(cust => cust).ToList();

            foreach (var item in queryLondonCustomers)
            {
                Console.WriteLine(item.Ciudad);
            }
        }


        static void Ordering()
        {
            /*   var queryLondonCustomers3 = from cust in context.clientes
                                          where cust.Ciudad == "London"
                                          orderby cust.NombreCompañia ascending
                                          select cust;
            */

            var queryLondonCustomers3 = context.clientes.Where(cust => cust.Ciudad == "London")
.OrderBy(cust => cust.NombreCompañia)
.Select(cust => cust).ToList();

            foreach (var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        
        static void Grouping()
        {
            var queryCustomerByCity = context.clientes.GroupBy(cust => cust.Ciudad).ToList();




            foreach (var customerGroup in queryCustomerByCity) {
                Console.WriteLine(customerGroup.Key);
                foreach(clientes customer in customerGroup)
                {
                    Console.WriteLine("    {0}", customer.NombreCompañia);
                }
            }; 

        }

        static void Grouping2()
        {
            var custQuery = context.clientes.GroupBy(cust => cust.Ciudad)
                .Where(custGroup => custGroup.Count() > 2)
                .OrderBy(custGroup => custGroup.Key)
                .Select(custGroup => custGroup).ToList();


            foreach (var item in custQuery)
            {
                Console.WriteLine(item.Key);
            };

        }
        static void Joining()
        {
            /*var innerJoinQuery =
                from cust in context.clientes
                join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };
         */
            var innerJoinQuery = context.clientes.Join(context.Pedidos, cust => cust.idCliente, 
                dist => dist.IdCliente, (cust, dist) => new { 
                    CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario 
                }).ToList();            
            
            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.CustomerName);
            };

        }



        private static void introToLink () {
            //int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            //ShowPares(numbers);
            //ShowParesLambda(numbers);

            InsertProducts();



            var productsExpensive = products.Where(x => x.Price > 50).ToList();


            //Tipado débil
            var productsNew = products.Where(x => x.Price < 50 && x.Name.Contains("m"))
                                      .Select(x => new {
                                          Nombre = x.Name,
                                          PrecioIGV = x.Price * 1.18
                                      }).ToList();

            foreach (var item in productsNew)
            {
                Console.Write(item.Nombre);
                Console.Write(item.PrecioIGV);
                Console.WriteLine();
            }
            //Un listado de productos precio <50 y contenga la letra m el nombre

            Console.ReadLine();
        }
        private static void ShowPares(int[] numbers)
        {
            Console.WriteLine("ShowPares");
            var pares = (from c in numbers
                         where c % 2 == 0
                         select c).ToList();


            foreach (var par in pares) { Console.WriteLine(par); }
        }
        private static void ShowParesLambda(int[] numbers)
        {
            Console.WriteLine("ShowParesLambda");
            var pares = numbers.Where(x => x % 2 == 0).ToList();
            foreach (var par in pares) { Console.WriteLine(par); }
        }

        private static void InsertProducts()
        {
            string[] basicNeeds = { "Leche", "Pan", "Arroz", "Huevos", "Azúcar", "Aceite", "Sal", "Harina", "Pasta", "Jabón", "Papel higiénico", "Detergente", "Cepillo de dientes", "Shampoo", "Cebolla", "Zanahoria", "Papa", "Tomate", "Atún", "Pollo" };

            Random random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                int productId = i;
                string name = basicNeeds[random.Next(0, basicNeeds.Length)];
                int price = random.Next(10, 100); // Genera un precio aleatorio entre 10 y 100
                products.Add(new Product { ProductId = productId, Name = name, Price = price });
            }
        }
    }
}
