# Library Management System — Commit Documentation

## Project Overview

This document explains the development commits of the Library Management System, a C# Console Application built using .NET 10, Entity Framework Core, and SQL Server Express.

## Commit 1: Create the Console Project

**Purpose:**
- Create the .NET Console Application.
- Add a `.gitignore` file.

**Main files:**
- `Program.cs`
- `.gitignore`
- `LibraryManagementSystem.csproj`

**Commit message:**
```text
chore: Create library management console project
```

## Commit 2: Add the Book Model

**Purpose:**
Create the model for storing book details.

**Main file:**
- `Models/Book.cs`

**Properties:**
- BookId
- Title
- Author
- ISBN
- Category
- TotalCopies
- AvailableCopies

**Commit message:**
```text
feat: Add Book model
```

## Commit 3: Add Member and Book Issue Models

**Purpose:**
- Create the member model.
- Create the book issue model for tracking borrowed books and returns.

**Main files:**
- `Models/Member.cs`
- `Models/BookIssue.cs`

**Member properties:**
- MemberId
- MemberName
- Email
- PhoneNumber

**BookIssue properties:**
- BookIssueId
- BookId
- MemberId
- IssueDate
- DueDate
- ReturnDate

**Commit message:**
```text
feat: Add member and book issue models
```

## Commit 4: Configure EF Core and SQL Server Database

**Purpose:**
- Install Entity Framework Core packages.
- Create `LibraryDbContext`.
- Configure the SQL Server connection.
- Create and apply the initial database migration.

**Main files:**
- `Data/LibraryDbContext.cs`
- `Data/DatabaseConfiguration.cs`
- `Program.cs`
- EF Core migration files

**Database details:**
- Database name: `LibraryManagementDb`
- SQL Server instance: `(local)\SQLEXPRESS`

**Commands:**
```powershell
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Commit message:**
```text
feat: Configure EF Core and SQL Server database
```

## Commit 5: Add Book and Member Management Operations

**Purpose:**
- Add database operations for books and members.
- Validate book and member details.
- Add, view, search, update, and delete books.
- Add, view, update, and delete members.
- Prevent deletion when issue history exists.

**Main files:**
- `Repositories/BookRepository.cs`
- `Repositories/MemberRepository.cs`
- `Services/BookService.cs`
- `Services/MemberService.cs`

**Commit message:**
```text
feat: Add book and member management operations
```

## Commit 6: Add Book Issue and Return Operations

**Purpose:**
- Issue books to registered members.
- Check book availability before issuing.
- Record issue dates and due dates.
- Decrease available copies when issuing a book.
- Record the return date.
- Increase available copies when a book is returned.
- Prevent returning the same issue record twice.

**Main files:**
- `Repositories/BookIssueRepository.cs`
- `Services/BookIssueService.cs`

**Commit message:**
```text
feat: Add book issue and return operations
```

## Commit 7: Add Console Menus and User Interaction

**Purpose:**
- Create the book management menu.
- Create the member management menu.
- Create the book issue and return menu.
- Connect the menus through the main menu.
- Register repositories, services, and menus using dependency injection.

**Main files:**
- `Menus/BookMenu.cs`
- `Menus/MemberMenu.cs`
- `Menus/BookIssueMenu.cs`
- `Program.cs`

**Commit message:**
```text
feat: Add console menus and user interaction
```

## Commit 8: Finalize and Verify Console Workflows

**Purpose:**
- Verify main-menu navigation.
- Test book management operations.
- Test member management operations.
- Test book issue and return operations.
- Fix integration problems found during testing.

**Main files, if modified:**
- `Program.cs`
- `Menus/BookMenu.cs`
- `Menus/MemberMenu.cs`
- `Menus/BookIssueMenu.cs`

**Suggested commit message:**
```text
test: Finalize and verify console workflows
```

This commit does not include a Reports menu. Use this commit message only if you actually make and commit final testing or integration changes.

## Build and Run

```powershell
dotnet restore
dotnet build
dotnet run
```

To update the database:

```powershell
dotnet ef database update
```

## Git Commands

Check changes:
```powershell
git status
```

Stage and commit changes:
```powershell
git add <file-or-folder>
git commit -m "your commit message"
```

Push to GitHub:
```powershell
git push
```

## Important Notes

- This is a C# Console Application, not a Web API.
- Entity Framework Core handles database access.
- Repositories contain database operations.
- Services contain validation and application logic.
- Menus handle console input and output.
- The project includes book management, member management, and book issue/return operations.
- Reports functionality is not included.