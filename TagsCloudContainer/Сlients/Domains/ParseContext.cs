using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Domains;

public class ParseContext(int capacity)
{
    private ClientKeyState state = ClientKeyState.Unset;
    private readonly List<string> rest = new(capacity);

    public void AddRest(string arg) => rest.Add(arg);

    public Result<Unit> SetClientKey(string raw)
    {
        var keyR = ClientKey.Parse(raw);
        if (!keyR.IsSuccess)
            return Result<Unit>.Failure(keyR.Error);
        var key = keyR.Value!;
        
        var stateR = state.Set(key);
        if (!stateR.IsSuccess)
            return Result<Unit>.Failure(stateR.Error);
        state = stateR.Value!;
        
        return Result<Unit>.Success(Unit.Value);
    }

    public Result<ClientSelection> Build()
    {
        var stateR = state.Get();
        return !stateR.IsSuccess ? Result<ClientSelection>.Failure(stateR.Error) :
            Result<ClientSelection>.Success(new ClientSelection(stateR.Value, rest.ToArray()));
    }
}

