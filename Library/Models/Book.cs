using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book(string title, int id = 0)
    {
      _id = id;
      _title = title;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId()) == newBook.GetId();
        bool titleEquality = (this.GetTitle()) == newBook.GetTitle();

        return (idEquality && titleEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetTitle().GetHashCode(); //Ask John which to choose title or id
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (title) VALUE (@title);";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@title";
      bookId.Value = _title;
      cmd.Parameters.Add(bookId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books ORDER BY title;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Book> allBooks = new List<Book>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        Book foundBook = new Book(title, id);
        allBooks.Add(foundBook);
      }
      conn.Close();
      return allBooks;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE authors FROM books
      JOIN authors_books ON (books.id = authors_books.book_id)
      JOIN authors ON (authors_books.author_id = authors.id)
      WHERE books.id = @id;

      DELETE FROM authors_books WHERE book_id = @id;

      DELETE FROM copies WHERE book_id = @id;

      DELETE FROM books WHERE id = @id;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Book Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = @id;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = searchId;
      cmd.Parameters.Add(bookId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newBookId = 0;
      string bookTitle = "";

      while(rdr.Read())
      {
        newBookId = rdr.GetInt32(0);
        bookTitle = rdr.GetString(1);
      }
      var foundBook = new Book(bookTitle, newBookId);
      conn.Close();
      return foundBook;
    }

    public void Update(string newTitle)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE books SET title = @title WHERE id = @id;";

      MySqlParameter titleParameter = new MySqlParameter();
      titleParameter.ParameterName = "@title";
      titleParameter.Value = newTitle;
      cmd.Parameters.Add(titleParameter);

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@authorId, @bookId);";

      MySqlParameter authorId = new MySqlParameter();
      authorId.ParameterName = "@authorId";
      authorId.Value = newAuthor.GetId();
      cmd.Parameters.Add(authorId);

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Author> GetAuthors()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT authors.*
      FROM books
      JOIN authors_books ON (books.id = authors_books.book_id)
      JOIN authors ON (authors_books.author_id = authors.id)
      WHERE books.id = @bookId;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Author> allAuthors = new List<Author>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Author newAuthor = new Author(name, id);
        allAuthors.Add(newAuthor);
      }
      conn.Close();
      return allAuthors;
    }

    public List<Copy> GetCopies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @id;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> allCopies = new List<Copy>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int bookIdParam = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);
        Copy newCopy = new Copy(bookIdParam, available, id);
        allCopies.Add(newCopy);
      }
      conn.Close();
      return allCopies;
    }

    public List<Copy> GetAvailableCopies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @id AND available = @true;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      MySqlParameter availableStatus = new MySqlParameter();
      availableStatus.ParameterName = "@true";
      availableStatus.Value = true;
      cmd.Parameters.Add(availableStatus);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> allCopies = new List<Copy>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int bookIdParam = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);
        Copy newCopy = new Copy(bookIdParam, available, id);
        allCopies.Add(newCopy);
      }
      conn.Close();
      return allCopies;
    }

    public List<Copy> GetUnavailableCopies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @id AND available = @false;";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@id";
      bookId.Value = _id;
      cmd.Parameters.Add(bookId);

      MySqlParameter availableStatus = new MySqlParameter();
      availableStatus.ParameterName = "@false";
      availableStatus.Value = false;
      cmd.Parameters.Add(availableStatus);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> allCopies = new List<Copy>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int bookIdParam = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);
        Copy newCopy = new Copy(bookIdParam, available, id);
        allCopies.Add(newCopy);
      }
      conn.Close();
      return allCopies;
    }

    public void AddCopy(int amount)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;

      for (int i = 0; i < amount; i++)
      {
        cmd.CommandText = @"INSERT INTO copies (book_id, available) VALUES (@bookId, @available);";

        MySqlParameter bookId = new MySqlParameter();
        bookId.ParameterName = "@bookId";
        bookId.Value = _id;
        cmd.Parameters.Add(bookId);

        MySqlParameter availableStatus = new MySqlParameter();
        availableStatus.ParameterName = "@available";
        availableStatus.Value = true;
        cmd.Parameters.Add(availableStatus);

        cmd.ExecuteNonQuery();
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
