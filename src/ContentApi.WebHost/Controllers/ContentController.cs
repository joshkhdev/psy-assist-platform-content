﻿using ContentApi.Core.Abstractions.Repositories;
using ContentApi.Core.Domain.Administration;
using ContentApi.WebHost.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace ContentApi.WebHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentController : ControllerBase
{
    private readonly IRepository<Content> _repository;
    private readonly IContentMapping _contentMapping;

    public ContentController(IRepository<Content> repository, IContentMapping contentMapping)
    {
        _repository = repository;
        _contentMapping = contentMapping;
    }

    /// <summary>
    /// Получить информацию по всем файлам
    /// </summary>
    [HttpGet("GetAllFilesInfo")]
    public async Task<IActionResult> GetFilesInfoAsync(CancellationToken cancellationToken)
    {
        var contents = await _repository.GetAllFilesInfoAsync(cancellationToken);

        if (contents == null || !contents.Any())
            return NotFound("Files don't found");

        return Ok(_contentMapping.CreateMap(contents));
    }

    /// <summary>
    /// Получить информацию файла по id
    /// </summary>
    [HttpGet("GetFileInfoById")]
    public async Task<IActionResult> GetFileInfoByIdAsync(string id, CancellationToken cancellationToken)
    {
        var content = await _repository.GetFileInfoByIdAsync(id, cancellationToken);

        if (content.Info == null)
            return NotFound($"File {id} doesn't found");

        return Ok(_contentMapping.CreateMap(content));
    }

    /// <summary>
    /// Получить информацию файлов по фильтру
    /// </summary>
    [HttpPost("GetFilesInfoByFilter")]
    public async Task<IActionResult> GetFilesInfoByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
    {
        var contents = await _repository.GetFilesInfoByFilterAsync(metadata, cancellationToken);

        if (contents == null || !contents.Any())
            return NotFound("Files don't found");

        return Ok(_contentMapping.CreateMap(contents));
    }

    /// <summary>
    /// Скачать файл по id
    /// </summary>
    [HttpGet("DownloadFileById")]
    public async Task<IActionResult> DownloadFileByIdAsync(string id, CancellationToken cancellationToken)
    {
        var content = await _repository.DownLoadFileByIdAsync(id, cancellationToken);

        if (content.Info == null)
            return NotFound($"File {id} doesn't found");

        return Ok(File(content.Bytes, "application/octet-stream", content.Info.Filename));
    }

    /// <summary>
    /// Скачать файлы по фильтру
    /// </summary>
    [HttpPost("DownloadFilesByFilter")]
    public async Task<ActionResult<List<FileContentResult>>> DownLoadFilesByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
    {
        var contents = await _repository.DownLoadFilesByFilterAsync(metadata, cancellationToken);

        if (contents == null || !contents.Any())
            return NotFound("Files don't found");

        return contents.ToList().ConvertAll(x => File(x.Bytes, "application/octet-stream", x.Info.Filename));
    }

    /// <summary>
    /// Удалить файл по id
    /// </summary>
    [HttpDelete("DeleteFileById")]
    public async Task<IActionResult> DeleteFileAsync(string id, CancellationToken cancellationToken)
    {
        await _repository.DeleteFileByIdAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить файлы по фильтру
    /// </summary>
    [HttpDelete("DeleteFilesByFilter")]
    public async Task<IActionResult> DeleteFileByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
    {
        await _repository.DeleteFilesByFilterAsync(metadata, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Загрузить файл
    /// </summary>
    [HttpPost("UploadFile")]
    public async Task<string> UploadFileAsync(IFormFile file, string psyId, int fileType, CancellationToken cancellationToken)
    {
        byte[] bytes;
        using (var item = new MemoryStream())
        {
            file.CopyTo(item);
            bytes = item.ToArray();
        }

        var metadata = new FileMetadata()
        {
            PsyId = psyId,
            Type = (FileType)fileType
        };

        return await _repository.UploadFileAsync(file.FileName, metadata, bytes, cancellationToken);
    }
}