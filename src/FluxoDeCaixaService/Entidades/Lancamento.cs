using FluxoDeCaixa.Service.Enumeradores;

namespace FluxoDeCaixa.Service.Entidades;

public class Lancamento
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public TipoLancamento Tipo { get; set; }
}