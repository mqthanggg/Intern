using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("dispenser")]
public class Dispenser{
    [Key]
    public required int dispenser_id {get; set;} 
    [ForeignKey("Station")]
    public required int station_id {get;set;}
    [ForeignKey("Tank")]
    public required int tank_id {get;set;}
    [ForeignKey("Fuel")]
    public required int fuel_id {get;set;}
    [Required]
    public required int name {get;set;}
    public Station? Station { get; set; }
    public Tank? Tank { get; set; }
    public Fuel? Fuel { get; set; }
}


public class Station{
    [Key]
    public required int station_id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string address { get; set; }

    // Navigation Property
    public ICollection<Tank>? Tanks { get; set; }
}

public class Fuel
{
    [Key]
    public required int fuel_id { get; set; }

    [Required]
    [MaxLength(3)]
    public required string short_name { get; set; }

    [Required]
    [MaxLength(15)]
    public required string long_name { get; set; }

    [Required]
    public required int price { get; set; }

    // Navigation Property
    public ICollection<Tank>? Tanks { get; set; }
}

public class Tank
{
    [Key]
    public required int tank_id { get; set; }

    [ForeignKey("Fuel")]
    public required int fuel_id { get; set; }

    [ForeignKey("Station")]
    public required int station_id { get; set; }

    [Required]
    public required int max_volume { get; set; }

    // Navigation Properties
    public Fuel? Fuel { get; set; }
    public Station? Station { get; set; }
}

public class User
{
    [Key]
    public required int user_id { get; set; }

    [Required]
    [MaxLength(15)]
    public required string username { get; set; }

    [Required]
    [MaxLength(72)]
    public required string password { get; set; }
}