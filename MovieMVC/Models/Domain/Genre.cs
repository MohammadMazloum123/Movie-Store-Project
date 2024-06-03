using System.ComponentModel.DataAnnotations;

namespace MovieMVC.Models.Domain
{
	public class Genre
	{
		public int Id { get; set; }
		[Required]
        public string? GenreName { get; set; }
    }
}
