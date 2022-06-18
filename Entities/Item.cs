using System.ComponentModel.DataAnnotations.Schema;
using Enums;
using Repositories;
using Microsoft.AspNetCore.Identity;

namespace Entities;

public class Item
{
    public Product Product { get; set; }    
    public int Quantity { get; set; }
}