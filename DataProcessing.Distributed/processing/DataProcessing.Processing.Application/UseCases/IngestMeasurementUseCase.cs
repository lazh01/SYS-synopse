using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Application.Interfaces;
using DataProcessing.Processing.Domain.Entities;

namespace DataProcessing.Processing.Application.UseCases;

public class IngestMeasurementUseCase
{
    private readonly IMeasurementRepository _repository;

    public IngestMeasurementUseCase(IMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Measurement measurement)
    {
        await _repository.AddMeasurementAsync(measurement);
    }
}