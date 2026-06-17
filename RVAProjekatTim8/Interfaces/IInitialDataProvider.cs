using System.Collections.Generic;

namespace RVAProjekatTim8.Interfaces
{
    public interface IInitialDataProvider<T>
    {
        /// <summary>Vraća minimalan skup instanci (specifikacija traži najmanje 3).</summary>
        List<T> GetInitialData();
    }
}
