# Bango.SqlImport.Tests

# Setup
To go through the functionality end to end, use SystemTests class. The class demonstrates a few options when importing data from a csv file to a test table called TransactionImport.
To set up the tests:
- Run the CreateTransactionImportTable.sql script on the database you will use for testing.
- Set up a DB login you will use for testing.
- Make sure your DB login is correctly configured and has Insert and Select permissions on the test table.
- Update the SystemTests with the connection string to your database using your configured login.

# Tests
## BulkCopy_ManualReaderCreation
This test demonstrates the simplest usage of the SqlImport functionality. In this case, the csv format matches 1 to 1 the test table.

## BulkCopy_ConfigurationDrivenReaderCreation
This test demonstrates more complex usages of the SqlImport functionality: column mapping and value transformation.