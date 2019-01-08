using ProfileApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProfileApp.Data;
using Microsoft.EntityFrameworkCore;
using ProfileApp.Common;
using ProfileApp.Filters;
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
        public IActionResult Index(string sortByAsc= "", string sortByDsc = "", string term = "", string term1 = "", string term2 = "")
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

            if (!string.IsNullOrEmpty(term1))
            {
                model = _db.Profiles.OrderBy(m => m.LastName).Where(b => b.LastName.Contains(term1));
            }
            else
            {
                model = _db.Profiles;
            }


            switch (sortByAsc)
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

            switch (sortByDsc)
            {
                case "FirstName":
                {
                    model = model.OrderBy(m => m.FirstName).Reverse();
                    break;
                }
                case "LastName":
                {
                    model = model.OrderBy(m => m.LastName).Reverse();
                    break;
                }
                case "Gender":
                {
                    model = model.OrderBy(m => m.Gender).Reverse();
                    break;
                }
                case "Age":
                {
                    model = model.OrderBy(m => m.Age).Reverse();
                    break;
                }
                case "IsActive":
                {
                    model = model.OrderBy(m => m.IsActive).Reverse();
                    break;
                }
                default:
                {

                    break;
                }
            }

            ViewData["term"] = term;
            ViewData["term1"] = term1;
            ViewData["term2"] = term2;
            ViewData["sortByAsc"] = sortByAsc;
            ViewData["sortByDsc"] = sortByDsc;

            return View(model);
        }
        [Route("profile/create/{id?}")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ValidateModel]
        [HttpPost]
        public IActionResult Create(Profile model)
        {
            
                model.TimeCreated = DateTime.Now;

                _db.Profiles.Add(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
           
           
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
