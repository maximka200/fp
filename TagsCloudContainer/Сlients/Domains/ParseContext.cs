using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Domains;

public class ParseContext(int capacity)
{
    private ClientKeyState state = ClientKeyState.Unset;
    private readonly List<string> rest = new(capacity);

    public void AddRest(string arg) => rest.Add(arg);

    public Result<Unit> SetClientKey(string raw)
    {
        return ClientKey.Parse(raw)   
            .Bind(key =>
                state.Set(key)    
                    .Map(newState =>
                    {
                        state = newState;
                        return Unit.Value;
                    })
            );
    }

    public Result<ClientSelection> Build()
    {
        var stateR = state.Get();
        return !stateR.IsSuccess ? Result<ClientSelection>.Failure(stateR.Error) :
            Result<ClientSelection>.Success(new ClientSelection(stateR.Value, rest.ToArray()));
    }
}

