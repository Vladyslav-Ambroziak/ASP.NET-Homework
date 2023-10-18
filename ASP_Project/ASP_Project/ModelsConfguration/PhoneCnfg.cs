using ASP_Project.Models;

namespace ASP_Project.ModelsConfguration
{
    public class PhoneCnfg
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Series { get; set; } = null!;
        public int? MemoryPhone { get; set; } = null;
        public string Color { get; set; } = null!;
        public int BatteryCapacity { get; set; }
        public float ScreenDiagonal { get; set; }
        public int Camera { get; set; }
        public float Price { get; set; }

        public PhoneCnfg() { }

        public PhoneCnfg(int id, string manufacturer, string series, Memory memory, string color, int batteryCapacity, float screenDiagonal, int camera, float price)
        {
            Id = id;
            Manufacturer = manufacturer;
            Series = series;
            if (memory != null)
                MemoryPhone = memory.Amount;
            Color = color;
            BatteryCapacity = batteryCapacity;
            ScreenDiagonal = screenDiagonal;
            Camera = camera;
            Price = price;
        }
    }
}
