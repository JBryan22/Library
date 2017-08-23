using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System;
using Library.Models;
using System.Collections.Generic;

namespace Library.Tests
{
  [TestClass]
  public class CopyTests : IDisposable
  {

     public CopyTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Copy.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Copy newCopy = new Copy(1, true);
      Copy newCopy2 = new Copy(1, true);

      bool result = newCopy.Equals(newCopy2);

      Assert.AreEqual(true, result);
    }

    // [TestMethod]
    // public void Save_SavesNewCopyToDatabase_List()
    // {
    //   Copy newCopy = new Copy(1);
    //   newCopy.Save();
    //
    //   List<Copy> expected = new List<Copy>(){newCopy};
    //   var actual = Copy.GetAll();
    //
    //   CollectionAssert.AreEqual(expected, actual);
    // }

    [TestMethod]
    public void Delete_DeletesACopy_List()
    {
      Copy newCopy = new Copy(1);
      newCopy.Save();
      Copy newCopy2 = new Copy(2);
      newCopy2.Save();

      newCopy.Delete();

      List<Copy> expected = new List<Copy>() {newCopy2};
      List<Copy> actual = Copy.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsCopyById_Copy()
    {
      Copy newCopy = new Copy(1);
      newCopy.Save();

      var expected = newCopy;
      var result = Copy.Find(newCopy.GetId());

      Assert.AreEqual(expected, result);
    }
  }
}
