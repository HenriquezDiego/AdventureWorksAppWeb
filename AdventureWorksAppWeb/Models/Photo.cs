﻿using System;
using System.Collections.Generic;

namespace AdventureWorksAppWeb.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}