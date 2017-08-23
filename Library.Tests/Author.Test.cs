using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using Library.Models;
using System.Collections.Generic;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTests : IDisposable
  {

     public AuthorTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Author.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Author newAuthor = new Author("JK Rowling");
      Author newAuthor2 = new Author("JK Rowling");

      bool result = newAuthor.Equals(newAuthor2);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Save_SavesNewAuthorToDatabase_List()
    {
      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();

      List<Author> expected = new List<Author>(){newAuthor};
      var actual = Author.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesAAuthor_List()
    {
      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();
      Author newAuthor2 = new Author("Clifford");
      newAuthor2.Save();

      newAuthor.Delete();

      List<Author> expected = new List<Author>() {newAuthor2};
      List<Author> actual = Author.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsAuthorById_Author()
    {
      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();

      var expected = newAuthor;
      var result = Author.Find(newAuthor.GetId());

      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Update_UpdatesAuthorTitle_Author()
    {
      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();

      Author newAuthor2 = new Author("J.K. Rowling");
      newAuthor.Update("J.K. Rowling");

      var expected = newAuthor2;
      var actual = newAuthor;
    }

    [TestMethod]
    public void AddBook_AddsNewBookToDatabase_List()
    {
      Book newBook = new Book("Harry Potter");
      newBook.Save();

      Author newAuthor = new Author("JK Rowling");
      newAuthor.Save();

      newAuthor.AddBook(newBook);

      var expected = new List<Book>() {newBook};
      var actual = newAuthor.GetBooks();

      CollectionAssert.AreEqual(expected, actual);
    }
  }
}
