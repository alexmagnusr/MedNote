using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace MedNote.API.Controllers.Sistema
{
    [RoutePrefix("api/Menu")]
    public class MenuController : BaseApiController
    {
        [Authorize]
        public Object Get(int? codigo)
        {
            //VIX.API.Helpers.Initializer.Run();
            return Json(MenuModel.LoadMenu(codigo));
        }

    }
}
