using FluxoDeCaixa.Service.Entidades;

namespace FluxoDeCaixa.Service.Interfaces;

public interface ILancamentosRepositorio
{
    Task AdicionarLancamento(Lancamento lancamento);
    Task<IEnumerable<Lancamento>> ObterLancamentosPorData(DateTime data);
    Task<IEnumerable<Lancamento>> ObterLancamentos();
}
