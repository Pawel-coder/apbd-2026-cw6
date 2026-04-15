using Microsoft.AspNetCore.Mvc;
using Tutorial6.Data;
using Tutorial6.Models;
namespace Tutorial6.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
        [HttpGet] //GET /api/reservations lub np. GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
        public IActionResult Get([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var query = DataBase.Reservations.AsQueryable();
            if (date.HasValue)
                query = query.Where(r=>r.Date==date.Value);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(r=>r.Status.Equals(status,StringComparison.OrdinalIgnoreCase));
            if (roomId.HasValue)
                query = query.Where(r=>r.RoomId==roomId.Value);
            return Ok(query.ToList());
        }
        [HttpGet("{id}")] //GET /api/reservations/{id}
        public IActionResult GetById(int id)
        {
            var reservation = DataBase.Reservations.FirstOrDefault(r=>r.Id==id);
            if (reservation == null)
                return NotFound($"Reservation o podanym ID {id} nie został znaleziony.");
            return Ok(reservation);
        }
        [HttpPost] //POST /api/reservations
        public IActionResult Add([FromBody] Reservation reservation)
        {
            var room = DataBase.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null)
                return NotFound($"Room o podanym ID {reservation.RoomId} nie został znaleziony.");
            if (!room.IsActive)
                return BadRequest("Nie wolno dodać Reservation dla Room oznaczonego jako nieaktywny");
            if (IsTimeConflict(reservation.RoomId, reservation.Date, reservation.StartTime, reservation.EndTime))
                return Conflict("Dwie Reservation tego samego Room nie mogą nakładać się czasowo tego samego dnia.");
            reservation.Id = DataBase.Reservations.Any()?DataBase.Reservations.Max(r=>r.Id)+1:1;
            DataBase.Reservations.Add(reservation);
            return CreatedAtAction(nameof(GetById), new {id=reservation.Id},reservation);
        }
        [HttpPut("{id}")] //PUT /api/reservations/{id}
        public IActionResult Update(int id, [FromBody] Reservation updatedReservation)
        {
            var reservation = DataBase.Reservations.FirstOrDefault(r=>r.Id==id);
            if (reservation == null)
                return NotFound($"Reservation o podanym ID {id} nie został znaleziony.");
            var room = DataBase.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
            if (room == null) return NotFound($"Room o podanym ID {updatedReservation.RoomId} nie został znaleziony.");
            if (!room.IsActive) return BadRequest($"Room o podanym ID {updatedReservation.RoomId} nie jest aktywny.");
            if (IsTimeConflict(updatedReservation.RoomId, updatedReservation.Date, updatedReservation.StartTime, updatedReservation.EndTime, id))
                return Conflict("Dwie Reservation tego samego Room nie mogą nakładać się czasowo tego samego dnia.");
            reservation.RoomId = updatedReservation.RoomId;
            reservation.OrganizerName = updatedReservation.OrganizerName;
            reservation.Topic = updatedReservation.Topic;
            reservation.Date = updatedReservation.Date;
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Status = updatedReservation.Status;
            return Ok(reservation);
        }
        [HttpDelete("{id}")] //DELETE /api/reservations/{id}
        public IActionResult Delete(int id)
        {
            var reservation = DataBase.Reservations.FirstOrDefault(r=>r.Id==id);
            if (reservation == null)
                return NotFound($"Reservation o podanym ID {id} nie został znaleziony.");
            DataBase.Reservations.Remove(reservation);
            return NoContent();
        } 
        private bool IsTimeConflict(int roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime, int? excceptionId = null)
        {
            return DataBase.Reservations.Any(r=>
                r.RoomId == roomId &&
                r.Date == date &&
                startTime < r.EndTime && endTime > r.StartTime &&
                r.Status != "cancelled" &&
                r.Id != excceptionId
            );
        }
}