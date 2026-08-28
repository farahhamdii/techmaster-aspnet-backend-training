namespace ApiRoutingDrills.Services
{
    public class ConverterService : IConverterService
    {
        public TemperatureConversionResult ConvertCelsiusToFahrenheit(decimal celsius)
        {
            decimal fahrenheit = Math.Round((celsius * 9m / 5m) + 32m, 2);

            return new TemperatureConversionResult
            {
                Celsius = Math.Round(celsius, 2),
                Fahrenheit = fahrenheit,
                FormulaUsed = "(Celsius * 9 / 5) + 32"
            };
        }
    }
}