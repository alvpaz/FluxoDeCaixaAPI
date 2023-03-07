using FluxoDeCaixa.Service.Entidades;

namespace FluxoDeCaixa.Service.Interfaces;

public interface ILancamentosServico
{
    Task AdicionarLancamento(Lancamento lancamento);
    Task<IEnumerable<Lancamento>> ObterLancamentos();
    Task<decimal> ObterSaldoDiario(string data);
}
