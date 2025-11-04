
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Models;

namespace User.Services
{
    public class SQLiteHelper
    {
        private readonly string dbPath;

        public SQLiteHelper()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "chenna.db");
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserName TEXT NOT NULL,
                    MobileNumber TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    PhotoPath TEXT
                );";
            command.ExecuteNonQuery(); // ✅ Execute the table creation

            command.CommandText = @"
               CREATE TABLE IF NOT EXISTS InstallationRecord (
                            ID Text PRIMARY KEY,
                            UserName TEXT,
                            CustomerName TEXT,
                            ContactNumber TEXT,
                            Email TEXT,
                            InstallDate TEXT,
                            Pincode TEXT,
                            Locality TEXT,
                            Address TEXT,
                            City TEXT,
                            SelectedState TEXT,
                            Landmark TEXT,
                            AlternatePhone TEXT,
                            IsHome INTEGER,
                            IsWork INTEGER,
                            InternetConnected INTEGER,
                            MobileAppConfigured INTEGER,
                            AppName TEXT,
                            ViewOnAndroid INTEGER,
                            ViewOniPhone INTEGER,
                            ViewOnDesktop INTEGER,
                            InstalledChecked INTEGER,
                            DemoReceived INTEGER,
                            UnderstoodOperation INTEGER,
                            CustomerSign TEXT,
                            TechnicianSign TEXT
                                );";
            command.ExecuteNonQuery();


            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS UpgradeSystems (
                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerName TEXT NOT NULL,
                    MobileNumber TEXT,
                    Address TEXT,
                    ExistingSystemType TEXT,
                    ExistingCameras INTEGER,
                    NewCameras INTEGER,
                    PreferredUpgradeDate TEXT,
                    Notes TEXT
                            );";
            command.ExecuteNonQuery();
        }

        public bool LoginUser(string username, string password)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE UserName = @username AND Password = @password";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            var result = command.ExecuteScalar();
            long count = result != null ? Convert.ToInt64(result) : 0;
            return count > 0;
        }

        public void RegisterUser(string username, string mobile, string email, string password, string photoPath = "")
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (UserName, MobileNumber, Email, Password, PhotoPath)
                VALUES (@Username, @mobile, @email, @password, @photoPath)";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@mobile", mobile);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@photoPath", photoPath ?? string.Empty);
            command.ExecuteNonQuery();
        }

        public void newInstallation(
    string customerName,
    string contactNumber,
    string email,
    DateTime installDate,
    string pincode,
    string locality,
    string address,
    string city,
    string selectedState,
    string landmark,
    string alternatePhone,
    bool isHome,
    bool isWork,
    bool internetConnected,
    bool mobileAppConfigured,
    string appName,
    bool viewOnAndroid,
    bool viewOniPhone,
    bool viewOnDesktop,
    bool installedChecked,
    bool demoReceived,
    bool understoodOperation,
    string customerSign,
    string technicianSign)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            // ✅ Use the parameter (don’t re-declare it)
          String  userName = Preferences.Get("LoggedInUsername", string.Empty);

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO InstallationRecord (
            ID, UserName, CustomerName, ContactNumber, Email, InstallDate,
            Pincode, Locality, Address, City, SelectedState,
            Landmark, AlternatePhone, IsHome, IsWork,
            InternetConnected, MobileAppConfigured, AppName,
            ViewOnAndroid, ViewOniPhone, ViewOnDesktop,
            InstalledChecked, DemoReceived, UnderstoodOperation,
            CustomerSign, TechnicianSign
        )
        VALUES (
            'SR' || printf('%04d',
                COALESCE(
                    (SELECT CAST(substr(ID, 3) AS INTEGER) + 1 
                     FROM InstallationRecord 
                     ORDER BY ID DESC LIMIT 1),
                    1
                )
            ),
            @UserName,
            @CustomerName,
            @ContactNumber,
            @Email,
            @InstallDate,
            @Pincode,
            @Locality,
            @Address,
            @City,
            @SelectedState,
            @Landmark,
            @AlternatePhone,
            @IsHome,
            @IsWork,
            @InternetConnected,
            @MobileAppConfigured,
            @AppName,
            @ViewOnAndroid,
            @ViewOniPhone,
            @ViewOnDesktop,
            @InstalledChecked,
            @DemoReceived,
            @UnderstoodOperation,
            @CustomerSign,
            @TechnicianSign
        );";

            command.Parameters.AddWithValue("@UserName", userName ?? string.Empty);
            command.Parameters.AddWithValue("@CustomerName", customerName ?? string.Empty);
            command.Parameters.AddWithValue("@ContactNumber", contactNumber ?? string.Empty);
            command.Parameters.AddWithValue("@Email", email ?? string.Empty);
            command.Parameters.AddWithValue("@InstallDate", installDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Pincode", pincode ?? string.Empty);
            command.Parameters.AddWithValue("@Locality", locality ?? string.Empty);
            command.Parameters.AddWithValue("@Address", address ?? string.Empty);
            command.Parameters.AddWithValue("@City", city ?? string.Empty);
            command.Parameters.AddWithValue("@SelectedState", selectedState ?? string.Empty);
            command.Parameters.AddWithValue("@Landmark", landmark ?? string.Empty);
            command.Parameters.AddWithValue("@AlternatePhone", alternatePhone ?? string.Empty);
            command.Parameters.AddWithValue("@IsHome", isHome ? 1 : 0);
            command.Parameters.AddWithValue("@IsWork", isWork ? 1 : 0);
            command.Parameters.AddWithValue("@InternetConnected", internetConnected ? 1 : 0);
            command.Parameters.AddWithValue("@MobileAppConfigured", mobileAppConfigured ? 1 : 0);
            command.Parameters.AddWithValue("@AppName", appName ?? string.Empty);
            command.Parameters.AddWithValue("@ViewOnAndroid", viewOnAndroid ? 1 : 0);
            command.Parameters.AddWithValue("@ViewOniPhone", viewOniPhone ? 1 : 0);
            command.Parameters.AddWithValue("@ViewOnDesktop", viewOnDesktop ? 1 : 0);
            command.Parameters.AddWithValue("@InstalledChecked", installedChecked ? 1 : 0);
            command.Parameters.AddWithValue("@DemoReceived", demoReceived ? 1 : 0);
            command.Parameters.AddWithValue("@UnderstoodOperation", understoodOperation ? 1 : 0);
            command.Parameters.AddWithValue("@CustomerSign", customerSign ?? string.Empty);
            command.Parameters.AddWithValue("@TechnicianSign", technicianSign ?? string.Empty);

            command.ExecuteNonQuery();
        }




        public void ServiceUpgradeSystem(
                    string customerName,
                    string mobileNumber,
                    string address,
                    string existingSystemType,
                    int existingCameras,
                    int newCameras,
                    DateTime preferredUpgradeDate,
                    string notes)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                 INSERT INTO UpgradeSystems (
                        CustomerName,
                        MobileNumber,
                        Address,
                        ExistingSystemType,
                        ExistingCameras,
                        NewCameras,
                        PreferredUpgradeDate,
                        Notes
                        ) VALUES (
                        @CustomerName,
                        @MobileNumber,
                        @Address,
                        @ExistingSystemType,
                        @ExistingCameras,
                        @NewCameras,
                        @PreferredUpgradeDate,
                        @Notes
                                );";

            command.Parameters.AddWithValue("@CustomerName", customerName);
            command.Parameters.AddWithValue("@MobileNumber", mobileNumber);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@ExistingSystemType", existingSystemType);
            command.Parameters.AddWithValue("@ExistingCameras", existingCameras);
            command.Parameters.AddWithValue("@NewCameras", newCameras);
            command.Parameters.AddWithValue("@PreferredUpgradeDate", preferredUpgradeDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Notes", notes);

            command.ExecuteNonQuery();
        }


        // ✅ Get all installations (RAW SQL)
        public async Task<List<NewInstallationModel>> GetInstallationsByUserAsync(string userName)
        {
            var installations = new List<NewInstallationModel>();

            await Task.Run(() =>
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                // ✅ Create command first
                var command = connection.CreateCommand();
                command.CommandText = @"
            SELECT ID, UserName, CustomerName, City, InstallDate 
            FROM InstallationRecord
            WHERE UserName = @UserName
            ORDER BY InstallDate DESC;";

                // ✅ Add parameter after command is declared
                command.Parameters.AddWithValue("@UserName", userName ?? string.Empty);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    installations.Add(new NewInstallationModel
                    {
                        Id = reader["ID"]?.ToString() ?? string.Empty,
                        UserName = reader["UserName"]?.ToString() ?? string.Empty,
                        CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                        City = reader["City"]?.ToString() ?? string.Empty,
                        InstallDate = reader["InstallDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["InstallDate"]).ToString("yyyy-MM-dd")
                            : string.Empty
                    });
                }
            });

            return installations;
        }


        public async Task<NewInstallationModel> GetInstallationByIdAsync(string id)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM InstallationRecord WHERE ID = @Id";
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new NewInstallationModel
                {
                    Id = reader["ID"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    CustomerName = reader["CustomerName"].ToString(),
                    ContactNumber = reader["ContactNumber"].ToString(),
                    Email = reader["Email"].ToString(),
                    InstallDate = reader["InstallDate"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    Locality = reader["Locality"].ToString(),
                    Address = reader["Address"].ToString(),
                    City = reader["City"].ToString(),
                    SelectedState = reader["SelectedState"].ToString(),
                    Landmark = reader["Landmark"].ToString(),
                    AlternatePhone = reader["AlternatePhone"].ToString(),
                    IsHome = Convert.ToInt32(reader["IsHome"]) == 1,
                    IsWork = Convert.ToInt32(reader["IsWork"]) == 1,
                    InternetConnected = Convert.ToInt32(reader["InternetConnected"]) == 1,
                    MobileAppConfigured = Convert.ToInt32(reader["MobileAppConfigured"]) == 1,
                    AppName = reader["AppName"].ToString(),
                    ViewOnAndroid = Convert.ToInt32(reader["ViewOnAndroid"]) == 1,
                    ViewOniPhone = Convert.ToInt32(reader["ViewOniPhone"]) == 1,
                    ViewOnDesktop = Convert.ToInt32(reader["ViewOnDesktop"]) == 1,
                    InstalledChecked = Convert.ToInt32(reader["InstalledChecked"]) == 1,
                    DemoReceived = Convert.ToInt32(reader["DemoReceived"]) == 1,
                    UnderstoodOperation = Convert.ToInt32(reader["UnderstoodOperation"]) == 1,
                    CustomerSign = reader["CustomerSign"].ToString(),
                    TechnicianSign = reader["TechnicianSign"].ToString()
                };
            }

            return null;
        }

        public async Task UpdateInstallationAsync(
            string id, string customerName, string contactNumber, string email, DateTime installDate,
            string pincode, string locality, string address, string city, string selectedState,
            string landmark, string alternatePhone, bool isHome, bool isWork, bool internetConnected,
            bool mobileAppConfigured, string appName, bool viewOnAndroid, bool viewOniPhone,
            bool viewOnDesktop, bool installedChecked, bool demoReceived, bool understoodOperation,
            string customerSign, string technicianSign)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE InstallationRecord SET
            CustomerName = @CustomerName,
            ContactNumber = @ContactNumber,
            Email = @Email,
            InstallDate = @InstallDate,
            Pincode = @Pincode,
            Locality = @Locality,
            Address = @Address,
            City = @City,
            SelectedState = @SelectedState,
            Landmark = @Landmark,
            AlternatePhone = @AlternatePhone,
            IsHome = @IsHome,
            IsWork = @IsWork,
            InternetConnected = @InternetConnected,
            MobileAppConfigured = @MobileAppConfigured,
            AppName = @AppName,
            ViewOnAndroid = @ViewOnAndroid,
            ViewOniPhone = @ViewOniPhone,
            ViewOnDesktop = @ViewOnDesktop,
            InstalledChecked = @InstalledChecked,
            DemoReceived = @DemoReceived,
            UnderstoodOperation = @UnderstoodOperation,
            CustomerSign = @CustomerSign,
            TechnicianSign = @TechnicianSign
        WHERE ID = @Id";

            command.Parameters.AddWithValue("@CustomerName", customerName);
            command.Parameters.AddWithValue("@ContactNumber", contactNumber);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@InstallDate", installDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Pincode", pincode);
            command.Parameters.AddWithValue("@Locality", locality);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@City", city);
            command.Parameters.AddWithValue("@SelectedState", selectedState);
            command.Parameters.AddWithValue("@Landmark", landmark);
            command.Parameters.AddWithValue("@AlternatePhone", alternatePhone);
            command.Parameters.AddWithValue("@IsHome", isHome ? 1 : 0);
            command.Parameters.AddWithValue("@IsWork", isWork ? 1 : 0);
            command.Parameters.AddWithValue("@InternetConnected", internetConnected ? 1 : 0);
            command.Parameters.AddWithValue("@MobileAppConfigured", mobileAppConfigured ? 1 : 0);
            command.Parameters.AddWithValue("@AppName", appName);
            command.Parameters.AddWithValue("@ViewOnAndroid", viewOnAndroid ? 1 : 0);
            command.Parameters.AddWithValue("@ViewOniPhone", viewOniPhone ? 1 : 0);
            command.Parameters.AddWithValue("@ViewOnDesktop", viewOnDesktop ? 1 : 0);
            command.Parameters.AddWithValue("@InstalledChecked", installedChecked ? 1 : 0);
            command.Parameters.AddWithValue("@DemoReceived", demoReceived ? 1 : 0);
            command.Parameters.AddWithValue("@UnderstoodOperation", understoodOperation ? 1 : 0);
            command.Parameters.AddWithValue("@CustomerSign", customerSign);
            command.Parameters.AddWithValue("@TechnicianSign", technicianSign);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

    }

}
