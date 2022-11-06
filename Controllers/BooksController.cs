using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BooksController : Controller
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration Configuration)
        {
            this._configuration = Configuration;
        }

        // GET: Books
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                cnn.Open();
                //-----Read-----
                SqlDataAdapter sda = new SqlDataAdapter("BookViewAll", cnn);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.Fill(dt);
            }
            return View(dt);
        }




        // GET: Books/AddOrEdit/5
        public IActionResult AddOrEdit(int? id)
        {

            BooksViewModel booksViewModel = new BooksViewModel();

            if (id > 0)
            {
                booksViewModel = FitchBookById(id);
            }
            //if (id > 0)
            //{
            //    booksViewModel = FetchBookById(id);
            //}

            return View(booksViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookID,BookTitle,BookAutor,BookPrice")] BooksViewModel booksViewModel)
        {
            

            if (ModelState.IsValid)
            {
                DataTable dt = new DataTable();
                using (SqlConnection cnn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    cnn.Open();
                    //-----insert(create)-----

                    SqlCommand cmd = new SqlCommand("BookAddOrEdit", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("BookID", booksViewModel.BookID);
                    cmd.Parameters.AddWithValue("BookTitle", booksViewModel.BookTitle);
                    cmd.Parameters.AddWithValue("BookAutor", booksViewModel.BookAutor);
                    cmd.Parameters.AddWithValue("BookPrice", booksViewModel.BookPrice);
                    cmd.ExecuteNonQuery();

                    
                }

                return RedirectToAction(nameof(Index));
            }
            return View(booksViewModel);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(int? id)
        {
            BooksViewModel booksViewModel = FitchBookById(id);
            

            return View(booksViewModel);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection cnn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                cnn.Open();
                //-----Delete-----

                SqlCommand cmd = new SqlCommand("BookDeleteByID", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("BookID", id);
               
                cmd.ExecuteNonQuery();


            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public BooksViewModel FitchBookById(int? id)
        {
            BooksViewModel booksViewModel = new BooksViewModel();
            using (SqlConnection cnn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dt = new DataTable();
                cnn.Open();

                //------Update-----
                
                SqlDataAdapter sda = new SqlDataAdapter("BookViewByID", cnn);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand.Parameters.AddWithValue("BookID",id);
                sda.Fill(dt);
                if(dt.Rows.Count == 1)
                {
                    booksViewModel.BookID = Convert.ToInt32(dt.Rows[0]["BookID"].ToString()); 
                    booksViewModel.BookTitle = dt.Rows[0]["BookTitle"].ToString();
                    booksViewModel.BookAutor = dt.Rows[0]["BookAutor"].ToString();
                    booksViewModel.BookPrice = Convert.ToInt32(dt.Rows[0]["BookPrice"].ToString());
                }
            }
            return booksViewModel;
        }

        //[NonAction]
        //public BooksViewModel FetchBookById(int? id)
        //{
        //    BooksViewModel booksViewModel = new BooksViewModel();
        //    using (SqlConnection cnn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
        //    {
        //        DataTable dt = new DataTable();
        //        cnn.Open();
        //        //-----Read-----
        //        SqlDataAdapter sda = new SqlDataAdapter("BookViewByID", cnn);
        //        sda.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        sda.SelectCommand.Parameters.AddWithValue("BookID",id);
        //        sda.Fill(dt);
        //        if (dt.Rows.Count == 1)
        //        {
        //            booksViewModel.BookID = Convert.ToInt32(dt.Rows[0]["BookID"].ToString());
        //            booksViewModel.BookTitle = dt.Rows[0]["BookTitle"].ToString();
        //            booksViewModel.BookAutor = dt.Rows[0]["BookAutor"].ToString();
        //            booksViewModel.BookPrice = Convert.ToInt32(dt.Rows[0]["BookPrice"].ToString());

        //        }
        //        return booksViewModel;
        //    }
        //}

        
    }
}
