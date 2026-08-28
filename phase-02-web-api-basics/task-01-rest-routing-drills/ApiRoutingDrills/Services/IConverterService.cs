namespace ApiRoutingDrills.Services
{
    public interface IConverterService
    {
        TemperatureConversionResult ConvertCelsiusToFahrenheit(decimal celsius);
    }

    public class TemperatureConversionResult
    {
        public decimal Celsius { get; set; }
        public decimal Fahrenheit { get; set; }
        public string FormulaUsed { get; set; } = string.Empty;
    }
}