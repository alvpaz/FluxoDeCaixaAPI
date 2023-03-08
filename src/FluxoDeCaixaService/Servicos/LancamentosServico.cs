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

        public async Task<OperationResult<Lancamento>> AdicionarLancamento(Lancamento lancamento)
        {
            if (lancamento == null)
                return OperationResult<Lancamento>.Fail("O lançamento é nulo");

            if (!lancamento.IsValid())
                return OperationResult<Lancamento>.Fail("O lançamento é inválido");

            await _lancamentosRepositorio.AdicionarLancamento(lancamento);

            return OperationResult<Lancamento>.Ok();
        }

        public async Task<OperationResult<Lancamento>> ObterLancamentos()
        {
            var lancamentos = await _lancamentosRepositorio.ObterLancamentos();

            return lancamentos == null 
                ? OperationResult<Lancamento>.Fail("Não foi possível obter os lançamentos") 
                : OperationResult<Lancamento>.Ok(lancamentos);
        }

        public async Task<OperationResult<decimal>> ObterSaldoDiario(string data)
        {
            DateTime.TryParse(data, out var dataLancamento);
            var lancamentos = await _lancamentosRepositorio.ObterLancamentosPorData(dataLancamento);

            if(lancamentos == null)
                OperationResult<Lancamento>.Fail("Não foi possível obter os lançamentos");

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

            return OperationResult<decimal>.Ok(new[] { saldo }); 
        }

    }
}