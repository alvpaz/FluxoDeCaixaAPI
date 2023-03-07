using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Enumeradores;
using FluxoDeCaixa.Service.Interfaces;

namespace FluxoDeCaixa.Service.Servicos
{
    public class LancamentosServico : ILancamentosServico
    {
        private readonly ILancamentosRepositorio _lancamentosRepositorio;

        public LancamentosServico(ILancamentosRepositorio lancamentosRepositorio)
        {
            _lancamentosRepositorio = lancamentosRepositorio;
        }

        public async Task AdicionarLancamento(Lancamento lancamento)
        {
            if (lancamento == null)
                throw new ArgumentNullException(nameof(lancamento));

            await _lancamentosRepositorio.AdicionarLancamento(lancamento);
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentos()
        {
            return await _lancamentosRepositorio.ObterLancamentos();
        }

        public async Task<decimal> ObterSaldoDiario(string data)
        {
            DateTime.TryParse(data, out var dataLancamento);
            var lancamentos = await _lancamentosRepositorio.ObterLancamentosPorData(dataLancamento);

            decimal receitas = lancamentos
                .Where(l => l.Tipo == TipoLancamento.Receita)
                .Sum(l => l.Valor);

            decimal pagamentos = lancamentos
                .Where(l => l.Tipo == TipoLancamento.Pagamento)
                .Sum(l => l.Valor);

            decimal despesas = lancamentos
                .Where(l => l.Tipo == TipoLancamento.Despesa)
                .Sum(l => l.Valor);

            var saldo = receitas - pagamentos - despesas;

            return saldo;
        }

    }
}