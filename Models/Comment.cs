using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wb6.Models
{
    [Bind(Include="Content")]
    public class Comment
    {
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Comments must include some text")]
        public string Content { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set;}

        [ScaffoldColumn(false)]
        public virtual Announcement Announcement { get; set; }

        [ScaffoldColumn(false)]
        public virtual ApplicationUser User { get; set; }
    }
}