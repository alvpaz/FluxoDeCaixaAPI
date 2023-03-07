using AutoMapper;
using FluxoDeCaixa.Repositorio.Entidades;
using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Interfaces;
using LiteDB;

namespace FluxoDeCaixa.Repositorio.Repositorios
{
    public class LancamentosRepositorio : ILancamentosRepositorio
    {
        private readonly LiteDatabase _db;
        private readonly IMapper _mapper;

        public LancamentosRepositorio(LiteDatabase db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task AdicionarLancamento(Lancamento lancamento)
        {
            var lancamentosCollection = _db.GetCollection<LancamentoLiteDb>("lancamentos");
            var lancamentoLiteDb = _mapper.Map<LancamentoLiteDb>(lancamento);
 
            await Task.Run(() => lancamentosCollection.Insert(lancamentoLiteDb));
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentosPorData(DateTime data)
        {
            var lancamentosCollection = _db.GetCollection<LancamentoLiteDb>("lancamentos");
            var lancamentos = await Task.Run(() => lancamentosCollection.Find(l => l.Data.Date == data.Date).ToList());
            var lancamentosEntidade = _mapper.Map<IEnumerable<Lancamento>>(lancamentos);
            return lancamentosEntidade;
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentos()
        {
            var lancamentosCollection = _db.GetCollection<LancamentoLiteDb>("lancamentos");
            var lancamentos = await Task.Run(() => lancamentosCollection.FindAll().ToList());
            var lancamentosEntidade = _mapper.Map<List<Lancamento>>(lancamentos);
            return lancamentosEntidade;
        }
    }
}