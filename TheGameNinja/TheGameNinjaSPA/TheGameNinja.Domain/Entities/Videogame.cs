using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameNinja.Domain.Entities
{
    public class Videogame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        public int MediaTypeId { get; set; }
        public MediaType MediaType { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DatePurchased { get; set; }

        public string Notes { get; set; }
        public int Rating { get; set; }
        public bool? CurrentlyPlaying { get; set; }
        public bool? Completed { get; set; }
    }
}
