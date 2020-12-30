using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ReactAPI1.Models;

using Microsoft.Data.Sqlite;

namespace ReactAPI1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        //public class ProductsController : ApiController
        //{
        ProductModel[] products = new ProductModel[]
        {
                new ProductModel { ProductId = 1, ProductName = "Tomato Soup", ProductCategory = "Groceries", ProductPrice = 1 },

                new ProductModel { ProductId = 2, ProductName = "Yo-yo",     ProductCategory = "Toys", ProductPrice = 3.75M },

                new ProductModel { ProductId = 3, ProductName = "Hammer",     ProductCategory = "Hardware", ProductPrice = 16.99M },

                new ProductModel { ProductId = 4, ProductName = "Sugar", ProductCategory = "Groceries", ProductPrice = 10 },

                new ProductModel { ProductId = 5, ProductName = "Chhota-Bheem", ProductCategory = "Toys", ProductPrice = 15 },

                new ProductModel { ProductId = 6, ProductName = "Printer", ProductCategory = "Hardware", ProductPrice = 120 }
        };



        [HttpGet]
        // GET: api/Products  
        public List<ProductModel> Get()
        {
            return products.ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public IEnumerable<ProductModel> Get(int id)
        {
            var product = products.Where(x => x.ProductId == id).ToList();
            if (product == null)
            {
                return (IEnumerable<ProductModel>)NotFound();
            }
            return product;
        }
        [HttpPost]
        public List<ProductModel> Post([FromBody] ProductModel product)
        {
            InsertProduct(product);
            return products.ToList();
            //  db.SaveChanges();
        }

        [HttpPut]
        [Route("{id}")]
        public List<ProductModel> Put([FromBody] ProductModel product)
        {
            UpdateProduct(product);
            return products.ToList();
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
        [HttpPut]
        [Route("{id}")]
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

                command.CommandText = "Update items SET(productId = @productId, productName = @productName, productCategory = @productCategory, productPrice = @productPrice) where productId= @productId";
                command.ExecuteNonQuery();
            }

        }
 
        //public void Delete(int Id)
        //{
        //    using (SqliteConnection connection = CreateConnection())
        //    {
        //        SqliteCommand command = connection.CreateCommand();
        //        command.CommandType = System.Data.CommandType.Text;
        //        command.Parameters.Add(new SqliteParameter("@productId",Id));

        //        command.CommandText = "DELETE FROM items WHERE productId=@productId";
        //        command.ExecuteNonQuery();
                

        //    }
        //}
    }
}