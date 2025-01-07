﻿using ASPWebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPWebApp.Models.Dto
{
    public class PersonPageModel : PageModel
    {
        public List<Person>? PersonList { get; set; }
    }
}
