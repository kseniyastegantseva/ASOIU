List<Department> departments =
[
    new() { Id = 1, Name = "Разработка" },
    new() { Id = 2, Name = "Тестирование" },
    new() { Id = 3, Name = "Аналитика" }
];

List<Employee> employees =
[
    new() { Id = 1, Name = "Иванов", DepartmentId = 1, Salary = 150000 },
    new() { Id = 2, Name = "Петрова", DepartmentId = 1, Salary = 180000 },
    new() { Id = 3, Name = "Сидоров", DepartmentId = 2, Salary = 120000 },
    new() { Id = 4, Name = "Козлова", DepartmentId = 1, Salary = 160000 },
    new() { Id = 5, Name = "Николаев", DepartmentId = 3, Salary = 140000 },
    new() { Id = 6, Name = "Морозова", DepartmentId = 2, Salary = 130000 }
];

List<Project> projects =
[
    new() { Id = 101, Title = "Сайт", Budget = 500000 },
    new() { Id = 102, Title = "Мобильное приложение", Budget = 800000 },
    new() { Id = 103, Title = "Аналитическая платформа", Budget = 1200000 }
];

List<EmployeeProject> employeeProjects =
[
    new() { EmployeeId = 1, ProjectId = 101 },
    new() { EmployeeId = 1, ProjectId = 102 },
    new() { EmployeeId = 2, ProjectId = 102 },
    new() { EmployeeId = 2, ProjectId = 103 },
    new() { EmployeeId = 3, ProjectId = 101 },
    new() { EmployeeId = 4, ProjectId = 101 },
    new() { EmployeeId = 4, ProjectId = 102 },
    new() { EmployeeId = 4, ProjectId = 103 },
    new() { EmployeeId = 5, ProjectId = 103 },
    new() { EmployeeId = 6, ProjectId = 101 },
    new() { EmployeeId = 6, ProjectId = 102 }
];

PrintSection("1. Сотрудники с зарплатой от 130000 до 160000");
var query1 = employees
    .Where(employee => employee.Salary is >= 130000 and <= 160000)
    .OrderBy(employee => employee.Name);
foreach (Employee employee in query1)
{
    Console.WriteLine($"  {employee.Name} — {employee.Salary:N0}");
}

PrintSection("2. Уникальные отделы с зарплатой выше 150000");
var query2 = employees
    .Where(employee => employee.Salary > 150000)
    .Join(departments, employee => employee.DepartmentId, department => department.Id, (_, department) => department.Name)
    .Distinct()
    .OrderBy(name => name);
foreach (string departmentName in query2)
{
    Console.WriteLine($"  {departmentName}");
}

PrintSection("3. Количество сотрудников по отделам");
var query3 = departments
    .GroupJoin(employees, department => department.Id, employee => employee.DepartmentId,
        (department, departmentEmployees) => new { department.Name, Count = departmentEmployees.Count() });
foreach (var item in query3)
{
    Console.WriteLine($"  {item.Name}: {item.Count}");
}

PrintSection("4. Имя — Отдел — Зарплата");
var query4 = from employee in employees
             join department in departments on employee.DepartmentId equals department.Id
             orderby department.Name, employee.Salary descending
             select new { employee.Name, Department = department.Name, employee.Salary };
foreach (var item in query4)
{
    Console.WriteLine($"  {item.Name} — {item.Department} — {item.Salary:N0}");
}

PrintSection("5. Фонд оплаты труда и средняя зарплата по отделам");
var query5 = from employee in employees
             group employee by employee.DepartmentId into grouped
             join department in departments on grouped.Key equals department.Id
             let average = grouped.Average(employee => employee.Salary)
             where average > 130000
             select new
             {
                 Department = department.Name,
                 Total = grouped.Sum(employee => employee.Salary),
                 Average = average
             };
foreach (var item in query5)
{
    Console.WriteLine($"  {item.Department}: фонд {item.Total:N0}, средняя {item.Average:N0}");
}

PrintSection("6. Отделы, где все сотрудники получают больше 100000");
var query6 = departments
    .GroupJoin(employees, department => department.Id, employee => employee.DepartmentId,
        (department, departmentEmployees) => new { department.Name, Employees = departmentEmployees.ToList() })
    .Where(group => group.Employees.Count > 0 && group.Employees.All(employee => employee.Salary > 100000));
foreach (var item in query6)
{
    Console.WriteLine($"  {item.Name}");
}

PrintSection("7. Проекты каждого сотрудника");
var query7 = from employee in employees
             join link in employeeProjects on employee.Id equals link.EmployeeId into employeeLinks
             select new
             {
                 employee.Name,
                 Projects = from link in employeeLinks
                            join project in projects on link.ProjectId equals project.Id
                            select project.Title
             };
foreach (var item in query7)
{
    Console.WriteLine($"  {item.Name}: {string.Join(", ", item.Projects)}");
}

PrintSection("8. Участники и средняя зарплата по проектам");
var query8 = from project in projects
             join link in employeeProjects on project.Id equals link.ProjectId
             join employee in employees on link.EmployeeId equals employee.Id
             group employee by project.Title into grouped
             select new
             {
                 Project = grouped.Key,
                 Count = grouped.Count(),
                 AverageSalary = grouped.Average(employee => employee.Salary)
             };
foreach (var item in query8)
{
    Console.WriteLine($"  {item.Project}: участников {item.Count}, средняя зарплата {item.AverageSalary:N0}");
}

PrintSection("9. Сотрудники более чем в одном проекте");
var query9 = from employee in employees
             join link in employeeProjects on employee.Id equals link.EmployeeId
             group link by employee.Name into grouped
             where grouped.Count() > 1
             select new { Name = grouped.Key, ProjectCount = grouped.Count() };
foreach (var item in query9)
{
    Console.WriteLine($"  {item.Name}: {item.ProjectCount}");
}

PrintSection("10. Пары сотрудников с общим проектом");
var query10 = from left in employees
              from right in employees
              where left.Id < right.Id
              let leftProjects = employeeProjects.Where(link => link.EmployeeId == left.Id).Select(link => link.ProjectId)
              let rightProjects = employeeProjects.Where(link => link.EmployeeId == right.Id).Select(link => link.ProjectId)
              let commonProjectIds = leftProjects.Intersect(rightProjects).ToList()
              where commonProjectIds.Count > 0
              select new
              {
                  First = left.Name,
                  Second = right.Name,
                  Projects = projects.Where(project => commonProjectIds.Contains(project.Id)).Select(project => project.Title)
              };
foreach (var pair in query10)
{
    Console.WriteLine($"  {pair.First} — {pair.Second}: {string.Join(", ", pair.Projects)}");
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 70));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 70));
}

internal class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

internal class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int DepartmentId { get; set; }
    public decimal Salary { get; set; }
}

internal class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public decimal Budget { get; set; }
}

internal class EmployeeProject
{
    public int EmployeeId { get; set; }
    public int ProjectId { get; set; }
}
