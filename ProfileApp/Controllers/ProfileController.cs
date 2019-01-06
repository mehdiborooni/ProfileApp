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

namespace ProfileApp.Controllers
{
    public class ProfileController : Controller
    {
        
        private readonly ProfileDbContext _db;

        public ProfileController(ProfileDbContext db)
        {
            _db = db;
        }

        [Route("profile/{sortBy?}/{term?}")]
        public IActionResult Index(string sortBy= "FirstName", string term="")
        {
            IEnumerable<Profile> model;

            //model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.FirstName) : _db.Profiles.OrderBy(m => m.FirstName).Where(b => b.FirstName.Contains(term));


            switch (sortBy)
            {
                case "FirstName":
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.FirstName) : _db.Profiles.OrderBy(m => m.FirstName).Where(b => b.FirstName.Contains(term));
                        break;
                    }
                case "LastName":
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.LastName) : _db.Profiles.OrderBy(m => m.LastName).Where(b => b.FirstName.Contains(term));
                        break;
                    }
                case "Gender":
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.Gender) : _db.Profiles.OrderBy(m => m.Gender).Where(b => b.FirstName.Contains(term));
                        break;
                    }
                case "Age":
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.Age) : _db.Profiles.OrderBy(m => m.Age).Where(b => b.FirstName.Contains(term));
                        break;
                    }
                case "IsActive":
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles.OrderBy(m => m.IsActive) : _db.Profiles.OrderBy(m => m.IsActive).Where(b => b.FirstName.Contains(term));
                        break;
                    }
                default:
                    {
                        model = string.IsNullOrEmpty(term) ? _db.Profiles : _db.Profiles.Where(b => b.FirstName.Contains(term));
                        break;
                    }
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Profile model)
        {
            model.TimeCreated = DateTime.Now;
           
            _db.Profiles.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

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
