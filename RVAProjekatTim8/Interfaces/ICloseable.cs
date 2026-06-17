using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVAProjekatTim8.Interfaces
{
    public interface ICloseable
    {
        /// <summary>
        /// Event koji ViewModel okida kada želi da se dijalog zatvori.
        /// Argument je DialogResult (true/false) koji se prosleđuje pozivaocu
        /// IDialogService.ShowDialog metode.
        /// </summary>
        event EventHandler<bool?>? CloseRequested;
    }
}
