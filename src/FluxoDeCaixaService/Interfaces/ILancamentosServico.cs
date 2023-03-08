using FluxoDeCaixa.Service.Entidades;

namespace FluxoDeCaixa.Service.Interfaces;

public interface ILancamentosServico
{
    /// <summary>
    /// Adiciona um novo lançamento e retorna um objeto "OperationResult<Lancamento>" que indica se a operação foi bem sucedida ou não, juntamente com uma mensagem de erro, se aplicável, e o próprio objeto "Lancamento".
    /// </summary>
    Task<OperationResult<Lancamento>> AdicionarLancamento(Lancamento lancamento);

    /// <summary>
    /// Obtém todos os lançamentos existentes e retorna um objeto "OperationResult<Lancamento>" que indica se a operação foi bem sucedida ou não, juntamente com uma mensagem de erro, se aplicável, e uma coleção de objetos "Lancamento".
    /// </summary>
    Task<OperationResult<Lancamento>> ObterLancamentos();

    /// <summary>
    /// Obtém o saldo diário para a data especificada e retorna um objeto "OperationResult<decimal>" que indica se a operação foi bem sucedida ou não, juntamente com uma mensagem de erro, se aplicável, e um valor decimal representando o saldo diário.
    /// </summary>
    Task<OperationResult<decimal>> ObterSaldoDiario(string data);
}
