using Microsoft.AspNetCore.Mvc;
using Tutorial6.Data;
using Tutorial6.Models;
namespace Tutorial6.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
        [HttpGet] //GET /api/rooms lub np. GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
        public IActionResult Get([FromQuery]int?minCapacity,[FromQuery]bool?hasProjector,[FromQuery]bool?activeOnly)
        {
            var query = DataBase.Rooms.AsQueryable();
            if (minCapacity.HasValue)
                query = query.Where(r=>r.Capacity>=minCapacity.Value);
            if (hasProjector.HasValue)
                query = query.Where(r=>r.HasProjector==hasProjector.Value);
            if (activeOnly.HasValue&&activeOnly.Value)
                query = query.Where(r=>r.IsActive);
            return Ok(query.ToList());
        }
        [HttpGet("{id}")] //GET /api/rooms/{id}
        public IActionResult GetById(int id)
        {
            var room = DataBase.Rooms.FirstOrDefault(r=>r.Id==id);
            if (room==null)
                return NotFound($"Room o podanym ID {id} nie został znaleziony.");
            return Ok(room);
        }
        [HttpGet("building/{buildingCode}")] //GET /api/rooms/building/{buildingCode}
        public IActionResult GetByBuilding(string buildingCode)
        {
            var rooms = DataBase.Rooms
                .Where(r =>r.BuildingCode.Equals(buildingCode,StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(rooms);
        }
        [HttpPost] //POST /api/rooms
        public IActionResult Add([FromBody] Room room)
        {
            room.Id = DataBase.Rooms.Any()?DataBase.Rooms.Max(r=>r.Id)+1:1;
            DataBase.Rooms.Add(room);
            return CreatedAtAction(nameof(GetById), new {id=room.Id},room);
        }
        [HttpPut("{id}")] //POST /api/rooms
        public IActionResult Update(int id, [FromBody] Room updatedRoom)
        {
            var room = DataBase.Rooms.FirstOrDefault(r=>r.Id==id);
            if (room == null)
                return NotFound($"Room o podanym ID {id} nie został znaleziony.");
            room.Name = updatedRoom.Name;
            room.BuildingCode = updatedRoom.BuildingCode;
            room.Floor = updatedRoom.Floor;
            room.Capacity = updatedRoom.Capacity;
            room.HasProjector = updatedRoom.HasProjector;
            room.IsActive = updatedRoom.IsActive;
            return Ok(room);
        }
        [HttpDelete("{id}")] //DELETE /api/rooms/{id}
        public IActionResult Delete(int id)
        {
            var room = DataBase.Rooms.FirstOrDefault(r=>r.Id==id);
            if (room == null)
                return NotFound($"Room o podanym ID {id} nie został znaleziony.");
            var dateToday=DateOnly.FromDateTime(DateTime.Now);
            var futureReservationsExist = DataBase.Reservations.Any(res=>res.RoomId==id&&res.Date>=dateToday);
            if (futureReservationsExist)
                return Conflict($"Nie można usunąć room o ID {id}, ponieważ istnieją dla niego przyszłe Reservations.");
            DataBase.Rooms.Remove(room);
            return NoContent();
        }
}