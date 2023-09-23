using Microsoft.AspNetCore.Mvc;
namespace tl2_tp4_2023_adanSmith01.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private static bool datosCargados = false;
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
            return Ok(oca.ListaCadetes);
        }
    }

    [HttpGet]
    [Route("InfoPedidos")]
    public ActionResult<IEnumerable<Pedido>> GetInfoPedidos(){
        if(oca.AccesoADatosPedidos.ObtenerListaPedidos().Count != 0){
            return Ok(oca.AccesoADatosPedidos.ObtenerListaPedidos());
        }else{
            return BadRequest("ERROR en el servidor");
        }
    }

    [HttpGet]
    [Route("Informe")]
    public ActionResult<Informe> GetInforme(){
        if(!datosCargados){
            return BadRequest("ERROR. Acceso a recurso no permitido.");
        } else{
            return Ok(oca.CrearInforme());
        }
    }

    [HttpPost("DarAltaPedido")]
    public ActionResult<string> DarAlta(string obsPedido, string nombreCliente, string direccionCliente, string telCliente, string datosReferenciaDireccionCliente){
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
