namespace StreamDeckYeelightPlugin.Models
{
    public class AmbientBrightnessSettingsModel : MainSettingsModel
    {
        public const int MIN_VALUE = 1;
        public const int MAX_VALUE = 100;

        public int Percent { get; set; }
        public int Step { get; set; }
    }
}
