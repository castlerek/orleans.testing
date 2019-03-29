namespace App.Grains
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public sealed class FormulaRecognizerGrain : Orleans.Grain, App.Interfaces.IFormulaRecognizer
    {
        private readonly ILogger _logger;

        public FormulaRecognizerGrain(ILogger<FormulaRecognizerGrain> logger)
        {
            this._logger = logger;
        }

        public Task<string> GetChemicalSubstanceName(string formula)
        {
            this._logger.LogInformation($"GetChemicalSubstanceName message received: formula = '{formula}'");
            return Task.FromResult(GetChemicalName(formula));
        }

        private string GetChemicalName(string formula)
        {
            switch (formula)
            {
                case "H2O2":
                    return "Hydrogen peroxide";
                case "CH3":
                    return "Methyl radical";
                case "C3H6":
                    return "Cyclopropane";
                default:
                    throw new Exception("Unknown formula.");
            }
        }
    }
}
