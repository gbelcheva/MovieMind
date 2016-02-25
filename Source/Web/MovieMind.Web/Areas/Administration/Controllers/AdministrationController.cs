namespace MovieMind.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using MovieMind.Common;
    using MovieMind.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
