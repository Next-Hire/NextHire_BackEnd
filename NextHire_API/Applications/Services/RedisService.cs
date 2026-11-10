using StackExchange.Redis;

public class RedisService
{
    private readonly IDatabase _database;

    public RedisService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task SalvarCodigo(int usuarioId, string codigo)
    {
        string chave = $"verificacao:{usuarioId}";

        Console.WriteLine($"Salvando chave: {chave}");

        bool resultado = await _database.StringSetAsync(
            chave,
            codigo,
            TimeSpan.FromMinutes(5)
        );

        Console.WriteLine($"Resultado do Redis: {resultado}");

        var valor = await _database.StringGetAsync(chave);

        Console.WriteLine($"Valor salvo: {valor}");
    }

    public async Task<bool> ValidarCodigo(int usuarioId, string codigo)
    {
        string chave = $"verificacao:{usuarioId}";

        Console.WriteLine($"Validando chave: {chave}");

        RedisValue codigoSalvo = await _database.StringGetAsync(chave);

        Console.WriteLine($"Código encontrado: {!codigoSalvo.IsNullOrEmpty}");

        if (codigoSalvo.IsNullOrEmpty)
            return false;

        if (codigoSalvo != codigo)
            return false;

        await _database.KeyDeleteAsync(chave);

        Console.WriteLine("Código validado e removido do Redis.");

        return true;
    }
}