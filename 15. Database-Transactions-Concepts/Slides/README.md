<!-- section start -->

<!-- attr: {id: 'title', class: 'slide-title', hasScriptWrapper: true} -->
# Database Transaction Concepts
## Transaction Management and Concurrency Control

<div class="signature">
    <p class="signature-course">Databases</p>
    <p class="signature-initiative">Telerik Software Academy</p>
    <a href="http://academy.telerik.com" class="signature-link">http://academy.telerik.com</a>
</div>

<!-- section start -->

<!-- attr: {id: 'table-of-contents', class:'table-of-contents'} -->
# Table of Contents

*   What is a Transaction?
*   ACID Transactions
*   Managing Transactions in SQL
*   Concurrency Problems in DBMS
*   Concurrency Control Techniques
    *   Locking Strategies
    *   Optimistic vs. Pessimistic Locking
*   Transaction Isolation Levels
*   Transaction Log and Recovery
*   When and How to Use Transactions?

<!-- section start -->

#   What is a Transaction?
##  And why they are a must-have

#   Transactions

*   Transactions is a sequence of actions (database operations) executed as a whole:
    *   Either all of them complete successfully
    *   Or none of the them
* __Example__ of transaction:
    *   A bank transfer from one account into another (withdrawal + deposit)
    *   If either the withdrawal or the deposit fails the whole operation is cancelled

<img src="imgs/example-transaction.png" />

#   Transactions' Behavior

*   Transactions guarantee the consistency and the integrity of the database
    *   All changes in a transaction are temporary
    *   Changes are persisted when COMMIT is executed
    *   At any time all changes can be canceled by ROLLBACK
*   All of the operations are executed as a whole
    *   Either all of them or none of them

#   Transactions: Example

<div>
  *   Withdraw $100
      1.   Read current balance
      *   New balance = current - $100
      *   Write new balance
      *   Dispense cash

</div> <!-- style="float:left; width:50%" -->

<div style="float:left; width:50%">
  *   Transfer $100
      *   Read savings
      *   New savings =current - $100
      *   Read checking
      *   New checking =current  + $100
      *   Write savings
      *   Write checking

</div> <!-- style="float:left; width:50%" -->

#   What Can Go Wrong Without Transactions?

*   Some actions fail to complete
    *   For example, the application software or database server crashes
*   Interference from another transaction
    *   What will happen if several transfers run for the same account in the same time?
*   Some data lost after actions complete
    *   Database crashes after withdraw is complete and all other actions are lost

<!-- section start -->

#   ACID Transactions
##    Atomicity, Consistency, Isolation, Durability

#   Transaction Properties

*   Modern DBMS servers have built-in transaction support
    *   Implement **ACID** transactions
    *   E.g. MS SQL Server, Oracle, MySQL, …

*   **ACID** means:
    *   ***A*** tomicity
    *   ***C*** onsistency
    *   ***I*** solation
    *   ***D*** urability

#   Atomicity

*   **Atomicity** means that
    *   Operations in a transaction execute as a whole
    *   DBMS to guarantee that either all of the operations are performed or none of them

*   _Atomicity example:_
    *   Transfer funds between bank accounts
        *   Either withdraw **and** deposit **both execute successfully** or none of them
        *   In case of failure the DB stays unchanged

#   Consistency

*   **Consistency** means that
    *   The database is in a legal state when the transaction begins and when it ends
    *   Only valid data will be written in the DB
    *   Transaction cannot break the rules of the database, e.g. integrity constraints
        *   Primary keys, foreign keys, alternate keys
*   _Consistency example:_
    *   Transaction cannot end with a duplicate primary key in a table

#   Isolation

*   Isolation means that
    *   Multiple transactions running at the same time do not impact each other's execution
    *   Transactions don't see othertransaction's uncommitted changes
    *   **Isolation level** defines how deep transactions isolate from one another
*   _Isolation example:_
    *   Manager can see the transferred funds on one account or the other, but never on both

#   Durability

*   Durability means that
    *   If a transaction is committed it becomes persistent
    *   Cannot be lost or undone
    *   Ensured by use of database transaction logs
