
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using HubEscola.Src.Modules.Alunos.Model;
using HubEscola.Src.Shared.Utils;
using Microsoft.Xrm.Sdk.Query;

namespace HubEscola.Src.Modules.Alunos.Service
{
    public class AlunoService
    {
        private IOrganizationServiceAsync2 organizationServiceAsync2;
        public AlunoService(IOrganizationServiceAsync2 organizationServiceAsync2) {
            this.organizationServiceAsync2 = organizationServiceAsync2;
        }

        public async Task<ServiceResult> CriarAluno(Aluno aluno)
        {
            try
            {
                Entity alunoEntity = new("tre_aluno");

                EntityReference curso = null;

                if (aluno.Curso.HasValue && aluno.Curso.Value != Guid.Empty)
                {

                    curso = new("tre_curso", aluno.Curso.Value);
                    alunoEntity["tre_curso"] = curso;
                }

                alunoEntity["tre_name"] = aluno.Name;
                alunoEntity["tre_cpf"] = aluno.CPF;
                

                Guid alunoId = await this.organizationServiceAsync2.CreateAsync(alunoEntity);

                object result = new Aluno
                {
                    Id = alunoId,
                    Name = aluno.Name,
                    CPF = aluno.CPF,
                    Curso = aluno.Curso,
                };

                return ServiceResult.SuccessResultData(result);
            }
            catch (Exception ex) {
                return ServiceResult.FailureResult(ex);
            }
        }
        public async Task<ServiceResult> BuscaAlunoPorId(Guid alunoId)
        {

            try
            {
                ColumnSet columns = new("tre_name", "tre_cpf", "tre_curso", "tre_alunoid");


                Entity entity = await this.organizationServiceAsync2.RetrieveAsync("tre_aluno", alunoId, columns);

                object result = new Aluno
                {
                    Id = entity.GetAttributeValue<Guid>("tre_alunoid"),
                    Name = entity.GetAttributeValue<string>("tre_name"),
                    CPF = entity.GetAttributeValue<string>("tre_cpf"),
                    Curso = entity.GetAttributeValue<EntityReference>("tre_curso") == null ? Guid.Empty : entity.GetAttributeValue<EntityReference>("tre_curso").Id,
                    NomeDoCurso = entity.GetAttributeValue<EntityReference>("tre_curso")?.Name,
                };

                return ServiceResult.SuccessResultData(result);
            }
            catch (Exception ex)
            {
                return ServiceResult.FailureResult(ex);
            }

        }

        public async Task<ServiceResult> ListaAlunos()
        {
            try
            {
                ColumnSet columns = new("tre_name", "tre_cpf", "tre_curso", "tre_alunoid");

                QueryExpression query = new("tre_aluno");
                FilterExpression filter = new();
                ConditionExpression condition = new("tre_curso", ConditionOperator.Equal, "f0ae011c-a82e-ef11-8e4e-00224835e395");
                filter.Conditions.Add(condition);

                query.Criteria = filter;
                query.ColumnSet = columns;

                EntityCollection result = await this.organizationServiceAsync2.RetrieveMultipleAsync(query);


                // Tipo de retorno que pode existir
                //if (result.Entities.Count > 0)
                //{
                   
                //    return ServiceResult.SuccessResultData(result);

                //}

                List<Aluno> alunos = [];

                foreach (Entity entity in result.Entities) {
                    alunos.Add(new Aluno
                    {   
                        Id = entity.Id,
                        Curso = entity.GetAttributeValue<EntityReference>("tre_curso").Id,
                        NomeDoCurso = entity.GetAttributeValue<EntityReference>("tre_curso").Name,
                        Name = entity.GetAttributeValue<string>("tre_name"),
                        CPF = entity.GetAttributeValue<string>("tre_cpf"),
                    });
                }


                if (alunos.Count > 0) {
                    return ServiceResult.SuccessResultData(alunos);
                }

                return ServiceResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return ServiceResult.FailureResult(ex);

            }
        }

        public async Task<ServiceResult> AtualizaAluno(Aluno aluno)
        {
            try
            {

                if (aluno.Id.HasValue && aluno.Id.Value != Guid.Empty)
                {
                    ColumnSet columns = new("tre_name", "tre_cpf", "tre_curso", "tre_alunoid");
                    if(aluno.Id.HasValue && aluno.Id.Value != Guid.Empty)
                    {
                        Entity entity = await this.organizationServiceAsync2.RetrieveAsync("tre_aluno", aluno.Id.Value, columns);
                        if (entity != null)
                        {

                            if (aluno.Name != null)
                                entity["tre_name"] = aluno.Name;

                            if (aluno.CPF != null)
                                entity["tre_cpf"] = aluno.CPF;

                            if (aluno.Curso.HasValue && aluno.Curso.Value != Guid.Empty)
                            {
                                EntityReference curso = new("tre_curso", aluno.Curso.Value);
                                entity["tre_curso"] = curso;
                            }

                            await this.organizationServiceAsync2.UpdateAsync(entity);

                            return ServiceResult.SuccessResultData(entity);
                        }

                    }


                    return ServiceResult.NotFound("Aluno não foi encontrado na base!");
                }

                return ServiceResult.NotFound("Id do aluno não pode ser vazio!");
            }
            catch (Exception ex)
            {
                return ServiceResult.FailureResult(ex);
            }
        }

        public async Task<ServiceResult> ApagarAluno(Guid idAluno)
        {
            try {
                await this.organizationServiceAsync2.DeleteAsync("tre_aluno", idAluno);

                return ServiceResult.SuccessResult();
            }
            catch (Exception ex) { 
                return ServiceResult.FailureResult(ex);
            }
        }
    }
}
