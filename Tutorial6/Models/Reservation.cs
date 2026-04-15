using System.ComponentModel.DataAnnotations;
namespace Tutorial6.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    [Required(ErrorMessage = "OrganizerName nie powinno być puste.")]
    public string OrganizerName { get; set; }
    [Required(ErrorMessage = "Topic nie powinno być puste.")]
    public string Topic { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } // np. planned, confirmed, cancelled
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime musi być późniejsze niż StartTime.",
                new[] {nameof(EndTime)}
            );
        }
    }
}