using System;
using System.Collections.Generic;

namespace RoadMapSystem.Models.DB
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int AuthorId { get; set; }
        public int MileStoneId { get; set; }
        public string CommentValue { get; set; }

        public MileStone MileStone { get; set; }
    }
}
