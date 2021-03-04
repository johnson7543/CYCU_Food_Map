using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;

using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        dbToDoEntities db = new dbToDoEntities();

        // Default action method for the Index
        // GET: Home
        public ActionResult Index( int page = 1)
        {
            // Using the lambda to query DB records and assign to the todos List
            var todos = db.tTodo.OrderByDescending(m => m.fTitle).ToList();

            int pagesize = 8; 
            int pagecurrent = page < 1 ? 1 : page;
            List<Data> list = new List<Data>();

            foreach (var item in todos)
            {
                Data temp = new Data();
                temp.Id = item.fId;
                temp.Title = item.fTitle;
                temp.Address = item.fAddress;
                temp.Type = item.fType;
                temp.Image = item.fImage;

                list.Add(temp);
            }

            var pagedlist = list.ToPagedList(pagecurrent, pagesize);

            // Return todos list to the View
            return View(pagedlist);
        }
        public ActionResult Create()
        {
            return View();
        }

        // Click Submit button in teh View will trigger this HTTP method
        [HttpPost]
        public ActionResult Create(Data temp, HttpPostedFileBase photo)
        {
            // Create a todo object of tht tToDo Model
            tTodo todo = new tTodo();
            todo.fTitle = temp.Title;
            todo.fAddress = temp.Address;
            todo.fType = temp.Type;

            string fileName = "";

            if (photo != null)
            {

                if (photo.ContentLength > 0)
                {
                    fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    photo.SaveAs(path);
                    todo.fImage = fileName;
                }
            }
            // Add corresponding data to database
            db.tTodo.Add(todo);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // Add new action method for the Delete
        public ActionResult Delete(int id)
        {
            // Find out the specific item and then remove it from the database
            var todo = db.tTodo.Where(m => m.fId == id).FirstOrDefault();
            db.tTodo.Remove(todo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Add new action methdo for the Update
        public ActionResult Edit(int id)
        {
            // Query the specific item from database and to be displayed on the View page
            var todo = db.tTodo.Where(m => m.fId == id).FirstOrDefault();
            return View(todo);
        }

        // Click submit button in the View will trigger this HTTP method
        [HttpPost]
        public ActionResult Edit(int fId, Data temp, HttpPostedFileBase photo)
        {
            // Find out the specific item and update the corresponding data to database
            var todo = db.tTodo.Where(m => m.fId == fId).FirstOrDefault();
            todo.fTitle = temp.Title;
            todo.fAddress = temp.Address;
            todo.fType = temp.Type;

            string fileName = "";

            if (photo != null)
            {

                if (photo.ContentLength > 0)
                {
                    fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    photo.SaveAs(path);
                    todo.fImage = fileName;
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Search( String type, int page = 1 )
        {
            var todos = db.tTodo.OrderByDescending(m => m.fType).ToList();

            int pagesize = 6;
            int pagecurrent = page < 1 ? 1 : page;
            List<Data> list = new List<Data>();

            foreach (var item in todos)
            {
                if (item.fType == type)
                {
                    Data temp = new Data();
                    temp.Id = item.fId;
                    temp.Title = item.fTitle;
                    temp.Address = item.fAddress;
                    temp.Type = item.fType;
                    temp.Image = item.fImage;

                    list.Add(temp);

                }

            }

            var pagedlist = list.ToPagedList(pagecurrent, pagesize);
            // Return todos list to the View
            return View(pagedlist);
        }


        public ActionResult RandomFood()
        {
            var todos = db.tTodo.OrderByDescending(m => m.fType).ToList();

            List<String> typeList = new List<String>();

            List<Data> list = new List<Data>();
            int i = 0;
            foreach (var item in todos)
            {
                Data temp = new Data();
                temp.Id = item.fId;
                temp.IdforRandom = i;
                temp.Title = item.fTitle;
                temp.Address = item.fAddress;
                temp.Type = item.fType;
                temp.Image = item.fImage;
                i++;
                list.Add(temp);
            }

            Random rand = new Random();
            int rId = rand.Next(0, i);

            foreach ( var item in list )
            {
                if( item.IdforRandom == rId )
                    return View(item);
            }

            return View();

        }
    }


}