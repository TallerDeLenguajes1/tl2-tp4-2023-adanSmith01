using System.Text.Json;
namespace tl2_tp4_2023_adanSmith01;

public class AccesoADatosCadetes
{
    public List<Cadete> ObtenerListaCadetes(){
        string rutaDatosCadetes = "cadetesInfo.json";
        FileInfo f = new FileInfo(rutaDatosCadetes);
        List<Cadete> cadetes = new List<Cadete>();

        if(f.Exists && f.Length > 0){
            string infoCadetes = File.ReadAllText(rutaDatosCadetes);
            cadetes = JsonSerializer.Deserialize<List<Cadete>>(infoCadetes);
        }
        
        return cadetes;
    }
}