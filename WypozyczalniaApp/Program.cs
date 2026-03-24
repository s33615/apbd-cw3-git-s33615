using System;
using System.Linq;
using WypozyczalniaApp.Domain;
using WypozyczalniaApp.Repositories;
using WypozyczalniaApp.Services;

IEquipmentRepository equipmentRepo = new InMemoryEquipmentRepository();
IUserRepository userRepo = new InMemoryUserRepository();
IRentalRepository rentalRepo = new InMemoryRentalRepository();
IPenaltyCalculator penaltyCalculator = new StandardPenaltyCalculator();

var rentalService = new RentalService(equipmentRepo, userRepo, rentalRepo, penaltyCalculator);

Console.WriteLine("=== SYSTEM WYPOŻYCZALNI SPRZĘTU ===\n");

var laptop = new Laptop("Dell XPS 15", 16, "Intel Core i7");
var projector = new Projector("Epson 1080p", "1920x1080", 3000);
var camera = new Camera("Sony A7 III", 24.2, "50mm f/1.8");

equipmentRepo.Add(laptop);
equipmentRepo.Add(projector);
equipmentRepo.Add(camera);
Console.WriteLine("[OK] Dodano sprzęt do bazy.");

var student = new Student("Jan", "Kowalski");
var employee = new Employee("Anna", "Nowak");

userRepo.Add(student);
userRepo.Add(employee);
Console.WriteLine("[OK] Dodano użytkowników do bazy.\n");

Console.WriteLine("--- Próba poprawnego wypożyczenia ---");
rentalService.RentEquipment(student.Id, laptop.Id, TimeSpan.FromDays(7));
rentalService.RentEquipment(student.Id, projector.Id, TimeSpan.FromDays(2));
Console.WriteLine($"[OK] Student {student.FirstName} wypożyczył Laptop i Projektor.\n");

Console.WriteLine("--- Próba niepoprawnej operacji (Limit) ---");
try
{
    rentalService.RentEquipment(student.Id, camera.Id, TimeSpan.FromDays(1));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[ZABLOKOWANO] Błąd biznesowy: {ex.Message}\n");
}

var studentRentals = rentalRepo.GetActiveRentalsForUser(student.Id).ToList();

Console.WriteLine("--- Zwrot w terminie ---");
var laptopRental = studentRentals.First(r => r.EquipmentId == laptop.Id);
decimal penalty1 = rentalService.ReturnEquipment(laptopRental.Id, DateTime.Now);
Console.WriteLine($"[OK] Zwrócono {laptop.Name} w terminie. Naliczona kara: {penalty1} PLN.\n");

Console.WriteLine("--- Zwrot opóźniony ---");
var projectorRental = studentRentals.First(r => r.EquipmentId == projector.Id);
DateTime lateReturnDate = projectorRental.DueDate.AddDays(8); 
decimal penalty2 = rentalService.ReturnEquipment(projectorRental.Id, lateReturnDate);
Console.WriteLine($"[OK] Zwrócono {projector.Name} z opóźnieniem. Naliczona kara: {penalty2} PLN.\n");

Console.WriteLine("=== RAPORT KOŃCOWY ===");
Console.WriteLine("Dostępny sprzęt w magazynie:");
foreach (var eq in equipmentRepo.GetAvailable())
{
    Console.WriteLine($"- {eq.Name} (ID: {eq.Id})");
}

Console.WriteLine("\nAktywne wypożyczenia w systemie:");
var allRentals = rentalRepo.GetActiveRentalsForUser(employee.Id).Concat(rentalRepo.GetActiveRentalsForUser(student.Id));
if (!allRentals.Any())
{
    Console.WriteLine("Brak aktywnych wypożyczeń.");
}