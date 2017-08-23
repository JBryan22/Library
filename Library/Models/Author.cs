using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _name;

    public Author(string name, int id = 0)
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

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId()) == newAuthor.GetId();
        bool nameEquality = (this.GetName()) == newAuthor.GetName();

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
      cmd.CommandText = @"INSERT INTO authors (name) VALUE (@name);";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@name";
      authorId.Value = _name;
      cmd.Parameters.Add(authorId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Author> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors ORDER BY name;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Author> allAuthors = new List<Author>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Author foundAuthor = new Author(name, id);
        allAuthors.Add(foundAuthor);
      }
      conn.Close();
      return allAuthors;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors WHERE id = @id;";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@id";
      authorId.Value = _id;
      cmd.Parameters.Add(authorId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Author Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors WHERE id = @id;";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@id";
      authorId.Value = searchId;
      cmd.Parameters.Add(authorId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newAuthorId = 0;
      string authorName = "";

      while(rdr.Read())
      {
        newAuthorId = rdr.GetInt32(0);
        authorName = rdr.GetString(1);
      }
      var foundAuthor = new Author(authorName, newAuthorId);
      conn.Close();
      return foundAuthor;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE authors SET name = @name WHERE id = @id;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@id";
      authorId.Value = _id;
      cmd.Parameters.Add(authorId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddBook(Book newBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@authorId, @bookId);";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@authorId";
      authorId.Value = _id;
      cmd.Parameters.Add(authorId);

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = newBook.GetId();
      cmd.Parameters.Add(bookId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Book> GetBooks()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.*
      FROM authors
      JOIN authors_books ON (authors.id = authors_books.author_id)
      JOIN books ON (book.id = authors_books.book_id)
      WHERE authors.id = @authorId;";

      // @"SELECT books.*
      // FROM books
      // JOIN authors_books ON (books.id = authors_books.book_id)
      // JOIN authors ON (authors_books.author_id = authors.id)
      // WHERE authors.id = @authorId;";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@authorId";
      authorId.Value = _id;
      cmd.Parameters.Add(authorId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Book> allBooks = new List<Book>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        Book newBook = new Book(title, id);
        allBooks.Add(newBook);
      }
      conn.Close();
      return allBooks;
    }
  }
}
