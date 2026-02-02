using System.Text.Json;

/// <summary>
/// Classe représentant la configuration de l'accès à l'API.
/// Permet de charger les paramètres définis dans le fichier appsettings.json,
/// tels que le mode local, les URLs d'accès et l'empreinte du certificat API.
/// </summary>
public class APIConfig
{

    private bool localMode;
    private string localUrl;
    private string baseUrl;
    private string certificate;

    /// <summary>
    /// Indique si l'application doit utiliser le mode local pour accéder à l'API.
    /// </summary>
    public bool LocalMode { 
        get
        {
            return localMode;
        }
        set
        {
            localMode = value;
        }
    }

    /// <summary>
    /// URL de l'API en mode local.
    /// </summary>
    public string LocalUrl {
        get
        {
            return localUrl;
        }
        set
        {
            localUrl = value;
        }
    }

    /// <summary>
    /// URL de base de l'API distante.
    /// </summary>
    public string BaseUrl {
        get
        {
            return baseUrl;
        }
        set
        {
            baseUrl = value;
        } 
    }

    /// <summary>
    /// Empreinte (hash) du certificat API utilisé pour valider la connexion.
    /// </summary>
    public string APICertificate {
        get
        {
            return certificate;
        }
        set
        {
            certificate = value;
        } 
    }

    /// <summary>
    /// Charge la configuration API à partir d'un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier de configuration (appsettings.json par défaut).</param>
    /// <returns>Instance d'APIConfig contenant les paramètres chargés.</returns>
    public static APIConfig Load(string filePath = "appsettings.json")
    {
        string json = File.ReadAllText(filePath);
        using var doc = JsonDocument.Parse(json);
        var api = doc.RootElement.GetProperty("API");
        return new APIConfig
        {
            LocalMode = api.GetProperty("LocalMode").GetString() == "true",
            LocalUrl = api.GetProperty("LocalUrl").GetString(),
            BaseUrl = api.GetProperty("BaseUrl").GetString(),
            APICertificate = api.GetProperty("APICertificate").GetString()
        };
    }
}

