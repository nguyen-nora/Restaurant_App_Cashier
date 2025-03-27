# Restaurant Management System

This project is a comprehensive restaurant management system built using .NET MVC architecture. It features a QR code-based menu system for customers and an integrated cashier and management application for restaurant staff.

## Overview

The Restaurant Management System streamlines restaurant operations through digital menu access via QR codes, order management, table assignment, and comprehensive administrative controls. The system is designed to enhance both customer experience and operational efficiency.

## Key Features

### Customer-Facing Features
- **QR Code Menu Access**: Customers can scan QR codes placed on tables to access the digital menu on their smartphones
- **Digital Menu Display**: Beautiful presentation of menu items with descriptions, prices, and images
- **Item Search**: Allows customers to easily find specific menu items
- **Order Customization**: Customers can customize their orders based on preferences
- **Online Ordering**: Place orders directly from smartphones without waiting for staff
- **Table Reservation**: Ability to book tables in advance through the system

### Management Features
- **Order Management**: Track and manage incoming orders in real-time
- **Table Assignment**: Efficiently manage table assignments and availability
- **Menu Management**: Easy updating of menu items, prices, and availability
- **Employee Management**: Manage staff accounts, roles, and permissions
- **Sales Reporting**: Comprehensive reporting and analytics tools
- **QR Code Generation**: Automatic generation of unique QR codes for each table

## Technology Stack

- **Backend**: ASP.NET Core MVC (.NET 8.0)
- **Database**: SQL Server
- **Authentication**: JWT-based authentication
- **Frontend**: HTML, CSS, JavaScript, Bootstrap
- **QR Code Technology**: For menu access and table identification

## Architecture

The system follows an N-Tier Architecture with Repository Pattern and Unit of Work for clean separation of concerns and maintainable code structure.

## Installation

1. Clone the repository
```bash
git clone https://github.com/nguyen-nora/restaurant-app.git
cd restaurant-management-system
```

2. Update the connection string in appsettings.json
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME; Database=RestaurantDB; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True"
},
"Jwt": {
  "Key": "YOUR_SECURE_KEY",
  "Issuer": "https://localhost:YOUR_PORT/",
  "Audience": "https://localhost:YOUR_PORT/"
}
```

3. Run migrations to set up the database
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

## Usage

### For Customers
1. Scan the QR code on the restaurant table
2. Browse the digital menu
3. Select items and customize as needed
4. Place order directly through the application
5. Track order status
6. Make payment through the app or at the counter

### For Restaurant Staff
1. Login to the management portal
2. Process incoming orders
3. Update menu items and availability
4. Manage table assignments
5. View sales reports and analytics

## Admin Credentials
- Username: 123
- Password: 123456

## Roadmap

- Integration with payment gateways
- Mobile app for customers
- Multi-language support
- Customer loyalty program
- Advanced analytics dashboard
- Kitchen display system integration

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

Your Name - norahieunguyen@gmail.com

Project Link: [https://github.com/norahieunguyen/restaurant_app)
