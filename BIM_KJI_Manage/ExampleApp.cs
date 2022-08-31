
namespace Propotype_Manage
{
    using System;
    using System.Windows.Input;
    using Fusion;
    using Propotype_Manage.ViewConductor;
    using BIMPropotype_Lib.ViewModel;

    /// <summary>
    /// Example application.
    /// </summary>
    public class ExampleApp : Fusion.App
    {
        public ConductorViewModel InConductorViewModel { get; set; }
        public PrefixDirectory InPrefixDirectory { get; set; }
        /// <summary>
        /// Определяет точку входа в приложение.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var prefixDirectory = new PrefixDirectory();
            Fusion.App.Start(new ExampleApp()
            {
                InPrefixDirectory = prefixDirectory,
                InConductorViewModel = new ConductorViewModel(prefixDirectory),
            });
        }

        /// <summary>
        /// Создает главное окно.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Модель просмотра главного окна.</returns>
        [PublishedView("App.MainWindow")]
        public ViewModel CreateMainWindow(object parameter)
        {
            return new MainWindowViewModel(InPrefixDirectory);
        }

        /// <summary>
        /// TreeView просетов.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Модель просмотра главного окна.</returns>
        [PublishedView("Example.Conductor.Conductor")]
        public ViewModel ConductorView(object parameter)
        {
            return InConductorViewModel;
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
