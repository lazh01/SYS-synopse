using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Application.Interfaces;
using DataProcessing.Processing.Domain.Entities;

namespace DataProcessing.Processing.Application.UseCases;

public class GetAggregatesUseCase
{
    private readonly IMeasurementRepository _repository;

    public GetAggregatesUseCase(IMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SensorAggregate>> ExecuteAsync()
    {
        return await _repository.GetAggregatesAsync();
    }
}
