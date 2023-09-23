using System.Text.Json;
namespace tl2_tp4_2023_adanSmith01;

public class AccesoADatosCadeteria
{
    public Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria){
        FileInfo f = new FileInfo(rutaDatosCadeteria);
        Cadeteria cadeteriaConInfo = null;

        if(f.Exists && f.Length > 0){
            string infoCadeteria = File.ReadAllText(rutaDatosCadeteria);
            cadeteriaConInfo = JsonSerializer.Deserialize<Cadeteria>(infoCadeteria);
        }

        return cadeteriaConInfo;
    }
}