using FluxoDeCaixa.Service.Entidades;

namespace FluxoDeCaixa.Service.Interfaces;

public interface ILancamentosRepositorio
{
    /// <summary>
    /// Adiciona um novo lançamento ao repositório.
    /// </summary>
    /// <param name="lancamento">O objeto "Lancamento" a ser adicionado.</param>
    Task AdicionarLancamento(Lancamento lancamento);

    /// <summary>
    /// Obtém todos os lançamentos do repositório que possuem a data especificada.
    /// </summary>
    /// <param name="data">A data dos lançamentos a serem obtidos.</param>
    /// <returns>Uma coleção de objetos "Lancamento" que possuem a data especificada.</returns>
    Task<IEnumerable<Lancamento>> ObterLancamentosPorData(DateTime data);

    /// <summary>
    /// Obtém todos os lançamentos do repositório.
    /// </summary>
    /// <returns>Uma coleção de todos os objetos "Lancamento" existentes no repositório.</returns>
    Task<IEnumerable<Lancamento>> ObterLancamentos();
}

