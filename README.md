# PART2_UPDATE

I declare I used AI in terms of helping with styling as well as with error handling and debugging and some to provide some guidance





 AgriConnect – Farmer-Employee Product Management System
AgriConnect is a role-based ASP.NET Core MVC web application designed to simplify and secure the management of agricultural product data between farmers and employees. The platform enables tailored access based on user roles, ensuring efficient collaboration while maintaining data security and integrity.

 System Overview
AgriConnect allows farmers to upload and manage their own products, while employees can view all farmers, manage farmer records, and access system-wide product information. The system implements strict role-based authentication to ensure that users interact only with the data and functionality relevant to their role.

🛠️ Features
✅ Secure Role-Based Authentication (Farmer & Employee)
✅ Farmer Product Upload and Management
✅ Employee View of All Products and Farmers
✅ Product Filtering by Name, Category, and Production Date
✅ Farmer Registration by Employees
✅ Responsive UI with TailwindCSS
✅ Data handled using Entity Framework Core & SQL Server

👥 System Roles
Farmer
Sign in to a personalized farmer dashboard

Add and manage their own product listings

Filter products by name, category, or production date

Employee
Sign in to an employee-specific dashboard

Register new farmers into the system

View and filter products submitted by all farmers

 Authentication and Access Control
Users authenticate using credentials stored securely in the SQL Server database

Upon login, the system detects the user's role (Farmer or Employee)

Role-based routing ensures:

Farmers access only their own product management dashboard

Employees access the full management dashboard for farmers and products

 Dashboard Functionality
Farmer Dashboard
Personalized welcome message

Displays the total number of products added by the logged-in farmer

Product listing table showing only the farmer's own products

Filtering by product name, category, and production date

Employee Dashboard
Full list of all products across all farmers

Filtering by product name, category, and production date

Ability to register new farmers into the system

View all registered farmers and their details

 Data Storage Structure
The system uses a relational SQL Server database with the following key tables:

Table	Description
Users	Stores login credentials and role (Farmer/Employee)
Farmers	Contains personal details and location of farmers
Products	Stores product details: name, category, production date, and associated farmer

All product entries are linked to a farmer, enabling efficient filtering and secure, role-specific access.

 Data Flow Summary
User logs in with their credentials

System verifies role (Farmer or Employee)

Users are directed to the relevant dashboard:

Farmers: Manage only their own products

Employees: View all products and manage farmer records

All database interactions use Entity Framework Core for reliable, efficient data management

 User Interface & Experience
Clean, modern design with TailwindCSS

Responsive layout for desktop and mobile

Intuitive navigation with clear forms and tables

Role-specific dashboards ensure relevant functionality

 Setup & Installation
Prerequisites
Visual Studio 2022 or newer

.NET 6 SDK or newer

SQL Server (LocalDB, Express, or Full version)

 Clone the Repository
bash
Copy
Edit
git clone https://github.com/your-username/agri-connect.git
cd agri-connect
 Configure Database Connection
Edit appsettings.json:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AgriConnectDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
Use (localdb)\\MSSQLLocalDB for local development if preferred.

Apply Database Migrations
In Visual Studio, open Package Manager Console:

bash
Copy
Edit
Update-Database
This creates the necessary tables for users, farmers, and products.

 Run the Application
Press F5 or click Run in Visual Studio.
The app will open at:

https://localhost:5001
Testing the System
Register as a Farmer and upload products
Login as an Employee to view all products and manage farmer records
Verify role-based restrictions:
Farmers see only their own products
Employees can access all data
