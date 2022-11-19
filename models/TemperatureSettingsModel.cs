namespace StreamDeckYeelightPlugin.Models
{
    public class TemperatureSettingsModel : MainSettingsModel
    {
        public const int MIN_VALUE = 2700;
        public const int MAX_VALUE = 6500;

        public int Value { get; set; }
        public int Step { get; set; }
    }
}
