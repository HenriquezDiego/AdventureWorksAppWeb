using System.Collections.Generic;

namespace AdventureWorksAppWeb.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<PhotoCategory> PhotoCategories { get; set; }
    }
}