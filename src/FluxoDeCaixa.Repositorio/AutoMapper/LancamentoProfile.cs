using AutoMapper;
using FluxoDeCaixa.Repositorio.Entidades;
using FluxoDeCaixa.Service.Entidades;

namespace FluxoDeCaixa.Repositorio.AutoMapper;

public class LancamentoProfile : Profile
{
    public LancamentoProfile()
    {
        CreateMap<Lancamento, LancamentoLiteDb>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
            .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo));
    }
}