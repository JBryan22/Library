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

    }
  }
}
