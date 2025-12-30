# MySQL Connection Test Script
# Replace YOUR_PASSWORD with your actual MySQL password

$password = "YOUR_PASSWORD"  # REPLACE THIS
$server = "localhost"
$user = "root"

Write-Host "Testing MySQL Connection..." -ForegroundColor Cyan

# Test 1: Check if MySQL is accessible via mysql.exe
Write-Host "`n1. Checking if mysql.exe is available..." -ForegroundColor Yellow
$mysqlPath = Get-Command mysql -ErrorAction SilentlyContinue
if ($mysqlPath) {
    Write-Host "   Found: $($mysqlPath.Source)" -ForegroundColor Green
    
    # Test connection and list databases
    Write-Host "`n2. Testing connection and listing databases..." -ForegroundColor Yellow
    Write-Host "   Running: mysql -h $server -u $user -p$password -e 'SHOW DATABASES;'" -ForegroundColor Gray
    
    $result = & mysql -h $server -u $user -p$password -e "SHOW DATABASES;" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   Connection successful!" -ForegroundColor Green
        Write-Host "`n   Available databases:" -ForegroundColor Green
        $result | ForEach-Object { Write-Host "   $_" }
        
        # Check if ToDoDB exists
        Write-Host "`n3. Checking if 'ToDoDB' exists..." -ForegroundColor Yellow
        if ($result -match "ToDoDB") {
            Write-Host "   ✓ ToDoDB database EXISTS" -ForegroundColor Green
            
            # Check tables in ToDoDB
            Write-Host "`n4. Checking tables in ToDoDB..." -ForegroundColor Yellow
            $tables = & mysql -h $server -u $user -p$password -D ToDoDB -e "SHOW TABLES;" 2>&1
            $tables | ForEach-Object { Write-Host "   $_" }
            
            if ($tables -match "Items") {
                Write-Host "`n   ✓ Items table EXISTS" -ForegroundColor Green
            } else {
                Write-Host "`n   ✗ Items table NOT FOUND" -ForegroundColor Red
            }
        } else {
            Write-Host "   ✗ ToDoDB database NOT FOUND" -ForegroundColor Red
        }
    } else {
        Write-Host "   Connection FAILED!" -ForegroundColor Red
        Write-Host "   Error: $result" -ForegroundColor Red
        Write-Host "`n   Possible issues:" -ForegroundColor Yellow
        Write-Host "   - MySQL server is not running" -ForegroundColor Yellow
        Write-Host "   - Wrong password" -ForegroundColor Yellow
        Write-Host "   - User 'root' doesn't have access from 'localhost'" -ForegroundColor Yellow
    }
} else {
    Write-Host "   mysql.exe NOT FOUND in PATH" -ForegroundColor Red
    Write-Host "`n   Please check:" -ForegroundColor Yellow
    Write-Host "   - Is MySQL installed?" -ForegroundColor Yellow
    Write-Host "   - Is MySQL bin folder in your PATH?" -ForegroundColor Yellow
    Write-Host "   - Common locations:" -ForegroundColor Yellow
    Write-Host "     C:\Program Files\MySQL\MySQL Server 8.0\bin" -ForegroundColor Gray
    Write-Host "     C:\xampp\mysql\bin" -ForegroundColor Gray
}

Write-Host "`n" -ForegroundColor Cyan
