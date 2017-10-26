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
    public class MediaTypesController : ApiController
    {
        private EFDBContext db = new EFDBContext();

        // GET: api/MediaTypes
        public IQueryable<MediaType> GetMediaTypes()
        {
            return db.MediaTypes;
        }

        // GET: api/MediaTypes/5
        [ResponseType(typeof(MediaType))]
        public IHttpActionResult GetMediaType(int id)
        {
            MediaType mediaType = db.MediaTypes.Find(id);
            if (mediaType == null)
            {
                return NotFound();
            }

            return Ok(mediaType);
        }

        // PUT: api/MediaTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMediaType(int id, MediaType mediaType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mediaType.Id)
            {
                return BadRequest();
            }

            db.Entry(mediaType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaTypeExists(id))
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

        // POST: api/MediaTypes
        [ResponseType(typeof(MediaType))]
        public IHttpActionResult PostMediaType(MediaType mediaType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MediaTypes.Add(mediaType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mediaType.Id }, mediaType);
        }

        // DELETE: api/MediaTypes/5
        [ResponseType(typeof(MediaType))]
        public IHttpActionResult DeleteMediaType(int id)
        {
            MediaType mediaType = db.MediaTypes.Find(id);
            if (mediaType == null)
            {
                return NotFound();
            }

            db.MediaTypes.Remove(mediaType);
            db.SaveChanges();

            return Ok(mediaType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MediaTypeExists(int id)
        {
            return db.MediaTypes.Count(e => e.Id == id) > 0;
        }
    }
}