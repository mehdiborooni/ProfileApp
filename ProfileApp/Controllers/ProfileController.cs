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
        public IActionResult Index(string sortByAsc= "", string sortByDsc = "", string fName = "", string lName = "", IsActiveType isActiveType=IsActiveType.All , GenderType genderType = GenderType.All)
        {
            IEnumerable<Profile> model;

            if (!string.IsNullOrEmpty(lName) && !string.IsNullOrEmpty(fName) && isActiveType != IsActiveType.All && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.FirstName.Contains(fName) && b.IsActive == isActiveType && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(lName) && !string.IsNullOrEmpty(fName) && isActiveType != IsActiveType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.FirstName.Contains(fName) && b.IsActive == isActiveType);
            }
            else if (!string.IsNullOrEmpty(lName) && !string.IsNullOrEmpty(fName) && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.FirstName.Contains(fName) && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(fName) && isActiveType != IsActiveType.All && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.FirstName.Contains(fName) && b.IsActive == isActiveType && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(lName) && isActiveType != IsActiveType.All && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.IsActive == isActiveType && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(lName) && !string.IsNullOrEmpty(fName))
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.FirstName.Contains(fName));
            }
            else if (!string.IsNullOrEmpty(fName) && isActiveType != IsActiveType.All)
            {
              model = _db.Profiles.Where(b => b.FirstName.Contains(fName) && b.IsActive == isActiveType);
            }
            else if (!string.IsNullOrEmpty(lName) && isActiveType != IsActiveType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.IsActive == isActiveType);
            }
            else if (!string.IsNullOrEmpty(fName) && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.FirstName.Contains(fName) && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(lName) && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName) && b.Gender == genderType);
            }
            else if (isActiveType != IsActiveType.All && genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.IsActive == isActiveType && b.Gender == genderType);
            }
            else if (!string.IsNullOrEmpty(fName))
            {
                model = _db.Profiles.Where(b => b.FirstName.Contains(fName));
            }
            else if (!string.IsNullOrEmpty(lName))
            {
                model = _db.Profiles.Where(b => b.LastName.Contains(lName));
            }
            else if (isActiveType != IsActiveType.All)
            {
                model = _db.Profiles.Where(p => p.IsActive == isActiveType);
            }
            else if (genderType != GenderType.All)
            {
                model = _db.Profiles.Where(b => b.Gender == genderType);
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

            
            
            
            ViewData["sortByAsc"] = sortByAsc;
            ViewData["sortByDsc"] = sortByDsc;
            var vm = new ProfileViewModel {Users = model, FName = fName, LName = lName, IsActiveType = isActiveType , GenderType = genderType};
            return View(vm);
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
