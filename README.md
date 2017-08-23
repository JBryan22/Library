# _Library_

#### _Librarian can manage a library._

#### By _**Jesse Bryan & Lois Choi**_

## Description

_Librarian can manage a library by creating patrons and checking out books for them. They will be able to view the checkout history for every patron. All books will have a certain amount of copies available._

## Setup/Installation Requirements

* _Requires MAMP and MySQL as well as C# and .NET_

_Download the necessary files from my GitHub Page._

| Specifications | Input   | Output   |
| -------  | ------- | -------   |
| 1. Librarian adds book | book name and author name | new Book object |
| 2. Librarian updates book | new book name AND/OR new book author | specific book object name updated |
| 3. Librarian deleted a book | delete book | specific book is deleted |
| 4. Librarian can view a list of all books | click on view list button/link | all books appear in a list |
| 5. Librarian can search for books by book name or book author | "harry potter" | Harry Potter and the .. * 7 |
| 6. Librarian can add additional authors to already existing book | Lib enters author name | name is attached to selected book |
| 7. Librarian can create a new patron | Librarian enters patron name | new Patron object created |
| 8. Librarian can check out a book for a patron | patron name + copyId | copy status changes to unavailable |
| 9. Checkout and due dates are assigned to a book upon checkout | copyId checked out | new DateTime for checkout as well as DateTime for Due. |
| 10. Librarian can view how many copies of a book are available | search Harry Potter | number of copies is displayed |
| 11. Librarian can view a patron's history | search patron detail | history of books checked out from library |
| 12. Librarian can view when a specific book is due for a patron | View patron details | currently checked out books + due dates appears |
| 13. Librarian can see a list of overdue books | Search overdue books / click on overdue books button | List of all overdue books for all patrons appears | 

## Known Bugs

_There are currently no known bugs in this program._

## Support and contact details

_For questions, please contact Jesse Bryan at jesse.bryan22@gmail.com or Lois Choi at loisch22@gmail.com _

## Technologies Used

_This app is programmed using HTML, CSS, C#, .NET, MVC, and Bootstrap._

### License

*All rights reserved.  Version 1.0.*

Copyright (c) 2017 **_Jesse Bryan & Lois Choi_**
