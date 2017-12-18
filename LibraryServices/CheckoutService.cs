﻿using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
	public class CheckoutService : ICheckout
	{
		private LibraryContext _context;

		public CheckoutService(LibraryContext context)
		{
			_context = context;
		}

		public void Add(Checkout newCheckout)
		{
			_context.Add(newCheckout);
			_context.SaveChanges();
		}

		public IEnumerable<Checkout> GetAll()
		{
			return _context.Checkouts;
		}

		public Checkout GetById(int checkoutId)
		{
			return GetAll()
				.FirstOrDefault(checkout => checkout.Id == checkoutId);
		}

		public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
		{
			return _context.CheckoutHistories
				.Include(h => h.LibraryAsset)
				.Include(h => h.LibraryCard)
				.Where(h => h.LibraryAsset.Id == id);
		}

		public IEnumerable<Hold> GetCurrentHolds(int id)
		{
			return _context.Holds
				.Include(h => h.LibraryAsset)
				.Where(h => h.LibraryAsset.Id == id);
		}

		public Checkout GetLatestCheckout(int assetId)
		{
			return _context.Checkouts
				.Where(c => c.LibraryAsset.Id == assetId)
				.OrderByDescending(c => c.Since)
				.FirstOrDefault();
		}

		public void MarkFound(int assetId)
		{
			var now = DateTime.Now;

			UpdateAssetStatus(assetId, "Available");
			RemoveExistingCheckouts(assetId);
			CloseExistingCheckoutHistory(assetId, now);

			_context.SaveChanges();
		}

		private void UpdateAssetStatus(int assetId, string newStatus)
		{
			var item = _context.LibraryAssets
				.FirstOrDefault(a => a.Id == assetId);

			_context.Update(item);

			item.Status = _context.Statuses
				.FirstOrDefault(status => status.Name == newStatus);
		}

		private void RemoveExistingCheckouts(int assetId)
		{
			var checkout = _context.Checkouts
				.FirstOrDefault(co => co.LibraryAsset.Id == assetId);

			if (checkout != null)
			{
				_context.Remove(checkout);
			}
		}

		private void CloseExistingCheckoutHistory(int assetId, DateTime now)
		{
			var history = _context.CheckoutHistories
				.FirstOrDefault(h => h.LibraryAsset.Id == assetId && h.CheckedIn == null);

			if (history != null)
			{
				_context.Update(history);
				history.CheckedIn = now;
			}
		}

		public void MarkLost(int assetId)
		{
			UpdateAssetStatus(assetId, "Lost");

			_context.SaveChanges();
		}

		public void CheckInItem(int assetId, int libraryCardId)
		{
			var now = DateTime.Now;

			var item = _context.LibraryAssets
				.FirstOrDefault(a => a.Id == assetId);

			RemoveExistingCheckouts(assetId);
			CloseExistingCheckoutHistory(assetId, now);

			var currentHolds = _context.Holds
				.Include(h => h.LibraryAsset)
				.Include(h => h.LibraryAsset)
				.Where(h => h.LibraryAsset.Id == assetId);

			if (currentHolds.Any())
			{
				CheckoutToEarliestHold(assetId, currentHolds);
			}

			UpdateAssetStatus(assetId, "Available");

			_context.SaveChanges();
		}

		private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
		{
			var earliestHold = currentHolds
				.OrderBy(holds => holds.HoldPlaced)
				.FirstOrDefault();

			var card = earliestHold.LibraryCard;

			_context.Remove(earliestHold);
			_context.SaveChanges();
			CheckInItem(assetId, card.Id);
		}

		

		public void PlaceHold(int assetId, int libraryCardId)
		{
			var now = DateTime.Now;

			var asset = _context.LibraryAssets
				.Include(a => a.Status)
				.FirstOrDefault(a => a.Id == libraryCardId);

			var card = _context.LibraryCard
				.FirstOrDefault(a => a.Id == libraryCardId);

			_context.Update(asset);

			if (asset.Status.Name == "Available")
			{
				UpdateAssetStatus(assetId, "On Hold");
			}

			var hold = new Hold
			{
				HoldPlaced = now,
				LibraryAsset = asset,
				LibraryCard = card
			};

			_context.Add(hold);
			_context.SaveChanges();

		}

		public string GetCurrentHoldPatronName(int holdId)
		{
			var hold = _context.Holds
				.Include(h => h.LibraryAsset)
				.Include(h => h.LibraryCard)
				.FirstOrDefault(h => h.Id == holdId);

			var cardId = hold?.LibraryCard.Id;

			var patron = _context.Patrons.Include(p => p.LibraryCard)
				.FirstOrDefault(P => P.LibraryCard.Id == cardId);

			return patron?.FirstName + " " + patron?.LastName;

		}

		public DateTime GetCurrentHoldPlaced(int holdId)
		{
			return
				_context.Holds
				.Include(h => h.LibraryAsset)
				.Include(h => h.LibraryCard)
				.FirstOrDefault(h => h.Id == holdId)
				.HoldPlaced;
		}

		public string GetCurrentCheckoutPatron(int assetId)
		{
			var checkout = GetCheckOutByAssetId(assetId);
			if (checkout == null)
			{
				return "";
			};

			var cardId = checkout.LibraryCard.Id;

			var patron = _context.Patrons
				.Include(p => p.LibraryCard)
				.FirstOrDefault(p => p.LibraryCard.Id == cardId);

			return patron.FirstName + " " + patron.LastName;
		}

		private Checkout GetCheckOutByAssetId(int assetId)
		{
			return _context.Checkouts
				.Include(co => co.LibraryAsset)
				.Include(co => co.LibraryCard)
				.FirstOrDefault(co => co.LibraryAsset.Id == assetId);
		}

		public void CheckInItem(int assetId)
		{
			var now = DateTime.Now;

			var item = _context.LibraryAssets
				.FirstOrDefault(a => a.Id == assetId);

			RemoveExistingCheckouts(assetId);

			CloseExistingCheckoutHistory(assetId, now);

			var currentHolds = _context.Holds
				.Include(h => h.LibraryAsset)
				.Include(h => h.LibraryCard)
				.Where(h => h.LibraryAsset.Id == assetId);

			if (currentHolds.Any())
			{
				CheckoutToEarliestHold(assetId, currentHolds);
				return;
			}

			UpdateAssetStatus(assetId, "Available");

			_context.SaveChanges();
		}


		public void CheckoutItem(int assetId, int libraryCardId)
		{
			if (IsCheckedOut(assetId))
			{
				return;
				// Add logic here to handle feedback to the user.
			}

			var item = _context.LibraryAssets
				.Include(a => a.Status)
				.FirstOrDefault(a => a.Id == assetId);

			_context.Update(item);

			item.Status = _context.Statuses
				.FirstOrDefault(a => a.Name == "Checked Out");

			var now = DateTime.Now;

			var libraryCard = _context.LibraryCard
				.Include(c => c.Checkouts)
				.FirstOrDefault(a => a.Id == libraryCardId);

			var checkout = new Checkout
			{
				LibraryAsset = item,
				LibraryCard = libraryCard,
				Since = now,
				Until = GetDefaultCheckoutTime(now)
			};

			_context.Add(checkout);

			var checkoutHistory = new CheckoutHistory
			{
				CheckedOut = now,
				LibraryAsset = item,
				LibraryCard = libraryCard
			};

			_context.Add(checkoutHistory);
			_context.SaveChanges();
		}

		private DateTime GetDefaultCheckoutTime(DateTime now)
		{
			return now.AddDays(30);
		}

		public bool IsCheckedOut(int id)
		{
			var isCheckedOut = _context.Checkouts.Where(a => a.LibraryAsset.Id == id).Any();

			return isCheckedOut;
		}
	}
}
