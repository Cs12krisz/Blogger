using Blogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Blogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Blogger.Models.Blogger> GetAllBlogger()
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

        [HttpGet("GetById")]
        public ActionResult<Blogger.Models.Blogger> GetBloggerById(int id)
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
        public ActionResult<Blogger.Models.Blogger> InsertBlogger([FromBody] BloggerDto bloggerDto)
        {

            using (BloggerDbContext context = new BloggerDbContext())
            {
                if (bloggerDto != null)
                {
                    Blogger.Models.Blogger newBlogger = new Blogger.Models.Blogger()
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
        public ActionResult<Blogger.Models.Blogger> ModifyBloggerData(int id, BloggerDto bloggerDto)
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

    }
}
