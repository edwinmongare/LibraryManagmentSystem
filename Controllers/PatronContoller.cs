using Library.Models.Patron;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
   
        public class PatronController : Controller
        {
            private IPatron _patron;

            public PatronController(IPatron patron)
            {
                _patron = patron;
            }

            public IActionResult Index()
            {
                var allPatrons = _patron.GetAll();

                var patronModels = allPatrons
                    .Select(p => new PatronDetailModel
                    {
                        Id = p.Id,
                        LastName = p.LastName ,
                        FirstName = p.FirstName ,
                        LibraryCardId = p.LibraryCard?.Id,
                        OverdueFees = p.LibraryCard?.Fees,
                        HomeLibraryBranch = p.HomeLibraryBranch.Name
                    }).ToList();

                var model = new PatronIndexModel
                {
                    Patrons = patronModels
                };

                return View(model);
            }

        public IActionResult Detail(int Id)
        {
            var patron = _patron.Get(Id);

            var model = new PatronDetailModel
            {
                Id = patron.Id,
                LastName = patron.LastName,
                FirstName = patron.FirstName,
                Address = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard?.Created,
                OverdueFees = patron.LibraryCard?.Fees,
                LibraryCardId = patron.LibraryCard?.Id,
                Telephone = patron.TelephoneNumber,
                AssetsCheckedOut = _patron.GetCheckouts(Id).ToList() ?? new List<Checkout>(),
                CheckoutHistory = _patron.GetCheckoutHistory(Id),
                Holds = _patron.GetHolds(Id)
            };

            return View(model);

        }

        }

    }

