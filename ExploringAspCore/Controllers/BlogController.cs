using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploringAspCore.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        // GET: /<controller>/
        [Route("")]
        public IActionResult Index()
        {
            return new ContentResult { Content = "Blog" };
        }
        [Route("{year:min(2000)}/{month:range(1,12)}/{key}" )]
        public IActionResult Post(int year, int month, string key)// look at map routes in startup it coorelates// use nullable or optional params = -1
        {
            //if (id == null)
            //{
            //    return new ContentResult { Content = "null" };

            //}
            //else
            //return new ContentResult { Content = id.ToString() };

            return new ContentResult {Content = String.Format("Year {0};  Month: {1};  key:{2}",year,month,key)};
        }
    }
}
