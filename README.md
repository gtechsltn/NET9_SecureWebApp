# Secure Your .NET Web App in .NET 9
* 6: Native AOT
* 5: OpenID Connect and OAuth 2.0
* 4: OAuth (Open Authorization)
* 3: JWT (JSON Web Token)
* 2: API Key authorization
* 1: Basic Authentication

* Role-Based Access Control (RBAC)
✅ Best for: Applications with predefined user roles (e.g., Admin, Manager, User).
✅ RBAC assigns users to specific roles, granting access based on their assigned role. It’s simple to implement and widely used.
* Claims-Based Authorization
✅ Best for: Scenarios where access is based on user attributes (e.g., department, clearance level).
✅ Claims-based authorization allows defining user claims (key-value pairs) to control access dynamically.
✅ Claims are typically stored in JWT tokens or identity providers.
* Policy-Based Authorization
✅ Best for: Applications with complex access rules beyond roles and claims.
✅ Policies define rules using claims, roles, or custom logic.
* Attribute-Based Authorization (Custom Handlers)
✅ Best for: Advanced scenarios requiring custom authorization logic.
✅ This method uses custom attributes and handlers to control access dynamically.

![Authorization Mechanism](https://github.com/user-attachments/assets/df75377f-6b2f-48a4-aeee-4bbcf1bf0ffa)

# Multi-Factor Authentication (MFA) in .NET Applications

## What is MFA?

Multi-Factor Authentication (MFA) is a security mechanism that requires users to provide two or more forms of verification before gaining access to an application. This significantly enhances security by reducing the risk of unauthorized access due to stolen passwords.

## Why Use MFA in .NET Applications?
🔹 Protects against credential theft and phishing attacks.
🔹 Strengthens access control by requiring multiple verification factors.
🔹 Ensures compliance with security standards (e.g., GDPR, HIPAA).

## Common MFA Methods
✅ Something You Know – Password, PIN
✅ Something You Have – OTP (One-Time Password) via SMS, Email, Authenticator App
✅ Something You Are – Biometric verification (fingerprint, facial recognition)

# Security Topics
* Authentication
* Authorization
* Data protection
* HTTPS enforcement
* Safe storage of app secrets in development
* XSRF/CSRF prevention
* Cross Origin Resource Sharing (CORS)
* Cross-Site Scripting (XSS) attacks
* OWASP Cheat Sheet Series
* Enforce HTTPS
* Configure Secure Headers
* Use Multi-Factor Authentication (MFA)
* Modern Authentication Protocols (OpenID Connect and OAuth 2.0)
* Security Best Practices
* Monitor and Log Securely

Protect Your .NET Applications: Best 4 Authorization Mechanisms

https://docs.google.com/document/d/1RK6_cc9xr9wgOfrq1aet1RUUVzQwJjd3nLqdHWmTHBw/

Protect Your .NET Applications: Best 4 Authorization Mechanisms

https://medium.com/@jeslurrahman/protect-your-net-applications-best-4-authorization-mechanisms-7065550c5fb7

.NET 9: Key Features and Improvements in Security, Performance, and Ease of Use

https://medium.com/@robertdennyson/net-9-key-features-and-improvements-in-security-performance-and-ease-of-use-57078501065f

Enhancing Security in .NET 9: New Features and Best Practices for Developers

https://dev.to/leandroveiga/enhancing-security-in-net-9-new-features-and-best-practices-for-developers-3b39

Secure Your .NET Applications: Top Security Practices in .NET 9

https://www.linkedin.com/pulse/secure-your-net-applications-top-security-practices-9-hetal-mehta-erxdf/

# CRUD using Stored Procedure

## Steps to Implement:
1️⃣ Create a SQL Server table
2️⃣ Create stored procedures for CRUD operations
3️⃣ Write C# code to call these procedures using ADO.NET

### 1. Create Table in SQL Server
```
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    City NVARCHAR(50)
);
```
### 2. Create Stored Procedures
```
CREATE PROCEDURE InsertCustomer
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @City NVARCHAR(50)
AS
BEGIN
    INSERT INTO Customers (Name, Email, City)
    VALUES (@Name, @Email, @City);
    
    SELECT SCOPE_IDENTITY(); -- Return the new ID
END;
CREATE PROCEDURE GetAllCustomers
AS
BEGIN
    SELECT * FROM Customers;
END;
CREATE PROCEDURE GetAllCustomers
AS
BEGIN
    SELECT * FROM Customers;
END;
CREATE PROCEDURE GetCustomerById
    @Id INT
AS
BEGIN
    SELECT * FROM Customers WHERE Id = @Id;
END;
CREATE PROCEDURE UpdateCustomer
    @Id INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @City NVARCHAR(50)
AS
BEGIN
    UPDATE Customers
    SET Name = @Name, Email = @Email, City = @City
    WHERE Id = @Id;
END;
CREATE PROCEDURE DeleteCustomer
    @Id INT
AS
BEGIN
    DELETE FROM Customers WHERE Id = @Id;
END;
```

### 3. C# Code for CRUD Operations
```
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertCustomerAsync(string name, string email, string city)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("InsertCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@City", city);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync()); // Returns new ID
            }
        }
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var customers = new List<Customer>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("GetAllCustomers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        customers.Add(new Customer
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            City = reader.GetString(3)
                        });
                    }
                }
            }
        }
        return customers;
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("GetCustomerById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Customer
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            City = reader.GetString(3)
                        };
                    }
                }
            }
        }
        return null; // If not found
    }

    public async Task<bool> UpdateCustomerAsync(int id, string name, string email, string city)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("UpdateCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@City", city);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("DeleteCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        string connectionString = "YourConnectionStringHere";
        var dbHelper = new DatabaseHelper(connectionString);

        // Insert a new customer
        int newCustomerId = await dbHelper.InsertCustomerAsync("John Doe", "john@example.com", "New York");
        Console.WriteLine($"Inserted new customer with ID: {newCustomerId}");

        // Get all customers
        var customers = await dbHelper.GetAllCustomersAsync();
        Console.WriteLine("All Customers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Id} - {customer.Name} ({customer.Email}) - {customer.City}");
        }

        // Get a single customer
        var singleCustomer = await dbHelper.GetCustomerByIdAsync(newCustomerId);
        Console.WriteLine($"Fetched Customer: {singleCustomer.Name} - {singleCustomer.Email}");

        // Update customer
        bool updateSuccess = await dbHelper.UpdateCustomerAsync(newCustomerId, "John Updated", "john.new@example.com", "Los Angeles");
        Console.WriteLine(updateSuccess ? "Customer updated successfully!" : "Update failed!");

        // Delete customer
        bool deleteSuccess = await dbHelper.DeleteCustomerAsync(newCustomerId);
        Console.WriteLine(deleteSuccess ? "Customer deleted successfully!" : "Delete failed!");
    }
}
```

## DataReaderExtensions.cs
```
public static class DataReaderExtensions
{
    public static T GetValueOrDefault<T>(SqlDataReader reader, int index)
    {
        return reader.IsDBNull(index) ? default : reader.GetFieldValue<T>(index);
    }

    public static T GetValueOrDefault<T>(this IDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
            return default; // Returns default value for type (null for ref types, 0 for int, etc.)
        
        return (T)reader.GetValue(index);
    }

    public static T GetValueOrDefault<T>(this IDataReader reader, string columnName)
    {
        int index = reader.GetOrdinal(columnName); // Get column index from name
        if (reader.IsDBNull(index))
            return default;

        return (T)reader.GetValue(index);
    }

    public static T? GetNullableValue<T>(this IDataReader reader, string columnName) where T : struct
    {
        int index = reader.GetOrdinal(columnName);
        return reader.IsDBNull(index) ? (T?)null : reader.GetFieldValue<T>(index);
    }
}
```

✅ Summary
🔥 Stored procedures ensure security and performance
🔥 C# ADO.NET ensures efficient execution
🔥 Supports Insert, Select, Update, and Delete operations

Would you like me to convert this into an ASP.NET Core Web API for CRUD operations? 🚀

## Handling Nullable Types (DateTime?, bool?)
```
public static class DataReaderExtensions
{
    public static T? GetNullableValue<T>(this IDataReader reader, string columnName) where T : struct
    {
        int index = reader.GetOrdinal(columnName);
        return reader.IsDBNull(index) ? (T?)null : reader.GetFieldValue<T>(index);
    }
}
```

### Usage:
```
DateTime? lastLogin = reader.GetNullableValue<DateTime>("LastLogin");
bool? isActive = reader.GetNullableValue<bool>("IsActive");
```

## DataWriterExtensions.cs
```
using System;
using System.Data;
using System.Data.SqlClient;

public static class DataWriterExtensions
{
    /// <summary>
    /// Adds a parameter to the SqlCommand, handling null values correctly.
    /// </summary>
    public static void AddParameter(this SqlCommand cmd, string paramName, object value)
    {
        cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
    }

    /// <summary>
    /// Adds a parameter with a specified SQL data type.
    /// </summary>
    public static void AddParameter(this SqlCommand cmd, string paramName, SqlDbType dbType, object value)
    {
        var param = cmd.Parameters.Add(paramName, dbType);
        param.Value = value ?? DBNull.Value;
    }

    /// <summary>
    /// Adds an output parameter.
    /// </summary>
    public static void AddOutputParameter(this SqlCommand cmd, string paramName, SqlDbType dbType)
    {
        var param = cmd.Parameters.Add(paramName, dbType);
        param.Direction = ParameterDirection.Output;
    }
}
```

### Usage:
```
public async Task<int> InsertCustomerAsync(string name, string email, string city)
{
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("InsertCustomer", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.AddParameter("@Name", name);
            cmd.AddParameter("@Email", email);
            cmd.AddParameter("@City", city);
            cmd.AddOutputParameter("@NewId", SqlDbType.Int); // Output parameter example

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)cmd.Parameters["@NewId"].Value; // Return new ID
        }
    }
}
```
