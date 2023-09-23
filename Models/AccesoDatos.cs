using System.Text.Json;
using System.Text.Json.Serialization;
namespace tl2_tp4_2023_adanSmith01;

public abstract class AccesoDatos
{
    public abstract Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria);
    public abstract List<Cadete> ObtenerListaCadetes(string rutaDatosCadetes);

    public bool ExisteArchivoDatos(string ruta){
        FileInfo f = new FileInfo(ruta);

        if(f.Exists && f.Length > 0){
            return true;
        }else{
            return false;
        }
    }
}

public class AccesoCSV : AccesoDatos
{
    
    public override Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria){
        string[] datosCadeteria;

        using (StreamReader s = new StreamReader(rutaDatosCadeteria))
        {
            datosCadeteria = s.ReadLine().Split(',');
        }

        Cadeteria cadeteria = new Cadeteria(datosCadeteria[0], datosCadeteria[1]);
        return cadeteria;
    } 

    public override List<Cadete> ObtenerListaCadetes(string rutaDatosCadetes){
        List<Cadete> cadetes = new List<Cadete>();

        string linea = "";
        string[] datosCadete;

        using(StreamReader s = new StreamReader(rutaDatosCadetes))
        {
            while((linea = s.ReadLine()) != null){
                datosCadete = linea.Split(',');
                Cadete cadete = new Cadete(Convert.ToInt32(datosCadete[0]), datosCadete[1], datosCadete[2], datosCadete[3]);
                cadetes.Add(cadete);
            }
        }

        return cadetes;
    }
}

public class AccesoJSON : AccesoDatos
{
    public override Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria){
        string infoCadeteria = File.ReadAllText(rutaDatosCadeteria);
        Cadeteria cadeteriaConInfo = JsonSerializer.Deserialize<Cadeteria>(infoCadeteria);
        return cadeteriaConInfo;
    }

    public override List<Cadete> ObtenerListaCadetes(string rutaDatosCadetes){
        string infoCadetes = File.ReadAllText(rutaDatosCadetes);
        List<Cadete> cadetes = JsonSerializer.Deserialize<List<Cadete>>(infoCadetes);
        return cadetes;
    }
}