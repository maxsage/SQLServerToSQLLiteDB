using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TheGameNinja.Domain.Concrete;
using TheGameNinja.Domain.Entities;

namespace TheGameNinja.WebApi.Controllers
{
    public class VideogamesController : ApiController
    {
        private EFDBContext db = new EFDBContext();

        // GET: api/Videogames
        public IQueryable<Videogame> GetVideogames()
        {
            return db.Videogames.OrderByDescending(v => v.DatePurchased);
        }

        // GET: api/Videogames/5
        [ResponseType(typeof(Videogame))]
        public IHttpActionResult GetVideogame(int id)
        {
            Videogame videogame = db.Videogames.Find(id);
            if (videogame == null)
            {
                return NotFound();
            }

            return Ok(videogame);
        }

        // PUT: api/Videogames/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVideogame(int id, Videogame videogame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != videogame.Id)
            {
                return BadRequest();
            }

            db.Entry(videogame).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideogameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Videogames
        [ResponseType(typeof(Videogame))]
        public IHttpActionResult PostVideogame(Videogame videogame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Videogames.Add(videogame);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = videogame.Id }, videogame);
        }

        // DELETE: api/Videogames/5
        [ResponseType(typeof(Videogame))]
        public IHttpActionResult DeleteVideogame(int id)
        {
            Videogame videogame = db.Videogames.Find(id);
            if (videogame == null)
            {
                return NotFound();
            }

            db.Videogames.Remove(videogame);
            db.SaveChanges();

            return Ok(videogame);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VideogameExists(int id)
        {
            return db.Videogames.Count(e => e.Id == id) > 0;
        }
    }
}