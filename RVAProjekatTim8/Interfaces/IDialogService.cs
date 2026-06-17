using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVAProjekatTim8.Interfaces
{
    public interface IDialogService
    {
        /// <summary>
        /// Prikazuje modalni dijalog čiji je sadržaj određen tipom datog
        /// ViewModel-a (View se pronalazi preko DataTemplate mapiranja u XAML-u).
        /// Vraća true ako je korisnik potvrdio (npr. kliknuo "Sačuvaj"),
        /// false ako je otkazao, null ako je dijalog zatvoren na drugi način (X dugme).
        /// </summary>
        bool? ShowDialog(object viewModel);
    }
}
