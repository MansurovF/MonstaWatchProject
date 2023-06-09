﻿using MonstaFinalProject.Models;

namespace MonstaFinalProject.ViewModels.ShopViewModels
{
	public class ShopVM
	{
		public List<Product> Products { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Color>? Colors { get; set; }
        public IEnumerable<Brand>? Brands { get; set; }
        public IEnumerable<Product>? AllProducts { get; set; }
        //public int SortSelect { get; set; }
        public double? MinimumPrice { get; set; }
        public double? MaximumPrice { get; set; }
    }
}
