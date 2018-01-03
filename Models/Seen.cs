using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wb6.Models
{   
    public class Seen
    {
        public int SeenID { get; set; }
        public bool HasSeen { get; set; }
        public virtual Announcement Announcement { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}