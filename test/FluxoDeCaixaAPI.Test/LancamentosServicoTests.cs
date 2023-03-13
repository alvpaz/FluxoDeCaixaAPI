using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Enumeradores;
using FluxoDeCaixa.Service.Interfaces;
using FluxoDeCaixa.Service.Servicos;
using Moq;

namespace FluxoDeCaixaAPI.Test;

public class LancamentosServicoTests
{
    private readonly Mock<ILancamentosRepositorio> _mockLancamentosRepositorio;
    private readonly LancamentosServico _lancamentosServico;

    public LancamentosServicoTests()
    {
        _mockLancamentosRepositorio = new Mock<ILancamentosRepositorio>();
        _lancamentosServico = new LancamentosServico(_mockLancamentosRepositorio.Object);
    }

    [Fact]
    public async Task AdicionarLancamento_DeveRetornarOperacaoComSucesso()
    {
        // Arrange
        var lancamento = new Lancamento { Id = 1, Tipo = TipoLancamento.Receita, Valor = 100 };

        // Act
        var resultado = await _lancamentosServico.AdicionarLancamento(lancamento);

        // Assert
        Assert.True(resultado.Success);
        Assert.Null(resultado.ErrorMessage);
    }

    [Fact]
    public async Task AdicionarLancamento_DeveRetornarOperacaoComFalha_SeLancamentoForNulo()
    {
        // Arrange
        Lancamento lancamento = null;

        // Act
        var resultado = await _lancamentosServico.AdicionarLancamento(lancamento);

        // Assert
        Assert.False(resultado.Success);
        Assert.Equal("O lançamento é nulo", resultado.ErrorMessage);
    }

    [Fact]
    public async Task AdicionarLancamento_DeveRetornarOperacaoComFalha_SeLancamentoForInvalido()
    {
        // Arrange
        var lancamento = new Lancamento { Id = 1, Tipo = TipoLancamento.Receita, Valor = -100 };

        // Act
        var resultado = await _lancamentosServico.AdicionarLancamento(lancamento);

        // Assert
        Assert.False(resultado.Success);
        Assert.Equal("O lançamento é inválido", resultado.ErrorMessage);
    }

    [Fact]
    public async Task ObterLancamentos_DeveRetornarOperacaoComSucesso_SeObterLancamentosComSucesso()
    {
        // Arrange
        var lancamentos = new List<Lancamento> { new Lancamento { Id = 1, Tipo = TipoLancamento.Receita, Valor = 100 } };
        _mockLancamentosRepositorio.Setup(m => m.ObterLancamentos()).ReturnsAsync(lancamentos);

        // Act
        var resultado = await _lancamentosServico.ObterLancamentos();

        // Assert
        Assert.True(resultado.Success);
        Assert.Null(resultado.ErrorMessage);
        Assert.Equal(lancamentos, resultado.Result);
    }

    [Fact]
    public async Task ObterLancamentos_DeveRetornarOperacaoComFalha_SeObterLancamentosComFalha()
    {
        // Arrange
        _mockLancamentosRepositorio.Setup(m => m.ObterLancamentos()).ReturnsAsync((List<Lancamento>)null);

        // Act
        var resultado = await _lancamentosServico.ObterLancamentos();

        // Assert
        Assert.False(resultado.Success);
        Assert.Equal("Não foi possível obter os lançamentos", resultado.ErrorMessage);
    }

}