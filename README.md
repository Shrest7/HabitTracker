# HabitTracker
HabitTracker is a Windows Forms app designed to keep track of your habits.

 <img src="https://user-images.githubusercontent.com/31261595/159293978-feafed7c-a761-4b3b-9dc3-a5220e976243.png" width="602" height="608"/>     

## Technologies used
- .NET Framework 4.7.2. 
- EFCore.

## Requirements
HabitTracker requires SQL Server installed (2016 or newer).

## Usage
- clone the repository
- in the Solution Explorer, right click on the solution and choose Restore NuGet Packages.
- close and reopen the solution.


## About
It's my first personal project, created primarly because I wanted to, well, build new habits and track whether I'm doing well or not.

The app performs CRUD operations. It also has a table (DataGridView, to be precise), which allows you to mark "habit completion" for a
desired day, by simply clicking on the DataGridView's cell. Using mouse scroll you can go up or down through the table. If you end up scrolling too far you can go back to today's date using button with appropriate description. There is also an option to change mark's color. The gray cells are here to make finding the current day easier.

One of the features I'm most proud of is dynamic updating/resizing of the DataGridView, whenever a habit is added/removed. Basically, the form with the DataGridView, will automatically resize, without needing to close and reopen it.

What I'm hoping to add in the future:
- representing streaks with appropriate "strength" of a color. Basically going from a lighter to a darker color to indicate a streak;
- make database calls async.