*   _Durability example:_
    *   After funds are transferred and committed the power supply at the DB server is lost
    *   Transaction stays persistent (no data is lost)

#   ACID Transactions and RDBMS

*   Modern RDBMS servers are transactional:
    *   Microsoft SQL Server, Oracle Database, PostgreSQL, FirebirdSQL, …
*     All of the above servers support ACID transactions
    *   MySQL can also run in ACID mode (InnoDB)
*     Most cloud databases are transactional as well
    *   Amazon SimpleDB, AppEngine Datastore, Azure Tables, MongoDB, …

<!-- section start -->

#   Managing Transactions in SQL Language

#   Transactions and SQL

*   Start a transaction

    ```sql
    BEGIN TRANSACTION;
    ```

    *   Some RDBMS use implicit start, e.g. Oracle
*   Ending a transaction:
    *   **Complete a successful** transaction and persist all changes maked    

    ```sql
    COMMIT;
    ```

    *   "Undo" changes from an aborted transaction
    *   May be done automatically when failure occurs

    ```sql
    ROLLBACK;
    ```

#   Transactions inSQL Server: Example

*   We have a table with bank accounts:

```sql
CREATE TABLE Accounts(
  Id int NOT NULL PRIMARY KEY,
  Balance decimal NOT NULL)
```

*   We use a transaction to transfer money from one account into another:

```sql
CREATE PROCEDURE sp_Transfer_Funds(
  @from_account INT,
  @to_account INT,
  @amount MONEY) AS
BEGIN
  BEGIN TRAN;  //or BEGIN TRANSACTION

  UPDATE Accounts set Balance = Balance - @amount
  WHERE ID = @from_account;

  if @@ROWCOUNT <> 1
  BEGIN
    ROLLBACK;
    RAISERROR('Invalid source Account!', 16, 1);
    RETURN;
  END;

  UPDATE Accounts SET Balance = Balance + @amount
  WHERE Id = @to_account;

  if @@ROWCOUNT <> 1
  BEGIN
    ROLLBACK;
    RAISERROR("Invalid destination account", 16, 1);
    RETURN;
  END;

  COMMIT;
END;
```

#   Transfer Funds
##    [Demo](http://)

<!-- section start -->

#   Concurrency Problems in Database Systems

#   Scheduling Transactions

*   **Serial schedule** - the ideal case
    *   Transactions execute one after another
        *   No overlapping: users wait one another
    *   Not scalable: doesn’t allow much concurrency
*   **Conflicting operations**
    *   Two operations conflict if they:
        1.   are performed in different transactions
        *   access the same piece of data
        *   at least one of the transactions does a write operation to that piece of data

#   Serial Schedule – Example

*   T1:	Adds $50 to the balance
*   T2: Subtracts $25 from the balance
*   T1 completes before T2 begins
    *   No concurrency problems

| Time | Transactions | Step | Value |
| ---- | -----------  | ---- | ----- |
| 1 | T1 | Read balance | 100 |
| 2 | T1 | Balance = 100 + 50 | 100  |
| 3 | T1 | Write balance | 150 |
| 4 | T2 | Read balance | 150 |
| 5 | T2 | Balance = 150 - 50 | 150 |
| 6 | T2 | Write balance | 125 |

#   Serializable Transactions

*   Serializability
    *   Want to get the effect of serial schedules,but allow for more concurrency
    *   Serializable schedules
        *   Equivalent to serial schedules
        *   Produce same final result as serial schedule
*   Locking mechanisms can ensure serializability
*   Serializability is too expensive
    *   Optimistic locking allows better concurrency

#   Concurrency Problems

*   Problems from conflicting operations:
  *   **Dirty Read**
      *   A transaction updates an item, then fails
      *   The item is accessed by another transaction before the rollback
      *   The second transaction reads invalid data
  *   Non-Repeatable Read
      *   A transaction reads the same item twice
      *   And gets different values
      *   Due to concurrent change in another transaction


<!-- section start -->

<!-- attr: {id: 'questions', class: 'slide-questions', showInPresentation: true} -->
# Database Transactions Concepts
## Questions
