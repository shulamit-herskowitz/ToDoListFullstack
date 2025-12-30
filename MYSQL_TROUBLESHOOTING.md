# MySQL Troubleshooting Guide for TodoAPI

## ✅ Step 1: Check MySQL Service Status

Run this command in PowerShell:
```powershell
Get-Service | Where-Object {$_.DisplayName -like "*mysql*" -or $_.Name -like "*mysql*"}
```

**If MySQL service is found but stopped:**
```powershell
Start-Service -Name "MySQL80"  # Replace with your service name
```

**Alternative - Check MySQL process:**
```powershell
Get-Process -Name "*mysql*" -ErrorAction SilentlyContinue
```

---

## ✅ Step 2: Verify MySQL Connection & Database

**Option A: Run the test script**
1. Edit `TestMySQLConnection.ps1` and replace `YOUR_PASSWORD` with your actual password
2. Run: `.\TestMySQLConnection.ps1`

**Option B: Manual command line test**
```powershell
# Txxx xxxxxction
mysql -h localhost -u root -p -e "SHOW DATABASES;"

# Check if ToDoDB exists
mysql -h localhost -u root -p -e "SHOW DATABASES;" | Select-String "ToDoDB"

# Check tables in ToDoDB
mysql -h localhost -u root -p -D ToDoDB -e "SHOW TABLES;"
```

---

## ✅ Step 3: Create Database & Table

**In MySQL Workbench:**
1. Open MySQL Workbench
2. Connect to your server (localhost, root, your password)
3. Open `CreateToDoDatabase.sql`
4. Execute the script (⚡ lightning bolt icon or Ctrl+Shift+Enter)

**OR via Command Line:**
```powershell
mysql -h localhost -u root -p < CreateToDoDatabase.sql
```

**OR manually:**
```sql
CREATE DATABASE IF NOT EXISTS ToDoDB;
USE ToDoDB;

CREATE TABLE IF NOT EXISTS Items (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    IsComplete TINYINT(1) NOT NULL DEFAULT 0
);
```

---

## ✅ Step 4: dotnet-ef PATH (COMPLETED)

The PATH has been permanently added! You can now use `dotnet-ef` in any new PowerShell session.

**Verify it works:**
```powershell
dotnet-ef --version
```

Should show: Entity Framework Core .NET Command-line Tools 9.0.0

---

## 📋 Step 5: Run the Scaffold Command

Once MySQL is running and the database is created, run:

```powershell
dotnet ef dbcontext scaffold "server=localhost;user=root;password=YOUR_PASSWORD;database=ToDoDB" Pomelo.EntityFrameworkCore.MySql --output-dir Models --context-dir Data --context ToDoDbContext --force
```

**Remember to:**
- Replace `YOUR_PASSWORD` with your actual MySQL password
- The `--force` flag will overwrite files if they already exist

---

## 🔧 Common Issues

### Issue: "Unable xx xxxxxct to any of the specified MySQL hosts"
**Solutions:**
1. MySQL server is not running → Start the service
2. Wrong password → Check your credentials
3. Database doesn't exist → Run the SQL script

### Issue: "Access denied for user 'root'@'localhost'"
**Solution:** Wrong password or user doesn't have permissions
```sql
-- Grant permissions (run as MySQL admin)
GRANT ALL PRIVILEGES ON ToDoDB.* TO 'root'@'localhost';
FLUSH PRIVILEGES;
```

### Issue: "Unknown database 'ToDoDB'"
**Solution:** Database doesn't exist → Run `CreateToDoDatabase.sql`

---

## 📁 Files Created

- ✅ `appsettings.json` - Updated with connection string
- ✅ `TestMySQLConnection.ps1` - Test your MySQL connection
- ✅ `CreateToDoDatabase.sql` - Create database and table
- ✅ `AddDotnetToolsToPath.ps1` - Already executed successfully

---

## Next Steps

1. ☐ Start MySQL server (if not running)
2. ☐ Run `TestMySQLConnection.ps1` (update password first)
3. ☐ Run `CreateToDoDatabase.sql` in MySQL Workbench
4. ☐ Run the scaffold command with your actual password
5. ☐ Start building your API!
