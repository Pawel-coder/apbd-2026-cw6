using Tutorial6.Models;
namespace Tutorial6.Data;

public class DataBase
{
    public static List<Room> Rooms { get; set; } = new List<Room>
    {
        new Room { Id = 1, Name = "Sala C1", BuildingCode = "C", Floor = 1, Capacity = 150, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "A360", BuildingCode = "A", Floor = 3, Capacity = 25, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "B204", BuildingCode = "B", Floor = 2, Capacity = 15, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Sala A1", BuildingCode = "A", Floor = 0, Capacity = 300, HasProjector = true, IsActive = true },
        new Room { Id = 5, Name = "B034", BuildingCode = "B", Floor = -1, Capacity = 20, HasProjector = false, IsActive = false }
    };

    public static List<Reservation> Reservations { get; set; } = new List<Reservation>
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Joseph Joestar", Topic = "Wykład C#", Date = new DateOnly(2026, 4, 16), StartTime = new TimeOnly(8, 30, 0), EndTime = new TimeOnly(10, 0, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 3, OrganizerName = "Jolyne Joestar", Topic = "Ćwiczenia C#", Date = new DateOnly(2026, 4, 16), StartTime = new TimeOnly(10, 15, 0), EndTime = new TimeOnly(11, 45, 0), Status = "confirmed" },
        new Reservation { Id = 3, RoomId = 2, OrganizerName = "Jonathan Joestar", Topic = "Spotkanie koła Gamelab", Date = new DateOnly(2026, 4, 16), StartTime = new TimeOnly(19, 15, 0), EndTime = new TimeOnly(20, 45, 0), Status = "planned" },
        new Reservation { Id = 4, RoomId = 4, OrganizerName = "Jotaro Joestar", Topic = "Koniec świata", Date = new DateOnly(2026, 4, 16), StartTime = new TimeOnly(0, 0, 0), EndTime = new TimeOnly(23, 59, 59), Status = "cancelled" }
    };
}