using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId()) == newPatron.GetId();
        bool nameEquality = (this.GetName()) == newPatron.GetName();

        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode(); //Ask John which to choose name or id
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons (name) VALUE (@name);";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@name";
      patronId.Value = _name;
      cmd.Parameters.Add(patronId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Patron> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons ORDER BY name;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Patron> allPatrons = new List<Patron>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Patron foundPatron = new Patron(name, id);
        allPatrons.Add(foundPatron);
      }
      conn.Close();
      return allPatrons;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @id;";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@id";
      patronId.Value = _id;
      cmd.Parameters.Add(patronId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Patron Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = @id;";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@id";
      patronId.Value = searchId;
      cmd.Parameters.Add(patronId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newPatronId = 0;
      string patronName = "";

      while(rdr.Read())
      {
        newPatronId = rdr.GetInt32(0);
        patronName = rdr.GetString(1);
      }
      var foundPatron = new Patron(patronName, newPatronId);
      conn.Close();
      return foundPatron;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patrons SET name = @name WHERE id = @id;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@id";
      patronId.Value = _id;
      cmd.Parameters.Add(patronId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddCopy(Copy newCopy)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO copies_patrons (copy_id, patron_id, due) VALUES (@copyId, @patronId, @due);";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@patronId";
      patronId.Value = _id;
      cmd.Parameters.Add(patronId);

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@copyId";
      copyId.Value = newCopy.GetId();
      cmd.Parameters.Add(copyId);

      DateTime dueDate = DateTime.Now;
      MySqlParameter due = new MySqlParameter();
      due.ParameterName = "@due";
      due.Value = dueDate.AddDays(14);
      cmd.Parameters.Add(due);

      cmd.ExecuteNonQuery();

      cmd.CommandText = @"UPDATE copies SET available = @false WHERE id = @newCopyId;";

      MySqlParameter copyAvailable = new MySqlParameter();
      copyAvailable.ParameterName = "@false";
      copyAvailable.Value = false;
      cmd.Parameters.Add(copyAvailable);

      MySqlParameter newCopyId = new MySqlParameter();
      newCopyId.ParameterName = "@newCopyId";
      newCopyId.Value = newCopy.GetId();
      cmd.Parameters.Add(newCopyId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Copy> GetCopies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT copies.*
      FROM patrons
      JOIN copies_patrons ON (patrons.id = copies_patrons.patron_id)
      JOIN copies ON (copies.id = copies_patrons.copy_id)
      WHERE patrons.id = @patronId;";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@patronId";
      patronId.Value = _id;
      cmd.Parameters.Add(patronId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> allCopies = new List<Copy>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);
        Copy newCopy = new Copy(bookId, available, id);
        allCopies.Add(newCopy);
      }
      conn.Close();
      return allCopies;
    }

    public List<DateTime> GetDueDate()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT due FROM copies_patrons WHERE patron_id = @id;";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@id";
      patronId.Value = _id;
      cmd.Parameters.Add(patronId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<DateTime> allDueDates = new List<DateTime>{};

      while(rdr.Read())
      {
        DateTime dueDate = rdr.GetDateTime(3);
        allCopies.Add(newCopy);
      }
      conn.Close();
      return allCopies;
    }
  }
}
