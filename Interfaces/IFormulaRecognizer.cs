namespace App.Interfaces
{
    using System.Threading.Tasks;

    public interface IFormulaRecognizer : Orleans.IGrainWithIntegerKey
    {
        Task<string> GetChemicalSubstanceName(string formula);
    }
}
