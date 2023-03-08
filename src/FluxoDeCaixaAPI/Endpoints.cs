using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Serilog;

namespace FluxoDeCaixa.API;

public static class Endpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/lancamentos", async (Lancamento lancamento, ILancamentosServico lancamentosServico) =>
        {
            try
            {
                var result = await lancamentosServico.AdicionarLancamento(lancamento);

                return result.Success 
                    ? Results.Ok() 
                    : Results.BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocorreu um erro ao adicionar lançamento");
                return Results.Problem("Ocorreu um erro ao adicionar lançamento");
            }
        });

        app.MapGet("/lancamentos", async (ILancamentosServico lancamentosServico) =>
        {
            try
            {
                var lancamentos = await lancamentosServico.ObterLancamentos();
                return lancamentos.Success
                    ? Results.Ok()
                    : Results.BadRequest(lancamentos.ErrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocorreu um erro ao obter lançamentos");
                return Results.Problem("Ocorreu um erro ao obter lançamentos");
            }
        });

        app.MapGet("/saldo-diario/{data}", async (string data, ILancamentosServico lancamentosServico) =>
        {
            try
            {
                var saldoDiario = await lancamentosServico.ObterSaldoDiario(data);
                return saldoDiario.Success
                    ? Results.Ok()
                    : Results.BadRequest(saldoDiario.ErrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocorreu um erro ao obter lançamentos por data");
                return Results.Problem("Ocorreu um erro ao obter lançamentos por data");
            }
           
        });

        app.MapGet("/saldo-diario-relatorio/{data}", async (HttpContext httpContext, string data, ILancamentosServico lancamentosServico) =>
        {
            try
            {
                return await ObterSaldoDiario(data, lancamentosServico);
            }
            catch (Exception ex)
            {
                // Registra o erro
                Log.Error(ex, "Ocorreu um erro ao obter lançamentos");

                // Define o código de status da resposta como 400 (Bad Request) 
                httpContext.Response.StatusCode = 400;

                // Escreve uma resposta JSON personalizada com uma mensagem de erro
                await httpContext.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro ao obter lançamentos" });
            }

            return null;
        });

        async Task<FileContentResult> ObterSaldoDiario(string data, ILancamentosServico lancamentosServico)
        {
            var saldoDiario = await lancamentosServico.ObterSaldoDiario(data);

            using var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            document.Add(new Paragraph($"Saldo diário para {data}: {saldoDiario}"));
            document.Close();
            var bytes = memoryStream.ToArray();

            var result = new FileContentResult(bytes, "application/pdf")
            {
                FileDownloadName = $"saldo-diario-relatorio-{data}.pdf" // Define o nome do arquivo para download
            };

            return result;
        }

    }
}

 