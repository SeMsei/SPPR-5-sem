using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Cart
{
	public Dictionary<int, CartItem> CartItems { get; set; } = new();

	public virtual void AddToCart(VideoGame videoGame)
	{
		if (CartItems.ContainsKey(videoGame.Id))
		{
			CartItems[videoGame.Id].Count += 1;
		}
		else
		{
			CartItems.Add(videoGame.Id, new CartItem { videoGame = videoGame, Count = 1 });
		}
	}

	public virtual void RemoveItems(int id)
	{
		if (CartItems.ContainsKey(id))
		{
			CartItems.Remove(id);
		}
	}

	public virtual void ClearAll()
	{
		CartItems.Clear();
	}

	public int Count { get => CartItems.Sum(item => item.Value.Count); }

	public double TotalPrice
	{
		get => CartItems.Sum(item => item.Value.videoGame.Price * item.Value.Count);
	}
}
