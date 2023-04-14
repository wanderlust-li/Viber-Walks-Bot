namespace Viber_bot.Models;

public class Walk
{
    public int Id { get; set; }
    
    public string IMEI { get; set; }
    
    public DateTime Date_track { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public double Kilometers { get; set; }
    
    public double Minutes { get; set; }
}