using System.Collections.Generic;

namespace RVAProjekatTim8.Interfaces
{
    public interface IPersistenceService<T>
    {
        /// <summary>
        /// Učitava kolekciju iz datoteke. Vraća praznu listu (ne null i ne
        /// exception) ako datoteka ne postoji ili je sadržaj prazan — pozivajući
        /// kod (Repository) odlučuje šta znači "praznо" u kontekstu domena.
        /// </summary>
        List<T> Load();

        /// <summary>Čuva kompletnu kolekciju, prepisujući prethodni sadržaj.</summary>
        void Save(IEnumerable<T> items);
    }
}
