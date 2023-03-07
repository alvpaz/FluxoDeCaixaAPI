using FluxoDeCaixa.Service.Entidades;
using LiteDB;

namespace FluxoDeCaixa.Repositorio.Configurações;

public class LancamentosDbContext
{
    public LiteDatabase Db { get; }

    public LancamentosDbContext(LiteDatabase db)
    {
        Db = db;
    }

    public LiteCollection<Lancamento> Lancamentos => (LiteCollection<Lancamento>)Db.GetCollection<Lancamento>("lancamentos");

 

}
