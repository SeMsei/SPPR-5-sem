using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_153501_fomichevskiy.Domain.Entity
{
	public class VideoGame
	{
		int Id;
		public String Name { get; set; }
		public String Description { get; set; }
		int CategoryId;
		public int Price { get; set; }
		public String? Path { get; set; }  

		public VideoGame(int id, string name, string description, int categoryId, int price, string? path)
		{
			Id = id;
			Name = name;
			Description = description;
			CategoryId = categoryId;
			Price = price;
			Path = path;
		}

		//mime


	}
}
