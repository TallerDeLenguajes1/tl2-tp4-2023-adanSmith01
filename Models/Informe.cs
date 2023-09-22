namespace tl2_tp4_2023_adanSmith01;

public class Informe
{
    private int cantCadetes;
    private List<int> idsCadetes;
    private List<string> nombresCadetes;
    private List<int> cantPedidosEntregadosCadetes;
    private List<double> montosCadetes;
    private int totalPedidosEntregados;
    private int cantPromedioDePedidosEntregados;

    public int CantCadetes {get => cantCadetes;}
    public List<int> IdsCadetes {get => idsCadetes;}
    public List<string> NombresCadetes {get => nombresCadetes;}
    public List<int> CantPedidosEntregadosCadetes { get => cantPedidosEntregadosCadetes;}
    public List<double> MontosCadetes {get => montosCadetes;}
    public int TotalPedidosEntregados {get => totalPedidosEntregados;}
    public int CantPromedioDePedidosEntregados {get => cantPromedioDePedidosEntregados;}

    public Informe(int cantCadetes, List<int> idsCadetes, List<string> nombresCadetes, List<int> cantPedidosEntregadosCadetes, List<double> montosCadetes, int totalPedidosEntregados, int cantPromedioDePedidosEntregados){
        this.cantCadetes = cantCadetes;
        this.idsCadetes = idsCadetes;
        this.nombresCadetes = nombresCadetes;
        this.cantPedidosEntregadosCadetes = cantPedidosEntregadosCadetes;
        this.montosCadetes = montosCadetes;
        this.totalPedidosEntregados = totalPedidosEntregados;
        this.cantPromedioDePedidosEntregados = cantPromedioDePedidosEntregados;
    }
}