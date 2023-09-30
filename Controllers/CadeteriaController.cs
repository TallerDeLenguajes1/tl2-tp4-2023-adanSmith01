using Microsoft.AspNetCore.Mvc;
namespace tl2_tp4_2023_adanSmith01.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private bool datosCargados = false;
    private readonly ILogger<CadeteriaController> _logger;
    private Cadeteria oca;
    private List<Cadete> listaCadetes;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        oca = Cadeteria.GetCadeteria();
        if(oca != null) datosCargados = true;
    }

    [HttpGet]
    [Route("InfoCadeteria")]
    public ActionResult<string> GetInfo(){
        if(!datosCargados){
            return NotFound("ERROR. Información no definida.");
        } else{
            string info = oca.Nombre + "," + oca.Telefono;
            return Ok(info);
        }
    }

    [HttpGet]
    [Route("InfoCadetes")]
    public ActionResult<IEnumerable<Cadete>> GetInfoCadetes(){
        if(!datosCargados){
            return NotFound("ERROR. Información no encontrada.");
        } else{
            List<Cadete> lista = oca.ListaCadetes;
            if(lista.Count != 0){
                return Ok(lista);
            }else{
                return StatusCode(500, "ERROR en el servidor");
            }
        }
    }

    [HttpGet]
    [Route("InfoPedidos")]
    public ActionResult<IEnumerable<Pedido>> GetInfoPedidos(){
        List<Pedido> lista = oca.ListaPedidos;
        if(lista.Count != 0){
            return Ok(lista);
        }else{
            return NotFound("ERROR. Recurso no encontrado");
        }
    }

    [HttpGet]
    [Route("Informe")]
    public ActionResult<Informe> GetInforme(){
        if(!datosCargados){
            return NotFound("ERROR. Acceso a recurso denegado.");
        } else{
            return Ok(oca.CrearInforme());
        }
    }

    [HttpGet]
    [Route("BuscarPedido/{nro}")]
    public ActionResult<Pedido> GetPedido(int nro){
        var ped = oca.ObtenerPedido(nro);
        if(ped != null){
            return Ok(ped);
        } else{
            return NotFound("No se encontro el pedido solicitado");
        }
    }

    [HttpGet]
    [Route("BuscarCadete/{idCadete}")]
    public ActionResult<Cadete> GetCadete(int idCadete){
        var cadete = oca.ObtenerCadete(idCadete);
        if(cadete != null){
            return Ok(cadete);
        } else{
            return NotFound("No se encontro el cadete solicitado");
        }
    }

    [HttpPost("AltaCadete")]
    public ActionResult<string> DarAltaCadete(string nombreCadete, string direccionCadete, string telCadete){
        if(String.IsNullOrEmpty(nombreCadete) || String.IsNullOrEmpty(direccionCadete) || String.IsNullOrEmpty(telCadete)){
            return BadRequest("ERROR. No se ingresaron los datos necesarios.");
        }else{
            Cadete nuevoCadete = new Cadete(oca.ListaCadetes.Count, nombreCadete, direccionCadete, telCadete);
            if(!oca.GuardarCadete(nuevoCadete)) return StatusCode(500, "ERROR en el servidor");
            return Ok("Se ha dado de alta al cadete correctamente");
        }
    }

    [HttpPost("DarAltaPedido")]
    public ActionResult<string> DarAltaPedido(string obsPedido, string nombreCliente, string direccionCliente, string telCliente, string datosReferenciaDireccionCliente){
        if(!oca.DarAltaPedido(obsPedido, nombreCliente,direccionCliente,telCliente, datosReferenciaDireccionCliente)){
            return BadRequest("ERROR. El pedido no puede ser registrado");
        } else{
            return Ok("Pedido dado de alta exitosamente");
        }
    }

    [HttpPut("AsignarPedido")]
    public ActionResult<string> AsignacionP(int idCadete, int nroPedido){
        if(!oca.AsignarCadeteAPedido(idCadete, nroPedido)){
            return BadRequest("ERROR. ID de cadete o nro pedido no existentes");
        } else{
            return Ok("Asignación realizada con éxito");
        }
    }

    [HttpPut("CambiarEstadoPedido")]
    public ActionResult<string> CambiarEstadoPedido(int nroPedido, int nuevoEstado){
        if(!oca.CambiarEstadoPedido(nroPedido, nuevoEstado)){
            return BadRequest("ERROR. No se pudo completar la operacion");
        } else{
            return Ok("Cambio de estado del pedido realizado exitosamente");
        }
    }

    [HttpPut("CambiarCadetePedido")]
    public ActionResult<string> ReasignarPedido(int nroPedido, int idCadeteAReasignar){
        if(!oca.ReasignarPedidoACadete(nroPedido, idCadeteAReasignar)){
            return BadRequest("ERROR. No se puede realizar la operacion");
        } else{
            return Ok("Cambio de cadete a pedido realizado exitosamente");
        }
    }
}
