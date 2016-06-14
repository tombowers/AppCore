# AppCore

AppCore is a lightweight .Net library for simplifying common operations, and speeding up development.

### Easy database access for Sql Server and MySql.

#### Installation
There are Nuget packages available for both SQL Server and MySQL. Use one of the following commands in your Nuget package manager console.

`Install-Package Invisual.Data.SqlServer`

or

`Install-Package Invisual.Data.MySql`

#### Prepare a select query and process the results into a collection of objects
```C#
var dataSource = new MsSqlDataSource("Server=serverAddress;Database=database;Trusted_Connection=True;");

var students = dataSource
  .Execute("SELECT * FROM Students")
  .Select(s =>
    new Student(
      s.GetValueOrDefault<int>("id"),
      s.GetValueOrDefault<string>("name"),
      s.GetValueOrDefault<bool>("enrolled")
    );
  });
```

The above code needs the following using statements.

```C#
using System.Linq; // For .Select()
using Invisual.Data; // Contains MsSqlDataSource;
using Invisual.AppCore.Extensions; // Contains GetValueOrDefault<T>()
```

N.B. The query passed to Execute won't run against the server until the returned sequence is iterated over. If you need it to run immediately, call .ToList() on the results. Conversely, repeatedly iterating over the results will repeat the query. This can be useful for uncacheable data!

#### Retrieve a single value
```C#
var firstId = dataSource.ExecuteScalar<int>("SELECT TOP 1 id FROM Students");
```

#### Execute a statement
```C#
var rowsAffected = dataSource.ExecuteNonQuery("DROP TABLE Students");
```

### Convenient extension methods

#### Get the value of the Description attribute from a class, property, or method
```C#
...
[Description("Such a lovely property")]
public string SomeProperty { get; private set; }
...

var propertyDescription = someObject.SomeProperty.GetDescription();
```

#### Convert a string to an enum based on the description attribute or its name
```C#
var activeStatus = "a".ToEnum<ProductStatus>();

var inactiveStatus = "Inactive".ToEnum<ProductStatus>();

...
public enum ProductStatus
{
  [Description("a")]
  Active,
  
  Inactive,
  
  [Description("d")]
  Discontinued
}
...
```

#### Hash a string (base64 encoded SHA1 hash)
N.B. Shouldn't be used for passwords, which should be salted, and use a more cryptographically secure hashing algorithm.
```C#
var plain = "MySecretText";
var hashed = plain.Sha1Base64Hash(); // returns "lScpxhyrfgHktfW6e5WDDSB190s="
```

