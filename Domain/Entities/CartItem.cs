using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class CartItem
{
	public VideoGame videoGame { get; set; }
	public int Count { get; set; }
}
