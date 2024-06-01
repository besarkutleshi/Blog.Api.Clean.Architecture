using Blog.Application.Helpers;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.BackgroundJobs;
using Blog.Infrastructure.Files;
using Blog.Infrastructure.Imports;
using Blog.SharedResources;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Features.Posts.Commands;

public class ImportPostsFromExcel
{ 
    public record ImportPostsFromExcelCommand(IFormFile File) : IRequest<Result>;

    public class ImportPostsFromExcelCommandHandler(IPostRepository _postRepository, IImportPostsFromExcel _importPostsFromExcel,
        IFileSaver _fileSaver, IEnqueueJob _enqueueJob, IRequestService _requestService) 
        : IRequestHandler<ImportPostsFromExcelCommand, Result>
    {
        public Task<Result> Handle(ImportPostsFromExcelCommand request, CancellationToken cancellationToken)
        {
            var filePath = _fileSaver.SaveFile(request.File);
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(Result.Failure(Error.Failure("Upload.Error", ["There is no file path to save file."])));

            var jobId = _enqueueJob.Enqueue(() => ImportPosts(filePath, 0, cancellationToken));
            if(string.IsNullOrEmpty(jobId))
                return Task.FromResult(Result.Failure(Error.Failure("Enqueue.Error", ["Failed to enqueue job for importing posts."])));

            return Task.FromResult(Result.Success(Success.Accept($"Job with id: {jobId} enqueued successfully")));
        }

        public async Task ImportPosts(string filePath, int workSheet, CancellationToken cancellationToken)
        {
            var result = _importPostsFromExcel.ImportDataFromExcel(filePath, workSheet, _requestService.UserId);
            if (result.IsSuccess)
            {
                var posts = (List<Post>)result.Response.Result!;
                await _postRepository.ImportPosts(posts, cancellationToken);
            }
        }
    }
}
