using Microsoft.AspNetCore.Mvc;
namespace tl2_tp4_2023_adanSmith01.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private readonly ILogger<CadeteriaController> _logger;
    private Cadeteria oca;
    private List<Cadete> listaCadetes;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        oca = Cadeteria.GetCadeteria();
    }

    [HttpGet]
    [Route("InfoCadeteria")]
    public ActionResult<string> GetInfo(){
        if(String.IsNullOrEmpty(oca.Nombre)){
            return BadRequest("ERROR. Información no definida.");
        } else{
            string info = oca.Nombre + "," + oca.Telefono;
            return Ok(info);
        }
    }

    [HttpGet]
    [Route("InfoCadetes")]
    public ActionResult<IEnumerable<Cadete>> GetInfoCadetes(){
        if(oca.ListaCadetes.Count != 0){
            return Ok(oca.ListaCadetes);
        } else{
            return StatusCode(500, "ERROR en el servidor");
        }
    }

    [HttpGet]
    [Route("InfoPedidos")]
    public ActionResult<IEnumerable<Pedido>> GetInfoPedidos(){
        if(oca.ListaPedidos.Count != 0){
            return Ok(oca.ListaPedidos);
        }else{
            return BadRequest("ERROR en el servidor");
        }
    }

    [HttpPost("CargaDatos")]
    public ActionResult<string> CargaInicialDatos(string tipoAcceso){
        if(!oca.CargaDatosIniciales(tipoAcceso)){
            return StatusCode(500, "ERROR. No se cargaron los datos correctamente.");
        } else{
            return Ok("Datos cargados correctamente");
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
    
}
