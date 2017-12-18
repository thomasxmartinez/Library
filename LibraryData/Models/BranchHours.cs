using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class BranchHours
    {
        [Required]
        public int Id { get; set; }
        public LibraryBranch Branch { get; set; }

        [Range (0,6)]
        public string Title { get; set; }

        [Range(0, 23)]
        public int DayOfWeek { get; set; }

        [Range(0, 6)]
        public int OpenTime { get; set; }

        [Range(0, 23)]
        public int CloseTime { get; set; }
    }
}