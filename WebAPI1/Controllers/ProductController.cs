using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ReactAPI1.Models;

using Microsoft.Data.Sqlite;
using System.Data;
using Newtonsoft.Json;
using System;

namespace ReactAPI1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        //public class ProductsController : ApiController
        //{
        //ProductModel[] products = new ProductModel[]
        //{
        //        new ProductModel { ProductId = 1, ProductName = "Tomato Soup", ProductCategory = "Groceries", ProductPrice = 1 },

        //        new ProductModel { ProductId = 2, ProductName = "Yo-yo",     ProductCategory = "Toys", ProductPrice = 3.75M },

        //        new ProductModel { ProductId = 3, ProductName = "Hammer",     ProductCategory = "Hardware", ProductPrice = 16.99M },

        //        new ProductModel { ProductId = 4, ProductName = "Sugar", ProductCategory = "Groceries", ProductPrice = 10 },

        //        new ProductModel { ProductId = 5, ProductName = "Chhota-Bheem", ProductCategory = "Toys", ProductPrice = 15 },

        //        new ProductModel { ProductId = 6, ProductName = "Printer", ProductCategory = "Hardware", ProductPrice = 120 }
        //};



        [HttpGet]
        // GET: api/Products  
        public IList<ProductModel> Get()
        {
            return GetProducts();
        }

        [HttpGet]
        [Route("{id}")]
        public ProductModel Get(int id)
        {
            var product = GetProductById(id);
            //var product = products.Where(x => x.ProductId == id).FirstOrDefault();
            if (product == null)
            {
                return null;
            }
            return product;
        }

        [HttpPost]
        public IList<ProductModel> Post([FromBody] ProductModel product)
        {
            InsertProduct(product);
            //return products.ToList();
            return GetProducts();
            
        }

        [HttpPut]
       
        public IList<ProductModel> Put([FromBody] ProductModel product)
        {
            UpdateProduct(product);
            //return products.ToList();
            return GetProducts();
        }


        public static SqliteConnection CreateConnection()
        {
            SqliteConnection connection = new SqliteConnection(@"Data Source= db\products.db;");

            connection.Open();
            return connection;
        }
        private void InsertProduct(ProductModel product)
        {

            using (SqliteConnection connection = CreateConnection())
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqliteParameter("@productId", product.ProductId));
                command.Parameters.Add(new SqliteParameter("@productName", product.ProductName));
                command.Parameters.Add(new SqliteParameter("@productCategory", product.ProductCategory));
                command.Parameters.Add(new SqliteParameter("@productPrice", product.ProductPrice.ToString()));

                command.CommandText = "insert into items(productid, productname, productcategory, productprice) values(@productId, @productName, @productCategory, @productPrice)";
                command.ExecuteNonQuery();                
            }

        }
        
        private void UpdateProduct(ProductModel product)
        {
            using (SqliteConnection connection = CreateConnection())
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqliteParameter("@productId", product.ProductId));
                command.Parameters.Add(new SqliteParameter("@productName", product.ProductName));
                command.Parameters.Add(new SqliteParameter("@productCategory", product.ProductCategory));
                command.Parameters.Add(new SqliteParameter("@productPrice", product.ProductPrice.ToString()));

                command.CommandText = "Update items SET productName = @productName, productCategory = @productCategory, productPrice = @productPrice where productId= @productId";
                command.ExecuteNonQuery();
            }

        }

        public IList<ProductModel> GetProducts()
        {
            using (SqliteConnection connection = CreateConnection())
            {
                SqliteCommand command = connection.CreateCommand();
                ;
                command.CommandType = System.Data.CommandType.Text;


                command.CommandText = "SELECT * FROM items";
                var data = command.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(data);
                IList<ProductModel> items = result.AsEnumerable().Select(row =>
                new ProductModel
                {
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    ProductName = row.Field<string>("ProductName"),
                    ProductCategory = row.Field<string>("ProductCategory"),
                    ProductPrice = row.Field<string>("ProductPrice")

                }).ToList();

                    //return JsonConvert.SerializeObject(items,Formatting.Indented);
                return items;
            }
        }

        public ProductModel GetProductById(int id)
        {
            using (SqliteConnection connection = CreateConnection())
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqliteParameter("@productId", id));

                command.CommandText = "SELECT * FROM items where productId = @productId";
                var data = command.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(data);
             ProductModel item = new ProductModel
                {
                    ProductId = Convert.ToInt32(result.Rows[0]["ProductId"]),
                    ProductName = result.Rows[0].Field<string>("ProductName"),
                    ProductCategory = result.Rows[0].Field<string>("ProductCategory"),
                    ProductPrice = result.Rows[0].Field<string>("ProductPrice")

                };

                //return JsonConvert.SerializeObject(items,Formatting.Indented);
                return item;
            }
        }
        [Route("{id}")]
        public void Delete(int id)
        {
            using (SqliteConnection connection = CreateConnection())
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqliteParameter("@productId", id));

                command.CommandText = "DELETE FROM items WHERE productId=@productId";
                command.ExecuteNonQuery();


            }
        }
    }
}