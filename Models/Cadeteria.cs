namespace tl2_tp4_2023_adanSmith01;

public class Cadeteria
{
    private static AccesoDatosCadeteria accesoDatos;
    private static Cadeteria cad;
    private string nombre;
    private string telefono;
    private List<Cadete> listaCadetes;
    private List<Pedido> listaPedidos;

    public string Nombre { get => nombre;}
    public string Telefono { get => telefono;}
    public List<Cadete> ListaCadetes { get => listaCadetes;}
    public List<Pedido> ListaPedidos { get => listaPedidos;}

    public Cadeteria(){}
    public Cadeteria(string nombre, string telefono){
        this.nombre = nombre;
        this.telefono = telefono;
        this.listaPedidos = new List<Pedido>();
    }

    public static Cadeteria GetCadeteria(){
        if(cad == null) cad = new Cadeteria();
        return cad;
    }

    public bool CargaDatosIniciales(string tipoAcceso){
        bool cargaRealizada = false;
        List<Cadete> listacadetes = new List<Cadete>();
        if(tipoAcceso == "csv"){
            accesoDatos = new AccesoCSV();
            if(accesoDatos.ExisteArchivoDatos("datosCadeteria.csv")){
                cad = accesoDatos.ObtenerInfoCadeteria("datosCadeteria.csv");
                if(accesoDatos.ExisteArchivoDatos("datosCadetes.csv")){
                    listacadetes = accesoDatos.ObtenerListaCadetes("datosCadetes.csv");
                    cargaRealizada = true;
                }
                cad.AgregarListaCadetes(listacadetes);
            } 
        } else{
            accesoDatos = new AccesoJSON();
            if(accesoDatos.ExisteArchivoDatos("cadeteriaInfo.json")){
                cad = accesoDatos.ObtenerInfoCadeteria("cadeteriaInfo.json");
                if(accesoDatos.ExisteArchivoDatos("cadetesInfo.json")){
                    listacadetes = accesoDatos.ObtenerListaCadetes("cadetesInfo.json");
                    cargaRealizada = true;
                }
                cad.AgregarListaCadetes(listacadetes);
            } 
        }

        return cargaRealizada;
    }

    public void AgregarListaCadetes(List<Cadete> listaCadetes){
        this.listaCadetes = listaCadetes;
    }

    public bool DarAltaPedido(string obsPedido, string nombreCliente, string direccionCliente, string telCliente, string datosReferenciaDireccionCliente){
        int nroPedido = listaPedidos.Count + 1;
        Pedido ped = new Pedido(nroPedido, obsPedido, nombreCliente, direccionCliente, telCliente, datosReferenciaDireccionCliente);
        bool pedidoAgregado = AgregarPedidoALista(ped);
        return pedidoAgregado;

    }
    
    public bool AgregarPedidoALista(Pedido ped){
        bool agregado = false;

        if(ped != null){
            listaPedidos.Add(ped);
            agregado = true;
        }

        return agregado;
    }
}