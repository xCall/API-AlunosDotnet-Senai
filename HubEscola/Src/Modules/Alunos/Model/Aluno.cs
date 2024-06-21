using System;
using Microsoft.Xrm.Sdk;
namespace HubEscola.Src.Modules.Alunos.Model
{
    public class Aluno
    {
       public Guid? Id { get; set; }
       public required string Name { get; set; }
       public required string CPF { get; set; }
       public Guid? Curso { get; set; }
       public string? NomeDoCurso { get; set; }
    }
}
