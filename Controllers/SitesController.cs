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
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Analytics.Controllers
{
    public class SitesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public SitesController()
        {
        }

        public SitesController(ApplicationUserManager userManager,
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



        // GET: api/Sites
        [Authorize]
        public IQueryable<Site> GetSites()
        {
            return db.Sites;
        }

        // GET: api/Sites/5
        [Authorize]
        [ResponseType(typeof(Site))]
        public async Task<IHttpActionResult> GetSite(int id)
        {
            Site site = await db.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            return Ok(site);
        }

        [Authorize]
        [Route("api/Sites/GetUserSites")]
        public async Task<IHttpActionResult> GetUserSites()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var sites = from s in db.Sites where s.Owner == user.Id select s;
            return Ok(sites);
        }

        // PUT: api/Sites/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSite(int id, Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != site.Id)
            {
                return BadRequest();
            }

            db.Entry(site).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
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

        // POST: api/Sites
        [ResponseType(typeof(Site))]
        public async Task<IHttpActionResult> PostSite(Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserManager.FindById(User.Identity.GetUserId());

            site.Owner = user.Id;

            db.Sites.Add(site);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = site.Id }, site);
        }

        // DELETE: api/Sites/5
        [ResponseType(typeof(Site))]
        public async Task<IHttpActionResult> DeleteSite(int id)
        {
            Site site = await db.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            db.Sites.Remove(site);
            await db.SaveChangesAsync();

            return Ok(site);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteExists(int id)
        {
            return db.Sites.Count(e => e.Id == id) > 0;
        }
    }
}