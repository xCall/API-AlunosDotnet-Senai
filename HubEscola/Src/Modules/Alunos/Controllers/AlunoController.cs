using HubEscola.Src.Modules.Alunos.Model;
using HubEscola.Src.Modules.Alunos.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HubEscola.Src.Modules.Alunos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AlunoController : ControllerBase
    {
        private readonly IOrganizationServiceAsync2 organizationServiceAsync2;
        private AlunoService service;

        public AlunoController(IOrganizationServiceAsync2 organizationServiceAsyn2)
        {
            this.organizationServiceAsync2 = organizationServiceAsyn2;
            this.service = new(this.organizationServiceAsync2);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAlunoRequest([FromBody] Aluno aluno)
        {
            
            var result = await this.service.CriarAluno(aluno);

            if (result.Success)
            {
                return Ok(new { Aluno = result.Data});
            }

            return BadRequest(new {message = $"Falha ao criar novo Aluno - Erro: {result.Error}"});
        }

        [HttpGet("{alunoId}")]
        public async Task<object> BuscaAlunoPorId(Guid alunoId)
        {
            var result = await this.service.BuscaAlunoPorId(alunoId);

            if (result.Success)
            {
                return Ok(new {Aluno = result.Data});
            }

            return BadRequest(new { message = $"Falha ao encontrar Aluno - Erro: {result.Error}" });
        }

        [HttpGet]
        public async Task<object> ListarAlunos()
        {
            var result = await this.service.ListaAlunos();

            if (result.Success)
            {
                return Ok(new { Aluno = result.Data });
            }

            return BadRequest(new { message = $"Falha ao encontrar Aluno - Erro: {result.Error}" });
        }

        [HttpPatch]
        public async Task<object> AtualizaAluno([FromBody] Aluno aluno)
        {
            var result = await this.service.AtualizaAluno(aluno);

            if (result.Success) {
                return Ok(new { Aluno = result.Data });
            }

            if (result.IsNotFound)
            {
                return NotFound(result.Message);
            }

            return BadRequest(new { message = $"Falha ao encontrar Aluno - Erro: {result.Error}" });
        }

        [HttpDelete("idAluno")]
        public async Task<IActionResult> ApagarAluno(Guid idAluno)
        {
            var result = await this.service.ApagarAluno(idAluno);

            if (result.Success)
            {
                return Ok(new { Message = "Aluno deletado com sucesso!" });
            }

            return BadRequest(new {message = $"Falha ao deletar aluno - Erro: {result.Error}"});
        }
        
    }
}
