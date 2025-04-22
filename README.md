# <img src="https://github.com/user-attachments/assets/73d90776-06ab-4f03-a260-c4d92b603279" width="50px" /> Motosphere 


**Motosphere** is a specialized social network for motorcycle enthusiasts, providing a dedicated platform for connecting riders, sharing experiences, selling motorcycles and parts, and organizing motorcycle events.

---

## Features

- **User Profiles:** Registration, login, profile customization, profile pictures.
- **Forum:** Post discussions, comment, reply, vote, and attach images.
- **Marketplace:** Create listings for motorcycles, parts, and accessories with images and category filtering.
- **Events:** Organize and join motorcycle events, categorized by type.
- **Roles & Permissions:** Role-based access control (User, Moderator, Administrator).
- **Admin Panel:** Manage users, posts, events, marketplace listings, and categories.

---

## Technologies Used

- **Backend:** ASP.NET Core MVC / .NET 8
- **Frontend:** Razor Views, Bootstrap
- **Database:** MS SQL Server, Entity Framework Core
- **Authentication:** ASP.NET Core Identity
- **Cloud Storage:** Amazon S3 for user-uploaded images
- **Hosting:** Docker containers deployed on Render

---

## Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/JoannaBaeva/motosphere.git
   ```

2. **Configure Environment Variables:**
   - Database connection string
   - AWS S3 credentials
   - Email service settings

3. **Create Database Migration:**
   ```bash
   dotnet ef migrations add InitialCreate
   ```

4. **Run Database Migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Build and Run Locally:**
   ```bash
   docker-compose up --build
   ```

6. **Access the Application:**
   Open your browser at `http://localhost:[your port]`.

---

## Project Structure

- `MotorcycleForum.Web` — Main ASP.NET Core Web application (Controllers, Views, wwwroot)
- `MotorcycleForum.Data` — Entity Framework Core models and DbContext
- `MotorcycleForum.Services` — Business logic and external services
- `MotorcycleForum.Tests` — Unit tests

---

‼️Not fully finished yet‼️