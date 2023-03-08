using FluxoDeCaixa.Service.Enumeradores;

namespace FluxoDeCaixa.Service.Entidades;

public class Lancamento
{
    /// <summary>
    /// Identificador único do lançamento.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Descrição do lançamento. Não pode ser nulo nem vazio.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor do lançamento. Deve ser maior que zero.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Data do lançamento. Deve ser uma data válida.
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Tipo do lançamento (crédito ou débito).
    /// </summary>
    public TipoLancamento Tipo { get; set; }

    /// <summary>
    /// Verifica se o objeto Lancamento é válido, ou seja, se todos os seus campos estão preenchidos corretamente.
    /// </summary>
    /// <returns>Retorna true se o objeto Lancamento é válido e false caso contrário.</returns>
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Descricao))
            return false;

        if (Valor <= 0)
            return false;

        if (Data == default(DateTime))
            return false;

        return true;
    }
}
