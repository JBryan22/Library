using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using Library.Models;
using System.Collections.Generic;

namespace Library.Tests
{
  [TestClass]
  public class BookTests : IDisposable
  {
     public BookTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Book.DeleteAll();
      Copy.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Book newBook = new Book("Harry Potter");
      Book newBook2 = new Book("Harry Potter");

      bool result = newBook.Equals(newBook2);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Save_SavesNewBookToDatabase_List()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      List<Book> expected = new List<Book>(){newBook};
      var actual = Book.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesABook_List()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();
      Book newBook2 = new Book("Clifford");
      newBook2.Save();

      newBook.Delete();

      List<Book> expected = new List<Book>() {newBook2};
      List<Book> actual = Book.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsBookById_Book()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      var expected = newBook;
      var result = Book.Find(newBook.GetId());

      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Update_UpdatesBookTitle_Book()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      newBook.Update("Harry Potter and the Sorcerers Stone");

      Book newBook2 = new Book("Harry Potter and the Sorcerers Stone");

      var expected = newBook2.GetTitle();
      var actual = Book.Find(newBook.GetId()).GetTitle();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddAuthor_AddsNewAuthorToDatabase_List()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();

      newBook.AddAuthor(newAuthor);

      var expected = new List<Author>() {newAuthor};
      var actual = newBook.GetAuthors();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddCopy_AddsXNumberOfCopiesToCopyDB_List()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      newBook.AddCopy(2);

      var result = newBook.GetCopies();
      var expected = Copy.GetAll();
      Console.WriteLine(result.Count);
      Console.WriteLine(expected.Count);

      CollectionAssert.AreEqual(expected, result);
    }

  }
}
