# Loan Management System
## სასესხო სისტემის REST API, დაწერილი ASP.NET Core-ით.

## ტექნოლოგიები:
### .NET 10
### ASP.NET Core Web API
### Entity Framework Core 9
### SQL Server LocalDB
### Swagger UI

## გაშვება:
### 1. პროექტის გადმოტანა: git clone https://github.com/NikitaStepnadze/LoanManagementSystem.git
### 2. Editor-ში გახსნა: გახსენით LoanManagementSystem.sln ფაილი
### 3. მონაცემთა ბაზის შექმნა: გახსენით Package Manager Console და გაუშვით: Update-Database
### 4. პროექტის გაშვება: დააჭირეთ F5-ს და გადადით: http://localhost:..../swagger

## API Endpoints:
### POST  /api/customers  ახალი მომხმარებლის დამატება
### POST  /api/Loans/CreateApplication  სესხის განაცხადის შექმნა
### GET  /api/loans/{id}  სესხის სტატუსის ნახვა
### POST  /api/payments  გადახდის განხორციელება
### GET  /api/customers/loans?customerId={id}  მომხმარებლის სესხების ისტორია

## ბიზნეს წესები:
### მომხმარებელი უნდა იყოს მინიმუმ 18 წლის
### CreditScore 300-ზე დაბლა -> სესხი უარყოფილია
### CreditScore 300 ან მეტი -> სესხი დამტკიცებულია
### სესხის თანხა: 500₾ -> 50,000₾
### სესხის ვადა: 6 -> 60 თვე
### დახურულ სესხზე გადახდა შეუძლებელია

## მონაცემთა ბაზა:
### Customers  მომხმარებლების ინფორმაცია
### Loans  სესხების  ინფორმაცია და სტატუსი
### Payments  გადახდების ისტორია
### LoanSchedules  ყოველთვიური გადახდების გრაფიკი
