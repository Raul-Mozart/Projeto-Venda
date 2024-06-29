using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Venda.Entidades;
using Venda.Service;

namespace VendaApi.Controllers
{
    [Route("api/[controller]")]
    public class DividaController : ControllerBase
    {
        private readonly DividaService service;

        public DividaController(DividaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetDividas(string busca = null)
        {
            var dados = busca == null ?
                            service.Listar() :
                            service.Listar(busca);
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dados = service.Retorna(id);
            return Ok(dados);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Divida divida)
        {
            var valido = service.Criar(divida, out List<ValidationResult> erros);
            return valido ? Ok(divida) : UnprocessableEntity(erros);
        }


        [HttpPut]
        public IActionResult Put([FromBody] Divida divida)
        {
            var valido = service.Editar(divida, out List<ValidationResult> erros);
            return valido ? Ok(divida) : UnprocessableEntity(erros);
        }

        [HttpDelete("{cursoId}")]
        public IActionResult Excluir(int cursoId)
        {
            var valido = service.Excluir(cursoId, out List<ValidationResult> erros);
            return valido ? Ok(valido) : UnprocessableEntity(erros);
        }

        [HttpGet("[action]")]
        public IActionResult Relatorio()
        {
            var dividas = service.ConsultaClientePorSituacao();
            return Ok(dividas);
        }
    }
}
