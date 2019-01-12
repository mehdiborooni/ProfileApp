using ProfileApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
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
        public IActionResult Index(string sortByAsc= "", string sortByDsc = "", string fName = "", string lName = "", IsActiveType isActiveType=IsActiveType.All , GenderType genderType = GenderType.All, string age="" , string startAge ="12" , string endAge ="99")
        {
            IEnumerable<Profile> model;

            model = _db.Profiles;

            if (!string.IsNullOrEmpty(fName))
            {
                model = model.Where(b => b.FirstName.Contains(fName));
            }
            if (!string.IsNullOrEmpty(lName))
            {
                model = model.Where(b => b.LastName.Contains(lName));
            }
            if (isActiveType != IsActiveType.All)
            {
                model = model.Where(b => b.IsActive == isActiveType);
            }
            if (genderType != GenderType.All)
            {
                model = model.Where(b => b.Gender == genderType);
            }
            if (!string.IsNullOrEmpty(startAge.ToString()) && !string.IsNullOrEmpty(endAge.ToString()))
            {
                model = model.Where(b => Convert.ToInt32(b.Age) >= Convert.ToInt32(startAge) && Convert.ToInt32(b.Age) <= Convert.ToInt32(endAge));
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

            
            
            
            ViewData["sortByAsc"] = sortByAsc;
            ViewData["sortByDsc"] = sortByDsc;
            var vm = new ProfileViewModel {Users = model, FName = fName, LName = lName, IsActiveType = isActiveType , GenderType = genderType, StartAge = startAge , EndAge = endAge};


            //var list = model.ToList();

            return View(vm);
        }

        public IActionResult Pagination(IQueryable<Profile> model)
        {
            
            var pageCount = (_db.Profiles.Count()) / 10;

            ViewData["pageCount"] = pageCount;

            for (int i = 0; i < pageCount; i++)
            {
               
                    model= _db.Profiles.Skip(i).Take(i + 10);
            }
            return RedirectToAction(nameof(Index));
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
