﻿using System.Collections.Generic;

namespace Catalog.BLL.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
