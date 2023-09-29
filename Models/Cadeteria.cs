namespace tl2_tp4_2023_adanSmith01;

public class Cadeteria
{
    private AccesoADatosPedidos accesoDatosPedidos;
    private  AccesoADatosCadeteria accesoDatosCadeteria;
    private AccesoADatosCadetes accesoDatosCadetes;
    private static AccesoDatos accesoDatos;
    private static Cadeteria instance;
    private List<Cadete> listaCadetes;
    private string nombre;
    private string telefono;

    public string Nombre { get => nombre; set => nombre = value;}
    public string Telefono { get => telefono; set => telefono = value;}
    public AccesoADatosPedidos AccesoADatosPedidos {get => accesoDatosPedidos;}
    public List<Cadete> ListaCadetes { get => listaCadetes; }

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
                if(instance.accesoDatosCadetes.ObtenerListaCadetes().Count != 0){
                    instance.AgregarListaCadetes(instance.accesoDatosCadetes.ObtenerListaCadetes());
                    instance.accesoDatosPedidos = new AccesoADatosPedidos();
                }
            } 
        }
        return instance;
    }

    public void AgregarListaCadetes(List<Cadete> listaCadetes){
        this.listaCadetes = listaCadetes;
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
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        bool asignacionRealizada = false;
        Cadete cad = listaCadetes.Find(x => x.Id == idCadete);

        if(cad != null){
            foreach(var p in listaPedidos){
                if(p.Nro == nroPedido){
                    p.VincularCadete(cad);
                    asignacionRealizada = true;
                    instance.accesoDatosPedidos.GuardarLista(listaPedidos);
                    break;
                }
            }
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
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        bool reasignacionRealizada = false;
        Cadete cad = listaCadetes.Find(cadete => cadete.Id == idCadete);

        if(cad != null){
            foreach(var p in listaPedidos){
                if(p.Nro == nroPedido && p.Estado != EstadoPedido.Entregado){
                    p.VincularCadete(cad);
                    reasignacionRealizada = true;
                    instance.accesoDatosPedidos.GuardarLista(listaPedidos);
                }
            }
        }

        return reasignacionRealizada;
    }

    public int CantPedidosCadete(int idCadete, EstadoPedido estado){
        List<Pedido> listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
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