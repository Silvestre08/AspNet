using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.Entities
{
    public class PointOfInterest
    {
        public PointOfInterest(string name)
        {    
            Name = name;
        }

        public City? City { get; set; }

        [ForeignKey("CityId")]
        public int CityId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}