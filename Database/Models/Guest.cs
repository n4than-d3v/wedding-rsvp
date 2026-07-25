using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Database.Models;

public class Guest
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public bool? Coming { get; set; }

    public Party Party { get; set; }
}
