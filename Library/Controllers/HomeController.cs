using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet("/catalog/list")]
    public ActionResult CatalogList()
    {
      List<Book> allBooks = Book.GetAll();

      return View(allBooks);
    }
    [HttpGet("/book/details/{id}")]
    public ActionResult BookDetail(int id)
    {
      return View(Book.Find(id));
    }
    [HttpPost("/book/details/{id}")]
    public ActionResult BookDetailPost(int id)
    {
      Author newAuthor = new Author(Request.Form["author-name"]);
      newAuthor.Save();
      Book foundBook = Book.Find(id);
      foundBook.AddAuthor(newAuthor);

      return View("BookDetail", Book.Find(id));
    }
    [HttpGet("/book/form/")]
    public ActionResult BookForm()
    {
      return View();
    }
    [HttpPost("/add/book/success")]
    public ActionResult AddBookSuccess()
    {
      string bookTitle = Request.Form["book-title"];
      int copies = int.Parse(Request.Form["book-copies"]);

      Author authorName = new Author(Request.Form["book-author"]);
      authorName.Save();

      Book newBook = new Book(bookTitle, copies);
      newBook.Save();
      newBook.AddAuthor(authorName);

      for (int i = 0; i < copies; i++)
      {
        Copy newCopy = new Copy(newBook.GetId());
        newCopy.Save();
      }

      return View(newBook);
    }
    [HttpGet("/book/{id}/delete")]
    public ActionResult DeleteBook(int id)
    {
      Book.Find(id).Delete();

      return View("DeleteSuccess");
    }

    [HttpGet("/patron/form")]
    public ActionResult PatronForm()
    {
      return View();
    }
    [HttpPost("/add/patron/success")]
    public ActionResult AddPatronSuccess()
    {
      string newName = Request.Form["name"];
      Patron newPatron = new Patron(newName);
      newPatron.Save();

      return View(newPatron);
    }
    [HttpGet("/patron/list")]
    public ActionResult PatronList()
    {
      return View(Patron.GetAll());
    }

    [HttpGet("/patron/details/{id}")]
    public ActionResult PatronDetail(int id)
    {
      Patron foundPatron = Patron.Find(id);
      List<Copy> patronCopies = foundPatron.GetCopies();

      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("patron", foundPatron);
      model.Add("copies", patronCopies);

      return View(model);
    }


    [HttpPost("/patron/details/{id}")]
    public ActionResult PatronDetailPost(int id)
    {
      Patron foundPatron = Patron.Find(id);
      List<Copy> patronCopies = foundPatron.GetCopies();

      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("patron", foundPatron);
      model.Add("copies", patronCopies);

      return View("PatronDetail", model);
    }

    [HttpGet("/checkout/{idCopy}/book/{idBook}")]
    public ActionResult CheckoutForm(int idCopy, int idBook)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();

      Copy foundCopy = Copy.Find(idCopy);
      model.Add("copy", foundCopy);

      Book foundBook = Book.Find(idBook);
      model.Add("book", foundBook);

      List<Patron> allPatrons = Patron.GetAll();
      model.Add("patron", allPatrons);

      return View(model);
    }

    [HttpPost("/success/checkout")]
    public ActionResult CheckoutSuccess()
    {
      int patronId = int.Parse(Request.Form["patron"]);

      Patron selectedPatron = Patron.Find(patronId);

      Copy selectedCopy = Copy.Find(int.Parse(Request.Form["copy-id"]));
      selectedPatron.AddCopy(selectedCopy);

      return View(selectedPatron);
    }

    [HttpGet("/patron/details/{copyId}/return/{patronId}")]
    public ActionResult PatronDetailReturn(int copyId, int patronId)
    {
      Patron selectedPatron = Patron.Find(patronId);
      Copy selectedCopy = Copy.Find(copyId);

      selectedPatron.ReturnCopy(selectedCopy);

      List<Copy> patronCopies = selectedPatron.GetCopies();

      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("patron", selectedPatron);
      model.Add("copies", patronCopies);

      return View("PatronDetail", model);
    }


  }
}
