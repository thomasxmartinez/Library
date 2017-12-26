using LibraryData.Models;
using System;
using System.Collections.Generic;

namespace LibraryData
{
	public interface ICheckout
    {
		void Add(Checkout newCheckout);

		IEnumerable<Checkout> GetAll();
		IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
		IEnumerable<Hold> GetCurrentHolds(int id);

		Checkout GetById(int checkoutId);
		Checkout GetLatestCheckout(int AssetId);
		string GetCurrentCheckoutPatron(int assetId);
		string GetCurrentHoldPatronName(int id);
		DateTime GetCurrentHoldPlaced(int id);
		bool IsCheckedOut(int id);

        void CheckInItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
		void PlaceHold(int assetId, int libraryCardId);
		void MarkLost(int assetId);
        void MarkFound(int assetId);
		void CheckoutItem(int assetId, int libraryCardId);
		string GetCurrentCheckoutPatronImage(int id);
	}
}
