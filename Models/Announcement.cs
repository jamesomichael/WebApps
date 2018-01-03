using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wb6.Models
{
    [Bind(Include="Subject, Description")]
    public class Announcement
    {
        public int AnnouncementID { get; set; }

        [Required(ErrorMessage = "Announcements must include a subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter some Announcement text")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        public virtual ApplicationUser User { get; set; }

        [ScaffoldColumn(false)]
        public List<ApplicationUser> MarkedAsRead { get; set; }

        [ScaffoldColumn(false)]
        public List<ApplicationUser> Unseen { get; set; }

    }
}