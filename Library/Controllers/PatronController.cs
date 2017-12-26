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

			var patronModels = allPatrons.Select(p => new PatronDetailModel
			{
				Id = p.Id,
				FirstName = p.FirstName,
				LastName = p.LastName,
				Paid = p.Paid,
				ImageUrl = p.ImageUrl,
				LibraryCardId = p.LibraryCard.Id,
				OverdueFees = p.LibraryCard.Fees,
				HomeLibraryBranch = p.HomeLibraryBranch.Name
			}).ToList();

			var model = new PatronIndexModel()
			{
				Patrons = patronModels
			};
			return View(model);
		}

		public IActionResult Detail(int id)
		{
			var patron = _patron.Get(id);

			var model = new PatronDetailModel
			{
				LastName = patron.LastName,
				FirstName = patron.FirstName,
				Address = patron.Address,
				HomeLibraryBranch = patron.HomeLibraryBranch.Name,
				MemberSince = patron.LibraryCard.Created,
				OverdueFees = patron.LibraryCard.Fees,
				LibraryCardId = patron.LibraryCard.Id,
				Telephone = patron.TelephoneNumber,
				AssetsCheckout = _patron.GetCheckouts(id).ToList() ?? new List<Checkout>(),
				CheckoutHistory = _patron.GetCheckoutHistory(id).ToList() ?? new List<CheckoutHistory>(),
				Holds = _patron.GetHolds(id)
			};

			return View(model);
		}
    }
}
