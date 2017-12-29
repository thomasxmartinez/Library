﻿using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryData
{
	public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAll();
        LibraryAsset GetById(int id);
        void Add(LibraryAsset newAsset);
        string GetAuthorOrDirector(int id);
        string GetDeweyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetIsbn(int id);
		string GetDescription(int id);

        LibraryBranch GetCurrentLocation(int id);
    }
}
