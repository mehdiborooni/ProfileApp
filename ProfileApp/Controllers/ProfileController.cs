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
        public IActionResult Index(string sortBy= "FirstName", string term="", SortOrderType orderType=SortOrderType.Asc)
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
            ViewData["sortby"] = sortBy;
            

            if (orderType==SortOrderType.Dsc)
            {
                model = model.Reverse();
                orderType = SortOrderType.Asc;
            }
            else
            {
                orderType = SortOrderType.Dsc;
            }
            ViewData["orderType"] = orderType;

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
