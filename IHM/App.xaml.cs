using IHM.Services;
using Service.Interfaces;
using System.Windows;
using ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ClientData.Interfaces;
using ClientData.Realisations;
using Service.Implementaions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IHM
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Conteneur de services pour l'injection de dépendances
        public ServiceProvider Services { get; private set; } = null!;

        /// <summary>
        /// Redéfinition de la méthode OnStartup pour initialiser la fenêtre principale.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Appel de la méthode de la classe de base
            base.OnStartup(e);

            var services = new ServiceCollection();

            // HttpClient pour les DAO REST
            services.AddHttpClient();

            // DAO REST côté client
            services.AddTransient<IAutomateDAO, AutomateDAO>();
            services.AddTransient<IUtilisateurDAO, UtilisateurDAO>();

            // Serializer pour le service
            services.AddTransient<IAutomateSerializer, JsonAutomateSerializer>();

            // Service métier côté client
            services.AddTransient<IAutomateService, AutomateService>();
            services.AddTransient<IUtilisateurService, UtilisateurService>();
            services.AddTransient<ICSharpService, CSharpService>();

            // Services IHM
            services.AddSingleton<IFileDialogService, WpfFileDialogService>();
            services.AddSingleton<IExportOptionsService, WpfExportOptionsService>();
            services.AddSingleton<ICanvasExportService, CanvasExportService>();

            // ViewModels
            services.AddTransient<AutomateEditorViewModel>();
            services.AddTransient<Func<AutomateEditorViewModel>>(
                sp => () => sp.GetRequiredService<AutomateEditorViewModel>());
            services.AddTransient<MainViewModel>();

            // Construction du conteneur de services
            Services = services.BuildServiceProvider();

            // Récupération et affichage de la fenêtre principale
            var mainVm = Services.GetRequiredService<MainViewModel>();

            var window = new MainWindow
            {
                DataContext = mainVm
            };
            // La fermeture de la fenêtre principale ferme l'application
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            window.Show();
        }
    }
}
