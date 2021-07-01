using Git.Data;
using Git.Data.Models;
using Git.Models.Commits;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly GitDbContext data;

        public CommitsController(
            GitDbContext data)
        {
            this.data = data;
        }
        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = this.data
                .Repositories
                .Where(x => x.Id == id)
                .Select(x => new CommitToRepositoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefault();

            if (repository == null)
            {
                return BadRequest();
            }
            return View(repository);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateCommitFormModel model)
        {

            if (!this.data.Repositories.Any(x => x.Id == model.Id))
            {
                return NotFound();
            }
            if (model.Description.Length < 5)
            {
                return Error($"Commit description have be at least {5} characters.");
            }

            var commit = new Commit
            {
                Description = model.Description,
                RepositoryId = model.Id,
                CreatorId = this.User.Id
            };

            this.data.Commits.Add(commit);

            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = this.data
                .Commits
                .Where(x => x.CreatorId == this.User.Id)
                .Select(x => new CommitListingViewModel
                {
                    Id = x.Id,
                    Repository = x.Repository.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToLocalTime().ToString("F")
                }).ToList();

            return View(commits);
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commit = this.data.Commits.Find(id);

            if (commit == null || commit.CreatorId != this.User.Id)
            {
                return BadRequest();
            }

            this.data.Commits.Remove(commit);

            this.data.SaveChanges();

            return Redirect("/Commits/All");
        }
    }
}
