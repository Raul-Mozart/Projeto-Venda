using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Venda.Entidades;
using Venda.Service;
using Venda.TranferirDados;

namespace VendaApi.Controllers
{
    // /api/aluno
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService clienteService;
        private readonly IWebHostEnvironment env;

        public ClienteController(ClienteService clienteService, IWebHostEnvironment env)
        {
            this.clienteService = clienteService;
            this.env = env;
        }

        [HttpGet]
        public IActionResult Listar(string pesquisa, int page = 0, int pageSize = 0)
        {
            var alunos = string.IsNullOrEmpty(pesquisa) ?
                clienteService.Listar(page, pageSize) :
                clienteService.Listar(pesquisa, page, pageSize);
            return Ok(alunos);
        }

        [HttpGet("{codigo}")]
        public IActionResult GetByCodigo(int codigo)
        {
            var aluno = clienteService.Retorna(codigo);
            return Ok(aluno);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            var sucesso = clienteService.Cadastrar(cliente,
                out List<ValidationResult> erros);
            if (sucesso)
            {
                return Ok(cliente);
            }
            else
            {
                return UnprocessableEntity(erros);
            }
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            var sucesso = clienteService.Atualizar(cliente,
                out List<ValidationResult> erros);
            if (sucesso)
            {
                return Ok(cliente);
            }
            else if (erros.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return UnprocessableEntity(erros);
            }
        }

        [HttpDelete("{codigo}")]
        public IActionResult Remover(int codigo)
        {
            var cliente = clienteService.Excluir(codigo, out _);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cliente);
            }
        }

        [HttpGet("[action]")]
        public IActionResult Cadastro()
        {
            // ternário
            var cadastro = clienteService.ListarCadastro();
            return Ok(cadastro);
        }

        [HttpPost("[action]")]
        public IActionResult Cadastro([FromBody] DividaTranfDados dados)
        {
            var divida = new Cadastro
            {
                Cliente = new Cliente { Codigo = dados.ClienteCodigo },
                Divida = new Divida { Id = dados.DividaId },
            };
            return Ok();
        }

        /*
        [HttpPost("[action]/{codigo}")]
        public IActionResult UploadImagem(int codigo)
        {
            var aluno = clienteService.Retorna(codigo);
            if (aluno == null)
            {
                return NotFound();
            }
            var imagem = Request.Form.Files.FirstOrDefault();
            if (imagem == null)
            {
                return BadRequest();
            }

            using var ms = new MemoryStream();
            imagem.CopyTo(ms);
            ms.Position = 0;
            var nome = Guid.NewGuid().ToString() + ".png";

            if (!Directory.Exists($"{env.WebRootPath}/imagens"))
            {
                Directory.CreateDirectory($"{env.WebRootPath}/imagens");
            }

            System.IO.File.WriteAllBytes($"{env.WebRootPath}/imagens/{nome}", ms.ToArray());

            aluno.PhotoUrl = $"http://172.16.102.202:5046/imagens/{nome}";

            clienteService.Atualizar(aluno, out _);

            return Ok(new { aluno.PhotoUrl });
        }*/

    }
}
