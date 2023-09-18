namespace tl2_tp4_2023_adanSmith01;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

public enum EstadoPedido
{
    [EnumMember(Value = "Entregado")]
    Entregado,

    [EnumMember(Value = "Pendiente")]
    Pendiente
}

public class Pedido
{
     private int nro;
    private string observaciones;
    private EstadoPedido estado;
    private Cliente cliente;
    private Cadete cadete;

    public int Nro{get => nro; set => nro = value;}
    public string Observaciones{get => observaciones; set => observaciones = value;}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EstadoPedido Estado{get => estado; set => estado = value;}

    public Cadete Cadete{get => cadete; set => cadete = value;}

    public Cliente Cliente{get => cliente; set => cliente = value;}

    public Pedido(){
        this.cliente = new Cliente();
    }
    public Pedido (int nro, string observaciones, string nombre, string direccion, string telefono, string datosReferenciaDireccion){
        this.nro = nro;
        this.observaciones = observaciones;
        this.estado = EstadoPedido.Pendiente;
        this.cliente = new Cliente(nombre, direccion, telefono, datosReferenciaDireccion);
    }
    
    public void VincularCadete(Cadete cad){
        cadete = cad;
    }
}