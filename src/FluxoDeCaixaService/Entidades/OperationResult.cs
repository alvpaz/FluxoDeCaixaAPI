namespace FluxoDeCaixa.Service.Entidades;

public class OperationResult<T>
{
    /// <summary>
    /// Indica se a operação foi bem sucedida ou não.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Contém uma mensagem de erro, caso a operação tenha falhado. Opcional e pode ser nulo.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Contém o resultado da operação, caso ela tenha sido bem sucedida. É uma coleção do tipo IEnumerable<T> e pode ser nulo.
    /// </summary>
    public IEnumerable<T>? Result { get; set; }

    /// <summary>
    /// Cria e retorna um novo objeto "OperationResult<T>" com "Success" definido como "false" e "ErrorMessage" definido como a mensagem de erro passada como argumento.
    /// </summary>
    public static OperationResult<T> Fail(string errorMessage)
    {
        return new OperationResult<T> { Success = false, ErrorMessage = errorMessage };
    }

    /// <summary>
    /// Cria e retorna um novo objeto "OperationResult<T>" com "Success" definido como "true" e "Result" definido como a coleção de resultados passada como argumento.
    /// </summary>
    public static OperationResult<T> Ok(IEnumerable<T> result)
    {
        return new OperationResult<T> { Success = true, Result = result };
    }

    /// <summary>
    /// Cria e retorna um novo objeto "OperationResult<T>" com "Success" definido como "true", mas sem nenhum resultado definido.
    /// </summary>
    public static OperationResult<T> Ok()
    {
        return new OperationResult<T> { Success = true };
    }
}