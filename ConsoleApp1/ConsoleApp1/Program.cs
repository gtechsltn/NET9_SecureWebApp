using System;

namespace ConsoleApp1
{
    internal class Program
    {
        /// <summary>
        /// 🎯 Summary: Best Practices
        ///   ✅ Avoid throw new Exception() – Use specific exceptions
        ///   ✅ Preserve stack trace – Use throw;, not throw ex;
        ///   ✅ Use custom exceptions for domain-specific errors
        ///   ✅ Use ArgumentException for invalid method parameters
        ///   ✅ Log exceptions before rethrowing them
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.Exception">
        /// Something went wrong!
        /// or
        /// An error occurred while processing.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Invalid operation occurred.</exception>
        private static void Main(string[] args)
        {
            // Basic Exception Throwing
            // ====================================================================================================
            throw new Exception("Something went wrong!");

            // ✅ Better approach with InvalidOperationException
            // ====================================================================================================
            throw new InvalidOperationException("Invalid operation occurred.");

            // Using throw new Exception() with Inner Exception
            // ====================================================================================================
            try
            {
                int result = 10 / int.Parse("0"); // Will throw DivideByZeroException
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing.", ex);
            }

            // ❌ Bad Practice(throw ex;)
            // ====================================================================================================
            try
            {
                // Some code
            }
            catch (Exception ex)
            {
                throw ex; // ❌ This resets the stack trace!
            }

            // ====================================================================================================
            // 🔴 Why is this bad?
            // It resets the stack trace, losing the original error location.
            // ====================================================================================================

            // ✅ Correct Way (throw;)
            // ====================================================================================================
            try
            {
                // Some code
            }
            catch (Exception ex)
            {
                throw; // ✅ Preserves original stack trace
            }

            // ====================================================================================================
            // ✅ Keeps the original stack trace intact!
            // ====================================================================================================

            // Try-Catch with Logging (Best Practice)
            // ====================================================================================================
            try
            {
                // Some operation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw;
            }

            // ✅ Ensures error is logged before rethrowing.
        }

        /// <summary>
        /// Throwing ArgumentException for Invalid Parameters
        /// </summary>
        /// <param name="age">The age.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">age - Age cannot be negative.</exception>
        private void SetAge(int age)
        {
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
        }

        /// <summary>
        /// Using NotImplementedException and NotSupportedException
        ///   🔹 For methods that are not yet implemented:
        /// </summary>
        /// <exception cref="System.NotImplementedException">This feature is not implemented yet.</exception>
        public void MyFutureFeature()
        {
            throw new NotImplementedException("This feature is not implemented yet.");
        }

        /// <summary>
        /// Using NotImplementedException and NotSupportedException
        ///   🔹 For methods that should never be called:
        /// </summary>
        /// <exception cref="System.NotSupportedException">This feature is no longer supported.</exception>
        public void LegacyFeature()
        {
            throw new NotSupportedException("This feature is no longer supported.");
        }
    }

    public class InsufficientFundsException : Exception
    {
        public decimal CurrentBalance { get; }
        public decimal WithdrawAmount { get; }

        public InsufficientFundsException(string message, decimal currentBalance, decimal withdrawAmount)
            : base(message)
        {
            CurrentBalance = currentBalance;
            WithdrawAmount = withdrawAmount;
        }
    }

    public class BankAccount
    {
        public decimal Balance { get; private set; } = 100;

        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
                throw new InsufficientFundsException($"Cannot withdraw {amount}. Available balance: {Balance}", Balance, amount);

            Balance -= amount;
        }
    }
}