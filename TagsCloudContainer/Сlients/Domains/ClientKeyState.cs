using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Domains;

public abstract class ClientKeyState
{
    public static readonly ClientKeyState Unset = new UnsetState();

    public abstract Result<ClientKeyState> Set(string key);
    public abstract Result<string> Get();

    private sealed class UnsetState : ClientKeyState
    {
        public override Result<ClientKeyState> Set(string key) =>
            Result<ClientKeyState>.Success(new SetState(key));

        public override Result<string> Get() =>
            Result<string>.Failure(
                $"Не задан клиент. Используй {ClientSelectionParser.ClientFlag} <id> или {ClientSelectionParser.ClientFlag}=<id>."
            );
    }

    private sealed class SetState(string key) : ClientKeyState
    {
        public override Result<ClientKeyState> Set(string _) =>
            Result<ClientKeyState>.Failure("Флаг указан дважды");

        public override Result<string> Get() =>
            Result<string>.Success(key);
    }
}