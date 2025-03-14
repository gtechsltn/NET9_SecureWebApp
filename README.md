# Secure Your .NET Web App in .NET 9
* 6: Native AOT
* 5: OpenID Connect and OAuth 2.0
* 4: OAuth (Open Authorization)
* 3: JWT (JSON Web Token)
* 2: API Key authorization
* 1: Basic Authentication

* Role-Based Access Control (RBAC)
    + ✅ Best for: Applications with predefined user roles (e.g., Admin, Manager, User).
    + ✅ RBAC assigns users to specific roles, granting access based on their assigned role. It’s simple to implement and widely used.
* Claims-Based Authorization
    + ✅ Best for: Scenarios where access is based on user attributes (e.g., department, clearance level).
    + ✅ Claims-based authorization allows defining user claims (key-value pairs) to control access dynamically.
    + ✅ Claims are typically stored in JWT tokens or identity providers.
* Policy-Based Authorization
    + ✅ Best for: Applications with complex access rules beyond roles and claims.
    + ✅ Policies define rules using claims, roles, or custom logic.
* Attribute-Based Authorization (Custom Handlers)
    + ✅ Best for: Advanced scenarios requiring custom authorization logic.
    + ✅ This method uses custom attributes and handlers to control access dynamically.

![Authorization Mechanism](https://github.com/user-attachments/assets/df75377f-6b2f-48a4-aeee-4bbcf1bf0ffa)

# Multi-Factor Authentication (MFA) in .NET Applications

## What is MFA?

Multi-Factor Authentication (MFA) is a security mechanism that requires users to provide two or more forms of verification before gaining access to an application. This significantly enhances security by reducing the risk of unauthorized access due to stolen passwords.

## Why Use MFA in .NET Applications?
🔹 Protects against credential theft and phishing attacks.
🔹 Strengthens access control by requiring multiple verification factors.
🔹 Ensures compliance with security standards (e.g., GDPR, HIPAA).

## Common MFA Methods
+ ✅ Something You Know – Password, PIN
+ ✅ Something You Have – OTP (One-Time Password) via SMS, Email, Authenticator App
+ ✅ Something You Are – Biometric verification (fingerprint, facial recognition)

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
* 1️⃣ Create a SQL Server table
* 2️⃣ Create stored procedures for CRUD operations
* 3️⃣ Write C# code to call these procedures using ADO.NET

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
+ 🔥 Stored procedures ensure security and performance
+ 🔥 C# ADO.NET ensures efficient execution
+ 🔥 Supports Insert, Select, Update, and Delete operations

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

### Usage of DataReaderExtensions:
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

### Usage of DataWriterExtensions:
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

## Updated DataWriterExtensions with VARBINARY(MAX) Support
```
using System;
using System.Data;
using System.Data.SqlClient;

public static class DataWriterExtensions
{
    /// <summary>
    /// Adds multiple parameters at once to a SqlCommand using a params array.
    /// </summary>
    public static void AddParameters(this SqlCommand cmd, params (string paramName, object value)[] parameters)
    {
        foreach (var (paramName, value) in parameters)
        {
            cmd.AddParameter(paramName, value);
        }
    }

    /// <summary>
    /// Adds a single parameter to the SqlCommand, handling null and VARBINARY values correctly.
    /// </summary>
    public static void AddParameter(this SqlCommand cmd, string paramName, object value)
    {
        if (value is byte[] byteArray)  // Handle VARBINARY(MAX) properly
        {
            cmd.Parameters.Add(paramName, SqlDbType.VarBinary, -1).Value = byteArray ?? DBNull.Value;
        }
        else
        {
            cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
        }
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
    /// Adds a VARBINARY(MAX) parameter.
    /// </summary>
    public static void AddVarBinaryParameter(this SqlCommand cmd, string paramName, byte[] value)
    {
        cmd.Parameters.Add(paramName, SqlDbType.VarBinary, -1).Value = value ?? DBNull.Value;
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

## Insert Example (Storing Image in Database)
```
public async Task InsertFileAsync(string fileName, byte[] fileData)
{
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("InsertFile", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            // Using variadic method
            cmd.AddParameters(
                ("@FileName", fileName),
                ("@FileData", fileData) // VARBINARY(MAX) automatically handled
            );

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
```

## Reading VARBINARY(MAX) (Retrieving File)
```
public async Task<(string fileName, byte[] fileData)> GetFileAsync(int fileId)
{
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("GetFile", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.AddParameter("@FileId", fileId);

            await conn.OpenAsync();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return (
                        reader.GetValueOrDefault<string>("FileName"),
                        reader.IsDBNull("FileData") ? null : (byte[])reader["FileData"]
                    );
                }
            }
        }
    }
    return (null, null); // No file found
}
```

## Efficiently Reading VARBINARY(MAX) for Large Files (1GB+)

**Optimized Method to Read Large Files**

```
public async Task ReadLargeFileAsync(int fileId, string outputFilePath)
{
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("GetLargeFile", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.AddParameter("@FileId", fileId);

            await conn.OpenAsync();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess)) // Stream mode
            {
                if (await reader.ReadAsync())
                {
                    string fileName = reader.GetValueOrDefault<string>("FileName");

                    using (Stream fileStream = File.Create(outputFilePath))
                    using (Stream dbStream = reader.GetStream(reader.GetOrdinal("FileData")))
                    {
                        await dbStream.CopyToAsync(fileStream); // Efficient streaming
                    }

                    Console.WriteLine($"File '{fileName}' saved to {outputFilePath}");
                }
            }
        }
    }
}
```

### ✅ How It Works
* Use CommandBehavior.SequentialAccess → Prevents memory overload by streaming data.
* reader.GetStream() → Reads VARBINARY(MAX) as a stream instead of byte[].
* CopyToAsync() → Streams data directly to a file without using excessive RAM.

## 📌 Why This is Better
* ✔️ Handles large files (1GB+) without memory issues
* ✔️ Efficient streaming to file storage
* ✔️ Prevents OutOfMemoryException

## Efficiently Writing VARBINARY(MAX) for Large Files (1GB+) in SQL Server

When writing large files (e.g., 1GB+) into SQL Server, we need to stream the file in chunks to prevent memory overload.

**Optimized Method to Insert Large Files**

Instead of loading the entire file into memory (byte[]), we use SqlParameter with Stream.

```
public async Task InsertLargeFileAsync(string filePath)
{
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        using (SqlCommand cmd = new SqlCommand("InsertLargeFile", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.AddParameter("@FileName", Path.GetFileName(filePath));

            // Open file as stream
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Create SQL parameter for VARBINARY(MAX)
                SqlParameter fileParam = new SqlParameter("@FileData", SqlDbType.VarBinary, -1)
                {
                    Value = fileStream // Assign stream instead of byte[]
                };

                cmd.Parameters.Add(fileParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
```

### Stored Procedure for VARBINARY(MAX)
```
CREATE PROCEDURE InsertLargeFile
    @FileName NVARCHAR(255),
    @FileData VARBINARY(MAX)
AS
BEGIN
    INSERT INTO FileStorage (FileName, FileData)
    VALUES (@FileName, @FileData);
END
```

### Why This is Better
* ✔️ Streams directly from disk to DB (no huge byte[] in memory)
* ✔️ Prevents OutOfMemoryException for 1GB+ files
* ✔️ Optimized for performance and large datasets

# Preventing OutOfMemoryException in .NET C# Console Apps

Handling large files (1GB+) in a memory-efficient way is critical to avoid OutOfMemoryException. Here are the best strategies:

## 1. Stream Large Files Instead of Using byte[]

### Avoid
```
byte[] fileData = File.ReadAllBytes(filePath); // ❌ BAD: Loads entire file into memory
cmd.Parameters.AddWithValue("@FileData", fileData);
```

### Use Streaming Instead:
```
using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    SqlParameter param = new SqlParameter("@FileData", SqlDbType.VarBinary, -1)
    {
        Value = fs // ✅ Stream instead of byte[]
    };
    cmd.Parameters.Add(param);
}
```

## 2. Read Large VARBINARY(MAX) from SQL Server Efficiently

### Avoid
```
byte[] fileData = (byte[])reader["FileData"]; // ❌ BAD: Loads entire file into memory
```

### Use Streaming Instead:
```
using (Stream dbStream = reader.GetStream(reader.GetOrdinal("FileData")))
using (FileStream fileStream = File.Create(outputPath))
{
    await dbStream.CopyToAsync(fileStream); // ✅ Efficient streaming
}
```

## 3. Process Large Files in Chunks (HAY HAY HAY)
* ✅ Use Buffered Reading Instead of ReadAllText() or ReadAllBytes()
```
using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
using (BufferedStream bs = new BufferedStream(fs, 8192)) // ✅ Buffered read
using (StreamReader sr = new StreamReader(bs))
{
    while (!sr.EndOfStream)
    {
        string line = await sr.ReadLineAsync();
        // Process each line without loading everything into memory
    }
}
```
* ✅ Advantage: Uses small buffer sizes to reduce memory footprint.

## 4. Use GC.Collect() Cautiously
* If you deal with temporary objects, force garbage collection:
```
GC.Collect();
GC.WaitForPendingFinalizers();
```
* ✅ Advantage: Clears unused memory, but only use it sparingly.

## 5. Increase Memory Limits for x86 Apps
* If running a 32-bit app, you can increase memory limits by enabling LARGEADDRESSAWARE:
* Build as x64 (preferred).
* ✅ Advantage: Allows more than 2GB memory for 32-bit applications.

## 🚀 Summary: Best Practices to Prevent OutOfMemoryException
* ✅ Use Stream instead of byte[] for large files
* ✅ Process files in chunks (e.g., BufferedStream, StreamReader)
* ✅ Use reader.GetStream() to read VARBINARY(MAX) efficiently
* ✅ Force garbage collection if needed (GC.Collect())
* ✅ Use x64 architecture to handle larger memory allocation

# View All Log Files in a Proper Way

**To list, open, and read log files, use this approach:**

* ✅ C# Code to Read All Log Files
```
using System;
using System.IO;

class LogViewer
{
    static void Main()
    {
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        if (!Directory.Exists(logDirectory))
        {
            Console.WriteLine("Log directory not found!");
            return;
        }

        Console.WriteLine("Available Log Files:");
        var logFiles = Directory.GetFiles(logDirectory, "*.log");

        for (int i = 0; i < logFiles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileName(logFiles[i])}");
        }

        Console.Write("\nEnter file number to view (or 0 to exit): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= logFiles.Length)
        {
            string selectedFile = logFiles[choice - 1];
            Console.WriteLine($"\nReading {Path.GetFileName(selectedFile)}...\n");

            using (StreamReader reader = new StreamReader(selectedFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
```

* ✔️ Lists all log files in the Logs/ directory.
* ✔️ Allows users to select and read a log file.
* ✔️ Prevents memory issues by reading logs line-by-line instead of loading everything at once.

## Use a Log Viewer for Better Visualization
* 🔹 Option 1: Use Notepad++ or Visual Studio Code
    + Open the logs in Notepad++ or VS Code.
    + Enable auto-refresh to see logs update in real-time.
* 🔹 Option 2: Use a Real-Time Tail Viewer
    + LogExpert (Free and lightweight)
    + BareTail (Great for tailing logs)
    + tail -f (Linux/Mac) → View logs updating live.

# RealTimeLogViewer.cs in .NET (C#)
```
using System;
using System.IO;

class RealTimeLogViewer
{
    static void Main()
    {
        string logFile = "Logs/LogFile.log"; // Adjust to latest log file
        if (!File.Exists(logFile))
        {
            Console.WriteLine("Log file not found!");
            return;
        }

        using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (StreamReader reader = new StreamReader(fs))
        {
            Console.WriteLine("Real-time Log Viewer started...\n");

            while (true)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }

                System.Threading.Thread.Sleep(500); // Small delay to avoid CPU overload
            }
        }
    }
}
```

## Configure Log4Net to Store Logs in Files

Example log4net.config (Rolling File Appender)

```
<configuration>
  <log4net>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="Logs/LogFile.log" />
      <appendToFile value="true" />
      <rollingStyle value="Date" />
      <datePattern value="yyyyMMdd'.log'" />
      <staticLogFileName value="false" />
      <maxSizeRollBackups value="10" />
      <maximumFileSize value="10MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>

    <root>
      <level value="DEBUG" />
      <appender-ref ref="RollingFileAppender" />
    </root>
  </log4net>
</configuration>
```

* ✔️ Logs will be stored in the Logs/ folder with daily rotation.
* ✔️ Old logs are automatically managed (maxSizeRollBackups="10" limits to 10 files).
* 
