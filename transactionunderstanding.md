We can implement this without changing your existing project structure too much.

The important concept is:

One user operation can involve multiple database changes. Those changes should either all succeed or all fail.

For example, when issuing a book:
1. Create a BookIssue record.
2. Decrease the book's AvailableCopies by 1.
3. Create an AuditLog record saying that the book was issued.
These three operations should be treated as one transaction.
1. Understand the concept using your project
Suppose Member 101 issues Book 5.

Before issuing:
Table	Data
Books	BookId = 5, AvailableCopies = 3
BookIssues	No new issue record
AuditLogs	No new audit record

After issuing successfully:
Table	Data
Books	BookId = 5, AvailableCopies = 2
BookIssues	New issue record created
AuditLogs	Book issued to Member 101

But suppose the audit log insertion fails.

We don't want the database to end up with:
- Book availability decreased.
- Book issue record created.
- Audit log missing.
Instead, all three changes should roll back.

here:
We'll also implement our first complete transaction:

Issue Book = Update Book Availability + Create BookIssue + Create AuditLog.

All three changes must succeed together. If one fails, all three should roll back.