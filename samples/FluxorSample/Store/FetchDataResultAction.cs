using FluxorSample.Data;

namespace FluxorSample.Store;

public record FetchDataResultAction(IEnumerable<WeatherForecast> Forecasts);
