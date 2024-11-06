using Fluxor;
using FluxorSample.Data;
using IDispatcher = Fluxor.IDispatcher;

namespace FluxorSample.Store.WeatherUseCase;

public class Effects(HttpClient http)
{
    [EffectMethod]
    public async Task HandleFetchDataAction(FetchDataAction action, IDispatcher dispatcher)
    {
        // simulate http request
        await Task.Delay(1500);
        var forecasts = new WeatherForecast[]
        {
            new() { Date = DateTime.Today, Summary = "Warm", TemperatureC = 24  },
            new() { Date = DateTime.Today.AddDays(1), Summary = "Cold", TemperatureC = 2  }
        };

        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    }
}
