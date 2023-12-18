using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
		public int CategoryId { get; set; }
        public VideoGameCategory? Category { get; set; }
        public int Price { get; set; }
        public string Path { get; set; } = String.Empty;

		//mime


	}
}
