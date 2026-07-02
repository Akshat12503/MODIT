namespace ModitBackend.Models
{
    public class ServiceZone
    {
        public int Id { get; set; }
        public string PincodeOrArea { get; set; } = string.Empty;
        public string City { get; set; } = "Delhi NCR";
        public string ZoneName { get; set; } = string.Empty; // e.g. "South Delhi", "Gurugram", "Noida"
    }
}