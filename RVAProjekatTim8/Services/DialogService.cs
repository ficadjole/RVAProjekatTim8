using RVAProjekatTim8.Interfaces;
using System;
using System.Windows;

namespace RVAProjekatTim8.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object viewModel)
        {
            var view = CreateViewFor(viewModel);
            view.DataContext = viewModel;

            var dialogWindow = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize
            };

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

        private static FrameworkElement CreateViewFor(object viewModel)
        {
            var viewModelTypeName = viewModel.GetType().FullName!;
            var viewTypeName = viewModelTypeName
                .Replace(".ViewModels.", ".Views.")
                .Replace("ViewModel", "View");

            var viewType = viewModel.GetType().Assembly.GetType(viewTypeName);

            if (viewType is null)
            {
                throw new InvalidOperationException(
                    $"Nije pronađen View za ViewModel tip '{viewModelTypeName}'. Očekivan tip: '{viewTypeName}'.");
            }

            return (FrameworkElement)Activator.CreateInstance(viewType)!;
        }
    }
}