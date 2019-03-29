# Тестовый проект с Microsoft Orleans

| CI Service | Status |
|------------|:------:|
| AppVeyor   | [![Build status](https://ci.appveyor.com/api/projects/status/2x397g96b2s19dtc?svg=true)](https://ci.appveyor.com/project/castlerek/orleans-testing) |

Данный solution состоит из:
1. grain-интерфейса
2. grain-класса
3. Silo
4. Client

**То, что клиент старается получить из Grain:**
```csharp
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
```

PS: в последующих версиях - добавиться поддержка работы на базе Docker/K8S
