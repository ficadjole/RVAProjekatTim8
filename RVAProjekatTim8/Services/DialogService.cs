using RVAProjekatTim8.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RVAProjekatTim8.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object viewModel)
        {
            var dialogWindow = new Window
            {
                Content = viewModel,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize
            };

            // Ako ViewModel ume da signalizira zatvaranje (implementira
            // ICloseable), pretplaćujemo se na taj event i prevodimo ga
            // u stvaran Window.Close() poziv sa DialogResult-om.
            if (viewModel is ICloseable closeable)
            {
                void OnCloseRequested(object? sender, bool? result)
                {
                    dialogWindow.DialogResult = result;
                    closeable.CloseRequested -= OnCloseRequested;
                }

                closeable.CloseRequested += OnCloseRequested;
            }

            return dialogWindow.ShowDialog();
        }
    }
}
