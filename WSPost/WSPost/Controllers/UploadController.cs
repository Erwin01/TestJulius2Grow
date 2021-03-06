﻿using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using WSPost.Logs;

namespace WSPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        //private readonly IServiceLogger _serviceLogger;

        #region [ SAVE FILE ]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    //_serviceLogger.SendHiddenMessageErrorQuery();
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                //_serviceLogger.SendHiddenMessageError();
                return StatusCode(500, $"Internal server error: {ex}");
            }
            #endregion

        }
    }
}