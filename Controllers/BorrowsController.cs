using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize] // 🔒 User must be logged in
public class BorrowsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BorrowsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 📚 BORROW BOOK
    public async Task<IActionResult> BorrowBook(int bookId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge(); // force login

        var book = await _context.Books.FindAsync(bookId);
        if (book == null || !book.IsAvailable)
            return NotFound();

        var borrow = new Borrow
        {
            BookId = bookId,
            UserId = user.Id,
            BorrowDate = DateTime.Now
        };

        book.IsAvailable = false;

        _context.Borrows.Add(borrow);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Books");
    }

    // 🔁 RETURN BOOK
    public async Task<IActionResult> ReturnBook(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var borrow = await _context.Borrows
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

        if (borrow == null)
            return NotFound();

        borrow.ReturnDate = DateTime.Now;
        borrow.Book.IsAvailable = true;

        await _context.SaveChangesAsync();

        return RedirectToAction("MyBooks");
    }

    // 👤 VIEW MY BORROWED BOOKS
    public async Task<IActionResult> MyBooks()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var borrowedBooks = await _context.Borrows
            .Include(b => b.Book)
            .Where(b => b.UserId == user.Id && b.ReturnDate == null)
            .ToListAsync();

        return View(borrowedBooks);
    }
}
