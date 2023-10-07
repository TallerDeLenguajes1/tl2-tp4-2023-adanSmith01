namespace tl2_tp4_2023_adanSmith01;

public class Cadeteria
{
    private AccesoADatosPedidos accesoDatosPedidos;
    private  AccesoADatosCadeteria accesoDatosCadeteria;
    private AccesoADatosCadetes accesoDatosCadetes;
    private static AccesoDatos accesoDatos;
    private static Cadeteria instance;
    private string nombre;
    private string telefono;
    private List<Pedido> listaPedidos;
    private List<Cadete> listaCadetes;

    public string Nombre { get => nombre; set => nombre = value;}
    public string Telefono { get => telefono; set => telefono = value;}
    public List<Pedido> ListaPedidos {get => accesoDatosPedidos.ObtenerListaPedidos();}
    public List<Cadete> ListaCadetes { get => accesoDatosCadetes.ObtenerListaCadetes();}

    public Cadeteria(){}
    public Cadeteria(string nombre, string telefono){
        this.nombre = nombre;
        this.telefono = telefono;
    }

    public static Cadeteria GetCadeteria(){
        if(instance == null){
            var accesoDatosCadeteria = new AccesoADatosCadeteria();
            if(accesoDatosCadeteria.ObtenerInfoCadeteria() != null){
                instance = accesoDatosCadeteria.ObtenerInfoCadeteria();
                instance.accesoDatosCadetes = new AccesoADatosCadetes();
                instance.accesoDatosPedidos = new AccesoADatosPedidos();
            } 
        }
        return instance;
    }

    public bool GuardarCadete(Cadete cadeteNuevo){
        bool guardadoRealizado = false;
        if(cadeteNuevo != null){
            var cadetes = accesoDatosCadetes.ObtenerListaCadetes();
            cadetes.Add(cadeteNuevo);
            guardadoRealizado = true;
            accesoDatosCadetes.GuardarListaCadetes(cadetes);
        }

        return guardadoRealizado;
    }

    public bool DarAltaPedido(string obsPedido, string nombreCliente, string direccionCliente, string telCliente, string datosReferenciaDireccionCliente){
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        int nroPedido = listaPedidos.Count + 1;
        Pedido ped = new Pedido(nroPedido, obsPedido, nombreCliente, direccionCliente, telCliente, datosReferenciaDireccionCliente);
        bool pedidoAgregado = AgregarPedidoALista(ped, listaPedidos);
        return pedidoAgregado;
    }
    
    public bool AgregarPedidoALista(Pedido ped, List<Pedido> listaPedidos){
        bool agregado = false;

        if(ped != null){
            listaPedidos.Add(ped);
            instance.accesoDatosPedidos.GuardarLista(listaPedidos);
            agregado = true;
        }

        return agregado;
    }

    public bool AsignarCadeteAPedido(int idCadete, int nroPedido){
        bool asignacionRealizada = false;
        List<Cadete> listaCadetes = accesoDatosCadetes.ObtenerListaCadetes();
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        Cadete cad = listaCadetes.FirstOrDefault(x => x.Id == idCadete);
        Pedido ped = listaPedidos.FirstOrDefault(pedido => pedido.Nro == nroPedido);

        if(cad != null && ped != null){
            ped.VincularCadete(idCadete);
            asignacionRealizada = true;
            instance.accesoDatosPedidos.GuardarLista(listaPedidos);
        }

        return asignacionRealizada;
    }

    public bool CambiarEstadoPedido(int nroPedido, int nuevoEstado){
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        foreach (var p in listaPedidos){
            if(p.Nro == nroPedido) {
                p.CambiarEstado(nuevoEstado);
                instance.accesoDatosPedidos.GuardarLista(listaPedidos);
                return true;
            }
        }

        return false;
    }

    public bool ReasignarPedidoACadete(int nroPedido, int idCadete){
        bool reasignacionRealizada = false;
        List<Cadete> listaCadetes = accesoDatosCadetes.ObtenerListaCadetes();
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        Cadete cad = listaCadetes.Find(cadete => cadete.Id == idCadete);
        Pedido ped = listaPedidos.FirstOrDefault(pedido => pedido.Nro == nroPedido && pedido.Estado != EstadoPedido.Entregado);

        if(cad != null && ped != null){
            ped.VincularCadete(idCadete);
            reasignacionRealizada = true;
            instance.accesoDatosPedidos.GuardarLista(listaPedidos);
        }

        return reasignacionRealizada;
    }

    private int CantPedidosCadete(int idCadete, EstadoPedido estado){
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        int cant = listaPedidos.Count(pedido => pedido.IdCadete == idCadete && pedido.Estado == estado);
        return cant;
    }

    private double JornalACobrar(int idCadete){
        return ((double)500 * CantPedidosCadete(idCadete, EstadoPedido.Entregado));
    }

    public Informe CrearInforme(){
        List<Cadete> listaCadetes = accesoDatosCadetes.ObtenerListaCadetes();
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

    public Pedido ObtenerPedido(int nroPedido){
        var listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        Pedido ped = listaPedidos.FirstOrDefault(p => p.Nro == nroPedido);
        return ped;
    }

    public Cadete ObtenerCadete(int idCadete){
        var listaCadetes = accesoDatosCadetes.ObtenerListaCadetes();
        Cadete cad = listaCadetes.FirstOrDefault(cadete => cadete.Id == idCadete);
        return cad;
    }
}