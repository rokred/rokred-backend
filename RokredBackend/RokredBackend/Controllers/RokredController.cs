using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RokredBackend.DataAccess;
using RokredBackend.Models;

namespace RokredBackend.Controllers
{
    [ApiController]
    public class RokredController : ControllerBase
    {
        [Route("api/rokred/getallopinions")]
        [HttpGet]
        public ActionResult<IEnumerable<Opinion>> GetAllOpinions()
        {
            using (var db = new DataStorage())
            {
                return db.Opinions.ToList();
            }
        }
        
        [Route("api/rokred/getallcategories")]
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAllCategories()
        {
            using (var db = new DataStorage())
            {
                return db.Categories.ToList();
            }
        }

        [Route("api/rokred/getallsubjects")]
        [HttpGet]
        public ActionResult<IEnumerable<Subject>> GetAllSubjects()
        {
            using (var db = new DataStorage())
            {
                return db.Subjects.ToList();
            }
        }

        [Route("api/rokred/getopinionthread")]
        [HttpGet]
        public ActionResult<IEnumerable<Opinion>> GetOpinionThread([FromBody] string id)
        {
            using (var db = new DataStorage())
            {
                var startOpinion = db.Opinions.FirstOrDefault(o => o.Guid == id);

                if (startOpinion != null)
                {
                    var opinionThread = new List<Opinion> { startOpinion };
                    opinionThread.AddRange(db.Opinions
                        .Where(o => o.OpinionThreadKey == startOpinion.Guid)
                        .OrderBy(i => i.Position)
                        .ToList());

                    return opinionThread;
                }
                return new List<Opinion>();
            }
        }

        [Route("api/rokred/getopinion")]
        [HttpGet]
        public ActionResult<Opinion> GetOpinion([FromBody] string id)
        {
            using (var db = new DataStorage())
            {
                var opinion = db.Opinions.FirstOrDefault(o => o.Guid == id);

                if (opinion != null)
                {
                    return opinion;
                }
                return new Opinion();
            }
        }


        // POST api/values
        [Route("api/rokred/postopinion")]
        [HttpPost]
        public void Post([FromBody] Opinion opinion)
        {
            using (var db = new DataStorage())
            {
                opinion.Guid = Guid.NewGuid().ToString();

                if (opinion.OpinionThreadKey != null)
                {
                    var opinions = db.Opinions
                        .Where(o => o.OpinionThreadKey == opinion.OpinionThreadKey);

                    if (opinions.Any())
                    {
                        var latest = opinions.OrderBy(l => l.Position).Last();
                        opinion.Position = latest.Position + 1;
                    }
                    else
                    {
                        opinion.Position = 1;
                    }
                }

                db.Opinions.Add(opinion);

                db.SaveChanges();
            }
        }

        // POST api/values
        [Route("api/rokred/postsubject")]
        [HttpPost]
        public string Post([FromBody] Subject subject)
        {
            using (var db = new DataStorage())
            {
                var dbSubject = db.Subjects
                        .FirstOrDefault(o => o.MySubject == subject.MySubject);

                if (dbSubject != null)
                {
                    return dbSubject.Guid;
                }

                subject.Guid = Guid.NewGuid().ToString();

                db.Subjects.Add(subject);

                db.SaveChanges();

                return subject.Guid;
            }
        }

        [Route("api/rokred/init")]
        [HttpGet]
        public void InitDatabase()
        {
            using (var db = new DataStorage())
            {
                foreach (var currentSubjects in State.CurrentSubjects)
                {
                    db.Subjects.Add(currentSubjects);
                }

                foreach (var currentOpinion in State.CurrentOpinions)
                {
                    db.Opinions.Add(currentOpinion);
                }
                
                foreach (var currentCategory in State.CurrentCategories)
                {
                    db.Categories.Add(currentCategory);
                }

                db.SaveChanges();
            }
        }

        [Route("api/rokred/clear")]
        [HttpGet]
        public void DeleteDatabase()
        {
            using (var db = new DataStorage())
            {
                var opinions = db.Opinions.ToList();

                foreach (var opinion in opinions)
                {
                    db.Opinions.Remove(opinion);
                }

                db.SaveChanges();
            }
        }
    }
}