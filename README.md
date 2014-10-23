# AppCore

AppCore is a lightweight .Net library for simplifying common operations, and speeding up development.


### Easy database access for Sql Server and MySql.

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

#### Retrieve a single value
```
// Retrieve a single value
var firstId = dataSource.ExecuteScalar<int>("SELECT TOP 1 id FROM Students");
```

#### Execute a statement
```
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
```
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

#### Hash a password (base 64 encoded SHA1 hash)
```
var password = "MySecretPassword";
var hashedPassword = password.HashPassword(); // returns "lScpxhyrfgHktfW6e5WDDSB190s="
```

