using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using Library.Models;
using System.Collections.Generic;

namespace Library.Tests
{
  [TestClass]
  public class PatronTests : IDisposable
  {

     public PatronTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Patron.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Patron newPatron = new Patron("Jim");
      Patron newPatron2 = new Patron("Jim");

      bool result = newPatron.Equals(newPatron2);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Save_SavesNewPatronToDatabase_List()
    {
      Patron newPatron = new Patron("Jim");
      newPatron.Save();

      List<Patron> expected = new List<Patron>(){newPatron};
      var actual = Patron.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesAPatron_List()
    {
      Patron newPatron = new Patron("Jim");
      newPatron.Save();
      Patron newPatron2 = new Patron("Tim");
      newPatron2.Save();

      newPatron.Delete();

      List<Patron> expected = new List<Patron>() {newPatron2};
      List<Patron> actual = Patron.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsPatronById_Patron()
    {
      Patron newPatron = new Patron("Jim");
      newPatron.Save();

      var expected = newPatron;
      var result = Patron.Find(newPatron.GetId());

      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Update_UpdatesPatronName_Patron()
    {
      Patron newPatron = new Patron("Jim");
      newPatron.Save();

      Patron newPatron2 = new Patron("Jimmy");
      newPatron.Update("Jimmy");

      var expected = newPatron2;
      var actual = newPatron;
    }

    [TestMethod]
    public void AddCopy_AddsNewCopyToPatron_Void()
    {
      Patron newPatron = new Patron("Jim");
      newPatron.Save();

      Book newBook = new Book("Harry Potter", 1);

      Copy newCopy = new Copy(1);
      newCopy.Save();

      newPatron.AddCopy(newCopy);

      var expected = newCopy.GetId();

      var actual = newPatron.GetCopies()[0].GetId();

      Assert.AreEqual(expected, actual);
    }
  }
}
