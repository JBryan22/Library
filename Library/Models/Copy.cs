using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Copy
  {
    private int _id;
    private int _bookId;
    private bool _available;

    public Copy(int bookId, bool available = true, int id = 0)
    {
      _bookId = bookId;
      _available = available;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public bool GetAvailable()
    {
      return _available;
    }

    public int GetBookId()
    {
      return _bookId;
    }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = (this.GetId()) == newCopy.GetId();
        bool bookIdEquality = (this.GetBookId()) == newCopy.GetBookId();
        bool availableEquality = (this.GetAvailable()) == newCopy.GetAvailable();

        return (idEquality && bookIdEquality && availableEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetBookId().GetHashCode(); //Ask John which to choose copyId or id
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO copies (book_id, available) VALUE (@bookId, @availableId);";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = _bookId;
      cmd.Parameters.Add(bookId);

      MySqlParameter availableId = new MySqlParameter();
      availableId.ParameterName = "@availableId";
      availableId.Value = _available;
      cmd.Parameters.Add(availableId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Copy> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies ORDER BY book_id;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Copy> allCopies = new List<Copy>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        bool newAvailable = rdr.GetBoolean(2);
        Copy foundCopy = new Copy(bookId, newAvailable, id);
        allCopies.Add(foundCopy);
      }
      conn.Close();
      return allCopies;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM copies;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM copies WHERE id = @id;";

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@id";
      copyId.Value = _id;
      cmd.Parameters.Add(copyId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Copy Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE id = @id;";

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@id";
      copyId.Value = searchId;
      cmd.Parameters.Add(copyId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newCopyId = 0;
      int bookId = 0;
      bool newAvailable = false;

      while(rdr.Read())
      {
        newCopyId = rdr.GetInt32(0);
        bookId = rdr.GetInt32(1);
        newAvailable = rdr.GetBoolean(2);
      }
      var foundCopy = new Copy(bookId, newAvailable, newCopyId);
      conn.Close();
      return foundCopy;
    }

    public DateTime GetDueDate()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT due FROM copies_patrons WHERE copy_id = @id ORDER BY due;";

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@id";
      copyId.Value = _id;
      cmd.Parameters.Add(copyId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      DateTime dueDate = DateTime.Now;
      while(rdr.Read())
      {
        dueDate = rdr.GetDateTime(0);
      }
      conn.Close();
      return dueDate;
    }

    public string GetTitle()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT title FROM books where id = @id;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _bookId;
      cmd.Parameters.Add(bookId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      string title = "";

      while(rdr.Read())
      {
        title = rdr.GetString(0);
      }
      conn.Close();
      return title;
    }
  }
}
