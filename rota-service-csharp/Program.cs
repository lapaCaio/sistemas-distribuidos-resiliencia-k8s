using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var circuitBreakerPolicy = Policy
    .Handle<Exception>()
    .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(10));

app.MapGet("/api/rotas/calcular", async (string origem, string destino) =>
{
    try
    {
        return await circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            if (origem.Equals("Castelo", StringComparison.OrdinalIgnoreCase))
                return Results.BadRequest(new { erro = "Regra violada: Não deve ser possível sair de Castelo." });
            if (destino.Equals("Alegre", StringComparison.OrdinalIgnoreCase))
                return Results.BadRequest(new { erro = "Regra violada: Não deve ser possível voltar para Alegre." });

            await Task.Delay(200);
            return Results.Ok(new { origem = origem, destino = destino, distancia_km = new Random().Next(10, 500), status = "Sucesso" });
        });
    }
    catch (BrokenCircuitException)
    {
        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
});

app.Run();
