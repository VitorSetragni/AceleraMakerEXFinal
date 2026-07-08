namespace InterfaceTerminal.TratamentoErros;

public class TratamentoErrosApi<T>
{
    public bool Sucesso { get; set; }
    public bool NaoEncontrado { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public T? Dados { get; set; }

    public static TratamentoErrosApi<T> Ok(T dados)
    {
        return new TratamentoErrosApi<T>
        {
            Sucesso = true,
            Dados = dados
        };
    }

    public static TratamentoErrosApi<T> Falha(string mensagem, bool naoEncontrado = false)
    {
        return new TratamentoErrosApi<T>
        {
            Sucesso = false,
            NaoEncontrado = naoEncontrado,
            Mensagem = mensagem
        };
    }
}