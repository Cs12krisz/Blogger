using Blogger.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blogger.Controllers;


namespace Blogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Models.Blogger> GetAllBlogger()
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                var bloggers = context.Blog.ToArray();

                if (bloggers != null)
                {
                    return Ok(bloggers);
                }

                return BadRequest(new { message = "Sikertelen lekérés" });
            }

        }

        [HttpGet("BloggersNumber")]
        public ActionResult<Models.Blogger> GetBloggerCount()
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                var bloggers = context.Blog.ToArray().Length;

                if (bloggers >= 0)
                {
                    return Ok(new { message = $"Bloggerek száma: {bloggers}" });
                }

                return BadRequest(new { message = "Sikertelen lekérés" });
            }

        }

        [HttpGet("BloggersNameAndEmail")]
        public ActionResult<Models.Blogger> GetBloggerNameAndEmail()
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                var bloggers = context.Blog.Select(b => new {b.Name, b.Email}).ToArray();

                if (bloggers != null)
                {
                    return Ok(bloggers);
                }

                return BadRequest(new {message = "Sikertelen lekérés"});
            }
        }

        [HttpGet("OldestBlogger")]
        public ActionResult<Models.Blogger> GetOldestBlogger()
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                var oldest = context.Blog.OrderBy(b => b.RegTime).FirstOrDefault();

                if (oldest != null)
                {
                    return Ok(new { message = "A legrégebbi blogger", value = oldest });
                }

               return BadRequest(new { message = "Sikertelen lekérdezés" });
            }

        }

        [HttpGet("GetById")]
        public ActionResult<Models.Blogger> GetBloggerById(int id)
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                var blogger = context.Blog.FirstOrDefault(b => b.Id == id);

                if (blogger != null)
                {
                    return Ok(blogger);
                }

            }

            return NotFound(new {message = "Nincs ilyen Id"});
        }

        [HttpPost]
        public ActionResult<Models.Blogger> InsertBlogger([FromBody] Dtos.BloggerDto bloggerDto)
        {

            using (BloggerDbContext context = new BloggerDbContext())
            {
                if (bloggerDto != null)
                {
                    Models.Blogger newBlogger = new Models.Blogger()
                    {
                        Name = bloggerDto.Name,
                        Email = bloggerDto.Email,
                        RegTime = DateTime.Now,
                        ModTime = DateTime.Now
                    };


                    if (newBlogger != null)
                    {
                        context.Blog.Add(newBlogger);
                        context.SaveChanges();
                        return StatusCode(201, newBlogger);
                    }
                }

                return BadRequest(new { message = "Sikertelen feltöltés" });
            }
            
        }

        [HttpPut]
        public ActionResult<Models.Blogger> ModifyBloggerData(int id, Dtos.BloggerDto bloggerDto)
        {
            using (BloggerDbContext context = new BloggerDbContext())
            {
                if (bloggerDto != null)
                {
                    var foundBlogger = context.Blog.FirstOrDefault(b => b.Id == id);

                    if (foundBlogger != null)
                    {
                        foundBlogger.Email = bloggerDto.Email;
                        foundBlogger.Name = bloggerDto.Name;
                        foundBlogger.ModTime = DateTime.Now;

                        context.Blog.Update(foundBlogger);
                        context.SaveChanges();
                        return StatusCode(201, foundBlogger);
                    }
                }
            }

            return BadRequest(new {message = "Sikertelen módosítás"});
        }


        [HttpDelete]
        public ActionResult<Models.Blogger> DeleteBlogger(int id)
        {
            using(BloggerDbContext context = new BloggerDbContext())
            {
                var foundBlogger = context.Blog.FirstOrDefault(b => b.Id == id);

                if (foundBlogger != null)
                {
                    context.Remove(foundBlogger);
                    context.SaveChanges();
                    return StatusCode(204);
                }

                return NotFound(new { message = "Nincs ilyen id" });
            }

        }
    }
}
