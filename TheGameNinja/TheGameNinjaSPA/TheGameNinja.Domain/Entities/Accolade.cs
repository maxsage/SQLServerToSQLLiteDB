using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace TheGameNinja.Domain.Entities
{
    public class Accolade
    {
        public int Id { get; set; }
        public int VideoGameId { get; set; }

        //public virtual Videogame VideoGame { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public DateTime DateEarned { get; set; }
    }
}
