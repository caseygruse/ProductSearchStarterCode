using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductSearchStarterCode.Models
{
	/// <summary>
	/// creating a viewmodel so you can filter products.
	/// </summary>
	public class ProductSearchViewModel
	{
		public ProductSearchViewModel()
		{
			SearchResults = new List<Product>();
		}

		public List<Product> SearchResults { get; set; }

		public string ProductName { get; set; }
		//the question mark makes the property optional
		public double? MinPrice { get; set; }
		//the question mark makes the property optional
		public double? MaxPrice { get; set; }

		public string Category { get; set; }
	}
}