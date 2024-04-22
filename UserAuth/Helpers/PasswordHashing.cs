using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace UserAuth.Helpers
{
	public class PasswordHashing
	{
		private const string PasswordRegexPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).{6,}$";

		public static string HashPassword(string password)
		{
			// Generate salt
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			// Create the Rfc2898DeriveBytes object
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

			// Hash the password
			byte[] hash = pbkdf2.GetBytes(20);

			// Combine the salt and password hash
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			// Convert to base64
			string hashedPassword = Convert.ToBase64String(hashBytes);

			return hashedPassword;
		}

		public static bool VerifyPassword(string password, string hashedPassword)
		{
			// Extract the salt from the hashed password
			byte[] hashBytes = Convert.FromBase64String(hashedPassword);
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);

			// Compute the hash of the given password using the same salt
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
			byte[] hash = pbkdf2.GetBytes(20);

			// Compare the computed hash with the hashed password
			for (int i = 0; i < 20; i++)
			{
				if (hashBytes[i + 16] != hash[i])
					return false;
			}

			return true;
		}

		public static bool IsPasswordComplex(string password)
		{
			return Regex.IsMatch(password, PasswordRegexPattern);
		}
	}
}
