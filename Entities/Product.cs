using System.ComponentModel.DataAnnotations.Schema;
using Enums;
using Repositories;
using Microsoft.AspNetCore.Identity;

namespace Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Size Size { get; set; } = Size.M;
    public string Name { get; set; } = String.Empty;
	public Color Color { get; set; } = Color.White;
	public string Image { get; set; } = String.Empty;
    public decimal Price { get; set; } = 0;
    public string Description { get; set; } = String.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; } = null;
    //public User User { get; set; } = null;

    //[NotMapped]
    public Category Category { get; set; } 
}