using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrud.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Api.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            var RotasEstudantes = app.MapGroup("estudantes");


            /// CRIAR POST
            
            RotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) => 
            {

                var JaExiste = await context.Estudantes.AnyAsync(Estudante => Estudante.Nome == request.Nome);

                if(JaExiste)
                  return Results.Conflict("Ja Existe!");

                var NovoEstudante = new Estudante(request.Nome);

                await context.Estudantes.AddAsync(NovoEstudante);
                await context.SaveChangesAsync(ct);

                var EstudanteRetorno = new EstudanteDto(NovoEstudante.Id, NovoEstudante.Nome);

                return Results.Ok(NovoEstudante);

            });


            /// RETORNAR ESTUDANTES CADASTRADOS
            RotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) => 
            {
              
              var estudantes = await context
              
              .Estudantes
              .Where(estudantes => estudantes.Ativo)
              .Select(Estudante => new EstudanteDto(Estudante.Id, Estudante.Nome))

              .ToListAsync(ct);
              

              

              return estudantes; 

            });


            // ATUALIZAR NOME ESTUDANTE

            RotasEstudantes.MapPut("{id:guid}",
             async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) => 
             {

              var Estudante = await context.Estudantes
              .SingleOrDefaultAsync(Estudante => Estudante.Id == id);

              
              
              if (Estudante == null)

              return Results.NotFound();

              Estudante.AtualizarNome(request.Nome);

              await context.SaveChangesAsync(ct);
              return Results.Ok(Estudante);


             }); 

             ///////// DELETAR

             RotasEstudantes.MapDelete("{id}",
             async (Guid id, AppDbContext context, CancellationToken ct ) => 
             
             
             {

               var delestudante = await context.Estudantes
               .SingleOrDefaultAsync(delestudante => delestudante.Id == id);

               if (delestudante == null)
                   return Results.NotFound();



                  delestudante.Desativar();
                  await context.SaveChangesAsync(ct);
                  return Results.Ok();




             });
        }
    }
}