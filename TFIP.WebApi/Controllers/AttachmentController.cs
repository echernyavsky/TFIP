﻿using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using TFIP.Business.Contracts;
using TFIP.Business.Models;
using TFIP.Common.Helpers;
using TFIP.Web.Api.Models;

namespace TFIP.Web.Api.Controllers
{
    public class AttachmentController : BaseApiController
    {
        private readonly IFileManagementService fileManagementService;

        public AttachmentController(IFileManagementService fileManagementService)
        {
            this.fileManagementService = fileManagementService;
        }

        [HttpPost]
        public HttpResponseMessage SaveAttachment(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var result = ProcessViewModel<FileViewModel, ListItem>(new FileViewModel()
                {
                    InputStream = file.InputStream,
                    FileName = file.FileName
                }, null, fileManagementService.SaveAttachmentToTempFolder);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
        }

        public HttpResponseMessage Download(ListItem file)
        {
            //if (Request.IsAjaxRequest())
            //{
            //    if (fileManagementService.FileExists(file))
            //    {
            //        return Json(new AjaxViewModel()
            //        {
            //            Data = Url.Action("Download", "Attachment", new
            //            {
            //                file.Id,
            //                file.Value,
            //                AdditionalInfo = file.AdditionalInfo
            //            })
            //        });
            //    }

            //    return Json(new AjaxViewModel()
            //    {
            //        Data = false,
            //        Message = "The file cannot be downloaded. Contact administrator for any questions."
            //    });
            //}

            //var file = fileManagementService.GetFileBytes(file);
            //return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, file.Value);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public HttpResponseMessage DeleteAttachment(ListItem file)
        {
            return Request.CreateResponse(HttpStatusCode.OK, ProcessViewModel(file, null, fileManagementService.RemoveTempAttachment));
        }
    }
}