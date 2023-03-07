using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Enumeradores;

namespace FluxoDeCaixa.Repositorio.Entidades;

public class LancamentoLiteDb
{
    public int Id { get; init; }
    public string Descricao { get; init; } = string.Empty;
    public decimal Valor { get; init; }
    public DateTime Data { get; init; }
    public TipoLancamento Tipo { get; init; }

    public static LancamentoLiteDb FromLancamento(Lancamento lancamento)
    {
        return new LancamentoLiteDb
        {
            Id = lancamento.Id,
            Descricao = lancamento.Descricao,
            Valor = lancamento.Valor,
            Data = lancamento.Data,
            Tipo = lancamento.Tipo
        };
    }

    public Lancamento ParaLancamento()
    {
        return new Lancamento
        {
            Id = Id,
            Descricao = Descricao,
            Valor = Valor,
            Data = Data,
            Tipo = Tipo
        };
    }
}
