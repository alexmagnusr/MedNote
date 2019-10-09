using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedNote.API.Models
{
    public class Menu
    {
        public string Id { get; set; }

        public string text { get; set; }

        public string hint { get; set; }

        public string sref { get; set; }

        public string icon { get; set; }

        public bool heading { get; set; }

        public string datatype { get; set; }

        public string translate { get; set; }

        public string alert { get; set; }

        public List<Menu> submenu { get; set; }

        public Menu Owner { get; set; }

        public Menu()
        {
            this.submenu = new List<Menu>();
        }

    }
}