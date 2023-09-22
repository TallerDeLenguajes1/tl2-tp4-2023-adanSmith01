namespace tl2_tp4_2023_adanSmith01;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Cadeteria
{
    private static AccesoDatosCadeteria accesoDatos;
    private static Cadeteria cad;
    private string nombre;
    private string telefono;
    private List<Cadete> listaCadetes;
    private List<Pedido> listaPedidos;

    public string Nombre { get => nombre; set => nombre = value;}
    public string Telefono { get => telefono; set => telefono = value;}
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

    public bool AsignarCadeteAPedido(int idCadete, int nroPedido){
        bool asignacionRealizada = false;
        Cadete cad = listaCadetes.Find(x => x.Id == idCadete);

        if(cad != null){
            foreach(var p in listaPedidos){
                if(p.Nro == nroPedido){
                    p.VincularCadete(cad);
                    asignacionRealizada = true;
                    break;
                }
            }
        }

        return asignacionRealizada;
    }

    public bool CambiarEstadoPedido(int nroPedido, int nuevoEstado){
        foreach (var p in listaPedidos){
            if(p.Nro == nroPedido) {
                p.CambiarEstado(nuevoEstado);
                return true;
            }
        }

        return false;
    }

    public bool ReasignarPedidoACadete(int nroPedido, int idCadete){
        bool reasignacionRealizada = false;
        Cadete cad = listaCadetes.Find(cadete => cadete.Id == idCadete);

        if(cad != null){
            foreach(var p in listaPedidos){
                if(p.Nro == nroPedido && p.Estado != EstadoPedido.Entregado){
                    p.VincularCadete(cad);
                    reasignacionRealizada = true;
                }
            }
        }

        return reasignacionRealizada;
    }

    public int CantPedidosCadete(int idCadete, EstadoPedido estado){
        int cant = 0;
        foreach(var p in listaPedidos){
            if((p.ExisteCadete()) && (p.IdCadete() == idCadete) && (p.Estado == estado)) cant++;
        }

        return cant;
    }
    public double JornalACobrar(int idCadete){
        return ((double)500 * CantPedidosCadete(idCadete, EstadoPedido.Entregado));
    }

    public Informe CrearInforme(){
        List<int> idsCadetes = listaCadetes.Select(cad => cad.Id).ToList();
        List<string> nombresCadetes = listaCadetes.Select(cad => cad.Nombre).ToList();

        List<int> cantPedidosEntregadosCadetes = new List<int>();
        List<double> montosCadetes = new List<double>();
        foreach(var cad in listaCadetes){
            cantPedidosEntregadosCadetes.Add(CantPedidosCadete(cad.Id, EstadoPedido.Entregado));
            montosCadetes.Add(JornalACobrar(cad.Id));
        }
        
        int totalPedidosEntregados = cantPedidosEntregadosCadetes.Sum();
        int cantPromedioDePedidosEntregados = (int)cantPedidosEntregadosCadetes.Average();

        Informe informe = new Informe(listaCadetes.Count, idsCadetes, nombresCadetes, cantPedidosEntregadosCadetes, montosCadetes, totalPedidosEntregados, cantPromedioDePedidosEntregados);
        return informe;
    }
}