
namespace BIM_KJI_Manage
{
    using System;
    using System.Windows.Input;
    using Fusion;

    /// <summary>
    /// Example application.
    /// </summary>
    public class ExampleApp : Fusion.App
    {
        /// <summary>
        /// Определяет точку входа в приложение.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Fusion.App.Start(new ExampleApp());
        }

        /// <summary>
        /// Создает главное окно.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Модель просмотра главного окна.</returns>
        [PublishedView("App.MainWindow")]
        public ViewModel CreateMainWindow(object parameter)
        {
            return new MainWindowViewModel();
        }

        /// <summary>
        /// Создает главное окно.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Модель просмотра главного окна.</returns>
        [PublishedView("Example.TreeViewMain.AlbumKJIControl")]
        public ViewModel CreateAlbumView(object parameter)
        {
            return new TreeViewMain.AlbumKJIControlViewModel();
        }



        /// <summary>
        /// Raises the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // F9 opens the diagnostic window.
            if (e.Match(Key.F9))
            {
                e.Handled = true;
                this.Host.UI.ShowDiagnosticWindow();
            }
        }
    }
}
