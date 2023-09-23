using System.Text.Json;
namespace tl2_tp4_2023_adanSmith01;

public class AccesoADatosPedidos
{
    public List<Pedido> ObtenerListaPedidos(){
        string rutaArchivoPedidos = "registroPedidos.json";
        FileInfo f = new FileInfo(rutaArchivoPedidos);
        List<Pedido> lista = new List<Pedido>();

        if(f.Exists &&f.Length > 0){
            string info = File.ReadAllText(rutaArchivoPedidos);
            lista = JsonSerializer.Deserialize<List<Pedido>>(info);
        }
        return lista;
    }

    public void GuardarLista(List<Pedido> Pedidos){
        string rutaArchivoPedidos = "registroPedidos.json";
        string infoLista = JsonSerializer.Serialize(Pedidos);
        File.WriteAllText(rutaArchivoPedidos, infoLista);
    }
}