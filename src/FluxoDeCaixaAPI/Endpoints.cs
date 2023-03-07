using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Interfaces;

namespace FluxoDeCaixa.API;

public static class Endpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/lancamentos", async (Lancamento lancamento, ILancamentosServico lancamentosServico) =>
        {
            await lancamentosServico.AdicionarLancamento(lancamento);

            return Results.Ok();
        });

        app.MapGet("/lancamentos", async (ILancamentosServico lancamentosServico) =>
        {
            var lancamentos = await lancamentosServico.ObterLancamentos();

            return Results.Ok(lancamentos);
        });

        app.MapGet("/saldo/{data}", async (string data, ILancamentosServico lancamentosServico) =>
        {
            var saldo = await lancamentosServico.ObterSaldoDiario(data);

            return Results.Ok(saldo);
        });
    }
}
