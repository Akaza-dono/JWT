using CRUD.Data;
using CRUD.DDBBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace CRUD.API
{
    [Route("[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        [HttpGet("GetUsuarios")]
        public IActionResult GetUsuarios()
        {
            using(CRUDbContext context = new())
            {
                var query = context.Usuarios.ToList();
                if (query == null)
                    return BadRequest("No hay usuarios por mostrar");
                return Ok(new { message = "ok", usuarios = query });
            }
        }

        [HttpGet("GetUsuarioById")]
        public IActionResult GetUsuarios(int id)
        {
            using (CRUDbContext context = new())
            {
                var query = context.Usuarios.Where(e => e.IdUsuario == id).FirstOrDefault();
                return Ok(new { usuarios = query });
            }
        }

        [HttpDelete("DeleteUsaurio")]
        public IActionResult DeleteUsuario(int id) 
        {
            try
            {
                using (CRUDbContext context = new())
                {
                    if (id < 0)
                        return BadRequest("El id no debe de ser nulo");
                    var query = context.Usuarios.Where(e => e.IdUsuario == id).FirstOrDefault();
                    if(query != null)
                        query.IsActive = false;
                        context.SaveChanges();
                        return Ok(new { message = "Usuario Eliminado correctamente" });
                }
            }
            catch (Exception ex)
            {
                throw new RankException("El usuario no pudo ser eliminado", ex);
            }
        }

        [HttpPost("CreateUsuario")]
        public IActionResult CrearUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Usuario no puede ser nulo");
            }
            return Ok(new { creado = true });
        }

    }
}
