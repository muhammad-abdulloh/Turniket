﻿namespace TaskSoliq.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public bool? IsEmployee { get; set; }
        public string? ImageUrl { get; set; }

    }
}
