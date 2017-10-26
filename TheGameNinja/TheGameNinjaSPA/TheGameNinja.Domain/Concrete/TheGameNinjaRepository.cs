using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGameNinja.Domain.Entities;

namespace TheGameNinja.Domain.Concrete
{
        public class TheGameNinjaRepository
        {
            private EFDBContext context = new EFDBContext();

            #region Videogame

            public void SaveVideogame(Videogame videogame)
            {
                if (videogame.Id == 0)
                {
                    context.Videogames.Add(videogame);
                }
                else
                {
                    context.Videogames.Attach(videogame);
                    context.Entry(videogame).State = EntityState.Modified;
                }
                context.SaveChanges();
            }

            public void DeleteVideogame(Videogame videogame)
            {
                context.Videogames.Attach(videogame);
                context.Videogames.Remove(videogame);
                context.SaveChanges();
            }

            //public IQueryable<Videogame> GetAllVideogames()
            //{

            //    IEnumerable<Videogame> videogames = context.Videogames.OrderByDescending(v => v.DatePurchased);
            //    return videogames;
            //}
            #endregion Videogame

            #region Accolade
            public IQueryable<Accolade> Accolade
            {
                get { return context.Accolades; }
            }
            #endregion Accolade

            #region Platform
            public IEnumerable<Platform> GetPlatforms()
            {
                IEnumerable<Platform> platforms =
                    from p in context.Platforms
                    select p;

                return platforms;
            }
            #endregion Platform

        }
}