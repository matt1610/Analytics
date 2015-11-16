using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Analytics.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Analytics.Controllers
{
    public class HitsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public HitsController()
        {
        }

        public HitsController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/Hits
        [Authorize]
        public IQueryable<Hit> GetHits()
        {
            return db.Hits;
        }

        [Authorize]
        [Route("api/Hits/GetSiteHits")]
        [HttpPost]
        public async Task<IHttpActionResult> GetSiteHits(SiteBindingModel site)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var dbSite = (from s in db.Sites where s.Url == site.Url select s).FirstOrDefault();
            var hits = from h in db.Hits where h.Site == site.Url select h;

            if (dbSite.Owner == user.Id)
            {
                return Ok(hits);
            }

            return Ok("This site isn't yours.");
        }

        // GET: api/Hits/5
        [Authorize]
        [ResponseType(typeof(Hit))]
        public async Task<IHttpActionResult> GetHit(int id)
        {
            Hit hit = await db.Hits.FindAsync(id);
            if (hit == null)
            {
                return NotFound();
            }

            return Ok(hit);
        }

        // PUT: api/Hits/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHit(int id, Hit hit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hit.Id)
            {
                return BadRequest();
            }

            db.Entry(hit).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HitExists(id))
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

        // POST: api/Hits
        [ResponseType(typeof(Hit))]
        public async Task<IHttpActionResult> PostHit(Hit hit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hits.Add(hit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hit.Id }, hit);
        }

        // DELETE: api/Hits/5
        [Authorize]
        [ResponseType(typeof(Hit))]
        public async Task<IHttpActionResult> DeleteHit(int id)
        {
            Hit hit = await db.Hits.FindAsync(id);
            if (hit == null)
            {
                return NotFound();
            }

            db.Hits.Remove(hit);
            await db.SaveChangesAsync();

            return Ok(hit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HitExists(int id)
        {
            return db.Hits.Count(e => e.Id == id) > 0;
        }
    }
}