using System.ComponentModel.DataAnnotations;
namespace Tutorial6.Models;

public class Room
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name nie powinno być puste.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "BuildingCode nie powinno być puste.")]
    public string BuildingCode { get; set; }
    public int Floor { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Capacity musi być większe od zera.")]
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}