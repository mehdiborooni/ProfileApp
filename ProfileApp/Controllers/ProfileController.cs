using ProfileApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using ProfileApp.Data;
using Microsoft.EntityFrameworkCore;
using ProfileApp.Common;
using ProfileApp.Models.Enums;

namespace ProfileApp.Controllers
{
    public class ProfileController : Controller
    {
        
        private readonly ProfileDbContext _db;

        public ProfileController(ProfileDbContext db)
        {
            _db = db;
        }

        [Route("/")]
        //[Route("profile/{sortBy?}/{term?}")]
        public IActionResult Index(string sortBy= "", string term="", SortOrderType orderType1=SortOrderType.Asc , SortOrderType orderType2= SortOrderType.Asc)
        {
            IEnumerable<Profile> model;

            if (!string.IsNullOrEmpty(term))
            {
              model = _db.Profiles.OrderBy(m => m.FirstName).Where(b => b.FirstName.Contains(term));
            }
            else
            {
                model = _db.Profiles;
            }
            

            switch (sortBy)
            {
                case "FirstName":
                    {
                       model = model.OrderBy(m => m.FirstName);
                        break;
                    }
                case "LastName":
                    {
                        model = model.OrderBy(m => m.LastName);
                        break;
                    }
                case "Gender":
                    {
                        model = model.OrderBy(m => m.Gender);
                        break;
                    }
                case "Age":
                    {
                        model = model.OrderBy(m => m.Age);
                        break;
                    }
                case "IsActive":
                    {
                        model = model.OrderBy(m => m.IsActive);
                        break;
                    }
                default:
                    {
                        
                        break;
                    }
            }

            ViewData["term"] = term;
            ViewData["sortBy"] = sortBy;



            //if (orderType==SortOrderType.Dsc)
            //{
            //    model = model.Reverse();
            //    orderType = SortOrderType.Asc;
            //}
            //else
            //{
            //    orderType = SortOrderType.Dsc;
            //}


            if (orderType1 == SortOrderType.Dsc)
            {
                orderType1 = SortOrderType.Asc;
                
                model = model.Reverse();
                orderType2 = SortOrderType.Asc;
            }
            else
            {
                orderType1 = SortOrderType.Dsc;
                orderType2 = SortOrderType.Dsc;
            }

            
            ViewData["orderType1"] = orderType1;
            ViewData["orderType2"] = orderType2;

            return View(model);
        }
        [Route("profile/create/{id?}")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public IActionResult Create(Profile model)
        {
            if (ModelState.IsValid)
            {
                model.TimeCreated = DateTime.Now;

                _db.Profiles.Add(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [Route("profile/details/{id?}")]
        public IActionResult Details(int id)
        {
            var model = _db.Profiles.Find(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _db.Profiles.Find(id);
            return View(model);
        }

        [Route("profile/edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(Profile model)
        {
            model.TimeEdited = DateTime.Now;
            
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        

    }
}
