using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Book : LibraryAsset
    {
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        public string DeweyIndex { get; set; }

    }
}
