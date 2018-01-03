using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wb6.Models
{
    public class AnnouncementViewModel
    {
        public Announcement Announcement { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Seen> MarkedAsRead { get; set; }
    }
}