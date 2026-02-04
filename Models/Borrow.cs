using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Borrow
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; }
}
