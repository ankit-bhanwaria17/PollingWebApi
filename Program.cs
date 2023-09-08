using PollingWebApi.Data;
using PollingWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/submit", () =>
{
    var job = new Job
    {
        Id = Guid.NewGuid(),
        percentageCompletion = 0
    };
    JobsData.JobsList.Add(job);
    JobsData.StartUpdating(job.Id);
    return Results.Ok($"\n\nJob with Id = {job.Id} started ....");
});

app.MapGet("status/{id:Guid}", (Guid id) => 
{
    Job? job = JobsData.JobsList.FirstOrDefault(j => j.Id == id);
    if (job == null)
    {
        return Results.BadRequest($"No job found with Id = {id}");
    }
    return Results.Ok($"\n\nJob {id} is {job.percentageCompletion}% completed");
});

app.MapGet("all-jobs", () => 
{
    return Results.Ok(JobsData.JobsList);
});

app.Run();

