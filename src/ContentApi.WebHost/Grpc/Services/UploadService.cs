using ContentApi.Core.Abstractions.Repositories;
using ContentApi.Core.Domain.Administration;
using ContentApi.WebHost.Grpc.Protos;
using Grpc.Core;

namespace ContentApi.WebHost.Grpc.Services
{
    public class UploadService : Uploader.UploaderBase
    {
        private readonly IRepository<Content> _repository;

        public UploadService(IRepository<Content> repository)
        {
            _repository = repository;
        }

        public override async Task<Id> UploadFile(UploadRequest request, ServerCallContext context)
        {
            byte[] bytes = Convert.FromBase64String(request.Base64);

            var metadata = new FileMetadata()
            {
                PsyId = request.PsyId,
                Type = (FileType)request.FileType
            };

            var id = await _repository.UploadFileAsync(request.FileName, metadata, bytes, context.CancellationToken);

            return new Id() { Value = id };
        }
    }
}
